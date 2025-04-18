using System.Text;
using RabbitMQ.Client;

namespace App.Application.Service.Concrete
{
    using Contracts;
    using System.Collections.Generic;
    using Constants;
    using Common.Utility.Extensions;
    using Persistence.Contexts;

    public class RabbitMqPublishService(IRabbitMqService rabbitMqService) : IRabbitMqPublishService
    {
        private static BasicProperties GetProperties()
        {
            return new BasicProperties
            {
                Persistent = true,
                Expiration = RabbitMqConsts.MessagesTTL.ToString()
            };
        }

        public async Task Enqueue<T>(IEnumerable<T> queueDataModels, string queueName) where T : class, new()
        {
            try
            {
                await using var connection = rabbitMqService.GetConnection();
                await using var channel = await rabbitMqService.GetModel(connection);
                await channel.QueueDeclareAsync(queueName, RabbitMqConsts.Options.Durable, RabbitMqConsts.Options.Exclusive, RabbitMqConsts.Options.AutoDelete, RabbitMqConsts.Options.Arguments);

                foreach (var queueDataModel in queueDataModels)
                {
                    var body = Encoding.UTF8.GetBytes(queueDataModel.Serialize(AppJsonSerializerContext.Default.SmtpModel));
                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: queueName,
                        basicProperties: GetProperties(),
                        body: body,
                        mandatory: true
                    );
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Enqueue<T>(T queueDataModel, string queueName) where T : class, new()
        {
            try
            {
                await using var connection = rabbitMqService.GetConnection();
                await using var channel = await rabbitMqService.GetModel(connection);
                await channel.QueueDeclareAsync(queueName, RabbitMqConsts.Options.Durable, RabbitMqConsts.Options.Exclusive, RabbitMqConsts.Options.AutoDelete, RabbitMqConsts.Options.Arguments);

                var body = Encoding.UTF8.GetBytes(queueDataModel.Serialize(AppJsonSerializerContext.Default.SmtpModel));
                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: GetProperties(),
                    body: body,
                    mandatory: true
                );

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}