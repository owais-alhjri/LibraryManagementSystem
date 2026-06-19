using LMS.API.Middleware;
using LMS.Application.Interfaces;
using LMS.Application.Services;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Repositories;
using LMS.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.Text.Json.Serialization;
using LMS.Application.Settings;
using LMS.Domain.Entities;
using LMS.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    


var jwtSecret = builder.Configuration["JwtSettings:SecretKey"];

if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("JWT_SECRET is not configured!");
}

// DbContext
builder.Services.AddDbContext<LmsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBorrowRecordService, BorrowRecordService>();
builder.Services.AddScoped<IPasswordHasherService, IdentityPasswordHasher>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//CORS - Angular

builder.Services.AddCors(options =>
{

    options.AddPolicy("Angular", policy =>
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtSettings:SecretKey"]!
                )
            ),

            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<ITokenService,JwtTokenService>();


var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LmsDbContext>();
    db.Database.Migrate();

    var adminEmail = app.Configuration["AdminSettings:Email"]!;
    var adminPassword = app.Configuration["AdminSettings:Password"]!;
    var adminName = app.Configuration["AdminSettings:Name"]!;

    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();

    if (!db.Users.Any(u => u.Email == adminEmail))
    {
        var admin = new User(adminName, adminEmail, hasher.Hash(adminPassword));
        admin.SetRole(Roles.ADMIN);
        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }
}


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("Angular");

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
