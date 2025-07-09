using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("UserController")]
	public class UserController : Controller
	{
		private readonly LaborMarketContext _context;
		private readonly Cloudinary _cloudinary;

		public UserController(LaborMarketContext context, Cloudinary cloudinary)
		{
			_context = context;
			_cloudinary = cloudinary;
		}

		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
		{
			return await _context.Workers.ToListAsync();
		}

		[HttpGet("GetUserByEmail")]
		public async Task<ActionResult<UserModel>> GetUserByEmail(string userEmail)
		{
			var user = await _context.Workers.FirstOrDefaultAsync(u => u.Email == userEmail);

			if (user == null)
				return NotFound();

			return user;
		}

		[HttpGet("GetUserById")]
		public async Task<ActionResult<UserModel>> GetUserById(int userId)
		{
			var user = await _context.Workers.FirstOrDefaultAsync(u => u.UserId == userId);

			if (user == null)
				return NotFound();

			return user;
		}

		[HttpPut("SetUserCV")]
		public async Task<ActionResult> SetUserCV(string userEmail, string cv)
		{
			var user = await _context.Workers.FirstOrDefaultAsync(u => u.Email == userEmail);

			if (user == null)
				return NotFound();

			user.CV = cv;

			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost("UploadProfileImage")]
		public async Task<IActionResult> UploadProfileImage([FromQuery] string userEmail, IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var user = await _context.Workers.FirstOrDefaultAsync(u => u.Email == userEmail);
			if (user == null)
				return NotFound();

			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, file.OpenReadStream()),
				Folder = "user_profile_pics"
			};
			var uploadResult = await _cloudinary.UploadAsync(uploadParams);

			user.ProfileImageUrl = uploadResult.SecureUrl.ToString();
			await _context.SaveChangesAsync();

			return Ok(new { imageUrl = user.ProfileImageUrl });
		}
	}
}
