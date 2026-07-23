using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class LegalCaseStudyRepository : ILegalCaseStudyRepository
    {
        private readonly AppDbContext _context;

        public LegalCaseStudyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LegalCaseStudy>> GetAllAsync()
        {
            return await _context.LegalCaseStudies.AsNoTracking().OrderBy(x => x.CaseTitleAndCitation).ToListAsync();
        }

        public async Task<LegalCaseStudy?> GetByIdAsync(int id)
        {
            return await _context.LegalCaseStudies.FindAsync(id);
        }

        public async Task<LegalCaseStudy> AddAsync(LegalCaseStudy legalCaseStudy)
        {
            await _context.LegalCaseStudies.AddAsync(legalCaseStudy);
            await _context.SaveChangesAsync();
            return legalCaseStudy;
        }

        public async Task UpdateAsync(LegalCaseStudy legalCaseStudy)
        {
            _context.Entry(legalCaseStudy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.LegalCaseStudies.FindAsync(id);
            if (existing != null)
            {
                _context.LegalCaseStudies.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LegalCaseStudies.AnyAsync(x => x.Id == id);
        }
    }
}
