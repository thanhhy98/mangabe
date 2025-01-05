using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models;
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<Manga> Mangas { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MangaGenre> MangaGenres { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuring the many-to-many relationship between Manga and Genre
        modelBuilder.Entity<MangaGenre>()
            .HasKey(mg => new { mg.MangaId, mg.GenreId });
    
        modelBuilder.Entity<MangaGenre>()
            .HasOne(mg => mg.Manga)
            .WithMany(m => m.MangaGenres)
            .HasForeignKey(mg => mg.MangaId);

        modelBuilder.Entity<MangaGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MangaGenres)
            .HasForeignKey(mg => mg.GenreId);
        
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

    }
    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<User>();
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreateAt = DateTime.UtcNow; // Set CreatedAt when a new entity is added
                    entry.Entity.UpdateAt = DateTime.UtcNow; // Set UpdatedAt to the current time
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdateAt = DateTime.UtcNow; // Update UpdatedAt when the entity is modified
                    break;
            }
        }

        return base.SaveChanges();
    }
}