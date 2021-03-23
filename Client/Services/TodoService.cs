using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIAHome.Shared.Model;

namespace CIAHome.Client.Services
{
	public interface ITodoService
	{
		Task<Todo>                  CreateTodoAsync();
		Task<TodoList>              CreateTodoListAsync();
		Task<IEnumerable<Todo>>     TodosAsync(string listId);
		Task<IEnumerable<TodoList>> TodoListsAsync();
	}

	public class TodoService : ITodoService
	{
		private readonly IList<Todo>     _todos = new List<Todo>();
		private readonly IList<TodoList> _lists = new List<TodoList>();

		/// <inheritdoc />
		public Task<Todo> CreateTodoAsync()
		{
			var todo = new Todo();
			_todos.Add(todo);

			return Task.FromResult(todo);
		}

		/// <inheritdoc />
		public Task<TodoList> CreateTodoListAsync()
		{
			var list = new TodoList();
			_lists.Add(list);

			return Task.FromResult(list);
		}

		/// <inheritdoc />
		public Task<IEnumerable<Todo>> TodosAsync(string listId)
		{
			return Task.FromResult(_todos.AsEnumerable());
		}

		/// <inheritdoc />
		public Task<IEnumerable<TodoList>> TodoListsAsync()
		{
			return Task.FromResult(_lists.AsEnumerable());
		}
	}
}