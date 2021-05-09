using System.Windows;

namespace DungeonRpg.Views
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class InventoryView
	{
		public InventoryView()
		{
			InitializeComponent();
		}

		public InventoryView(ViewModels.InventoryViewModel vm)
		{
			this.DataContext = vm;
			InitializeComponent();
		}

		private void Use_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
