using DungeonRpg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Model
{
	public static class CharacterGenerator
	{
		public static Character Generate(bool isPlayerCharacter = false)
		{
			var player = new Character();
			return player;
		}
	}
}
