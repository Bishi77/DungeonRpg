using System;
using TechTalk.SpecFlow;

namespace DungeonRpg.Tests
{
    [Binding]
    public class InventorySteps
    {
        [Given(@"egy Inventory példány")]
        public void AmennyibenEgyInventoryPeldany()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"hozzáadunk egy InventoryItem példányt")]
        public void MajdHozzaadunkEgyInventoryItemPeldanyt()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"(.*) InventoryItem példányunk lesz az Inventoryban felsorolva")]
        public void AkkorInventoryItemPeldanyunkLeszAzInventorybanFelsorolva(int p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
