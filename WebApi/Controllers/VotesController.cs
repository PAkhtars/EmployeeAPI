using EmployeeAPI.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeAPI.WebApi.Controllers
{
    // WebAPI/Controllers/VotesController.cs
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly VoteService _voteService;

        public VotesController(VoteService voteService)
        {
            _voteService = voteService;
        }

        // POST: api/votes
        [HttpPost]
        public async Task<IActionResult> CastVote([FromBody] CastVoteDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                await _voteService.CastVoteAsync(userId, dto.PollId, dto.OptionId);
                return Ok(new { message = "Vote cast successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTO
    public class CastVoteDto
    {
        public Guid PollId { get; set; }
        public Guid OptionId { get; set; }
    }
}
