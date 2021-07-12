using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CIA.Infrastructure
{
	public class TodoContext : ContextBase<TodoContext>
	{
		public DbSet<TodoItem> Todos     { get; set; }
		public DbSet<TodoList> TodoLists { get; set; }

		/// <inheritdoc />
		public TodoContext(ILogger<TodoContext> logger)
			: base(logger) { }

		/// <inheritdoc />
		public TodoContext(ILogger<TodoContext> logger, DbContextOptions<TodoContext> options)
			: base(logger, options) { }

		/// <inheritdoc />
		public override void Initialize()
		{
			base.Initialize();

#if DEBUG
			Todos.Add(new TodoItem());
			Todos.Add(new TodoItem());
			Todos.Add(new TodoItem());
			SaveChanges();

			TodoLists.Add(new TodoList());
			TodoLists.Add(new TodoList());
			TodoLists.Add(new TodoList());
			SaveChanges();
#endif
			
		}
	}
}