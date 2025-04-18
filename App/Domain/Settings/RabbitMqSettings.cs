namespace App.Domain.Settings
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public int RecoveryInternalTime { get; set; }

        public bool AutomaticRecovery { get; set; }
    }
}
