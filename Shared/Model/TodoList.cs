using System;

namespace CIAHome.Shared.Model
{
	public class TodoList
	{
		public string Id   { get; } = Guid.NewGuid().ToString("N");
		public string Text { get; } = "New Todo-List";
	}
}