namespace LaborMarket.Api.Models
{
    using LaborMarket.Api.Models.AuthenticationModels;
    using LaborMarket.Api.Models.JobApplicationModels;
    using LaborMarket.Api.Models.JobModels;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class LaborMarketContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		public LaborMarketContext(DbContextOptions<LaborMarketContext> options) : base(options) { }

		public DbSet<UserModel> Workers { get; set; }
		public DbSet<JobModel> Jobs { get; set; }
		public DbSet<EmployerModel> Employers { get; set; }
		public DbSet<ApplicationModel> Applications { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

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
