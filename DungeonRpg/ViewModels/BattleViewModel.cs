using DungeonRpg.Models;
using DungeonRpg.Models.Helpers;
using DungeonRpg.ViewModels.Helpers;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DungeonRpg.ViewModels
{
	public class BattleViewModel : ModelBase
	{
		private Monster _enemy;
		public Monster Enemy { get => _enemy; set => _enemy = value; }
		
		private Player _player;
		public Player Player { get => _player; set => _player = value; }

		private BitmapImage _playerImage;
		public BitmapImage PlayerImage { get => _playerImage; set => _playerImage = value; }

		private BitmapImage _enemyImage;
		public BitmapImage EnemyImage { get => _enemyImage; set => _enemyImage = value; }

		public ICommand GoToGameCommand { get; set; }
		public ICommand RunAwayCommand { get; set; }

		public BattleViewModel(){}

		public BattleViewModel(Player player, Monster enemy, BitmapImage playerImg, BitmapImage enemyImg)
		{
			Player = player;
			Enemy = enemy;
			_playerImage = playerImg;
			_enemyImage = enemyImg;
			RunAwayCommand = new CommandImplementation(Run);
		}

		private void Run(object obj)
		{
			if(Dice.Rnd.Next(20) + (Player.GetAttributeModify(Player.Dexterity)) < (Dice.Rnd.Next(20) + (Monster.GetAttributeModify(Enemy.Dexterity))))
			{
				Player.HP -= Enemy.Attack();
			}
			GoToGameCommand.Execute("Game");

		}
	}
}
