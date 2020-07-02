using System.Linq;
using GameUI.Entities;
using GameUI.Map.Tiles;
using Microsoft.Xna.Framework;
using SadConsole;

namespace GameUI.Map
{
	// sparar och ändrar på tiledata
	public class Map
	{
		// bygg mappen
		public Map(int width, int height)
		{
			Width = width;
			Height = height;
			Tiles = new TileBase[width * height];
			Entities = new GoRogue.MultiSpatialMap<Entity>();
		}

		// har alla tiles
		public TileBase[] Tiles { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }
		
		// GoRogue
		// håller reda på alla entities på mappen
		public GoRogue.MultiSpatialMap<Entity> Entities;
		// en static id-generator som varje entity körs emot
		public static GoRogue.IDGenerator IDGenerator = new GoRogue.IDGenerator();


		// kollar om actor försöker gå till en validtile
		// returnar false om det är utanför mappen eller isblockingmove
		public bool IsTileWalkable(Point location)
		{
			// inte gå utanför mappen
			if (location.X < 0 || location.Y < 0 || location.X >= Width ||
					location.Y >= Height) return false;
			// sen returna true om den inte är blockad
			return Tiles[location.Y * Width + location.X].IsBlockingMove == false;
		}
		
		// kolla om en typ av entity finns på en location
		public T GetEntityAt<T>(Point location) where T : Entity
		{
			// om det existerar, returna T annars false
			return Entities.GetItems(location).OfType<T>().FirstOrDefault();
		}
		
		// ta bort en entity från "MultiSpatialMap"
		public void Remove(Entity entity)
		{
			// ta bort
			Entities.Remove(entity);
			
			// länka av entityns move-event till en handler
			entity.Moved -= OnEntityMoved;
		}
		
		// lägg till en entity till "MultiSpatialMap"
		public void Add(Entity entity)
		{
			// lägg till
			Entities.Add(entity, entity.Position);
			
			// länka på entityns move-event till handlern
			entity.Moved += OnEntityMoved;
		}
		
		// när en entitys .Moved-value ändras, trigga den här eventhandlern
		// vilket uppdaterar currentPos i SpatialMap
		private void OnEntityMoved(object sender, SadConsole.Entities.Entity.EntityMovedEventArgs args)
		{
			Entities.Move(args.Entity as Entity, args.Entity.Position);
		}
		
		// kolla om en tile är på en viss position, och om den existerar så returna den
		// accepterar enkla x/y-koordinater
		public T GetTileAt<T>(int x, int y) where T : TileBase
		{
			int locationIndex = Helpers.GetIndexFromPoint(x, y, Width);
			
			// se till att index är innanför mappen
			if (locationIndex <= Width * Height && locationIndex >= 0)
			{
				if (Tiles[locationIndex] is T)
					return (T) Tiles[locationIndex];
				else return null;
			}
			else return null;
		}
	}
}