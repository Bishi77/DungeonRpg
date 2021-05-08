using DungeonRpg.Models;
using DungeonRpg.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UniversalDesign;

namespace DungeonRpg.ViewModels
{
	public class GameViewModel : BindableBaseViewModel, INotifyPropertyChanged, ICanEnableField
	{
		#region fields
		private Dungeon _dungeon = new Dungeon(new List<DungeonElement>[0, 0]);
		private Character _character = new Character();
		private ObservableCollection<MapItem> _mapItems = new ObservableCollection<MapItem>();
		private string _possibleDirections = "";
		private readonly CanEnableFieldHelper _helper;
		#endregion fields

		public ICommand MoveCommand { get; set; }
		public ICommand GoToInventoryCommand { get; set; }

		#region properties
		public ObservableCollection<MapItem> MapItems { get => _mapItems; set => _mapItems = value; }

		public Dungeon Dungeon
		{
			get { return _dungeon; }
			set
			{
				_dungeon = value;
				OnPropertyChanged(nameof(Dungeon));
			}
		}

		public Character Character
		{
			get { return _character; }
			set
			{
				_character = value;
				OnPropertyChanged(nameof(Character));
			}
		}

		public string PossibleDirections
		{
			get { return _possibleDirections; }
			set
			{
				_possibleDirections = value;
				OnPropertyChanged(nameof(CanEnable));
				OnPropertyChanged(nameof(MapItems));
			}
		}

		public dynamic CanEnable
		{
			get { return _helper; }
		}

		public bool CanEnableField(string key)
		{
			return PossibleDirections.Contains(key);
		}
		#endregion properties

		#region constructor
		public GameViewModel()
		{
			MoveCommand = new CommandImplementation(MoveCharacter);
			_helper = new CanEnableFieldHelper(this);
			OnPropertyChanged(nameof(CanEnable));
		}
		#endregion constructor

		#region View Commands
		public void StartGame()
		{
			DungeonGenerator _generator = new DungeonGenerator(20, 20, 5, 70, 70, 5);
			Dungeon = _generator.GenerateDungeon();
			Character = CharacterGenerator.Generate(Dungeon.GetFirstDungeonElementPosition(DungeonElementType.StartPoint));
			Dungeon.SetVisitedArea(Character.Position, Character.VisibilityRange);
			Dungeon.LevelData[Character.Position.Item1, Character.Position.Item2].Add(new DungeonElement(DungeonElementType.Player, -1));
			DrawMap();
			SetPossibleDirection();
		}
		#endregion View Commands

		#region methods
		private void MoveCharacter(object obj)
		{
			if (PossibleDirections.Contains(obj.ToString()))
			{
				(int, int) oldPosition = Character.Position;
				Character.Move((Dungeon.Direction)obj.ToString()[0], Dungeon);
				RefreshMapItems2(new List<ValueTuple<int, int>> { oldPosition, Character.Position }, Character.VisibilityRange);
				SetPossibleDirection();
				OnPropertyChanged(nameof(MapItems));
			}
		}

		private void SetPossibleDirection()
		{
			PossibleDirections = Dungeon.GetPossibleMoveDirections(Character.Position.Item1, Character.Position.Item2);
		}

		private void DrawMap()
		{
			for (int row = 0; row < Dungeon.LevelData.GetLength(0); row++)
			{
				for (int col = 0; col < Dungeon.LevelData.GetLength(1); col++)
				{
					MapItems.Add(GetMapItemByPosition(row, col));
				}
			}
			MapItem.Rows = Dungeon.LevelData.GetLength(0);
			MapItem.Columns = Dungeon.LevelData.GetLength(1);
		}

		private void RefreshMapItems(List<(int, int)> pozitionList, int visibleRange)
		{
			var visibles = new List<MapItem>();
			MapItem newMapitem = null;
			MapItem oldMapItem = null;
			//új és régi poz. frisssítés
			foreach (var poz in pozitionList)
			{
				newMapitem = GetMapItemByPosition(poz.Item1, poz.Item2);
				oldMapItem = MapItems.FirstOrDefault(s => s.Row == poz.Item1 && s.Column == poz.Item2);
				if (oldMapItem == null)
				{
					oldMapItem = new MapItem();
					oldMapItem.Row = poz.Item1;
					oldMapItem.Column = poz.Item2;
				}
				oldMapItem.ImagesSumValue = newMapitem.ImagesSumValue;
			}

			//láthatóság
			for (int r = Math.Max(0, newMapitem.Row - visibleRange); r < Math.Min(newMapitem.Row + visibleRange + 1, Dungeon.LevelData.GetLength(0)); r++)
			{
				for (int c = Math.Max(0, newMapitem.Column - visibleRange); c < Math.Min(newMapitem.Column + visibleRange + 1, Dungeon.LevelData.GetLength(1)); c++)
				{
					oldMapItem = MapItems.FirstOrDefault(s => s.Row == r && s.Column == c);
					newMapitem = GetMapItemByPosition(r, c);
					oldMapItem.ImagesSumValue = newMapitem.ImagesSumValue;
				}
			}
		}

