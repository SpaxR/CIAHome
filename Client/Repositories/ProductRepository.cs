using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;

namespace CIAHome.Client.Repositories
{
	public class ProductRepository : IAsyncRepository<Product>
	{
		private readonly ILocalStorageService _storage;

		public ProductRepository(ILocalStorageService storage)
		{
			_storage = storage;
		}


		/// <inheritdoc />
		public async Task<IEnumerable<Product>> All()
		{
			string[] ids = await _storage.GetItemAsync<string[]>(nameof(Product));

			var products = await Task.WhenAll(ids.Select(id => _storage.GetItemAsync<Product>(id).AsTask()));

			return products;
		}

		/// <inheritdoc />
		public Task<Product> Find(string gtin)
		{
			return _storage.GetItemAsync<Product>(gtin).AsTask();
		}

		/// <inheritdoc />
		public async Task<Product> Find(Func<Product, bool> predicate)
		{
			var products = await All();
			return products.FirstOrDefault(predicate);
		}

		/// <inheritdoc />
		public async Task<Product> Create()
		{
			var product = new Product();
			await _storage.SetItemAsync(product.GTIN, product);
			return product;
		}

		/// <inheritdoc />
		public Task Update(Product product)
		{
			return _storage.SetItemAsync(product.GTIN, product).AsTask();
		}

		/// <inheritdoc />
		public Task Delete(Product product)
		{
			return _storage.RemoveItemAsync(product.GTIN).AsTask();
		}
	}
}