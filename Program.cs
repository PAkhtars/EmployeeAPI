using EmployeeAPI.Application.Services;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Infrastructure.Repositories;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ New .NET 9+ OpenAPI + SwaggerUI Setup
builder.Services.AddOpenApi();

// Choose connection string based on environment or USE_TEST_DB environment variable.
var useTestDb = builder.Environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase)
            || Environment.GetEnvironmentVariable("USE_TEST_DB") == "true";
 if (useTestDb)
{
    var testDbPath = Path.Combine(builder.Environment.ContentRootPath, "EmployeeTest.db");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite($"Data Source={testDbPath}"));
}
else
{           
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IActMasterRepository, ActMasterRepository>();
builder.Services.AddScoped<IActDetailsRepository, ActDetailsRepository>();
builder.Services.AddScoped<ILegalCategoryMasterRepository, LegalCategoryMasterRepository>();
// Add JWT Authentication
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IPollRepository, PollRepository>();
builder.Services.AddScoped<PollService>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<VoteService>();

builder.Services.Configure<EmployeeAPI.Core.Models.MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<EmployeeAPI.Interfaces.IEmailRepository, EmployeeAPI.Infrastructure.Repositories.EmailRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
    app.MapOpenApi();                    // Required for .NET 9+

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Employee API V1");
        options.RoutePrefix = string.Empty;   // Makes Swagger UI appear at root[](https://localhost:xxxx/)
    });
    if (useTestDb)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Users.RemoveRange(db.Users);
        db.Employees.RemoveRange(db.Employees);
        db.Polls.RemoveRange(db.Polls);
        //db.Votes.RemoveRange(db.Votes);
        await db.SaveChangesAsync();
    }
    app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();

public partial class Program { }