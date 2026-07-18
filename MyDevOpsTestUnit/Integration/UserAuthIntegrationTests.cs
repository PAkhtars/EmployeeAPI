using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EmployeeAPI.Core.DTOs;
using FluentAssertions;
using Xunit;

namespace MyDevOpsTestUnit.Integration;

[Collection("IntegrationTests")]
public class UserAuthIntegrationTests: IClassFixture<AuthenticationFixture>
{
    private readonly HttpClient _client;
    
    private readonly IntegrationTestFixture _fixture;

    public UserAuthIntegrationTests(AuthenticationFixture auth, IntegrationTestFixture fixture)
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7105/")
        };        
        _fixture = fixture;
    }

    [Fact]
    public async Task Login_ShouldReturnOkAndToken_WhenCredentialsAreValid()
    {
        var registerRequest = CreateValidRegisterRequest();

        var registerResponse = await _client.PostAsJsonAsync("/api/Users/register", registerRequest);
        registerResponse.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);

        var loginRequest = new UserLoginDto
        {
            EmailOrUsername = registerRequest.Email,
            Password = registerRequest.Password
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Users/login", loginRequest);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        authResponse.Should().NotBeNull();
        authResponse!.Token.Should().NotBeNullOrWhiteSpace();
        authResponse.UserId.Should().NotBeEmpty();
        authResponse.Email.Should().Be(registerRequest.Email);
        authResponse.Username.Should().Be(registerRequest.Username);
        
    }

    [Fact]
    public async Task Profile_ShouldReturnOk_WhenAuthenticatedWithBearerToken()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _fixture.Auth.Token);

        var profileResponse = await _client.GetAsync("/api/Users/profile");

        profileResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var profile = await profileResponse.Content.ReadFromJsonAsync<UserDto>();
        profile.Should().NotBeNull();
        profile.UserId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        var loginRequest = new UserLoginDto
        {
            EmailOrUsername = "nonexistent@test.com",
            Password = "WrongPassword123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Users/login", loginRequest);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static UserRegisterDto CreateValidRegisterRequest()
    {
        var random = Guid.NewGuid().ToString("N")[..8];

        return new UserRegisterDto
        {
            Username = $"user_{random}",
            Email = $"user_{random}@test.com",
            Password = "Password@123",
            FullName = $"Test User {random}"
        };
    }
}
