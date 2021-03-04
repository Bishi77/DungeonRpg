using DungeonRpg.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.View.Model
{
	public class InventoryItem : IInventoryItem
	{
		public List<object> GetDescription()
		{
			throw new NotImplementedException();
		}

		public string GetName()
		{
			throw new NotImplementedException();
		}

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
