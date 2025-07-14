using LaborMarket.Api.Models.AuthenticationModels;
using Microsoft.AspNetCore.Mvc;
using LaborMarket.Api.Interfaces;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("RegisterController")]
	public class RegisterController : ControllerBase
	{
		private readonly IRegisterService _registerService;

		public RegisterController(IRegisterService registerService)
		{
			_registerService = registerService;
		}

		[HttpPost("register-user")]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterUserModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _registerService.RegisterUserAsync(model);

			if (!result.Succeeded)
				return BadRequest(result.Errors.Select(e => e.Description));

			return Ok(new { message = "User registered successfully." });
		}

		[HttpPost("register-employer")]
		public async Task<IActionResult> RegisterEmployer([FromBody] RegisterEmployerModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _registerService.RegisterEmployerAsync(model);

			if (!result.Succeeded)
				return BadRequest(result.Errors.Select(e => e.Description));

			return Ok("Employer registered successfully.");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var (success, email, role, error) = await _registerService.LoginAsync(model);

			if (!success)
				return Unauthorized();

			return Ok(new { Email = email, Role = role });
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _registerService.LogoutAsync();
			return Ok(new { message = "User logged out successfully." });
		}
	}
}