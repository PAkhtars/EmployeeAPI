using EmployeeAPI.Core.DTOs;

namespace EmployeeAPI.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(EmailRequestDto request);
    }
}
