using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add CORS policy allowing requests from React (localhost:5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        b => b
            .WithOrigins("http://localhost:5173")  // React app URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var key = "Sjnr8WeKSMry00u4xp1+nhRj/yAxWpzwdb707EjPgfg="; // Use a more secure key in production
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer ="http://localhost:5065",
            ValidAudience = "http://localhost:5065",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default for authentication
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Default for challenge
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default sign-in scheme

}).AddCookie("Cookies").AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "213129353998-mmn40tib0min40tbcfdalq6sk1pk33vl.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
});


builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));  // PostgreSQL setup

// Add Swagger services if needed
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowReactApp");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure authentication is enabled
app.UseAuthorization(); 

app.MapControllers();

app.Run();
