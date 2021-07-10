namespace CIAHome.Core.Entities
{
	public interface ITodoItem : IUnique
	{
		public string Text      { get; }
		public bool   IsChecked { get; }
	}
}