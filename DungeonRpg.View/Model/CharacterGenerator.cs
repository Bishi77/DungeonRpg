using DungeonRpg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.View.Model
{
	public static class CharacterGenerator
	{
		public static Character Generate(bool isPlayerCharacter = false)
		{
			return new Character(); 
		}
	}
}
