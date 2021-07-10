using System.ComponentModel.DataAnnotations;
using CIAHome.Core.Entities;

namespace CIA.Infrastructure
{
	public class Pantry : IPantry
	{
		[Key] public string Id { get; set; }

	}
}