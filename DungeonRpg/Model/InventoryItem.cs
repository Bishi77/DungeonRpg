using DungeonRpg.Model.Interface;
using System;

namespace DungeonRpg.Model
{
	public class InventoryItem : IInventoryItem
	{
		private string _name;

		private string _description;

		public string Name { get => _name; set => _name = value; }
		public string Description { get => _description; set => _description = value; }

		// TODO
		private Action _exec;

		// TODO
		public Action Exec { get => _exec; set => _exec = value; }
		

		// TODO
		public bool CanExec(Func<Action, bool> canExec)
		{
			return true;
		}
	}
}
