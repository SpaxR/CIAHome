using System.Threading;
using System.Threading.Tasks;
using CIAHome.Shared.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PumpControl
{
	public class WaterMonitor : BackgroundService
	{
		private readonly ILogger<WaterMonitor> _logger;
		private readonly WatertankSensor       _sensor;
		private readonly DataProvider          _provider;


		public WaterMonitor(ILogger<WaterMonitor> logger, WatertankSensor sensor, DataProvider provider)
		{
			_logger   = logger;
			_sensor   = sensor;
			_provider = provider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_provider.StartListening(stoppingToken);
			_logger.LogInformation("Started Listening for Clients");
			
			while (!stoppingToken.IsCancellationRequested)
			{
				// Measure Tank
				var status = new WatertankStatus
				{
					VolumeFilled = _sensor.GetWaterLevel(),
					VolumeTotal  = 100
				};
				_logger.LogInformation("Status: {Filled} / {Total}", status.VolumeFilled, status.VolumeTotal);

				// Send Data to clients
				await _provider.SendDataAsync(status, stoppingToken);
				_logger.LogInformation("Clients Updated");

				// Every second
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}