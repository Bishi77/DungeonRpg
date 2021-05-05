using System;
using System.Collections.Generic;

namespace DungeonRpg.Models
{
	public class Dungeon
	{
		public enum Direction { UP = 'U', DOWN = 'D', LEFT = 'L', RIGHT = 'R' };

		private List<DungeonElement>[,] _levelData = new List<DungeonElement>[0, 0];
		public List<DungeonElement>[,] LevelData
		{
			get { return _levelData; }
		}

		#region ctor
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
		#endregion ctor

		#region methods
		/// <summary>
		/// Keresett elemtípus első előfordulásának a pozíciója a pályán
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

		/// <summary>
		/// Lehetséges mozgási irányok kigyűjtése egy pozícióból
		/// A térkép széle, azaz a pálya első vagy utolsó sora, oszlopa nem járható.
		/// </summary>
		/// <param name="dungeon">pálya adata</param>
		/// <param name="row">vizsgált helyzet sora</param>
		/// <param name="col">vizsgált helyzet oszlopa</param>
		/// <returns>Lehetséges mozgási irányok angol kezdőbetűinek felsorolása</returns>
		public string GetPossibleMoveDirections(int row, int col)
		{
			string result = "";

			if ((row > 1) && !LevelPositionHasDungeonElementType(row - 1, col, DungeonElementType.Wall))
				result += "U";
			if ((row < LevelData.GetLength(0) - 1) && !LevelPositionHasDungeonElementType(row + 1, col, DungeonElementType.Wall))
				result += "D";
			if ((col > 0 && !LevelPositionHasDungeonElementType(row, col - 1, DungeonElementType.Wall)))
				result += "L";
			if ((col < LevelData.GetLength(1) - 1) && !LevelPositionHasDungeonElementType(row, col + 1, DungeonElementType.Wall))
				result += "R";
			return result;
		}

		public int GetPositionSumValue(int row, int col)
		{
			int result = 0;
			LevelData[row, col].ForEach(x => result += (int)x.ElementType);
			return result;
		}
		#endregion methods
	}
}
