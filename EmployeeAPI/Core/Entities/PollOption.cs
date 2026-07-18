namespace EmployeeAPI.Core.Entities
{
    // Core/Entities/PollOption.cs
    public class PollOption
    {
        public Guid OptionId { get; set; } = Guid.NewGuid();
        public Guid PollId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public long VoteCount { get; set; } = 0;
        public int DisplayOrder { get; set; }

        public Poll Poll { get; set; } = null!;
    }
}
