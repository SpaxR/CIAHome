using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CIAHome.Shared.Model
{
	public class TodoListSpec
	{
		private readonly TodoList _sut = new();
		
		
		[Fact]
		public void Id_is_not_null_or_whitespace()
		{
			Assert.NotEmpty(_sut.Id.Trim());
		}

		[Fact]
		public void Id_is_unique()
		{
			List<string> ids = new List<string>
			{
				new TodoList().Id,
				new TodoList().Id,
				new TodoList().Id,
			};

			Assert.Equal(ids, ids.Distinct());
		}

		[Fact]
		public void Id_is_GUID()
		{
			bool result = Guid.TryParse(_sut.Id, out _);
			
			Assert.True(result);
		}


		[Fact]
		public void Text_is_not_null_or_whitespace()
		{
			Assert.NotEmpty(_sut.Text);
		}
		
	}
}