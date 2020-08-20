using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Snips.Data;
using Snips.Models;

namespace Snips.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public NotesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        } 

        // GET: Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string SearchTerm, DateTime CreatedDate, DateTime LastModifiedDate)
        {
            IQueryable<Note> snipsQuery = _context.Notes.Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()));


            if (!string.IsNullOrEmpty(SearchTerm))
            {
                var SearchTermEsc = SearchTerm.Trim().Replace(" ", "|");
                snipsQuery = snipsQuery
                        .Where(x => x.SearchVector.Matches(EF.Functions.ToTsQuery(SearchTermEsc)));
            }


            if (LastModifiedDate != new DateTime()) {

                snipsQuery = snipsQuery.Where(x => x.LastModified.Value.Date == LastModifiedDate.ToUniversalTime().Date);
            }

            if (CreatedDate != new DateTime())
            {
                snipsQuery = snipsQuery.Where(x => x.Created.Date == CreatedDate.ToUniversalTime().Date);
            }
            var snipsQueryItems = snipsQuery.Select(x => new NoteDTO
            {
                Id = x.Id,
                Name = x.Name,
                HasCode = x.HasCode,
                CodeLanguage = x.CodeLanguage,
                Created = x.Created,
                LastModified = (DateTime)x.LastModified
            });
            return View("Index",await snipsQueryItems.ToListAsync());
        }

        // GET: Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReIndex()
        {
            IQueryable<Note> snipsQuery = _context.Notes.Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()));

            var allSnipQueries = await snipsQuery.ToListAsync();

            foreach (var item in allSnipQueries)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var snipsQueryItems = _context.Notes.Select(x => new
            NoteDTO
            {
                Id = x.Id,
                Name = x.Name,
                HasCode = x.HasCode,
                CodeLanguage = x.CodeLanguage,
                Created = x.Created,
                LastModified = (DateTime)x.LastModified
            }).ToListAsync();
            return View(await snipsQueryItems);
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Content,CodeLanguage,CodeContent")] Note note)
        {
            if (ModelState.IsValid)
            {
                note.ApplicationUserId = GetCurrentUserId();
                if (string.IsNullOrEmpty(note.CodeContent))
                {
                    note.HasCode = false;
                }
                else
                {
                    note.HasCode = true;
                }
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", note.ApplicationUserId);
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", note.ApplicationUserId);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Content,CodeLanguage,CodeContent")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    note.ApplicationUserId = GetCurrentUserId();
                    if (string.IsNullOrEmpty(note.CodeContent))
                    {
                        note.HasCode = false;
                    }
                    else
                    {
                        note.HasCode = true;
                    }
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", note.ApplicationUserId);
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
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
