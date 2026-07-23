using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface IActSectionDtlsRepository
    {
        Task<IEnumerable<ActSectionDtls>> GetAllAsync();
        Task<ActSectionDtls?> GetByIdAsync(int sectionId);
        Task<ActSectionDtls> AddAsync(ActSectionDtls entity);
        Task UpdateAsync(ActSectionDtls entity);
        Task DeleteAsync(int sectionId);
        Task<bool> ExistsAsync(int sectionId);
    }
}
