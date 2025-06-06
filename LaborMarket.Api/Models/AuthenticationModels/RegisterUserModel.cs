﻿namespace LaborMarket.Api.Models.AuthenticationModels
{
	public class RegisterUserModel
	{
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;
		public string Role { get; set; } = null!;
	}
}
