using CIA.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CIAHome.Server
{
	public static class WebHostExtensions
	{
		public static IHost InitializeContext<T>(this IHost host) where T : ContextBase<T>
		{
			using var scope = host.Services.CreateScope();

			scope.ServiceProvider
				 .GetRequiredService<T>()
				 .Initialize();

			return host;
		}

		public static IHost InitializeUserContext(this IHost host)
		{
			using var scope       = host.Services.CreateScope();
			var       userManager = scope.ServiceProvider.GetRequiredService<UserManager<CIAUser>>();

			scope.ServiceProvider
				 .GetRequiredService<CIAContext>()
				 .Initialize(userManager);

			return host;
		}
	}
}