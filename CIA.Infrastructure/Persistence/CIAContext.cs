using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CIA.Infrastructure
{
	public class CIAContext : IdentityDbContext<CIAUser>
	{
		private readonly ILogger<CIAContext> _logger;

		public CIAContext(ILogger<CIAContext> logger)
			: this(logger, new DbContextOptions<CIAContext>()) { }

		/// <inheritdoc />
		public CIAContext(ILogger<CIAContext> logger, DbContextOptions<CIAContext> options)
			: base(options)
		{
			_logger = logger;
		}

		public void Initialize(UserManager<CIAUser> manager)
		{
			// Update Database
#if DEBUG
			_logger.LogWarning("Recreating Database {DB}", Database.GetDbConnection().Database);
			Database.EnsureDeleted();
			Database.EnsureCreated();

#else
			_logger.LogInformation("Migrating Database {DB}", Database.GetDbConnection().Database);
			Database.Migrate();
#endif

			// Seed Data
			if (!Users.Any())
			{
				var admin = new CIAUser { UserName = "Administrator" };
				manager.CreateAsync(admin, "admin").Wait();
				SaveChanges();
			}
		}
	}
}