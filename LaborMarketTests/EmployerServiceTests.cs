using Moq;
using LaborMarket.Api.Models;
using LaborMarket.Api.Services;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;

namespace LaborMarket.Tests
{
	public class EmployerServiceTests
	{
		private LaborMarketContext GetInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<LaborMarketContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			return new LaborMarketContext(options);
		}

		private EmployerModel GetFakeEmployer() => new EmployerModel
		{
			EmployerId = 1,
			CompanyName = "TestCo",
			ContactEmail = "employer@test.com",
			ContactPhone = "0888888888"
		};

		private Cloudinary GetMockCloudinary()
		{
			// Use dummy values, no real cloud account is required for unit tests
			var dummyAccount = new Account("cloud", "key", "secret");
			return new Cloudinary(dummyAccount);
		}

		[Fact]
		public async Task GetAllEmployersAsync_ReturnsAllEmployers()
		{
			using var context = GetInMemoryContext();
			context.Employers.Add(GetFakeEmployer());
			context.Employers.Add(new EmployerModel
			{
				EmployerId = 2,
				CompanyName = "SecondCo",
				ContactEmail = "second@test.com",
				ContactPhone = "0888999999"
			});
			await context.SaveChangesAsync();

			var service = new EmployerService(context, GetMockCloudinary());
			var result = await service.GetAllEmployersAsync();

			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task GetEmployerByEmailAsync_ReturnsEmployer_WhenExists()
		{
			using var context = GetInMemoryContext();
			var employer = GetFakeEmployer();
			context.Employers.Add(employer);
			await context.SaveChangesAsync();

			var service = new EmployerService(context, GetMockCloudinary());
			var result = await service.GetEmployerByEmailAsync("employer@test.com");

			Assert.NotNull(result);
			Assert.Equal(employer.EmployerId, result.EmployerId);
		}

		[Fact]
		public async Task GetEmployerByEmailAsync_ReturnsNull_WhenNotFound()
		{
			using var context = GetInMemoryContext();
			var service = new EmployerService(context, GetMockCloudinary());

			var result = await service.GetEmployerByEmailAsync("notfound@test.com");
			Assert.Null(result);
		}

		[Fact]
		public async Task GetEmployerByIdAsync_ReturnsEmployer_WhenExists()
		{
			using var context = GetInMemoryContext();
			var employer = GetFakeEmployer();
			context.Employers.Add(employer);
			await context.SaveChangesAsync();

			var service = new EmployerService(context, GetMockCloudinary());
			var result = await service.GetEmployerByIdAsync(1);

			Assert.NotNull(result);
			Assert.Equal("TestCo", result.CompanyName);
		}

		[Fact]
		public async Task GetEmployerByIdAsync_ReturnsNull_WhenNotFound()
		{
			using var context = GetInMemoryContext();
			var service = new EmployerService(context, GetMockCloudinary());

			var result = await service.GetEmployerByIdAsync(99);
			Assert.Null(result);
		}

		[Fact]
		public async Task SetEmployerDescriptionAsync_UpdatesDescription_WhenEmployerExists()
		{
			using var context = GetInMemoryContext();
			var employer = GetFakeEmployer();
			context.Employers.Add(employer);
			await context.SaveChangesAsync();

			var service = new EmployerService(context, GetMockCloudinary());
			var result = await service.SetEmployerDescriptionAsync("employer@test.com", "New description");

			Assert.True(result);
			Assert.Equal("New description", context.Employers.First().Description);
		}

		[Fact]
		public async Task SetEmployerDescriptionAsync_ReturnsFalse_WhenEmployerNotFound()
		{
			using var context = GetInMemoryContext();
			var service = new EmployerService(context, GetMockCloudinary());

			var result = await service.SetEmployerDescriptionAsync("notfound@test.com", "desc");
			Assert.False(result);
		}

		[Fact]
		public async Task UploadProfileImageAsync_ReturnsNull_WhenEmployerNotFound()
		{
			using var context = GetInMemoryContext();
			var dummyAccount = new Account("cloud", "key", "secret");
			var mockCloudinary = new Mock<Cloudinary>(dummyAccount);
			var mockFile = new Mock<IFormFile>();

			var service = new EmployerService(context, mockCloudinary.Object);
			var result = await service.UploadProfileImageAsync("notfound@test.com", mockFile.Object);
			Assert.Null(result);
		}

		[Fact]
		public async Task UploadProfileImageAsync_ReturnsNull_WhenFileIsNullOrEmpty()
		{
			using var context = GetInMemoryContext();
			var employer = GetFakeEmployer();
			context.Employers.Add(employer);
			await context.SaveChangesAsync();

			var dummyAccount = new Account("cloud", "key", "secret");
			var mockCloudinary = new Mock<Cloudinary>(dummyAccount);
			var service = new EmployerService(context, mockCloudinary.Object);

			// File is null
			var resultNull = await service.UploadProfileImageAsync("employer@test.com", null);
			Assert.Null(resultNull);

			// File is empty
			var mockFile = new Mock<IFormFile>();
			mockFile.Setup(f => f.Length).Returns(0);
			var resultEmpty = await service.UploadProfileImageAsync("employer@test.com", mockFile.Object);
			Assert.Null(resultEmpty);
		}
	}
}