using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models;
using LaborMarket.Api.Models.JobApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Services
{
	public class JobApplicationService : IJobApplicationService
	{
		private readonly LaborMarketContext _context;

		public JobApplicationService(LaborMarketContext context)
		{
			_context = context;
		}

		public async Task<ApplicationModel?> CreateApplicationAsync(CreateApplicationModel model)
		{
			var application = new ApplicationModel
			{
				UserId = model.UserId,
				JobId = model.JobId,
				ApplicationDate = DateTime.UtcNow,
				Status = "Pending"
			};

			_context.Applications.Add(application);
			await _context.SaveChangesAsync();
			return application;
		}

		public async Task<List<DisplayApplicationModel>> GetApplicationsForEmployerAsync(string employerEmail)
		{
			var jobApplications = await _context.Applications.Where(app => app.Job.Employer.ContactEmail == employerEmail).ToListAsync();

			var response = jobApplications.Select(a => new DisplayApplicationModel
			{
				ApplicationId = a.ApplicationId,
				ApplicationDate = a.ApplicationDate,
				Status = a.Status,
				UserId = a.UserId,
				UserName = _context.Workers.Find(a.UserId)!.FirstName,
				JobId = a.JobId,
				JobName = _context.Jobs.Find(a.JobId)!.Title,
			}).ToList();

			return response;
		}
	}
}
