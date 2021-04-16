using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Client.Repositories;
using CIAHome.Shared.Model;
using Moq;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class ProductRepositorySpec
	{
		private          ProductRepository          _sut;
		private readonly Mock<ILocalStorageService> _storageMock = new();

		private ProductRepository SUT
		{
			get
			{
				_sut ??= new ProductRepository(_storageMock.Object);
				return _sut;
			}
		}

		private void SetupProducts(params Product[] products)
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(Product)))
						.ReturnsAsync(products.Select(p => p.GTIN).ToArray);

			foreach (Product product in products)
			{
				_storageMock.Setup(storage => storage.GetItemAsync<Product>(product.GTIN))
							.ReturnsAsync(product);
			}
		}


		[Fact]
		public async Task All_returns_all_Products_from_Storage()
		{
			var products = new[]
			{
				new Product(),
				new Product(),
			};
			SetupProducts(products);

			var result = await SUT.All();

			Assert.All(products, p => AssertEx.ContainsEquivalent(p, result));
		}

		[Fact]
		public async Task Find_Id_returns_Product_with_specified_Gtin()
		{
			var products = new[]
			{
				new Product {GTIN = "abc"},
				new Product {GTIN = "123"},
			};
			SetupProducts(products);

			var result = await SUT.Find("abc");

			Assert.Equal(products[0], result);
		}

		[Fact]
		public async Task Find_Predicate_returns_Product_that_matches_predicate()
		{
			var products = new[]
			{
				new Product {GTIN = "abc"},
				new Product {GTIN = "123"},
			};
			SetupProducts(products);

			var result = await SUT.Find(p => p.GTIN.Equals("abc"));

			Assert.Equal(products[0], result);
		}

		[Fact]
		public async Task Create_returns_new_Product()
		{
			var result = await SUT.Create();

			Assert.IsAssignableFrom<Product>(result);
		}

		[Fact]
		public async Task Create_stores_new_Product_in_storage()
		{
			var product = await SUT.Create();

			_storageMock.Verify(storage => storage.SetItemAsync(product.GTIN, product));
		}

		[Fact]
		public async Task Delete_removes_Item_from_Storage()
		{
			var product = new Product();
			SetupProducts(product);

			await SUT.Delete(product);

			_storageMock.Verify(storage => storage.RemoveItemAsync(product.GTIN));
		}

		[Fact]
		public async Task Update_overrides_Product_in_Storage()
		{
			var product = await SUT.Create();

			await SUT.Update(product);

			_storageMock.Verify(storage => storage.SetItemAsync(product.GTIN, product));
		}
	}
}