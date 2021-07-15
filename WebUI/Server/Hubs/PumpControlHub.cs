using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebUI.Shared.Interfaces;
using WebUI.Shared.Models;

namespace WebUI.Server.Hubs
{
	public class PumpControlHub : Hub<IPumpControlCallback>
	{
		private readonly ILogger<PumpControlHub> _log;

		private WatertankStatus _watertank;
		private PumpStatus      _pump;

		public PumpControlHub(ILogger<PumpControlHub> log)
		{
			_log = log;
		}

		public Task<WatertankStatus> WatertankStatus()
		{
			return Task.FromResult(_watertank);
		}

		public Task UpdateWatertank(WatertankStatus status)
		{
			_watertank = status;
			Clients.All.UpdateWatertank(status);
			return Task.CompletedTask;
		}

		public Task<PumpStatus> PumpStatus()
		{
			return Task.FromResult(_pump);
		}

		public Task UpdatePump(PumpStatus status)
		{
			_pump = status;
			Clients.All.UpdatePump(status);
			return Task.CompletedTask;
		}
	}
}