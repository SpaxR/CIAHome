using System;
using System.Linq;
using System.Net.Http;
using Bunit;
using Microsoft.AspNetCore.Components;
using Moq;
using Moq.Contrib.HttpClient;
using MudBlazor;

namespace Tests.Unit
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

		public static IHttpClientFactory CreateClientFactory(this Mock<HttpMessageHandler> handler,
															 Uri                           baseAddress,
															 string                        clientName = null)
		{
			var factory = handler.CreateClientFactory();

			Mock.Get(factory)
				.Setup(fac => fac.CreateClient(clientName ?? string.Empty))
				.Returns(() =>
				{
					var client = handler.CreateClient();
					client.BaseAddress = baseAddress;
					return client;
				});

			return factory;
		}
	}
}