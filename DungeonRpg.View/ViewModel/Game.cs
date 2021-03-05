using DungeonRpg.Model;
using DungeonRpg.View.Model;
using System.ComponentModel;

namespace DungeonRpg.ViewModel
{
	public class Game : INotifyPropertyChanged
	{
		private Dungeon _dungeon = new Dungeon();
		private Character _character = new Character();

		public Game()
		{
			_dungeon = DungeonGenerator.Generate(10, 10);
			_character = CharacterGenerator.Generate(true);
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

		#region change notify 
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion change notify 

	}
}