using Bunit;
using CIAHome.Client.Components;
using MudBlazor;

namespace CIAHome.Client.Tests.PageModel
{
	public class ProductCardComponent : ComponentBase<ProductCard>
	{
		public IRenderedComponent<MudTextField<string>> GtinTextField => Root.FindComponent<MudTextField<string>>();


		/// <inheritdoc />
		public ProductCardComponent(IRenderedComponent<ProductCard> root) : base(root) { }
	}
}