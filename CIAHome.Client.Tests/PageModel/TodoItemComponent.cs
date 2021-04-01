using Bunit;
using CIAHome.Client.Components.ListItems;

namespace CIAHome.Client.Tests.PageModel
{
	public class TodoItemComponent : ComponentBase<TodoItem>
	{
		/// <inheritdoc />
		public TodoItemComponent(IRenderedComponent<TodoItem> root) : base(root) { }
	}
}