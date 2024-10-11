namespace WebApplication3.Models;

public class MangaGenre
{
    public string MangaId { get; set; }
    public Manga Manga { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}