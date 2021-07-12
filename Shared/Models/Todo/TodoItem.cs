using System;
using CIAHome.Core.Entities;

namespace CIAHome.Shared.Models
{
	public class TodoItem : ITodoItem
	{
		/// <inheritdoc />
		public Guid Id { get; set; }

		/// <inheritdoc />
		public string Text { get; set; } = "ToDo";

		/// <inheritdoc />
		public bool IsChecked { get; set; }
	}
}