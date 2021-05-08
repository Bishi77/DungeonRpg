namespace UniversalDesign
{
	public class TileItemSubCategory : TileSubCategory
    {
        private TileItemSubCategory(string value) : base(value) {}

        public static TileItemSubCategory Armour { get { return new TileItemSubCategory("armour"); } }
        public static TileItemSubCategory Potion { get { return new TileItemSubCategory("potion"); } }
        public static TileItemSubCategory Weapon { get { return new TileItemSubCategory("weapon"); } }
    }
}
