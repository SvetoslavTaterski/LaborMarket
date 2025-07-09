using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models.JobModels;
using LaborMarket.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Services
{
	public class JobService : IJobService
	{
		private readonly LaborMarketContext _context;

		public JobService(LaborMarketContext context)
		{
			_context = context;
		}

#pragma warning disable CS8601 //This is ignored because it will be handled in controller
		public async Task<List<JobModel>> GetAllJobsAsync()
		{
			return await _context.Jobs
				.Include(job => job.Employer)
				.Select(job => new JobModel
				{
					JobId = job.JobId,
					Title = job.Title,
					Description = job.Description,
					Company = job.Company,
					Location = job.Location,
					PostedAt = job.PostedAt,
					EmployerId = job.EmployerId,
					Employer = job.Employer == null ? null : new EmployerModel
					{
						ContactEmail = job.Employer.ContactEmail
					}
				})
				.ToListAsync();
		}

		public async Task<JobModel?> GetJobByIdAsync(int jobId)
		{
			return await _context.Jobs.FindAsync(jobId);
		}

		public async Task<JobModel?> CreateJobAsync(CreateJobModel jobModel)
		{
			var employer = await _context.Employers.FirstOrDefaultAsync(u => u.ContactEmail == jobModel.EmployerEmail);
			if (employer == null)
				return null;

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

			return newJob;
		}

		public async Task<bool> EditJobAsync(EditJobModel jobModel)
		{
			var jobToEdit = await _context.Jobs.FindAsync(jobModel.JobId);
			if (jobToEdit == null)
				return false;

			jobToEdit.Title = jobModel.Title;
			jobToEdit.Description = jobModel.Description;
			jobToEdit.Location = jobModel.Location;

			_context.Jobs.Update(jobToEdit);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> DeleteJobAsync(int jobId)
		{
			var job = await _context.Jobs.FindAsync(jobId);
			if (job == null)
				return false;

			_context.Jobs.Remove(job);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
