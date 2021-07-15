using Bunit;
using WebUI.Client.Components.Todo;
using WebUI.Shared.Models;

namespace WebUI.Tests.UnitTests.PageModel
{
	public class TodoItemComponent : ComponentBase<TodoListItem>
	{
		public TodoItem Todo => Root.Instance.Todo;

		/// <inheritdoc />
		public TodoItemComponent(IRenderedComponent<TodoListItem> root) : base(root) { }

		public void InvokeDelete() => Root.InvokeAsync(Root.Instance.OnDelete.InvokeAsync);
	}
}