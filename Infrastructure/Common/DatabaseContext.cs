using System.Threading.Tasks;
using CIAHome.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CIA.Infrastructure
{
	public class DatabaseContext : DbContext, IUserContext, ITodoContext, IPantryContext, IRemoteControlContext
	{
		/// <inheritdoc />
		public DbSet<User> Users { get; set; }

		/// <inheritdoc />
		public DbSet<Pantry> Pantries { get; set; }

		/// <inheritdoc />
		public DbSet<TodoItem> TodoItems { get; set; }

		/// <inheritdoc />
		public DbSet<TodoList> TodoLists { get; set; }

		/// <inheritdoc />
		public DbSet<Pump> Pumps { get; set; }

		/// <inheritdoc />
		public DbSet<Watertank> Watertanks { get; set; }

		private readonly ILogger<DatabaseContext> _log;

		public DatabaseContext(ILogger<DatabaseContext> log)
			: this(log, new DbContextOptions<DatabaseContext>()) { }

		/// <inheritdoc />
		public DatabaseContext(ILogger<DatabaseContext> log, DbContextOptions<DatabaseContext> options)
			: base(options)
		{
			_log = log;
		}
		
		public async Task InitializeAsync()
		{
#if DEBUG
			_log.LogWarning("Recreating Database {DB}", Database.GetDbConnection().Database);
			await Database.EnsureDeletedAsync();
			await Database.EnsureCreatedAsync();

			await Task.WhenAll(
				InitializeUsers(),
				InitializePantries(),
				InitializePumps(),
				InitializeTodos()
			);
#else
			_log.LogInformation("Migrating Database {DB}", Database.GetDbConnection().Database);
			Database.Migrate();
#endif
			if (!await Users.AnyAsync())
			{
				_log.LogInformation("No Users found - Creating Administrator");
				await Users.AddAsync(new User { UserName = "Administrator" });
				await SaveChangesAsync();
			}
		}

		private async Task InitializeUsers()
		{
			_log.LogInformation("Seeding Users");
			await Users.AddAsync(new User());
			await SaveChangesAsync();
		}

		private async Task InitializePantries()
		{
			_log.LogInformation("Seeding Pantries");
			await Pantries.AddAsync(new Pantry());
			await SaveChangesAsync();
		}

		private async Task InitializePumps()
		{
			_log.LogInformation("Seeding Pumps");
			await Pumps.AddAsync(new Pump());
			await SaveChangesAsync();
		}

		private async Task InitializeTodos()
		{
			_log.LogInformation("Seeding TodoItems");
			await TodoItems.AddAsync(new TodoItem());
			await TodoItems.AddAsync(new TodoItem());
			await TodoItems.AddAsync(new TodoItem());
			await SaveChangesAsync();

			_log.LogInformation("Seeding TodoLists");
			await TodoLists.AddAsync(new TodoList());
			await TodoLists.AddAsync(new TodoList());
			await TodoLists.AddAsync(new TodoList());
			await SaveChangesAsync();
		}
	}
}