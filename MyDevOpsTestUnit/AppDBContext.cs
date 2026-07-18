using Microsoft.EntityFrameworkCore;
using EmployeeAPI.Core.Models;

public class AppTestDbContext : DbContext
{
    public AppTestDbContext(DbContextOptions<AppTestDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
}