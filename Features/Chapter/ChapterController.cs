using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Features.Chapter;
using WebApplication3.Features.Manga;
using WebApplication3.Models;

namespace WebApplication3.Features
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController(TodoContext context, ChapterService chapterService) : ControllerBase
    {
        // GET: api/Chapter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Chapter>>> GetChapters()
        {
            return await context.Chapters.ToListAsync();
        }

        // GET: api/Chapter/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Chapter>> GetChapter(Guid id)
        {
            var chapter = await context.Chapters.FindAsync(id);

            if (chapter == null)
            {
                return NotFound();
            }

            return Ok(chapter);
        }

        // PUT: api/Chapter/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChapter(Guid id, Models.Chapter chapter)
        {
            if (id != chapter.Id)
            {
                return BadRequest();
            }

            context.Entry(chapter).State = EntityState.Modified;

            try
            { 
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChapterExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Chapter
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost][Authorize][ProducesResponseType(typeof(Models.Chapter), StatusCodes.Status201Created)]
        public async Task<ActionResult<Models.Chapter>> PostChapter(ChapterDtos chapter)
        {
            var newChapter = new Models.Chapter();
            try
            {
                newChapter = await chapterService.AddChapter(chapter);
            }
            catch (DbUpdateException)
            {
                if (ChapterExists(newChapter.Id))
                {
                    return Conflict();
                }

                throw;
            }

            return CreatedAtAction("GetChapter", new { id = newChapter.Id }, newChapter);
        }

        // DELETE: api/Chapter/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(string id)
        {
            var chapter = await context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }

            context.Chapters.Remove(chapter);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChapterExists(Guid id)
        {
            return context.Chapters.Any(e => e.Id == id);
        }
    }
}
