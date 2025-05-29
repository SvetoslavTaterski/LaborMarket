namespace LaborMarket.Api.Models.AuthenticationModels
{
	public class RegisterEmployerModel
	{
		public string CompanyName { get; set; } = null!;
		public string ContactEmail { get; set; } = null!;
		public string ContactPhone { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Role { get; set; } = null!;
	}
}
