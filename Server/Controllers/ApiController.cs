using Microsoft.AspNetCore.Mvc;

namespace CIAHome.Server.Controllers
{
	public class ApiController : ControllerBase
	{
		public IActionResult Fallback()
		{
			return NotFound();
		}
	}
}