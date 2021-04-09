﻿using System.Threading.Tasks;
using AngleSharp.Dom;
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

namespace CIAHome.Client.Tests
{
	public class TodoDetailSpec : TestContext
	{
		private          IRenderedComponent<TodoDetail>   _sut;
		private readonly Mock<IAsyncRepository<TodoList>> _repositoryMock = new();
		private readonly TodoList                         _list           = new();
		private readonly FakeNavigationManager            _navigation;

		private IRenderedComponent<TodoDetail> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoDetail>((nameof(TodoDetail.Id), _list.Id));
				return _sut;
			}
		}

		public TodoDetailSpec()
		{
			_repositoryMock.Setup(repository => repository.Find(_list.Id)).ReturnsAsync(_list);

			Services.AddScoped(_ => _repositoryMock.Object);
			Services.AddScoped<NavigationManager>(_ => _navigation);

			_navigation = new FakeNavigationManager(Renderer);
		}

		[Fact]
		public void WithId_loads_Todos_of_List()
		{
			_list.Todos.Add(new Todo());

			var todoItems = SUT.FindComponents<TodoItem>();

			Assert.Equal(_list.Todos.Count, todoItems.Count);
		}

		[Fact]
		public void contains_add_Todo_Button()
		{
			var button = SUT.FindComponent<MudButton>();

			Assert.NotNull(button);
		}

		[Fact]
		public void contains_Text_of_list()
		{
			var text = SUT.FindComponent<MudText>();

			Assert.Contains(_list.Text, text.Markup);
		}

		[Fact]
		public void contains_return_button()
		{
			Assert.NotNull(SUT.FindIconButton(Icons.Sharp.ArrowBack));
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
			_list.Todos.Add(new Todo());

			await SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			Assert.Empty(SUT.FindComponents<TodoItem>());
		}

		[Fact]
		public async Task Deleting_Todo_removes_Todo_from_List()
		{
			_list.Todos.Add(new Todo());

			await SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			Assert.Empty(_list.Todos);
		}

		[Fact]
		public async Task Deleting_Todo_updates_List_in_Repository()
		{
			_list.Todos.Add(new Todo());

			await SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			_repositoryMock.Verify(repository => repository.Update(_list));
		}

		[Fact]
		public void Invoking_TodoItem_OnDelete_deletes_Todo()
		{
			_list.Todos.Add(new Todo());

			SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnDelete.InvokeAsync);

			Assert.Empty(_list.Todos);
			_repositoryMock.Verify(repository => repository.Update(_list));
		}

		[Fact]
		public void contains_TodoItem_foreach_Todo_in_List()
		{
			_list.Todos.Add(new Todo());

			Assert.Equal(1, SUT.FindComponents<TodoItem>().Count);
		}

		[Fact]
		public void Invoking_TodoItem_OnUpdate_updates_List_in_Repository()
		{
			_list.Todos.Add(new Todo());

			SUT.InvokeAsync(SUT.FindComponent<TodoItem>().Instance.OnUpdate.InvokeAsync);

			_repositoryMock.Verify(repository => repository.Update(_list));
		}

		[Fact]
		public void Click_BackButton_navigates_to_TodosPage()
		{
			IElement button = SUT.FindIconButton(Icons.Sharp.ArrowBack).Find("button");

			Assert.Raises<LocationChangedEventArgs>(
				handler => _navigation.LocationChanged += handler,
				handler => _navigation.LocationChanged -= handler,
				() => button.Click());
		}
	}
}