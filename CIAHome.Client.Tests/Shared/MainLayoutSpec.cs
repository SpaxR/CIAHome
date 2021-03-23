using System.Linq;
using Bunit;
using CIAHome.Client.Shared;
using MudBlazor;
using MudBlazor.Services;
using Xunit;

namespace CIAHome.Client.Tests.Shared
{
	public class MainLayoutSpec : TestContext
	{
		private IRenderedComponent<MainLayout> _sut;

		private void CreateSUT()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
			Services.AddMudServices();
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
	}
}