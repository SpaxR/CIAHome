using CIAHome.Server.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CIAHome.Server.Data;
using CIAHome.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CIAHome.Server.Hubs;

namespace CIAHome.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<CIAContext>(_ => _.UseInMemoryDatabase("CIADB"));

			services.AddIdentity<CIAUser, IdentityRole>()
					.AddEntityFrameworkStores<CIAContext>();

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit           = false;
				options.Password.RequireLowercase       = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase       = false;
				options.Password.RequiredLength         = 1;
			});

			services.AddSignalR();
			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<PumpControlHub>("/hubs/pumpcontrol");
				endpoints.MapRazorPages();
				endpoints.MapControllers();endpoints.MapFallbackToController(CIAPath.ApiRoute, nameof(ApiController.Fallback), "api");
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}