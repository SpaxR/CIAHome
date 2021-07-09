using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Shared.Entities;
using CIAHome.Shared.Interfaces;

namespace CIAHome.Client.Repositories
{
	public class TodoListRepository : IAsyncRepository<TodoList>
	{
		private readonly ILocalStorageService _storage;

		public TodoListRepository(ILocalStorageService storage)
		{
			_storage = storage;
		}

		/// <inheritdoc />
		public async Task<IEnumerable<TodoList>> All()
		{
			string[] ids = await _storage.GetItemAsync<string[]>(nameof(TodoList)) ?? Array.Empty<string>();

			var loadLists = ids.Select(id => _storage.GetItemAsync<TodoList>(id).AsTask());

			return await Task.WhenAll(loadLists);
		}

		/// <inheritdoc />
		public Task<TodoList> Find(string id)
		{
			return _storage.GetItemAsync<TodoList>(id ?? nameof(Todo)).AsTask();
		}

		/// <inheritdoc />
		public async Task<TodoList> Find(Func<TodoList, bool> predicate)
		{
			string[] ids = await _storage.GetItemAsync<string[]>(nameof(TodoList)) ?? Array.Empty<string>();

			foreach (string id in ids)
			{
				var list = await _storage.GetItemAsync<TodoList>(id);

				if (predicate(list))
				{
					return list;
				}
			}

			return null;
		}

		/// <inheritdoc />
		public async Task<TodoList> Create()
		{
			var list = new TodoList();

			await _storage.SetItemAsync(list.Id, list);
			await UpdateIDs(ids => ids.Append(list.Id));

			return list;
		}

		/// <inheritdoc />
		public async Task Update(TodoList list)
		{
			await _storage.SetItemAsync(list.Id, list);
			await UpdateIDs(ids => ids.Append(list.Id).Distinct());
		}

		/// <inheritdoc />
		public async Task Delete(TodoList list)
		{
			await _storage.RemoveItemAsync(list.Id);
			await UpdateIDs(ids => ids.Except(new[] {list.Id}));
		}

		private async Task UpdateIDs(Func<IEnumerable<string>, IEnumerable<string>> update)
		{
			var ids = await _storage.GetItemAsync<string[]>(nameof(TodoList))
					  ?? Enumerable.Empty<string>();

			await _storage.SetItemAsync(nameof(TodoList), update(ids).ToArray());
		}
	}
}