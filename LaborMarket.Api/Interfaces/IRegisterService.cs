using LaborMarket.Api.Models.AuthenticationModels;
using Microsoft.AspNetCore.Identity;

namespace LaborMarket.Api.Interfaces
{
	public interface IRegisterService
	{
		Task<IdentityResult> RegisterUserAsync(RegisterUserModel model);
		Task<IdentityResult> RegisterEmployerAsync(RegisterEmployerModel model);
		Task<(bool Success, string Email, string Role, string Error)> LoginAsync(LoginModel model);
		Task LogoutAsync();
	}
}
