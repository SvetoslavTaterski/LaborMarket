using System.ComponentModel.DataAnnotations;

namespace LaborMarket.Api.Models
{
	public class UserModel
	{
		[Key]
		public int UserId { get; set; }
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}
