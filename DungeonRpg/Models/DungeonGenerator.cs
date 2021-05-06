using System;
using System.Collections.Generic;

namespace DungeonRpg.Models
{
	public class DungeonGenerator
	{
        /// <summary>
        /// Generálást vezérlő paraméterek
        /// </summary>
        private readonly int _wayConnectionCreatePercent;
        private readonly int _minWayPercentInDungeon;
        private readonly int _maxWayPercentInDungeon;
        private readonly int _monsterGeneratePercent;
        private readonly int _width;
        private readonly int _height;
        private Random _rnd = new Random();

        private bool _enableConnectionByRandom
        {
            get
            {
                return _rnd.Next(0, 100) < _wayConnectionCreatePercent;
            }
        }

        #region ctor
        /// <summary>
        /// Térkép generáló konstruktor
        /// </summary>
        /// <param name="width">Oszlopok száma</param>
        /// <param name="height">Sorok száma</param>
        /// <param name="wayConnectionCreatePercent">út készítés valószínűsége</param>
        /// <param name="minWayPercentInDungeon">út lehetségének minimuma</param>
        /// <param name="maxWayPercentInDungeon">út lehetségének maximuma</param>
        /// <param name="monsterGeneratePercent">szörnyek gyakorisága</param>
        public DungeonGenerator(int width, int height, int wayConnectionCreatePercent, int minWayPercentInDungeon, int maxWayPercentInDungeon, int monsterGeneratePercent)
		{
            _width = width;
            _height = height;
			_wayConnectionCreatePercent = wayConnectionCreatePercent;
			_minWayPercentInDungeon = minWayPercentInDungeon;
			_maxWayPercentInDungeon = maxWayPercentInDungeon;
			_monsterGeneratePercent = monsterGeneratePercent;
		}
        #endregion ctor

        #region methods
        public Dungeon GenerateDungeon()
        {
            Dungeon generated = GenerateDungeonLevel(_height, _width);
            AddWaysToDungeonLevel(generated);
            AddPlacePOIsToDungeonLevel(generated, true, true);

            return generated;
        }

		#region empty dungeon generating
		/// <summary>
		/// Pálya generálása.
		/// </summary>
		/// <param name="rows">pálya mérete, sorok</param>
		/// <param name="columns">pálya mérete, oszlopok</param>
		/// <returns>generált pálya</returns>
		private Dungeon GenerateDungeonLevel(int rows, int columns)
		{
			return new Dungeon(new List<DungeonElement>[rows, columns]);			
		}
        #endregion empty dungeon generating

        #region way generating
        /// <summary>
        /// Útak feltöltése a Dungeonba
        /// </summary>
        /// <param name="dungeon"></param>
        /// <returns></returns>
        private Dungeon AddWaysToDungeonLevel(Dungeon dungeon)
		{
			string possibleDirs;
			var fillPercent = _rnd.Next(_minWayPercentInDungeon, _maxWayPercentInDungeon);
            int wayCellNr = dungeon.LevelData.GetLength(0) * dungeon.LevelData.GetLength(1) * fillPercent / 100;
            ValueTuple<int, int> pos = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Wall, false);			
			int actCellNr = 0;

            //míg kevés az út
			while (actCellNr < wayCellNr)
			{
                //hozzáadjuk az utat
				dungeon.AddDungeonElementByPosition(pos.Item1, pos.Item2, new DungeonElement(DungeonElementType.Way, 1), true);
                //szomszédban van út?
                possibleDirs = GetPossibleWayGenerationDirection(dungeon, pos.Item1, pos.Item2);
                //ha nincs, akkor keresünk egy falat, úttal szomszédosan
				if (string.IsNullOrEmpty(possibleDirs))
				{
					pos = GetRandomTypePositionWithAdjacentAnotherType(dungeon, DungeonElementType.Wall, DungeonElementType.Way);
				}
                else
				{
                    switch (possibleDirs[_rnd.Next(0, possibleDirs.Length)])
                    {
                        case 'U':
                            pos.Item1--;
                            break;
                        case 'D':
                            pos.Item1++;
                            break;
                        case 'L':
                            pos.Item2--;
                            break;
                        case 'R':
                            pos.Item2++;
                            break;
                    }
                }
                actCellNr++;
            }

            return dungeon;
		}

