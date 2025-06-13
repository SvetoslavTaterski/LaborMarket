namespace LaborMarket.Api.Models.JobModels
{
	public class CreateJobModel
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Location { get; set; } = null!;
		public string EmployerEmail { get; set; } = null!;
	}
}
