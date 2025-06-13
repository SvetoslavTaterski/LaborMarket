using LaborMarket.Api.Models;
using LaborMarket.Api.Models.JobModels;
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

		[HttpPost("CreateJob")]
		public async Task<ActionResult<JobModel>> CreateJob(CreateJobModel jobModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var employer = await _context.Employers.FirstOrDefaultAsync(u => u.ContactEmail == jobModel.EmployerEmail);

			if (employer == null)
				throw new Exception($"There is no such user with email: {jobModel.EmployerEmail}");


			var newJob = new JobModel()
			{
				Title = jobModel.Title,
				Description = jobModel.Description,
				Location = jobModel.Location,
				PostedAt = DateTime.UtcNow,
				Company = employer.CompanyName,
				Employer = employer,
				EmployerId = employer.EmployerId
			};

			await _context.Jobs.AddAsync(newJob);
			await _context.SaveChangesAsync();

			return Ok(newJob);
		}
	}
}
