using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface IActDetailsRepository
    {
        Task<IEnumerable<ActDetails>> GetAllAsync();
        Task<ActDetails?> GetByIdAsync(int id);
        Task<ActDetails> AddAsync(ActDetails actDetails);
        Task UpdateAsync(ActDetails actDetails);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
