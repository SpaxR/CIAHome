using System.Linq;
using Bunit;
using CIAHome.Client.Components;
using Xunit;

namespace Tests.Unit
{
	public class PumpImageSpec : TestContext
	{
		private IRenderedComponent<PumpImage> _sut;
		private bool                          _isRunning;

		private IRenderedComponent<PumpImage> SUT
		{
			get
			{
				_sut ??= RenderComponent<PumpImage>((nameof(PumpImage.IsRunning), _isRunning));
				return _sut;
			}
		}

		[Fact]
		public void contains_image_of_pump()
		{
			var images = SUT.FindAll("img");
			var paths  = images.Select(img => img.GetAttribute("src"));

			Assert.Contains(paths, path => path.ToLower().Contains("pump"));
		}

		[Fact]
		public void contains_image_of_water()
		{
			var images = SUT.FindAll("img");
			var paths  = images.Select(img => img.GetAttribute("src"));

			Assert.Contains(paths, path => path.ToLower().Contains("water"));
		}

		[Theory]
		[InlineData(true), InlineData(false)]
		public void changing_Status_shows_Water_image(bool isRunning)
		{
			_isRunning = isRunning;

			var    image          = SUT.Find("img");
			string style          = image.GetAttribute("style");
			int    maskPercentage = isRunning ? 0 : 100;


			Assert.Contains($"{maskPercentage}% 0 0 0", style);
		}
	}
}