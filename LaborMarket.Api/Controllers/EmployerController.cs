using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("EmployerController")]
	public class EmployerController : Controller
	{
		private readonly LaborMarketContext _context;
		private readonly Cloudinary _cloudinary;


		public EmployerController(LaborMarketContext context, Cloudinary cloudinary)
		{
			_context = context;
			_cloudinary = cloudinary;
		}

		[HttpGet("GetAllEmployers")]
		public async Task<ActionResult<IEnumerable<EmployerModel>>> GetAllEmployers()
		{
			return await _context.Employers.ToListAsync();
		}

		[HttpGet("GetEmployerByEmail")]
		public async Task<ActionResult<EmployerModel>> GetEmployerByEmail(string userEmail)
		{
			var employer = await _context.Employers.FirstOrDefaultAsync(u => u.ContactEmail == userEmail);

			if (employer == null)
				return NotFound();

			return employer;
		}

		[HttpGet("GetEmployerById")]
		public async Task<ActionResult<EmployerModel>> GetEmployerById(int userId)
		{
			var employer = await _context.Employers.FirstOrDefaultAsync(u => u.EmployerId == userId);

			if (employer == null)
				return NotFound();

			return employer;
		}

		[HttpPut("SetEmployerDescription")]
		public async Task<ActionResult> SetEmployerDescription(string employerEmail,  string description)
		{
			var employer = await _context.Employers.FirstOrDefaultAsync(u => u.ContactEmail == employerEmail);

			if (employer == null)
				return NotFound();

			employer.Description = description;

			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost("UploadProfileImage")]
		public async Task<IActionResult> UploadProfileImage([FromQuery] string employerEmail, IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var employer = await _context.Employers.FirstOrDefaultAsync(e => e.ContactEmail == employerEmail);
			if (employer == null)
				return NotFound();

			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, file.OpenReadStream()),
				Folder = "employer_profile_pics"
			};
			var uploadResult = await _cloudinary.UploadAsync(uploadParams);

			employer.ProfileImageUrl = uploadResult.SecureUrl.ToString();
			await _context.SaveChangesAsync();

			return Ok(new { imageUrl = employer.ProfileImageUrl });
		}
	}
}
