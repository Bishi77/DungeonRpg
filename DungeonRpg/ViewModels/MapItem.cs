using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace DungeonRpg.ViewModels
{
	public class MapItem : BindableBaseViewModel
	{
		public static int Rows;
		public static int Columns;

		private static Dictionary<int, BitmapImage> _mapItemCache = new Dictionary<int, BitmapImage>();
		public static Dictionary<int, BitmapImage> MapItemCache
		{
			get { return _mapItemCache; }
			set
			{
				_mapItemCache = value;
			}
		}

		private int _row;
		public int Row { get => _row; set => _row = value; }

		private int _column;
		public int Column { get => _column; set => _column = value; }

		private int _imagesSumValue;
		public int ImagesSumValue
		{
			get => _imagesSumValue;
			set
			{
				_imagesSumValue = value;
				OnPropertyChanged(nameof(Image));
			}
		}

		public BitmapImage Image
		{
			get => MapItemCache[ImagesSumValue]; 
			set
			{
				MapItemCache[ImagesSumValue] = value;
				OnPropertyChanged(nameof(Image));
			}
		}		
	}
}