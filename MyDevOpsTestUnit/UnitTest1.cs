using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using EmployeeAPI.Core.Models;
namespace MyDevOpsTestUnit;

public class EmployeesApiTests
{
    private readonly HttpClient _client;
   private Employee _employee;
   private Employee createdEmployee;
    public EmployeesApiTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7105/")
        };
        _employee=CreateEmployee();
        createdEmployee=null!;
        Environment.SetEnvironmentVariable(
            "ASPNETCORE_ENVIRONMENT",
            "Testing");
      
    }

    [Fact]
    public async Task GetEmployees_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("api/Employees");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetEmployees_ReturnsEmployees()
    {
        // Act
        var response = await _client.GetAsync("api/Employees");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(content));
    }
    private Employee CreateEmployee()
    {
        return new Employee
        {
            FirstName = "Parvez",
            LastName = "Akhtar Test",
            DateOfBirth = new DateTime(DateTime.Now.Year - 30, 1, 1),
            Gender = "Male",
            Email = $"john{Guid.NewGuid()}@test.com",
            Phone = "7652074782",
            Department = "IT",
            Designation = "Senior Project Architect",
            JoinDate = DateTime.Today,
            Salary = 150000,
            Status = "Active",
            City = "Mumbai",
            State = "Maharashtra",
            Country = "India"
        };
    }
    [Fact]
    public async Task PostEmployee_ShouldInsertIntoDatabase()
    {

        var response = await _client.PostAsJsonAsync(
            "/api/Employees",
            _employee);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

          createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();

            createdEmployee.Should().NotBeNull();
            createdEmployee!.EmployeeId.Should().BeGreaterThan(0);
            createdEmployee.FirstName.Should().Be(_employee.FirstName);
            createdEmployee.LastName.Should().Be(_employee.LastName);
            createdEmployee.Email.Should().Be(_employee.Email);
    }

    [Fact]
public async Task DeleteEmployee_ShouldReturnOk()
{
    // Arrange    
if (createdEmployee == null)
    {
        // If the employee hasn't been created yet, create it first
        var postResponse = await _client.PostAsJsonAsync(
            "/api/Employees",
            _employee);

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        createdEmployee = await postResponse.Content.ReadFromJsonAsync<Employee>();
        createdEmployee.Should().NotBeNull();
    }   

    var deleteResponse = await _client.DeleteAsync($"api/Employees/{createdEmployee!.EmployeeId}");   

    // Assert
    deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
}

}