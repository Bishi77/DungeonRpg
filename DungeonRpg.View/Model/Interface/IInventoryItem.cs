using DungeonRpg.Helpers;
using System.Collections.Generic;

namespace DungeonRpg.Model.Interface
{
	public interface IInventoryItem
	{
		string GetName();
		
		List<object> GetDescription();

		void AddItem(IInventoryItem item);

		void RemoveItem(IInventoryItem item);
	}
}