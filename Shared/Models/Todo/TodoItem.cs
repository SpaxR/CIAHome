using System;
using CIAHome.Core.Entities;

namespace CIAHome.Shared.Models
{
	public class TodoItem : ITodoItem
	{
		/// <inheritdoc />
		public string Id { get; set; } = Guid.NewGuid().ToString("N");

		/// <inheritdoc />
		public string Text { get; set; } = "ToDo";

		/// <inheritdoc />
		public bool IsChecked { get; set; }
	}
}