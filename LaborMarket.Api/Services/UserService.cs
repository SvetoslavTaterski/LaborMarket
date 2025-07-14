using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Services
{
	public class UserService : IUserService
	{
		private readonly LaborMarketContext _context;
		private readonly Cloudinary _cloudinary;

		public UserService(LaborMarketContext context, Cloudinary cloudinary)
		{
			_context = context;
			_cloudinary = cloudinary;
		}

		public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
		{
			return await _context.Workers.ToListAsync();
		}

		public async Task<UserModel?> GetUserByEmailAsync(string email)
		{
			return await _context.Workers.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<UserModel?> GetUserByIdAsync(int userId)
		{
			return await _context.Workers.FirstOrDefaultAsync(u => u.UserId == userId);
		}

		public async Task<bool> SetUserCVAsync(string userEmail, string cv)
		{
			var user = await _context.Workers.FirstOrDefaultAsync(u => u.Email == userEmail);
			if (user == null)
				return false;

			user.CV = cv;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<string?> UploadProfileImageAsync(string userEmail, IFormFile file)
		{
			var user = await _context.Workers.FirstOrDefaultAsync(u => u.Email == userEmail);
			if (user == null)
				return null;

			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, file.OpenReadStream()),
				Folder = "user_profile_pics"
			};

			var uploadResult = await _cloudinary.UploadAsync(uploadParams);

			user.ProfileImageUrl = uploadResult.SecureUrl.ToString();
			await _context.SaveChangesAsync();

			return user.ProfileImageUrl;
		}
	}
}
