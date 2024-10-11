using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;
public class Chapter
{
    [Key]
    public string Id { get; set; }

    public string MangaId { get; set; }
    public Manga Manga { get; set; }  // Navigation Property

    [Required]
    public string Title { get; set; }

    public int VolumeNumber { get; set; }

    public int ChapterNumber { get; set; }

    public string Language { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string ChapterFileUrl { get; set; }  // URL to the chapter pages
}