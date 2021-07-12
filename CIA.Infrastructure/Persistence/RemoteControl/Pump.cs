using System;
using System.ComponentModel.DataAnnotations;
using CIAHome.Core.Entities;

namespace CIA.Infrastructure
{
	public class Pump : IPump
	{
		/// <inheritdoc />
		[Key]
		public Guid Id { get; set; }

		/// <inheritdoc />
		public bool IsRunning { get; set; }
	}
}