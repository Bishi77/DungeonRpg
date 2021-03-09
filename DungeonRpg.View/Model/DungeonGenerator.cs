using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Model
{
	public class DungeonGenerator
	{
        public enum FieldTypes { Wall = 0, Finish = 1, Down = 2, Up = 3, Monster = 4, Way = 5, Start = 6 };
        public enum Direction { UP, DOWN, LEFT, RIGHT };
        private const int _wayConnectionPercent = 10;
        private bool _enableConnection
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
        public float[,] GenerateDungeonLevel(int rows, int columns)
		{
			return new float[rows, columns];			
		}

        /// <summary>
        /// Pálya út generálása
        /// </summary>
        /// <param name="levelData">pálya adata</param>
        /// <param name="fillPercent">út feltöltés százaléka</param>
        /// <returns></returns>
        public float[,] AddWaysToDungeonLevel(float[,] levelData, int fillPercent = -1)
        {
            Random rnd = new Random();

            //ha nincs fix érték megadva, generálunk egy általános gyakoriságot
            if (fillPercent==-1)
			{
                int minFillPercent = 20, maxFillPercent = 50;
                fillPercent = rnd.Next(minFillPercent, maxFillPercent);
            }

            (int, int) startpos = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Wall, true);
            string possibleDirs = GetPossibleWayGenerationDirection(levelData, startpos.Item1, startpos.Item2);          
            int wayCellNr = levelData.GetLength(0) * levelData.GetLength(1) * fillPercent / 100;
            int actCellNr = 0;
            while (actCellNr < wayCellNr)
            {
                if (!string.IsNullOrEmpty(possibleDirs))
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
                    levelData[startpos.Item1, startpos.Item2] = (int)FieldTypes.Way;
                    possibleDirs = GetPossibleWayGenerationDirection(levelData, startpos.Item1, startpos.Item2);
                    actCellNr++;
                }
            }
            return levelData;
        }

        /// <summary>
        /// Térképen speciális elemek elhelyezése, akár többszintes dungeonhoz is. 
        /// Ha kezdőszint, akkor kezdőpontot generál, 
        /// ha végszint, akkor vég pontot generál,
        /// A kezdő és végszinteknek megfelelően, lépcsőket generál a szintek közt.
        /// </summary>
        /// <param name="levelData">Térkép adata</param>
        /// <param name="isStartLevel">Kezdőszint-e. Ehhez kellő elemeket generáljon-e. (Kezdő pont)</param>
        /// <param name="isEndLevel">Vég szint-e. Ehhez kellő elemeket generáljon-e. (Vég pont)</param>
        /// <returns></returns>
        public float[,] AddPlacePOIsToDungeonLevel(float[,] levelData, bool isStartLevel, bool isEndLevel, int monsterGeneratePercent)
        {
            Random rnd = new Random();
            var point = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Way, true);
            levelData[point.Item1, point.Item2] = isStartLevel ? (int)FieldTypes.Start : (int)FieldTypes.Down;

            point = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Way, true); ;
            levelData[point.Item1, point.Item2] = isEndLevel ? (int)FieldTypes.Finish : (int)FieldTypes.Up;

            int monsterNumber = levelData.GetLength(0) * levelData.GetLength(1) * monsterGeneratePercent / 100;
            for (int actMonster = 0; actMonster < monsterNumber; actMonster++)
            {
                point = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Way, true);
                
                levelData[point.Item1, point.Item2] = (int)FieldTypes.Monster;
            }

            return levelData;
        }

        /// <summary>
        /// A keresett mezőtípusú pályaelemek közül választunk egyet véletlenszerűen
        /// </summary>
        /// <param name="levelData">pálya adata</param>
        /// <param name="searchedFieldType">keresett mezőtípus</param>
        /// <param name="withoutConnectionVerify"></param>
        /// <returns></returns>
        private (int, int) GetRandomFieldTypePointFromLevel(float[,] levelData, FieldTypes searchedFieldType, bool withoutConnectionVerify)
        {
            Random rnd = new Random();
            List<object> searchedFieldTypeCoords = new List<object>();
            (int, int) coord = (-1, -1);  

            for (int row = 0; row < levelData.GetLength(0); row++)
            {
                for (int col = 0; col < levelData.GetLength(1); col++)
                {
                    if (levelData[row, col] == (int)searchedFieldType)
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
                else if (!_enableConnection && HasConnectedWay(levelData, coord.Item1, coord.Item2))
                {
                    searchedFieldTypeCoords.RemoveAt(coordNr);
                }
                else
                {
                    done = true;
                }
            }
            return coord;

            /*régi kódban benne volt itt, nem néztem miért, talán alternatív megoldás miatt? akkor meg tesztelendő melyik a jobb
            int xPos, yPos;
            xPos = yPos = -1;
            while (xPos == -1 || (Math.Truncate(levelData[yPos, xPos]) != (int)fieldtype) && (!enableConnection && HasConnectedWay(levelData, yPos, xPos)))
            {
                xPos = rnd.Next(2, levelData.GetUpperBound(1)-1);
                yPos = rnd.Next(2, levelData.GetUpperBound(0)-1);
            }

            return (yPos, xPos);   
             */
        }

        /// <summary>
        /// Lehetséges mozgási irányok kigyűjtése egy pozícióból
        /// A térkép széle, azaz a pálya első vagy utolsó sora, oszlopa nem járható.
        /// </summary>
        /// <param name="levelData">pálya adata</param>
        /// <param name="row">vizsgált helyzet sora</param>
        /// <param name="col">vizsgált helyzet oszlopa</param>
        /// <returns></returns>
        public string GetPossibleMoveDirections(float[,] levelData, int row, int col)
        {
            string result = "";

            if ((row > 1) && (levelData[row - 1, col] != 0) && (_enableConnection || !HasConnectedWay(levelData, row - 1, col)))
                result += "U";
            if ((row < levelData.GetLength(0) - 1) && (levelData[row + 1, col] != 0) && (_enableConnection || !HasConnectedWay(levelData, row + 1, col)))
                result += "D";
            if ((col > 1 && levelData[row, col - 1] != 0) && (_enableConnection || !HasConnectedWay(levelData, row, col - 1)))
                result += "L";
            if ((col < levelData.GetLength(1) - 1) && (levelData[row, col + 1] != 0) && (_enableConnection || !HasConnectedWay(levelData, row, col + 1)))
                result += "R";
            return result;
        }

        /// <summary>
        /// Út generáláshoz pont fordítva kell keresni mint a mozgási irányokhoz
        /// Ha nincs út, akkor generálhatunk oda.
        /// A térkép szélén nem mehet út.
        /// </summary>
        /// <param name="levelData">pálya adata</param>
        /// <param name="row">aktuális pozício sora</param>
        /// <param name="col">aktuális pozício oszlopa</param>
        /// <returns>Lehetséges irányok 1. betűjének felsorolása angolul</returns>
        public string GetPossibleWayGenerationDirection(float[,] levelData, int row, int col)
		{
            string wallDirections = "";
            string wayDirections = GetPossibleMoveDirections(levelData, row, col);
            wallDirections += row > 1 && !wayDirections.Contains("U") ? "U" : "";
            wallDirections += row < levelData.GetLength(0) - 1 && !wayDirections.Contains("D") ? "D" : "";
            wallDirections += col > 1 && !wayDirections.Contains("L") ? "L" : "";
            wallDirections += col < levelData.GetLength(1) -1 && !wayDirections.Contains("R") ? "R" : "";

            return wallDirections;
        }

        /// <summary>
        /// Útra bekötött e a pozíció.
        /// Ha van legalább 2 szomszédos út mezője, akkor annak tekintjük
        /// </summary>
        /// <param name="levelData">pályaadat</param>
        /// <param name="row">aktuális pozício sora</param>
        /// <param name="col">aktuális pozício  oszlopa</param>
        /// <returns></returns>
        private bool HasConnectedWay(float[,] levelData, int row, int col)
        {
            int connections = 0;
            if (row > 0 && levelData[row - 1, col] != 0)
                connections++;
            if (row < levelData.GetLength(0) - 1 && levelData[row + 1, col] != 0)
                connections++;
            if (connections < 2 && col > 0 && levelData[row, col - 1] != 0)
                connections++;
            if (connections < 2 && col < levelData.GetLength(1) - 1 && levelData[row, col + 1] != 0)
                connections++;
            return connections > 1;
        }

    }
}
