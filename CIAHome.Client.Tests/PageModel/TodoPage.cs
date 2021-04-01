using System.Collections.Generic;
using System.Linq;
using Bunit;
using CIAHome.Client.Components.Cards;
using CIAHome.Client.Components.ListItems;
using CIAHome.Client.Pages;
using MudBlazor;

namespace CIAHome.Client.Tests.PageModel
{
	public class TodoPage : ComponentBase<Todos>
	{
		public IRenderedComponent<MudButton> AddTodoButton     { get; }
		public IRenderedComponent<MudButton> AddTodoList { get; }

		public IEnumerable<TodoListCardComponent> ListCards
			=> Root.FindComponents<TodoListCard>().Select(card => new TodoListCardComponent(card));

		public IEnumerable<TodoItemComponent> TodoItems
			=> Root.FindComponents<TodoItem>().Select(item => new TodoItemComponent(item));
		
		public TodoPage(IRenderedComponent<Todos> root) : base(root)
		{
			var buttons = root.FindComponents<MudButton>();

			AddTodoButton     = buttons.FirstOrDefault(btn => btn.Markup.Contains("Add Todo"));
			AddTodoList = buttons.FirstOrDefault(btn => btn.Markup.Contains("Add List"));
		}
	}
}