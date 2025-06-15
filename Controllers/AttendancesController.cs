using QRCoder;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Identity;
using StaffManagement.Models;
using Microsoft.EntityFrameworkCore;
using StaffManagement.contexts;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
 
namespace StaffManagement.Cotrollers
{
    [Authorize(Roles = "Admin,Manager,Staff")]
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const decimal PenaltyPer5Min = 10000m;

        public AttendancesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Attendance/Scan
        [HttpGet]
        public async Task<IActionResult> Scan(Guid token)
        {


            // 1. Validate token existence & expiry
            var attendanceToken = await _context.AttendanceTokens
                                                .Include(t => t.Shift)
                                                .FirstOrDefaultAsync(t => t.Id == token);
            if (attendanceToken == null || !attendanceToken.IsActive)
            {
                return BadRequest("Mã QR không hợp lệ hoặc đã hết hạn.");
            }

            // 2. Resolve current employee via Identity login
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.EmployeeId == null)
                return Forbid();

            var employeeId = currentUser.EmployeeId.Value;
            var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));
            var nowTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(7));

            // 3. Fetch or create attendance record for today & shift
            var attendance = await _context.Attendances
                                           .FirstOrDefaultAsync(a => a.EmployeeId == employeeId &&
                                                                     a.ShiftId == attendanceToken.ShiftId &&
                                                                     a.Date == today);
            if (attendance == null)
            {
                // First scan => CHECK‑IN
                attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    ShiftId = attendanceToken.ShiftId,
                    Date = today,
                    CheckInTime = nowTime
                };

                // Calculate lateness
                var diffMinutes = (int)(nowTime.ToTimeSpan() - attendanceToken.Shift.StartTime.ToTimeSpan() - attendance.Shift.CutoffTime.ToTimeSpan()).TotalMinutes;
                attendance.MinutesLate = diffMinutes > 0 ? diffMinutes : 0;

                _context.Attendances.Add(attendance);
            }
            else if (!attendance.CheckOutTime.HasValue)
            {
                // Second scan => CHECK‑OUT
                attendance.CheckOutTime = nowTime;
            }
            else
            {
                // Already checked out
                return BadRequest("Đã hoàn tất điểm danh cho ca này.");
            }

            await _context.SaveChangesAsync();
            return View("ScanResult", attendance);
        }
    }

}

