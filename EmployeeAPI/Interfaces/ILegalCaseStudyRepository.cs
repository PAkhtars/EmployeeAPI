using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface ILegalCaseStudyRepository
    {
        Task<IEnumerable<LegalCaseStudy>> GetAllAsync();
        Task<LegalCaseStudy?> GetByIdAsync(int id);
        Task<LegalCaseStudy> AddAsync(LegalCaseStudy legalCaseStudy);
        Task UpdateAsync(LegalCaseStudy legalCaseStudy);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
