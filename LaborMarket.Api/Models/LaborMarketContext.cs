namespace LaborMarket.Api.Models
{
	using Microsoft.EntityFrameworkCore;

	public class LaborMarketContext : DbContext
	{
		public LaborMarketContext(DbContextOptions<LaborMarketContext> options) : base(options) { }

		public DbSet<UserModel> Users { get; set; }
		public DbSet<JobModel> Jobs { get; set; }
		public DbSet<EmployerModel> Employers { get; set; }
		public DbSet<ApplicationModel> Applications { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ApplicationModel>()
				.HasOne(a => a.User)
				.WithMany()
				.HasForeignKey(a => a.UserId);

			modelBuilder.Entity<ApplicationModel>()
				.HasOne(a => a.Job)
				.WithMany()
				.HasForeignKey(a => a.JobId);

			modelBuilder.Entity<JobModel>()
				.HasOne(j => j.Employer)
				.WithMany()
				.HasForeignKey(j => j.EmployerId);
		}
	}
}
