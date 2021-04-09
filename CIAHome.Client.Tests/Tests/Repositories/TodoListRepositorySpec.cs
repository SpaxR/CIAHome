using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Client.Repositories;
using CIAHome.Shared.Model;
using Moq;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class TodoListRepositorySpec
	{
		private readonly TodoListRepository         _sut;
		private readonly Mock<ILocalStorageService> _storageMock = new();

		public TodoListRepositorySpec()
		{
			_sut = new TodoListRepository(_storageMock.Object);
		}

		private void SetupLists(params TodoList[] lists)
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(TodoList)))
						.ReturnsAsync(lists.Select(list => list.Id).ToArray);

			foreach (TodoList list in lists)
			{
				_storageMock.Setup(storage => storage.GetItemAsync<TodoList>(list.Id))
							.ReturnsAsync(list);
			}
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

			_storageMock.Verify(s => s.SetItemAsync(result.Id, result), Times.Once);
		}

		[Fact]
		public async Task Create_saves_id_in_Storage_List()
		{
			var result = await _sut.Create();

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), new[] {result.Id}));
		}

		[Fact]
		public async Task Find_returns_TodoList_matching_Predicate()
		{
			var list = new TodoList();
			SetupLists(list);

			var result = await _sut.Find(t => t.Id.Equals(list.Id));

			Assert.Equal(list, result);
		}

		[Fact]
		public async Task Find_without_Storage_entry_returns_null()
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(TodoList)))
						.ReturnsAsync((string[]) null);

			var result = await _sut.Find(_ => true);

			Assert.Null(result);
		}

		[Fact]
		public async Task Find_Id_returns_TodoList_with_specified_Id()
		{
			var list = new TodoList();
			SetupLists(list);

			var result = await _sut.Find(list.Id);

			Assert.Equal(list.Id, result.Id);
			_storageMock.Verify(storage => storage.GetItemAsync<TodoList>(list.Id));
		}

		[Fact]
		public async Task Find_Id_withNull_returns_uncategorized_TodoList()
		{
			var list = new TodoList {Id = null, Todos = {new Todo()}};
			_storageMock.Setup(storage => storage.GetItemAsync<TodoList>(nameof(Todo))).ReturnsAsync(list);

			var result = await _sut.Find((string) null);

			Assert.Equal(list, result);
		}

		[Fact]
		public async Task All_returns_all_lists()
		{
			var lists = new TodoList[] {new(), new(), new()};
			SetupLists(lists);

			var result = await _sut.All();

			Assert.Equal(lists.ToList(), result);
		}

		[Fact]
		public async Task All_without_Storage_entry_returns_empty_IEnumerable()
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(TodoList)))
						.ReturnsAsync((string[]) null);

			var result = await _sut.All();

			Assert.NotNull(result);
		}

		[Fact]
		public async Task Update_stores_TodoList()
		{
			var list = new TodoList();

			await _sut.Update(list);

			_storageMock.Verify(storage => storage.SetItemAsync(list.Id, list));
		}

		[Fact]
		public async Task Update_stores_unsaved_id_in_Storage_List()
		{
			var list = new TodoList();

			await _sut.Update(list);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), new[] {list.Id}));
		}

		[Fact]
		public async Task Update_does_not_duplicate_id_in_Storage_List()
		{
			var list = new TodoList();
			SetupLists(list);

			await _sut.Update(list);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), new[] {list.Id}));
		}
		
		[Fact]
		public async Task Delete_removes_TodoList_from_Storage()
		{
			var list = new TodoList();
			SetupLists(list);

			await _sut.Delete(list);

			_storageMock.Verify(storage => storage.RemoveItemAsync(list.Id));
		}

		[Fact]
		public async Task Delete_removes_id_from_Storage_List()
		{
			var list = new TodoList();
			SetupLists(list);

			await _sut.Delete(list);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(TodoList), Array.Empty<string>()));
		}
		
	}
}