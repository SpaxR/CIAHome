namespace CIAHome.Core.Entities
{
	public interface IWatertank : IUnique
	{
		public double Volume { get; }
		public double Filled { get; }
	}
}