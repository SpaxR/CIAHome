using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using WebUI.Server.Hubs;
using WebUI.Shared.Interfaces;
using WebUI.Shared.Models;
using Xunit;

namespace WebUI.Tests.UnitTests
{
	public class ProviderHubSpec
	{
		private PumpControlHub _sut;

		private PumpControlHub SUT
		{
			get
			{
				_sut ??= new PumpControlHub(_loggerMock.Object) {Clients = _clientsMock.Object};
				return _sut;
			}
		}

		private readonly Mock<ILogger<PumpControlHub>>                 _loggerMock   = new();
		private readonly Mock<IPumpControlCallback>                    _callbackMock = new();
		private readonly Mock<IHubCallerClients<IPumpControlCallback>> _clientsMock  = new();

		public ProviderHubSpec()
		{
			_clientsMock.Setup(clients => clients.All).Returns(_callbackMock.Object);
			_clientsMock.Setup(clients => clients.Caller).Returns(_callbackMock.Object);
		}

		[Fact]
		public async Task WatertankStatus_without_Data_returns_null()
		{
			var status = await SUT.WatertankStatus();

			Assert.Null(status);
		}

		[Fact]
		public async Task PumpStatus_without_Data_returns_Null()
		{
			var status = await SUT.PumpStatus();

			Assert.Null(status);
		}

		[Fact]
		public async Task WatertankStatus_returns_provided_Status()
		{
			var status = new WatertankStatus
			{
				VolumeFilled = 25,
				VolumeTotal  = 75
			};

			await SUT.UpdateWatertank(status);
			var result = await SUT.WatertankStatus();

			Assert.Equal(status, result);
		}

		[Fact]
		public async Task PumpStatus_returns_provided_Status()
		{
			var status = new PumpStatus
			{
				IsRunning = true
			};

			await SUT.UpdatePump(status);
			var result = await SUT.PumpStatus();

			Assert.Equal(status, result);
		}

		[Fact]
		public async Task Updating_WatertankStatus_notifies_all_Clients()
		{
			var status = new WatertankStatus();

			await SUT.UpdateWatertank(status);

			_callbackMock.Verify(cb => cb.UpdateWatertank(status));
		}
		
		[Fact]
		public async Task Updating_PumpStatus_notifies_all_Clients()
		{
			var status = new PumpStatus();

			await SUT.UpdatePump(status);

			_callbackMock.Verify(cb => cb.UpdatePump(status));
		}

	}
}