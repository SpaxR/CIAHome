using System.Threading.Tasks;
using CIAHome.Shared.Models;

namespace CIAHome.Shared.Interfaces
{
	public interface IPumpControlCallback
	{
		public Task UpdatePump(PumpStatus           status);
		public Task UpdateWatertank(WatertankStatus status);
	}
}