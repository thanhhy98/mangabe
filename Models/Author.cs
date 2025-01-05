using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class Author
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public string Bio { get; set; }

    public ICollection<Manga> Mangas { get; set; } = new List<Manga>();
}