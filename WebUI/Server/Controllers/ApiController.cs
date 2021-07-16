using Microsoft.AspNetCore.Mvc;

namespace WebUI.Server.Controllers
{
	public class ApiController : ControllerBase
	{
		public IActionResult Fallback()
		{
			return NotFound();
		}
	}
}