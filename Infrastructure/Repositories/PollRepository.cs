
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
// Infrastructure/Repositories/PollRepository.cs
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
   

    public class PollRepository : IPollRepository
    {
        private readonly AppDbContext _context;

        public PollRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Poll?> GetByIdAsync(Guid pollId)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(p => p.PollId == pollId);
        }

        public async Task<IEnumerable<Poll>> GetActivePollsAsync(int count = 50)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .Include(p => p.Creator)
                .Where(p => p.Status == "active" && p.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Poll>> GetPollsByUserAsync(Guid userId)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .Where(p => p.CreatorId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Poll>> GetTrendingPollsAsync(int count = 20)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .Where(p => p.Status == "active")
                .OrderByDescending(p => p.TotalVotes)
                .Take(count)
                .ToListAsync();
        }

        public async Task AddAsync(Poll poll)
        {
            await _context.Polls.AddAsync(poll);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Poll poll)
        {
            _context.Polls.Update(poll);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid pollId)
        {
            var poll = await _context.Polls.FindAsync(pollId);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<Poll> Polls, int TotalCount)> GetPollsPagedAsync(
            int page, int pageSize, string? category = null, string? searchTerm = null)
        {
            var query = _context.Polls
                .Include(p => p.Options)
                .Include(p => p.Creator)
                .Where(p => p.Status == "active" && p.IsPublic);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category == category);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.Question.Contains(searchTerm));

            var totalCount = await query.CountAsync();

            var polls = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (polls, totalCount);
        }

        public async Task IncrementTotalVotesAsync(Guid pollId)
        {
            var poll = await _context.Polls.FindAsync(pollId);
            if (poll != null)
            {
                poll.TotalVotes++;
                poll.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
