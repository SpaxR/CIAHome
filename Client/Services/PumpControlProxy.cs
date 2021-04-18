using System;
using System.Threading.Tasks;
using CIAHome.Shared;
using CIAHome.Shared.EventArgs;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
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

			_connection.On<PumpEventArgs>(nameof(PumpUpdated),
										  args => PumpUpdated?.Invoke(this, args));
			_connection.On<WatertankEventArgs>(nameof(WatertankUpdated),
											   args => WatertankUpdated?.Invoke(this, args));
		}

		/// <inheritdoc />
		public event EventHandler<WatertankEventArgs> WatertankUpdated;

		/// <inheritdoc />
		public event EventHandler<PumpEventArgs> PumpUpdated;

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

		/// <inheritdoc />
		public async Task<WatertankStatus> Watertank()
		{
			await EnsureConnection();
			return await _connection.InvokeAsync<WatertankStatus>(nameof(Watertank));
		}

		/// <inheritdoc />
		public async Task<PumpStatus> Pump()
		{
			await EnsureConnection();
			return await _connection.InvokeAsync<PumpStatus>(nameof(Pump));
		}

		private Task EnsureConnection()
		{
			return _connection.State == HubConnectionState.Disconnected
				? _connection.StartAsync()
				: Task.CompletedTask;
		}
	}
}