using FluentEmail.Smtp;
using Microsoft.Extensions.Options;
using MovieStore.Infrastructure.Configurations;
using System.Net.Mail;

namespace MovieStore.Infrastructure
{
    public class EmailService
    {
        private readonly SmtpSender _smtpSender;
        private readonly EmailServiceOptions _emailOptions;

        public EmailService(IOptions<EmailServiceOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
            _smtpSender = new SmtpSender(() => new SmtpClient(_emailOptions.SmtpHost, _emailOptions.SmtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(_emailOptions.SmtpUsername, _emailOptions.SmtpPassword),
                EnableSsl = true
            });
        }

        public async Task SendEmailAsync(string from, string to, string subject, string body)
        {
            var client = new SmtpClient(_emailOptions.SmtpHost, _emailOptions.SmtpPort);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential(_emailOptions.SmtpUsername, _emailOptions.SmtpPassword);
            var message = new MailMessage(from, to, subject, body);
            await client.SendMailAsync(message);
        }
    }
}
