using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class ActDetailsRepository : IActDetailsRepository
    {
        private readonly AppDbContext _context;

        public ActDetailsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActDetails>> GetAllAsync()
        {
            return await _context.ActDetails.AsNoTracking().ToListAsync();
        }

        public async Task<ActDetails?> GetByIdAsync(int id)
        {
            return await _context.ActDetails.FindAsync(id);
        }

        public async Task<ActDetails> AddAsync(ActDetails actDetails)
        {
            await _context.ActDetails.AddAsync(actDetails);
            await _context.SaveChangesAsync();
            return actDetails;
        }

        public async Task UpdateAsync(ActDetails actDetails)
        {
            _context.Entry(actDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var actDetails = await _context.ActDetails.FindAsync(id);
            if (actDetails != null)
            {
                _context.ActDetails.Remove(actDetails);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActDetails.AnyAsync(a => a.Id == id);
        }
    }
}
