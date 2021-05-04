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
		public static Character Generate((int, int) p)
		{
			var character = new Character
			{
				Position = p
			};
			character.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			return character;
		}
	}
}
