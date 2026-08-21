using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);

        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);

        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);

        Task<IEnumerable<User>> GetActiveUsersAsync(int count = 50);
        Task UpdateLastActiveAsync(Guid userId);

        // For pagination in admin/search
        Task<(IEnumerable<User> Users, int TotalCount)> GetUsersPagedAsync(int page, int pageSize, string? searchTerm = null);
    }
}
