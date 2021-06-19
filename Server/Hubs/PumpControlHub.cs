using System.Threading.Tasks;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CIAHome.Server.Hubs
{
	public class PumpControlHub : Hub
	{
		private readonly ILogger<PumpControlHub> _log;

		private WatertankStatus _watertank;
		private PumpStatus      _pump;

		public PumpControlHub(ILogger<PumpControlHub> log)
		{
			_log = log;
		}
		
		public Task UpdateStatus(IPumpControlUpdate status)
		{
			switch (status)
			{
				case WatertankStatus watertank:
					_watertank = watertank;
					break;
				case PumpStatus pump:
					_pump = pump;
					break;
			}

			return Task.CompletedTask;
		}
		
		public Task<WatertankStatus> WatertankStatus()
		{
			return Task.FromResult(_watertank);
		}

		public Task<PumpStatus> PumpStatus()
		{
			return Task.FromResult(_pump);
		}
	}
}