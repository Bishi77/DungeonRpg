using DungeonRpg.Model.Interface;
using DungeonRpg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Model
{
	public class Character
	{

		private IInventory _inventory;

		private string _name;
		private int _strength;
		private int _dexterity;
		private int _intelligence;
		private int _constitution;
		private int _wisdom;
		private int _charisma;

		private int _level;
		private int _hp;
		private int _mp;
		private (int, int) _position = (-1, -1);

		public Character()
		{
			Random rnd = new Random();
			Dice diceD6 = new Dice(6);
			Level = 1;

			Strength = diceD6.ResetSum().RollDices(3).SumRolls;
			Dexterity = diceD6.ResetSum().RollDices(3).SumRolls; ;
			Intelligence = diceD6.ResetSum().RollDices(3).SumRolls;
			Constitution = diceD6.ResetSum().RollDices(3).SumRolls;
			Wisdom = diceD6.ResetSum().RollDices(3).SumRolls;

			MP = MaxMp;
			HP = MaxHp;
		}

		#region properties

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		public int Level
		{
			get { return _level; }
			set { _level = value; }
		}

		public int HP
		{
			get { return _hp; }
			set { _hp = value; }
		}

		public int MP
		{
			get { return _mp; }
			set { _mp = value; }
		}

		public int MaxHp
		{
			get 
			{ 
				return Level * (8 + (Constitution - 10)/2); 
			}
		}

		public int MaxMp
		{
			get { return Level * (6 + (Intelligence - 10) / 2); }
		}

		public int Strength 
		{
			get { return _strength; }
			set { _strength = value; } 
		}
		
		public int Dexterity
		{
			get { return _dexterity; }
			set{ _dexterity = value; }
		}
		
		public int Intelligence 
		{
			get { return _intelligence; }
			set { _intelligence = value; } 
		}

		public int Constitution
		{
			get { return _constitution;	}
			set{ _constitution = value;}
		}

		public int Wisdom
		{
			get { return _wisdom; }
			set{ _wisdom = value; }
		}

		public int Charisma
		{
			get { return _charisma; }
			set { _charisma = value; }
		}

		#endregion properties

		public IInventory Inventory
		{
			get { return _inventory; }
			set { _inventory = value; }
		}

		internal void Move(DungeonGenerator.Direction direction)
		{
			switch (direction)
			{
				case DungeonGenerator.Direction.UP:
					Position = (Position.Item1 - 1, Position.Item2);
					break;
				case DungeonGenerator.Direction.DOWN:
					Position = (Position.Item1 + 1, Position.Item2);
					break;
				case DungeonGenerator.Direction.LEFT:
					Position = (Position.Item1, Position.Item2 - 1);
					break;
				case DungeonGenerator.Direction.RIGHT:
					Position = (Position.Item1, Position.Item2 + 1);
					break;
				default:
					break;
			}
		}

		public (int, int) Position
		{
			get { return _position; }
			set
			{
				_position = value;
			}
		}
	}
}
