using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Model
{
	public class DungeonGenerator
	{
        public enum Direction { UP = 'U', DOWN = 'D' , LEFT = 'L', RIGHT = 'R'};

        private const int _wayConnectionPercent = 10;
        private bool _enableConnectionByRandom
        {
            get
            {
                Random rnd = new Random();
                return rnd.Next(0, 100) < _wayConnectionPercent;
            }
        }

        /// <summary>
        /// Pálya generálása.
        /// </summary>
        /// <param name="rows">pálya mérete, sorok</param>
        /// <param name="columns">pálya mérete, oszlopok</param>
        /// <returns>generált pálya</returns>
        public Dungeon GenerateDungeonLevel(int rows, int columns)
		{
			return new Dungeon(new List<DungeonElement>[rows, columns]);			
		}

        /// <summary>
        /// Pálya út generálása
        /// </summary>
        /// <param name="dungeon">pálya adata</param>
        /// <param name="fillPercent">út feltöltés százaléka</param>
        /// <returns>pálya teljes adata</returns>
        public Dungeon AddWaysToDungeonLevel(Dungeon dungeon, int fillPercent = -1)
        {
            Random rnd = new Random();

            //ha nincs fix érték megadva, generálunk egy általános gyakoriságot
            if (fillPercent==-1)
			{
                int minFillPercent = 20, maxFillPercent = 50;
                fillPercent = rnd.Next(minFillPercent, maxFillPercent);
            }

            (int, int) startpos = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Wall, true);
            string possibleDirs = GetPossibleWayGenerationDirection(dungeon, startpos.Item1, startpos.Item2);          
            int wayCellNr = dungeon.LevelData.GetLength(0) * dungeon.LevelData.GetLength(1) * fillPercent / 100;
            int actCellNr = 0;
            while (actCellNr < wayCellNr && !string.IsNullOrEmpty(possibleDirs))
            {
                    switch (possibleDirs[rnd.Next(0, possibleDirs.Length)])
                    {
                        case 'U':
                            startpos.Item1--;
                            break;
                        case 'D':
                            startpos.Item1++;
                            break;
                        case 'L':
                            startpos.Item2--;
                            break;
                        case 'R':
                            startpos.Item2++;
                            break;
                    }

                dungeon.AddDungeonElementByPosition(startpos.Item1, startpos.Item2, new DungeonElement(DungeonElementType.Way, 1), true);
                    possibleDirs = GetPossibleWayGenerationDirection(dungeon, startpos.Item1, startpos.Item2);
                    actCellNr++;
            }
            return dungeon;
        }

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
        public Dungeon AddPlacePOIsToDungeonLevel(Dungeon dungeon, bool isStartLevel, bool isEndLevel, int monsterGeneratePercent)
        {
            Random rnd = new Random();
            var point = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Way, true);
            if(point.Item1 != -1 && point.Item2!=-1)
			{
                var newElement = new DungeonElement(isStartLevel ? DungeonElementType.StartPoint : DungeonElementType.DownStairs, -1);
                dungeon.AddDungeonElementByPosition(point.Item1, point.Item2, newElement, true);
			}

            point = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Way, true); ;
            if (point.Item1 != -1 && point.Item2 != -1)
            {
                var newElement = new DungeonElement(isEndLevel ? DungeonElementType.EndPoint : DungeonElementType.UpStairs, -1);
                dungeon.AddDungeonElementByPosition(point.Item1, point.Item2, newElement, true);
            }

            int monsterNumber = dungeon.LevelData.GetLength(0) * dungeon.LevelData.GetLength(1) * monsterGeneratePercent / 100;
            for (int actMonster = 0; actMonster < monsterNumber; actMonster++)
            {
                point = GetRandomFieldTypePointFromLevel(dungeon, DungeonElementType.Way, true);

                if (point.Item1 == -1 && point.Item2 == -1)
                    break;

                dungeon.AddDungeonElementByPosition(point.Item1, point.Item2, GenerateMonster(), true);
            }

            return dungeon;
        }

		private DungeonElement GenerateMonster()
		{
            DungeonElement newElement = new DungeonElement(DungeonElementType.Monster, -1);

            return newElement;
		}

		/// <summary>
		/// A keresett mezőtípusú pályaelemek közül választunk egyet véletlenszerűen
		/// </summary>
		/// <param name="dungeon">pálya adata</param>
		/// <param name="searchedFieldType">keresett mezőtípus</param>
		/// <param name="withoutConnectionVerify"></param>
		/// <returns></returns>
		private (int, int) GetRandomFieldTypePointFromLevel(Dungeon dungeon, DungeonElementType searchedFieldType, bool withoutConnectionVerify)
        {
            Random rnd = new Random();
            List<object> searchedFieldTypeCoords = new List<object>();
            (int, int) coord = (-1, -1);  

            for (int row = 0; row < dungeon.LevelData.GetLength(0); row++)
            {
                for (int col = 0; col < dungeon.LevelData.GetLength(1); col++)
                {
                    if (dungeon.LevelPositionHasDungeonElementType(row, col, searchedFieldType))
                    {
                        searchedFieldTypeCoords.Add((row,col));
                    }
                }
            }

            bool done = false;
            while (searchedFieldTypeCoords.Count > 0 && !done)
            {
                var coordNr = rnd.Next(0, searchedFieldTypeCoords.Count);
                coord = ((int, int))searchedFieldTypeCoords.ToArray()[coordNr];
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
        /// Út generáláshoz pont fordítva kell keresni mint a mozgási irányokhoz
        /// Ha nincs út, akkor generálhatunk oda.
        /// A térkép szélén nem mehet út.
        /// </summary>
        /// <param name="dungeon">pálya adata</param>
        /// <param name="row">aktuális pozício sora</param>
        /// <param name="col">aktuális pozício oszlopa</param>
        /// <returns>Lehetséges mozgási irányok angol kezdőbetűinek felsorolása</returns>
        public string GetPossibleWayGenerationDirection(Dungeon dungeon, int row, int col)
		{
            string wallDirections = "";
            string wayDirections = dungeon.GetPossibleMoveDirections(row, col);
            wallDirections += row > 1 && !wayDirections.Contains("U") ? "U" : "";
            wallDirections += row < dungeon.LevelData.GetLength(0) - 1 && !wayDirections.Contains("D") ? "D" : "";
            wallDirections += col > 1 && !wayDirections.Contains("L") ? "L" : "";
            wallDirections += col < dungeon.LevelData.GetLength(1) -1 && !wayDirections.Contains("R") ? "R" : "";

            return wallDirections;
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
            return connections > 1;
        }

    }
}
