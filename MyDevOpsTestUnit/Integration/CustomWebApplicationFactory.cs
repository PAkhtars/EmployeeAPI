using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EmployeeAPI.Infrastructure.Data;

namespace MyDevOpsTestUnit;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=EmployeeTest.db");
            });

            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}