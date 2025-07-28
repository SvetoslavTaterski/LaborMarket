using Microsoft.Extensions.Configuration;
using LaborMarket.Api.Services;
using System.Net.Sockets;

namespace LaborMarket.Tests
{
	public class EmailServiceTests
	{
		[Fact]
		public async Task SendEmailAsync_ThrowsSocketExceptionWithFakeServer()
		{
			var configMock = new Moq.Mock<IConfiguration>();
			configMock.Setup(c => c["Email:From"]).Returns("sender@test.com");
			configMock.Setup(c => c["Email:SmtpServer"]).Returns("smtp.test.com");
			configMock.Setup(c => c["Email:Username"]).Returns("user");
			configMock.Setup(c => c["Email:Password"]).Returns("pass");

			var service = new EmailService(configMock.Object);

			await Assert.ThrowsAsync<SocketException>(async () =>
			{
				await service.SendEmailAsync("recipient@test.com", "Test Subject", "Test Body");
			});
		}
	}
}