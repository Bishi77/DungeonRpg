using DungeonRpg.Model;
using DungeonRpg.View.Model;

namespace DungeonRpg.ViewModel
{
	public class Game
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
	}
}