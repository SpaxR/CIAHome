using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Shared.Models;

namespace Tests.Unit.PageModel
{
	public class TodoListCardComponent : ComponentBase<TodoListCard>
	{
		public TodoList List => Root.Instance.List;

		/// <inheritdoc />
		public TodoListCardComponent(IRenderedComponent<TodoListCard> root) : base(root) { }

		public void InvokeDelete() => Root.InvokeAsync(Root.Instance.OnDelete.InvokeAsync);
	}
}