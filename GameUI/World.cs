using System;
using GameUI.Entities;
using GameUI.Map;
using GameUI.Map.Tiles;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using SadConsole.Components;

namespace GameUI
{
	public class World
	{
		Random random = new Random();
		
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
			CreateMonsters();
			CreateLoot();
		}

		// playerdata
		public Player Player { get; set; }

		// mapdata
		public Map.Map CurrentMap { get; set; }

		private void CreateMap()
		{
			_mapTiles = new TileBase[_mapWidth * _mapHeight];
			CurrentMap = new Map.Map(_mapWidth, _mapHeight);
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
			CurrentMap.Add(Player);
		}

		// gör lite random monsters med random values
		// och sätt ut dem på random ställen
		private void CreateMonsters()
		{
			int numMonsters = 10;
			
			// skapa monster och plocka en random position
			// om position är blockad (typ vägg) så testa igen
			for (int i = 0; i < numMonsters; i++)
			{
				int monsterPosition = 0;
				Monster newMonster = new Monster(Color.Blue, Color.Transparent);
				while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
				{
					// plocka en random spot
					monsterPosition = random.Next(0, CurrentMap.Width * CurrentMap.Height);
				}
				
				// sätt lite randomvalues
				newMonster.Defense = random.Next(0, 10);
				newMonster.DefenseChance = random.Next(0, 50);
				newMonster.Attack = random.Next(0, 10);
				newMonster.AttackChance = random.Next(0, 50);
				newMonster.Name = "Jagger";
				
				// sätt position
				newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
				CurrentMap.Add(newMonster);

			}

		}
		
		// skapa lite randomloot för testing
		private void CreateLoot()
		{
			int numLoot = 20;

			for (int i = 0; i < numLoot; i++)
			{
				int lootPosition = 0;
				Item newLoot = new Item(Color.HotPink, Color.Transparent, "Random loot", 'L', 2, 70);
				
				// lägg till komponent så position osv syncas till mappen
				
				// försök skapa på lootpos, om fail försök tills det går
				while (CurrentMap.Tiles[lootPosition].IsBlockingMove)
				{
					// random place på mappen
					lootPosition = random.Next(0, CurrentMap.Width * CurrentMap.Height);
				}
				
				// sätt positionen
				newLoot.Position = new Point(lootPosition % CurrentMap.Width, lootPosition / CurrentMap.Width);
				
				// lägg till i MultipSpatialMap
				CurrentMap.Add(newLoot);
			}
		}
	}
}