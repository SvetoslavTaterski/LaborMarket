using System.ComponentModel.DataAnnotations;

namespace LaborMarket.Api.Models.JobApplicationModels
{
	public class CreateApplicationModel
	{
		public DateTime ApplicationDate { get; set; }
		public string Status { get; set; } = null!;
		public int UserId { get; set; }
		public int JobId { get; set; }
	}
}
