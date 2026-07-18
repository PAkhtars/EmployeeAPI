using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using EmployeeAPI.Core.Models;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Infrastructure.Repositories;
using Xunit;

public class EmployeeRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=EmployeeTest.db")
            .Options;

        var context =  new AppDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }

    [Fact]
    public async Task GetAllEmployees_ShouldReturnEmpty_WhenDatabaseEmpty()
    {
        var context = GetDbContext();

        var repository = new EmployeeRepository(context);

        var result = await repository.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddEmployee_ShouldInsertEmployee()
    {
        var context = GetDbContext();

        var repository = new EmployeeRepository(context);

        var employee = new Employee
        {
            FirstName="John",
            LastName="Smith",
            Email="john@test.com",
            Phone="9876543210",
            Gender="Male",
            Department="IT",
            Designation="Developer",
            Salary=50000,
            Status="Active",
            City="Bangalore",
            State="Karnataka",
            Country="India",
            DateOfBirth=new DateTime(1995,1,1),
            JoinDate=DateTime.Now
        };

        await repository.AddAsync(employee);

        context.Employees.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetEmployeeById_ShouldReturnEmployee()
    {
        var context=GetDbContext();

        context.Employees.Add(new Employee
        {
            FirstName="John",
            LastName="Smith",
            Email="john@test.com"
        });

        context.SaveChanges();

        var repository=new EmployeeRepository(context);

        var employee=await repository.GetByIdAsync(1);

        employee.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateEmployee_ShouldUpdateSalary()
    {
        var context=GetDbContext();

        var emp=new Employee
        {
            FirstName="John",
            LastName="Smith",
            Salary=1000
        };

        context.Employees.Add(emp);
        context.SaveChanges();

        var repository=new EmployeeRepository(context);

        emp.Salary=25000;

        await repository.UpdateAsync(emp);

        var employee=await repository.GetByIdAsync(emp.EmployeeId);

        employee!.Salary.Should().Be(25000);
    }

    [Fact]
    public async Task DeleteEmployee_ShouldDeleteEmployee()
    {
        var context=GetDbContext();

        var emp=new Employee
        {
            FirstName="John",
            LastName="Smith"
        };

        context.Employees.Add(emp);
        context.SaveChanges();

        var repository=new EmployeeRepository(context);

        await repository.DeleteAsync(emp.EmployeeId);

        context.Employees.Count().Should().Be(0);
    }
}