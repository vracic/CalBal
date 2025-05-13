using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalBal.Models;

namespace CalBal.Controllers
{
    public class CiljsController : Controller
    {
        private readonly CalbalContext _context;

        public CiljsController(CalbalContext context)
        {
            _context = context;
        }

        // GET: Ciljs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ciljs.ToListAsync());
        }

        // GET: Ciljs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cilj = await _context.Ciljs
                .FirstOrDefaultAsync(m => m.CiljId == id);
            if (cilj == null)
            {
                return NotFound();
            }

            return View(cilj);
        }

        // GET: Ciljs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ciljs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CiljId,Opis,PocTezina,CiljTezina,DatumPostavljen,DatumZavrsen")] Cilj cilj)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cilj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cilj);
        }

        // GET: Ciljs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cilj = await _context.Ciljs.FindAsync(id);
            if (cilj == null)
            {
                return NotFound();
            }
            return View(cilj);
        }

        // POST: Ciljs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CiljId,Opis,PocTezina,CiljTezina,DatumPostavljen,DatumZavrsen")] Cilj cilj)
        {
            if (id != cilj.CiljId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cilj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CiljExists(cilj.CiljId))
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
            return View(cilj);
        }

        // GET: Ciljs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cilj = await _context.Ciljs
                .FirstOrDefaultAsync(m => m.CiljId == id);
            if (cilj == null)
            {
                return NotFound();
            }

            return View(cilj);
        }

        // POST: Ciljs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cilj = await _context.Ciljs.FindAsync(id);
            if (cilj != null)
            {
                _context.Ciljs.Remove(cilj);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CiljExists(int id)
        {
            return _context.Ciljs.Any(e => e.CiljId == id);
        }
    }
}