		private void RefreshMapItems2(List<ValueTuple<int, int>> pozitionList, int visibleRange)
		{
			MapItem newMapitem = null;
			MapItem oldMapItem = null;
			List<ValueTuple<int, int>> visibles = new List<ValueTuple<int, int>>();
			visibles.AddRange(pozitionList);
			//új és régi poz. frisssítés-hez hozzáadjuk a láthatóságot, ami az új poz. környezete
			for (int r = Math.Max(0, pozitionList.Last().Item1 - visibleRange); r <= Math.Min(pozitionList.Last().Item1 + visibleRange, Dungeon.LevelData.GetLength(0) - 1); r++)
			{
				for (int c = Math.Max(0, pozitionList.Last().Item2 - visibleRange); c <= Math.Min(pozitionList.Last().Item2 + visibleRange, Dungeon.LevelData.GetLength(1) - 1); c++)
				{
					visibles.Add(new ValueTuple<int, int>(r, c));
				}
			}

			foreach (var poz in visibles.Distinct())
			{
				newMapitem = GetMapItemByPosition(poz.Item1, poz.Item2);
				oldMapItem = MapItems.FirstOrDefault(s => s.Row == poz.Item1 && s.Column == poz.Item2);
				if (oldMapItem == null)
				{
					oldMapItem = new MapItem();
					oldMapItem.Row = poz.Item1;
					oldMapItem.Column = poz.Item2;
				}
				oldMapItem.ImagesSumValue = newMapitem.ImagesSumValue;
			}
		}

		private MapItem GetMapItemByPosition(int row, int col)
		{
			MapItem mapItem = new MapItem();
			mapItem.Row = row;
			mapItem.Column = col;
			mapItem.ImagesSumValue = Dungeon.GetPositionSumValue(row, col);
			if(!MapItem.MapItemCache.ContainsKey(mapItem.ImagesSumValue))
				mapItem.Image = MergeImages(GetMapPositionTilesPathsWithFileName(Dungeon.LevelData[row, col], Dungeon.LevelVisited[row, col]));

			return mapItem;
		}

		private List<string> GetMapPositionTilesPathsWithFileName(List<DungeonElement> elementsAtPosition, bool visited)
		{
			List<string> positionImages = new List<string>();
			elementsAtPosition.ForEach(x => positionImages.Add(GetMapPositionTilePathWithFileName(x, visited)));
			return positionImages;
		}

		private string GetMapPositionTilePathWithFileName(DungeonElement element, bool visited)
		{
			TileCategory tileCategory = TileCategory.Error;
			TileSubCategory tileSubCategory = TileSubCategory.Error;
			string pngName = "";

			if(!visited)
				return $"UniversalDesign.Resources.Tiles.rltiles.{TileCategory.Dungeon.Value}.unseen.png";


			switch (element.ElementType)
			{
				case DungeonElementType.Wall:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Wall;
					pngName = "catacombs0.png";
					break;
				case DungeonElementType.StartPoint:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "bazaar_portal.png";
					break;
				case DungeonElementType.EndPoint:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "exit_dungeon.png";
					break;
				case DungeonElementType.Item:
					tileCategory = TileCategory.Item;
					tileSubCategory = TileItemSubCategory.Armour;
					pngName = "ring_mail1.png";
					break;
				case DungeonElementType.Player:
					tileCategory = TileCategory.Monster;
					tileSubCategory = TileMonsterSubCategory.Humanoids;
					pngName = "dwarf.png";
					break;
				case DungeonElementType.Monster:
					tileCategory = TileCategory.Monster;
					tileSubCategory = TileMonsterSubCategory.Demons;
					pngName = "crimson_imp.png";
					break;
				case DungeonElementType.UpStairs:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "stone_stairs_up.png";
					break;
				case DungeonElementType.DownStairs:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					pngName = "stone_stairs_down.png";
					break;
				case DungeonElementType.Way:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Floor;
					pngName = "sand1.png";
					break;
				default:
					break;
			}

			return $"UniversalDesign.Resources.Tiles.rltiles.{tileCategory.Value}.{tileSubCategory.Value}.{pngName}";
		}

		private BitmapImage MergeImages(List<string> imagesPathWithFileNames)
		{
			BitmapImage result = new BitmapImage();
			Assembly ass = AppDomain.CurrentDomain.GetAssemblies().
								SingleOrDefault(assembly => assembly.GetName().Name == "UniversalDesign");
				
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
		#endregion methods
	}
}