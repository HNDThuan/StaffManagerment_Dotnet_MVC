using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.contexts;
using StaffManagement.Dtos.Employee;
using StaffManagement.Models;

namespace StaffManagement.Areas.Admin.Controllers
{

    public class EmployeesController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Employees.Include(e=> e.Department).Include(e=> e.Position ).ToListAsync();
            return View(applicationDbContext);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            await SetSelectList();
            return View(new CreateEmployeeDto());
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                if (await PhoneExist(dto.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "This phone number already exists");
                }

                if (await EmailExist(dto.Email) || await _context.Users.AnyAsync(e => e.Email != null && e.Email == dto.Email))
                {
                    ModelState.AddModelError("Email", "This email already exists");
                }

                if (!await DepartmentExist(dto.DepartmentId))
                {
                    ModelState.AddModelError("DepartmentId", "This Department does NOT exist");
                }

                if (!await PositionExist(dto.PositionId))
                {
                    ModelState.AddModelError("PositionId", "This Position does NOT exist");
                }
            }

            if (!ModelState.IsValid)
            {
                await SetSelectList();
                return View(dto);
            }

            var employee = new Employee()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Nationality = dto.Nationality,
                Address = dto.Address,
                DateOfBirth = dto.DateOfBirth,
                DepartmentId = dto.DepartmentId,
                PositionId = dto.PositionId,
                EmployeeCode = await GenerateUniqueEmployeeCode(),
                CreatedById = "code",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await SetSelectList();
            return View(new EditEmployeeDto
            {
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Nationality = employee.Nationality,
                Address = employee.Address,
                PositionId = employee.PositionId,
                DateOfBirth = employee.DateOfBirth,
                DepartmentId = employee.DepartmentId,
                FirstName = employee.FirstName
            });
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditEmployeeDto dto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (dto.PhoneNumber != employee.PhoneNumber && await PhoneExist(dto.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "This phone number already exists");
                }

                if (dto.Email != employee.Email && await EmailExist(dto.Email))
                {
                    ModelState.AddModelError("Email", "This email already exists");
                }

                if (!await DepartmentExist(dto.DepartmentId))
                {
                    ModelState.AddModelError("DepartmentId", "This Department does NOT exist");
                }

                if (!await PositionExist(dto.PositionId))
                {
                    ModelState.AddModelError("PositionId", "This Position does NOT exist");
                }
            }

            if (!ModelState.IsValid)
            {
                await SetSelectList();
                return View(dto);
            }

            bool isChanged = false;

            if (dto.FirstName != employee.FirstName)
            {
                employee.FirstName = dto.FirstName;
                isChanged = true;
            }

            if (dto.MiddleName != null && dto.MiddleName != employee.MiddleName)
            {
                employee.MiddleName = dto.MiddleName;
                isChanged = true;
            }

            if (dto.LastName != employee.LastName)
            {
                employee.LastName = dto.LastName;
                isChanged = true;
            }

            if (dto.PhoneNumber != employee.PhoneNumber)
            {
                employee.PhoneNumber = dto.PhoneNumber;
                isChanged = true;
            }

            if (dto.Email != employee.Email)
            {
                employee.Email = dto.Email;
                isChanged = true;
            }

            if (dto.Nationality != employee.Nationality)
            {
                employee.Nationality = dto.Nationality;
                isChanged = true;
            }

            if (dto.Address != employee.Address)
            {
                employee.Address = dto.Address;
                isChanged = true;
            }

            if (dto.DateOfBirth != employee.DateOfBirth)
            {
                employee.DateOfBirth = dto.DateOfBirth;
                isChanged = true;
            }

            if (dto.DepartmentId != employee.DepartmentId)
            {
                employee.DepartmentId = dto.DepartmentId;
                isChanged = true;
            }

            if (dto.PositionId != employee.PositionId)
            {
                employee.PositionId = dto.PositionId;
                isChanged = true;
            }

            if (isChanged)
            {
                employee.ModifiedOn = DateTime.Now;
            }

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EmployeeExists(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> DepartmentExist(int id)
        {
            return await _context.Departments.AnyAsync(d => d.Id == id);
        }

        private async Task<bool> PositionExist(int id)
        {
            return await _context.Positions.AnyAsync(p => p.Id == id);
        }

        private async Task<bool> EmailExist(string email)
        {
            return await _context.Employees.AnyAsync(e => e.Email == email);
        }

        private async Task<bool> PhoneExist(string phoneNumber)
        {
            return await _context.Employees.AnyAsync(e => e.PhoneNumber == phoneNumber);
        }

        private async Task<bool> EmployeeCodeExists(string employeeCode)
        {
            return await _context.Employees.AnyAsync(e => e.EmployeeCode == employeeCode);
        }

        private async Task<string> GenerateUniqueEmployeeCode()
        {
            string employeeCode;
            do
            {
                employeeCode = GenerateRandomCode(20);
            }
            while (await EmployeeCodeExists(employeeCode));

            return employeeCode;
        }

        private string GenerateRandomCode(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Range(0, length)
                                         .Select(_ => chars[random.Next(chars.Length)])
                                         .ToArray());
        }

        private async Task SetSelectList()
        {
            var departmentSelectList = await _context.Departments.Select(d => new SelectListItem
            {
                Text = $"{d.Name} - {d.Location}",
                Value = d.Id.ToString()
            }).ToListAsync();

            departmentSelectList.Insert(0, new SelectListItem { Value = "", Text = "-- Select Department --", Selected = true, Disabled = true });

            var positionSelectList = await _context.Positions.Select(d => new SelectListItem
            {
                Text = $"{d.Name}",
                Value = d.Id.ToString()
            }).ToListAsync();

            positionSelectList.Insert(0, new SelectListItem { Value = "", Text = "-- Select Positions --", Selected = true, Disabled = true });

            ViewData["Departments"] = departmentSelectList;
            ViewData["Positions"] = positionSelectList;
        }
    }
}
