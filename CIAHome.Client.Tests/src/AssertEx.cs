using System;
using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;

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

		public static IRenderedComponent<T> FindComponent<T>(this IRenderedComponent<IComponent> component,
															 Func<IRenderedComponent<T>, bool>   predicate)
			where T : IComponent
		{
			return component.FindComponents<T>().FirstOrDefault(predicate);
		}

		public static IRenderedComponent<MudIconButton> FindIconButton(this IRenderedComponent<IComponent> component,
																	   string                              icon)
		{
			return component.FindComponent<MudIconButton>(btn => btn.Instance.Icon.Equals(icon));
		}
	}
}