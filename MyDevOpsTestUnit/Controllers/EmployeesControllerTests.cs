using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using EmployeeAPI.Core.Models;
using Xunit;
using EmployeeAPI.WebApi.Controllers;
using EmployeeAPI.Interfaces;

public class EmployeesControllerTests
{
    private readonly Mock<IEmployeeRepository> _repository;

    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
        _repository=new Mock<IEmployeeRepository>();

        _controller=new EmployeesController(_repository.Object);
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnOk()
    {
        _repository.Setup(x=>x.GetAllAsync())
            .ReturnsAsync(new List<Employee>
            {
                new Employee
                {
                    EmployeeId=1,
                    FirstName="John"
                }
            });

        var result=await _controller.GetEmployees();

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetEmployee_ShouldReturnNotFound()
    {
        _repository.Setup(x=>x.GetByIdAsync(100))
            .ReturnsAsync((Employee)null);

        var result=await _controller.GetEmployee(100);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task PostEmployee_ShouldReturnCreated()
    {
        var emp=new Employee
        {
            EmployeeId=1,
            FirstName="John",
            LastName = "Smith",
            DateOfBirth = new DateTime(1995, 1, 1),
            Gender = "Male",
            Email = $"john{Guid.NewGuid()}@test.com",
            Phone = "9999999999",
            Department = "IT",
            Designation = "Developer",
            JoinDate = DateTime.Today,
            Salary = 50000,
            Status = "Active",
            City = "Bangalore",
            State = "Karnataka",
            Country = "India"
        };

        _repository.Setup(x=>x.AddAsync(It.IsAny<Employee>()))
            .ReturnsAsync(emp);

        var result=await _controller.PostEmployee(emp);

        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task DeleteEmployee_ShouldReturnOk()
    {
        var emp=new Employee
        {
            EmployeeId=1,
            FirstName="John"
        };

        _repository.Setup(x=>x.ExistsAsync(1))
            .ReturnsAsync(true);
        _repository.Setup(x=>x.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var result=await _controller.DeleteEmployee(1);

        result.Should().BeOfType<OkResult>();
    }
}