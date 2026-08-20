using EmployeeAPI.Core.DTOs;
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class VoterRepository : IVoterRepository
    {
        private readonly AppDbContext _context;

        public VoterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Voter>> GetAllAsync()
        {
            return await _context.Voters.AsNoTracking().OrderBy(v => v.SerialNo).ToListAsync();
        }

        public async Task<IEnumerable<VoterDetailsDto>> GetVotersAsync(int areaNumber, int partNumber)
        {
            return await _context.VoterDetails
                .FromSqlInterpolated($"EXEC Get_VoterDetails @AreaNumber={areaNumber}, @PartNumber={partNumber}")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Voter?> GetByIdAsync(int id)
        {
            return await _context.Voters.FindAsync(id);
        }

        public async Task<Voter> AddAsync(Voter voter)
        {
            await _context.Voters.AddAsync(voter);
            await _context.SaveChangesAsync();
            return voter;
        }

        public async Task<List<Voter>> AddRangeAsync(List<Voter> voters)
        {
            if (voters == null || voters.Count == 0)
            {
                return new List<Voter>();
            }

            await _context.Voters.AddRangeAsync(voters);
            await _context.SaveChangesAsync();
            return voters;
        }

        public async Task UpdateAsync(Voter voter)
        {
            _context.Entry(voter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Voters.FindAsync(id);
            if (existing != null)
            {
                _context.Voters.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Voters.AnyAsync(v => v.Id == id);
        }
    }
}
