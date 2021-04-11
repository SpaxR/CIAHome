using System;
using System.Net.Http;
using Moq;
using Moq.Contrib.HttpClient;

namespace CIAHome.Client.Tests
{
	public static class AssertEx
	{
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