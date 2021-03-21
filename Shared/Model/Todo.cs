using System;

namespace CIAHome.Shared.Model
{
	public class Todo
	{
		public string Id { get; } = Guid.NewGuid().ToString("N");

		public string Text    { get; } = string.Empty;
		public bool   Checked { get; set; }

		public Todo() { }
	}
}