using System;

namespace PumpControl
{
	public class WatertankSensor
	{
		private readonly Random _generator = new();

		public double GetWaterLevel()
		{
			return _generator.NextDouble() * 100;
		}
	}
}