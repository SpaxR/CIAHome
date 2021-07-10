using System.Collections.Generic;

namespace CIAHome.Core.Entities
{
	public interface ITodoList<out T> : IUnique where T : ITodoItem
	{
		public string         Text  { get; }
		public IEnumerable<T> Todos { get; }
	}
}