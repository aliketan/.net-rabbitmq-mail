using System.Net.Mail;
using System.Text.Json.Serialization;

namespace App.Domain.Models
{
    public class SmtpModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        [JsonIgnore]
        public Attachment[] Attachments { get; set; }
    }
}
