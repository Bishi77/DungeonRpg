using DungeonRpg.Models;
using DungeonRpg.Models.Helpers;
using DungeonRpg.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DungeonRpg.ViewModels
{
	public class GameViewModel : ModelBase, ICanEnableField
	{
		#region fields
		private Dungeon _dungeon = new Dungeon(new List<DungeonElement>[0, 0]);
		private Player _player = new Player();
		private ObservableCollection<MapItem> _mapItems = new ObservableCollection<MapItem>();
		private string _possibleDirections = "";
		private readonly CanEnableFieldHelper _helper;
		#endregion fields

		public ICommand MoveCommand { get; set; }
		public ICommand GoToInventoryCommand { get; set; }

		#region properties
		public ObservableCollection<MapItem> MapItems { get => _mapItems; set => _mapItems = value; }

		public Dungeon Dungeon
		{
			get { return _dungeon; }
			set
			{
				_dungeon = value;
				OnPropertyChanged(nameof(Dungeon));
			}
		}

		public Player Player
		{
			get { return _player; }
			set
			{
				_player = value;
				OnPropertyChanged(nameof(Player));
			}
		}

		public string PossibleDirections
		{
			get { return _possibleDirections; }
			set
			{
				_possibleDirections = value;
				OnPropertyChanged(nameof(CanEnable));
				OnPropertyChanged(nameof(MapItems));
			}
		}

		public dynamic CanEnable
		{
			get { return _helper; }
		}

		public bool CanEnableField(string key)
		{
			return PossibleDirections.Contains(key);
		}
		#endregion properties

		#region constructor
		public GameViewModel()
		{
			MoveCommand = new CommandImplementation(MoveCharacter);
			_helper = new CanEnableFieldHelper(this);
			OnPropertyChanged(nameof(CanEnable));
		}
		#endregion constructor

		#region View Commands
		public void StartGame()
		{
			DungeonGenerator _generator = new DungeonGenerator(20, 20, 5, 70, 70, 5);
			Dungeon = _generator.GenerateDungeon();
			Player = CharacterGenerator.GeneratePlayer(Dungeon.GetFirstDungeonElementPosition(DungeonElementType.StartPoint), Dungeon, 6, 3);
			Dungeon.SetVisitedArea(Player.Position, Player.VisibilityRange);
			DrawMap();
			SetPossibleDirection();
		}
		#endregion View Commands

		#region methods
		private void MoveCharacter(object obj)
		{
			if (PossibleDirections.Contains(obj.ToString()))
			{
				(int, int) oldPosition = Player.Position;
				Player.Move((Dungeon.Direction)obj.ToString()[0], Dungeon);
				RefreshMapItems(new List<ValueTuple<int, int>> { oldPosition, Player.Position }, Player.VisibilityRange);
				SetPossibleDirection();
				OnPropertyChanged(nameof(MapItems));
			}
		}

		private void SetPossibleDirection()
		{
			PossibleDirections = Dungeon.GetPossibleMoveDirections(Player.Position.Item1, Player.Position.Item2);
		}

		private void DrawMap()
		{
			for (int row = 0; row < Dungeon.LevelData.GetLength(0); row++)
			{
				for (int col = 0; col < Dungeon.LevelData.GetLength(1); col++)
				{
					MapItems.Add(Dungeon.GetMapItemByPosition(row, col));
				}
			}
			MapItem.Rows = Dungeon.LevelData.GetLength(0);
			MapItem.Columns = Dungeon.LevelData.GetLength(1);
		}

		private void RefreshMapItems(List<ValueTuple<int, int>> pozitionList, int visibleRange)
		{
			MapItem newMapitem = null;
			MapItem oldMapItem = null;
			List<ValueTuple<int, int>> visibles = new List<ValueTuple<int, int>>();
			visibles.AddRange(pozitionList);
			//új és régi poz. frisssítés-hez hozzáadjuk a láthatóságot, ami az új poz. környezete
			for (int r = Math.Max(0, pozitionList.Last().Item1 - visibleRange); r <= Math.Min(pozitionList.Last().Item1 + visibleRange, Dungeon.LevelData.GetLength(0) - 1); r++)
			{
				for (int c = Math.Max(0, pozitionList.Last().Item2 - visibleRange); c <= Math.Min(pozitionList.Last().Item2 + visibleRange, Dungeon.LevelData.GetLength(1) - 1); c++)
				{
					visibles.Add(new ValueTuple<int, int>(r, c));
				}
			}

			foreach (var poz in visibles.Distinct())
			{
				newMapitem = Dungeon.GetMapItemByPosition(poz.Item1, poz.Item2);
				oldMapItem = MapItems.FirstOrDefault(s => s.Row == poz.Item1 && s.Column == poz.Item2);
				if (oldMapItem == null)
				{
					oldMapItem = new MapItem();
					oldMapItem.Row = poz.Item1;
					oldMapItem.Column = poz.Item2;
				}
				oldMapItem.ImagesSumValue = newMapitem.ImagesSumValue;
			}
		}
		#endregion methods
	}
}