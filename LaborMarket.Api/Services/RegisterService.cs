using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models.AuthenticationModels;
using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace LaborMarket.Api.Services
{
	public class RegisterService : IRegisterService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly LaborMarketContext _context;

		public RegisterService(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<ApplicationRole> roleManager,
			LaborMarketContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_context = context;
		}

		public async Task<IdentityResult> RegisterUserAsync(RegisterUserModel model)
		{
			if (!await _roleManager.RoleExistsAsync(model.Role))
				return IdentityResult.Failed(new IdentityError { Description = "Unexisting Role" });

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
				return result;

			await _userManager.AddToRoleAsync(user, model.Role);

			var newUser = new UserModel
			{
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				PasswordHash = model.Password,
				CreatedAt = DateTime.UtcNow,
				PhoneNumber = model.PhoneNumber
			};

			await _context.Workers.AddAsync(newUser);
			await _context.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public async Task<IdentityResult> RegisterEmployerAsync(RegisterEmployerModel model)
		{
			if (!await _roleManager.RoleExistsAsync(model.Role))
				return IdentityResult.Failed(new IdentityError { Description = "Unexisting Role" });

			var user = new ApplicationUser
			{
				UserName = model.ContactEmail,
				Email = model.ContactEmail,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
				return result;

			await _userManager.AddToRoleAsync(user, model.Role);

			var newEmployer = new EmployerModel
			{
				CompanyName = model.CompanyName,
				ContactEmail = model.ContactEmail,
				ContactPhone = model.ContactPhone
			};

			await _context.Employers.AddAsync(newEmployer);
			await _context.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public async Task<(bool Success, string Email, string Role, string Error)> LoginAsync(LoginModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
				return (false, null, null, "Unauthorized")!;

			await _signInManager.SignInAsync(user, isPersistent: false);
			var roles = await _userManager.GetRolesAsync(user);

			return (true, user.Email, roles.FirstOrDefault() ?? "", null)!;
		}

		public async Task LogoutAsync()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
