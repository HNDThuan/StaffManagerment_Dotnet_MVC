using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.contexts;
using StaffManagement.Dtos.Department;
using StaffManagement.Models;

namespace StaffManagement.Areas.Admin.Controllers
{

    public class DepartmentsController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Departments;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.Include(d=>d.Employees).ThenInclude(e=> e.Position).FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View(new CreateDepartmentDto());
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var department = new Department
            {
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                DepartmentCode = await GenerateUniqueDepartmenCode()
            };

            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var dto = new EditDepartmentDto
            {
                DepartmentCode = department.DepartmentCode,
                Location = department.Location,
                Description = department.Description,
                Id = department.Id,
                Name = department.Name
            };
            return View(dto);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDepartmentDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == dto.Id);
            if (department == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (department.DepartmentCode != dto.DepartmentCode && await DepartmenCodeExists(dto.DepartmentCode))
                {
                    ModelState.AddModelError("DepartmentCode", "Department code already exists");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            bool isChanged = false;

            if (department.Name != dto.Name)
            {
                department.Name = dto.Name;
                isChanged = true;
            }

            if (department.DepartmentCode != dto.DepartmentCode)
            {
                department.DepartmentCode = dto.DepartmentCode;
                isChanged = true;
            }

            if (dto.Description != null && department.Description != null && department.Description != dto.Name)
            {
                department.Name = dto.Name;
                isChanged = true;
            }

            if (department.Location != dto.Location)
            {
                department.Location = dto.Location;
                isChanged = true;
            }

            if (isChanged)
            {
                department.ModifiedOn = DateTime.Now;
            }

            _context.Update(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.Include(d => d.Employees).FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            else if (department.Employees.Count > 0)
            {
                ModelState.AddModelError("", "Can not DELETE, since Department relate to other Employees");
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                if (department.Employees.Count > 0)
                {
                    ModelState.AddModelError("", "Can not DELETE, since Department relate to other Employees");
                    return View(department);
                }

                _context.Departments.Remove(department);
            }
            

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }


        private async Task<bool> DepartmenCodeExists(string departmentCode)
        {
            return await _context.Departments.AnyAsync(d => d.DepartmentCode == departmentCode);
        }

        private async Task<string> GenerateUniqueDepartmenCode()
        {
            string departmentCode;
            do
            {
                departmentCode = GenerateRandomCode(3);
            }
            while (await DepartmenCodeExists(departmentCode));

            return departmentCode;
        }

        private string GenerateRandomCode(int length)
        {
            var random = new Random();
            const string chars = "0123456789";
            return "D" + new string(Enumerable.Range(0, length)
                                         .Select(_ => chars[random.Next(chars.Length)])
                                         .ToArray());
        }
    }
}
