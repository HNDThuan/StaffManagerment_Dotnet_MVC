using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Models;
using StaffManagement.Dtos;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using StaffManagement.Dtos.Position;
using StaffManagement.contexts;


namespace StaffManagement.Areas.Admin.Controllers
{
    public class PositionsController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Positions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Positions.ToListAsync());
        }

        // GET: Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.Include(p=> p.Employees).ThenInclude(e=>e.Department).FirstOrDefaultAsync(p => p.Id == id);


            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Positions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePositionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);

            }
            var position = new Models.Position
            {
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Name = dto.Name,
                Description = dto.Description,
                PositionCode = await GenerateUniquePositionCode()
            };

            _context.Add(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(new EditPositionDto
            {
                PositionCode = position.PositionCode,
                Description = position.Description,
                Name = position.Name,
                Id = position.Id
            });
        }

        // Fix for CS0161: Ensure all code paths return a value in the Edit method.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPositionDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (position.PositionCode != dto.PositionCode && await PositionCodeExists(dto.PositionCode))
                {
                    ModelState.AddModelError("PositionCode", "This code already exists");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            bool isChanged = false;
            if (position.Name != dto.Name)
            {
                position.Name = dto.Name;
                isChanged = true;
            }

            if (position.Description != null && dto.Description != null && position.Name != dto.Name)
            {
                position.Description = dto.Description;
                isChanged = true;
            }

            if (position.PositionCode != dto.PositionCode)
            {
                position.PositionCode = dto.PositionCode;
                isChanged = true;
            }

            if (isChanged)
            {
                position.ModifiedOn = DateTime.Now;
            }

            _context.Update(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Positions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.Include(x => x.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }
            else if (position.Employees.Count > 0)
            {
                ModelState.AddModelError("", "Can NOT delete this Position, since there are employees related");
            }

            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position != null)
            {
                if (position.Employees.Count > 0)
                {
                    ModelState.AddModelError("", "Can NOT delete this Position, since there are employees related");
                    return View(position);
                }

                _context.Positions.Remove(position);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.Id == id);
        }

        private async Task<bool> PositionCodeExists(string positionCode)
        {
            return await _context.Positions.AnyAsync(p => p.PositionCode == positionCode);
        }

        private async Task<string> GenerateUniquePositionCode()
        {
            string positionCode;
            do
            {
                positionCode = GenerateRandomCode(3);
            }
            while (await PositionCodeExists(positionCode));

            return positionCode;
        }

        private string GenerateRandomCode(int length)
        {
            var random = new Random();
            const string chars = "0123456789";
            return "P" + new string(Enumerable.Range(0, length)
                                         .Select(_ => chars[random.Next(chars.Length)])
                                         .ToArray());
        }
    }
}
