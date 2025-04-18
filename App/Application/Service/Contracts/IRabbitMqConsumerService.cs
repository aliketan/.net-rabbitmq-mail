namespace App.Application.Service.Contracts
{
    using Domain.Models;

    public interface IRabbitMqConsumerService:IDisposable
    {
        Task Start(SmtpModel item, string queueName);
        void Stop();
    }
}
