using System;

namespace DungeonRpg.Models
{
	public class Character 
	{
		#region properties
		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private int _level;
		public int Level
		{
			get { return _level; }
			set { _level = value; }
		}

		private int _hp;
		public int HP
		{
			get { return _hp; }
			set { _hp = value; }
		}

		private int _mp;
		public int MP
		{
			get { return _mp; }
			set { _mp = value; }
		}

		public int MaxHp
		{
			get
			{
				return Level * (8 + (Constitution - 10) / 2);
			}
		}

		public int MaxMp
		{
			get { return Level * (6 + (Intelligence - 10) / 2); }
		}

		private int _strength;
		public int Strength
		{
			get { return _strength; }
			set { _strength = value; }
		}

		private int _dexterity;
		public int Dexterity
		{
			get { return _dexterity; }
			set { _dexterity = value; }
		}

		private int _intelligence;
		public int Intelligence
		{
			get { return _intelligence; }
			set { _intelligence = value; }
		}

		private int _constitution;
		public int Constitution
		{
			get { return _constitution; }
			set { _constitution = value; }
		}

		private int _wisdom;
		public int Wisdom
		{
			get { return _wisdom; }
			set { _wisdom = value; }
		}

		private int _charisma;
		public int Charisma
		{
			get { return _charisma; }
			set { _charisma = value; }
		}

		private (int, int) _position = (-1, -1);
		public (int, int) Position
		{
			get { return _position; }
			set
			{
				_position = value;
			}
		}

		private int _visibilityRange;
		public int VisibilityRange
		{
			get => _visibilityRange;
			set => _visibilityRange = value;
		}

		private Inventory _inventory;
		public Inventory Inventory
		{
			get { return _inventory; }
			set { _inventory = value; }
		}

		#endregion properties

		#region ctor		
		public Character()
		{
			Dice dice = new Dice(6);
			Level = 1;
			VisibilityRange = 1;

			Strength = dice.ResetSum().RollDices(3).SumRolls;
			Dexterity = dice.ResetSum().RollDices(3).SumRolls; ;
			Intelligence = dice.ResetSum().RollDices(3).SumRolls;
			Constitution = dice.ResetSum().RollDices(3).SumRolls;
			Wisdom = dice.ResetSum().RollDices(3).SumRolls;
			Charisma = dice.ResetSum().RollDices(3).SumRolls;
			MP = MaxMp;
			HP = MaxHp;

			_inventory = new Inventory();
		}
		#endregion ctor

		#region public methods
		public void Move(Dungeon.Direction direction, Dungeon dungeon)
		{
			var oldPosition = Position;
			switch (direction)
			{
				case Dungeon.Direction.UP:
					Position = (Position.Item1 - 1, Position.Item2);
					break;
				case Dungeon.Direction.DOWN:
					Position = (Position.Item1 + 1, Position.Item2);
					break;
				case Dungeon.Direction.LEFT:
					Position = (Position.Item1, Position.Item2 - 1);
					break;
				case Dungeon.Direction.RIGHT:
					Position = (Position.Item1, Position.Item2 + 1);
					break;
				default:
					break;
			}

			dungeon.SetVisitedArea(Position, VisibilityRange);
			dungeon.MoveItem(oldPosition, Position, DungeonElementType.Player);
		}
		#endregion public methods
	}
}
