using Bunit;
using CIAHome.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CIAHome.Client.Pages
{
	public sealed class TodoSpec : TestContext
	{
		private readonly Mock<ITodoService> _todoServiceMock = new();

		private IRenderedComponent<Todos> SUT { get; }

		public TodoSpec()
		{
			Services.AddScoped(_ => _todoServiceMock.Object);
			SUT = RenderComponent<Todos>();
		}
	}
}