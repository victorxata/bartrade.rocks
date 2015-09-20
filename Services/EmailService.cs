using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Mailer;
using MongoDB.Driver;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailTemplatesRepository _repository;


        public EmailService(IEmailTemplatesRepository repository)
        {
            _repository = repository;
        }

        public string GetHtmlEmailPrepared(string templateName, Dictionary<string, string> content, string language = null)
        {
            var lang = "en-US";
                if (!string.IsNullOrEmpty(language))
                    lang = language;

                var template = _repository.Collection.Find(x => x.Name == templateName && x.Language == lang).FirstOrDefaultAsync().Result ??
                               _repository.Collection.Find(x => x.Name == templateName && x.Language == "en-US").FirstOrDefaultAsync().Result;

                return template != null ? PrepareEmail(template.HtmlBody, content) : null;
        }

        public string GetPlainEmailPrepared(string templateName, Dictionary<string, string> content, string language = null)
        {
            var lang = "en-US";
                if (!string.IsNullOrEmpty(language))
                    lang = language;

                var template = _repository.Collection.Find(x => x.Name == templateName && x.Language == lang).FirstOrDefaultAsync().Result ??
                               _repository.Collection.Find(x => x.Name == templateName && x.Language == "en-US").FirstOrDefaultAsync().Result;

                return template != null ? PrepareEmail(template.PlainText, content) : null;
        }

        public async Task SendEmail(string mailTo, string subject, string templateName, Dictionary<string, string> content, string language = null, string attachment = null)
        {
            var htmlbody = GetHtmlEmailPrepared(templateName, content, language);

                var plainbody = GetPlainEmailPrepared(templateName, content, language);

                await SendEmail(mailTo, subject, htmlbody, plainbody, attachment);
        }

        #region This method will work until we activate the Queue system and the ServiceBus

        public async Task<bool> SendEmail(string mailTo, string subject, string htmlBody, string plainBody, string attachment)
        {
            
                var emailSender = new MailSender();

                await emailSender.SendMail(mailTo, subject, htmlBody, plainBody, attachment);
            
                return true;
        }

        #endregion

        private static string PrepareEmail(string body, Dictionary<string, string> content)
        {
            var newBody = body;
            foreach (var pair in content)
            {
                var searchString = "{{" + pair.Key + "}}";
                newBody = newBody.Replace(searchString, pair.Value);
            }
            return newBody;
        }
    }
}

