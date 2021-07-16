using Bunit;
using WebUI.Client.Components.Todo;
using WebUI.Shared.Models;

namespace WebUI.Tests.UnitTests.PageModel
{
	public class TodoListCardComponent : ComponentBase<TodoListCard>
	{
		public TodoList List => Root.Instance.List;

		/// <inheritdoc />
		public TodoListCardComponent(IRenderedComponent<TodoListCard> root) : base(root) { }

		public void InvokeDelete() => Root.InvokeAsync(Root.Instance.OnDelete.InvokeAsync);
	}
}