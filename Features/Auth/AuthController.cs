
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using WebApplication3.DTOs;
using WebApplication3.Features.Auth;


namespace WebApplication3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{

    [HttpPost("register")][ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public async Task<ActionResult<User>> Register(RegisterDto userDto)
    {
            
        // Validate the input
        var newUser=await authService.Register(userDto);

        return CreatedAtAction(nameof(GetByName), new { id = newUser.Data.Id }, new { message = "User registered successfully!" });
    }

        
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UserDto userDto)
    {
        var dbUser = await authService.FindUser(userDto.Name);
        if(dbUser == null) return NotFound();
        var result = authService.VerifyPassword(dbUser, userDto.Password);
        if(result == PasswordVerificationResult.Failed) return BadRequest("Invalid password.");

        var token = authService.GenerateJwtToken(dbUser.Username);
        return Ok(new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });

    }
    
    // GET: api/<AuthController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get()
    {
        var users = await authService.GetAllUsers();
        return Ok(users);
    }

    // GET api/<AuthController>/5
    [HttpGet("one/{id}")]
    public async Task<ActionResult<UserDto>> GetByName(string id)
    {
        var user = await authService.FindUser(id);
        if (user == null)
        {
            return NotFound();
        }    
        return Ok(new UserDto
        {
            Name = user.Username
        });
    }
    
        
    // [HttpGet("google-login")]
    // public IActionResult GoogleLogin()
    // {
    //     var properties = new AuthenticationProperties { RedirectUri = "/api/Auth/google-callback",         Items =
    //     {
    //         { "scheme", "Google" },
    //     } };
    //
    //     return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    // }
    //
    // [HttpGet("google-callback")]
    // public async Task<IActionResult> GoogleCallback()
    // {
    //     Console.WriteLine(3333);
    //     var result = await HttpContext!.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
    //     if (result is { Succeeded: true, Principal: not null })
    //     {
    //         var googleUser = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     
    //         var claimsPrincipal = new ClaimsPrincipal(result.Principal);
    //
    //         var s = claimsPrincipal.Claims.ToList();
    //
    //         var principal = new ClaimsPrincipal(
    //             new ClaimsIdentity(s, CookieAuthenticationDefaults.AuthenticationScheme));
    //         await HttpContext.SignInAsync("Cookies",
    //             principal,
    //             new AuthenticationProperties()
    //             {
    //                 IsPersistent = true
    //             });
    //     }
    //     var authenticateResult = await HttpContext.AuthenticateAsync();
    //
    //     if (!authenticateResult.Succeeded)
    //         return BadRequest("Google authentication failed");
    //
    //     // Extract user information from authenticateResult.Principal
    //     var claims = authenticateResult.Principal.Claims;
    //     var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    //     var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    //     foreach (var product in claims) {
    //         Console.WriteLine(product);
    //     }
    //     if (name == null || email == null)
    //     {
    //         return NotFound();
    //     }
    //     // Here, you can create a JWT or do something with the authenticated user's information.
    //     var dbUser = _context.Users.Find(name);
    //         
    //     var claimsthis = new[]
    //     {
    //         new Claim(ClaimTypes.Name, name)
    //     };
    //
    //     var token = new JwtSecurityToken(
    //         issuer: "http://localhost:5065",
    //         audience: "http://localhost:5065",
    //         claims: claimsthis,
    //         expires: DateTime.UtcNow.AddMinutes(30),
    //         signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)), SecurityAlgorithms.HmacSha256));
    //     if (email != dbUser?.Username)
    //     {
    //         _context.Users.Add(new User(name, email, email));
    //         await _context.SaveChangesAsync();
    //              
    //         return Ok(new
    //         {
    //             Token = new JwtSecurityTokenHandler().WriteToken(token),
    //             Expiration = token.ValidTo
    //         });
    //     }
    //     return Ok(new
    //     {
    //         Token = new JwtSecurityTokenHandler().WriteToken(token),
    //         Expiration = token.ValidTo
    //     });
    //
    // }
}