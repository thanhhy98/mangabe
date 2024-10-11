using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication3.Models;

public class Manga
{
    [Key]
    public string Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime PublicationDate { get; set; }

    [MaxLength(200)]
    public string Status { get; set; }  // e.g., ongoing, completed

    [MaxLength(300)]
    public string CoverImageUrl { get; set; }

    // Relationships
    public string AuthorId { get; set; }
    public Author Author { get; set; }  // Navigation Property

    public string ArtistId { get; set; }
    public Artist Artist { get; set; }  // Navigation Property

    public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    public ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();
}