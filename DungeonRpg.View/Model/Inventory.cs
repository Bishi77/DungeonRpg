using DungeonRpg.Model.Interface;
using System.Collections.Generic;

namespace DungeonRpg.Model
{
	public class Inventory : IInventory
	{
		private List<IInventoryItem> itemList = new List<IInventoryItem>();

		public List<IInventoryItem> ItemList 
		{ 
			get => itemList; 
			private set => itemList = value; 
		}

		public void AddItem(IInventoryItem item)
		{
			ItemList.Add(item);
		}

		public void RemoveItem(IInventoryItem item)
		{
			ItemList.Remove(item);
		}
	}
}