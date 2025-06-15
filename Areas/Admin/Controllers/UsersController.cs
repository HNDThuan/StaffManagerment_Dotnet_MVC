using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.contexts;
using StaffManagement.Dtos.User;
using StaffManagement.Models;
using StaffManagement.ViewModels;
using System.Data;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StaffManagement.Areas.Admin.Controllers
{

    public class UsersController : AdminBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(x => x.Roles).Include(e => e.Employee).ToListAsync();
            return View(users);
        }
        // GET: Admin/Users/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.Employee.FirstName,
                MiddleName = user.Employee.MiddleName,
                LastName = user.Employee.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                NationalId = user.Employee.Nationality,
                Address = user.Employee.Address,
                UserName = user.UserName,
                Roles = user.Roles
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await SetSelectLists(false);
            return View(new CreateUserDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto model)
        {


            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == model.EmployeeId);
          
            if (employee == null)
            {
                ModelState.AddModelError("RoleId", "Employee does NOT exists");
            }
            if (await _context.Users.AnyAsync(e=> e.EmployeeId != null && e.EmployeeId == model.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Employee already has an account");
            }
            if (!ModelState.IsValid)
            {
                await SetSelectLists(false);
                return View(model);
            }
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName ?? employee.Email,
                NormalizedEmail = employee.Email.ToUpper(),
                NormalizedUserName = (model.UserName ?? employee.Email).ToUpper(),
                Email = employee.Email,
                EmployeeId = model.EmployeeId,
                CreatedOn = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user, model.RoleNames);
                if (result.Succeeded)
                    return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            await SetSelectLists(false);
            return View(model);
        }
        // GET: Admin/Users/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _context.Users.Include(e => e.Roles).FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateUserDto
            {
                EmployeeId = user.EmployeeId,
                RoleNames = user.Roles.Select(e => e.Name).ToList(),
                UserName = user.UserName,
            };

            await SetSelectLists(false);
            ViewData["Id"] = id;
            return View(model);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateUserDto model)
        {
            var user = await _context.Users.Include(e => e.Roles).FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var hasEmployee = await _context.Employees.AnyAsync(e => e.Id == model.EmployeeId);
            if (!hasEmployee)
            {
                ModelState.AddModelError("EmployeeId", "Employee does NOT exists");
            }

            if (!ModelState.IsValid)
            {
                await SetSelectLists(false);
                return View(model);
            }


            user.UserName = model.UserName;
            user.EmployeeId = model.EmployeeId;
            user.ModifiedOn = DateTime.Now;

            _context.Update(user);
            await _context.SaveChangesAsync();

            var result = await _userManager.RemoveFromRolesAsync(user, model.RoleNames);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user, model.RoleNames);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ModelState.AddModelError("", "Edit user unsuccessfully");
            await SetSelectLists(false);
            return View(model);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
