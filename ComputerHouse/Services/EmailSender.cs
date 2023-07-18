using ComputerHouse.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ComputerHouse.Services
{
    //Microsoft.AspNetCore.Identity.UI.Service already contains this interface we need to implement it.
    public class EmailSender : IEmailSender
    {
        private readonly MailOptions _options;
        private readonly string _adminAddress;
        private readonly string _fromName;
        private readonly string _fromAddress;
        private readonly string _apiKey;

        public EmailSender(IOptions<MailOptions> mailOptions)
        {
            _options = mailOptions.Value;
            _adminAddress = _options.AdminAddress;
            _fromName = _options.FromName;
            _fromAddress = _options.FromAddress;
            _apiKey = _options.ApiKey;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false, //This should always be above credentials.
                Credentials = new NetworkCredential("computerhouse48@gmail.com", "Computer8house."),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage("computerhouse48@gmail.com", email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            smtpClient.Send(mailMessage);

            return Task.CompletedTask;
        }
    }
}
