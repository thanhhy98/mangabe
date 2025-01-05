using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Features.Manga;

public class ChapterDtos
{
    public int MangaId { get; set; } 
    public string Title { get; set; }
    public int VolumeNumber { get; set; }

    public int ChapterNumber { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Language   { get; set; }
    public List<IFormFile> PageFiles { get; set; }

}