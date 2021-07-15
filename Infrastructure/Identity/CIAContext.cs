using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CIA.Infrastructure.Identity
{
	public class CIAContext : IdentityDbContext<IdentityUser>
	{
		// 		private readonly ILogger<CIAContext> _logger;
		//
		// 		public CIAContext(ILogger<CIAContext> logger)
		// 			: this(logger, new DbContextOptions<CIAContext>()) { }
		//
		// 		/// <inheritdoc />
		// 		public CIAContext(ILogger<CIAContext> logger, DbContextOptions<CIAContext> options)
		// 			: base(options)
		// 		{
		// 			_logger = logger;
		// 		}
		//
		public void Initialize(UserManager<IdentityUser> manager)
		{
			// #if DEBUG
			// 			_logger.LogWarning("Recreating Database {DB}", Database.GetDbConnection().Database);
			// 			Database.EnsureDeleted();
			// 			Database.EnsureCreated();
			//
			// #else
			// 			_logger.LogInformation("Migrating Database {DB}", Database.GetDbConnection().Database);
			// 			Database.Migrate();
			// #endif
			//
			// 			// Seed Data
			// 			if (!Users.Any())
			// 			{
			// 				var admin = new IdentityUser { UserName = "Administrator" };
			// 				manager.CreateAsync(admin, "admin").Wait();
			// 				SaveChanges();
			// 			}
		}
	}
}