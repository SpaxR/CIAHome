using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class TodoListListSpec : TestContext
	{
		private          IRenderedComponent<TodoListList> _sut;
		private readonly Mock<TodoList>                   _listMock           = new();
		private readonly Mock<IAsyncRepository<TodoList>> _listRepositoryMock = new();

		public TodoListListSpec()
		{
			// JSInterop.Mode = JSRuntimeMode.Loose;
			Services.AddScoped(_ => _listRepositoryMock.Object);
		}

		private IRenderedComponent<TodoListList> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoListList>((nameof(TodoListList.List), _listMock.Object));
				return _sut;
			}
		}

		[Fact]
		public void contains_add_Todo_Button()
		{
			var button = SUT.FindComponent<MudButton>();

			Assert.NotNull(button);
		}

		[Fact]
		public void Adding_Todo_adds_TodoItem()
		{
			SUT.FindComponent<MudButton>().Find("button").Click();

			Assert.Equal(1, SUT.FindComponents<TodoItem>().Count);
		}

		[Fact]
		public void Deleting_Todo_removes_TodoItem() { }

		[Fact]
		public void Invoking_TodoItem_OnDelete_deletes_Todo()
		{
			_listMock.Object.Todos.Add(new Todo());

			SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			Assert.Empty(_listMock.Object.Todos);
			_listRepositoryMock.Verify(repository => repository.Update(_listMock.Object));
		}

		[Fact]
		public void contains_TodoItem_foreach_Todo_in_List()
		{
			_listMock.Object.Todos.Add(new Todo());

			Assert.Equal(1, SUT.FindComponents<TodoItem>().Count);
		}

		[Fact]
		public void Invoking_TodoItem_OnUpdate_updates_List_in_Repository()
		{
			_listMock.Object.Todos.Add(new Todo());

			SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnUpdate.InvokeAsync);

			_listRepositoryMock.Verify(repository => repository.Update(_listMock.Object));
		}
	}
}