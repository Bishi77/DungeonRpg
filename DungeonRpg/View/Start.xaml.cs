using DungeonRpg.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DungeonRpg.View
{
	/// <summary>
	/// Interaction logic for Start.xaml
	/// </summary>
	public partial class Start : Window
	{
		public Start()
		{
			InitializeComponent();
		}

		private void OpenMainWindow_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Window main = new Game();
				App.Current.MainWindow = main;
				this.Close();
				main.Show();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
			}
		}
	}
}
