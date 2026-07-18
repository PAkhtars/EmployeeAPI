using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface IActMasterRepository
    {
        Task<IEnumerable<ActMaster>> GetAllAsync();
        Task<ActMaster?> GetByIdAsync(int id);
        Task<ActMaster> AddAsync(ActMaster actMaster);
        Task UpdateAsync(ActMaster actMaster);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
