using LaborMarket.Api.Models;

namespace LaborMarket.Api.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserModel>> GetAllUsersAsync();
		Task<UserModel?> GetUserByEmailAsync(string email);
		Task<UserModel?> GetUserByIdAsync(int userId);
		Task<bool> SetUserCVAsync(string userEmail, string cv);
		Task<string?> UploadProfileImageAsync(string userEmail, IFormFile file);
	}
}
