using System;
using System.Threading.Tasks;
using CIAHome.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using CIAHome.Client.Repositories;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
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

			builder.Services.AddAuthorizationCore();

			// builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
			builder.Services.AddHttpClient(string.Empty,
										   client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
			
			// MudBlazor
			builder.Services.AddMudServices();
			builder.Services.AddMudBlazorDialog();
			builder.Services.AddMudBlazorSnackbar();
			
			// Blazored.LocalStorage
			builder.Services.AddBlazoredLocalStorage();
			
			// CIA Service
			builder.Services.AddScoped<AuthenticationStateProvider, CIAuthenticationService>();
			builder.Services.AddScoped<IAuthenticationService, CIAuthenticationService>();
			builder.Services.AddScoped<IThemeProvider, ThemeProvider>();
			builder.Services.AddScoped<IAsyncRepository<TodoList>, TodoListRepository>();
			
			await builder.Build().RunAsync();
		}
	}
}