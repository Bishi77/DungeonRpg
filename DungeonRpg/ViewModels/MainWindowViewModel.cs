using DungeonRpg.Models.Helpers;
using DungeonRpg.ViewModels.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DungeonRpg.ViewModels
{
	public class MainWindowViewModel : ModelBase
	{
		#region commands
		public ICommand InventoryCommand { get; set; }
		public ICommand MainCommand { get; set; }
		public ICommand MoveCommand { get; set; }

		private ICommand _changePageCommand;
		public ICommand ChangePageCommand
		{
			get
			{
				if (_changePageCommand == null)
				{
					_changePageCommand = new RelayCommand(
						p => ChangeViewModel((ModelBase)p),
						p => p is ModelBase);
				}

				return _changePageCommand;
			}
		}
		#endregion commands

		#region properties
		private List<ModelBase> _pageViewModels = new List<ModelBase>();
		public List<ModelBase> PageViewModels
		{
			get
			{
				if (_pageViewModels == null)
					_pageViewModels = new List<ModelBase>();

				return _pageViewModels;
			}
		}

		private ModelBase _currentPageViewModel = new ModelBase();
		public ModelBase CurrentPageViewModel
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
			var inventory = new InventoryViewModel(gamevm.Player.Inventory);
			var battle = new BattleViewModel(gamevm.Player, gamevm.Enemy);

			gamevm.StartGame();
			gamevm.GoToInventoryCommand = new CommandImplementation(OnNav);
			inventory.GoToGameCommand = new CommandImplementation(OnNav);
			battle.ChangeContentCommand = new CommandImplementation(OnNav);
			PageViewModels.Add(gamevm);
			PageViewModels.Add(inventory);
			PageViewModels.Add(battle);
			CurrentPageViewModel = gamevm;
			MoveCommand = gamevm.MoveCommand;
		}
		#endregion ctor

		#region methods
		private void ChangeViewModel(ModelBase viewModel)
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
					ChangeViewModel("InventoryViewModel");
					break;
				case "Battle":
					PageViewModels.Remove(PageViewModels.First(x => x.GetType().Name == "BattleViewModel"));
					GameViewModel gamevm = (GameViewModel)PageViewModels.First(x => x.GetType().Name == "GameViewModel");
					PageViewModels.Add(new BattleViewModel(gamevm.Player, gamevm.Enemy));
					ChangeViewModel("BattleViewModel");
					break;
			}
		}
		#endregion methods
	}
}
