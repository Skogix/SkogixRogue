using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;

namespace GameUI
{
	public class World
	{
		// faktiskt mapstorleken
		private static readonly int _mapWidth = 100;
		private static readonly int _mapHeight = 100;

		// mapgeneration
		private static readonly int _maxRooms = 100;
		private static readonly int _minRoomSize = 4;
		private static readonly int _maxRoomSize = 15;
		private TileBase[] _mapTiles;

		public World()
		{
			CreateMap();
			CreatePlayer();
		}

		// playerdata
		public Player Player { get; set; }

		// mapdata
		public Map CurrentMap { get; set; }

		private void CreateMap()
		{
			_mapTiles = new TileBase[_mapWidth * _mapHeight];
			CurrentMap = new Map(_mapWidth, _mapHeight);
			var mapGen = new MapGenerator();
			CurrentMap = mapGen.GenerateMap(
				_mapWidth,
				_mapHeight,
				_maxRooms,
				_minRoomSize,
				_maxRoomSize);
		}

		// sadconsole har entities med point och andra values (tänk transform i unity)
		// ToDo: Kolla upp entities och hur lika dem är unity
		private void CreatePlayer()
		{
			Player = new Player(Color.HotPink, Color.Transparent);
			
			// sätt player på första tilen som inte är blockad
			for (int i = 0; i < CurrentMap.Tiles.Length; i++)
			{
				if (CurrentMap.Tiles[i].IsBlockingMove == false)
				{
					// sätt players position på current pos
					Player.Position = Helpers.GetPointFromIndex(i, CurrentMap.Width);
				}
			}

			// adda viewport sync-component till player
			Player.Components.Add(new EntityViewSyncComponent());
		}
	}
}