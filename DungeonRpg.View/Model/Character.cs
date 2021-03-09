﻿using DungeonRpg.Model.Interface;
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

		private int _level;
		private int _hp;
		private int _maxHp;
		private int _mp;
		private int _maxMp;
		private (int, int) _position = (-1, -1);

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

		public int MaxHp
		{
			get { return _maxHp; }
			set
			{
				_maxHp = value;
			}
		}

		public int MaxMp
		{
			get { return _maxMp; }
			set
			{
				_maxMp = value;
			}
		}
		#endregion properties
	}
}
