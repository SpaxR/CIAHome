using System;
using System.Threading.Tasks;
using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Pages;
using CIAHome.Client.Services;
using CIAHome.Server;
using CIAHome.Server.Hubs;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Integration
{
	public class PumpControlSpec : TestContext, IClassFixture<WebApplicationFactory<Startup>>
	{
		private readonly HubConnection _dataProvider;

		/// <inheritdoc />
		public PumpControlSpec(WebApplicationFactory<Startup> factory)
		{
			TestServer server = factory.Server;
			server.BaseAddress = new Uri("http://localhost");

			_dataProvider = new HubConnectionBuilder()
							.WithUrl(new Uri(server.BaseAddress, "/hubs/pumpcontrol"),
									 options => options.HttpMessageHandlerFactory = _ => server.CreateHandler())
							.Build();

			var proxyConnection = new HubConnectionBuilder()
								  .WithUrl(new Uri(server.BaseAddress, "/hubs/pumpcontrol"),
										   options => options.HttpMessageHandlerFactory = _ => server.CreateHandler())
								  .Build();

			Services.AddSingleton(server.CreateClient());
			Services.AddScoped<IPumpControlService>(_ => new PumpControlProxy(proxyConnection));
		}

		[Fact]
		public async Task Client_gets_notified_of_Status_updates()
		{
			// Arrange
			var watertankStatus = new WatertankStatus {VolumeFilled = 25, VolumeTotal = 100};
			var pumpStatus      = new PumpStatus {IsRunning         = true};

			var pumpControlComp = RenderComponent<PumpControl>();

			// Act - Send new Data
			await _dataProvider.StartAsync();
			await _dataProvider.InvokeAsync(nameof(PumpControlHub.UpdateWatertank), watertankStatus);
			await _dataProvider.InvokeAsync(nameof(PumpControlHub.UpdatePump),      pumpStatus);
			await _dataProvider.StopAsync();

			// Assert
			pumpControlComp.WaitForAssertion(
				() => Assert.Equal(
					100 / watertankStatus.VolumeTotal * watertankStatus.VolumeFilled,
					pumpControlComp.FindComponent<TankImage>().Instance.FilledPercentage,
					2), TimeSpan.FromSeconds(3));

			// pumpControlComp.WaitForAssertion(
			// 	() => Assert.Equal(true, pumpControlComp.FindComponent<PumpImage>().Instance.IsRunning),
			// 	TimeSpan.FromSeconds(3));
		}
	}
}