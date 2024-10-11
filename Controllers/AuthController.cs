
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

public class UserDto
{
    public string Name { get; set; }
    public string Password { get; set; }
};

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(TodoContext context) : ControllerBase
    {
        private const string Key = "Sjnr8WeKSMry00u4xp1+nhRj/yAxWpzwdb707EjPgfg=";
        private readonly TodoContext _context = context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto userDto)
        {
            
            // Validate the input
            if (string.IsNullOrWhiteSpace(userDto.Username) || 
                string.IsNullOrWhiteSpace(userDto.Email) || 
                string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Invalid user data.");
            }

            // Check if the user already exists (optional)
            if (_context.Users.Any(u => u.Email == userDto.Email) || _context.Users.Any(u => u.Username == userDto.Username))
            {
                return Conflict("A user with this email or username already exists.");
            }
            var newUser = new User(userDto.Username, userDto.Email, userDto.Password);

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, newUser.PasswordHash);
    
            // Save the new user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByName), new { id = newUser.Id }, new { message = "User registered successfully!" });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Name || u.Username == userDto.Name);
            if(dbUser == null) return NotFound();
            var result = _passwordHasher.VerifyHashedPassword(dbUser, dbUser.PasswordHash, userDto.Password);
            if(result == PasswordVerificationResult.Failed) return BadRequest("Invalid password.");
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userDto.Name)
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5065",
                audience: "http://localhost:5065",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)), SecurityAlgorithms.HmacSha256));

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });

        }
    
        // GET: api/<AuthController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var users = await _context.Users.Select(x => ItemToDto(x)).ToListAsync();
            return Ok(users);
        }

        // GET api/<AuthController>/5
        [HttpGet("one/{id}")]
        public async Task<ActionResult<UserDto>> GetByName(Guid id)
        {
            Console.WriteLine(id);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }    
            return Ok(ItemToDto(user));
        }
        

        private static UserDto ItemToDto(User user) =>
            new()
            {
                Name = user.Username
            };
        
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/Auth/google-callback",         Items =
            {
                { "scheme", "Google" },
            } };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            Console.WriteLine(3333);
            var result = await HttpContext!.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result is { Succeeded: true, Principal: not null })
            {
                var googleUser = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
                var claimsPrincipal = new ClaimsPrincipal(result.Principal);

                var s = claimsPrincipal.Claims.ToList();

                var principal = new ClaimsPrincipal(
                    new ClaimsIdentity(s, CookieAuthenticationDefaults.AuthenticationScheme));
                await HttpContext.SignInAsync("Cookies",
                    principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = true
                    });
            }
            var authenticateResult = await HttpContext.AuthenticateAsync();

            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed");

            // Extract user information from authenticateResult.Principal
            var claims = authenticateResult.Principal.Claims;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            foreach (var product in claims) {
                Console.WriteLine(product);
            }
            if (name == null || email == null)
            {
                return NotFound();
            }
            // Here, you can create a JWT or do something with the authenticated user's information.
            var dbUser = _context.Users.Find(name);
            
            var claimsthis = new[]
            {
                new Claim(ClaimTypes.Name, name)
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5065",
                audience: "http://localhost:5065",
                claims: claimsthis,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)), SecurityAlgorithms.HmacSha256));
            if (email != dbUser?.Username)
            {
                _context.Users.Add(new User(name, email, email));
                 await _context.SaveChangesAsync();
                 
                 return Ok(new
                 {
                     Token = new JwtSecurityTokenHandler().WriteToken(token),
                     Expiration = token.ValidTo
                 });
            }
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });

        }
    }
}
