using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models;
using LaborMarket.Api.Models.JobModels;
using Microsoft.AspNetCore.Mvc;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("JobController")]
	public class JobController : Controller
	{
		private readonly IJobService _jobService;

		public JobController(IJobService jobService)
		{
			_jobService = jobService;
		}

		[HttpGet("GetAllJobs")]
		public async Task<ActionResult<IEnumerable<JobModel>>> GetAllJobs()
		{
			var jobs = await _jobService.GetAllJobsAsync();
			return Ok(jobs);
		}

		[HttpGet("GetJobById")]
		public async Task<ActionResult<JobModel>> GetJobById(int jobId)
		{
			var foundJob = await _jobService.GetJobByIdAsync(jobId);
			if (foundJob == null)
				return NotFound();

			return foundJob;
		}

		[HttpPost("CreateJob")]
		public async Task<ActionResult<JobModel>> CreateJob(CreateJobModel jobModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newJob = await _jobService.CreateJobAsync(jobModel);
			if (newJob == null)
				return BadRequest($"There is no such user with email: {jobModel.EmployerEmail}");

			return Ok(newJob);
		}

		[HttpPut("EditJob")]
		public async Task<IActionResult> EditJob(EditJobModel jobModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _jobService.EditJobAsync(jobModel);
			if (!result)
				return NotFound();

			return Ok();
		}

		[HttpDelete("DeleteJob")]
		public async Task<IActionResult> DeleteJob(int jobId)
		{
			var result = await _jobService.DeleteJobAsync(jobId);
			if (!result)
				return NotFound();

			return NoContent();
		}
	}
}
