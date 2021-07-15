using System.Collections.Generic;
using System.Linq;

namespace WebUI.Shared.Models
{
	public class TodoList
	{
		public string Id { get; set; }

		public string Text { get; set; } = "Neue Liste";

		public IEnumerable<TodoItem> Todos { get; private set; } = new List<TodoItem>();

		public void AddTodo(TodoItem todo)
		{
			Todos = Todos.Concat(new[] { todo });
		}

		public void RemoveTodo(TodoItem todo)
		{
			Todos = Todos.Where(t => t != todo);
		}
	}
}