namespace LaborMarket.Api.Models.JobModels
{
	public class EditJobModel
	{
		public int JobId { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Company { get; set; } = null!;
		public string Location { get; set; } = null!;
		public DateTime PostedAt { get; set; }
		public int EmployerId { get; set; }
	}
}
