using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalDesign
{
    public class TileCategory
    {
        public TileCategory(string value) { Value = value; }

        public string Value { get; set; }

        public static TileCategory Dungeon { get { return new TileCategory("dngn"); } }
        public static TileCategory Monster { get { return new TileCategory("mon"); } }
        public static TileCategory Item { get { return new TileCategory("item"); } }
        public static TileCategory Error { get { return new TileCategory(""); } }
    }
}
