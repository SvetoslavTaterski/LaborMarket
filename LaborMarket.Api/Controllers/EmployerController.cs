using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;
using LaborMarket.Api.Interfaces;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("EmployerController")]
	public class EmployerController : Controller
	{
		private readonly IEmployerService _employerService;

		public EmployerController(IEmployerService employerService)
		{
			_employerService = employerService;
		}

		[HttpGet("GetAllEmployers")]
		public async Task<ActionResult<IEnumerable<EmployerModel>>> GetAllEmployers()
			=> Ok(await _employerService.GetAllEmployersAsync());

		[HttpGet("GetEmployerByEmail")]
		public async Task<ActionResult<EmployerModel>> GetEmployerByEmail(string userEmail)
		{
			var employer = await _employerService.GetEmployerByEmailAsync(userEmail);
			if (employer == null) return NotFound();
			return employer;
		}

		[HttpGet("GetEmployerById")]
		public async Task<ActionResult<EmployerModel>> GetEmployerById(int userId)
		{
			var employer = await _employerService.GetEmployerByIdAsync(userId);
			if (employer == null) return NotFound();
			return employer;
		}

		[HttpPut("SetEmployerDescription")]
		public async Task<ActionResult> SetEmployerDescription(string employerEmail, string description)
		{
			var result = await _employerService.SetEmployerDescriptionAsync(employerEmail, description);
			if (!result) return NotFound();
			return Ok();
		}

		[HttpPost("UploadProfileImage")]
		public async Task<IActionResult> UploadProfileImage([FromQuery] string employerEmail, IFormFile file)
		{
			var imageUrl = await _employerService.UploadProfileImageAsync(employerEmail, file);
			if (imageUrl == null) return BadRequest("Upload failed or employer not found.");
			return Ok(new { imageUrl });
		}
	}
}
