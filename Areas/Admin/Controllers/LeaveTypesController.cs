using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Models;
using StaffManagement.Dtos.LeaveType;
using StaffManagement.contexts;

namespace StaffManagement.Areas.Admin.Controllers
{

    public class LeaveTypesController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public LeaveTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {

            return View(await _context.LeaveTypes.ToListAsync());
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View(new CreateLeaveTypeDto());
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLeaveTypeDto dto)
        {
            if (ModelState.IsValid)
            {
                if (await CodeExist(dto.Code))
                {
                    ModelState.AddModelError("Code", "This code already exist");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var leaveType = new LeaveType
            {
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Code = dto.Code,
                Name = dto.Name,
            };

            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(new EditLeaveTypeDto { Id = (int)id, Code = leaveType.Code, Name = leaveType.Name });
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditLeaveTypeDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(e => e.Id == id);
            if (leaveType == null) {
                ModelState.AddModelError("","Not found");
            }

            if (ModelState.IsValid)
            {
                if (leaveType.Code != dto.Code && await CodeExist(dto.Code))
                {
                    ModelState.AddModelError("Code", "This code already exists");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            leaveType.Name = dto.Name;
            leaveType.Code = dto.Code;
            leaveType.ModifiedOn = DateTime.Now;

            _context.LeaveTypes.Update(leaveType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }

        private async Task<bool> CodeExist(string code)
        {
            return await _context.LeaveTypes.AnyAsync(lt => lt.Code == code);
        }
    }
}
