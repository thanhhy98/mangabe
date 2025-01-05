using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;
public class Chapter
{
    [Key]
    public Guid Id { get; set; } =  Guid.NewGuid();

    public int MangaId { get; set; }
    public Manga Manga { get; set; }  // Navigation Property

    [Required]
    public string Title { get; set; }

    public int VolumeNumber { get; set; }

    public int ChapterNumber { get; set; }

    public string Language { get; set; }

    public DateTime ReleaseDate { get; set; }

    public int TotalPages { get; set; }  // Total number of pages

    public List<string> PageUrls { get; set; }  // URLs of the individual pages
}