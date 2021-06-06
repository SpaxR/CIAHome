using System.Threading.Tasks;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CIAHome.Server.Hubs
{
	public class ProviderHub : Hub
	{
		private readonly ILogger<ProviderHub> _log;

		private WatertankStatus _watertank;
		private PumpStatus      _pump;

		public ProviderHub(ILogger<ProviderHub> log)
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
			return Task.CompletedTask;
		}

		public Task<PumpStatus> PumpStatus()
		{
			return Task.FromResult(_pump);
		}

		public Task UpdatePump(PumpStatus status)
		{
			_pump = status;
			return Task.CompletedTask;
		}
	}
}