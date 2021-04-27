namespace UniversalDesign
{
	public class TileSubCategory
	{
		public TileSubCategory(string value) { Value = value; }

		public string Value { get; set; }

		public static TileSubCategory Error { get { return new TileSubCategory(""); } }
	}
}