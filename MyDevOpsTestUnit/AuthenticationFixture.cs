using System.Net.Http.Json;
using EmployeeAPI.Core.DTOs;

namespace MyDevOpsTestUnit
{
    public  class AuthenticationFixture
    {
        public   string Token { get; private set; } = string.Empty;
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
        public async Task InitializeAsync(HttpClient client)
        {
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

                if(authResponse!=null)
                {
                    Token = authResponse.Token??string.Empty;   
                }
                
        }
    }
}