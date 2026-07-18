using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class LegalCategoryMasterRepository : ILegalCategoryMasterRepository
    {
        private readonly AppDbContext _context;

        public LegalCategoryMasterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LegalCategoryMaster>> GetAllAsync()
        {
            return await _context.LegalCategoryMasters.AsNoTracking().OrderBy(x => x.CategoryName).ToListAsync();
        }

        public async Task<LegalCategoryMaster?> GetByIdAsync(int id)
        {
            return await _context.LegalCategoryMasters.FindAsync(id);
        }

        public async Task<LegalCategoryMaster> AddAsync(LegalCategoryMaster category)
        {
            await _context.LegalCategoryMasters.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(LegalCategoryMaster category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.LegalCategoryMasters.FindAsync(id);
            if (existing != null)
            {
                _context.LegalCategoryMasters.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LegalCategoryMasters.AnyAsync(x => x.Id == id);
        }
    }
}