        /// <summary>
        /// Véletlenszerűen veszünk egy pozíciót, amin adott elem van, és szomszédjában van megadott elemtípus
        /// </summary>
        /// <param name="dungeon">elemeket tartalmazó pálya</param>
        /// <param name="searchedType">keresett típus</param>
        /// <param name="adjacentType">szomszéd típusa</param>
        /// <returns></returns>
		private ValueTuple<int, int> GetRandomTypePositionWithAdjacentAnotherType(Dungeon dungeon, DungeonElementType searchedType, DungeonElementType adjacentType)
        {
            List<ValueTuple<int, int>> searchedFieldTypeCoords = GetAllDungeonPositionByDungeonElementType(dungeon, DungeonElementType.Wall);
            int posNr = 0;
            while (posNr < searchedFieldTypeCoords.Count)
            {
                if (!HasPositionAdjacentType(dungeon, searchedFieldTypeCoords[posNr], DungeonElementType.Way))
                {
                    searchedFieldTypeCoords.RemoveAt(posNr);
                    posNr--;
                }
                posNr++;
            }

            return searchedFieldTypeCoords[_rnd.Next(0, searchedFieldTypeCoords.Count)];
        }

        /// <summary>
        /// Van a keresett pozícióval szomszédosan keresett elemtípus? 
        /// </summary>
        /// <param name="dungeon"></param>
        /// <param name="position"></param>
        /// <param name="adjacentType"></param>
        /// <returns></returns>
        private bool HasPositionAdjacentType(Dungeon dungeon, ValueTuple<int, int> position, DungeonElementType adjacentType)
        {
            if (dungeon.LevelPositionHasDungeonElementType(position.Item1 - 1, position.Item2, adjacentType))
                return true;
            if (dungeon.LevelPositionHasDungeonElementType(position.Item1 + 1, position.Item2, adjacentType))
                return true;
            if (dungeon.LevelPositionHasDungeonElementType(position.Item1, position.Item2 - 1, adjacentType))
                return true;
            if (dungeon.LevelPositionHasDungeonElementType(position.Item1, position.Item2 + 1, adjacentType))
                return true;

            return false;
        }

        /// <summary>
        /// Út generáláshoz pont fordítva kell keresni mint a mozgási irányokhoz
        /// Ha nincs út, akkor generálhatunk oda.
        /// A térkép szélén nem mehet út.
        /// </summary>
        /// <param name="dungeon">pálya adata</param>
        /// <param name="row">aktuális pozício sora</param>
        /// <param name="col">aktuális pozício oszlopa</param>
        /// <returns>Lehetséges mozgási irányok angol kezdőbetűinek felsorolása</returns>
        private string GetPossibleWayGenerationDirection(Dungeon dungeon, int row, int col)
        {
            string wallDirections = "";
            string wayDirections = dungeon.GetPossibleMoveDirections(row, col);
            wallDirections += row > 0 && !wayDirections.Contains("U") ? "U" : "";
            wallDirections += row < dungeon.LevelData.GetLength(0) - 1 && !wayDirections.Contains("D") ? "D" : "";
            wallDirections += col > 0 && !wayDirections.Contains("L") ? "L" : "";
            wallDirections += col < dungeon.LevelData.GetLength(1) - 1 && !wayDirections.Contains("R") ? "R" : "";

            return wallDirections;
        }
        #endregion way generating

        #region POI generating
        /// <summary>
        /// Térképen speciális elemek elhelyezése, akár többszintes dungeonhoz is. 
        /// Ha kezdőszint, akkor kezdőpontot generál, 
        /// ha végszint, akkor vég pontot generál,
        /// A kezdő és végszinteknek megfelelően, lépcsőket generál a szintek közt.
        /// </summary>
        /// <param name="dungeon">Térkép adata</param>
        /// <param name="isStartLevel">Kezdőszint-e. Ehhez kellő elemeket generáljon-e. (Kezdő pont)</param>
        /// <param name="isEndLevel">Vég szint-e. Ehhez kellő elemeket generáljon-e. (Vég pont)</param>
        /// <returns>pálya teljes adata</returns>
        private Dungeon AddPlacePOIsToDungeonLevel(Dungeon dungeon, bool isStartLevel, bool isEndLevel)
        {
            ValueTuple<int, int> point = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Way, true);
            if(point.Item1 != -1 && point.Item2!=-1)
			{
                var newElement = new DungeonElement(isStartLevel ? DungeonElementType.StartPoint : DungeonElementType.DownStairs, -1);
                dungeon.AddDungeonElementByPosition(point.Item1, point.Item2, newElement, true);
			}

