using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Console = SadConsole.Console;
using SadConsole;

namespace GameUI
{
	internal class GameLoop
	{
		private const int Width = 80;
		private const int Height = 25;
		private static SadConsole.Entities.Entity player;
		
		// array med tilebases som har alla tiles
		private static TileBase[] _tiles;
		private const int _testRoomWidth = 10;
		private const int _testRoomHeight = 20;
		
		
		private static void Main()
		{
			// skapa mainwindow med engine
			SadConsole.Game.Create(Width, Height);
			
			// hooka starteventet så vi kan lägga till consoler till systemet
			SadConsole.Game.OnInitialize = Init;
			
			// hooka frameupdate till en egen logikupdate
			// ToDo: kolla om det finns någon anledning att separera dem i ett konsollspel?
			SadConsole.Game.OnUpdate = Update;
			
			// gogogo
			SadConsole.Game.Instance.Run();
			
			// här körs först efter terminalen stängs
			SadConsole.Game.Instance.Dispose();
		}

		private static void Update(GameTime time)
		{
			// körs varje LOGISK update men hookat med frameupdate
			// t.ex keypresses, toggles osv
			// blir nog att calla logik här med

			if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
				player.Position += new Point(0, -1);
			if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
				player.Position += new Point(0, 1);
			if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
				player.Position += new Point(-1, 0);
			if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
				player.Position += new Point(1, 0);
		}

		private static void Init()
		{
			// loada och preppa saker
			
			// bygg väggar i hela rummet och sen lägg ut golv
			CreateWalls();
			CreateFloors();
			
			// scrollingconsole använder viewports och en "kamera" om följer något
			Console startingConsole = new ScrollingConsole(
				Width,
				Height,
				Global.FontDefault,
				// använder viewPort (camera i unity)
				new Rectangle(0, 0, Width, Height),
				_tiles);
			
			// sätt den här konsollen till den som ska köras
			SadConsole.Global.CurrentScreen = startingConsole;
			// instansiera och adda till currentconsole
			CreatePlayer();
			startingConsole.Children.Add(player);
			
		}

		// sadconsole har entities med point och andra values (tänk transform i unity)
		// ToDo: Kolla upp entities och hur lika dem är unity
		private static void CreatePlayer()
		{
			player = new SadConsole.Entities.Entity(1, 1);
			player.Animation.CurrentFrame[0].Glyph = '@';
			player.Animation.CurrentFrame[0].Foreground = Color.HotPink;
			player.Position = new Point(20, 10);
		}

		private static void CreateFloors()
		{
			// kötta ut en rektangel med floors
			for (int x = 0; x < _testRoomWidth; x++)
			{
				for (int y = 0; y < _testRoomHeight; y++)
				{
					// skapa en ny tile för varje position i 2dmatrix
					// för att få ut en matrix från en endimensionell array med Width:
					// y * Width + x
					_tiles[y * Width + x] = new TileFloor(); 
				}
			}
		}

		private static void CreateWalls()
		{
			// array stor som mapsize
			_tiles = new TileBase[Width * Height];
			
			// fyll med walls
			// lär som "bas", vill inte ha nå null-skit från sadconsole
			for (int i = 0; i < _tiles.Length; i++)
			{
				_tiles[i] = new TileWall();
			}
		}
	}
}