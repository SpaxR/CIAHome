using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Xunit;
using Xunit.Sdk;

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

		private static bool IsEquivalent<T>(T expected, T actual, int depth = 0)
		{
			foreach (PropertyInfo info in typeof(T).GetProperties().Where(prop => prop.CanRead))
			{
				if (depth <= 0)
				{
					object expectedValue = info.GetValue(expected);
					object actualValue   = info.GetValue(actual);

					if (expectedValue != null && expectedValue != actualValue && !expectedValue.Equals(actualValue))
					{
						return false;
					}
				}
				else
				{
					return IsEquivalent(info.GetValue(expected), info.GetValue(actual), depth - 1);
				}
			}

			return true;
		}

		public static void Equivalent<T>(T expected, T actual, int depth = 0)
		{
			Assert.True(IsEquivalent(expected, actual, depth));
		}

		public static void ContainsEquivalent<T>(T expected, IEnumerable<T> collection, int depth = 0)
		{
			if (!collection.Any(item => IsEquivalent(expected, item, depth)))
			{
				throw new ContainsException(expected, collection);
			}
		}

		public static void DoesNotContainEquivalent<T>(T expected, IEnumerable<T> collection, int depth = 0)
		{
			if (collection.Any(item => IsEquivalent(expected, item, depth)))
			{
				throw new DoesNotContainException(expected, collection);
			}
		}
	}
}