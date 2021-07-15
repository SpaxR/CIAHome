using System.Threading.Tasks;
using WebUI.Shared.Models;

namespace WebUI.Shared.Interfaces
{
	public interface IPumpControlCallback
	{
		public Task UpdatePump(PumpStatus           status);
		public Task UpdateWatertank(WatertankStatus status);
	}
}