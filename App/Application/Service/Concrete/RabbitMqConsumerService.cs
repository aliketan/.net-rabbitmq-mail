using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace App.Application.Service.Concrete
{
    using Contracts;
    using Constants;
    using Domain.Models;

    public class RabbitMqConsumerService : IRabbitMqConsumerService
    {
        private SemaphoreSlim _semaphore;
        // eventler - olaylar
        public event EventHandler<bool> MessageProcessed;

        private AsyncEventingBasicConsumer _consumer;
        private IChannel _channel;
        private IConnection _connection;

        private readonly IRabbitMqService _rabbitMqService;
        private readonly ISmtpService _smtpService;
        private SmtpModel MailSenderOpts { get; set; }

        #region Constructor
        public RabbitMqConsumerService(
            IRabbitMqService rabbitMqService,
            ISmtpService smtpService
            )
        {
            _rabbitMqService = rabbitMqService;
            _smtpService = smtpService;
        }
        #endregion

        private async Task Consumer_Email_Received(object sender, BasicDeliverEventArgs ea)
        {
            try
            {
                await _semaphore.WaitAsync();
                // E-Posta akışını başlatma yeri
                try
                {
                    var result = await _smtpService.SendAsync(MailSenderOpts);
                    MessageProcessed?.Invoke(this, result);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    // Teslimat Onayı
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    // akışı - thread'i serbest bırakıyoruz ek thread alabiliriz.
                    _semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Start(SmtpModel item, string queueName)
        {
            try
            {
                MailSenderOpts = item;
                _semaphore = new SemaphoreSlim(RabbitMqConsts.ParallelThreadsCount);
                _connection = _rabbitMqService.GetConnection();
                _channel = await _rabbitMqService.GetModel(_connection);

                await _channel.QueueDeclareAsync(queueName, RabbitMqConsts.Options.Durable, RabbitMqConsts.Options.Exclusive, RabbitMqConsts.Options.AutoDelete, RabbitMqConsts.Options.Arguments);

                await _channel.BasicQosAsync(0, RabbitMqConsts.ParallelThreadsCount, false);
                _consumer = new AsyncEventingBasicConsumer(_channel);
                _consumer.ReceivedAsync += Consumer_Email_Received;
                await Task.FromResult(_channel.BasicConsumeAsync(queue: queueName,
                    autoAck: false,
                    /* autoAck: bir mesajı aldıktan sonra bunu anladığına       
                       dair(acknowledgment) kuyruğa bildirimde bulunur ya da timeout gibi vakalar oluştuğunda 
                       mesajı geri çevirmek(Discard) veya yeniden kuyruğa aldırmak(Re-Queue) için dönüşler yapar */
                    consumer: _consumer
                    )
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            _channel.Dispose();
            //_connection.Dispose();
            _semaphore.Dispose();
        }
    }
}
