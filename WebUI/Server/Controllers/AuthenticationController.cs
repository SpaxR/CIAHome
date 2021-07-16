using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Shared;
using WebUI.Shared.Models;

namespace WebUI.Server.Controllers
{
	[ApiController]
	[Route(CIAPath.Api)]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<IdentityUser>   _userManager;
		private readonly SignInManager<IdentityUser> _signinManager;

		public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
		{
			_userManager   = userManager;
			_signinManager = signinManager;
		}

		[Route(CIAPath.Login)]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			if (await _userManager.FindByNameAsync(model.Username) == null)
			{
				return Unauthorized();
			}

			var result =
				await _signinManager.PasswordSignInAsync(model.Username, model.Password, model.Remember, false);

			if (result.Succeeded)
			{
				return Ok();
			}

			return Unauthorized();
		}

		[Route(CIAPath.Logout)]
		public async Task<IActionResult> Logout()
		{
			await _signinManager.SignOutAsync();
			return Ok();
		}

		[Route(CIAPath.UserProfile)]
		public IActionResult UserProfile()
		{
			if (User != null)
			{
				return Ok(new UserProfile
				{
					Username = User.Identity?.Name,
					Claims   = User.Claims.ToDictionary(c => c.Type, c => c.Value)
				});
			}

			return Ok();
		}
	}
}