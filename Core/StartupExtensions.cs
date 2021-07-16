using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CIAHome.Core
{
	public static class StartupExtensions
	{
		public static IServiceCollection AddCoreServices(this IServiceCollection services)
		{
			services.AddMediatR(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}