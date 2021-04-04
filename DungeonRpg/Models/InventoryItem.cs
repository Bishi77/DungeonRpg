using System;

namespace DungeonRpg.Models
{
	public enum Category
	{
		MeleeWeapon = 1, RangedWeapon = 2, HealingPotion = 3, Shield = 4, Armour = 5
	}

	public class InventoryItem
	{
		private readonly Category _category;
		private string _name;
		private string _description;		

		public string Name { get => _name; set => _name = value; }
		public string Description { get => _description; set => _description = value; }

		// TODO
		private Action _exec;

		// TODO
		public Action Exec { get => _exec; set => _exec = value; }

		public Category Category => _category;


		public InventoryItem(Category category)
		{
			_category = category;
		}

		// TODO
		public bool CanExec(Func<Action, bool> canExec)
		{
			return true;
		}
	}
}
