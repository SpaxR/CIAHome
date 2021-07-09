using System.Linq;
using Bunit;
using CIAHome.Client.Components;
using Xunit;

namespace Tests.Unit
{
	public class TankImageSpec : TestContext
	{
		private int                           _filledPercentage;
		private IRenderedComponent<TankImage> _sut;

		private IRenderedComponent<TankImage> SUT
		{
			get
			{
				_sut ??= RenderComponent<TankImage>((nameof(TankImage.FilledPercentage), _filledPercentage));
				return _sut;
			}
		}

		[Fact]
		public void contains_image_of_filled_tank()
		{
			var images = SUT.FindAll("img");
			var paths  = images.Select(img => img.GetAttribute("src"));

			Assert.Contains(paths, path => path.ToLower().Contains("blue"));
		}

		[Fact]
		public void contains_image_of_empty_tank()
		{
			var divs        = SUT.FindAll("div");
			var backgrounds = divs.Select(div => div.GetAttribute("style"));

			Assert.Contains(backgrounds, bg => bg.ToLower().Contains("white"));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(20)]
		[InlineData(50)]
		[InlineData(75)]
		[InlineData(100)]
		public void Changing_Percentages_changes_Clipping(int percent)
		{
			_filledPercentage = percent;

			var image = SUT.Find("img");
			string style  = image.GetAttribute("style");

			Assert.Contains($"{100 - percent}% 0 0 0", style);

		}
	}
}