namespace DungeonRpg.Models
{
	/// <summary>
	/// Pálya elem típusa, típus függő speciális működés lehet szükséges
	/// </summary>
	public enum DungeonElementType { Wall = 0, StartPoint = 1, EndPoint = 2, Item = 3, Player = 4, Monster = 5, UpStairs = 6, DownStairs = 7, Way = 8 }
	
	//régi mezők, megfeleltetni az újjal
	//public enum FieldTypes { Wall = 0, Finish = 1, Down = 2, Up = 3, Monster = 4, Way = 5, Start = 6 };
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

		public DungeonElement(DungeonElementType elementType, int elementID)
		{
			_elementType = elementType;
			this._elementID = elementID;
		}
	}
}