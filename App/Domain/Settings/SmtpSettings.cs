namespace App.Domain.Settings
{
    public class SmtpSettings
    {
        public string Server { get; set; }

        public string SenderName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Ssl { get; set; }

        public int Port { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool DeliveryMethod { get; set; }

        public string DeliveryMethodType { get; set; }
    }
}
