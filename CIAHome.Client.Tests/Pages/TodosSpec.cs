using System.Collections.Generic;
using System.Linq;
using Bunit;
using CIAHome.Client.Pages;
using CIAHome.Client.Tests.PageModel;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CIAHome.Client.Tests.Pages
{
	public sealed class TodosSpec : TestContext
	{
		private TodoPage _sut;

		private TodoPage SUT
		{
			get
			{
				_sut ??= new TodoPage(RenderComponent<Todos>());
				return _sut;
			}
		}


		private readonly Mock<IAsyncRepository<Todo>>     _todoRepositoryMock = new();
		private readonly Mock<IAsyncRepository<TodoList>> _listRepositoryMock = new();

		public TodosSpec()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
			Services.AddScoped(_ => _todoRepositoryMock.Object);
			Services.AddScoped(_ => _listRepositoryMock.Object);
		}

		[Fact]
		public void contains_add_todo_button()
		{
			Assert.NotNull(SUT.AddTodoButton);
		}

		[Fact]
		public void contains_TodoListCard_foreach_TodoList()
		{
			var lists = new List<TodoList> {new(), new(), new(),};
			_listRepositoryMock.Setup(s => s.All()).ReturnsAsync(lists);

			Assert.Equal(lists.Count, SUT.ListCards.Count());
		}

		[Fact]
		public void contains_TodoItem_foreach_uncategorized_Todo()
		{
			var todos = new List<Todo> {new(), new(), new(),};
			_todoRepositoryMock.Setup(s => s.All()).ReturnsAsync(todos);

			Assert.Equal(todos.Count, SUT.TodoItems.Count());
		}
	}
}