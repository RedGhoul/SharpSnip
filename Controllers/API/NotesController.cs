using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snips.Data;
using Snips.Models;

namespace Snips.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : BaseController
    {

        public NotesController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager):base(context, userManager)
        {
        }

        // POST: api/Notes/NoteTitle
        [HttpPost("NoteName/{id}")]
        public async Task<ActionResult> UpdateCodeName(int id, [FromBody] UpdateNoteName value)
        {
            var curUser = await GetCurrentUser();
            var note = await _context.Notes.Where(
                x => x.Id == id &&
                x.ApplicationUserId.Equals(curUser.Id)).FirstOrDefaultAsync();

            if (note == null)
            {
                return NotFound();
            }

            note.Name = value.noteName;
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Saved");
        }

        // POST: api/Notes/NoteContent
        [HttpPost("NoteContent/{id}")]
        public async Task<ActionResult> UpdateCodeContent(int id,[FromBody] UpdateNoteContent value)
        {
            var curUser = await GetCurrentUser();
            var note = await _context.Notes.Where(
                x => x.Id == id && 
                x.ApplicationUserId.Equals(curUser.Id)).FirstOrDefaultAsync();

            if (note == null)
            {
                return NotFound();
            }

            note.Content = value.noteContent;
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Saved");
        }

        // POST: api/Notes/CodeContent
        [HttpPost("CodeContent/{id}")]
        public async Task<ActionResult> UpdateCodeContent(int id, [FromBody] UpdateCodeContent updateCodeContent)
        {
            var curUser = await GetCurrentUser();
            var note = await _context.Notes.Where(
                x => x.Id == id &&
                x.ApplicationUserId.Equals(curUser.Id)).FirstOrDefaultAsync();

            if (note == null)
            {
                return NotFound();
            }

            note.CodeContent = updateCodeContent.codeContent;
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Saved");
        }

        // POST: api/Notes/CodeLang
        [HttpPost("CodeLang/{id}/{langId}")]
        public async Task<ActionResult> UpdateCodeLang(int id, int langId)
        {
            var curUser = await GetCurrentUser();
            var note = await _context.Notes.Where(
                x => x.Id == id &&
                x.ApplicationUserId.Equals(curUser.Id)).FirstOrDefaultAsync();

            var codeLang = await _context.CodingLanguages.FindAsync(langId);

            if (note == null || codeLang == null)
            {
                return NotFound();
            }

            note.CodingLanguageId = codeLang.Id;
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Saved");
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNote()
        {
            return await _context.Notes.ToListAsync();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Note>> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return note;
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
