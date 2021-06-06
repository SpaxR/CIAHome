using System.Threading.Tasks;
using CIAHome.Shared.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CIAHome.Server.Hubs
{
	public class ProviderHubSpec
	{
		private ProviderHub _sut;

		private ProviderHub SUT
		{
			get
			{
				_sut ??= new ProviderHub(_loggerMock.Object);
				return _sut;
			}
		}

		private readonly Mock<ILogger<ProviderHub>> _loggerMock = new();

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
	}
}