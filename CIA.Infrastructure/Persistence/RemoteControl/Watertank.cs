using System.ComponentModel.DataAnnotations;
using CIAHome.Core.Entities;

namespace CIA.Infrastructure
{
	public class Watertank : IWatertank
	{
		/// <inheritdoc />
		[Key]
		public string Id { get; set; }

		/// <inheritdoc />
		public double Volume { get; set; }

		/// <inheritdoc />
		public double Filled { get; set; }
	}
}