namespace WebUI.Shared
{
	public static class CIAPath
	{
		public const string Api         = "/api";
		public const string ApiRoute    = "/api/{action?}";
		public const string Login       = Api + "/login";
		public const string Logout      = Api + "/logout";
		public const string UserProfile = Api + "/user";
	}
}