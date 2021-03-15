using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonRpg.Model.DungeonGenerator;

namespace DungeonRpg.Model
{
	public class Dungeon
	{
		private List<DungeonElement>[,] _levelData = new List<DungeonElement>[0, 0];

		public Dungeon(List<DungeonElement>[,] levelData)
		{
			_levelData = levelData;
			for (int row = 0; row < levelData.GetLength(0); row++)
			{
				for (int column = 0; column < levelData.GetLength(1); column++)
				{
					if (_levelData[row, column] == null)
						_levelData[row, column] = new List<DungeonElement>{new DungeonElement(DungeonElementType.Wall, -1)};
				}
			}
		}

		public List<DungeonElement>[,] LevelData
		{
			get { return _levelData; }
		}

		#region public methods

		/// <summary>
		/// Keresett elemtípus első előfordulásána pozíciója a pályán
		/// </summary>
		/// <param name="searchedElement">Keresett elemtípus</param>
		/// <returns>Első talált előfordulási pozíció</returns>
		public (int, int) GetFirstDungeonElementPosition(DungeonElementType searchedElement)
		{
			for (int r = 0; r < LevelData.GetLength(0); r++)
				for (int c = 0; c < LevelData.GetLength(1); c++)
				{
					if (LevelData[r, c].Exists(x => x.ElementType == searchedElement))
						return (r, c);
				}
			throw new Exception($"Keresett érték ({searchedElement}) nem található a tömbben!");
		}

		/// <summary>
		/// Adott pályapozíción van-e adott típusú eleme
		/// </summary>
		/// <param name="rowPosition">sor pozíció</param>
		/// <param name="columnPosition">oszlop pozció</param>
		/// <param name="elementType">keresett elemtípus</param>
		/// <returns></returns>
		public bool LevelPositionHasDungeonElementType(int rowPosition, int columnPosition, DungeonElementType elementType)
		{
			if (rowPosition < 0 || columnPosition < 0 || rowPosition >= LevelData.GetLength(0) || columnPosition >= LevelData.GetLength(1))
				return false;
			return LevelData[rowPosition, columnPosition].Exists(x => x.ElementType == elementType);
		}

		/// <summary>
		/// Adott pálya pozcióra egy elemet helyezünk
		/// 1 pozíció csak vagy út vagy fal lehet
		/// </summary>
		/// <param name="rowPosition">sor azonosító</param>
		/// <param name="columnPosition">oszlop azonosító</param>
		/// <param name="element">hozzáadott új elem</param>
		/// <param name="justOneTypeEnabledInAPosition">csak maximum 1 lehet-e az adott elemtípusból?</param>
		public void AddDungeonElementByPosition(int rowPosition, int columnPosition, DungeonElement element, bool justOneTypeEnabledInAPosition)
		{
			if (justOneTypeEnabledInAPosition && LevelPositionHasDungeonElementType(rowPosition, columnPosition, element.ElementType))
				return;

			//Az elem vagy fall vagy út típusú, nem lehet mindkettő
			if (element.ElementType == DungeonElementType.Way)
				LevelData[rowPosition, columnPosition].RemoveAll(x => x.ElementType == DungeonElementType.Wall);
			else if (element.ElementType == DungeonElementType.Wall)
				LevelData[rowPosition, columnPosition].RemoveAll(x => x.ElementType == DungeonElementType.Way);

			LevelData[rowPosition, columnPosition].Add(element);
		}

		#endregion public methods

		//private string GetFieldView(float f)
		//{
		//	int type = (int)Math.Truncate(f);
		//	FieldTypes ft = (FieldTypes)type;
		//	return ft == FieldTypes.Way ? " " : ft.ToString()[0].ToString();
		//}



	}
}
