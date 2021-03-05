using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.View.Model
{
	public class CoordinateValue<T>
	{
		public T Value { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }
	}
}

