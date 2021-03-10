using System.Collections.Generic;

namespace DungeonRpg.Model.Interface
{
	public interface IInventory
	{
		IList<IInventoryItem> GetInventoryItem();

		void AddItem(IInventoryItem item);

		void RemoveItem(IInventoryItem item);
	}
}