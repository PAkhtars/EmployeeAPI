namespace EmployeeAPI.WebApi.Controllers
{
    using EmployeeAPI.Application.Services;    
    using EmployeeAPI.Core.DTOs;
    using EmployeeAPI.Core.Models;
    using Microsoft.AspNetCore.Authorization;
    // WebAPI/Controllers/UsersController.cs
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //var currentUserId = Guid.Parse(userIdClaim!);

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var result = await _userService.RegisterAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var response = await _userService.LoginAsync(dto);
                return Ok(response); // Contains Token + User info
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET: api/users/profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(sub) || !Guid.TryParse(sub, out var userId))
                return Unauthorized(); // or BadRequest("Invalid or missing user id claim");

            var profile = await _userService.GetUserProfileAsync(userId);
            return Ok(profile);
        }

        // PUT: api/users/profile
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
        {
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(sub) || !Guid.TryParse(sub, out var userId))
                return Unauthorized(); // or BadRequest("Invalid or missing user id claim");

            var updatedUser = await _userService.UpdateProfileAsync(userId, dto);
            return Ok(updatedUser);
        }

        // GET: api/users/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string? term, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _userService.SearchUsersAsync(term, page, pageSize);
            return Ok(result);
        }

        // POST: api/users/verify-email  (Optional)
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] string email)
        {
            var success = await _userService.VerifyEmailAsync(email);
            return success ? Ok(new { message = "Email verified" }) : BadRequest();
        }
    }
}
