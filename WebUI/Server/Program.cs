using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebUI.Server
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args)
				.Build()
				.InitializeContext()
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