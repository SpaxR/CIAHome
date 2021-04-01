using Bunit;
using CIAHome.Client.Components.Cards;

namespace CIAHome.Client.Tests.PageModel
{
	public class TodoListCardComponent : ComponentBase<TodoListCard>
	{
		/// <inheritdoc />
		public TodoListCardComponent(IRenderedComponent<TodoListCard> root) : base(root) { }
	}
}