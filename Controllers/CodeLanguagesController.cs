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
    [Authorize(Roles ="Admin")]
    public class CodeLanguagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CodeLanguagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CodeLanguages
        public async Task<IActionResult> Index()
        {
            return View(await _context.CodingLanguages.ToListAsync());
        }

        // GET: CodeLanguages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeLanguage = await _context.CodingLanguages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codeLanguage == null)
            {
                return NotFound();
            }

            return View(codeLanguage);
        }

        // GET: CodeLanguages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CodeLanguages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CodeLanguage codeLanguage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codeLanguage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(codeLanguage);
        }

        // GET: CodeLanguages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeLanguage = await _context.CodingLanguages.FindAsync(id);
            if (codeLanguage == null)
            {
                return NotFound();
            }
            return View(codeLanguage);
        }

        // POST: CodeLanguages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CodeLanguage codeLanguage)
        {
            if (id != codeLanguage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codeLanguage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodeLanguageExists(codeLanguage.Id))
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
            return View(codeLanguage);
        }

        // GET: CodeLanguages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeLanguage = await _context.CodingLanguages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codeLanguage == null)
            {
                return NotFound();
            }

            return View(codeLanguage);
        }

        // POST: CodeLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codeLanguage = await _context.CodingLanguages.FindAsync(id);
            _context.CodingLanguages.Remove(codeLanguage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodeLanguageExists(int id)
        {
            return _context.CodingLanguages.Any(e => e.Id == id);
        }
    }
}
