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
    public class AktivnostsController : Controller
    {
        private readonly CalbalContext _context;

        public AktivnostsController(CalbalContext context)
        {
            _context = context;
        }

        // GET: Aktivnosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aktivnosts.ToListAsync());
        }

        // GET: Aktivnosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktivnost = await _context.Aktivnosts
                .FirstOrDefaultAsync(m => m.AktivnostId == id);
            if (aktivnost == null)
            {
                return NotFound();
            }

            return View(aktivnost);
        }

        // GET: Aktivnosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aktivnosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AktivnostId,Naziv,Potrosnja")] Aktivnost aktivnost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aktivnost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aktivnost);
        }

        // GET: Aktivnosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktivnost = await _context.Aktivnosts.FindAsync(id);
            if (aktivnost == null)
            {
                return NotFound();
            }
            return View(aktivnost);
        }

        // POST: Aktivnosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AktivnostId,Naziv,Potrosnja")] Aktivnost aktivnost)
        {
            if (id != aktivnost.AktivnostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aktivnost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AktivnostExists(aktivnost.AktivnostId))
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
            return View(aktivnost);
        }

        // GET: Aktivnosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktivnost = await _context.Aktivnosts
                .FirstOrDefaultAsync(m => m.AktivnostId == id);
            if (aktivnost == null)
            {
                return NotFound();
            }

            return View(aktivnost);
        }

        // POST: Aktivnosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aktivnost = await _context.Aktivnosts.FindAsync(id);
            if (aktivnost != null)
            {
                _context.Aktivnosts.Remove(aktivnost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AktivnostExists(int id)
        {
            return _context.Aktivnosts.Any(e => e.AktivnostId == id);
        }
    }
}
