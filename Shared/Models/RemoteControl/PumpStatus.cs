namespace CIAHome.Shared.Models
{
	public class PumpStatus
	{
		public static readonly PumpStatus Unavailable = new();

		public bool IsRunning { get; set; }
	}
}