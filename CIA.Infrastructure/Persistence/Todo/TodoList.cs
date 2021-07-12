using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIAHome.Core.Entities;

namespace CIA.Infrastructure
{
	public class TodoList : ITodoList<TodoItem>
	{
		[Key] public Guid                  Id    { get; set; }
		public       string                Text  { get; set; } = "New Todo-List";
		public       IEnumerable<TodoItem> Todos { get; set; } = new List<TodoItem>();
	}
}