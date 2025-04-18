using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace App.Application.Service.Concrete
{
    using Contracts;
    using Common.Enums.ComplexTypes;
    using Common.Utility.Extensions;
    using Domain.Models;
    using Domain.Settings;

    public class SmtpService : ISmtpService
    {
        private readonly IOptions<SmtpSettings> _smtpOptions;

        #region Constructor
        public SmtpService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpOptions = smtpOptions;
        }
        #endregion

        private bool Submit(SmtpModel item)
        {
            try
            {
                MailMessage message = new();
                MailAddress fromAddress = new(_smtpOptions.Value.UserName, _smtpOptions.Value.SenderName);
                message.From = fromAddress;
                message.Subject = item.Subject;
                message.IsBodyHtml = true;
                message.Body = item.Body;
                if (item.Attachments != null && item.Attachments.Any())
                    item.Attachments.ToList().ForEach(e => message.Attachments.Add(e));

                var toList = item.To.Split(';');
                foreach (var s in toList)
                    message.To.Add(s);

                SmtpClient smtpClient = new()
                {
                    UseDefaultCredentials = _smtpOptions.Value.UseDefaultCredentials
                };
                NetworkCredential basicCredential = new(_smtpOptions.Value.UserName, _smtpOptions.Value.Password);
                smtpClient.Host = _smtpOptions.Value.Server;
                smtpClient.Port = _smtpOptions.Value.Port;
                smtpClient.EnableSsl = _smtpOptions.Value.Ssl;
                smtpClient.Credentials = basicCredential;

                if (_smtpOptions.Value.DeliveryMethod && _smtpOptions.Value.DeliveryMethodType.IsEnum<DeliveryMethodType>())

                    switch (_smtpOptions.Value.DeliveryMethodType.ToEnum<DeliveryMethodType>())
                    {
                        case DeliveryMethodType.Network:
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            break;
                        case DeliveryMethodType.PickupDirectoryFromIis:
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                            break;
                        case DeliveryMethodType.SpecifiedPickupDirectory:
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                            break;
                    }

                smtpClient.Send(message);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SendAsync(SmtpModel item) => await Task.Run(() => Submit(item));

        public bool Send(SmtpModel item) => Submit(item);
    }
}
