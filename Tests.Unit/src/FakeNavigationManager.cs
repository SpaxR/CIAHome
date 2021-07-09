using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit
{
	public class FakeNavigationManager : NavigationManager
	{
		private readonly ITestRenderer _renderer;

		public FakeNavigationManager(ITestRenderer renderer)
		{
			_renderer = renderer;
			Initialize("http://localhost/", "http://localhost/");
		}

		protected override void NavigateToCore(string uri, bool forceLoad)
		{
			Uri = ToAbsoluteUri(uri).ToString();

			_renderer.Dispatcher.InvokeAsync(() => NotifyLocationChanged(false));
		}
	}
}