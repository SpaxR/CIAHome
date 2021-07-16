namespace WebUI.Shared.Models
{
	public class TodoItem
	{
		public string Id { get; set; }

		public string Text { get; set; } = "ToDo";

		public bool IsChecked { get; set; }
	}
}