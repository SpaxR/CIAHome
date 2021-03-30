using System;
using System.Net.Http;
using System.Threading.Tasks;
using CIAHome.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace CIAHome.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(_ => new HttpClient
										   {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

			builder.Services.AddMudServices();
			builder.Services.AddMudBlazorDialog();
			builder.Services.AddMudBlazorSnackbar();

			builder.Services.AddScoped<IThemeProvider, ThemeProvider>();

			await builder.Build().RunAsync();
		}
	}
}