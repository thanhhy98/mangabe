using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Features.Manga;

[Route("api/[controller]")]
[ApiController]
public class MangaController(MangaService mangaService) : ControllerBase
{
    // GET: api/Manga
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<global::Manga>>> GetMangas()
    {
        var mangas = await mangaService.GetMangasAsync();
        return Ok(mangas);
    }

    // GET: api/Manga/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetMangaByIdDto>> GetManga(int id)
    {
        var manga = await mangaService.GetMangaByIdAsync(id);

        if (manga == null)
        {
            return NotFound();
        }

        return manga;
    }

    // PUT: api/Manga/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutManga(int id, global::Manga manga)
    {
        var result = await mangaService.UpdateMangaAsync(id, manga);
        if (!result)
        {
            return BadRequest();
        }

        return NoContent();
    }

    // POST: api/Manga
    [HttpPost][ProducesResponseType(typeof(PostMangaResponseDto), StatusCodes.Status201Created)] // For 201 Created
    public async Task<ActionResult<PostMangaResponseDto>> PostManga([FromForm] MangaDtos manga)
    {
        var createdManga = await mangaService.AddMangaAsync(manga);
        return CreatedAtAction(nameof(GetManga), new { id = createdManga.Id }, new PostMangaResponseDto()
        {
            Id = createdManga.Id
        });
    }
    // [HttpGet("image-url")]
    // public async Task<IActionResult> GetImageUrl(string bucketId, string fileName)
    // {
    //     try
    //     {
    //         // Get authorization token
    //         string authorizationToken = await _backblazeB2Service.GetAuthorizationToken();
    //
    //         // Generate a signed URL valid for 1 hour
    //         
    //         var signedUrlGenerator = new SignedUrlGenerator();
    //         string signedUrl = signedUrlGenerator.GenerateSignedUrl(fileName, authorizationToken);
    //         
    //         return Ok(new { SignedUrl = signedUrl });
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(new { Error = ex.Message });
    //     }
    // }
    // DELETE: api/Manga/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManga(long id)
    {
        var result = await mangaService.DeleteMangaAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}