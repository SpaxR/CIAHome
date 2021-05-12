using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CIAHome.Shared.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PumpControl
{
	public class DataProvider
	{
		private readonly ILogger<DataProvider> _logger;
		private const    int                   ListenerPort = 5000;
		private readonly IList<TcpClient>      _clients     = new List<TcpClient>();
		private readonly TcpListener           _listener;
		private readonly Thread                _listenerThread;
		private          CancellationToken     _cancellationToken;

		public DataProvider(ILogger<DataProvider> logger)
		{
			_logger         = logger;
			_listener       = new TcpListener(IPAddress.Any, ListenerPort);
			_listenerThread = new Thread(AcceptClients);
		}

		public void StartListening(CancellationToken cancellationToken)
		{
			_cancellationToken = cancellationToken;
			_listenerThread.Start();
		}

		private void AcceptClients()
		{
			_listener.Start();
			_logger.LogDebug("Waiting for Clients");

			while (!_cancellationToken.IsCancellationRequested)
			{
				var client = _listener.AcceptTcpClient();
				_clients.Add(client);
			}
		}

		public async Task SendDataAsync(WatertankStatus status, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Sending Status [{Filled}/{Total}] to {Count} Clients ",
								   status.VolumeFilled, status.VolumeTotal, _clients.Count);

			// JsonConvert.SerializeObjectAsync is obsolete
			// ReSharper disable once MethodHasAsyncOverload
			byte[]           data          = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(status));
			IList<TcpClient> failedClients = new List<TcpClient>();

			foreach (var client in _clients)
			{
				try
				{
					await client.GetStream().WriteAsync(data, cancellationToken);
				}
				catch (Exception)
				{
					failedClients.Add(client);
				}
			}

			if (failedClients.Count > 0) _logger.LogWarning("{Amount} Client(s) disconnected", failedClients.Count);

			foreach (var client in failedClients)
			{
				_clients.Remove(client);
			}
		}
	}
}