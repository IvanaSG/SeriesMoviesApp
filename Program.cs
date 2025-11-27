using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SeriesMovieTrailers.Data;
using SeriesMovieTrailers.Models;
using SeriesMovieTrailers.Services;
using Serilog;
using System.Net.Http.Headers;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
// Configuration
var configuration = builder.Configuration;
var services = builder.Services;


// Serilog
Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(configuration)
.Enrich.FromLogContext()
.WriteTo.Console()
.CreateLogger();

builder.Host.UseSerilog();


// DbContext - SQL Server
services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


// Identity
services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Redis distributed cache
services.AddStackExchangeRedisCache(options => {
    options.Configuration = configuration.GetConnectionString("RedisConnection");
});

// CORS - allow your frontend(s)
services.AddCors(o => {
    o.AddPolicy("AllowTrustedOrigins", p =>
    {
        p.WithOrigins("https://localhost:3000", "https://frontend-page.example.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// HttpClient for TMDb (typed)
services.AddHttpClient("tmdb", client => {
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Authentication - JWT bearer
var jwtKey = configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key missing");
var key = Encoding.ASCII.GetBytes(jwtKey);
services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromMinutes(2)
    };
});


services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


// Application services
services.AddScoped<IMovieService, MovieService>();
services.AddScoped<IJwtTokenService, JwtTokenService>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Security headers middleware
app.Use(async (context, next) => {
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    // Content-Security-Policy - allow frames from youtube/vimeo for embeds and scripts from self
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; frame-src https://www.youtube.com https://player.vimeo.com; img-src 'self' https://image.tmdb.org https:; script-src 'self' 'unsafe-inline';";
    await next();
});


app.UseHttpsRedirection();
app.UseCors("AllowTrustedOrigins");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


try
{
    Log.Information("Starting SecureMoviesApi");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
