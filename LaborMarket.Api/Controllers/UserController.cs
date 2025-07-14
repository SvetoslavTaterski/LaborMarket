using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("UserController")]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
		{
			var users = await _userService.GetAllUsersAsync();
			return Ok(users);
		}

		[HttpGet("GetUserByEmail")]
		public async Task<ActionResult<UserModel>> GetUserByEmail(string userEmail)
		{
			var user = await _userService.GetUserByEmailAsync(userEmail);
			if (user == null)
				return NotFound();
			return Ok(user);
		}

		[HttpGet("GetUserById")]
		public async Task<ActionResult<UserModel>> GetUserById(int userId)
		{
			var user = await _userService.GetUserByIdAsync(userId);
			if (user == null)
				return NotFound();
			return Ok(user);
		}

		[HttpPut("SetUserCV")]
		public async Task<ActionResult> SetUserCV(string userEmail, string cv)
		{
			var success = await _userService.SetUserCVAsync(userEmail, cv);
			if (!success)
				return NotFound();
			return Ok();
		}

		[HttpPost("UploadProfileImage")]
		public async Task<IActionResult> UploadProfileImage([FromQuery] string userEmail, IFormFile file)
		{
			var imageUrl = await _userService.UploadProfileImageAsync(userEmail, file);
			if (imageUrl == null)
				return NotFound();
			return Ok(new { imageUrl });
		}
	}
}