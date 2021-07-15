using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using WebUI.Client.Components;
using Xunit;

namespace WebUI.Tests.UnitTests.Components
{
	public class StatusOverlaySpec : TestContext
	{
		private IRenderedComponent<StatusOverlay> _sut;

		private IRenderedComponent<StatusOverlay> SUT
		{
			get
			{
				_sut ??= RenderComponent<StatusOverlay>(
					(nameof(StatusOverlay.ChildContent), _childContent),
					(nameof(StatusOverlay.Status), _status),
					(nameof(StatusOverlay.Background), _background));
				return _sut;
			}
		}

		private StatusOverlay.OverlayStatus     _status;
		private StatusOverlay.OverlayBackground _background;
		private RenderFragment                  _childContent;

		public StatusOverlaySpec()
		{
			Services.AddMudServices();
		}

		[Fact]
		public void LoadingStatus_contains_loading_spinner()
		{
			_status = StatusOverlay.OverlayStatus.Loading;

			var spinner = SUT.FindComponent<MudProgressCircular>();

			Assert.NotNull(spinner);
		}

		[Fact]
		public void ErrorStatus_contains_error_icon()
		{
			_status = StatusOverlay.OverlayStatus.Error;

			var icon = SUT.FindComponent<MudIcon>();

			Assert.Equal(Icons.Filled.Error, icon.Instance.Icon);
		}

		[Fact]
		public void AutoBackground_is_Light_while_loading()
		{
			_status = StatusOverlay.OverlayStatus.Loading;

			var overlay = SUT.FindComponent<MudOverlay>();

			Assert.True(overlay.Instance.LightBackground);
		}

		[Fact]
		public void AutoBackground_is_Dark_while_error()
		{
			_status = StatusOverlay.OverlayStatus.Error;

			var overlay = SUT.FindComponent<MudOverlay>();

			Assert.True(overlay.Instance.DarkBackground);
		}

		[Fact]
		public void DarkBackground_is_not_Light_while_loading()
		{
			_status     = StatusOverlay.OverlayStatus.Loading;
			_background = StatusOverlay.OverlayBackground.Dark;

			var overlay = SUT.FindComponent<MudOverlay>();

			Assert.False(overlay.Instance.LightBackground);
		}

		[Fact]
		public void LightBackground_is_not_Dark_while_error()
		{
			_status     = StatusOverlay.OverlayStatus.Error;
			_background = StatusOverlay.OverlayBackground.Light;

			var overlay = SUT.FindComponent<MudOverlay>();

			Assert.False(overlay.Instance.DarkBackground);
		}

		[Fact]
		public void ChildContent_gets_Rendered()
		{
			string rendered = "";
			_childContent = builder =>
			{
				builder.AddContent(0, "Hello, World!");
				rendered = builder.GetFrames().Array.First().MarkupContent;
			};

			Assert.Contains(rendered, SUT.Markup);
		}
	}
}