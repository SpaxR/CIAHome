using Bunit;
using CIAHome.Client.Components;

namespace CIAHome.Client.Tests
{
	public class ProductCardSpec : TestContext
	{
		private readonly CIAHome.Shared.Model.Product _product = new();

		private IRenderedComponent<ProductCard> _sut;

		private IRenderedComponent<ProductCard> SUT
		{
			get
			{
				_sut ??= RenderComponent<ProductCard>((nameof(ProductCard.Product), _product));
				return _sut;
			}
		}
	}
}