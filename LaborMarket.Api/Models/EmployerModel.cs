using System.ComponentModel.DataAnnotations;

namespace LaborMarket.Api.Models
{
	public class EmployerModel
	{
		[Key]
		public int EmployerId { get; set; }
		public string CompanyName { get; set; } = null!;
		public string ContactEmail { get; set; } = null!;
		public string ContactPhone { get; set; } = null!;
	}
}
