using System.Threading.Tasks;
using CIAHome.Server.Data;
using CIAHome.Shared;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CIAHome.Server.Controllers
{
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<CIAUser>   _userManager;
		private readonly SignInManager<CIAUser> _signinManager;

		public AuthenticationController(UserManager<CIAUser> userManager, SignInManager<CIAUser> signinManager)
		{
			_userManager   = userManager;
			_signinManager = signinManager;
		}

		[HttpPost(CIAPath.Login)]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			
			var result = await _signinManager.PasswordSignInAsync(model.Username, model.Password, model.Remember, false);

			if (result.Succeeded)
			{
				return Ok();
			}

			return Unauthorized();
		}
	}
}