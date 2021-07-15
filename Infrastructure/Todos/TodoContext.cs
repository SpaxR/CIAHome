namespace CIA.Infrastructure
{
// 	public class TodoContext : ContextBase<TodoContext>
// 	{
// 		public DbSet<TodoItem> Todos     { get; set; }
// 		public DbSet<TodoList> TodoLists { get; set; }
//
// 		/// <inheritdoc />
// 		public TodoContext(ILogger<TodoContext> logger)
// 			: base(logger) { }
//
// 		/// <inheritdoc />
// 		public TodoContext(ILogger<TodoContext> logger, DbContextOptions<TodoContext> options)
// 			: base(logger, options) { }
//
// 		/// <inheritdoc />
// 		protected override void OnModelCreating(ModelBuilder modelBuilder)
// 		{
// 			base.OnModelCreating(modelBuilder);
//
// 			modelBuilder.Entity<TodoItem>()
// 						.HasKey(item => item.Id);
// 		}
//
//
// 		/// <inheritdoc />
// 		public override void Initialize()
// 		{
// 			base.Initialize();
//
// #if DEBUG
// 			Logger.LogInformation("Seeding TodoItems");
// 			Todos.Add(new TodoItem());
// 			Todos.Add(new TodoItem());
// 			Todos.Add(new TodoItem());
// 			SaveChanges();
//
// 			Logger.LogInformation("Seeding TodoLists");
// 			TodoLists.Add(new TodoList());
// 			TodoLists.Add(new TodoList());
// 			TodoLists.Add(new TodoList());
// 			SaveChanges();
// #endif
// 		}
// 	}
}