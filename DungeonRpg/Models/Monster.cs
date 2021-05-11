using DungeonRpg.Models;
using System;
using UniversalDesign;

namespace DungeonRpg.Models
{
	public class Monster : Character
	{
		public readonly int MonsterID;

		private static int _lastMonsterId = 0;
		public static int LastMonsterId
		{
			get
			{
				return _lastMonsterId++;
			}
		}

		private int _agression = 0;
		public int Agression { get => _agression; set => _agression = value; }

		private string _imageName = "";
		public string ImageName { get => _imageName; set => _imageName = value; }

		private string _category;
		public string Category { get => _category; set => _category = value; }

		public Monster(int diceSize, int diceNr, ValueTuple<int, int> startPosition) : base(diceSize, diceNr, startPosition)
		{
			MonsterID = LastMonsterId;
			GenerateType();
		}

		private void GenerateType()
		{
			var tileCategory = TileCategory.Monster.Value;
			var tileSubCategory = TileMonsterSubCategory.Demons;
			var pngName = "crimson_imp.png";
		}
	}
}
