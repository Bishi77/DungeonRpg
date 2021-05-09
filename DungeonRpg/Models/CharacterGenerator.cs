using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace DungeonRpg.Models
{
	public static class CharacterGenerator
	{
		private const string PATHPREFIX = "UniversalDesign.Resources.Tiles.rltiles.";

		public static Player GeneratePlayer(ValueTuple<int, int> startPosition, Dungeon dungeon, int diceSize, int diceNr)
		{
			Player player = new Player(diceSize, diceNr, startPosition);
			dungeon.AddDungeonElementByPosition(player.Position.Item1, player.Position.Item2, new DungeonElement(DungeonElementType.Player), true);
			player.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			return player;
		}

		public static Monster GenerateMonster((int, int) startPosition, Dungeon dungeon, int diceSize, int diceNr)
		{
			var monster = new Monster(diceSize, diceNr, startPosition)
			{
				Position = startPosition,
				Agression = 1
			};
			SetRandomMonsterTypeData(monster);
			dungeon.AddDungeonElementByPosition(monster.Position.Item1, monster.Position.Item2, new DungeonElement(DungeonElementType.MonsterType, monster.MonsterID), false);
			monster.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			return monster;
		}

		private static void SetRandomMonsterTypeData(Monster monster)
		{
			CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
			TextInfo textInfo = cultureInfo.TextInfo;
			string tileCategory = "mon";
			var filenames = GetItemsName(tileCategory);
			//"UniversalDesign.Resources.Tiles.rltiles.mon.aberrations.glowing_orange_brain.png"

			monster.ImageName = filenames[Dice.Rnd.Next(filenames.Count)];
			var newName = monster.ImageName.Replace(PATHPREFIX + tileCategory + ".", "");
			monster.Category = newName.Substring(0, newName.IndexOf("."));
			monster.Name = newName.Substring(monster.Category.Length + 1, newName.IndexOf(".", newName.IndexOf(".") + 1) - monster.Category.Length -1);
			monster.Name = textInfo.ToTitleCase( monster.Name.Replace("_", " "));
		}

		public static List<string> GetItemsName(string tileCategory, string tileSubCategory = "")
		{
			var ass = AppDomain.CurrentDomain.GetAssemblies().
								SingleOrDefault(assembly => assembly.GetName().Name == "UniversalDesign");
			
			string path = $"{PATHPREFIX}{tileCategory}" + (tileSubCategory != "" ? $".{tileSubCategory}" : "");
			var nameslist = ass.GetManifestResourceNames().Where(w => w.StartsWith(path) && w.EndsWith(".png")).ToList();

			return nameslist;
		}
	}
}
