using System;
using System.Linq;
using Bunit;
using CIAHome.Client.Services;
using CIAHome.Client.Shared;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class MainLayoutSpec : TestContext
	{
		private          IRenderedComponent<MainLayout> _sut;
		private readonly Mock<IThemeProvider>           _themeProviderMock = new();


		public MainLayoutSpec()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
			
			Services.AddMudServices();
			Services.AddMudBlazorDialog();
			Services.AddMudBlazorSnackbar();
			
			Services.AddScoped(_ => Mock.Of<INavigationInterception>());
			Services.AddScoped(_ => _themeProviderMock.Object);
		}
		
		private void CreateSUT()
		{
			_sut = RenderComponent<MainLayout>();
		}

		[Fact]
		public void contains_appbar()
		{
			CreateSUT();
			_sut.FindComponent<MudAppBar>();
		}

		[Fact]
		public void contains_drawer()
		{
			CreateSUT();
			_sut.FindComponent<MudDrawer>();
		}

		[Fact]
		public void AppBar_contains_title()
		{
			CreateSUT();
			var title = _sut.FindComponent<MudAppBar>()
							.FindComponents<MudButton>()
							.FirstOrDefault(b => b.Markup.Contains("CIAHome"));

			Assert.NotNull(title);
		}

		[Fact]
		public void drawer_defaults_to_closed()
		{
			CreateSUT();
			var drawer = _sut.FindComponent<MudDrawer>();

			Assert.False(drawer.Instance.Open);
		}

		[Fact]
		public void click_menu_toggles_drawer()
		{
			CreateSUT();
			var drawer = _sut.FindComponent<MudDrawer>();
			var menu   = _sut.FindComponent<MudIconButton>();
			menu.Find("button").Click();

			Assert.True(drawer.Instance.Open);
		}

		[Fact]
		public void title_links_to_index()
		{
			CreateSUT();
			var title = _sut
						.FindComponents<MudButton>()
						.FirstOrDefault(b => b.Markup.Contains("CIAHome"));

			Assert.Equal("/", title?.Instance.Link);
		}
		
		[Fact]
		public void sets_MudThemeProvider_to_current_theme()
		{
			var theme = new MudTheme();
			_themeProviderMock.Setup(tp => tp.CurrentTheme).Returns(theme);
			CreateSUT();

			var mudThemeProvider = _sut.FindComponent<MudThemeProvider>();

			Assert.Equal(theme, mudThemeProvider.Instance.Theme);
		}

		[Fact]
		public void ThemeChanged_triggers_Render()
		{
			CreateSUT();
			
			_sut.InvokeAsync(() => _themeProviderMock.Raise(tp => tp.ThemeChanged += null, EventArgs.Empty));

			Assert.Equal(2, _sut.RenderCount);
		}
	}
}