using CIAHome.Server.Data;
using CIAHome.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CIAHome.Server.Repositories
{
	public class ProductRepositorySpec
	{
		private          ProductRepository _sut;
		private readonly ProductContext    _ctx;

		private ProductRepository SUT
		{
			get
			{
				_sut ??= new ProductRepository(_ctx);
				return _sut;
			}
		}

		public ProductRepositorySpec()
		{
			DbContextOptions options = new DbContextOptionsBuilder()
									   .UseInMemoryDatabase("InMemory")
									   .Options;
			_ctx = new ProductContext(options);
		}

		[Fact]
		public void All_returns_All_Products()
		{
			_ctx.Products.AddRange(new Product(),
								   new Product(),
								   new Product());

			var result = SUT.All();

			Assert.Equal(_ctx.Products, result);
		}
	}
}