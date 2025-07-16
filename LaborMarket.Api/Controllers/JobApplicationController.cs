using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models.JobApplicationModels;
using Microsoft.AspNetCore.Mvc;

namespace LaborMarket.Api.Controllers
{
    [ApiController]
	[Route("JobApplicationController")]
	public class JobApplicationController : Controller
	{
		private readonly IJobApplicationService _jobApplicationService;

		public JobApplicationController(IJobApplicationService jobApplicationService)
		{
			_jobApplicationService = jobApplicationService;
		}

		[HttpPost("CreateJobApplication")]
		public async Task<IActionResult> CreateJobApplication([FromBody] CreateApplicationModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var application = await _jobApplicationService.CreateApplicationAsync(model);

			if (application == null)
				return NotFound("User or Job does not exist.");

			return Ok();
		}

		[HttpGet("GetEmployerApplicationsByEmployerEmail")]
		public async Task<IActionResult> GetEmployerApplicationsByEmployerEmail([FromQuery] string employerEmail)
		{
			if(employerEmail == null)
				return BadRequest();

			var applications = await _jobApplicationService.GetApplicationsForEmployerAsync(employerEmail);

			if (applications == null)
				return BadRequest();

			return Ok(applications);
		}

		[HttpPut("ChangeApplicationStatus")]
		public async Task<IActionResult> ChangeApplicationStatus([FromQuery] int applicationId, string newStatus)
		{
			var result = await _jobApplicationService.ChangeApplicationStatusAsync(applicationId, newStatus);

			if (!result)
				return NotFound("Application not found.");

			return Ok();
		}
	}
}
