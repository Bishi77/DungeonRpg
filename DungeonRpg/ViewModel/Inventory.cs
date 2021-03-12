using DungeonRpg.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DungeonRpg.ViewModel
{
	public class Inventory : INotifyPropertyChanged
	{
		List<InventoryItem> _items = new List<InventoryItem>();

		public Inventory()
		{			
		}

		public Inventory(IList<InventoryItem> items)
		{
				Items = items.ToList();
		}

		public List<InventoryItem> Items
		{
			get { return _items; }
			set
			{
				_items = value;
				OnPropertyChanged(nameof(Items));
			}
		}

		#region change notify 
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion change notify 
	}
}
