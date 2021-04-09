using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIAHome.Shared.Interfaces
{
	// Notice: May be an anti-pattern, we'll see.

	public interface IRepository<T>
	{
		IEnumerable<T> All();

		T Find(string        id);
		T Find(Func<T, bool> predicate);

		T    Create();
		void Update(T entity);
		void Delete(T entity);
	}

	public interface IAsyncRepository<T>
	{
		Task<IEnumerable<T>> All();

		Task<T> Find(string        id);
		Task<T> Find(Func<T, bool> predicate);

		Task<T> Create();
		Task    Update(T entity);
		Task    Delete(T entity);
	}
}