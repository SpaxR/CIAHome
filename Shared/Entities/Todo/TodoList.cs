using System;
using System.Collections.Generic;

namespace CIAHome.Shared.Entities
{
	public class TodoList
	{
		public string      Id    { get; set; } = Guid.NewGuid().ToString("N");
		public string      Text  { get; set; } = "New Todo-List";
		public IList<Todo> Todos { get; set; } = new List<Todo>();
	}
}