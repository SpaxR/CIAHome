using System;
using Bunit;
using CIAHome.Client.Components.ListItems;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class TodoItemSpec : TestContext
	{
		private IRenderedComponent<TodoItem> _sut;

		private readonly Todo                         _todo           = new();
		private readonly Mock<IAsyncRepository<Todo>> _todoRepository = new();

		private IRenderedComponent<TodoItem> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoItem>((nameof(TodoItem.Todo), _todo));
				return _sut;
			}
		}

		public TodoItemSpec()
		{
			Services.AddScoped(_ => _todoRepository.Object);
		}

		private void Enable_Editing()
		{
			SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Create)
			   .Find("button")
			   .Click();
		}

		[Fact]
		public void shows_text_of_Todo()
		{
			_todo.Text = "SOME TEXT";
			Assert.Contains(_todo.Text, SUT.Markup);
		}

		[Fact]
		public void missing_Todo_throws_ArgumentNullException()
		{
			var exception = Assert.Throws<ArgumentNullException>(
				() => RenderComponent<TodoItem>((nameof(TodoItem.Todo), null)));

			Assert.Equal(nameof(TodoItem.Todo), exception.ParamName);
		}

		[Fact]
		public void contains_IconButton_for_editing()
		{
			var buttons = SUT.FindComponents<MudIconButton>();

			Assert.Contains(buttons, button
								=> button.Instance.Icon == Icons.Sharp.Create);
		}

		[Fact]
		public void clicking_edit_IconButton_changes_Text_to_Input()
		{
			Enable_Editing();

			Assert.NotNull(SUT.FindComponent<MudInput<string>>());
		}

		[Fact]
		public void editing_Text_changes_Text_of_Todo()
		{
			Enable_Editing();
			var input = SUT.FindComponent<MudInput<string>>();

			input.Find("input").Change("SOME TEXT");

			Assert.Equal("SOME TEXT", _todo.Text);
		}

		[Fact]
		public void editing_Text_updates_Todo_in_Repository()
		{
			Enable_Editing();
			var input = SUT.FindComponent<MudInput<string>>();

			input.Find("input").Change("SOME TEXT");

			_todoRepository.Verify(repo => repo.Update(_todo));
		}

		[Fact]
		public void editing_Text_shows_Confirm_IconButton()
		{
			Enable_Editing();

			var button = SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Check);

			Assert.NotNull(button);
		}

		[Fact]
		public void editing_Text_does_not_show_delete_IconButton()
		{
			Enable_Editing();

			var button = SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Delete);

			Assert.Null(button);
		}

		[Fact]
		public void clicking_confirm_IconButton_changes_Input_to_Text()
		{
			Enable_Editing();

			SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Check)
			   .Find("button")
			   .Click();

			Assert.NotNull(SUT.FindComponent<MudText>());
		}

		[Fact]
		public void contains_button_for_deletion()
		{
			var button = SUT
				.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Delete);

			Assert.NotNull(button);
		}

		[Fact]
		public void clicking_DeleteButton_calls_OnDelete_callback()
		{
			var button = SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Delete);

			Assert.Raises<MouseEventArgs>(
				handler => SUT.SetParametersAndRender((nameof(TodoItem.OnDelete), handler.AsCallback())),
				_ => SUT.Instance.OnDelete = EventCallback<MouseEventArgs>.Empty,
				() => button.Find("button").Click());
		}

		[Fact]
		public void text_is_strikethrough_if_todo_is_checked()
		{
			_todo.Checked = true;

			var text = SUT.FindComponent<MudText>();

			Assert.Contains("text-decoration:line-through", text.Instance.Style.Trim());
		}

		[Fact]
		public void text_is_not_strikethrough_if_todo_is_unchecked()
		{
			_todo.Checked = false;

			var text = SUT.FindComponent<MudText>();

			Assert.DoesNotContain("line-through", text.Instance.Style.Trim());
		}

		[Fact]
		public void clicking_Todo_toggles_Checked_to_true()
		{
			SUT.Find("button").Click();

			Assert.True(_todo.Checked);
		}

		[Fact]
		public void toggling_Checked_updates_Todo_in_Repository()
		{
			SUT.Find("p").Click();

			_todoRepository.Verify(repo => repo.Update(_todo));
		}
	}
}