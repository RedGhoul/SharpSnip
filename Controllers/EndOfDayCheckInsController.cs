using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Snips.Data;

namespace Snips.Controllers
{
    [Authorize]
    public class EndOfDayCheckInsController : Controller
    {
        private readonly SnipsContext _context;

        public EndOfDayCheckInsController(SnipsContext context)
        {
            _context = context;
        }

        // GET: EndOfDayCheckIns
        public async Task<IActionResult> Index()
        {
            var snipsContext = _context.EndOfDayCheckIn.Include(e => e.ApplicationUser);
            return View(await snipsContext.ToListAsync());
        }

        // GET: EndOfDayCheckIns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endOfDayCheckIn = await _context.EndOfDayCheckIn
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endOfDayCheckIn == null)
            {
                return NotFound();
            }

            return View(endOfDayCheckIn);
        }

        // GET: EndOfDayCheckIns/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: EndOfDayCheckIns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comments,WhatWentWell,WhatWentBad,ApplicationUserId,Created,LastModified")] EndOfDayCheckIn endOfDayCheckIn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(endOfDayCheckIn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", endOfDayCheckIn.ApplicationUserId);
            return View(endOfDayCheckIn);
        }

        // GET: EndOfDayCheckIns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endOfDayCheckIn = await _context.EndOfDayCheckIn.FindAsync(id);
            if (endOfDayCheckIn == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", endOfDayCheckIn.ApplicationUserId);
            return View(endOfDayCheckIn);
        }

        // POST: EndOfDayCheckIns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comments,WhatWentWell,WhatWentBad,ApplicationUserId,Created,LastModified")] EndOfDayCheckIn endOfDayCheckIn)
        {
            if (id != endOfDayCheckIn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endOfDayCheckIn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EndOfDayCheckInExists(endOfDayCheckIn.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", endOfDayCheckIn.ApplicationUserId);
            return View(endOfDayCheckIn);
        }

        // GET: EndOfDayCheckIns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endOfDayCheckIn = await _context.EndOfDayCheckIn
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endOfDayCheckIn == null)
            {
                return NotFound();
            }

            return View(endOfDayCheckIn);
        }

        // POST: EndOfDayCheckIns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var endOfDayCheckIn = await _context.EndOfDayCheckIn.FindAsync(id);
            _context.EndOfDayCheckIn.Remove(endOfDayCheckIn);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EndOfDayCheckInExists(int id)
        {
            return _context.EndOfDayCheckIn.Any(e => e.Id == id);
        }
    }
}
