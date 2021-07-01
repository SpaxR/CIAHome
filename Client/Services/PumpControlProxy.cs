using System;
using System.Threading.Tasks;
using CIAHome.Shared;
using CIAHome.Shared.EventArgs;
using CIAHome.Shared.Interfaces;
using CIAHome.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace CIAHome.Client.Services
{
	// Warning: This class is untested!
	public class PumpControlProxy : IPumpControlService
	{
		private readonly ILogger<PumpControlProxy> _log;
		private readonly HubConnection             _connection;

		public PumpControlProxy(ILogger<PumpControlProxy> log, NavigationManager navigation) : this(log)
		{
			_connection = new HubConnectionBuilder()
						  .WithUrl(navigation.ToAbsoluteUri(CIAPaths.PumpControlHub))
						  .Build();

			RegisterCallbacks();
		}

		public PumpControlProxy(ILogger<PumpControlProxy> log, HubConnection connection) : this(log)
		{
			_connection = connection;
			RegisterCallbacks();
		}

		private PumpControlProxy(ILogger<PumpControlProxy> log)
		{
			_log = log;
			_log.LogInformation("Creating HubProxy");
		}

		private void RegisterCallbacks()
		{
			_connection.On<PumpStatus>(
				nameof(IPumpControlCallback.UpdatePump),
				status => PumpUpdated?.Invoke(this, new PumpEventArgs(status)));

			_connection.On<WatertankStatus>(
				nameof(IPumpControlCallback.UpdateWatertank),
				status => WatertankUpdated?.Invoke(this, new WatertankEventArgs(status)));
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
		public async Task<WatertankStatus> WatertankStatus()
		{
			await EnsureConnection();
			return await _connection.InvokeAsync<WatertankStatus>(nameof(WatertankStatus));
		}

		/// <inheritdoc />
		public async Task<PumpStatus> PumpStatus()
		{
			await EnsureConnection();
			return await _connection.InvokeAsync<PumpStatus>(nameof(PumpStatus));
		}

		private Task EnsureConnection()
		{
			if (_connection.State == HubConnectionState.Disconnected)
			{
				_log.LogInformation("Hub not connected, connecting...");
				return _connection.StartAsync();
			}

			return Task.CompletedTask;

			// return _connection.State == HubConnectionState.Disconnected
			// 	? _connection.StartAsync()
			// 	: Task.CompletedTask;
		}
	}
}