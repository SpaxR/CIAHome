using System;
using AngleSharp.Css.Dom;
using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace Tests.Unit
{
	public class TodoItemSpec : TestContext
	{
		private IRenderedComponent<TodoListItem> _sut;

		private readonly TodoItem                             _todo           = new();
		private readonly Mock<IAsyncRepository<TodoList>> _listRepository = new();

		private IRenderedComponent<TodoListItem> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoListItem>((nameof(TodoListItem.Todo), _todo));
				return _sut;
			}
		}

		public TodoItemSpec()
		{
			Services.AddScoped(_ => _listRepository.Object);
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
				() => RenderComponent<TodoListItem>((nameof(TodoListItem.Todo), null)));

			Assert.Equal(nameof(TodoListItem.Todo), exception.ParamName);
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
		public void editing_Text_shows_Confirm_IconButton()
		{
			Enable_Editing();

			var button = SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Check);

			Assert.NotNull(button);
		}

		[Fact]
		public void editing_Text_hides_delete_IconButton()
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
				handler => SUT.SetParametersAndRender((nameof(TodoListItem.OnDelete), handler.AsCallback())),
				_ => SUT.Instance.OnDelete = EventCallback<MouseEventArgs>.Empty,
				() => button.Find("button").Click());
		}

		[Fact]
		public void text_is_strikethrough_if_todo_is_checked()
		{
			_todo.IsChecked = true;

			var text = SUT.FindComponent<MudText>().Find("p");

			Assert.Contains("line-through", text.GetStyle()["text-decoration"]);
		}

		[Fact]
		public void text_is_not_strikethrough_if_todo_is_unchecked()
		{
			_todo.IsChecked = false;

			var text = SUT.FindComponent<MudText>().Find("p");

			Assert.DoesNotContain("line-through", text.GetStyle()["text-decoration"]);
		}

		[Fact]
		public void clicking_Todo_toggles_Checked_to_true()
		{
			SUT.Find("button").Click();

			Assert.True(_todo.IsChecked);
		}

		[Fact]
		public void toggling_Checked_triggers_OnUpdate()
		{
			Assert.Raises<EventArgs>(
				handler => SUT.SetParametersAndRender((nameof(TodoListItem.OnUpdate), handler.AsCallback())),
				_ => {},
				() => SUT.Find("p").Click());
		}

		[Fact]
		public void clicking_Confirm_callsOnUpdate()
		{
			Assert.Raises<EventArgs>(
				handler => SUT.SetParametersAndRender((nameof(TodoListItem.OnUpdate), handler.AsCallback())),
				_ => {},
				() =>
				{
					Enable_Editing();
					var button = SUT.FindComponent<MudIconButton>(btn => btn.Instance.Icon == Icons.Sharp.Check);
					button
						.Find("button")
						.Click();
				});
		}

		[Fact]
		public void Checkbox_is_Checked_when_Todo_is_Checked()
		{
			_todo.IsChecked = true;
			var checkbox = SUT.FindComponent<MudCheckBox<bool>>();
			
			Assert.True(checkbox.Instance.Checked);
		}

		[Fact]
		public void Checkbox_is_not_Checked_when_Todo_is_not_Checked()
		{
			_todo.IsChecked = false;
			var checkbox = SUT.FindComponent<MudCheckBox<bool>>();
			
			Assert.False(checkbox.Instance.Checked);
		}
		
	}
}