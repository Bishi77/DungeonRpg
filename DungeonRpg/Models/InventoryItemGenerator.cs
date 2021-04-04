using System;
using System.Collections.Generic;

namespace DungeonRpg.Models
{
	public class InventoryItemGenerator
	{
		public static List<InventoryItem> GenerateRandomItems(int itemNumber)
		{
			List<InventoryItem> itemList = new List<InventoryItem>();
			Random rnd = new Random();

			for (int i = 0; i < itemNumber; i++)
			{
				Array types = Enum.GetValues(typeof(Category));
				InventoryItem item = new InventoryItem((Category)types.GetValue(rnd.Next(types.Length)));
				item.Name = GenerateName(item.Category);
				item.Description = GenerateDescription(item);

				itemList.Add(item);
			}

			return itemList;
		}

		#region Generate name members

		public static string GenerateName(Category category)
		{
			string name;
			switch (category)
			{
				case Category.MeleeWeapon:
					name = GetRandomItem(GetMeleeWeaponNames());
					break;
				case Category.RangedWeapon:
					name = GetRandomItem(GetRangedWeaponNames());
					break;
				case Category.HealingPotion:
					name = GetRandomItem(GetHealingPotionNames());
					break;
				case Category.Shield:
					name = GetRandomItem(GetShieldNames());
					break;
				case Category.Armour:
					name = GetRandomItem(GetArmourNames());
					break;
				default:
					name = "Ismeretlen";
					break;
			}

			return name;
		}

		private static string GetRandomItem(IList<string> items)
		{
			Random rnd = new Random();
			return items[rnd.Next(items.Count)];
		}

		private static IList<string> GetArmourNames()
		{
			List<string> names = new List<string>();

			names.Add("Fémvért");
			names.Add("Bronzvért");
			names.Add("Vasing");
			names.Add("Bőrvért");
			names.Add("Láncing");
			names.Add("Sodronying");

			return names;
		}

		private static IList<string> GetShieldNames()
		{
			List<string> names = new List<string>();

			names.Add("Kis pajzs");
			names.Add("Kerek pajzs");
			names.Add("Tükör pajzs");
			names.Add("Nagy pajzs");
			names.Add("Fa pajzs");
			names.Add("Mithril pajzs");

			return names;
		}

		private static IList<string> GetHealingPotionNames()
		{
			List<string> names = new List<string>();

			names.Add("Kis gyógyital");
			names.Add("Közepes gyógyital");
			names.Add("Nagy gyógyital");

			return names;
		}

		private static IList<string> GetRangedWeaponNames()
		{
			List<string> names = new List<string>();

			names.Add("Dobó tű");
			names.Add("Köpőcső");
			names.Add("Hosszú íj");
			names.Add("Rövid íj");
			names.Add("Számszeríj");
			names.Add("Nehéz számszeríj");

			return names;
		}

		private static IList<string> GetMeleeWeaponNames()
		{
			List<string> names = new List<string>();

			names.Add("Balta");
			names.Add("Bot");
			names.Add("Hosszú kard");
			names.Add("Rövid kard");
			names.Add("Harci kalapács");
			names.Add("Kétkezes kard");
			names.Add("Buzogány");
			names.Add("Tőr");

			return names;
		}

		#endregion Generate name members

		#region Generate description

		public static string GenerateDescription(InventoryItem item)
		{
			string desc;

			switch (item.Category)
			{
				case Category.MeleeWeapon:
					desc = "Közelharci fegyver";
					break;
				case Category.RangedWeapon:
					desc = "Távolsági fegyver";
					break;
				case Category.HealingPotion:
					desc = "Gyógyital fegyver";
					break;
				case Category.Shield:
					desc = "Pajzs";
					break;
				case Category.Armour:
					desc = "Páncél";
					break;
				default:
					desc = "Ismeretlen tárgy. Kockázatos a használata.";
					break;
			}

			return desc;
		}

		#endregion Generate description		
	}
}