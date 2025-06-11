using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("UserController")]
	public class UserController : Controller
	{
		private readonly LaborMarketContext _context;

		public UserController(LaborMarketContext context)
		{
			_context = context;
		}

		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
		{
			return await _context.Workers.ToListAsync();
		}

		[HttpGet("GetUserByEmail")]
		public async Task<ActionResult<UserModel>> GetUserByEmail(string userEmail)
		{
			var user = await _context.Workers.FirstOrDefaultAsync(u => u.Email == userEmail);

			if (user == null)
				return NotFound();

			return user;
		}
	}
}
