using System.Collections.Generic;

namespace DungeonRpg.Models
{
	public class Inventory
	{
		private List<InventoryItem> _itemList = new List<InventoryItem>();
		
		public List<InventoryItem> ItemList
		{
			get { return _itemList; }
			set { _itemList = value; }
		}

		#region ctor
		public Inventory(){}

		public Inventory(List<InventoryItem> itemList)
		{
			this._itemList = itemList;
		}
		#endregion ctor
	}
}