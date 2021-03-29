using System.Collections.Generic;

namespace CIAHome.Shared.Models
{
	public class UserProfile
	{
		public string         Username { get; set; }
		public IDictionary<string,string> Claims   { get; set; }
	}
}