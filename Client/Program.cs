using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Client.Repositories;
using CIAHome.Client.Services;
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

			builder.Services.AddScoped(_ => new HttpClient
										   {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

			// MudBlazor
			builder.Services.AddMudServices();
			builder.Services.AddMudBlazorDialog();
			builder.Services.AddMudBlazorSnackbar();

			// Blazored.LocalStorage
			builder.Services.AddBlazoredLocalStorage();
			
			builder.Services.AddScoped<IThemeProvider, ThemeProvider>();
			builder.Services.AddScoped<IAsyncRepository<TodoList>, TodoListRepository>();
			builder.Services.AddScoped<IAsyncRepository<Product>, ProductRepository>();
			
			await builder.Build().RunAsync();
		}
	}
}