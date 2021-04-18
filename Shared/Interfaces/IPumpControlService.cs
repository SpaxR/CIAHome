using System;
using System.Threading.Tasks;
using CIAHome.Shared.EventArgs;

namespace CIAHome.Shared.Interfaces
{
	public interface IPumpControlService
	{
		event EventHandler<WatertankEventArgs> WatertankStatusUpdated;
		event EventHandler<PumpEventArgs> PumpStatusUpdated;

		Task StartPump();
		Task StopPump();
	}
}