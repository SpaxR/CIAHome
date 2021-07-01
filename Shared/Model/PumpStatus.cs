namespace CIAHome.Shared.Model
{
	public class PumpStatus
	{
		public static readonly PumpStatus Unavailable = new();

		public bool IsRunning { get; set; }
	}
}