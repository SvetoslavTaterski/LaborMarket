using LaborMarket.Api.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;

namespace LaborMarket.Api.Services
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _config;

		public EmailService(IConfiguration config)
		{
			_config = config;
		}
		public async Task SendEmailAsync(string to, string subject, string body)
		{
			var fromEmail = _config["Email:From"];
			var smtpServer = _config["Email:SmtpServer"];
			var username = _config["Email:Username"];
			var password = _config["Email:Password"];

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Трудова Борса", fromEmail));
			message.To.Add(MailboxAddress.Parse(to));
			message.Subject = subject;
			message.Body = new TextPart("plain") { Text = body };

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(smtpServer, 587, false);
			await smtp.AuthenticateAsync(username, password);
			await smtp.SendAsync(message);
			await smtp.DisconnectAsync(true);
		}
	}
}
