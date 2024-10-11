namespace WebApplication3.Models;
using System.ComponentModel.DataAnnotations;

public class UserSetting
{
    [Key]
    public string Id { get; set; }

    public bool DarkMode { get; set; } = true;
    public string PreferedLanguage { get; set; } = String.Empty;
}