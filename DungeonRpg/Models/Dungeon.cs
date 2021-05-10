using DungeonRpg.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UniversalDesign;

namespace DungeonRpg.Models
{
	public class Dungeon
	{
		public enum Direction { UP = 'U', DOWN = 'D', LEFT = 'L', RIGHT = 'R' };

		#region properties
		/// <summary>
		/// Pálya adatai.
		/// 2 dimenziós tömb, 1. sorok, 2. oszlopok pozíciója, a pozíciókban DungeonElement listák vannak, jelölve a artalmat
		/// </summary>
		private List<DungeonElement>[,] _levelData = new List<DungeonElement>[0, 0];
		public List<DungeonElement>[,] LevelData
		{
			get { return _levelData; }
		}

		/// <summary>
		/// Pálya látottsága
		/// Adott pozíción, ha 1 van akkor ismert, ha 0 akkor nem.
		/// Nem ismert pozíció esetén a térképen nem látszik a tartalom
		/// </summary>
		private bool[,] _levelVisited = new bool[0, 0];
		public bool[,] LevelVisited { get => _levelVisited; set => _levelVisited = value; }

		private Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
		public Dictionary<int, Monster> Monsters { get => _monsters; set => _monsters = value; }
		#endregion properties

		#region ctor
		public Dungeon(List<DungeonElement>[,] levelData)
		{
			_levelData = levelData;
			for (int row = 0; row < levelData.GetLength(0); row++)
			{
				for (int column = 0; column < levelData.GetLength(1); column++)
				{
					if (_levelData[row, column] == null)
						_levelData[row, column] = new List<DungeonElement>{new DungeonElement(DungeonElementType.Wall)};
				}
			}
			_levelVisited = new bool[levelData.GetLength(0), levelData.GetLength(1)];
		}
		#endregion ctor

		#region methods
		public Monster GetMonsterForBattle(int row, int col)
		{
			var monster = GetDungeonElementByTypeFromLevelPosition(row, col, DungeonElementType.MonsterType);
			if (monster == null)
				return null;

			return Monsters[monster.ElementID];
		}

