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
			GenerateName(player);
			dungeon.AddDungeonElementByPosition(player.Position.Item1, player.Position.Item2, new DungeonElement(DungeonElementType.Player), true);
			player.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			return player;
		}

		private static void GenerateName(Player player)
		{
			string characterName;
			List<string> nameParts = new List<string>();
			bool isHigh;
			Dictionary<string, int> attribs = new Dictionary<string, int>();
			attribs.Add(nameof(player.Strength), player.Strength);
			attribs.Add(nameof(player.Wisdom), player.Wisdom);
			attribs.Add(nameof(player.Intelligence), player.Intelligence);
			attribs.Add(nameof(player.Dexterity), player.Dexterity);
			attribs.Add(nameof(player.Constitution), player.Constitution);
			attribs.Add(nameof(player.Charisma), player.Charisma);

			var minKey = attribs.Aggregate((l, r) => l.Value < r.Value ? l : r);
			var maxKey = attribs.Aggregate((l, r) => l.Value > r.Value ? l : r);
			
			isHigh = ((minKey.Value - 3) >= (18 - maxKey.Value));
			var actKey = isHigh ? maxKey : minKey;

			switch (actKey.Key)
			{
				case nameof(player.Strength):
					if (isHigh)
						nameParts = new List<string>() { "Óriás", "Big", "Pici", "Hegy", "Medveölő" };
					else
						nameParts = new List<string>() { "Zabszem", "Törpe", "Bányász", "Földtúró", "Gyáva" };
					break;
				case nameof(player.Charisma):
					if (isHigh)
						nameParts = new List<string>() { "Sármos", "Arany", "Long", "Szépfiú" };
					else
						nameParts = new List<string>() { "Durva", "Csúnya", "Sebhelyes", "Varangy", "Fogatlan" };
					break;
				case nameof(player.Constitution):
					if (isHigh)
						nameParts = new List<string>() { "Tiszta", "Szép", "Makulátlan", "Szépfiú" };
					else
						nameParts = new List<string>() { "Beteges", "Csúnya", "Sebhelyes", "Kérész", "Fogatlan" };
					break;
				case nameof(player.Dexterity):
					if (isHigh)
						nameParts = new List<string>() { "Ügyes", "Gyors", "Fegyvermester", "Késes" };
					else
						nameParts = new List<string>() { "Lyukaskezű", "Szobor", "Csiga", "Ügyetlen", "Jókiejtésű" };
					break;
				case nameof(player.Intelligence):
					if (isHigh)
						nameParts = new List<string>() { "Okos", "Ravasz", "Róka", "Számító" };
					else
						nameParts = new List<string>() { "Buta", "Lassú", "Értetlen", "Félfogású", "Sötét" };
					break;
				case nameof(player.Wisdom):
					if (isHigh)
						nameParts = new List<string>() { "Bölcs", "Tiszta", "Erkölcsös", "Mester", "Tanító", "Tiszta" };
					else
						nameParts = new List<string>() { "Mocskos", "Sötét", "Erkölcstelen", "Rabló", "Modortalan" };
					break;
				default:
					nameParts = new List<string>() { "Csak", "Egyszerű" };
					break;
			}
			characterName = nameParts[Dice.Rnd.Next(nameParts.Count)];
			nameParts = new List<string>() { "Pista", "Jóska", "János", "Péter", "Béla" };
			characterName += " " + nameParts[Dice.Rnd.Next(nameParts.Count)];
			player.Name = characterName;
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
