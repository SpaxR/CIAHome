using System;
using System.Linq;
using Bunit;
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

		public static IRenderedComponent<TR> FindComponent<TR>(this IRenderedComponent<IComponent>        component,
																  Func<IRenderedComponent<TR>, bool> predicate)
			where TR : IComponent
		{
			return component.FindComponents<TR>().FirstOrDefault(predicate);
		}
	}
}