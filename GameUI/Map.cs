using Microsoft.Xna.Framework;

namespace GameUI
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
		}

		// har alla tiles
		public TileBase[] Tiles { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }


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
	}
}