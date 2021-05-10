using Bunit;
using CIAHome.Client.Pages;
using CIAHome.Client.Tests.PageModel;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class PantrySpec : TestContext
	{
		private readonly Mock<IAsyncRepository<Product>> _repoMock = new();

		private PantryPage _sut;

		private PantryPage SUT
		{
			get
			{
				_sut ??= new PantryPage(RenderComponent<Pantry>());
				return _sut;
			}
		}

		public PantrySpec()
		{
			Services.AddScoped(_ => _repoMock.Object);
		}

		[Fact]
		public void contains_TextField()
		{
			Assert.NotNull(SUT.InputTextField);
		}

		[Fact]
		public void Input_GTIN_loads_Product()
		{
			var product   = new Product {GTIN = "SOME GTIN"};
			var textField = SUT.InputTextField;

			textField.Find("input").Change(product.GTIN);

			_repoMock.Verify(repo => repo.Find(product.GTIN));
		}

		[Fact]
		public void Loading_Product_shows_ProductCard()
		{
			_repoMock.Setup(repo => repo.Find(It.IsAny<string>()))
					 .ReturnsAsync(new Product());


			SUT.InputTextField.Find("input").Change("SOME GTIN");

			Assert.NotNull(SUT.CurrentProduct);
		}

		[Fact]
		public void Failing_to_load_Product_shows_ProductCard_with_new_Product()
		{
			SUT.InputTextField.Find("input").Change("SOME GTIN");

			Assert.NotNull(SUT.CurrentProduct);
		}
	}
}