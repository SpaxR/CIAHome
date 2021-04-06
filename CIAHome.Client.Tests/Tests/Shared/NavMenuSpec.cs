using Bunit;
using CIAHome.Client.Shared;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class NavMenuSpec : TestContext
	{
		private IRenderedComponent<NavMenu> _sut;

		private void CreateSUT()
		{
			_sut = RenderComponent<NavMenu>();
		}

		[Theory]
		[InlineData("/todos")]
		[InlineData("/pantry")]
		[InlineData("/pumpcontrol")]
		public void contains_button_foreach_page(string page)
		{
			CreateSUT();
			
			var buttons = _sut.FindComponents<MudButton>();

			Assert.Contains(buttons, b => b.Instance.Link.Equals(page));
		}
	}
}