namespace LaborMarket.Api.Models.JobApplicationModels
{
	public class DisplayApplicationModel
	{
		public int ApplicationId { get; set; }
		public DateTime ApplicationDate { get; set; }
		public string Status { get; set; } = null!;
		public int UserId { get; set; }
		public string UserName { get; set; } = null!;
		public int JobId { get; set; }
		public string JobName { get; set; } = null!;
	}
}
