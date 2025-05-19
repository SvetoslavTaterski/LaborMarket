using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("JobController")]
	public class JobController : Controller
	{
		private readonly LaborMarketContext _context;

		public JobController(LaborMarketContext context)
		{
			_context = context;
		}

		[HttpGet("GetAllJobs")]
		public async Task<ActionResult<IEnumerable<JobModel>>> GetAllJobs()
		{
			return await _context.Jobs.ToListAsync();
		}
	}
}
