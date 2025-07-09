using LaborMarket.Api.Models.JobModels;
using LaborMarket.Api.Models;

namespace LaborMarket.Api.Interfaces
{
	public interface IJobService
	{
		Task<List<JobModel>> GetAllJobsAsync();
		Task<JobModel?> GetJobByIdAsync(int jobId);
		Task<JobModel?> CreateJobAsync(CreateJobModel jobModel);
		Task<bool> EditJobAsync(EditJobModel jobModel);
		Task<bool> DeleteJobAsync(int jobId);
	}
}
