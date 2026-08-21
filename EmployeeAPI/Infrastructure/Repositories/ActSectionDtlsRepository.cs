using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class ActSectionDtlsRepository : IActSectionDtlsRepository
    {
        private readonly AppDbContext _context;

        public ActSectionDtlsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActSectionDtls>> GetAllAsync()
        {
            return await _context.ActSectionDtls.AsNoTracking().ToListAsync();
        }

        public async Task<ActSectionDtls?> GetByIdAsync(int sectionId)
        {
            return await _context.ActSectionDtls.FindAsync(sectionId);
        }

        public async Task<ActSectionDtls> AddAsync(ActSectionDtls entity)
        {
            await _context.ActSectionDtls.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ActSectionDtls entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int sectionId)
        {
            var entity = await _context.ActSectionDtls.FindAsync(sectionId);
            if (entity != null)
            {
                _context.ActSectionDtls.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int sectionId)
        {
            return await _context.ActSectionDtls.AnyAsync(a => a.SectionId == sectionId);
        }
    }
}
