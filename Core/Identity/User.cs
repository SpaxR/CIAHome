using Microsoft.AspNetCore.Identity;

namespace CIAHome.Core
{
	public class User : IdentityUser
	{
		public string UserName { get; set; }
	}
}