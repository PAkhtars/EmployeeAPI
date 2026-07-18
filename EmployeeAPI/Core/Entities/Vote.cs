namespace EmployeeAPI.Core.Entities
{
    // Core/Entities/Vote.cs
    public class Vote
    {
        public Guid VoteId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid PollId { get; set; }
        public Guid OptionId { get; set; }
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;
        public Poll Poll { get; set; } = null!;
        public PollOption Option { get; set; } = null!;
    }
}
