using Microsoft.EntityFrameworkCore;

namespace CIAHome.Core
{
	public interface IPantryContext
	{
		DbSet<Pantry> Pantries { get; set; }
	}
}