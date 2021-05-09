using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.Models
{
	public class Dice
	{
		//Kocka oldalainak a száma
		private int DiceSide;
		private static Random _rnd = new Random();
		public static Random Rnd { get => _rnd; }

		///Dobás összege
		public int SumRolls = 0;

		#region ctor
		public Dice(int sides)
		{
			DiceSide = sides;
		}
		#endregion ctor

		#region methods
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
			return Rnd.Next(DiceSide) + 1;
		}
		#endregion methods
	}
}
