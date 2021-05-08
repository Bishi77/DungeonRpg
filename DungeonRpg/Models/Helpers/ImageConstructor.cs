using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace DungeonRpg.Models.Helpers
{
	public static class ImageConstructor
	{
		private static Assembly ass = AppDomain.CurrentDomain.GetAssemblies().
								SingleOrDefault(assembly => assembly.GetName().Name == "UniversalDesign");

		public static BitmapImage MergeImages(List<string> imagesPathWithFileNames)
		{
			BitmapImage result = new BitmapImage();
			

			if (imagesPathWithFileNames.Count == 0)
				return result;

			Bitmap baseImage = new Bitmap(32, 32);

			using (var g = Graphics.FromImage(baseImage))
			{
				foreach (var image in imagesPathWithFileNames)
				{
					g.DrawImage(System.Drawing.Image.FromStream(ass.GetManifestResourceStream(image)), 0, 0);
				}
			}
			using (var ms = new MemoryStream())
			{
				baseImage.Save(ms, ImageFormat.Png);
				ms.Seek(0, SeekOrigin.Begin);

				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = ms;
				bitmapImage.EndInit();

				return bitmapImage;
			}
		}
	}
}
