using DungeonRpg.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

namespace DungeonRpg.Tests
{
    [Binding]
    public class DungeonGeneratorSteps
    {
        private DungeonGenerator dgt;
        private Dungeon dungeon = new Dungeon(new float[0, 0]);
        private float[,] level = new float[0, 0];


        [Given(@"egy DungeonGenerator példány")]
        public void AmennyibenEgyDungeonGeneratorPeldany()
        {
            dgt = new DungeonGenerator();
        }

        [When(@"létrehozzuk a pályát (.*) rows és (.*) columns")]
        public void MajdLetrehozzukAPalyatRowsEsColumns(int rows, int columns)
        {
            level = dgt.GenerateDungeonLevel(rows, columns);
        }

        [When(@"létrehozunk egy Dungeon példányt a pályával")]
        public void MajdLetrehozunkEgyDungeonPeldanytAPalyaval()
        {
            dungeon = new Dungeon(level);
        }

        [Then(@"a Dungeon\.Leveldata mérete (.*) és (.*) lesz\.")]
        public void AkkorADungeon_LeveldataMereteTombLesz_(int rows, int columns)
        {
            Assert.IsTrue((dungeon.LevelData.GetLength(0) == rows) && (dungeon.LevelData.GetLength(1) == columns));
        }
    }
}
