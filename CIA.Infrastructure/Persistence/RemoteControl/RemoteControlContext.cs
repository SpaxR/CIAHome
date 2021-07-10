using Microsoft.EntityFrameworkCore;

namespace CIA.Infrastructure
{
	public class RemoteControlContext : DbContext
	{
		public DbSet<Pump>      Pumps      { get; set; }
		public DbSet<Watertank> Watertanks { get; set; }

		/// <inheritdoc />
		protected RemoteControlContext() { }

		/// <inheritdoc />
		public RemoteControlContext(DbContextOptions<RemoteControlContext> options) : base(options) { }
	}
}