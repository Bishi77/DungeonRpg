using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace DungeonRpg.Views
{
	public partial class Game
	{
		public Game()
		{
			InitializeComponent();
		}

		private void c_dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			DataGridTextColumn column = e.Column as DataGridTextColumn;
			Binding binding = column.Binding as Binding;
			binding.Path = new PropertyPath(binding.Path.Path + ".Value");
		}

		public static void SelectCellByIndex(DataGrid dataGrid, int rowIndex, int columnIndex)
		{
			if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.Cell))
				throw new ArgumentException("The SelectionUnit of the DataGrid must be set to Cell.");

			if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
				throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

			if (columnIndex < 0 || columnIndex > (dataGrid.Columns.Count - 1))
				throw new ArgumentException(string.Format("{0} is an invalid column index.", columnIndex));

			dataGrid.SelectedCells.Clear();

			object item = dataGrid.Items[rowIndex]; //=Product X
			DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
			if (row == null)
			{
				dataGrid.ScrollIntoView(item);
				row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
			}
			if (row != null)
			{
				DataGridCell cell = GetCell(dataGrid, row, columnIndex);
				if (cell != null)
				{
					DataGridCellInfo dataGridCellInfo = new DataGridCellInfo(cell);
					dataGrid.SelectedCells.Add(dataGridCellInfo);
					cell.Focus();
				}
			}
		}

		private static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
		{
			if (rowContainer != null)
			{
				DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
				if (presenter == null)
				{
					/* if the row has been virtualized away, call its ApplyTemplate() method 
					 * to build its visual tree in order for the DataGridCellsPresenter
					 * and the DataGridCells to be created */
					rowContainer.ApplyTemplate();
					presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
				}
				if (presenter != null)
				{
					DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
					if (cell == null)
					{
						/* bring the column into view
						 * in case it has been virtualized away */
						dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
						cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
					}
					return cell;
				}
			}
			return null;
		}

		private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if (child != null && child is T)
					return (T)child;
				else
				{
					T childOfChild = FindVisualChild<T>(child);
					if (childOfChild != null)
						return childOfChild;
				}
			}
			return null;
		}

		//private void btnInventory_Click(object sender, RoutedEventArgs e)
		//{
		//	ViewModel.Inventory vm = new ViewModel.Inventory();
		//	Inventory myOwnedDialog = new Inventory(vm);
		//	myOwnedDialog.Owner = this;
		//	myOwnedDialog.Width = this.Width * 0.9;
		//	myOwnedDialog.Height = this.Height * 0.9;
		//	myOwnedDialog.ShowDialog();
		//}
	}
}
