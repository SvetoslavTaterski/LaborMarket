using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("EmployerController")]
	public class EmployerController : Controller
	{
		private readonly LaborMarketContext _context;

		public EmployerController(LaborMarketContext context)
		{
			_context = context;
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
	}
}
