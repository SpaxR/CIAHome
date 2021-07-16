using System;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using WebUI.Client.Components;
using WebUI.Client.Pages;
using WebUI.Shared.Interfaces;
using WebUI.Shared.Models;
using Xunit;

namespace WebUI.Tests.UnitTests.Pages
{
	public class PumpControlSpec : TestContext
	{
		private readonly Mock<IPumpControlService> _pumpControlMock = new();
		private readonly WatertankStatus           _watertankStatus = new() {VolumeTotal = 100};
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

			Services.AddMudServices();
			Services.AddScoped(_ => _pumpControlMock.Object);
		}

		[Fact]
		public void OnInitialized_subscribes_to_Watertank_updates()
		{
			_ = SUT;

			_pumpControlMock.VerifyAdd(service => service.WatertankUpdated +=
										   It.IsAny<EventHandler<WatertankEventArgs>>());
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
			SUT.Render();

			SUT.WaitForAssertion(() =>
			{
				_pumpControlMock.Verify(control => control.WatertankStatus());
				_pumpControlMock.Verify(control => control.PumpStatus());
			});
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

		[Fact]
		public void Exception_while_loading_WatertankStatus_sets_Overlay_to_Error()
		{
			_pumpControlMock.Setup(pcm => pcm.WatertankStatus()).ThrowsAsync(new Exception());

			var overlay = SUT.FindComponent<StatusOverlay>();

			Assert.Equal(StatusOverlay.OverlayStatus.Error, overlay.Instance.Status);
		}
		
		[Fact]
		public void contains_PumpImage()
		{
			var image = SUT.FindComponent<PumpImage>();

			Assert.NotNull(image);
		}

		[Fact]
		public async Task Updating_Pump_updates_PumpImage()
		{
			var statusEventArgs = new PumpEventArgs(new PumpStatus {IsRunning = true});
			var pumpImage       = SUT.FindComponent<PumpImage>();

			await SUT.InvokeAsync(() => _pumpControlMock.Raise(pc => pc.PumpUpdated += null, statusEventArgs));

			Assert.Equal(true, pumpImage.Instance.IsRunning);
		}
		
		[Fact]
		public void Exception_while_loading_PumpStatus_sets_Overlay_to_Error()
		{
			_pumpControlMock.Setup(pcm => pcm.PumpStatus()).ThrowsAsync(new Exception());

			var overlay = SUT.FindComponents<StatusOverlay>()[1];

			Assert.Equal(StatusOverlay.OverlayStatus.Error, overlay.Instance.Status);
		}

	}
}