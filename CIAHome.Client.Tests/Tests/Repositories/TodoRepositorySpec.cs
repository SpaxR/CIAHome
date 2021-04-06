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
	public class TodoRepositorySpec
	{
		private readonly TodoRepository             _sut;
		private readonly Mock<ILocalStorageService> _storageMock = new();

		public TodoRepositorySpec()
		{
			_sut = new TodoRepository(_storageMock.Object);
		}

		private void SetupTodos(params Todo[] todos)
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(Todo)))
						.ReturnsAsync(todos.Select(todo => todo.Id).ToArray);

			foreach (Todo todo in todos)
			{
				_storageMock.Setup(storage => storage.GetItemAsync<Todo>(todo.Id))
							.ReturnsAsync(todo);
			}
		}


		[Fact]
		public async Task Create_returns_new_instance_of_Todo()
		{
			var result = await _sut.Create();

			Assert.IsAssignableFrom<Todo>(result);
		}

		[Fact]
		public async Task Create_stores_Todo_in_storage()
		{
			var result = await _sut.Create();

			_storageMock.Verify(s => s.SetItemAsync(result.Id, result), Times.Once);
		}

		[Fact]
		public async Task Create_saves_id_in_Storage_List()
		{
			var result = await _sut.Create();

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(Todo), new[] {result.Id}));
		}

		[Fact]
		public async Task Find_returns_Todo_matching_Predicate()
		{
			var todo = new Todo();
			SetupTodos(todo);

			var result = await _sut.Find(t => t.Id.Equals(todo.Id));

			Assert.Equal(todo, result);
		}

		[Fact]
		public async Task Find_without_Storage_entry_returns_null()
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(Todo)))
						.ReturnsAsync((string[]) null);

			var result = await _sut.Find(_ => true);

			Assert.Null(result);
		}

		[Fact]
		public async Task All_returns_all_todos()
		{
			var todos = new Todo[] {new(), new(), new()};
			SetupTodos(todos);

			var result = await _sut.All();

			Assert.Equal(todos.ToList(), result);
		}

		[Fact]
		public async Task All_without_Storage_entry_returns_empty_IEnumerable()
		{
			_storageMock.Setup(storage => storage.GetItemAsync<string[]>(nameof(Todo)))
						.ReturnsAsync((string[]) null);

			var result = await _sut.All();

			Assert.NotNull(result);
		}

		[Fact]
		public async Task Update_stores_Todo()
		{
			var todo = new Todo();

			await _sut.Update(todo);

			_storageMock.Verify(storage => storage.SetItemAsync(todo.Id, todo));
		}

		[Fact]
		public async Task Update_stores_unsaved_id_in_Storage_List()
		{
			var todo = new Todo();

			await _sut.Update(todo);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(Todo), new[] {todo.Id}));
		}

		[Fact]
		public async Task Update_does_not_duplicate_id_in_Storage_List()
		{
			var todo = new Todo();
			SetupTodos(todo);

			await _sut.Update(todo);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(Todo), new[] {todo.Id}));
		}

		[Fact]
		public async Task Delete_removes_Todo_from_Storage()
		{
			var todo = new Todo();
			SetupTodos(todo);

			await _sut.Delete(todo);

			_storageMock.Verify(storage => storage.RemoveItemAsync(todo.Id));
		}

		[Fact]
		public async Task Delete_removes_id_from_Storage_List()
		{
			var todo = new Todo();
			SetupTodos(todo);

			await _sut.Delete(todo);

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(Todo), Array.Empty<string>()));
		}
	}
}