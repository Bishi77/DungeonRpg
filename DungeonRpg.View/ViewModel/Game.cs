using DungeonRpg.Model;
using DungeonRpg.View.Model;
using DungeonRpg.ViewModel.Helpers;
using System.ComponentModel;
using System.Data;

namespace DungeonRpg.ViewModel
{
	public class Game : INotifyPropertyChanged, ICanEnableField
	{
		private Dungeon _dungeon = new Dungeon(new float[0,0]);
		private Character _character = new Character();
		private DataView _map = new DataView();
		private DungeonGenerator _generator = new DungeonGenerator();
		private string _possibleDirections = "";
		private readonly CanEnableFieldHelper _helper;

		public Game()
		{
			_generator = new DungeonGenerator();
			Dungeon = new Dungeon(_generator.GenerateDungeonLevel(10, 10, 25));
			Character = CharacterGenerator.Generate(true);
			Character.Position = Dungeon.GetValuePosition((int)DungeonGenerator.FieldTypes.Start);
			SetPossibleDirection();
			_helper = new CanEnableFieldHelper(this);
			OnPropertyChanged(nameof(CanEnable));
		}

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
			get => ConversionFunctions.GetBindable2DArray<float>(Dungeon.LevelData); 
			set => _map = value; 
		}

		public string PossibleDirections
		{
			get { return _possibleDirections; }
			set
			{
				_possibleDirections = value;
				OnPropertyChanged(nameof(PossibleDirections));
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

		private void SetPossibleDirection()
		{
			PossibleDirections = _generator.GetPossibleDirections(Dungeon.LevelData, Character.Position.Item1, Character.Position.Item2);
		}

		#region change notify 
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion change notify 

	}
}