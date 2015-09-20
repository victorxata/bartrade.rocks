using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mailer
{
    public class MailSender
    {
        private static string _smtphost;
        private static string _mailfrom;
        private static string _mailuser;
        private static string _mailpassword;
        private static int _smtpport;
        private static bool _ssl;


        public MailSender LoadSettings()
        {
            _smtphost = ConfigurationManager.AppSettings["MailServer.host"];
            _smtpport = Convert.ToInt32(ConfigurationManager.AppSettings["MailServer.port"]);
            _mailfrom = ConfigurationManager.AppSettings["MailServer.smtpFrom"];
            _mailuser = ConfigurationManager.AppSettings["MailServer.username"];
            _mailpassword = ConfigurationManager.AppSettings["MailServer.password"];
            _ssl = false;

            return this;
        }

        private SmtpClient GetSmtpClient()
        {
            var result = new SmtpClient
                {
                    Port = _smtpport,
                    EnableSsl = _ssl,
                    Host = _smtphost,
                    Credentials = new NetworkCredential(_mailuser, _mailpassword)
                };
                return result;
        }

        public async Task SendMail(string mailTo, string subject, string body, string plainbody,
            string urlAttachmentFilename)
        {
            LoadSettings();

            var bodytext = body;

            var av1 = AlternateView.CreateAlternateViewFromString(bodytext, null, MediaTypeNames.Text.Html);

            var message = new MailMessage
            {
                From = new MailAddress(_mailfrom)
            };

            message.AlternateViews.Add(av1);

            message.To.Add(new MailAddress(mailTo));

            PrepareAttachment(message, urlAttachmentFilename);

            message.BodyEncoding = Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = plainbody;

            var client = GetSmtpClient();

            await client.SendMailAsync(message);
        }

        private void PrepareAttachment(MailMessage message, string urlAttachmentFilename)
        {
            if (string.IsNullOrEmpty(urlAttachmentFilename))
                return;

            var uri = new Uri(urlAttachmentFilename);

            var filename = Path.GetFileName(uri.LocalPath);

            var attachmentFilename = Path.Combine(Path.GetTempPath(), filename);

            var webClient = new WebClient();
            webClient.DownloadFile(uri, attachmentFilename);

            var attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
            var disposition = attachment.ContentDisposition;
            disposition.CreationDate = File.GetCreationTime(attachmentFilename);
            disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
            disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
            disposition.FileName = Path.GetFileName(attachmentFilename);
            disposition.Size = new FileInfo(attachmentFilename).Length;
            disposition.DispositionType = DispositionTypeNames.Attachment;

            message.Attachments.Add(attachment);
        }
    }
}

