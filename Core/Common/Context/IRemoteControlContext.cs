using Microsoft.EntityFrameworkCore;

namespace CIAHome.Core
{
	public interface IRemoteControlContext
	{
		DbSet<Pump>      Pumps      { get; set; }
		DbSet<Watertank> Watertanks { get; set; }
	}
}