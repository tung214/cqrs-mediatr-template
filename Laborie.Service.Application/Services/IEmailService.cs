namespace Laborie.Service.Application.Services
{
    public interface IEmailService
    {
        Task SendMail(string to, string subject, string body);
    }
}