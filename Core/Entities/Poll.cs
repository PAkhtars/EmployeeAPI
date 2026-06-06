namespace EmployeeAPI.Core.Entities
{
    // Core/Entities/Poll.cs
    public class Poll
    {
        public Guid PollId { get; set; } = Guid.NewGuid();
        public Guid CreatorId { get; set; }

        public string Question { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime? ExpiresAt { get; set; }
        public bool IsPublic { get; set; } = true;
        public bool AllowMultipleVotes { get; set; } = false;
        public int MinAgeRestriction { get; set; } = 0;

        public long TotalVotes { get; set; } = 0;
        public string Status { get; set; } = "active"; // active, expired, closed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User Creator { get; set; } = null!;
        public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
    }
}
