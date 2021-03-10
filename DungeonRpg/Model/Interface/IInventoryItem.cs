using System.Collections.Generic;
using System.Windows.Input;

namespace DungeonRpg.Model.Interface
{
	public interface IInventoryItem
	{
		string GetName();

		List<object> GetDescription();
	}
}