using DungeonRpg.ViewModels;
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
				MainWindow main = new MainWindow();
				App.Current.MainWindow = (Window)main;
				this.Close();
				main.Show();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message +"\n" + ex.StackTrace, "Error", MessageBoxButton.OK);
			}
		}
	}
}
