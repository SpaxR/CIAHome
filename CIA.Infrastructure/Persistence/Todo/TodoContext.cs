using Microsoft.EntityFrameworkCore;

namespace CIA.Infrastructure
{
	public class TodoContext : DbContext
	{
		public DbSet<TodoItem> Todos     { get; set; }
		public DbSet<TodoList> TodoLists { get; set; }

		/// <inheritdoc />
		protected TodoContext() { }

		/// <inheritdoc />
		public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
	}
}