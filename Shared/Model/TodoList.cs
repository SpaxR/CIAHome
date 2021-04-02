using System;

namespace CIAHome.Shared.Model
{
	public class TodoList
	{
		public string Id   { get; set; } = Guid.NewGuid().ToString("N");
		public string Text { get; set; } = "New Todo-List";
	}
}