using System.Linq;
using System.Threading.Tasks;
using CIAHome.Server.Data;
using CIAHome.Shared;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CIAHome.Server.Controllers
{
	[ApiController]
	[Route(CIAPath.Api)]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<CIAUser>   _userManager;
		private readonly SignInManager<CIAUser> _signinManager;

		public AuthenticationController(UserManager<CIAUser> userManager, SignInManager<CIAUser> signinManager)
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
				await _userManager.CreateAsync(new CIAUser
				{
					UserName = model.Username
				}, model.Password);
			}

			var result =
				await _signinManager.PasswordSignInAsync(model.Username, model.Password, model.Remember, false);

			if (result.Succeeded)
			{
				return Redirect(model.ReturnUrl ?? "/");
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