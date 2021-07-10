namespace CIAHome.Shared.Models
{
	public class WatertankStatus
	{
		public static readonly WatertankStatus Unavailable = new() {VolumeTotal = -1};

		public double VolumeTotal  { get; set; }
		public double VolumeFilled { get; set; }
	}
}