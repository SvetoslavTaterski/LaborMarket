using LaborMarket.Api.Models;

namespace LaborMarket.Api.Interfaces
{
	public interface IEmployerService
	{
		Task<IEnumerable<EmployerModel>> GetAllEmployersAsync();
		Task<EmployerModel?> GetEmployerByEmailAsync(string email);
		Task<EmployerModel?> GetEmployerByIdAsync(int id);
		Task<bool> SetEmployerDescriptionAsync(string email, string description);
		Task<string?> UploadProfileImageAsync(string email, IFormFile file);
	}
}
