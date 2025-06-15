using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using StaffManagement.contexts;
using StaffManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StaffManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class AttendanceController : AdminBaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly QRCodeGenerator _qrGenerator = new QRCodeGenerator();

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Generates a QR code for the specified Shift and renders it on screen.
        ///  URL encoded inside the QR points to /Attendances/Scan?token={Guid}
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GenerateQRCode(int shiftId, int ttlMinutes = 30)
        {
            var shift = await _context.Shifts.FindAsync(shiftId);
            if (shift == null)
                return NotFound();

            // 1. Check the present time is in the shift
            var now = DateTime.UtcNow.AddHours(7);

            var shiftStart = DateTime.UtcNow.AddHours(7).Date.Add(shift.StartTime.ToTimeSpan());
            var shiftEnd = DateTime.UtcNow.AddHours(7).Date.Add(shift.EndTime.ToTimeSpan());

            if (now < shiftStart || now > shiftEnd)
            {
                // Nếu thời gian hiện tại không nằm trong khoảng thời gian ca làm, trả lại lỗi
                TempData["ErrorMessage"] = "Không thể tạo mã QR ngoài giờ làm việc!";
                return View("QRDisplay", "");  // Trở về view QRDisplay
            }
            // 2. Persist token in database
            var token = new AttendanceToken
            {
                Id = Guid.NewGuid(),
                ShiftId = shiftId,
                GeneratedAt = DateTime.UtcNow.AddHours(7),
                ExpiresAt = DateTime.UtcNow.AddHours(7).AddMinutes(ttlMinutes)
            };

            _context.AttendanceTokens.Add(token);
            await _context.SaveChangesAsync();

            // 3. Build absolute callback URL for staff‑side Scan action
            var callbackUrl = Url.Action(

                action: "Scan",
                controller: "Attendances",
                values: new { area = "", token = token.Id },
                protocol: Request.Scheme,
                host: Request.Host.Value)!; // e.g., https://hrm.local/attendances/scan?token=guid

            // 4. Generate PNG QR image (byte[])
            using var qrData = _qrGenerator.CreateQrCode(callbackUrl, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            var qrBytes = qrCode.GetGraphic(pixelsPerModule: 10);

            // 5. Return as Base64 so Razor view can embed directly
            var base64 = Convert.ToBase64String(qrBytes);
            return View("QrDisplay", base64);
        }
    }
}

