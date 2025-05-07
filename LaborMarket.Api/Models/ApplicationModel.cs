using System.ComponentModel.DataAnnotations;

namespace LaborMarket.Api.Models
{
	public class ApplicationModel
	{
		[Key]
		public int ApplicationId { get; set; }
		public int UserId { get; set; }
		public int JobId { get; set; }
		public DateTime ApplicationDate { get; set; }
		public string Status { get; set; } = null!;

		public UserModel User { get; set; } = null!;
		public JobModel Job { get; set; } = null!;
	}
}
