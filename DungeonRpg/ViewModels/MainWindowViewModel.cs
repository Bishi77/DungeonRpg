using DungeonRpg.ViewModels.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DungeonRpg.ViewModels
{
	public class MainWindowViewModel : BindableBaseViewModel
	{
		#region commands
		public ICommand InventoryCommand { get; set; }
		public ICommand MainCommand { get; set; }

		private ICommand _changePageCommand;
		public ICommand ChangePageCommand
		{
			get
			{
				if (_changePageCommand == null)
				{
					_changePageCommand = new RelayCommand(
						p => ChangeViewModel((BindableBaseViewModel)p),
						p => p is BindableBaseViewModel);
				}

				return _changePageCommand;
			}
		}
		#endregion commands

		#region properties
		private List<BindableBaseViewModel> _pageViewModels = new List<BindableBaseViewModel>();
		public List<BindableBaseViewModel> PageViewModels
		{
			get
			{
				if (_pageViewModels == null)
					_pageViewModels = new List<BindableBaseViewModel>();

				return _pageViewModels;
			}
		}

		private BindableBaseViewModel _currentPageViewModel = new BindableBaseViewModel();
		public BindableBaseViewModel CurrentPageViewModel
		{
			get
			{
				return _currentPageViewModel;
			}
			set
			{
				_currentPageViewModel = value;
				OnPropertyChanged("CurrentPageViewModel");
			}
		}
		#endregion properties

		#region ctor
		public MainWindowViewModel()
		{
			var gamevm = new GameViewModel();
			var inventory = new InventoryViewModel(gamevm.Character.Inventory);

			gamevm.StartGame();
			gamevm.GoToInventoryCommand = new CommandImplementation(OnNav);
			inventory.GoToGameCommand = new CommandImplementation(OnNav);
			PageViewModels.Add(gamevm);
			PageViewModels.Add(inventory);
			CurrentPageViewModel = gamevm;
		}
		#endregion ctor

		#region methods
		private void ChangeViewModel(BindableBaseViewModel viewModel)
		{
			if (!PageViewModels.Contains(viewModel))
				PageViewModels.Add(viewModel);

			CurrentPageViewModel = PageViewModels
				.FirstOrDefault(vm => vm == viewModel);
		}

		private void ChangeViewModel(string viewModelName)
		{
			CurrentPageViewModel = PageViewModels
				.FirstOrDefault(vm => vm.GetType().Name.ToString() == viewModelName);
		}

		private void OnNav(object obj)
		{
			switch (obj.ToString())
			{
				case "Game":
					ChangeViewModel("GameViewModel");
					break;
				case "Inventory":
				default:
					ChangeViewModel("InventoryViewModel");
					break;
			}
		}
		#endregion methods
	}
}
