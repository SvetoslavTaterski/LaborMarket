using Moq;
using LaborMarket.Api.Services;
using LaborMarket.Api.Models;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace LaborMarket.Tests
{
	public class UserServiceTests
	{
		private LaborMarketContext GetDbContextWithUsers(List<UserModel> users)
		{
			var options = new DbContextOptionsBuilder<LaborMarketContext>()
				.UseInMemoryDatabase(databaseName: "LaborMarketTestDb" + Guid.NewGuid())
				.Options;
			var context = new LaborMarketContext(options);
			context.Workers.AddRange(users);
			context.SaveChanges();
			return context;
		}

		[Fact]
		public async Task GetAllUsersAsync_ReturnsAllUsers()
		{
			// Arrange
			var users = new List<UserModel>
		{
			new UserModel
			{
				UserId = 1,
				Email = "a@test.com",
				FirstName = "Test",
				LastName = "User",
				PasswordHash = "hash",
				PhoneNumber = "0000000000"
			},
			new UserModel
			{
				UserId = 2,
				Email = "b@test.com",
				FirstName = "Test2",
				LastName = "User2",
				PasswordHash = "hash2",
				PhoneNumber = "1111111111"
			}
		};
			var context = GetDbContextWithUsers(users);
			var cloudinaryMock = new Mock<Cloudinary>(new Account("a", "b", "c"));
			var service = new UserService(context, cloudinaryMock.Object);

			// Act
			var result = await service.GetAllUsersAsync();

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task GetUserByEmailAsync_ReturnsUser_WhenExists()
		{
			var users = new List<UserModel>
		{
			new UserModel
			{
				UserId = 1,
				Email = "a@test.com",
				FirstName = "Test",
				LastName = "User",
				PasswordHash = "hash",
				PhoneNumber = "0000000000"
			}
		};
			var context = GetDbContextWithUsers(users);
			var cloudinaryMock = new Mock<Cloudinary>(new Account("a", "b", "c"));
			var service = new UserService(context, cloudinaryMock.Object);

			var result = await service.GetUserByEmailAsync("a@test.com");

			Assert.NotNull(result);
			Assert.Equal(1, result.UserId);
		}

		[Fact]
		public async Task GetUserByEmailAsync_ReturnsNull_WhenNotExists()
		{
			var context = GetDbContextWithUsers(new List<UserModel>());
			var cloudinaryMock = new Mock<Cloudinary>(new Account("a", "b", "c"));
			var service = new UserService(context, cloudinaryMock.Object);

			var result = await service.GetUserByEmailAsync("notfound@test.com");

			Assert.Null(result);
		}

		[Fact]
		public async Task SetUserCVAsync_UpdatesCV_WhenUserExists()
		{
			var users = new List<UserModel>
		{
			new UserModel
			{
				UserId = 1,
				Email = "a@test.com",
				FirstName = "Test",
				LastName = "User",
				PasswordHash = "hash",
				PhoneNumber = "0000000000"
			}
		};
			var context = GetDbContextWithUsers(users);
			var cloudinaryMock = new Mock<Cloudinary>(new Account("a", "b", "c"));
			var service = new UserService(context, cloudinaryMock.Object);

			var result = await service.SetUserCVAsync("a@test.com", "mycv.pdf");

			Assert.True(result);
			var user = context.Workers.First(u => u.Email == "a@test.com");
			Assert.Equal("mycv.pdf", user.CV);
		}

		[Fact]
		public async Task SetUserCVAsync_ReturnsFalse_WhenUserNotFound()
		{
			var context = GetDbContextWithUsers(new List<UserModel>());
			var cloudinaryMock = new Mock<Cloudinary>(new Account("a", "b", "c"));
			var service = new UserService(context, cloudinaryMock.Object);

			var result = await service.SetUserCVAsync("notfound@test.com", "cv.pdf");

			Assert.False(result);
		}

		[Fact]
		public async Task UploadProfileImageAsync_ReturnsNull_WhenUserNotFound()
		{
			var context = GetDbContextWithUsers(new List<UserModel>());
			var cloudinaryMock = new Mock<Cloudinary>(new Account("a", "b", "c"));
			var service = new UserService(context, cloudinaryMock.Object);

			var fileMock = new Mock<IFormFile>();

			var result = await service.UploadProfileImageAsync("notfound@test.com", fileMock.Object);

			Assert.Null(result);
		}
	}
}