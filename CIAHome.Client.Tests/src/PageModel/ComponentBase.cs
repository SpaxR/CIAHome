using Bunit;
using Microsoft.AspNetCore.Components;

namespace CIAHome.Client.Tests.PageModel
{
	public abstract class ComponentBase<T> where T : IComponent
	{
		public IRenderedComponent<T> Root { get; }

		protected ComponentBase(IRenderedComponent<T> root)
		{
			Root = root;
		}
	}
}