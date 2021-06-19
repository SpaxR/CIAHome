using CIAHome.Shared.Interfaces;

namespace CIAHome.Shared.Model
{
	public class WatertankStatus : IPumpControlUpdate
	{
		/// <inheritdoc />
		public string Identifier { get; }

		/// <summary> Liters of Water, that this Watertank can hold totally </summary>
		public double VolumeTotal { get; set; }

		/// <summary> Liters of Water, that this Watertank holds currently </summary>
		public double VolumeFilled { get; set; }
	}
}