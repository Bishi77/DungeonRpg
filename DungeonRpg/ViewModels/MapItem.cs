using System.Windows.Media.Imaging;

namespace DungeonRpg.ViewModels
{
	public class MapItem : BindableBaseViewModel
	{
		public static int Rows;
		public static int Columns;

		private int _row;
		public int Row { get => _row; set => _row = value; }

		private int _column;
		public int Column { get => _column; set => _column = value; }

		private BitmapImage _image;
		public BitmapImage Image
		{
			get => _image; set
			{
				SetProperty(ref _image, value);
			}
		}
	}
}