using Bunit;
using CIAHome.Client.Components.Cards;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class TodoListCardSpec : TestContext
	{
		private IRenderedComponent<TodoListCard> _sut;

		private IRenderedComponent<TodoListCard> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoListCard>();
				return _sut;
			}
		}

		[Fact]
		public void shows_text_of_list()
		{
			var list = new TodoList();
			SUT.SetParametersAndRender((nameof(TodoListCard.List), list));

			Assert.Contains(list.Text, SUT.Markup);
		}

		[Fact]
		public void shows_amount_of_Todos_in_list()
		{
			var list = new TodoList();
			list.Todos.Add(new Todo());
			list.Todos.Add(new Todo());
			SUT.SetParametersAndRender((nameof(TodoListCard.List), list));

			Assert.Contains("2 Todos", SUT.Markup);
		}

		[Fact]
		public void shows_empty_todos_as_No_Todos()
		{
			SUT.SetParametersAndRender((nameof(TodoListCard.List), new TodoList()));

			Assert.Contains("No Todos", SUT.Markup);
		}

		[Fact]
		public void MissingList_shows_MudSkeleton_for_Text_and_TodoCount()
		{
			var skeletons = SUT.FindComponents<MudSkeleton>();

			Assert.Equal(2, skeletons.Count);
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
	}
}