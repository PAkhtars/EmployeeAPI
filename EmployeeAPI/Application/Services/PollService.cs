using EmployeeAPI.Core.DTOs;
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;

namespace EmployeeAPI.Application.Services
{
    // Application/Services/PollService.cs
    public class PollService
    {
        private readonly IPollRepository _pollRepository;
        private readonly IUserRepository _userRepository;

        public PollService(IPollRepository pollRepository, IUserRepository userRepository)
        {
            _pollRepository = pollRepository;
            _userRepository = userRepository;
        }

        // ==================== CREATE POLL ====================
        public async Task<PollDto> CreatePollAsync(Guid creatorId, CreatePollDto dto)
        {
            // Validate creator exists
            var creator = await _userRepository.GetByIdAsync(creatorId);
            if (creator == null)
                throw new KeyNotFoundException("User not found");

            var poll = new Poll
            {
                CreatorId = creatorId,
                Question = dto.Question,
                Description = dto.Description,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                ExpiresAt = dto.ExpiresAt,
                IsPublic = dto.IsPublic,
                AllowMultipleVotes = dto.AllowMultipleVotes,
                MinAgeRestriction = dto.MinAgeRestriction,
                Status = "active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add Options
            int order = 1;
            foreach (var opt in dto.Options)
            {
                poll.Options.Add(new PollOption
                {
                    OptionText = opt.OptionText,
                    ImageUrl = opt.ImageUrl,
                    DisplayOrder = order++
                });
            }

            if (poll.Options.Count < 2)
                throw new InvalidOperationException("Poll must have at least 2 options");

            await _pollRepository.AddAsync(poll);

            return MapToDto(poll);
        }

        // ==================== GET BY ID ====================
        public async Task<PollDto?> GetPollByIdAsync(Guid pollId)
        {
            var poll = await _pollRepository.GetByIdAsync(pollId);
            return poll == null ? null : MapToDto(poll);
        }

        // ==================== GET PAGED / FEED ====================
        public async Task<(IEnumerable<PollDto> Polls, int TotalCount)> GetPollsPagedAsync(
            int page = 1, int pageSize = 20, string? category = null, string? searchTerm = null)
        {
            var (polls, total) = await _pollRepository.GetPollsPagedAsync(page, pageSize, category, searchTerm);
            return (polls.Select(MapToDto), total);
        }

        // ==================== TRENDING ====================
        public async Task<IEnumerable<PollDto>> GetTrendingPollsAsync(int count = 20)
        {
            var polls = await _pollRepository.GetTrendingPollsAsync(count);
            return polls.Select(MapToDto);
        }

        // ==================== USER'S POLLS ====================
        public async Task<IEnumerable<PollDto>> GetPollsByUserAsync(Guid userId)
        {
            var polls = await _pollRepository.GetPollsByUserAsync(userId);
            return polls.Select(MapToDto);
        }

        // ==================== INCREMENT VOTES (Called from VoteService) ====================
        public async Task IncrementVoteCountAsync(Guid pollId)
        {
            await _pollRepository.IncrementTotalVotesAsync(pollId);
        }

        // ==================== MAPPING HELPER ====================
        private PollDto MapToDto(Poll poll)
        {
            return new PollDto
            {
                PollId = poll.PollId,
                Question = poll.Question,
                Description = poll.Description,
                Category = poll.Category,
                ImageUrl = poll.ImageUrl,
                ExpiresAt = poll.ExpiresAt,
                IsPublic = poll.IsPublic,
                TotalVotes = poll.TotalVotes,
                Status = poll.Status,
                CreatedAt = poll.CreatedAt,
                CreatorId = poll.CreatorId,
                CreatorName = poll.Creator?.Username ?? "Unknown",
                CreatorAvatar = poll.Creator?.ProfileImageUrl,

                Options = poll.Options
                    .OrderBy(o => o.DisplayOrder)
                    .Select(o => new PollOptionDto
                    {
                        OptionId = o.OptionId,
                        OptionText = o.OptionText,
                        ImageUrl = o.ImageUrl,
                        VoteCount = o.VoteCount
                    }).ToList()
            };
        }
    }
}
