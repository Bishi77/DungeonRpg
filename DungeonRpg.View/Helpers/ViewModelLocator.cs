using DungeonRpg.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Helpers
{
	public static class ViewModelLocator
	{
		public static MainWindowViewModel GetMainWindowViewModel
		{
			get
			{
				return new MainWindowViewModel();
			}
		}
	}
}
