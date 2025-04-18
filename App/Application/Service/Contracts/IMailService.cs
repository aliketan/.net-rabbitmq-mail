namespace App.Application.Service.Contracts
{
    using Common.Utility.Results.Contracts;
    using Domain.Dto;

    public interface IMailService
    {
        Task<IDialogResult> SendWelcomeMessage(WelcomeMessageDto item);
    }
}
