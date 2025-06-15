using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using StaffManagement.Models;
using StaffManagement.contexts;
using Microsoft.EntityFrameworkCore;

namespace StaffManagement.ViewComponents
{
    public class AdminSidebarMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AdminSidebarMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var shifts = await _context.Shifts.ToListAsync();

            ViewData["ShiftMenuList"] = shifts;
            return View();
        }
    }
}
