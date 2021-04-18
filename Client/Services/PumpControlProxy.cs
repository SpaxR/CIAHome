using System;
using System.Threading.Tasks;
using CIAHome.Shared;
using CIAHome.Shared.EventArgs;
using CIAHome.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace CIAHome.Client.Services
{
	// Warning: This class is untested!
	public class PumpControlProxy : IPumpControlService
	{
		private readonly HubConnection _connection;

		public PumpControlProxy()
		{
			_connection = new HubConnectionBuilder()
						  .WithUrl(CIAPaths.PumpControlHub)
						  .Build();

			_connection.On<PumpEventArgs>(nameof(PumpStatusUpdated),
										  args => PumpStatusUpdated?.Invoke(this, args));
			_connection.On<WatertankEventArgs>(nameof(WatertankStatusUpdated),
											   args => WatertankStatusUpdated?.Invoke(this, args));
		}

		/// <inheritdoc />
		public event EventHandler<WatertankEventArgs> WatertankStatusUpdated;

		/// <inheritdoc />
		public event EventHandler<PumpEventArgs> PumpStatusUpdated;

		/// <inheritdoc />
		public async Task StartPump()
		{
			await EnsureConnection();
			await _connection.InvokeAsync(nameof(StartPump));
		}

		/// <inheritdoc />
		public async Task StopPump()
		{
			await EnsureConnection();
			await _connection.InvokeAsync(nameof(StopPump));
		}

		private Task EnsureConnection()
		{
			return _connection.State == HubConnectionState.Disconnected
				? _connection.StartAsync()
				: Task.CompletedTask;
		}
	}
}