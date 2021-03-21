using System.Collections.Generic;
using CIAHome.Shared.Model;

namespace CIAHome.Client.Services
{
	public interface ITodoService
	{
		IEnumerable<Todo> FindAll();
		void Create();
		void Delete();
	}
	
	public class TodoService : ITodoService
	{
		/// <inheritdoc />
		public IEnumerable<Todo> FindAll()
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public void Create()
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public void Delete()
		{
			throw new System.NotImplementedException();
		}
	}
}