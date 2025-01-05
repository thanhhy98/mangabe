using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication3.DTOs;
using WebApplication3.Models;

namespace WebApplication3.Features.Auth;

public class AuthService(TodoContext context, IConfiguration configuration)
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<ApiResponse<User>> Register(RegisterDto userDto) {
        if (string.IsNullOrWhiteSpace(userDto.Username) || 
            string.IsNullOrWhiteSpace(userDto.Email) || 
            string.IsNullOrWhiteSpace(userDto.Password))
        {
            return new ApiResponse<User>(false, "Please fill all the fields");
        }

        // Check if the user already exists (optional)
        if (context.Users.Any(u => u.Email == userDto.Email) || context.Users.Any(u => u.Username == userDto.Username))
        {
            return new ApiResponse<User>(false, "A user with this email or username already exists.");
        }
        var newUser = new User(userDto.Username, userDto.Email, userDto.Password);

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, newUser.PasswordHash);
    
        // Save the new user to the database
        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        return new ApiResponse<User>(true, "User registered successfully", newUser);
    }
    public JwtSecurityToken GenerateJwtToken(string userName)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName)
        };
    
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        return new JwtSecurityToken(
            issuer: configuration["url"],
            audience: configuration["url"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);
    }
    public async Task<User?> FindUser(string identifier)
    {
        if (Guid.TryParse(identifier, out var userId))
        {
            // If the identifier is a valid GUID, search by ID
            return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    
        // If the identifier is not a valid GUID, search by username or email
        return await context.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.Username == identifier);
    }

    public  PasswordVerificationResult VerifyPassword(User dbUser , string password)
    {
      return  _passwordHasher.VerifyHashedPassword(dbUser, dbUser.PasswordHash, password);
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await context.Users.Select(x => ItemToDto(x)).ToListAsync();
        return users;
    }
    
    private static UserDto ItemToDto(User user) =>
        new ()
        {
            Name = user.Username
        };
}

public class ApiResponse<T>(bool success, string message, T? data = default )
{
    public bool Success { get; set; } = success;
    public string Message { get; set; } = message;
    public T? Data { get; set; } = data; // Holds the response data of type T
}