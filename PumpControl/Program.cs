using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PumpControl
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) =>
				{
					services.AddSingleton<DataProvider>();
					services.AddSingleton<WatertankSensor>();
					services.AddHostedService<WaterMonitor>();
				});
	}
}