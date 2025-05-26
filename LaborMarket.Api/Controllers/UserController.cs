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
			return await _context.Users.ToListAsync();
		}

		[HttpPost("CreateUser")]
		public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModel user)
		{
			if (user == null)
			{
				return BadRequest("User data is invalid.");
			}

			try
			{
				_context.Users.Add(user);
				await _context.SaveChangesAsync();
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
