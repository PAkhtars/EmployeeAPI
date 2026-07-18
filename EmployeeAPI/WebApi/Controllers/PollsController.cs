using EmployeeAPI.Application.Services;
using EmployeeAPI.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeAPI.WebApi.Controllers
{
    // WebAPI/Controllers/PollsController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly PollService _pollService;   // We'll create this next if needed

        public PollsController(PollService pollService)
        {
            _pollService = pollService;
        }

        // GET: api/polls
        [HttpGet]
        public async Task<IActionResult> GetPolls(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? category = null,
            [FromQuery] string? search = null)
        {
            var result = await _pollService.GetPollsPagedAsync(page, pageSize, category, search);
            return Ok(result);
        }

        // GET: api/polls/trending
        [HttpGet("trending")]
        public async Task<IActionResult> GetTrending()
        {
            var polls = await _pollService.GetTrendingPollsAsync(20);
            return Ok(polls);
        }

        // GET: api/polls/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoll(Guid id)
        {
            var poll = await _pollService.GetPollByIdAsync(id);
            return poll == null ? NotFound() : Ok(poll);
        }

        // POST: api/polls
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePoll([FromBody] CreatePollDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var poll = await _pollService.CreatePollAsync(userId, dto);
            return CreatedAtAction(nameof(GetPoll), new { id = poll.PollId }, poll);
        }

        // GET: api/polls/my
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyPolls()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var polls = await _pollService.GetPollsByUserAsync(userId);
            return Ok(polls);
        }
    }
}
