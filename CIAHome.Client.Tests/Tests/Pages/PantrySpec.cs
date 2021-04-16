using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Pages;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class PantrySpec : TestContext
	{
		private readonly Mock<IAsyncRepository<Product>> _repoMock = new();

		private IRenderedComponent<Pantry> _sut;

		private IRenderedComponent<Pantry> SUT
		{
			get
			{
				_sut ??= RenderComponent<Pantry>();
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
			var field = SUT.FindComponent<MudTextField<string>>();

			Assert.NotNull(field);
		}

		[Fact]
		public void Input_GTIN_loads_Product()
		{
			var product   = new Product {GTIN = "SOME GTIN"};
			var textField = SUT.FindComponent<MudTextField<string>>();

			textField.Find("input").Change(product.GTIN);

			_repoMock.Verify(repo => repo.Find(product.GTIN));
		}

		[Fact]
		public void Loading_Product_shows_ProductCard()
		{
			_repoMock.Setup(repo => repo.Find(It.IsAny<string>()))
					 .ReturnsAsync(new Product());


			SUT.FindComponent<MudTextField<string>>().Find("input").Change("GTIN");

			Assert.NotNull(SUT.FindComponent<ProductCard>());
		}
	}
}