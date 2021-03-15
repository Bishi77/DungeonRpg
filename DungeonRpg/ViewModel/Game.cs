using DungeonRpg.Model;
using DungeonRpg.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;

namespace DungeonRpg.ViewModel
{
	public class Game : INotifyPropertyChanged, ICanEnableField
	{
		private Dungeon _dungeon = new Dungeon(new List<DungeonElement>[0,0]);
		private Character _character = new Character();
		private DataView _map = new DataView();
		private string _possibleDirections = "";
		private readonly CanEnableFieldHelper _helper;

		public ICommand MoveCommand { get; set; }

		#region constructor

		public Game()
		{
			DungeonGenerator _generator = new DungeonGenerator();
			Dungeon = _generator.AddPlacePOIsToDungeonLevel(
						_generator.AddWaysToDungeonLevel(
							_generator.GenerateDungeonLevel(10, 10)
						, 40),
					  true, true, 10);
			Character = CharacterGenerator.Generate(true);
			Character.Position = Dungeon.GetFirstDungeonElementPosition(DungeonElementType.StartPoint);
			SetPossibleDirection();

			MoveCommand = new CommandImplementation(MoveCharacter);
			_helper = new CanEnableFieldHelper(this);
			OnPropertyChanged(nameof(CanEnable));
		}

		#endregion constructor

		#region properties

		public Dungeon Dungeon
		{
			get { return _dungeon; }
			set
			{
				_dungeon = value;
				OnPropertyChanged(nameof(Dungeon));
			}
		}

		public Character Character
		{
			get { return _character; }
			set
			{
				_character = value;
				OnPropertyChanged(nameof(Character));
			}
		}

		public DataView Map 
		{
			get => ConversionFunctions.GetBindable2DArray<List<DungeonElement>>(Dungeon.LevelData); 
			set => _map = value; 
		}

		public string PossibleDirections
		{
			get { return _possibleDirections; }
			set
			{
				_possibleDirections = value;
				OnPropertyChanged(nameof(CanEnable));
				OnPropertyChanged(nameof(Map));
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

		#region View Commands

		public void MoveCharacter(char direction)
		{
			if (PossibleDirections.Contains(direction.ToString()))
			{
				Character.Move((DungeonGenerator.Direction)direction);
				SetPossibleDirection();
				OnPropertyChanged(nameof(Map));
			}
		}

		public bool EnablePossibleDirection(char direction)
		{
			return PossibleDirections.Contains(direction.ToString());
		}

		#endregion View Commands

		#region private methods

		private void MoveCharacter(object obj)
		{
			if (PossibleDirections.Contains(obj.ToString()))
			{
				Character.Move((DungeonGenerator.Direction)obj.ToString()[0]);
				SetPossibleDirection();
				OnPropertyChanged(nameof(Map));
			}
		}

		private void SetPossibleDirection()
		{
			PossibleDirections = Dungeon.GetPossibleMoveDirections(Character.Position.Item1, Character.Position.Item2);
		}

		#endregion private methods

		#region change notify 
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion change notify 

	}
}