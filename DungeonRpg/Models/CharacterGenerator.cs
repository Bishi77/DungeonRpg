namespace DungeonRpg.Models
{
	public static class CharacterGenerator
	{
		public static Character Generate((int, int) startPosition, Dungeon dungeon)
		{
			var character = new Character
			{
				Position = startPosition
			};
			dungeon.AddDungeonElementByPosition(character.Position.Item1, character.Position.Item2, new DungeonElement(DungeonElementType.Player, -1), true);
			character.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			return character;
		}
	}
}
