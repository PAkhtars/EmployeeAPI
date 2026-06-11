
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
// Infrastructure/Repositories/VoteRepository.cs
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _context;

        public VoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasUserVotedAsync(Guid userId, Guid pollId)
        {
            return await _context.Votes
                .AnyAsync(v => v.UserId == userId && v.PollId == pollId);
        }

        public async Task<Vote?> GetByIdAsync(Guid voteId)
        {
            return await _context.Votes.FindAsync(voteId);
        }

        public async Task AddAsync(Vote vote)
        {
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Vote>> GetVotesByPollAsync(Guid pollId)
        {
            return await _context.Votes
                .Include(v => v.Option)
                .Where(v => v.PollId == pollId)
                .ToListAsync();
        }

        public async Task<List<Vote>> GetVotesByUserAsync(Guid userId)
        {
            return await _context.Votes
                .Where(v => v.UserId == userId)
                .ToListAsync();
        }
    }
}
