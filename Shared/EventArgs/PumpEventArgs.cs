using CIAHome.Shared.Model;

namespace CIAHome.Shared.EventArgs
{
	public class PumpEventArgs : System.EventArgs
	{
		public PumpStatus Status { get; set; }
	}
}