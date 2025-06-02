using LaborMarket.Api.Models;
using LaborMarket.Api.Models.AuthenticationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("RegisterController")]
	public class RegisterController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly LaborMarketContext _context;

		public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, LaborMarketContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_context = context;
		}

		[HttpPost("register-user")]
		public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!await _roleManager.RoleExistsAsync(model.Role))
				return BadRequest("Unexisting Role");

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
				return MethodHasNotSucceeded(result);

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


			return Ok("User registered successfully.");
		}

		[HttpPost("register-employer")]
		public async Task<IActionResult> Register([FromBody] RegisterEmployerModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!await _roleManager.RoleExistsAsync(model.Role))
				return BadRequest("Unexisting role");

			var user = new ApplicationUser
			{
				UserName = model.ContactEmail,
				Email = model.ContactEmail,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
				return MethodHasNotSucceeded(result);

			await _userManager.AddToRoleAsync(user, model.Role);

			var newEmployer = new EmployerModel
			{
				CompanyName = model.CompanyName,
				ContactEmail = model.ContactEmail,
				ContactPhone = model.ContactPhone
			};

			await _context.Employers.AddAsync(newEmployer);
			await _context.SaveChangesAsync();

			return Ok("Employer registered successfully.");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
				return Unauthorized();


			await _signInManager.SignInAsync(user, isPersistent: false);

			var roles = await _userManager.GetRolesAsync(user);

			return Ok(new { user.Email, Role = roles[0] });
		}

		private IActionResult MethodHasNotSucceeded(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return BadRequest(ModelState);
		}
	}
}