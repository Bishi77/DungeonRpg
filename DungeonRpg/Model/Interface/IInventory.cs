using System.Collections.Generic;

namespace DungeonRpg.Model.Interface
{
	public interface IInventory
	{
		void AddItem(IInventoryItem item);

		void RemoveItem(IInventoryItem item);
	}
}