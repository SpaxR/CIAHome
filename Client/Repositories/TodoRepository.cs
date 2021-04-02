using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;

namespace CIAHome.Client.Repositories
{
	public class TodoRepository : IAsyncRepository<Todo>
	{
		private readonly ILocalStorageService _storage;

		public TodoRepository(ILocalStorageService storage)
		{
			_storage = storage;
		}

		/// <inheritdoc />
		public async Task<IEnumerable<Todo>> All()
		{
			string[] ids = await _storage.GetItemAsync<string[]>(nameof(Todo)) ?? Array.Empty<string>();

			var loadTodos = ids.Select(id => _storage.GetItemAsync<Todo>(id).AsTask());

			return await Task.WhenAll(loadTodos);
		}

		/// <inheritdoc />
		public async Task<Todo> Find(Func<Todo, bool> predicate)
		{
			string[] ids = await _storage.GetItemAsync<string[]>(nameof(Todo)) ?? Array.Empty<string>();

			foreach (string id in ids)
			{
				var todo = await _storage.GetItemAsync<Todo>(id);

				if (predicate(todo))
				{
					return todo;
				}
			}

			return null;
		}

		/// <inheritdoc />
		public async Task<Todo> Create()
		{
			var todo = new Todo();

			await _storage.SetItemAsync(todo.Id, todo);
			await UpdateIDs(ids => ids.Append(todo.Id));

			return todo;
		}

		/// <inheritdoc />
		public async Task Update(Todo todo)
		{
			await _storage.SetItemAsync(todo.Id, todo);
			await UpdateIDs(ids => ids.Append(todo.Id));
		}

		/// <inheritdoc />
		public async Task Delete(Todo todo)
		{
			await _storage.RemoveItemAsync(todo.Id);
			await UpdateIDs(ids => ids.Except(new[] {todo.Id}));
		}

		private async Task UpdateIDs(Func<IEnumerable<string>, IEnumerable<string>> update)
		{
			var ids = await _storage.GetItemAsync<IEnumerable<string>>(nameof(Todo))
					  ?? Enumerable.Empty<string>();

			await _storage.SetItemAsync(nameof(Todo), update(ids).ToArray());
		}
	}
}