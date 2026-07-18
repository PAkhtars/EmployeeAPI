using System.Net.Http.Headers;
using System.Net.Http.Json;
using EmployeeAPI.Core.DTOs;

namespace MyDevOpsTestUnit
{   public class IntegrationTestFixture : IAsyncLifetime
    {
        public HttpClient Client { get; }

        public AuthenticationContext Auth { get; } = new();

        public IntegrationTestFixture()
        {
            Client = new HttpClient
                    {
                        BaseAddress = new Uri("https://localhost:7105/")
                    };
        }

        public async Task InitializeAsync()
        {
            var registerRequest = CreateValidRegisterRequest();

            await Client.PostAsJsonAsync(
                "/api/Users/register",
                registerRequest);

            var loginResponse = await Client.PostAsJsonAsync(
                "/api/Users/login",
                new UserLoginDto
                {
                    EmailOrUsername = registerRequest.Email,
                    Password = registerRequest.Password
                });

            var auth =
                await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

            Auth.Token = auth!.Token;
            Auth.UserId = auth.UserId;
            Auth.Email = auth.Email;
            Auth.Username = auth.Username;

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Auth.Token);
        }

        public Task DisposeAsync() => Task.CompletedTask;

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