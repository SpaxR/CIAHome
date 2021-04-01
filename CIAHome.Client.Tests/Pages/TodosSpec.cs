using System.Collections.Generic;
using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Pages;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests.Pages
{
	public sealed class TodosSpec : TestContext
	{
		private          IRenderedComponent<Todos>        _sut;
		private readonly Mock<IAsyncRepository<Todo>>     _todoRepositoryMock = new();
		private readonly Mock<IAsyncRepository<TodoList>> _listRepositoryMock = new();

		public TodosSpec()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
			Services.AddScoped(_ => _todoRepositoryMock.Object);
			Services.AddScoped(_ => _listRepositoryMock.Object);
		}

		private void CreateSUT()
		{
			_sut = RenderComponent<Todos>();
		}

		[Fact]
		public void contains_add_button()
		{
			CreateSUT();
			var button = _sut.FindComponent<MudButton>();

			Assert.NotNull(button);
		}

		[Fact]
		public void contains_TodoListItem_foreach_TodoList()
		{
			var lists = new List<TodoList> {new(), new(), new(),};
			_listRepositoryMock.Setup(s => s.All()).ReturnsAsync(lists);
			CreateSUT();

			var result = _sut.FindComponents<TodoListItem>();

			Assert.Equal(lists.Count, result.Count);
		}

		[Fact]
		public void contains_TodoItem_foreach_uncategorized_Todo()
		{
			var todos = new List<Todo> {new(), new(), new(),};
			_todoRepositoryMock.Setup(s => s.All()).ReturnsAsync(todos);
			CreateSUT();

			var result = _sut.FindComponents<TodoItem>();

			Assert.Equal(todos.Count, result.Count);
		}
	}
}