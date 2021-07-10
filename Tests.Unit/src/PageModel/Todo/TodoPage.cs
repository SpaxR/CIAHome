using System.Collections.Generic;
using System.Linq;
using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Client.Pages.Todo;
using MudBlazor;

namespace Tests.Unit.PageModel
{
	public class TodoPage : ComponentBase<TodoMaster>
	{
		public IRenderedComponent<MudButton> AddTodoBtn     { get; }
		public IRenderedComponent<MudButton> AddTodoListBtn { get; }

		public IEnumerable<TodoListCardComponent> ListCards
			=> Root.FindComponents<TodoListCard>().Select(card => new TodoListCardComponent(card));

		public IEnumerable<TodoItemComponent> TodoItems
			=> Root.FindComponents<TodoListItem>().Select(item => new TodoItemComponent(item));
		
		public TodoPage(IRenderedComponent<TodoMaster> root) : base(root)
		{
			var buttons = root.FindComponents<MudButton>();

			AddTodoBtn     = buttons.FirstOrDefault(btn => btn.Markup.Contains("Add Todo"));
			AddTodoListBtn = buttons.FirstOrDefault(btn => btn.Markup.Contains("Add List"));
		}
	}
}