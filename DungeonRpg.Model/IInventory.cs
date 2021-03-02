namespace DungeonRpg.Model
{
	public interface IInventory
	{
		void AddItem(IInventoryItem item);
		void RemoveItem(IInventoryItem item);

	}
}