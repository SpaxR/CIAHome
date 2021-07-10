namespace CIAHome.Core.Entities
{
	public interface IPump : IUnique
	{
		public bool IsRunning { get; }
	}
}