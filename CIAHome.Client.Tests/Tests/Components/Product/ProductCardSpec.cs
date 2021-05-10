using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Tests.PageModel;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class ProductCardSpec : TestContext
	{
		private readonly CIAHome.Shared.Model.Product _product = new();

		private ProductCardComponent _sut;

		private ProductCardComponent SUT
		{
			get
			{
				_sut ??= new ProductCardComponent(RenderComponent<ProductCard>(
													  (nameof(ProductCard.Product), _product)));
				return _sut;
			}
		}

		[Fact]
		public void contains_TextField_with_Gtin()
		{
			_product.GTIN = "SOME GTIN";

			Assert.Equal(_product.GTIN, SUT.GtinTextField.Find("input").GetAttribute("value"));
			
		}
	}
}