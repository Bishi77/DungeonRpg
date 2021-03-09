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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DungeonRpg.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class Game : Window
	{
		public Game()
		{
			InitializeComponent();
		}

		public Game(ViewModel.Game vm)
		{
			this.DataContext = vm;
			InitializeComponent();
		}

		private void c_dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			DataGridTextColumn column = e.Column as DataGridTextColumn;
			Binding binding = column.Binding as Binding;
			binding.Path = new PropertyPath(binding.Path.Path + ".Value");
		}

		private void Up_Click(object sender, RoutedEventArgs e)
		{
			var viewModel = (ViewModel.Game)DataContext;
			viewModel.MoveCharacter('U');
		}

		private void Down_Click(object sender, RoutedEventArgs e)
		{
			var viewModel = (ViewModel.Game)DataContext;
			viewModel.MoveCharacter('D');
		}

		private void Left_Click(object sender, RoutedEventArgs e)
		{
			var viewModel = (ViewModel.Game)DataContext;
			viewModel.MoveCharacter('L');
		}

		private void Right_Click(object sender, RoutedEventArgs e)
		{
			var viewModel = (ViewModel.Game)DataContext;
			viewModel.MoveCharacter('R');
		}
	}
}
