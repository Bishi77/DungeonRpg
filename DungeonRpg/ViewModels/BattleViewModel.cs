using DungeonRpg.Models;
using DungeonRpg.Models.Helpers;
using System.Windows.Input;

namespace DungeonRpg.ViewModels
{
	public class BattleViewModel : ModelBase
	{
		private Monster _enemy;
		public Monster Enemy { get => _enemy; set => _enemy = value; }
		
		private Player _player;
		public Player Player { get => _player; set => _player = value; }

		public ICommand ChangeContentCommand { get; set; }

		public BattleViewModel()
		{
		}

		public BattleViewModel(Player player, Monster enemy)
		{
			Player = player;
			Enemy = enemy;
		}

		
	}
}
