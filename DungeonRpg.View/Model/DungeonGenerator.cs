using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.View.Model
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
        /// <param name="fillPercent">kitöltés százaléka</param>
        /// <returns>generált pálya</returns>
        public float[,] GenerateDungeonLevel(int rows, int columns, int fillPercent = -1)
		{
			float[,] dungeon = new float[rows, columns];
			dungeon = AddWays(dungeon, fillPercent);
			dungeon = PlacePOIs(dungeon, true, true);
			return dungeon;
		}

        private float[,] AddWays(float[,] levelData, int fillPercent = -1)
        {
            Random rnd = new Random();

            //ha nincs fix érték megadva, generálunk egy általános gyakoriságot
            if (fillPercent==-1)
			{
                int minFillPercent = 20, maxFillPercent = 30;
                fillPercent = rnd.Next(minFillPercent, maxFillPercent);
            }

            (int, int) startpos = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Wall, true);
            string possibleDirs = GetPossibleDirections(levelData, startpos.Item1, startpos.Item2);          
            int cells = levelData.GetLength(0) * levelData.GetLength(1) * fillPercent / 100;
            int cellNr = 0;
            while (cellNr < cells)
            {
                if (possibleDirs == "")
                {
                    startpos = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Way, false);
                    possibleDirs = GetPossibleDirections(levelData, startpos.Item1, startpos.Item2);
                }

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
                    possibleDirs = GetPossibleDirections(levelData, startpos.Item1, startpos.Item2);
                    cellNr++;
                }
            }
            return levelData;
        }

        /// <summary>
        /// Térképen speciális elemek elhelyezése, akár többszintes dungeonhoz
        /// </summary>
        /// <param name="levelData">Térkép adata</param>
        /// <param name="isStartLevel">Kezdőszint-e. Ehhez kellő elemeket generáljon-e.</param>
        /// <param name="isEndLevel">Vég szint-e. Ehhez kellő elemeket generáljon-e.</param>
        /// <returns></returns>
        private float[,] PlacePOIs(float[,] levelData, bool isStartLevel, bool isEndLevel)
        {
            Random rnd = new Random();
            var point = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Way, true);
            levelData[point.Item1, point.Item2] = isStartLevel ? (int)FieldTypes.Start : (int)FieldTypes.Down;

            point = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Way, true); ;
            levelData[point.Item1, point.Item2] = isEndLevel ? (int)FieldTypes.Finish : (int)FieldTypes.Up;

            for (int m = 0; m < rnd.Next(10, 20); m++)
            {
                point = GetRandomFieldTypePointFromLevel(levelData, FieldTypes.Way, true);
                
                levelData[point.Item1, point.Item2] = (int)FieldTypes.Monster;
            }

            return levelData;
        }

        private (int, int) GetRandomFieldTypePointFromLevel(float[,] levelData, FieldTypes fieldtype, bool withoutConnectionVerify)
        {
            Random rnd = new Random();
            List<object> typecoords = new List<object>();
            (int, int) coord = (-1, -1);  

            for (int row = 0; row < levelData.GetLength(0); row++)
            {
                for (int col = 0; col < levelData.GetLength(1); col++)
                {
                    if (levelData[row, col] == (int)fieldtype)
                    {
                        typecoords.Add((row,col));
                    }
                }
            }

            bool done = false;
            while (typecoords.Count>=0 && !done)
            {
                var coordNr = rnd.Next(0, typecoords.Count);
                coord = ((int, int))typecoords.ToArray()[coordNr];
                if (withoutConnectionVerify)
                {
                    done = true;
                }
                else if (!_enableConnection && HasConnectedWay(levelData, coord.Item1, coord.Item2))
                {
                    typecoords.RemoveAt(coordNr);
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
        /// Lehetséges irányok kigyűjtése egy pozícióból
        /// A térkép széle, azaz a pálya első vagy utolsó sora, oszlopa nem járható.
        /// </summary>
        /// <param name="levelData"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetPossibleDirections(float[,] levelData, int row, int col)
        {
            string result = "";

            if (row > 1 && levelData[row - 1, col] == 0 && (_enableConnection || !HasConnectedWay(levelData, row - 1, col)))
                result += "U";
            if (row < levelData.GetLength(0) - 2 && levelData[row + 1, col] == 0 && (_enableConnection || !HasConnectedWay(levelData, row + 1, col)))
                result += "D";
            if (col > 1 && levelData[row, col - 1] == 0 && (_enableConnection || !HasConnectedWay(levelData, row, col - 1)))
                result += "L";
            if (col < levelData.GetLength(1) - 2 && levelData[row, col + 1] == 0 && (_enableConnection || !HasConnectedWay(levelData, row, col + 1)))
                result += "R";
            return result;
        }

        /// <summary>
        /// Ami mellé rakjuk az 1 kapcsolat
        /// </summary>
        /// <param name="levelData"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
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
