using System;
using System.ComponentModel.DataAnnotations;
using CIAHome.Core.Entities;

namespace CIA.Infrastructure
{
	public class TodoItem : ITodoItem
	{
		[Key]
		public Guid Id { get; set; }

		public string Text    { get; set; } = string.Empty;
		public bool   IsChecked { get; set; }
	}
}