using Bunit;
using CIAHome.Client.Components.ListItems;
using CIAHome.Shared.Model;

namespace CIAHome.Client.Tests.PageModel
{
	public class TodoItemComponent : ComponentBase<TodoItem>
	{
		public Todo Todo => Root.Instance.Todo;

		/// <inheritdoc />
		public TodoItemComponent(IRenderedComponent<TodoItem> root) : base(root) { }

		public void InvokeDelete() => Root.InvokeAsync(Root.Instance.OnDelete.InvokeAsync);
	}
}