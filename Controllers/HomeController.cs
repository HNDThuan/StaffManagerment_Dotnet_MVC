using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.contexts;
using StaffManagement.Models;
using StaffManagement.ViewModels;
using System.Diagnostics;

namespace StaffManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return !User.Identity.IsAuthenticated ? this.Redirect("~/identity/account/login") : View();
        }

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel
            {
                Id = user.Id,
              
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            
                Address = "",
                UserName = user.UserName,
            };

            if (user.Employee != null)
            {
                model.FirstName = user.Employee.FirstName;
                model.MiddleName = user.Employee.MiddleName;
                model.LastName = user.Employee.LastName;
                model.NationalId = user.Employee.Nationality;
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _context.Users.Include(e => e.Employee).FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel
            {
                Id = user.Id,
              
            
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,

            };

            if (user.Employee != null)
            {
                model.FirstName = user.Employee.FirstName ?? "";
                model.MiddleName = user.Employee.MiddleName ?? "";
                model.LastName = user.Employee.LastName ?? "";
                model.NationalId = user.Employee.Nationality;
            }

            await SetSelectLists(false);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists(false);
                return View(model);
            }

            var user = await _context.Users.FindAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(user.EmployeeId);
            if (user == null)
            {
                return NotFound();
            }
            employee.PhoneNumber = user.PhoneNumber = model.PhoneNumber;
            employee.Email = model.Email;
            employee.Address =   model.Address;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.ModifiedOn = DateTime.Now;
            user.ModifiedById = user.Id;
            
            _context.Update(employee);
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> LeaveApplication()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == user.Email);

            List<LeaveApplication> leaveApplications = new();

            if (employee != null)
            {
                leaveApplications = await _context.LeaveApplications
                    .Include(l => l.Duration)
                    .Include(l => l.Employee)
                    .Include(l => l.LeaveType)
                    .Include(l => l.Status)
                    .Where(l => l.EmployeeId == employee.Id)
                    .ToListAsync();
            }

            return View(leaveApplications);
        }

        [HttpPost]
        public async Task<IActionResult> RejectLeave(LeaveApplication leave)
        {
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status).FirstOrDefaultAsync(m => m.Id == leave.Id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            leaveApplication.ApprovalOn = DateTime.Now;
            leaveApplication.StatusId = 8;
            leaveApplication.ApprovalNotes = leave.ApprovalNotes;
            _context.Update(leaveApplication);
            await _context.SaveChangesAsync();
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ApproveLeave(LeaveApplication leave)
        {

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status).FirstOrDefaultAsync(m => m.Id == leave.Id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            leaveApplication.ApprovalOn = DateTime.Now;
            leaveApplication.StatusId = 7;
            leaveApplication.ApprovalNotes = leave.ApprovalNotes;
            _context.Update(leaveApplication);
            await _context.SaveChangesAsync();
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DetailsLeaveApplication(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        public IActionResult CreateLeaveApplication()
        {
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLeaveApplication(LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                leaveApplication.CreatedOn = DateTime.Now;
                leaveApplication.StatusId = 6;
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        public async Task<IActionResult> EditLeaveApplication(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLeaveApplication(int id, LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id || leaveApplication.StatusId == 7 || leaveApplication.StatusId == 8)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    leaveApplication.ModifiedOn = DateTime.Now;
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", "Home");
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails, "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        private bool LeaveApplicationExists(int id)
        {
            throw new NotImplementedException();
        }

   

        private async Task SetSelectLists(bool isEmployeeContainUser)
        {
            var employeeSelect = await _context.Employees.Where(e => isEmployeeContainUser ? e.User != null : e.User == null).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.FullName} - {e.EmployeeCode}"
            }).ToListAsync();

            var roles = new SelectList(_context.Roles, "Name", "Name");

            ViewData["EmployeeIds"] = employeeSelect;
            ViewData["RoleNames"] = roles;
        }
    }
}

