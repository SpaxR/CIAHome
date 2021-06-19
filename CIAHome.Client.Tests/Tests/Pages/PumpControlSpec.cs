using System;
using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Pages;
using CIAHome.Shared.EventArgs;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class PumpControlSpec : TestContext
	{
		private readonly Mock<IPumpControlService> _pumpControlMock = new();
		private readonly WatertankStatus           _watertankStatus = new(){VolumeTotal = 100};
		private readonly PumpStatus                _pumpStatus      = new();

		private IRenderedComponent<PumpControl> _sut;

		private IRenderedComponent<PumpControl> SUT
		{
			get
			{
				_sut ??= RenderComponent<PumpControl>();
				return _sut;
			}
		}

		public PumpControlSpec()
		{
			_pumpControlMock.Setup(control => control.WatertankStatus()).ReturnsAsync(_watertankStatus);
			_pumpControlMock.Setup(control => control.PumpStatus()).ReturnsAsync(_pumpStatus);

			Services.AddScoped(_ => _pumpControlMock.Object);
		}

		[Fact]
		public void OnInitialized_subscribes_to_Watertank_updates()
		{
			_ = SUT;

			_pumpControlMock.VerifyAdd(service => service.WatertankUpdated += It.IsAny<EventHandler<WatertankEventArgs>>());
		}

		[Fact]
		public void OnInitialized_subscribes_to_Pump_updates()
		{
			_ = SUT;

			_pumpControlMock.VerifyAdd(service => service.PumpUpdated += It.IsAny<EventHandler<PumpEventArgs>>());
		}

		[Fact]
		public void OnInitialized_fetches_initial_Status()
		{
			_ = SUT;

			_pumpControlMock.Verify(control => control.WatertankStatus());
			_pumpControlMock.Verify(control => control.PumpStatus());
		}

		[Fact]
		public void contains_TankImage()
		{
			var image = SUT.FindComponent<TankImage>();

			Assert.NotNull(image);
		}

		[Fact]
		public void sets_TankImage_to_current_status_of_Watertank()
		{
			_watertankStatus.VolumeFilled = 50;
			
			var tankImage = SUT.FindComponent<TankImage>();

			Assert.Equal(50, tankImage.Instance.FilledPercentage);
		}

		[Fact]
		public void Updating_Watertank_updates_TankImage()
		{
			_watertankStatus.VolumeFilled = 50;
			var tankImage = SUT.FindComponent<TankImage>();

			SUT.InvokeAsync(() =>
			{
				_pumpControlMock.Raise(control => control.WatertankUpdated += null,
									   new WatertankEventArgs(new WatertankStatus
									   {
										   VolumeTotal  = 100,
										   VolumeFilled = 20
									   }));
			});

			Assert.Equal(20, tankImage.Instance.FilledPercentage);

		}
	}
}