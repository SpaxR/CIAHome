using System;
using Bunit;
using CIAHome.Client.Pages;
using CIAHome.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests.Pages
{
	public sealed class SettingsSpec : TestContext
	{
		private IRenderedComponent<Settings> _sut;

		private readonly Mock<IThemeProvider> _themeProviderMock = new();


		public SettingsSpec()
		{
			Services.AddScoped(_ => _themeProviderMock.Object);
		}

		private void CreateSUT()
		{
			_sut = RenderComponent<Settings>();
		}

		[Fact]
		public void ThemeSwitch_withDarkTheme_isActivated()
		{
			_themeProviderMock.Setup(tp => tp.CurrentTheme).Returns(ThemeProvider.DarkTheme);
			CreateSUT();
			var themeSwitch = _sut.FindComponent<MudSwitch<bool>>();

			Assert.True(themeSwitch.Instance.Checked);
		}

		[Fact]
		public void ThemeSwitch_withLightTheme_isNotActivated()
		{
			_themeProviderMock.Setup(tp => tp.CurrentTheme).Returns(ThemeProvider.LightTheme);
			CreateSUT();
			var themeSwitch = _sut.FindComponent<MudSwitch<bool>>();

			Assert.False(themeSwitch.Instance.Checked);
		}

		[Fact]
		public void ToggleTheme_WithLightTheme_SetsDarkTheme()
		{
			_themeProviderMock.Setup(tp => tp.CurrentTheme).Returns(ThemeProvider.LightTheme);
			CreateSUT();
			var themeSwitch = _sut.FindComponent<MudSwitch<bool>>();

			themeSwitch.Find("input").Change(true);

			_themeProviderMock.Verify(tp => tp.ChangeTheme(ThemeProvider.DarkTheme), Times.Once);
		}

		[Fact]
		public void ToggleTheme_WithDarkTheme_SetsLightTheme()
		{
			_themeProviderMock.Setup(tp => tp.CurrentTheme).Returns(ThemeProvider.DarkTheme);
			CreateSUT();
			var themeSwitch = _sut.FindComponent<MudSwitch<bool>>();

			themeSwitch.Find("input").Change(false);

			_themeProviderMock.Verify(tp => tp.ChangeTheme(ThemeProvider.LightTheme), Times.Once);
		}

		[Fact]
		public void ThemeChange_triggers_Render()
		{
			CreateSUT();

			_sut.InvokeAsync(() => _themeProviderMock.Raise(tp => tp.ThemeChanged += null, EventArgs.Empty));

			Assert.Equal(2, _sut.RenderCount);
		}
	}
}