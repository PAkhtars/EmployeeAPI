using EmployeeAPI.Core.DTOs;
using EmployeeAPI.Core.Models;
using EmployeeAPI.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EmployeeAPI.Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly MailSettings _mailSettings;

        public EmailRepository(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailRequestDto request)
        {
            ValidateSettings();

            using var message = new MailMessage();
            message.From = new MailAddress(_mailSettings.FromEmail, _mailSettings.FromName);
            message.To.Add(new MailAddress(_mailSettings.ToEmail, _mailSettings.ToName));
            message.Subject = request.Subject;
            message.Body = ComposeBody(request);
            message.IsBodyHtml = false;

            if (!string.IsNullOrWhiteSpace(request.ClientEmail))
            {
                message.ReplyToList.Add(new MailAddress(request.ClientEmail, request.ClientName));
            }

            using var smtp = new SmtpClient(_mailSettings.SmtpHost, _mailSettings.SmtpPort)
            {
                EnableSsl = _mailSettings.EnableSsl
            };

            if (!string.IsNullOrWhiteSpace(_mailSettings.UserName))
            {
                smtp.Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password);
            }

            await smtp.SendMailAsync(message);
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(_mailSettings.SmtpHost))
                throw new InvalidOperationException("SMTP host is not configured.");

            if (string.IsNullOrWhiteSpace(_mailSettings.FromEmail))
                throw new InvalidOperationException("Mail sender address is not configured.");

            if (string.IsNullOrWhiteSpace(_mailSettings.ToEmail))
                throw new InvalidOperationException("Mail recipient address is not configured.");
        }

        private static string ComposeBody(EmailRequestDto request)
        {
            return $"Client Name: {request.ClientName}\n" +
                   $"Client Email: {request.ClientEmail}\n\n" +
                   "Message:\n" +
                   request.Message;
        }
    }
}
