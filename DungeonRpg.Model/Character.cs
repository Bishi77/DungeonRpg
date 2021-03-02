﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Model
{
	public class Character
	{
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

		private IInventory _inventory;

		public IInventory Inventory
		{
			get { return  _inventory; }
			set {  _inventory = value; }
		}





	}
}
