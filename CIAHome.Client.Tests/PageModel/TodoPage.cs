using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using CIAHome.Client.Components.Cards;
using CIAHome.Client.Components.ListItems;
using CIAHome.Client.Pages;
using MudBlazor;

namespace CIAHome.Client.Tests.PageModel
{
	public class TodoPage : ComponentBase<Todos>
	{
		public IRenderedComponent<MudButton> AddTodoBtn     { get; }
		public IRenderedComponent<MudButton> AddTodoListBtn { get; }

		public IEnumerable<TodoListCardComponent> ListCards
			=> Root.FindComponents<TodoListCard>().Select(card => new TodoListCardComponent(card));

		public IEnumerable<TodoItemComponent> TodoItems
			=> Root.FindComponents<TodoItem>().Select(item => new TodoItemComponent(item));
		
		public TodoPage(IRenderedComponent<Todos> root) : base(root)
		{
			var buttons = root.FindComponents<MudButton>();

			AddTodoBtn     = buttons.FirstOrDefault(btn => btn.Markup.Contains("Add Todo"));
			AddTodoListBtn = buttons.FirstOrDefault(btn => btn.Markup.Contains("Add List"));
		}
	}
}