using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using WebUI.Server.Hubs;
using WebUI.Shared.Models;

namespace FakeData
{
	internal class Program
	{
		private readonly HubConnection _connection;
		private readonly Random        _random = new();


		private static void Main()
		{
			var _ = new Program();
			Console.ReadLine();
		}

		private Program()
		{
			var tokenSource = new CancellationTokenSource();

			_connection = new HubConnectionBuilder()
						  .WithUrl("http://localhost/hubs/pumpcontrol")
						  .WithAutomaticReconnect()
						  .Build();


			Task.Run(() => SendFakeData(tokenSource.Token)).ConfigureAwait(false);
		}

		private async Task SendFakeData(CancellationToken token)
		{
			await _connection.StartAsync(token);
			Console.WriteLine("Hub Connected");

			while (!token.IsCancellationRequested)
			{
				var pump = new PumpStatus {IsRunning = _random.Next() % 2 == 0};
				await _connection.InvokeAsync(nameof(PumpControlHub.UpdatePump), pump, token);
				Console.WriteLine($"Pump: {(pump.IsRunning ? "Running" : "Halted")}");

				var tank = new WatertankStatus
				{
					VolumeFilled = _random.NextDouble() * 100,
					VolumeTotal  = 100
				};
				await _connection.InvokeAsync(nameof(PumpControlHub.UpdateWatertank), tank, token);
				Console.WriteLine($"Watertank: {tank.VolumeFilled} / {tank.VolumeTotal}");

				await Task.Delay(1000, token);
			}
		}
	}
}