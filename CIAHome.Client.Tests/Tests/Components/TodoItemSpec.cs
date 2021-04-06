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
			SUT.FindComponent<MudIconButton>().Find("button").Click();

			Assert.NotNull(SUT.FindComponent<MudInput<string>>());
		}

		[Fact]
		public void editing_Text_changes_Text_of_Todo()
		{
			SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Create)
			   .Find("button")
			   .Click();
			var input = SUT.FindComponent<MudInput<string>>();

			input.Find("input").Change("SOME TEXT");

			Assert.Equal("SOME TEXT", _todo.Text);
		}

		[Fact]
		public void editing_Text_updates_Todo_in_Repository()
		{
			SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Create)
			   .Find("button")
			   .Click();
			var input = SUT.FindComponent<MudInput<string>>();

			input.Find("input").Change("SOME TEXT");

			_todoRepository.Verify(repo => repo.Update(_todo));
		}

		[Fact]
		public void editing_Text_shows_Confirm_IconButton()
		{
			SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Create)
			   .Find("button")
			   .Click();

			var button = SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Check);
			
			Assert.NotNull(button);
		}

		[Fact]
		public void clicking_confirm_IconButton_changes_Input_to_Text()
		{
			SUT.FindComponent<MudIconButton>(b => b.Instance.Icon == Icons.Sharp.Create)
			   .Find("button")
			   .Click();
			
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
	}
}