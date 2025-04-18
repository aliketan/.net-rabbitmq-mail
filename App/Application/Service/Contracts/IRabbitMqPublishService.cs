namespace App.Application.Service.Contracts
{
    public interface IRabbitMqPublishService
    {
        Task Enqueue<T>(IEnumerable<T> queueDataModels, string queueName) where T : class, new();
        Task Enqueue<T>(T queueDataModel, string queueName) where T : class, new();
    }
}
