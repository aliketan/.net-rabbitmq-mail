using Microsoft.Extensions.Options;
using RabbitMQ.Client.Exceptions;

namespace App.Application.Service.Concrete
{
    using Contracts;
    using RabbitMQ.Client;
    using Domain.Settings;

    public class RabbitMqService(IOptions<RabbitMqSettings> rabbitMqConfiguration) : IRabbitMqService
    {
        private int TotalFailedConnection { get; set; } = 0;
        private const int TotalAllowedConnection = 3;

        public IConnection GetConnection()
        {
            try
            {
                if (TotalFailedConnection > TotalAllowedConnection)
                    return null;

                return new ConnectionFactory
                {
                    HostName = rabbitMqConfiguration.Value.Host,
                    UserName = rabbitMqConfiguration.Value.UserName,
                    Password = rabbitMqConfiguration.Value.Password,
                    Port = rabbitMqConfiguration.Value.Port,
                    AutomaticRecoveryEnabled = rabbitMqConfiguration.Value.AutomaticRecovery, //otomatik bağlantı kurtarmayı etkinleştirmek için
                    NetworkRecoveryInterval =
                        TimeSpan.FromSeconds(rabbitMqConfiguration.Value.RecoveryInternalTime), //her 10 saniyede bir tekrar bağlantıyı toparlamayı dene
                    TopologyRecoveryEnabled = false //bağlantı kesildikten sonra kuyruktaki mesaj tüketimini sürdürmez.
                }.CreateConnectionAsync().Result;
            }
            catch (BrokerUnreachableException)
            {
                TotalFailedConnection++;
                //3 saniye bekletip tekrardan bağlantı kurmaya çalışmasını sağlıyoruz.
                Thread.Sleep(3000);
                return GetConnection();
            }
        }

        public async Task<IChannel> GetModel(IConnection connection) => await connection.CreateChannelAsync();
    }
}
