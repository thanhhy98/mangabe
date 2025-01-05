using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

using AuthorModel = WebApplication3.Models.Author;

namespace WebApplication3.Features.Author;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly TodoContext _context;

    public AuthorController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/Author
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorModel>>> GetAuthors()
    {
        return await _context.Authors.ToListAsync();
    }

    // GET: api/Author/5
    [HttpGet("{search}")]
    public async Task<ActionResult<List<AuthorModel>>> GetAuthor(string search)
    {
        var results = await _context.Authors
            .Where(artist => artist.Name.Contains(search))
            .ToListAsync();

        return results;
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<AuthorModel>> GetAuthorById(string id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        return author;
    }
    // PUT: api/Author/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuthor(Guid id, AuthorModel author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }

        _context.Entry(author).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(id))
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

    // POST: api/Author
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost][ProducesResponseType(typeof(AuthorDtos), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuthorModel>> PostAuthor(AuthorDtos author)
    {

        var newAuthor = new AuthorModel
        {
            Name = author.Name,
            Bio = author.Bio,
        };
        
        _context.Authors.Add(newAuthor);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (AuthorExists(newAuthor.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthor.Id }, author);
    }

    // DELETE: api/Author/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(string id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AuthorExists(Guid id)
    {
        return _context.Authors.Any(e => e.Id == id);
    }
}