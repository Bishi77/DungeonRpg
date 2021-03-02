using System.Collections.Generic;

namespace DungeonRpg.Model
{
	public interface IInventoryItem
	{
		CommandImplementation ItemCommand;
		string GetName();
		List<object> GetProperties();
	}
}