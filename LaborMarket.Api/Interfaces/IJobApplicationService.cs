using LaborMarket.Api.Models.JobApplicationModels;

namespace LaborMarket.Api.Interfaces
{
    public interface IJobApplicationService
	{
		Task<ApplicationModel?> CreateApplicationAsync(CreateApplicationModel model);
	}
}
