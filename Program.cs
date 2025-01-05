using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using WebApplication3.Features.Auth;
using WebApplication3.Features.Manga;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WebApplication3.Clients.BackblazeService;
using WebApplication3.Features.Chapter;
using WebApplication3.Utils;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection("AwsOptions"));

var reactAppUrl = builder.Configuration["FrontEndUrl"] ?? "http://localhost:5173"; // Fallback to default if not set
var url = builder.Configuration["url"] ?? "http://localhost:5065"; // Fallback to default if not set

// Register S3 client with Backblaze B2 (S3-compatible) settings
builder.Services.AddSingleton<IAmazonS3>(sp => {
    var awsOptions = sp.GetRequiredService<IOptions<AwsOptions>>().Value;
    return new AmazonS3Client(awsOptions.AccessKey, awsOptions.SecretKey, new AmazonS3Config
    {
        ServiceURL = awsOptions.ServiceUrl, 
        ForcePathStyle = true 
    });
});

builder.Services.AddScoped<IUploadToBackBlaze, UploadToBackBlaze>();


builder.Services.AddScoped<MangaService>();  // Add this line
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ChapterService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddScoped<BackblazeB2Service>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<AwsOptions>>().Value;

    if (string.IsNullOrEmpty(options.AccessKey) || string.IsNullOrEmpty(options.SecretKey))
    {
        throw new InvalidOperationException("Backblaze configuration is missing ApplicationKeyId or ApplicationKey.");
    }

    return new BackblazeB2Service(options.AccessKey, options.SecretKey);

});

// Add CORS policy allowing requests from React (localhost:5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        b => b
            .WithOrigins(reactAppUrl, url)  // React app URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            );
});

//to be implement
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
// });

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
            ValidIssuer =url,
            ValidAudience = url,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });

//For google authentication

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default for authentication
//     options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Default for challenge
//     options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default sign-in scheme
//
// }).AddCookie("Cookies").AddGoogle(googleOptions =>
// {
//     var accessKey = builder.Configuration["GoogleOptions:AccessKey"] ?? ""; // Fallback to default if not set
//     var secretKey = builder.Configuration["GoogleOptions:SecretKey"] ?? ""; // Fallback to default if not set
//
//     googleOptions.ClientId =  accessKey;
//     googleOptions.ClientSecret = secretKey;
// });


builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));  // PostgreSQL setup

// Add Swagger services if needed
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JadeWebAPI", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT token must be provided",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Name = "Bearer",
    //            In = ParameterLocation.Header,
    //            Reference = new OpenApiReference
    //            {
    //                Id="Bearer",
    //                Type=ReferenceType.SecurityScheme,
    //            }
    //        },
    //        new string[]{ }
    //    }
    //});

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

var app = builder.Build();

app.UseCors("AllowReactApp");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();

