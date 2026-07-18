
using System.Net.Http.Json;
using EmployeeAPI.Core.DTOs;

namespace MyDevOpsTestUnit
{
    public sealed class AuthenticationTokenProvider
    {
        private static readonly Lazy<AuthenticationTokenProvider> _instance =
            new(() => new AuthenticationTokenProvider());

        public static AuthenticationTokenProvider Instance => _instance.Value;

        private AuthenticationTokenProvider()
        {
        }

        private readonly SemaphoreSlim _lock = new(1, 1);

        public string? Token { get; private set; }
        public Guid UserId { get; private set; }
        public string? Email { get; private set; }
        public string? Username { get; private set; }

        public async Task<string> GetTokenAsync(HttpClient client)
        {
            if (!string.IsNullOrWhiteSpace(Token))
                return Token;

            await _lock.WaitAsync();

            try
            {
                if (!string.IsNullOrWhiteSpace(Token))
                    return Token;

                var registerRequest = CreateValidRegisterRequest();

                var registerResponse = await client.PostAsJsonAsync(
                    "/api/Users/register",
                    registerRequest);

                registerResponse.EnsureSuccessStatusCode();

                var loginRequest = new UserLoginDto
                {
                    EmailOrUsername = registerRequest.Email,
                    Password = registerRequest.Password
                };

                var loginResponse = await client.PostAsJsonAsync(
                    "/api/Users/login",
                    loginRequest);

                loginResponse.EnsureSuccessStatusCode();

                var authResponse =
                    await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

                Token = authResponse!.Token;
                UserId = authResponse.UserId;
                Email = authResponse.Email;
                Username = authResponse.Username;

                return Token!;
            }
            finally
            {
                _lock.Release();
            }
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
}