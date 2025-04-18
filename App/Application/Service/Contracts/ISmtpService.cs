namespace App.Application.Service.Contracts
{
    using Domain.Models;

    public interface ISmtpService
    {
        Task<bool> SendAsync(SmtpModel item);
        bool Send(SmtpModel item);
    }
}
