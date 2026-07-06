using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    // Core/Interfaces/IVoteRepository.cs
    public interface IVoteRepository
    {
        Task<bool> HasUserVotedAsync(Guid userId, Guid pollId);
        Task<Vote?> GetByIdAsync(Guid voteId);
        Task AddAsync(Vote vote);
        Task<List<Vote>> GetVotesByPollAsync(Guid pollId);
        Task<List<Vote>> GetVotesByUserAsync(Guid userId);
    }
}
