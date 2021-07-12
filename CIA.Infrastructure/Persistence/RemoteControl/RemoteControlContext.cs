using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CIA.Infrastructure
{
	public class RemoteControlContext : ContextBase<RemoteControlContext>
	{
		public DbSet<Pump>      Pumps      { get; set; }
		public DbSet<Watertank> Watertanks { get; set; }

		/// <inheritdoc />
		public RemoteControlContext(ILogger<RemoteControlContext> logger)
			: base(logger) { }

		/// <inheritdoc />
		public RemoteControlContext(ILogger<RemoteControlContext> logger, DbContextOptions<RemoteControlContext> options)
			: base(logger, options) { }


		public override void Initialize()
		{
			base.Initialize();

#if DEBUG
			Pumps.Add(new Pump());
			SaveChanges();
#endif
		}
	}
}