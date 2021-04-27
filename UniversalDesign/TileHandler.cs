using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UniversalDesign
{
	public class TileHandler
    {
        //Using: bitmap = new BitmapImage(uri);
        public Uri GetTileImageUri(TileCategory category, TileSubCategory subCategory, List<string> detailPattern)
		{
			Uri bitmapUriSource = new Uri(GetTilePathWithFileName(category, subCategory, detailPattern));

			return bitmapUriSource;
		}

		public string GetTilePathWithFileName(TileCategory category, TileSubCategory subCategory, List<string> detailPattern)
		{
			string result = "";
			string path = $"..\\..\\..\\UniversalDesign\\Resources\\Tiles\\rltiles\\{category.Value}\\{subCategory.Value}\\";
			path = Path.Combine(Environment.CurrentDirectory, path);
			if (Directory.Exists(path))
			{
				string[] fileEntries = Directory.GetFiles(path);
				result = fileEntries.Where(w => detailPattern.Count == 0 || (detailPattern.Count > 0 && detailPattern.All(d => w.Contains(d)))).FirstOrDefault();
			}

			return result;
		}

	}
}
