using System;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using EmployeeAPI.Core.DTOs;
using Xunit;

namespace MyDevOpsTestUnit;

public class RegisterIntegrationTests
{
    private readonly HttpClient _client;
    private RegisterRequest _request;
    private RegisterRequest _createdUser;
    public RegisterIntegrationTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7105/")
        };
        
    }

    public class RegisterRequest : UserRegisterDto
    {
    }

    private static RegisterRequest CreateValidRequest()
    {
        var random = Guid.NewGuid().ToString("N")[..8];

        return new RegisterRequest
        {
            Username = $"user_{random}",
            Email = $"user_{random}@test.com",
            Password = "Password@123",
            FullName = $"Test User {random}"
        };
    }

    [Fact]
    public async Task Register_ShouldReturnCreated_WhenPayloadIsValid()
    {
        _request = CreateValidRequest(); //   var request = CreateValidRequest();

        var response = await _client.PostAsJsonAsync("/api/Users/register", _request);

        _createdUser = await response.Content.ReadFromJsonAsync<RegisterRequest>();
        if(_createdUser == null)
        {
            throw new Exception("Failed to deserialize the response content.");
        }
        _createdUser.Should().NotBeNull();
        _createdUser!.Username.Should().Be(_request.Username);
        _createdUser.Email.Should().Be(_request.Email);
        _createdUser.FullName.Should().Be(_request.FullName);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenEmailIsInvalid()
    {
        if(_request == null)
        {
            _request = CreateValidRequest();
        }
        _request.Email = "invalid-email";

        var response = await _client.PostAsJsonAsync("/api/Users/register", _request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShouldReturnConflict_WhenUsernameAlreadyExists()
    {

        if(_createdUser == null)
        {
            _request = CreateValidRequest();
            
           var response = await _client.PostAsJsonAsync("/api/Users/register", _request);

            _createdUser = await response.Content.ReadFromJsonAsync<RegisterRequest>();
            if(_createdUser == null)
            {

                throw new Exception("Failed to deserialize the response content.");
            }

        }


        var duplicate = new RegisterRequest
        {
            Username = _createdUser.Username,
            Email = $"new_{Guid.NewGuid():N}@test.com",
            Password = "Password@123",
            FullName = "Duplicate Username"
        };

        var dupResponse = await _client.PostAsJsonAsync("/api/Users/register", duplicate);

        dupResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Register_ShouldReturnConflict_WhenEmailAlreadyExists()
    {
         if(_createdUser == null)
        {
            _request = CreateValidRequest();
            
           var response = await _client.PostAsJsonAsync("/api/Users/register", _request);

            _createdUser = await response.Content.ReadFromJsonAsync<RegisterRequest>();
            if(_createdUser == null)
            {

                throw new Exception("Failed to deserialize the response content.");
            }

        }
        
        var duplicate = new RegisterRequest
        {
            Username = $"user_{Guid.NewGuid():N}",
            Email = _createdUser.Email,
            Password = "Password@123",
            FullName = "Duplicate Email"
        };

        var dupResponse = await _client.PostAsJsonAsync("/api/Users/register", duplicate);

        dupResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenRequiredFieldsMissing()
    {
        var request = new RegisterRequest
        {
            Username = "",
            Email = "",
            Password = "",
            FullName = ""
        };

        var response = await _client.PostAsJsonAsync("/api/Users/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenPayloadIsEmpty()
    {
        var response = await _client.PostAsJsonAsync("/api/Users/register", new { });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShouldAllowMultipleUniqueUsers()
    {
        for (int i = 0; i < 10; i++)
        {
            var request = CreateValidRequest();

            var response = await _client.PostAsJsonAsync("/api/Users/register", request);

            response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);
        }
    }
}
