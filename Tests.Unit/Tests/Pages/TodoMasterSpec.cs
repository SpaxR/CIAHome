using System;
using System.Collections.Generic;
using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Client.Pages.Todo;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace Tests.Unit
{
	public sealed class TodoMasterSpec : TestContext
	{
		private          IRenderedComponent<TodoMaster>   _sut;
		private readonly Mock<IAsyncRepository<TodoList>> _listRepositoryMock = new();

		private readonly FakeNavigationManager _navigation;

		private IRenderedComponent<TodoMaster> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoMaster>();
				return _sut;
			}
		}


		public TodoMasterSpec()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
			Services.AddScoped(_ => _listRepositoryMock.Object);
			Services.AddScoped<NavigationManager>(_ => _navigation);

			_navigation = new FakeNavigationManager(Renderer);
		}


		[Fact]
		public void contains_add_TodoList_Button()
		{
			var button = SUT.FindComponent<MudButton>();

			Assert.NotNull(button);
		}

		[Fact]
		public void contains_TodoListCard_foreach_TodoList()
		{
			var lists = new List<TodoList> { new(), new(), new(), };
			_listRepositoryMock.Setup(s => s.All()).ReturnsAsync(lists);

			var cards = SUT.FindComponents<TodoListCard>();

			Assert.Equal(lists.Count, cards.Count);
		}

		[Fact]
		public void Adding_List_adds_ListCard()
		{
			_ = SUT;
			_listRepositoryMock.Setup(repo => repo.All()).ReturnsAsync(new[] { new TodoList() });

			SUT.FindComponent<MudButton>()
			   .Find("button")
			   .Click();

			var cards = SUT.FindComponents<TodoListCard>();

			Assert.Equal(1, cards.Count);
		}

		[Fact]
		public void Click_AddList_creates_new_list()
		{
			SUT
				.FindComponent<MudButton>()
				.Find("button")
				.Click();

			_listRepositoryMock.Verify(repo => repo.Create());
		}

		[Fact]
		public void Invoking_ListCard_OnDelete_deletes_List()
		{
			var list = new TodoList();
			_listRepositoryMock.Setup(repo => repo.All()).ReturnsAsync(new[] { list });

			SUT.InvokeAsync(SUT.FindComponent<TodoListCard>().Instance.OnDelete.InvokeAsync);

			_listRepositoryMock.Verify(repo => repo.Delete(list));
		}

		[Fact]
		public void Deleting_List_removes_ListCard()
		{
			_listRepositoryMock.SetupSequence(repo => repo.All())
							   .ReturnsAsync(new[] { new TodoList() })
							   .ReturnsAsync(Array.Empty<TodoList>());

			SUT.InvokeAsync(SUT.FindComponent<TodoListCard>().Instance.OnDelete.InvokeAsync);


			var cards = SUT.FindComponents<TodoListCard>();

			Assert.Equal(0, cards.Count);
		}

		[Fact]
		public void Clicking_ListCard_navigates_to_Detail_View()
		{
			_listRepositoryMock.Setup(repository => repository.All()).ReturnsAsync(new[] { new TodoList() });

			var card = SUT.FindComponent<TodoListCard>().Find("div");

			Assert.Raises<LocationChangedEventArgs>(
				handler => _navigation.LocationChanged += handler,
				handler => _navigation.LocationChanged -= handler,
				() => card.Click());
		}
	}
}