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
		public static Character Generate((int, int) startPosition, Dungeon dungeon)
		{
			var character = new Character
			{
				Position = startPosition
			};
			dungeon.LevelData[character.Position.Item1, character.Position.Item2].Add(new DungeonElement(DungeonElementType.Player, -1));
			character.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			return character;
		}
	}
}
