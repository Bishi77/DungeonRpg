﻿using DungeonRpg.Model.Interface;
using System.Collections.Generic;

namespace DungeonRpg.Model
{
	public class Inventory : IInventory
	{
		private List<IInventoryItem> _itemList = new List<IInventoryItem>();

		public Inventory(List<IInventoryItem> itemList)
		{
			this._itemList = itemList;
		}

		public Inventory()
		{
		}

		public List<IInventoryItem> ItemList
		{
			get { return _itemList; }
			private set { _itemList = value; }
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