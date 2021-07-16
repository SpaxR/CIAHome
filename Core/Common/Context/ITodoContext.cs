using Microsoft.EntityFrameworkCore;

namespace CIAHome.Core
{
	public interface ITodoContext
	{
		DbSet<TodoItem> TodoItems { get; set; }
		DbSet<TodoList> TodoLists { get; set; }
	}
}