using CIAHome.Shared.Model;

namespace CIAHome.Shared.EventArgs
{
	public class WatertankEventArgs : System.EventArgs
	{
		public WatertankStatus Status { get; }

		/// <inheritdoc />
		public WatertankEventArgs(WatertankStatus status)
		{
			Status = status;
		}
	}
}