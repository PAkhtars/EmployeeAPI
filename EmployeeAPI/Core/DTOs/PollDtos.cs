namespace EmployeeAPI.Core.DTOs
{
    // Core/DTOs/PollDtos.cs

    public class CreatePollDto
    {
        public string Question { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsPublic { get; set; } = true;
        public bool AllowMultipleVotes { get; set; } = false;
        public int MinAgeRestriction { get; set; } = 0;
        public List<PollOptionCreateDto> Options { get; set; } = new();
    }

    public class PollOptionCreateDto
    {
        public string OptionText { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    public class PollDto
    {
        public Guid PollId { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public string? CreatorAvatar { get; set; }

        public string Question { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime? ExpiresAt { get; set; }
        public bool IsPublic { get; set; }
        public long TotalVotes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<PollOptionDto> Options { get; set; } = new();
    }

    public class PollOptionDto
    {
        public Guid OptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public long VoteCount { get; set; }
    }
}
