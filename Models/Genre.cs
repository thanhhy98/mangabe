using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();
}