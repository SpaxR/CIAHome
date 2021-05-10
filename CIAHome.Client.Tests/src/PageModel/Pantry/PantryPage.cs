using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Pages;
using MudBlazor;

namespace CIAHome.Client.Tests.PageModel
{
	public class PantryPage : ComponentBase<Pantry>
	{

		public ProductCardComponent CurrentProduct => new(Root.FindComponent<ProductCard>());

		public IRenderedComponent<MudTextField<string>> InputTextField => Root.FindComponent<MudTextField<string>>();
		
		
		
		/// <inheritdoc />
		public PantryPage(IRenderedComponent<Pantry> root) : base(root) { }
	}
}