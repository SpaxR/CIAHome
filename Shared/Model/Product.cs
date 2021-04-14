using System.ComponentModel.DataAnnotations;

namespace CIAHome.Shared.Model
{
	public class Product
	{
		[Key]
		public int ID { get; set; }

		public string Name        { get; set; }
		public string DetailName  { get; set; }
		public string Vendor      { get; set; }
		public string Category    { get; set; }
		public int    Contents    { get; set; }
		public string Description { get; set; }
		public string Origin      { get; set; }
	}
}