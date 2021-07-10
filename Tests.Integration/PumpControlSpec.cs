using System;
using System.Threading.Tasks;
using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Pages;
using CIAHome.Client.Services;
using CIAHome.Server;
using CIAHome.Server.Hubs;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MudBlazor.Services;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Integration
{
	public class PumpControlSpec : TestContext, IClassFixture<WebApplicationFactory<Startup>>
	{
		private readonly ITestOutputHelper               _testConsole;
		private readonly HubConnection                   _dataProvider;
		private readonly Mock<ILogger<PumpControlProxy>> _loggerMock = new();


		/// <inheritdoc />
		public PumpControlSpec(ITestOutputHelper testConsole, WebApplicationFactory<Startup> factory)
		{
			_testConsole = testConsole;
			TestServer server = factory.Server;
			server.BaseAddress = new Uri("http://localhost");

			_dataProvider = new HubConnectionBuilder()
							.WithUrl(new Uri(server.BaseAddress, "/hubs/pumpcontrol"),
									 options => options.HttpMessageHandlerFactory = _ => server.CreateHandler())
							.Build();
			testConsole.WriteLine("Build HubConnection for DataProvider");

			var proxyConnection = new HubConnectionBuilder()
								  .WithUrl(new Uri(server.BaseAddress, "/hubs/pumpcontrol"),
										   options => options.HttpMessageHandlerFactory = _ => server.CreateHandler())
								  .Build();
			testConsole.WriteLine("Build HubConnection for SUT-Proxy");


			Services.AddMudServices();
			Services.AddSingleton(server.CreateClient());
			Services.AddScoped<IPumpControlService>(_ => new PumpControlProxy(_loggerMock.Object, proxyConnection));
			testConsole.WriteLine("Added Services");
			testConsole.WriteLine(" - - - - - - - - - - ");
		}

		[Fact]
		public async Task Client_gets_notified_of_Status_updates()
		{
			// Arrange
			var watertankStatus = new WatertankStatus {VolumeFilled = 25, VolumeTotal = 100};
			var pumpStatus      = new PumpStatus {IsRunning         = true};
			var pumpControlComp = RenderComponent<PumpControl>();
			_testConsole.WriteLine("Arrange - Created References");

			// Act - Send new Data
			await _dataProvider.StartAsync();
			await _dataProvider.InvokeAsync(nameof(PumpControlHub.UpdateWatertank), watertankStatus);
			await _dataProvider.InvokeAsync(nameof(PumpControlHub.UpdatePump),      pumpStatus);
			await _dataProvider.StopAsync();
			_testConsole.WriteLine("Act - Send new Data to Client");

			// Assert
			pumpControlComp.WaitForAssertion(
				() => Assert.Equal(25d, pumpControlComp.FindComponent<TankImage>().Instance.FilledPercentage),
				TimeSpan.FromSeconds(3));
			_testConsole.WriteLine("Assert -  Watertank gets updated");

			pumpControlComp.WaitForAssertion(
				() => Assert.True(pumpControlComp.FindComponent<PumpImage>().Instance.IsRunning),
				TimeSpan.FromSeconds(3));
			_testConsole.WriteLine("Assert -  Pump gets updated");
		}
	}
}