using System;
using System.Windows;

namespace DungeonRpg.Views
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
				MainWindowView main = new MainWindowView();
				App.Current.MainWindow = (Window)main;
				this.Close();
				main.Show();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK);
			}
		}
	}
}
