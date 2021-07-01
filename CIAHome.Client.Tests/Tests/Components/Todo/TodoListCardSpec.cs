using System;
using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class TodoListCardSpec : TestContext
	{
		private          IRenderedComponent<TodoListCard> _sut;
		private readonly TodoList                         _list = new();

		private IRenderedComponent<TodoListCard> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoListCard>((nameof(TodoListCard.List), _list));
				return _sut;
			}
		}

		[Fact]
		public void shows_text_of_list()
		{
			Assert.Contains(_list.Text, SUT.Markup);
		}

		[Fact]
		public void shows_amount_of_Todos_in_list()
		{
			_list.Todos.Add(new Todo());
			_list.Todos.Add(new Todo());

			Assert.Contains("2 Todos", SUT.Markup);
		}

		[Fact]
		public void shows_empty_todos_as_No_Todos()
		{
			Assert.Contains("No Todos", SUT.Markup);
		}

		[Fact]
		public void contains_delete_IconButton()
		{
			var button = SUT.FindComponent<MudIconButton>();

			Assert.Equal(Icons.Filled.Delete, button.Instance.Icon);
		}

		[Fact]
		public void delete_IconButton_calls_OnDelete()
		{
			var button = SUT.FindComponent<MudIconButton>();

			Assert.Raises<MouseEventArgs>(
				handler => SUT.SetParametersAndRender((nameof(TodoListCard.OnDelete), handler.AsCallback())),
				_ => SUT.Instance.OnDelete = EventCallback<MouseEventArgs>.Empty,
				() => button.Find("button").Click());
		}

		[Fact]
		public void missing_TodoList_throws_ArgumentNullException()
		{
			var exception = Assert.Throws<ArgumentNullException>(
				() => RenderComponent<TodoListCard>((nameof(TodoListCard.List), null)));

			Assert.Equal(nameof(TodoListCard.List), exception.ParamName);
		}

		[Fact]
		public void click_anywhere_calls_OnClick()
		{
			Assert.Raises<MouseEventArgs>(
				handler => SUT.SetParametersAndRender((nameof(TodoListCard.OnClick), handler.AsCallback())),
				_ => SUT.Instance.OnClick = EventCallback<MouseEventArgs>.Empty,
				() => SUT.Find(".mud-card").Click());
		}
	}
}