            point = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Way, true);
            if (point.Item1 != -1 && point.Item2 != -1)
            {
                var newElement = new DungeonElement(isEndLevel ? DungeonElementType.EndPoint : DungeonElementType.UpStairs, -1);
                dungeon.AddDungeonElementByPosition(point.Item1, point.Item2, newElement, true);
            }

            int monsterNumber = dungeon.LevelData.GetLength(0) * dungeon.LevelData.GetLength(1) * _monsterGeneratePercent / 100;
            int actMonster = 0;
            ValueTuple<int, int> startPoint = dungeon.GetFirstDungeonElementPosition(DungeonElementType.StartPoint);
            while (actMonster < monsterNumber)
            {
                point = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Way, true);

                if (point.Item1 == -1 && point.Item2 == -1)
                    break;

                if(point != startPoint)
                {
                    dungeon.AddDungeonElementByPosition(point.Item1, point.Item2, GenerateMonster(), true);
                    actMonster++;
                }
            }
            
            return dungeon;
        }

        #region monster generating
        /// <summary>
        /// Az aktuális szörny komplett generálása 
        /// </summary>
        /// <returns></returns>
        private DungeonElement GenerateMonster()
        {
            return new DungeonElement(DungeonElementType.Monster, -1);
        }
		#endregion monster generating
		#endregion POI generating

		#region common helpers
		/// <summary>
		/// A keresett mezőtípusú pályaelemek közül választunk egyet véletlenszerűen
		/// </summary>
		/// <param name="dungeon">pálya adata</param>
		/// <param name="searchedFieldType">keresett mezőtípus</param>
		/// <param name="withoutConnectionVerify">kihagyható-e az útcsatlakozás ellenőrzés</param>
		/// <returns></returns>
		private ValueTuple<int, int> GetRandomFieldTypePointFromLevel(Dungeon dungeon, DungeonElementType searchedFieldType, bool withoutConnectionVerify)
		{
			List<ValueTuple<int, int>> searchedFieldTypeCoords = new List<ValueTuple<int, int>>();
			ValueTuple<int, int> coord = (-1, -1);

            searchedFieldTypeCoords = GetAllDungeonPositionByDungeonElementType(dungeon, searchedFieldType);

			bool done = false;
			while (searchedFieldTypeCoords.Count > 0 && !done)
			{
				var coordNr = _rnd.Next(0, searchedFieldTypeCoords.Count);
				coord = (ValueTuple<int, int>)searchedFieldTypeCoords.ToArray()[coordNr];
				if (withoutConnectionVerify)
				{
					done = true;
				}
				else if (!_enableConnectionByRandom && HasConnectedWay(dungeon, coord.Item1, coord.Item2))
				{
					searchedFieldTypeCoords.RemoveAt(coordNr);
				}
				else
				{
					done = true;
				}
			}
			return coord;
		}

        /// <summary>
        /// Az összes pozíciót vesszük, ami a keresett típust tartalmazza
        /// </summary>
        /// <param name="dungeon">pálya, amiben keresünk</param>
        /// <param name="searchedFieldType">keresett típus</param>
        /// <returns></returns>
		private List<ValueTuple<int, int>> GetAllDungeonPositionByDungeonElementType(Dungeon dungeon, DungeonElementType searchedFieldType)
        {
            List<ValueTuple<int, int>> searchedFieldTypeCoords = new List<ValueTuple<int, int>>();

            for (int row = 0; row < dungeon.LevelData.GetLength(0); row++)
            {
                for (int col = 0; col < dungeon.LevelData.GetLength(1); col++)
                {
                    if (dungeon.LevelPositionHasDungeonElementType(row, col, searchedFieldType))
                    {
                        searchedFieldTypeCoords.Add(new ValueTuple<int, int>(row, col));
                    }
                }
            }

            return searchedFieldTypeCoords;
        }

        /// <summary>
        /// Útra bekötött e a pozíció.
        /// Ha van legalább 2 szomszédos út mezője, akkor annak tekintjük
        /// </summary>
        /// <param name="dungeon">pályaadat</param>
        /// <param name="row">aktuális pozício sora</param>
        /// <param name="col">aktuális pozício  oszlopa</param>
        /// <returns>Ha a pozíciónak van 2 szomszédos út mezúje, akkor igaz</returns>
        private bool HasConnectedWay(Dungeon dungeon, int row, int col)
        {
            int connections = 0;
            if (row > 0 && !dungeon.LevelPositionHasDungeonElementType(row - 1, col, DungeonElementType.Wall))
                connections++;
            if (row < dungeon.LevelData.GetLength(0) - 1 && !dungeon.LevelPositionHasDungeonElementType(row + 1, col, DungeonElementType.Wall))
                connections++;
            if (connections < 2 && col > 0 && !dungeon.LevelPositionHasDungeonElementType(row, col - 1, DungeonElementType.Wall))
                connections++;
            if (connections < 2 && col < dungeon.LevelData.GetLength(1) - 1 && !dungeon.LevelPositionHasDungeonElementType(row, col + 1, DungeonElementType.Wall))
                connections++;
            return connections > 0;
        }
        #endregion common helpers
        #endregion methods
    }
}
