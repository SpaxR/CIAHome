using System;
using System.Collections.Generic;
using CIAHome.Server.Data;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;

namespace CIAHome.Server.Repositories
{
	public class ProductRepository : IRepository<Product>
	{
		private readonly ProductContext _context;

		public ProductRepository(ProductContext context)
		{
			_context = context;
		}


		/// <inheritdoc />
		public IEnumerable<Product> All()
		{
			return _context.Products;
		}

		/// <inheritdoc />
		public Product Find(string              id)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Product Find(Func<Product, bool> predicate)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Product Create()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void Update(Product entity)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void Delete(Product entity)
		{
			throw new NotImplementedException();
		}
	}
}