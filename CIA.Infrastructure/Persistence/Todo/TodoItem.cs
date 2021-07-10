using System;
using System.ComponentModel.DataAnnotations;
using CIAHome.Core.Entities;

namespace CIA.Infrastructure
{
	public class TodoItem : ITodoItem
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString("N");

		public string Text    { get; set; } = string.Empty;
		public bool   IsChecked { get; set; }
	}
}