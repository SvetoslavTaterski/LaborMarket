using Moq;
using LaborMarket.Api.Services;
using LaborMarket.Api.Models.AuthenticationModels;
using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Tests
{
	public class RegisterServiceTests
	{
		private Mock<UserManager<ApplicationUser>> _userManagerMock;
		private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
		private Mock<RoleManager<ApplicationRole>> _roleManagerMock;
		private LaborMarketContext _context;

		public RegisterServiceTests()
		{
			// UserManager mock
			var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
			_userManagerMock = new Mock<UserManager<ApplicationUser>>(
				userStoreMock.Object, null, null, null, null, null, null, null, null);

			// SignInManager dependencies
			var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
			var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
			var optionsMock = new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>();
			var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<SignInManager<ApplicationUser>>>();
			var schemesMock = new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>();
			var confirmationMock = new Mock<IUserConfirmation<ApplicationUser>>();

			_signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
				_userManagerMock.Object,
				contextAccessorMock.Object,
				claimsFactoryMock.Object,
				optionsMock.Object,
				loggerMock.Object,
				schemesMock.Object,
				confirmationMock.Object
			);

			// RoleManager mock
			var roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
			_roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
				roleStoreMock.Object, null, null, null, null);

			// In-memory context
			var options = new DbContextOptionsBuilder<LaborMarketContext>()
				.UseInMemoryDatabase(databaseName: "LaborMarketTestDb_" + System.Guid.NewGuid())
				.Options;
			_context = new LaborMarketContext(options);
		}

		[Fact]
		public async Task RegisterUserAsync_ReturnsSuccess_WhenValid()
		{
			_roleManagerMock.Setup(r => r.RoleExistsAsync("Worker")).ReturnsAsync(true);
			_userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);
			_userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Worker"))
				.ReturnsAsync(IdentityResult.Success);

			var service = new RegisterService(
				_userManagerMock.Object,
				_signInManagerMock.Object,
				_roleManagerMock.Object,
				_context);

			var model = new RegisterUserModel
			{
				Email = "test@test.com",
				Password = "Password123!",
				FirstName = "Test",
				LastName = "User",
				PhoneNumber = "0000000000",
				Role = "Worker"
			};

			var result = await service.RegisterUserAsync(model);

			Assert.True(result.Succeeded);
			Assert.Single(_context.Workers.Where(u => u.Email == "test@test.com"));
		}

		[Fact]
		public async Task RegisterUserAsync_ReturnsFailed_WhenRoleDoesNotExist()
		{
			_roleManagerMock.Setup(r => r.RoleExistsAsync("Worker")).ReturnsAsync(false);

			var service = new RegisterService(
				_userManagerMock.Object,
				_signInManagerMock.Object,
				_roleManagerMock.Object,
				_context);

			var model = new RegisterUserModel
			{
				Email = "test@test.com",
				Password = "Password123!",
				FirstName = "Test",
				LastName = "User",
				PhoneNumber = "0000000000",
				Role = "Worker"
			};

			var result = await service.RegisterUserAsync(model);

			Assert.False(result.Succeeded);
			Assert.Empty(_context.Workers);
		}

		[Fact]
		public async Task RegisterEmployerAsync_ReturnsSuccess_WhenValid()
		{
			_roleManagerMock.Setup(r => r.RoleExistsAsync("Employer")).ReturnsAsync(true);
			_userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);
			_userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Employer"))
				.ReturnsAsync(IdentityResult.Success);

			var service = new RegisterService(
				_userManagerMock.Object,
				_signInManagerMock.Object,
				_roleManagerMock.Object,
				_context);

			var model = new RegisterEmployerModel
			{
				CompanyName = "TestCo",
				ContactEmail = "employer@test.com",
				ContactPhone = "1234567890",
				Password = "Password123!",
				Role = "Employer"
			};

			var result = await service.RegisterEmployerAsync(model);

			Assert.True(result.Succeeded);
			Assert.Single(_context.Employers.Where(e => e.ContactEmail == "employer@test.com"));
		}

		[Fact]
		public async Task LoginAsync_ReturnsSuccess_WhenCredentialsAreValid()
		{
			var user = new ApplicationUser { Email = "test@test.com" };

			_userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com"))
				.ReturnsAsync(user);
			_userManagerMock.Setup(u => u.CheckPasswordAsync(user, "Password123!"))
				.ReturnsAsync(true);
			_userManagerMock.Setup(u => u.GetRolesAsync(user))
				.ReturnsAsync(new List<string> { "Worker" });
			_signInManagerMock.Setup(s => s.SignInAsync(user, false, null))
				.Returns(Task.CompletedTask);

			var service = new RegisterService(
				_userManagerMock.Object,
				_signInManagerMock.Object,
				_roleManagerMock.Object,
				_context);

			var model = new LoginModel
			{
				Email = "test@test.com",
				Password = "Password123!"
			};

			var (success, email, role, error) = await service.LoginAsync(model);

			Assert.True(success);
			Assert.Equal("test@test.com", email);
			Assert.Equal("Worker", role);
			Assert.Null(error);
		}

		[Fact]
		public async Task LoginAsync_ReturnsUnauthorized_WhenCredentialsAreInvalid()
		{
			_userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com"))
				.ReturnsAsync((ApplicationUser)null);

			var service = new RegisterService(
				_userManagerMock.Object,
				_signInManagerMock.Object,
				_roleManagerMock.Object,
				_context);

			var model = new LoginModel
			{
				Email = "test@test.com",
				Password = "Password123!"
			};

			var (success, email, role, error) = await service.LoginAsync(model);

			Assert.False(success);
			Assert.Null(email);
			Assert.Null(role);
			Assert.Equal("Unauthorized", error);
		}

		[Fact]
		public async Task LogoutAsync_CallsSignOut()
		{
			_signInManagerMock.Setup(s => s.SignOutAsync()).Returns(Task.CompletedTask).Verifiable();

			var service = new RegisterService(
				_userManagerMock.Object,
				_signInManagerMock.Object,
				_roleManagerMock.Object,
				_context);

			await service.LogoutAsync();

			_signInManagerMock.Verify(s => s.SignOutAsync(), Times.Once);
		}
	}
}