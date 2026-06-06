using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    // Core/Interfaces/IPollRepository.cs
    public interface IPollRepository
    {
        Task<Poll?> GetByIdAsync(Guid pollId);
        Task<IEnumerable<Poll>> GetActivePollsAsync(int count = 50);
        Task<IEnumerable<Poll>> GetPollsByUserAsync(Guid userId);
        Task<IEnumerable<Poll>> GetTrendingPollsAsync(int count = 20);

        Task AddAsync(Poll poll);
        Task UpdateAsync(Poll poll);
        Task DeleteAsync(Guid pollId);

        // Feed & Search
        Task<(IEnumerable<Poll> Polls, int TotalCount)> GetPollsPagedAsync(
            int page, int pageSize, string? category = null, string? searchTerm = null);

        Task IncrementTotalVotesAsync(Guid pollId);
    }
}
