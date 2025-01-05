using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Clients.BackblazeService;
using WebApplication3.Models;

namespace WebApplication3.Features.Manga;

public class MangaService
{
    private readonly IUploadToBackBlaze _uploadUtility;
    private readonly TodoContext _context;
    public MangaService(TodoContext context, IUploadToBackBlaze uploadUtility)
    {
        _context = context;
        _uploadUtility = uploadUtility;
    }

    public async Task<List<global::Manga>> GetMangasAsync()
    {

        return await _context.Mangas.ToListAsync();
    }

    public async Task<GetMangaByIdDto?> GetMangaByIdAsync(int id)
    {
        return await _context.Mangas
            .Include(m => m.Chapters)
            .Select(m => new GetMangaByIdDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                PublicationDate = m.PublicationDate,
                Status = m.Status,
                CoverImageUrl = m.CoverImageUrl,
                Chapters = m.Chapters.Select(c => new ChapterDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    ReleaseDate = c.ReleaseDate,
                    VolumeNumber = c.VolumeNumber
                }).ToList()
            })
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<global::Manga> AddMangaAsync(MangaDtos mangaDtos)
    {

        
        var artist = await _context.Artists.FindAsync(mangaDtos.ArtistId);
        var author = await _context.Authors.FindAsync(mangaDtos.AuthorId);
        if (artist == null || author == null)
        {
            throw new Exception("Artist does not exist.");
        }

        var fileUrl = await _uploadUtility.Upload(mangaDtos.CoverImageUrl);
        
        var manga = new global::Manga
        {
            Title = mangaDtos.Title,
            Description = mangaDtos.Description,
            PublicationDate = mangaDtos.PublicationDate,
            CoverImageUrl = fileUrl,
            Artists = new List<Models.Artist>()
            {
                artist
            },
            Authors = new List<Models.Author>()
            {
                author
            }
           ,   
            Status = mangaDtos.Status,
        };
        _context.Mangas.Add(manga);
        await _context.SaveChangesAsync();
        return manga;
    }

    public async Task<bool> UpdateMangaAsync(int id, global::Manga manga)
    {
        if (id != manga.Id)
        {
            return false;
        }

        _context.Entry(manga).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MangaExists(id))
            {
                return false;
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<bool> DeleteMangaAsync(long id)
    {
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return false;
        }

        _context.Mangas.Remove(manga);
        await _context.SaveChangesAsync();
        return true;
    }

    
    public bool MangaExists(int id)
    {
        return _context.Mangas.Any(e => e.Id == id);
    }
}