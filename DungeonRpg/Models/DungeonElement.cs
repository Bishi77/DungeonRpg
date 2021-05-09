namespace DungeonRpg.Models
{
	/// <summary>
	/// Pálya elem típusa, típus függő speciális működés lehet szükséges
	/// </summary>
	public enum DungeonElementType { Wall = 0, StartPoint = 1, EndPoint = 2, Item = 1000, Player = 4, UpStairs = 8, DownStairs = 16, Way = 32, MonsterType = 64 }
	
	public class DungeonElement
	{
		/// <summary>
		/// Elem típusa
		/// </summary>
		private readonly DungeonElementType _elementType = DungeonElementType.Wall;
		/// <summary>
		/// Adott elem azonosító a típuson belül
		/// </summary>
		private readonly int _elementID = -1;

		public DungeonElementType ElementType => _elementType;
		public int ElementID => _elementID;

		public DungeonElement(DungeonElementType elementType, int elementID = -1)
		{
			_elementType = elementType;
			this._elementID = elementID;
		}
	}
}