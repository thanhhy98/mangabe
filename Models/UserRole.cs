namespace WebApplication3.Models;
using System.ComponentModel.DataAnnotations;

public class UserRole
{
    [Key]
    public int Id { get; set; }

    public string Role { get; set; } = null!;
    public string? Description { get; set; }
    
}