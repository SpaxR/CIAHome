using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIAHome.Client.Services;
using CIAHome.Shared.Model;
using Xunit;

namespace CIAHome.Client.Tests.Services
{
	public class TodoServiceSpec
	{
		private readonly TodoService _sut = new();

		[Fact]
		public async Task CreateTodoAsync_returns_new_Todo()
		{
			var result = await _sut.CreateTodoAsync();

			Assert.IsAssignableFrom<Todo>(result);
		}

		[Fact]
		public async Task CreateTodoListAsync_returns_newTodoList()
		{
			var result = await _sut.CreateTodoListAsync();

			Assert.IsAssignableFrom<TodoList>(result);
		}

		[Fact]
		public async Task TodosAsync_with_null_returns_todos_without_list()
		{
			var todos = new List<Todo>
			{
				await _sut.CreateTodoAsync(),
				await _sut.CreateTodoAsync(),
				await _sut.CreateTodoAsync(),
			};

			var result = await _sut.TodosAsync(null);

			Assert.Equal(todos, result);
		}
		
		[Fact]
		public async Task TodoListsAsync_returns_all_TodoLists()
		{
			var lists = new List<TodoList>
			{
				await _sut.CreateTodoListAsync(),
				await _sut.CreateTodoListAsync(),
				await _sut.CreateTodoListAsync(),
			};

			var result = await _sut.TodoListsAsync();

			Assert.Equal(lists, result);
		}
		
		
	}
}