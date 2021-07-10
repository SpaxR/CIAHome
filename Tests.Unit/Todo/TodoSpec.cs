﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIAHome.Shared.Models;
using Xunit;

namespace Tests.Unit
{
	public class TodoSpec
	{
		private TodoItem _sut;
		
		public TodoSpec()
		{
			_sut = new TodoItem();
		}
		
		[Fact]
		public void Id_is_not_null_or_whitespace()
		{
			Assert.NotEmpty(_sut.Id.Trim());
		}

		[Fact]
		public void Id_is_unique()
		{
			IList<string> ids = new List<string>();
			Parallel.For(0, 10, _ => ids.Add(new TodoItem().Id));

			Assert.Equal(ids, ids.Distinct());
		}

		[Fact]
		public void Id_is_GUID()
		{
			bool result = Guid.TryParse(_sut.Id, out _);
			Assert.True(result);
		}

		[Fact]
		public void Text_is_not_null()
		{
			Assert.NotNull(_sut.Text);
		}

	}
}