using DungeonRpg.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace DungeonRpg.ViewModels
{
	public class InventoryViewModel : BindableBaseViewModel, INotifyPropertyChanged, IPageViewModel
	{
		private Inventory _inventory = new Inventory(null);

		public ICommand GoToGameCommand { get; set; }

		public InventoryViewModel()
		{
		}

		public InventoryViewModel(Inventory inventory)
		{
			_inventory = inventory;
		}

		public List<InventoryItem> Items
		{
			get { return _inventory.ItemList; }
			set
			{
				_inventory.ItemList = value;
				OnPropertyChanged(nameof(Items));
			}
		}
	}
}
