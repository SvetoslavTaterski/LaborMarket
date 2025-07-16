using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models;
using LaborMarket.Api.Models.JobApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Services
{
	public class JobApplicationService : IJobApplicationService
	{
		private readonly LaborMarketContext _context;
		private readonly IEmailService _emailService;

		public JobApplicationService(LaborMarketContext context, IEmailService emailService)
		{
			_context = context;
			_emailService = emailService;
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

		public async Task<bool> ChangeApplicationStatusAsync(int applicationId, string newStatus)
		{
			var application = await _context.Applications
			.Include(a => a.User) // make sure you have the user's email
			.FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

			if (application == null)
				return false;

			application.Status = newStatus;
			await _context.SaveChangesAsync();

			// Prepare and send email
			string subject = "Статус на кандидатурата";
			string body = newStatus == "Одобрен"
				? "Вашата кандидатура беше одобрена! Очаквайте да се свържем с вас!"
				: "Вашата кандидатура беше отказана.";

			await _emailService.SendEmailAsync(application.User.Email, subject, body);

			return true;
		}
	}
}
