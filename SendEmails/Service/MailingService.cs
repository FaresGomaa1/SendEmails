using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendEmails.Settings;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SendEmails.Service
{
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailSetting;

        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSetting = mailSettings.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSetting.Email),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(mailTo));
            var builder = new BodyBuilder();
            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            byte[] fileBytes = ms.ToArray();
                            builder.Attachments.Add(file.Name, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Email));

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_mailSetting.Host,587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSetting.Email, _mailSetting.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
