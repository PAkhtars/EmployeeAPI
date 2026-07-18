using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface IComplaintMasterRepository
    {
        Task<IEnumerable<ComplaintMaster>> GetAllAsync();
        Task<ComplaintMaster?> GetByIdAsync(int id);
        Task<ComplaintMaster> AddAsync(ComplaintMaster complaintMaster);
        Task UpdateAsync(ComplaintMaster complaintMaster);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
