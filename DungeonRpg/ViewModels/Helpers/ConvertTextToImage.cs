using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DungeonRpg.ViewModels.Helpers
{
	public class ConvertTextToImage: IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is null)
                return new BitmapImage();
               
            return new BitmapImage(new Uri(value.ToString(), UriKind.RelativeOrAbsolute));

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
