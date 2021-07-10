using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Shared.Models;

namespace Tests.Unit.PageModel
{
	public class TodoItemComponent : ComponentBase<TodoListItem>
	{
		public TodoItem Todo => Root.Instance.Todo;

		/// <inheritdoc />
		public TodoItemComponent(IRenderedComponent<TodoListItem> root) : base(root) { }

		public void InvokeDelete() => Root.InvokeAsync(Root.Instance.OnDelete.InvokeAsync);
	}
}