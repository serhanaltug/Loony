using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Loony.Tools
{
    public class MailSender
    {
        public static bool SendEmail(string mailTo, string body, string subject)
        {
            return SendEmail(mailTo, body, subject, true);
        }

        public static bool SendEmail(string mailTo, string body, string subject, bool sendEmail)
        {
            bool result = false;
            if (sendEmail == true)
            {
                try
                {
                    var mailSettings = new MailConfiguration();

                    MailMessage mail = new MailMessage();
                    mail.To.Add(new MailAddress(mailTo));
                    mail.From = new MailAddress(mailSettings.from);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = Encoding.GetEncoding(65001);

                    SmtpClient smtp = new SmtpClient(mailSettings.host, mailSettings.port);
                    //smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(mailSettings.username, mailSettings.password);
                    smtp.Send(mail);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }

    internal class MailConfiguration
    {
        public MailConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            host = config["Smtp:Host"];
            from = config["Smtp:From"];
            username = config["Smtp:Username"];
            password = config["Smtp:Password"];
            port = int.Parse(config["Smtp:Port"]);
        }

        public string host { get; set; }
        public string from { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int port { get; set; }
    }
}
