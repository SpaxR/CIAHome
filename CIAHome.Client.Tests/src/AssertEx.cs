using System;
using Microsoft.AspNetCore.Components;

namespace CIAHome.Client.Tests
{
	public static class AssertEx
	{
		public static EventCallback<T> AsCallback<T>(this EventHandler<T> handler)
		{
			return EventCallback.Factory
								.Create<T>(
									handler,
									args => handler.Invoke(null, args));
		}
	}
}