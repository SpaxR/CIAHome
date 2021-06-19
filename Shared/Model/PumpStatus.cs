using CIAHome.Shared.Interfaces;

namespace CIAHome.Shared.Model
{
	public class PumpStatus : IPumpControlUpdate
	{
		/// <inheritdoc />
		public string Identifier { get; }

		/// <summary> True, if PumpStatus is currently pumping Water </summary>
		public bool IsRunning { get; set; }
	}
}