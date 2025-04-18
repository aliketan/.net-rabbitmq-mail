using RabbitMQ.Client;

namespace App.Application.Service.Contracts
{
    public interface IRabbitMqService
    {
        IConnection GetConnection();
        Task<IChannel> GetModel(IConnection connection);
    }
}