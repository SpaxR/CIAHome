using Bunit;
using CIAHome.Client.Components;

namespace CIAHome.Client.Tests.PageModel
{
	public class ProductCardComponent : ComponentBase<ProductCard>
	{
		/// <inheritdoc />
		public ProductCardComponent(IRenderedComponent<ProductCard> root) : base(root) { }
	}
}