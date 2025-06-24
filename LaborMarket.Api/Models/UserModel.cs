using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LaborMarket.Api.Models
{
	public class UserModel
	{
		[Key]
		[JsonIgnore]
		public int UserId { get; set; }
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public string? CV { get; set; }
	}
}
