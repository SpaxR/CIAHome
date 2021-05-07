using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace PumpControl
{
	public class WaterMonitorSpec
	{
		private WaterMonitor _sut;

		private readonly Mock<ILogger<WaterMonitor>> _loggerMock = new();

		private WaterMonitor SUT
		{
			get
			{
				_sut ??= new WaterMonitor(_loggerMock.Object);
				return _sut;
			}
		}

		[Fact]
		public void SanityCheck()
		{
			Assert.NotNull(SUT);
		}
		

	}
}