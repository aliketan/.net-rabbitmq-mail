namespace App.Persistence.Endpoints
{
    using Application.Service.Contracts;
    using Domain.Dto;

    public static class MailSendEndpoints
    {
        public static WebApplication MapMailSendEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/mail");

            group.MapPost("/send", async (WelcomeMessageDto item, IMailService mailService) => await mailService.SendWelcomeMessage(item))
                .WithName("SendWelcomeMail")
                .WithDescription("Send a welcome mail to user with RabbitMQ");

            return app;
        }
    }
}
