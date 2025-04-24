using Microsoft.AspNetCore.Mvc;

namespace LaborMarket.Api.Controllers
{
	[ApiController]
	[Route("main")]
	public class MainController : Controller
	{
		[HttpGet("ping")]
		public IActionResult Ping()
		{
			return Ok(new { message = "SVEEEEEEEEEEEEEEEEEEE!" });
		}
	}
}
