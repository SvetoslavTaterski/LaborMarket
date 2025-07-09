using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using LaborMarket.Api.Interfaces;
using LaborMarket.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Services
{
	public class EmployerService : IEmployerService
	{
		private readonly LaborMarketContext _context;
		private readonly Cloudinary _cloudinary;

		public EmployerService(LaborMarketContext context, Cloudinary cloudinary)
		{
			_context = context;
			_cloudinary = cloudinary;
		}

		public async Task<IEnumerable<EmployerModel>> GetAllEmployersAsync()
			=> await _context.Employers.ToListAsync();

		public async Task<EmployerModel?> GetEmployerByEmailAsync(string email)
			=> await _context.Employers.FirstOrDefaultAsync(u => u.ContactEmail == email);

		public async Task<EmployerModel?> GetEmployerByIdAsync(int id)
			=> await _context.Employers.FirstOrDefaultAsync(u => u.EmployerId == id);

		public async Task<bool> SetEmployerDescriptionAsync(string email, string description)
		{
			var employer = await GetEmployerByEmailAsync(email);
			if (employer == null)
				return false;

			employer.Description = description;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<string?> UploadProfileImageAsync(string email, IFormFile file)
		{
			var employer = await GetEmployerByEmailAsync(email);
			if (employer == null || file == null || file.Length == 0)
				return null;

			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, file.OpenReadStream()),
				Folder = "employer_profile_pics"
			};
			var uploadResult = await _cloudinary.UploadAsync(uploadParams);

			employer.ProfileImageUrl = uploadResult.SecureUrl.ToString();
			await _context.SaveChangesAsync();

			return employer.ProfileImageUrl;
		}
	}
}
