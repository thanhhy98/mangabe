using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

using ArtistModel = WebApplication3.Models.Artist;
namespace WebApplication3.Features.Artist
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController(TodoContext context) : ControllerBase
    {
        // GET: api/Artist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistModel>>> GetArtists()
        {
            return await context.Artists.ToListAsync();
        }

        // GET: api/Artist/5
        [HttpGet("{search}")]
        public async Task<ActionResult<List<ArtistModel>>> GetArtist(string search)
        {
            var results = await context.Artists
                .Where(artist => artist.Name.Contains(search))
                .ToListAsync();

            return results;
        }
        
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ArtistModel>> GetArtistById(string id)
        {
            var artist = await context.Artists.FindAsync(id);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }


        // PUT: api/Artist/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(Guid id, ArtistModel artist)
        {
            if (id != artist.Id)
            {
                return BadRequest();
            }

            context.Entry(artist).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        // POST: api/Artist
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost][ProducesResponseType(typeof(ArtistModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<ArtistModel>> PostArtist(ArtistDto artistDto)
        {

            var artist = new ArtistModel
            {
                Name = artistDto.Name,
                Bio = artistDto.Bio
            };
            context.Artists.Add(artist);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArtistExists(artist.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetArtistById), new { id = artist.Id }, artist);
        }

        // DELETE: api/Artist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(string id)
        {
            var artist = await context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            context.Artists.Remove(artist);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistExists(Guid id)
        {
            return context.Artists.Any(e => e.Id == id);
        }
    }
}
