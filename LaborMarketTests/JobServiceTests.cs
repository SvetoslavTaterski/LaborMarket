using LaborMarket.Api.Services;
using LaborMarket.Api.Models.JobModels;
using LaborMarket.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Tests
{
	public class JobServiceTests
	{
		private LaborMarketContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<LaborMarketContext>()
				.UseInMemoryDatabase(databaseName: "LaborMarketTestDb_" + Guid.NewGuid())
				.Options;
			return new LaborMarketContext(options);
		}

		[Fact]
		public async Task GetAllJobsAsync_ReturnsAllJobs()
		{
			// Arrange
			var context = GetDbContext();
			var employer = new EmployerModel { EmployerId = 1, CompanyName = "TestCo", ContactEmail = "employer@test.com", ContactPhone = "0000000000" };
			context.Employers.Add(employer);
			context.Jobs.Add(new JobModel
			{
				JobId = 1,
				Title = "Developer",
				Description = "Dev job",
				Company = "TestCo",
				Location = "Remote",
				PostedAt = DateTime.UtcNow,
				EmployerId = employer.EmployerId,
				Employer = employer
			});
			context.SaveChanges();

			var service = new JobService(context);

			// Act
			var jobs = await service.GetAllJobsAsync();

			// Assert
			Assert.Single(jobs);
			Assert.Equal("Developer", jobs[0].Title);
			Assert.Equal("employer@test.com", jobs[0].Employer.ContactEmail);
		}

		[Fact]
		public async Task GetJobByIdAsync_ReturnsJob_WhenExists()
		{
			var context = GetDbContext();
			var job = new JobModel
			{
				JobId = 1,
				Title = "Test Job",
				Company = "TestCo",
				Description = "Test Description",
				Location = "Remote",
				PostedAt = DateTime.UtcNow
			};
			context.Jobs.Add(job);
			context.SaveChanges();

			var service = new JobService(context);

			var result = await service.GetJobByIdAsync(1);

			Assert.NotNull(result);
			Assert.Equal("Test Job", result.Title);
		}

		[Fact]
		public async Task GetJobByIdAsync_ReturnsNull_WhenNotExists()
		{
			var context = GetDbContext();
			var service = new JobService(context);

			var result = await service.GetJobByIdAsync(999);

			Assert.Null(result);
		}

		[Fact]
		public async Task CreateJobAsync_ReturnsJob_WhenEmployerExists()
		{
			var context = GetDbContext();
			var employer = new EmployerModel { EmployerId = 1, CompanyName = "TestCo", ContactEmail = "employer@test.com", ContactPhone = "0888123456" };
			context.Employers.Add(employer);
			context.SaveChanges();

			var service = new JobService(context);

			var createModel = new CreateJobModel
			{
				Title = "New Job",
				Description = "Desc",
				Location = "Remote",
				EmployerEmail = "employer@test.com",
			};

			var result = await service.CreateJobAsync(createModel);

			Assert.NotNull(result);
			Assert.Equal("New Job", result.Title);
			Assert.Equal("TestCo", result.Company);
			Assert.Equal(employer.EmployerId, result.EmployerId);
		}

		[Fact]
		public async Task CreateJobAsync_ReturnsNull_WhenEmployerNotFound()
		{
			var context = GetDbContext();
			var service = new JobService(context);

			var createModel = new CreateJobModel
			{
				Title = "New Job",
				Description = "Desc",
				Location = "Remote",
				EmployerEmail = "notfound@test.com"
			};

			var result = await service.CreateJobAsync(createModel);

			Assert.Null(result);
		}

		[Fact]
		public async Task EditJobAsync_UpdatesJob_WhenExists()
		{
			var context = GetDbContext();
			var job = new JobModel
			{
				JobId = 1,
				Title = "Old",
				Description = "Old",
				Location = "Old",
				Company = "TestCo"
			};
			context.Jobs.Add(job);
			context.SaveChanges();

			var service = new JobService(context);

			var editModel = new EditJobModel
			{
				JobId = 1,
				Title = "New",
				Description = "New",
				Location = "New"
			};

			var result = await service.EditJobAsync(editModel);

			Assert.True(result);
			var updated = context.Jobs.First(j => j.JobId == 1);
			Assert.Equal("New", updated.Title);
			Assert.Equal("New", updated.Description);
			Assert.Equal("New", updated.Location);
		}

		[Fact]
		public async Task EditJobAsync_ReturnsFalse_WhenJobNotFound()
		{
			var context = GetDbContext();
			var service = new JobService(context);

			var editModel = new EditJobModel
			{
				JobId = 999,
				Title = "New",
				Description = "New",
				Location = "New"
			};

			var result = await service.EditJobAsync(editModel);

			Assert.False(result);
		}

		[Fact]
		public async Task DeleteJobAsync_DeletesJob_WhenExists()
		{
			var context = GetDbContext();
			var job = new JobModel
			{
				JobId = 1,
				Title = "ToDelete",
				Company = "TestCompany",
				Description = "TestDesc",
				Location = "TestLocation"
			};
			context.Jobs.Add(job);
			context.SaveChanges();

			var service = new JobService(context);

			var result = await service.DeleteJobAsync(1);

			Assert.True(result);
			Assert.Empty(context.Jobs);
		}

		[Fact]
		public async Task DeleteJobAsync_ReturnsFalse_WhenJobNotFound()
		{
			var context = GetDbContext();
			var service = new JobService(context);

			var result = await service.DeleteJobAsync(999);

			Assert.False(result);
		}
	}
}