		/// <summary>
		/// Elemtípus keresése. Első előfordulás pozíciója a pályán
		/// </summary>
		/// <param name="searchedElement">Keresett elemtípus</param>
		/// <returns>Első talált pozíció</returns>
		public ValueTuple<int, int> GetFirstDungeonElementPosition(DungeonElementType searchedElement)
		{
			for (int r = 0; r < LevelData.GetLength(0); r++)
				for (int c = 0; c < LevelData.GetLength(1); c++)
				{
					if (LevelData[r, c].Exists(x => x.ElementType == searchedElement))
						return new ValueTuple<int, int>(r, c);
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
		/// Adott pozíción keresett elem kérése
		/// </summary>
		/// <param name="rowPosition">sor pozíció</param>
		/// <param name="columnPosition">oszlop pozció</param>
		/// <param name="elementType">keresett elemtípus</param>
		/// <returns>Keresett elem, vagy null, ha nincs</returns>
		public DungeonElement GetDungeonElementByTypeFromLevelPosition(int rowPosition, int columnPosition, DungeonElementType elementType)
		{
			if (rowPosition < 0 || columnPosition < 0 || rowPosition >= LevelData.GetLength(0) || columnPosition >= LevelData.GetLength(1))
				return null;
			return LevelData[rowPosition, columnPosition].FirstOrDefault(x => x.ElementType == elementType);
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

			if ((row > 0) && !LevelPositionHasDungeonElementType(row - 1, col, DungeonElementType.Wall))
				result += "U";
			if ((row < LevelData.GetLength(0) - 1) && !LevelPositionHasDungeonElementType(row + 1, col, DungeonElementType.Wall))
				result += "D";
			if ((col > 0 && !LevelPositionHasDungeonElementType(row, col - 1, DungeonElementType.Wall)))
				result += "L";
			if ((col < LevelData.GetLength(1) - 1) && !LevelPositionHasDungeonElementType(row, col + 1, DungeonElementType.Wall))
				result += "R";
			return result;
		}

		//a környék láthatóság állítása
		public void SetVisitedArea((int, int) position, int visibilityRange)
		{
			for (int r = Math.Max(0, position.Item1 - visibilityRange); r < Math.Min(position.Item1 + visibilityRange + 1, LevelData.GetLength(0)); r++)
			{
				for (int c = Math.Max(0, position.Item2 - visibilityRange); c < Math.Min(position.Item2 + visibilityRange + 1, LevelData.GetLength(1)); c++)
				{
					LevelVisited[r, c] = true;
				}
			}
		}

		public void MoveItem((int, int) oldPosition, (int, int) newPosition, DungeonElementType elementType)
		{
			LevelData[oldPosition.Item1, oldPosition.Item2].RemoveAll(x => x.ElementType == elementType);
			LevelData[newPosition.Item1, newPosition.Item2].Add(new DungeonElement(elementType));
		}

		private string GetPositionKeyValue(int row, int col)
		{
			int sum = 0;
			if (!LevelVisited[row, col])
				return "-1";
			LevelData[row, col].ForEach(x => sum += (int)x.ElementType);
			var monster = GetDungeonElementByTypeFromLevelPosition(row, col, DungeonElementType.MonsterType);
			if (monster != null)
			{
				return $"{sum}_{Monsters[monster.ElementID].ImageName}";
			}
			return sum.ToString();
		}
		#region mapitems
		public MapItem GetMapItemByPosition(int row, int col)
		{
			MapItem mapItem = new MapItem();
			mapItem.Row = row;
			mapItem.Column = col;
			mapItem.ImagesSumValue = GetPositionKeyValue(row, col).ToString();
			mapItem.Tooltip = GetPositionToolTip(row, col, LevelVisited[row, col]);
			if (!MapItem.MapItemCache.ContainsKey(mapItem.ImagesSumValue))
				mapItem.Image = ImageConstructor.MergeImages(GetMapPositionTilesPathsWithFileName(LevelData[row, col], LevelVisited[row, col]));

			return mapItem;
		}

		private string GetPositionToolTip(int row, int col, bool visited)
		{
			if (!visited)
				return "";
			List<DungeonElement> elements = LevelData[row, col];
			var element = elements.FirstOrDefault(c => c.ElementType == DungeonElementType.MonsterType);
			if (element != null)
				return Monsters[element.ElementID].Name;

			return ""; 
		}

		public List<string> GetMapPositionTilesPathsWithFileName(List<DungeonElement> elementsAtPosition, bool visited)
		{
			List<string> positionImages = new List<string>();
			elementsAtPosition.ForEach(x => positionImages.Add(GetMapPositionTilePathWithFileName(x, visited)));
			return positionImages;
		}

		private string GetMapPositionTilePathWithFileName(DungeonElement element, bool visited)
		{
			TileCategory tileCategory = TileCategory.Error;
			TileSubCategory tileSubCategory = TileSubCategory.Error;
			string pngName = "";

			if (!visited)
				return $"UniversalDesign.Resources.Tiles.rltiles.{TileCategory.Dungeon.Value}.unseen.png";

			switch (element.ElementType)
			{
				case DungeonElementType.Wall:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Wall;
					pngName = "catacombs0.png";
					break;
				case DungeonElementType.StartPoint:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "bazaar_portal.png";
					break;
				case DungeonElementType.EndPoint:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "exit_dungeon.png";
					break;
				case DungeonElementType.Item:
					tileCategory = TileCategory.Item;
					tileSubCategory = TileItemSubCategory.Armour;
					pngName = "ring_mail1.png";
					break;
				case DungeonElementType.Player:
					tileCategory = TileCategory.Monster;
					tileSubCategory = TileMonsterSubCategory.Humanoids;
					pngName = "dwarf.png";
					break;
				case DungeonElementType.MonsterType:
					tileCategory = TileCategory.Monster;
					tileSubCategory = TileMonsterSubCategory.Demons;
					return Monsters[element.ElementID].ImageName;
				case DungeonElementType.UpStairs:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "stone_stairs_up.png";
					break;
				case DungeonElementType.DownStairs:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "stone_stairs_down.png";
					break;
				case DungeonElementType.Way:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Floor;
					pngName = "sand1.png";
					break;
				default:
					break;
			}

			return $"UniversalDesign.Resources.Tiles.rltiles.{tileCategory.Value}.{tileSubCategory.Value}.{pngName}";
		}
		#endregion mapitems
		#endregion methods
	}
}
