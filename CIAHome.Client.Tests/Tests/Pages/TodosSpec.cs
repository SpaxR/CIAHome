using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Client.Pages.Todo;
using CIAHome.Client.Tests.PageModel;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CIAHome.Client.Tests
{
	public sealed class TodosSpec : TestContext
	{
		private TodoPage _sut;

		private TodoPage SUT
		{
			get
			{
				_sut ??= new TodoPage(RenderComponent<TodoMaster>());
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
		public void contains_add_Todo_Button()
		{
			Assert.NotNull(SUT.AddTodoBtn);
		}

		[Fact]
		public void contains_add_TodoList_Button()
		{
			Assert.NotNull(SUT.AddTodoListBtn);
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

		[Fact]
		public void Adding_List_adds_ListCard()
		{
			_ = SUT;
			var list = new TodoList();
			_listRepositoryMock.Setup(repo => repo.Create()).ReturnsAsync(list);
			_listRepositoryMock.Setup(repo => repo.All()).ReturnsAsync(new[] {list});

			SUT.AddTodoListBtn.Find("button").Click();

			Assert.Contains(SUT.ListCards, card => card.List.Id.Equals(list.Id));
		}

		[Fact]
		public void Click_AddList_creates_new_list()
		{
			SUT.AddTodoListBtn.Find("button").Click();

			_listRepositoryMock.Verify(repo => repo.Create());
		}

		[Fact]
		public void Adding_Todo_adds_TodoItem()
		{
			_ = SUT;
			var todo = new Todo();
			_todoRepositoryMock.Setup(repo => repo.Create()).ReturnsAsync(todo);
			_todoRepositoryMock.Setup(repo => repo.All()).ReturnsAsync(new[] {todo});

			SUT.AddTodoBtn.Find("button").Click();

			Assert.Contains(SUT.TodoItems, item => item.Todo == todo);
		}

		[Fact]
		public void Click_AddTodo_creates_new_Todo()
		{
			SUT.AddTodoBtn.Find("button").Click();

			_todoRepositoryMock.Verify(repo => repo.Create());
		}

		[Fact]
		public void Invoking_ListCard_OnDelete_deletes_List()
		{
			var list = new TodoList();
			_listRepositoryMock.Setup(repo => repo.All()).ReturnsAsync(new[] {list});

			SUT.ListCards.Single().InvokeDelete();

			_listRepositoryMock.Verify(repo => repo.Delete(list));
		}

		[Fact]
		public void Deleting_List_removes_ListCard()
		{
			_listRepositoryMock.SetupSequence(repo => repo.All())
							   .ReturnsAsync(new[] {new TodoList()})
							   .ReturnsAsync(Array.Empty<TodoList>());

			SUT.ListCards.Single().InvokeDelete();

			Assert.Empty(SUT.ListCards);
		}

		[Fact]
		public void Invoking_TodoItem_OnDelete_deletes_Todo()
		{
			var todo = new Todo();
			_todoRepositoryMock.Setup(repo => repo.All()).ReturnsAsync(new[] {todo});

			SUT.TodoItems.Single().InvokeDelete();

			_todoRepositoryMock.Verify(repo => repo.Delete(todo));
		}

		[Fact]
		public void Deleting_Todo_removes_TodoItem()
		{
			_todoRepositoryMock.SetupSequence(repo => repo.All())
							   .ReturnsAsync(new[] {new Todo()})
							   .ReturnsAsync(Array.Empty<Todo>());

			SUT.TodoItems.Single().InvokeDelete();

			Assert.Empty(SUT.TodoItems);
		}

		[Fact]
		public void WithId_loads_Todos_of_List_in_detail_View()
		{
			var list = new TodoList();
			list.Todos.Add(new Todo());
			list.Todos.Add(new Todo());

			_listRepositoryMock.Setup(repo => repo.Find(It.IsAny<Func<TodoList, bool>>()))
							   .ReturnsAsync(list);
			
			var sut = RenderComponent<TodoMaster>((nameof(TodoMaster.Id), list.Id));

			foreach (var todo in list.Todos)
			{
				Assert.NotNull(sut.FindComponent<TodoItem>(item => item.Instance.Todo == todo));
			}
		}
	}
}