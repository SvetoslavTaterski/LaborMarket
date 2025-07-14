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
	}
}
