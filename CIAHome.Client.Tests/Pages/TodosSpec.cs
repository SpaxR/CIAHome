using System.Collections.Generic;
using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Pages;
using CIAHome.Client.Services;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests.Pages
{
	public sealed class TodosSpec : TestContext
	{
		private readonly IRenderedComponent<Todos> _sut;
		private readonly Mock<ITodoService>        _todoServiceMock = new();

		public TodosSpec()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
			Services.AddScoped(_ => _todoServiceMock.Object);
			_sut = RenderComponent<Todos>();
		}

		[Fact]
		public void contains_add_button()
		{
			var button = _sut.FindComponent<MudButton>();

			Assert.NotNull(button);
		}

		[Fact]
		public void contains_TodoListItem_foreach_TodoList()
		{
			var lists = new List<TodoList> {new(), new(), new(),};
			_todoServiceMock.Setup(s => s.TodoListsAsync()).ReturnsAsync(lists);
			_sut.Render();
			
			var result = _sut.FindComponents<TodoListItem>();

			Assert.Equal(lists.Count, result.Count);
		}
	}
}