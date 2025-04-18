namespace App.Application.Service.Concrete
{
    using Contracts;
    using Common.Enums.ComplexTypes;
    using App.Common.Utility.Results.Concrete;
    using App.Common.Utility.Results.Contracts;
    using Domain.Dto;
    using Domain.Models;

    public class MailService : IMailService
    {
        private readonly IRabbitMqPublishService _rabbitMqPublishService;
        private readonly IRabbitMqConsumerService _rabbitMqConsumerService;
        private readonly ITemplateEngine _templateEngine;
        private const string TemplateName = "WelcomeMessageTemplate.txt";
        private readonly string _welcomeMessageTemplatePath;

        #region Constructor
        public MailService(
            IRabbitMqPublishService rabbitMqPublishService,
            IRabbitMqConsumerService rabbitMqConsumerService,
            ITemplateEngine templateEngine,
            IWebHostEnvironment environment)
        {
            _rabbitMqPublishService = rabbitMqPublishService;
            _rabbitMqConsumerService = rabbitMqConsumerService;
            _templateEngine = templateEngine;
            _welcomeMessageTemplatePath = Path.Combine(environment.WebRootPath, "templates", TemplateName);
        }
        #endregion

        private async Task<string> ReadTemplate()
        {
            if (!File.Exists(_welcomeMessageTemplatePath))
                throw new FileNotFoundException($"Template file not found: {TemplateName}");

            return await File.ReadAllTextAsync(_welcomeMessageTemplatePath);
        }

        public async Task<IDialogResult> SendWelcomeMessage(WelcomeMessageDto item)
        {
            string output = await _templateEngine.ParseAsync(await ReadTemplate(), item);
            var model = new SmtpModel
            {
                To = item.Email,
                Body = output,
                Subject = "Welcome!"
            };

            string queueName = $"Welcome mail to: {item.Email}";
            _ =  _rabbitMqPublishService.Enqueue(model, queueName);
            await _rabbitMqConsumerService.Start(model, queueName);

            return new DialogResult(ResultStatus.Success);
        }
    }
}
