using DungeonRpg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Models
{
	public static class CharacterGenerator
	{
		public static Character Generate(List<DungeonElement>[,] levelData, (int, int) p)
		{
			var character = new Character();
			character.Position = p;
			levelData[p.Item1, p.Item2].Add(new DungeonElement(DungeonElementType.Player, -1));
			character.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			return character;
		}
	}
}
