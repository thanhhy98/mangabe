using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication3.Models;

public class Manga
{

    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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
    public ICollection<Author> Authors { get; set; } = new List<Author>();  // Navigation Property

    public ICollection<Artist> Artists { get; set; } =  new List<Artist>();  // Navigation Property

    public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    public ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();
}