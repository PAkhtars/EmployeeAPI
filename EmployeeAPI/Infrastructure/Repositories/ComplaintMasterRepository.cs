using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class ComplaintMasterRepository : IComplaintMasterRepository
    {
        private readonly AppDbContext _context;

        public ComplaintMasterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplaintMaster>> GetAllAsync()
        {
            return await _context.ComplaintMasters.AsNoTracking().ToListAsync();
        }

        public async Task<ComplaintMaster?> GetByIdAsync(int id)
        {
            return await _context.ComplaintMasters.FindAsync(id);
        }

        public async Task<ComplaintMaster> AddAsync(ComplaintMaster complaintMaster)
        {
            await _context.ComplaintMasters.AddAsync(complaintMaster);
            await _context.SaveChangesAsync();
            return complaintMaster;
        }

        public async Task UpdateAsync(ComplaintMaster complaintMaster)
        {
            _context.Entry(complaintMaster).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var complaintMaster = await _context.ComplaintMasters.FindAsync(id);
            if (complaintMaster != null)
            {
                _context.ComplaintMasters.Remove(complaintMaster);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ComplaintMasters.AnyAsync(c => c.ComplaintId == id);
        }
    }
}
