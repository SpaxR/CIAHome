using Bunit;
using CIAHome.Client.Components.Todo;
using CIAHome.Client.Pages.Todo;
using CIAHome.Shared.Model;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class TodoDetailSpec : TestContext
	{
		private IRenderedComponent<TodoDetail> _sut;


		private IRenderedComponent<TodoDetail> SUT
		{
			get
			{
				_sut ??= RenderComponent<TodoDetail>();
				return _sut;
			}
		}

		// [Fact]
		// public void WithId_loads_Todos_of_List_in_detail_View()
		// {
		// 	var list = new TodoList();
		// 	list.Todos.Add(new Todo());
		// 	list.Todos.Add(new Todo());
		//
		// 	_listRepositoryMock.Setup(repo => repo.Find(list.Id))
		// 					   .ReturnsAsync(list);
		//
		// 	var sut = RenderComponent<TodoMaster>((nameof(TodoMaster.Id), list.Id));
		//
		// 	var cards = SUT.FindComponents<TodoListCard>();
		//
		// 	Assert.Equal(list.Todos.Count, cards.Count);
		// }
	}
}