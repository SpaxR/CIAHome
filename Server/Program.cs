using CIA.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CIAHome.Server
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args)
				.Build()
				.InitializeUserContext()
				.InitializeContext<RemoteControlContext>()
				.InitializeContext<TodoContext>()
				.InitializeContext<PantryContext>()
				.Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host
				   .CreateDefaultBuilder(args)
				   .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
		}
	}
}