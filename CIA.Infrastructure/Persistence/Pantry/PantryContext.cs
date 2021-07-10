using Microsoft.EntityFrameworkCore;

namespace CIA.Infrastructure
{
	public class PantryContext : DbContext
	{
		public DbSet<Pantry> Pantries { get; set; }

		/// <inheritdoc />
		protected PantryContext() { }

		/// <inheritdoc />
		public PantryContext(DbContextOptions<PantryContext> options) : base(options) { }
	}
}