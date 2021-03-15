using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Model
{
	public class Dice
	{
		int DiceSide;
		Random _rnd = new Random();

		public int SumRolls = 0;
		
		public Dice(int sides)
		{
			DiceSide = sides;
		}

		public Dice SetSides(int sides)
		{
			DiceSide = sides;
			return this;
		}

		public Dice RollDices(int dices)
		{
			for(int roll = 0; roll < dices; roll++ )
			{
				SumRolls += RollDice();
			}
			
			return this;
		}

		public Dice ResetSum()
		{
			SumRolls = 0;
			return this;
		}

		private int RollDice()
		{
			return _rnd.Next(DiceSide) + 1;
		}
	}
}
