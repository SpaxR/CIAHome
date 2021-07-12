using System;
using System.Collections.Generic;
using System.Linq;

namespace CIAHome.Shared.Models
{
	public class TodoList : Core.Entities.ITodoList<TodoItem>
	{
		/// <inheritdoc />
		public Guid Id { get; set; }

		/// <inheritdoc />
		public string Text { get; set; } = "Neue Liste";

		/// <inheritdoc />
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