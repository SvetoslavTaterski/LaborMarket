﻿namespace LaborMarket.Api.Interfaces
{
	public interface IEmailService
	{
		Task SendEmailAsync(string to, string subject, string body);
	}
}
