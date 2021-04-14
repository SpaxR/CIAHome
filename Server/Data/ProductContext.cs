using CIAHome.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace CIAHome.Server.Data
{
	public class ProductContext : DbContext
	{
		public DbSet<Product> Products { get; set; }

		/// <inheritdoc />
		public ProductContext(DbContextOptions options) : base(options) { }
	}
}