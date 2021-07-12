using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CIA.Infrastructure
{
	public class ContextBase<T> : DbContext where T : ContextBase<T>
	{
		protected ILogger<T> Logger { get; }
		public ContextBase(ILogger<T> logger) : this(logger, new DbContextOptions<T>()) { }

		/// <inheritdoc />
		public ContextBase(ILogger<T> logger, DbContextOptions options) : base(options)
		{
			Logger = logger;
		}

		public virtual void Initialize()
		{
			// Update Database
#if DEBUG
			Logger.LogWarning("Recreating Database {DB}", Database.GetDbConnection().Database);
			Database.EnsureDeleted();
			Database.EnsureCreated();

#else
			_logger.LogInformation("Migrating Database {DB}", Database.GetDbConnection().Database);
			Database.Migrate();
#endif
		}
	}
}