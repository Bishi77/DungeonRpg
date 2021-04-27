using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalDesign
{
    public class TileDungeonSubCategory : TileSubCategory
    {
        private TileDungeonSubCategory(string value) : base(value) {}

        public static TileDungeonSubCategory Door { get { return new TileDungeonSubCategory("doors"); } }
        public static TileDungeonSubCategory Floor { get { return new TileDungeonSubCategory("floor"); } }
        public static TileDungeonSubCategory Wall { get { return new TileDungeonSubCategory("wall"); } }
        public static TileDungeonSubCategory Gateways { get { return new TileDungeonSubCategory("gateways"); } }
    }
}
