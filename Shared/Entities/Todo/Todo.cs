using System;

namespace CIAHome.Shared.Entities
{
	public class Todo
	{
		public string Id { get; set; } = Guid.NewGuid().ToString("N");

		public string Text    { get; set; } = string.Empty;
		public bool   Checked { get; set; }
	}
}