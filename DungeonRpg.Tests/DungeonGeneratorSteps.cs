using DungeonRpg.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace DungeonRpg.Tests
{
	[Binding]
    public class DungeonGeneratorSteps
    {
        private DungeonGenerator dgt;
        private Dungeon dungeon = new Dungeon(new List<DungeonElement>[0, 0]);

        [Given(@"egy DungeonGenerator példány")]
        public void AmennyibenEgyDungeonGeneratorPeldany()
        {
            dgt = new DungeonGenerator(10, 10, 20, 40, 50);
        }

        [When(@"létrehozzuk a Dungeon példányt (.*) rows és (.*) columns méretben")]
        public void MajdLetrehozzukADungeonPeldanytRowsEsColumnsMeretben(int row, int col)
        {
            dungeon = new Dungeon(new List<DungeonElement>[row, col]);
        }

        [Then(@"a Dungeon\.Leveldata mérete (.*) és (.*) lesz\.")]
        public void AkkorADungeon_LeveldataMereteEsLesz_(int row, int col)
        {
            Assert.IsTrue(dungeon.LevelData.GetLength(0) == row && dungeon.LevelData.GetLength(1) == col);
        }

        [When(@"hozzáadunk egy utat az (.*) (.*) pozícióba")]
        public void MajdHozzaadunkEgyUtatAzPozicioba(int row, int col)
        {
            dungeon.AddDungeonElementByPosition(row, col, new DungeonElement(DungeonElementType.Way, -1), true);
        }

        [Then(@"nem lehet fal az (.*) (.*) pozíción")]
        public void AkkorNemLehetFalAzPozicion(int row, int col)
        {
            Assert.IsFalse(dungeon.LevelPositionHasDungeonElementType(row, col, DungeonElementType.Wall));
        }

        [When(@"hozzáadunk megint egy utat az (.*) (.*) pozícióba")]
        public void MajdHozzaadunkMegintEgyUtatAzPozicioba(int row, int col)
        {
            dungeon.AddDungeonElementByPosition(row, col, new DungeonElement(DungeonElementType.Way, -1), true);
        }

        [Then(@"az (.*) (.*) pozícióban csak (.*) út lehet, és más nem")]
        public void AkkorAzPoziciobanCsakUtLehetEsMasNem(int row, int col, int count)
        {
            Assert.IsTrue(dungeon.LevelData[row, col].Where(x=>x.ElementType == DungeonElementType.Way).Count() == dungeon.LevelData[row, col].Count());
        }
    }
}
