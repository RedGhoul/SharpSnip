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
using Snips.Models;

namespace Snips.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<NotesController> _logger;

        public NotesController(ILogger<NotesController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var snipsQueryItems = _context.Notes.Include(x => x.CodingLanguage)
                .Where(x => x.Deleted == false)
                .OrderByDescending(x => x.LastModified)
                .Select(x => new
            NoteDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    HasCode = x.HasCode,
                    CodeLanguage = x.CodingLanguage.Name,
                    Created = x.Created,
                    LastModified = x.LastModified.GetValueOrDefault(DateTime.UtcNow)
                }).ToListAsync();
            return View(await snipsQueryItems);
        }

        // POST: Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string SearchTerm, DateTime CreatedDate, DateTime LastModifiedDate)
        {
            IQueryable<Note> snipsQuery = _context.Notes.Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()) && x.Deleted == false);


            if (!string.IsNullOrEmpty(SearchTerm))
            {
                var SearchTermEsc = SearchTerm.Trim().Replace(" ", "|");
                snipsQuery = snipsQuery
                        .Where(x => x.SearchVector.Matches(EF.Functions.ToTsQuery(SearchTermEsc)));
            }


            if (LastModifiedDate != new DateTime())
            {

                snipsQuery = snipsQuery.Where(x => x.LastModified.Value.Date == LastModifiedDate.ToUniversalTime().Date);
            }

            if (CreatedDate != new DateTime())
            {
                snipsQuery = snipsQuery.Where(x => x.Created.Date == CreatedDate.ToUniversalTime().Date);
            }
            var snipsQueryItems = snipsQuery.Include(x => x.CodingLanguage).OrderByDescending(x => x.LastModified).Select(x => new NoteDTO
            {
                Id = x.Id,
                Name = x.Name,
                HasCode = x.HasCode,
                CodeLanguage = x.CodingLanguage.Name,
                Created = x.Created,
                LastModified = (DateTime)x.LastModified
            });

            try
            {
                return View("Index", await snipsQueryItems.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching the DB for Notes with SearchTerms {SearchTerm}");
                var AllUserSnips = await _context.Notes.Include(x => x.CodingLanguage).Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()) && x.Deleted == false).Select(x => new NoteDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    HasCode = x.HasCode,
                    CodeLanguage = x.CodingLanguage.Name,
                    Created = x.Created,
                    LastModified = (DateTime)x.LastModified
                }).ToListAsync();
                return View("Index", AllUserSnips);
            }
        }

        // GET: Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReIndex()
        {
            IQueryable<Note> snipsQuery = _context.Notes.Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()) && x.Deleted == false);

            var allSnipQueries = await snipsQuery.ToListAsync();

            foreach (var item in allSnipQueries)
            {
                item.LastModified = DateTime.UtcNow;
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.Where(x => x.Deleted == false)
                .Include(x => x.ApplicationUser)
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
            var vm = new NoteEditViewModel
            {
                Note = new Note()
                {
                    CodingLanguageId = 0
                },
                CodingLanguages = new SelectList(_context.CodingLanguages.ToList(), "Id", "Name")
            };
            return View(vm);
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(vm.Note.Name))
                {
                    return View(vm.Note);
                }
                vm.Note.ApplicationUserId = GetCurrentUserId();
                if (string.IsNullOrEmpty(vm.Note.CodeContent))
                {
                    vm.Note.HasCode = false;
                }
                else
                {
                    vm.Note.HasCode = true;
                }

                vm.Note.LastModified = DateTime.UtcNow;
                vm.Note.Created = DateTime.UtcNow;

                _context.Add(vm.Note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm.Note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(x => x.CodingLanguage)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            var vm = new NoteEditViewModel
            {
                Note = note,
                CodingLanguages = new SelectList(_context.CodingLanguages.ToList(), "Id", "Name")
            };

            return View(vm);
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteEditViewModel vm)
        {
            if (id != vm.Note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vm.Note.ApplicationUserId = GetCurrentUserId();
                    if (string.IsNullOrEmpty(vm.Note.CodeContent))
                    {
                        vm.Note.HasCode = false;
                    }
                    else
                    {
                        vm.Note.HasCode = true;
                    }
                    vm.Note.LastModified = DateTime.UtcNow;
   
                    _context.Update(vm.Note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(vm.Note.Id))
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
            return View(vm);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()))
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
            var note = await _context.Notes
                .Where(x => x.ApplicationUserId.Equals(GetCurrentUserId()) && x.Id == id)
                .FirstOrDefaultAsync();
            if (note != null)
            {
                note.Deleted = true;
                await _context.SaveChangesAsync();
            }

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
