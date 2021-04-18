using System;
using System.Threading.Tasks;
using CIAHome.Shared.EventArgs;
using CIAHome.Shared.Model;

namespace CIAHome.Shared.Interfaces
{
	public interface IPumpControlService
	{
		event EventHandler<WatertankEventArgs> WatertankUpdated;
		event EventHandler<PumpEventArgs> PumpUpdated;

		Task StartPump();
		Task StopPump();

		Task<WatertankStatus> Watertank();
		Task<PumpStatus>      Pump();
	}
}