using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Features.Manga;

public class MangaDtos
{
    [MaxLength(200)]
    public string Title { get; set; } 
    public string Description { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Status   { get; set; }
    public IFormFile CoverImageUrl { get; set; }
    public Guid AuthorId { get; set; }
    public Guid ArtistId { get; set; }

}

public class GetMangaByIdDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Status { get; set; }
    public string CoverImageUrl { get; set; }
    public List<ChapterDto> Chapters { get; set; } // Include Chapters here
}

public class ChapterDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int VolumeNumber { get; set; }
}

public class PostMangaResponseDto
{
    public int Id { get; set; }
}