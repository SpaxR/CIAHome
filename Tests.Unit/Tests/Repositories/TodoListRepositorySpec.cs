using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Client.Repositories;
using CIAHome.Shared.Models;
using Moq;
using Xunit;

namespace Tests.Unit.Repositories
{
	public class TodoListRepositorySpec
	{
		private readonly TodoListRepository         _sut;
		private readonly Mock<ILocalStorageService> _storageMock = new();

		public TodoListRepositorySpec()
		{
			_sut = new TodoListRepository(_storageMock.Object);
		}

		private TodoList[] SetupLists(params TodoList[] lists)
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(TodoList)))
						.ReturnsAsync(lists.Select(list => list.Id.ToString()).ToArray);

			foreach (TodoList list in lists)
			{
				_storageMock.Setup(storage => storage.GetItemAsync<TodoList>(list.Id.ToString()))
							.ReturnsAsync(list);
			}

			return lists;
		}


		[Fact]
		public async Task Create_returns_new_instance_of_TodoList()
		{
			var result = await _sut.Create();

			Assert.IsAssignableFrom<TodoList>(result);
		}

		[Fact]
		public async Task Create_stores_TodoList_in_storage()
		{
			var result = await _sut.Create();

			_storageMock.Verify(s => s.SetItemAsync(result.Id.ToString(), result), Times.Once);
		}

		[Fact]
		public async Task Create_saves_id_in_Storage_List()
		{
			var result = await _sut.Create();

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), new[] { result.Id.ToString() }));
		}

		[Fact]
		public async Task Find_returns_TodoList_matching_Predicate()
		{
			var list = SetupLists(new TodoList { Id = Guid.Empty }).First();

			var result = await _sut.Find(t => t.Id.Equals(list.Id));

			Assert.Equal(list, result);
		}

		[Fact]
		public async Task Find_without_Storage_entry_returns_null()
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(TodoList)))
						.ReturnsAsync((string[])null);

			var result = await _sut.Find(_ => true);

			Assert.Null(result);
		}

		[Fact]
		public async Task Find_Id_returns_TodoList_with_specified_Id()
		{
			var list = SetupLists(new TodoList { Id = Guid.Empty }).First();

			var result = await _sut.Find(list.Id.ToString());

			Assert.Equal(list.Id, result.Id);
			_storageMock.Verify(storage => storage.GetItemAsync<TodoList>(list.Id.ToString()));
		}

		[Fact]
		public async Task Find_Id_withNull_returns_uncategorized_TodoList()
		{
			var list = new TodoList { Id = Guid.Empty };
			list.AddTodo(new TodoItem());

			_storageMock.Setup(storage => storage.GetItemAsync<TodoList>(nameof(TodoItem))).ReturnsAsync(list);

			var result = await _sut.Find((string)null);

			Assert.Equal(list, result);
		}

		[Fact]
		public async Task All_returns_all_lists()
		{
			var lists = SetupLists(
				new TodoList { Id = Guid.NewGuid() },
				new TodoList { Id = Guid.NewGuid() },
				new TodoList { Id = Guid.NewGuid() });

			var result = await _sut.All();

			Assert.Equal(lists, result);
		}

		[Fact]
		public async Task All_without_Storage_entry_returns_empty_IEnumerable()
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(TodoList)))
						.ReturnsAsync((string[])null);

			var result = await _sut.All();

			Assert.NotNull(result);
		}

		[Fact]
		public async Task Update_stores_TodoList()
		{
			var list = new TodoList();

			await _sut.Update(list);

			_storageMock.Verify(storage => storage.SetItemAsync(list.Id.ToString(), list));
		}

		[Fact]
		public async Task Update_stores_unsaved_id_in_Storage_List()
		{
			var list = new TodoList();

			await _sut.Update(list);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), new[] { list.Id.ToString() }));
		}

		[Fact]
		public async Task Update_does_not_duplicate_id_in_Storage_List()
		{
			var list = SetupLists(new TodoList()).First();

			await _sut.Update(list);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), new[] { list.Id.ToString() }));
		}

		[Fact]
		public async Task Delete_removes_TodoList_from_Storage()
		{
			var list = SetupLists(new TodoList()).First();

			await _sut.Delete(list);

			_storageMock.Verify(storage => storage.RemoveItemAsync(list.Id.ToString()));
		}

		[Fact]
		public async Task Delete_removes_id_from_Storage_List()
		{
			var list = SetupLists(new TodoList()).First();

			await _sut.Delete(list);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), Array.Empty<string>()));
		}
	}
}