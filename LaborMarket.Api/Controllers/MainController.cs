using LaborMarket.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("main")]
	public class MainController : Controller
	{
		private readonly LaborMarketContext _context;

		public MainController(LaborMarketContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		[HttpGet("ping")]
		public IActionResult Ping()
		{
			return Ok(new { message = "SVEEEEEEEEEEEEEEEEEEE!" });
		}
	}
}
