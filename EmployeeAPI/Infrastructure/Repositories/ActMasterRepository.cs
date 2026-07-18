using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class ActMasterRepository : IActMasterRepository
    {
        private readonly AppDbContext _context;

        public ActMasterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActMaster>> GetAllAsync()
        {
            return await _context.ActMasters.AsNoTracking().ToListAsync();
        }

        public async Task<ActMaster?> GetByIdAsync(int id)
        {
            return await _context.ActMasters.FindAsync(id);
        }

        public async Task<ActMaster> AddAsync(ActMaster actMaster)
        {
            await _context.ActMasters.AddAsync(actMaster);
            await _context.SaveChangesAsync();
            return actMaster;
        }

        public async Task UpdateAsync(ActMaster actMaster)
        {
            _context.Entry(actMaster).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var actMaster = await _context.ActMasters.FindAsync(id);
            if (actMaster != null)
            {
                _context.ActMasters.Remove(actMaster);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActMasters.AnyAsync(a => a.ActId == id);
        }
    }
}
