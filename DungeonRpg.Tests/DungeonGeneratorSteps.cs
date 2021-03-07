using DungeonRpg.View.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

namespace DungeonRpg.Tests
{
    [Binding]
    public class DungeonGeneratorSteps
    {
        private DungeonGenerator dgt;
        private float[,] level;

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

        [Then(@"az eredmény float\[(.*)] tömb lesz\.")]
        public void AkkorAzEredmenyFloatTombLesz_(int rows, int columns)
        {
            Assert.IsTrue((level.GetLength(0) == rows) && (level.GetLength(1) == columns));
        }

    }
}
