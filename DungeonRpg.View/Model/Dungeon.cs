using System;
using System.Collections.Generic;
using static DungeonRpg.View.Model.DungeonGenerator;

namespace DungeonRpg.View.Model
{
	public class Dungeon
	{
		private float[,] _levelData = new float[0, 0];

		public Dungeon(float[,] levelData)
		{
			LevelData = levelData;
		}

		public float[,] LevelData
		{
			get { return _levelData; }
			set
			{
				_levelData = value;
			}
		}

		public (int, int) GetValuePosition(int Value)
		{
			return GetFirstCoordinateValueFromArray(Value);
		}

		private string GetFieldView(float f)
		{
			int type = (int)Math.Truncate(f);
			FieldTypes ft = (FieldTypes)type;
			return ft == FieldTypes.Way ? " " : ft.ToString()[0].ToString();
		}

		private (int, int) GetFirstCoordinateValueFromArray(int searchedValue)
		{
			for (int r = 0; r < LevelData.GetLength(0); r++)
				for (int c = 0; c < LevelData.GetLength(1); c++)
				{
					if (LevelData[r, c] == searchedValue)
						return (r, c);
				}
			throw new Exception($"Keresett érték ({searchedValue}) nem található a tömbben!");
		} 
	}
}
