using System;

namespace DungeonRpg.Models
{
	public class Player : Character
	{
		public Player(){}

		public Player(int diceSize, int diceNr, ValueTuple<int, int> startPosition) : base(diceSize, diceNr, startPosition)
		{

		}
	}
}
