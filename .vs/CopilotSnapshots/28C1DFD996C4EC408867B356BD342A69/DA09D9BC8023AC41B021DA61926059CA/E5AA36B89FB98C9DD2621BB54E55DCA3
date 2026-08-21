using EmployeeAPI.Core.DTOs;
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Org.BouncyCastle.Crypto.Generators;

namespace EmployeeAPI.Application.Services
{
    // Application/Services/UserService.cs
    // Application/Services/UserService.cs
    // Application/Services/UserService.cs
    using BCrypt.Net;


    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        // ==================== REGISTER ====================
        public async Task<UserDto> RegisterAsync(UserRegisterDto dto)
        {
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException("Email already exists");

            if (await _userRepository.ExistsByUsernameAsync(dto.Username))
                throw new InvalidOperationException("Username already taken");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = BCrypt.EnhancedHashPassword(dto.Password, 13),
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return MapToDto(user);
        }

        // ==================== LOGIN ====================
        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.EmailOrUsername)
                    ?? await _userRepository.GetByUsernameAsync(dto.EmailOrUsername);

            if (user == null || !BCrypt.EnhancedVerify(dto.Password, user.PasswordHash!))
                throw new UnauthorizedAccessException("Invalid email or password");

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        // ==================== GET BY ID ====================
        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : MapToDto(user);
        }

        // ==================== GET PROFILE ====================
        public async Task<UserDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return MapToDto(user);
        }

        // ==================== UPDATE PROFILE ====================
        public async Task<UserDto> UpdateProfileAsync(Guid userId, UserUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.FullName = dto.FullName ?? user.FullName;
            user.Bio = dto.Bio ?? user.Bio;
            user.ProfileImageUrl = dto.ProfileImageUrl ?? user.ProfileImageUrl;
            user.DateOfBirth = dto.DateOfBirth ?? user.DateOfBirth;
            user.LastActiveAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        // ==================== SEARCH USERS (New) ====================
        public async Task<(IEnumerable<UserDto> Users, int TotalCount)> SearchUsersAsync(
            string? searchTerm, int page = 1, int pageSize = 20)
        {
            var (users, totalCount) = await _userRepository.GetUsersPagedAsync(page, pageSize, searchTerm);

            var userDtos = users.Select(MapToDto);
            return (userDtos, totalCount);
        }

        // ==================== VERIFY EMAIL (New) ====================
        public async Task<bool> VerifyEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return false;

            user.IsVerified = true;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        // ==================== HELPER ====================
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                ProfileImageUrl = user.ProfileImageUrl,
                Bio = user.Bio,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
