using DungeonRpg.Models.Helpers;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace DungeonRpg.Models
{
	public class MapItem : ModelBase
	{
		public static int Rows;
		public static int Columns;

		private static Dictionary<string, BitmapImage> _mapItemCache = new Dictionary<string, BitmapImage>();
		public static Dictionary<string, BitmapImage> MapItemCache
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

		private string _imagesSumValue;
		public string ImagesSumValue
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