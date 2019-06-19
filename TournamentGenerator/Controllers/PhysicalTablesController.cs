using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TournamentGenerator.Data;
using TournamentGenerator.Models;

namespace TournamentGenerator.Controllers
{
    public class PhysicalTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhysicalTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PhysicalTables
        public async Task<IActionResult> Index()
        {
            return View(await _context.PhysicalTables.ToListAsync());
        }

        // GET: PhysicalTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalTable = await _context.PhysicalTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physicalTable == null)
            {
                return NotFound();
            }

            return View(physicalTable);
        }

        // GET: PhysicalTables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhysicalTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] PhysicalTable physicalTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(physicalTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(physicalTable);
        }

        // GET: PhysicalTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalTable = await _context.PhysicalTables.FindAsync(id);
            if (physicalTable == null)
            {
                return NotFound();
            }
            return View(physicalTable);
        }

        // POST: PhysicalTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] PhysicalTable physicalTable)
        {
            if (id != physicalTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(physicalTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhysicalTableExists(physicalTable.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(physicalTable);
        }

        // GET: PhysicalTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalTable = await _context.PhysicalTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physicalTable == null)
            {
                return NotFound();
            }

            return View(physicalTable);
        }

        // POST: PhysicalTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var physicalTable = await _context.PhysicalTables.FindAsync(id);
            _context.PhysicalTables.Remove(physicalTable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhysicalTableExists(int id)
        {
            return _context.PhysicalTables.Any(e => e.Id == id);
        }
    }
}
