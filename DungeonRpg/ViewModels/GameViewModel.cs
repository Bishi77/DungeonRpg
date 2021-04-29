using DungeonRpg.Models;
using DungeonRpg.ViewModels.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UniversalDesign;
using static System.Net.Mime.MediaTypeNames;

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
			DungeonGenerator _generator = new DungeonGenerator();
			Dungeon = _generator.AddPlacePOIsToDungeonLevel(
						_generator.AddWaysToDungeonLevel(
							_generator.GenerateDungeonLevel(10, 10)
						, 40),
					  true, true, 10);
			Character = CharacterGenerator.Generate(true);
			Character.Position = Dungeon.GetFirstDungeonElementPosition(DungeonElementType.StartPoint);
			Dungeon.LevelData[Character.Position.Item1, Character.Position.Item2].Add(new DungeonElement(DungeonElementType.Player, -1));
			Character.Inventory.ItemList = InventoryItemGenerator.GenerateRandomItems(10);
			DrawMap();
			SetPossibleDirection();
		}

		//public void MoveCharacter(char direction)
		//{
		//	if (PossibleDirections.Contains(direction.ToString()))
		//	{
		//		(int, int) oldPosition = Character.Position;
		//		Character.Move((DungeonGenerator.Direction)direction);
		//		Dungeon.LevelData[oldPosition.Item1, oldPosition.Item2].RemoveAll(x => x.ElementType == DungeonElementType.Player);
		//		Dungeon.LevelData[Character.Position.Item1, Character.Position.Item2].Add(new DungeonElement(DungeonElementType.Player, -1));
		//		RefreshMapItems(new List<(int, int)> { oldPosition, Character.Position });
		//		SetPossibleDirection();
		//		OnPropertyChanged(nameof(MapItems));
		//	}
		//}

		public bool EnablePossibleDirection(char direction)
		{
			return PossibleDirections.Contains(direction.ToString());
		}
		#endregion View Commands

		#region methods
		private void MoveCharacter(object obj)
		{
			if (PossibleDirections.Contains(obj.ToString()))
			{
				(int, int) oldPosition = Character.Position;
				Character.Move((DungeonGenerator.Direction)obj.ToString()[0]);
				Dungeon.LevelData[oldPosition.Item1, oldPosition.Item2].RemoveAll(x => x.ElementType == DungeonElementType.Player);
				Dungeon.LevelData[Character.Position.Item1, Character.Position.Item2].Add(new DungeonElement(DungeonElementType.Player, -1));
				RefreshMapItems(new List<(int, int)> { oldPosition, Character.Position });
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

		private void RefreshMapItems(List<(int, int)> pozitionList)
		{
			foreach (var poz in pozitionList)
			{
				var newMapitem = GetMapItemByPosition(poz.Item1, poz.Item2);
				var oldMapItem = MapItems.FirstOrDefault(s => s.Row == poz.Item1 && s.Column == poz.Item2);
				if (oldMapItem == null)
				{
					oldMapItem = new MapItem();
					oldMapItem.Row = poz.Item1;
					oldMapItem.Column = poz.Item2;
				}

				oldMapItem.Image = newMapitem.Image;
			}
		}

		private MapItem GetMapItemByPosition(int row, int col)
		{
			MapItem mapItem = new MapItem();
			mapItem.Row= row;
			mapItem.Column = col;
			mapItem.Image = MergeImages(GetMapPositionTilesPathsWithFileName(Dungeon.LevelData[row, col]));

			return mapItem;
		}

		private List<string> GetMapPositionTilesPathsWithFileName(List<DungeonElement> elementsAtPosition)
		{
			List<string> positionImages = new List<string>();
			elementsAtPosition.ForEach(x => positionImages.Add(GetMapPositionTilePathWithFileName(x)));
			return positionImages;
		}

		private string GetMapPositionTilePathWithFileName(DungeonElement element)
		{
			TileHandler handler = new TileHandler();
			TileCategory tileCategory = TileCategory.Error;
			TileSubCategory tileSubCategory = TileSubCategory.Error;
			List<string> types = new List<string>();

			switch (element.ElementType)
			{
				case DungeonElementType.Wall:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Wall;
					break;
				case DungeonElementType.StartPoint:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					break;
				case DungeonElementType.EndPoint:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					break;
				case DungeonElementType.Item:
					tileCategory = TileCategory.Item;
					tileSubCategory = TileItemSubCategory.Armour;
					break;
				case DungeonElementType.Player:
					tileCategory = TileCategory.Monster;
					tileSubCategory = TileMonsterSubCategory.Humanoids;
					break;
				case DungeonElementType.Monster:
					tileCategory = TileCategory.Monster;
					tileSubCategory = TileMonsterSubCategory.Animals;
					break;
				case DungeonElementType.UpStairs:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					break;
				case DungeonElementType.DownStairs:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Gateways;
					break;
				case DungeonElementType.Way:
					tileCategory = TileCategory.Dungeon;
					tileSubCategory = TileDungeonSubCategory.Floor;
					types.Add("pedestal");
					types.Add("full");
					break;
				default:
					break;
			}

			return handler.GetTilePathWithFileName(tileCategory, tileSubCategory, types);
		}

		private BitmapImage MergeImages(List<string> imagesPathWithFileNames)
		{
			BitmapImage result = new BitmapImage();

			if (imagesPathWithFileNames.Count == 0)
				return result;

			Bitmap baseImage = new Bitmap(32, 32);
			
			using (var g = Graphics.FromImage(baseImage))
			{
				foreach (var image in imagesPathWithFileNames)
				{
					g.DrawImage(System.Drawing.Image.FromFile(image), 0, 0);
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