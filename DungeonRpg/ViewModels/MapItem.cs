using System.Windows.Media.Imaging;

namespace DungeonRpg.ViewModels
{
	public class MapItem
	{
		static public int Rows;
		static public int Columns;
		int _row;	
		int _column;
		BitmapImage _image;

		public int Row { get => _row; set => _row = value; }
		public int Column { get => _column; set => _column = value; }
		public BitmapImage Image { get => _image; set => _image = value; }
	}
}