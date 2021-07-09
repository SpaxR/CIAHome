using System;
using System.Threading.Tasks;
using CIAHome.Shared.Model;

namespace CIAHome.Shared.Interfaces
{
	public interface IPumpControlService
	{
		event EventHandler<WatertankEventArgs> WatertankUpdated;
		event EventHandler<PumpEventArgs>      PumpUpdated;

		Task StartPump();
		Task StopPump();

		Task<WatertankStatus> WatertankStatus();
		Task<PumpStatus>      PumpStatus();
	}
	
	public class PumpEventArgs : EventArgs
	{
		public PumpStatus Status { get; set; }

		public PumpEventArgs(PumpStatus status)
		{
			Status = status;
		}
	}
	
	public class WatertankEventArgs : EventArgs
	{
		public WatertankStatus Status { get; }

		/// <inheritdoc />
		public WatertankEventArgs(WatertankStatus status)
		{
			Status = status;
		}
	}
}