using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CIA.Infrastructure
{
	public class PantryContext : ContextBase<PantryContext>
	{
		public DbSet<Pantry> Pantries { get; set; }

		/// <inheritdoc />
		public PantryContext(ILogger<PantryContext> logger) 
			: base(logger) { }

		/// <inheritdoc />
		public PantryContext(ILogger<PantryContext> logger, DbContextOptions<PantryContext> options) 
			: base(logger, options) { }

		public override void Initialize()
		{
			base.Initialize();

#if DEBUG // Seed Test-Data
			Pantries.Add(new Pantry());
			SaveChanges();
#endif
		}
	}
}