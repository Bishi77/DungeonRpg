using DungeonRpg.Models;
using DungeonRpg.Models.Helpers;
using System.Collections.Generic;
using System.Windows.Input;

namespace DungeonRpg.ViewModels
{
	public class InventoryViewModel : ModelBase
	{
		private Inventory _inventory = new Inventory(null);

		public ICommand GoToGameCommand { get; set; }

		public List<InventoryItem> Items
		{
			get { return _inventory.ItemList; }
			set
			{
				_inventory.ItemList = value;
				OnPropertyChanged(nameof(Items));
			}
		}

		#region ctor
		public InventoryViewModel()
		{
		}

		public InventoryViewModel(Inventory inventory)
		{
			_inventory = inventory;
		}
		#endregion ctor
	}
}
