namespace WebApplication3.DTOs;

public class UserDto
{
    public string Name { get; set; }
    public string Password { get; set; }
};
public class RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}