using Moq;
using LaborMarket.Api.Models;
using LaborMarket.Api.Models.JobApplicationModels;
using LaborMarket.Api.Services;
using LaborMarket.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Tests
{
	public class JobApplicationServiceTests
	{
		private LaborMarketContext GetInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<LaborMarketContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			return new LaborMarketContext(options);
		}

		[Fact]
		public async Task CreateApplicationAsync_CreatesAndReturnsApplication()
		{
			// Arrange
			using var context = GetInMemoryContext();

			context.Workers.Add(new UserModel
			{
				UserId = 1,
				FirstName = "TestUser",
				LastName = "Testov",
				Email = "user1@email.com",
				PasswordHash = "hashedpw",
				PhoneNumber = "0888123456"
			});
			context.Jobs.Add(new JobModel
			{
				JobId = 1,
				Title = "Dev",
				Company = "ACME",
				Description = "desc",
				Location = "Sofia",
				EmployerId = 1
			});
			context.SaveChanges();

			var mockEmail = new Mock<IEmailService>();
			var service = new JobApplicationService(context, mockEmail.Object);

			var model = new CreateApplicationModel { UserId = 1, JobId = 1 };

			// Act
			var result = await service.CreateApplicationAsync(model);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.UserId);
			Assert.Equal(1, result.JobId);
			Assert.Equal("Pending", result.Status);
			Assert.Single(context.Applications);
		}

		[Fact]
		public async Task ChangeApplicationStatusAsync_UpdatesStatus_And_SendsEmail()
		{
			using var context = GetInMemoryContext();

			// Seed user, employer, job, application
			var user = new UserModel
			{
				UserId = 1,
				Email = "test@user.com",
				FirstName = "User",
				LastName = "Testov",        // Required
				PasswordHash = "hashedpw",  // Required
				PhoneNumber = "0888123456"  // Required
			};
			var employer = new EmployerModel
			{
				EmployerId = 1,
				CompanyName = "Empl",
				ContactEmail = "employer@test.com",
				ContactPhone = "0888123456"
			};
			var job = new JobModel
			{
				JobId = 1,
				Title = "Dev",
				Company = "ACME",
				Description = "desc",
				Location = "Sofia",
				EmployerId = 1,
				Employer = employer
			};
			var application = new ApplicationModel
			{
				ApplicationId = 1,
				Status = "Pending",
				UserId = 1,
				JobId = 1,
				User = user,
				Job = job,
				ApplicationDate = DateTime.UtcNow
			};
			context.Workers.Add(user);
			context.Employers.Add(employer);
			context.Jobs.Add(job);
			context.Applications.Add(application);
			await context.SaveChangesAsync();

			var mockEmail = new Moq.Mock<IEmailService>();
			var service = new JobApplicationService(context, mockEmail.Object);

			// Act
			var result = await service.ChangeApplicationStatusAsync(1, "Одобрен");

			// Assert
			Assert.True(result);
			Assert.Equal("Одобрен", context.Applications.First().Status);

			mockEmail.Verify(e => e.SendEmailAsync("test@user.com", It.IsAny<string>(), It.Is<string>(s => s.Contains("одобрена"))), Times.Once);
		}

		[Fact]
		public async Task ChangeApplicationStatusAsync_ReturnsFalse_WhenNotFound()
		{
			using var context = GetInMemoryContext();
			var mockEmail = new Mock<IEmailService>();
			var service = new JobApplicationService(context, mockEmail.Object);

			var result = await service.ChangeApplicationStatusAsync(99, "Одобрен");

			Assert.False(result);
			mockEmail.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}

		[Fact]
		public async Task GetApplicationsForEmployerAsync_ReturnsApplications()
		{
			using var context = GetInMemoryContext();

			var employer = new EmployerModel
			{
				EmployerId = 1,
				CompanyName = "TestCo",
				ContactEmail = "employer@test.com",
				ContactPhone = "0888888888"
			};
			var user = new UserModel
			{
				UserId = 1,
				FirstName = "TestUser",
				LastName = "TestLast",
				Email = "user1@email.com",
				PasswordHash = "hashedpw",
				PhoneNumber = "0888123456"
			};
			var job = new JobModel
			{
				JobId = 1,
				Title = "Dev",
				Company = "ACME",
				Description = "desc",
				Location = "Sofia",
				EmployerId = 1,
				Employer = employer
			};
			var application = new ApplicationModel
			{
				ApplicationId = 1,
				UserId = 1,
				JobId = 1,
				ApplicationDate = DateTime.UtcNow,
				Status = "Pending",
				User = user,
				Job = job
			};

			context.Employers.Add(employer);
			context.Workers.Add(user);
			context.Jobs.Add(job);
			context.Applications.Add(application);
			await context.SaveChangesAsync();

			var mockEmail = new Moq.Mock<IEmailService>();
			var service = new JobApplicationService(context, mockEmail.Object);

			// Act
			var result = await service.GetApplicationsForEmployerAsync("employer@test.com");

			// Assert
			Assert.Single(result);
			var app = result.First();
			Assert.Equal(application.ApplicationId, app.ApplicationId);
			Assert.Equal("TestUser", app.UserName);
			Assert.Equal("Dev", app.JobName);
		}
	}
}