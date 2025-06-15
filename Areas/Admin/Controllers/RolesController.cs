using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffManagement.contexts;
using StaffManagement.Models;
using StaffManagement.ViewModels;

namespace StaffManagement.Areas.Admin.Controllers
{
    public class RolesController : AdminBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public RolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            IdentityRole roles = new IdentityRole();
            roles.Name = model.RoleName;
            roles.NormalizedName = model.RoleName.ToUpper();
            var result = await _roleManager.CreateAsync(roles);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = new RoleViewModel();
            var result = await _roleManager.FindByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            role.Id = result.Id;
            role.RoleName = result.Name;

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var checkIfResult = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!checkIfResult)
            {
                var result = await _roleManager.FindByIdAsync(id);
                result.Name = model.RoleName;
                result.NormalizedName = model.RoleName.ToUpper();
                var finalResult = await _roleManager.UpdateAsync(result);
                if (finalResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in finalResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View(model);
        }
    }
}
