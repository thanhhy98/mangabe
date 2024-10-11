using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController: ControllerBase
    {
        private readonly TodoContext _context;

        public MangaController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manga>>> GetTodoItems()
        {
            return await _context.Mangas.ToListAsync();
        }

        // GET: api/TodoItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Manga>> GetTodoItem(long id)
        {
            var todoItem = await _context.Mangas.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(string id, Manga manga)
        {
            if (id != manga.Id)
            {
                return BadRequest();
            }

            _context.Entry(manga).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // POST: api/TodoItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Manga>> PostTodoItem(Manga manga)
        {
            
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = manga.Id }, manga);
        }

        // DELETE: api/TodoItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.Mangas.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Mangas.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(string id)
        {
            return _context.Mangas.Any(e => e.Id == id);
        }
    }
}
