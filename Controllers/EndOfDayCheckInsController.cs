using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Snips.Data;

namespace Snips.Controllers
{
    [Authorize]
    public class EndOfDayCheckInsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EndOfDayCheckInsController> _logger;
        public EndOfDayCheckInsController(ILogger<EndOfDayCheckInsController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // GET: EndOfDayCheckIns
        public async Task<IActionResult> Index()
        {
            var snipsContext = _context.EndOfDayCheckIns
                .Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()) && x.Deleted == false);
            return View(await snipsContext.ToListAsync());
        }

        // GET: EndOfDayCheckIns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endOfDayCheckIn = await _context.EndOfDayCheckIns
                .Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()) && x.Deleted == false)
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
            return View();
        }

        // POST: EndOfDayCheckIns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comments,WhatWentWell,WhatWentBad,Created")] EndOfDayCheckIn endOfDayCheckIn)
        {
            endOfDayCheckIn.LastModified = DateTime.UtcNow;
            if (endOfDayCheckIn.Created == new DateTime())
            {
                endOfDayCheckIn.Created = DateTime.UtcNow;
            }
            endOfDayCheckIn.ApplicationUserId = GetCurrentUserId();
            _context.Add(endOfDayCheckIn);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could properly save EndOfDayCheckin");
                return View(endOfDayCheckIn);
            }
        }

        // GET: EndOfDayCheckIns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endOfDayCheckIn = await _context.EndOfDayCheckIns
                .Where(x => x.Id == id && x.Deleted == false).FirstOrDefaultAsync();
            if (endOfDayCheckIn == null)
            {
                return NotFound();
            }
            return View(endOfDayCheckIn);
        }

        // POST: EndOfDayCheckIns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comments,WhatWentWell,WhatWentBad,Created")] EndOfDayCheckIn endOfDayCheckIn)
        {
            if (id != endOfDayCheckIn.Id)
            {
                return NotFound();
            }

            try
            {
                endOfDayCheckIn.LastModified = DateTime.UtcNow;                
                endOfDayCheckIn.ApplicationUserId = GetCurrentUserId();
                _context.Update(endOfDayCheckIn);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EndOfDayCheckInExists(endOfDayCheckIn.Id))
                {
                    _logger.LogError(ex, $"DbUpdateConcurrencyException occured when " +
                        $"trying to update non existant");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"DbUpdateConcurrencyException occured when " +
                       $"trying to update endOfDayCheckIn with Id {endOfDayCheckIn.Id}");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: EndOfDayCheckIns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endOfDayCheckIn = await _context.EndOfDayCheckIns
                .Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()))
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
            var endOfDayCheckIn = await _context.EndOfDayCheckIns.FindAsync(id);
            endOfDayCheckIn.Deleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EndOfDayCheckInExists(int id)
        {
            return _context.EndOfDayCheckIns.Any(e => e.Id == id);
        }

        private string GetCurrentUserId()
        {
            if (User != null)
            {
                return _userManager.GetUserId(User);
            }
            else { return null; }
        }
    }
}
