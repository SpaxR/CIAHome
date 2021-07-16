using System.Collections.Generic;

namespace WebUI.Shared.Models
{
	public class UserProfile
	{
		public string Username { get; set; }

		public IDictionary<string, string> Claims { get; set; }
			= new Dictionary<string, string>();
	}
}