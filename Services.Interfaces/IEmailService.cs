using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEmailService
    {
        string GetHtmlEmailPrepared(string templateName, Dictionary<string, string> content, string language = null);
        string GetPlainEmailPrepared(string templateName, Dictionary<string, string> content, string language = null);
        Task SendEmail(string mailTo, string subject, string templateName, Dictionary<string, string> content, string language = null, string attachment = null);

        Task<bool> SendEmail(string mailTo, string subject, string htmlBody, string plainBody, string attachment);
    }
}
