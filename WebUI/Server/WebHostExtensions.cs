using CIA.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebUI.Server
{
	public static class WebHostExtensions
	{
		public static IHost InitializeContext(this IHost host)
		{
			using var scope = host.Services.CreateScope();
	
			scope.ServiceProvider
				 .GetRequiredService<DatabaseContext>()
				 .InitializeAsync()
				 .Wait();
	
			return host;
		}
	}
}