using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface IVoterRepository
    {
        Task<IEnumerable<Voter>> GetAllAsync();
        Task<Voter?> GetByIdAsync(int id);
        Task<Voter> AddAsync(Voter voter);
        Task UpdateAsync(Voter voter);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
