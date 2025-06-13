using System.ComponentModel.DataAnnotations;

namespace LaborMarket.Api.Models
{
	public class JobModel
	{
		[Key]
		public int JobId { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Company { get; set; } = null!;
		public string Location { get; set; } = null!;
		public DateTime PostedAt { get; set; }
		public int EmployerId { get; set; }
		public EmployerModel Employer { get; set; } = null!;
	}
}
