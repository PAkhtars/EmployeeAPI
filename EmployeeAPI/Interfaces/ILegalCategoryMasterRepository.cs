using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface ILegalCategoryMasterRepository
    {
        Task<IEnumerable<LegalCategoryMaster>> GetAllAsync();
        Task<LegalCategoryMaster?> GetByIdAsync(int id);
        Task<LegalCategoryMaster> AddAsync(LegalCategoryMaster category);
        Task UpdateAsync(LegalCategoryMaster category);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
