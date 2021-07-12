using System;
using CIAHome.Core.Entities;

namespace CIAHome.Shared.Models
{
	public class Pantry : IPantry
	{
		/// <inheritdoc />
		public Guid Id { get; set; }
	}
}