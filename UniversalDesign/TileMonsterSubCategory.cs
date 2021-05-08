namespace UniversalDesign
{
	public class TileMonsterSubCategory : TileSubCategory
    {
        private TileMonsterSubCategory(string value) : base(value) { }

        public static TileMonsterSubCategory Animals { get { return new TileMonsterSubCategory("animals"); } }
        public static TileMonsterSubCategory Demons { get { return new TileMonsterSubCategory("demons"); } }
        public static TileMonsterSubCategory Humanoids { get { return new TileMonsterSubCategory("humanoids"); } }
    }
}
