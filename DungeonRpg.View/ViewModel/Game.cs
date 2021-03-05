using DungeonRpg.Model;
using DungeonRpg.View.Model;
using DungeonRpg.ViewModel.Helpers;
using System.ComponentModel;
using System.Data;

namespace DungeonRpg.ViewModel
{
	public class Game : INotifyPropertyChanged
	{
		private Dungeon _dungeon = new Dungeon(new float[0,0]);
		private Character _character = new Character();
		private DataView _map = new DataView();

		public Game()
		{
			DungeonGenerator generator = new DungeonGenerator();
			Dungeon.LevelData = generator.GenerateDungeonLevel(10, 10, 25);
			Character = CharacterGenerator.Generate(true);
			Character.Position = Dungeon.GetValuePosition((int)DungeonGenerator.FieldTypes.Start);
		}

		

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
			}
		}

		public DataView Map 
		{ 
			get => ConversionFunctions.GetBindable2DArray<float>(Dungeon.LevelData); 
			set => _map = value; 
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