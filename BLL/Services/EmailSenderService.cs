using BLL.Configuration;
using BLL.Interfaces;
using BLL.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EmailSenderService : INotificationSenderService
    {
        private readonly EmailSenderConfiguration _senderConfiguration;

        public EmailSenderService(IOptions<EmailSenderConfiguration> options)
        {
            _senderConfiguration = options.Value;
        }

        public async Task SendNotification(NotificationModel model)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_senderConfiguration.SenderName, _senderConfiguration.SenderEmail));
            message.To.Add(new MailboxAddress(model.RecipientName, model.EmailAdress));

            message.Subject = model.EmailSubject;
            
            //Add body
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = model.EmailText;

            message.Body = bodyBuilder.ToMessageBody();

            //Send
            await Task.Run(() =>
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                    client.Connect(_senderConfiguration.SmtpServer, Convert.ToInt32(_senderConfiguration.SmtpPort), true);
                    client.Authenticate(_senderConfiguration.SenderEmail, _senderConfiguration.Password);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            );

        }
    }
}
