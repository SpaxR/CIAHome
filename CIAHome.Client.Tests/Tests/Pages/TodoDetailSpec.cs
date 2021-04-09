using System.Threading.Tasks;
using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Client.Pages.Todo;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class TodoDetailSpec : TestContext
	{
		private          IRenderedComponent<TodoDetail>   _sut;
		private readonly Mock<IAsyncRepository<TodoList>> _repositoryMock = new();
		private readonly Mock<TodoList>                   _listMock       = new();

		private IRenderedComponent<TodoDetail> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoDetail>((nameof(TodoDetail.Id), _listMock.Object.Id));
				return _sut;
			}
		}

		public TodoDetailSpec()
		{
			_listMock.Object.Id = new TodoList().Id;
			_repositoryMock.Setup(repository => repository.Find(_listMock.Object.Id)).ReturnsAsync(_listMock.Object);

			Services.AddScoped(_ => _repositoryMock.Object);
		}

		[Fact]
		public void WithId_loads_Todos_of_List()
		{
			_listMock.Object.Todos.Add(new Todo());

			var todoItems = SUT.FindComponents<TodoItem>();

			Assert.Equal(_listMock.Object.Todos.Count, todoItems.Count);
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
		public async Task Deleting_Todo_removes_TodoItem()
		{
			_listMock.Object.Todos.Add(new Todo());

			await SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			Assert.Empty(SUT.FindComponents<TodoItem>());
		}

		[Fact]
		public async Task Deleting_Todo_removes_Todo_from_List()
		{
			_listMock.Object.Todos.Add(new Todo());

			await SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			Assert.Empty(_listMock.Object.Todos);
		}

		[Fact]
		public async Task Deleting_Todo_updates_List_in_Repository()
		{
			_listMock.Object.Todos.Add(new Todo());

			await SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			_repositoryMock.Verify(repository => repository.Update(_listMock.Object));
		}

		[Fact]
		public void Invoking_TodoItem_OnDelete_deletes_Todo()
		{
			_listMock.Object.Todos.Add(new Todo());

			SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			Assert.Empty(_listMock.Object.Todos);
			_repositoryMock.Verify(repository => repository.Update(_listMock.Object));
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

			_repositoryMock.Verify(repository => repository.Update(_listMock.Object));
		}
	}
}