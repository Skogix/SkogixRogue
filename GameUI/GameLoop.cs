using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Console = SadConsole.Console;
using SadConsole;

namespace GameUI
{
	internal class GameLoop
	{
		// för viewPort / "camera-fov"
		private const int Width = 80;
		private const int Height = 40;
		
		private static Player player;
		public static Map GameMap;

		// faktiskt mapstorleken
		private static int _mapWidth = 100;
		private static int _mapHeight = 100;
		// mapgeneration
		private static int _maxRooms = 1000;
		private static int _minRoomSize = 15;
		private static int _maxRoomSize = 25;
		
		
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
			// t.ex keypresses, toggles, viss logik(?) osv
			CheckKeyboard();

		}

		private static void Init()
		{
			// sätt en ny empty map
			GameMap = new Map(_mapWidth, _mapHeight);
			
			// newa mapGenerator och kör
			MapGenerator mapGen = new MapGenerator();
			GameMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms
				, _minRoomSize, _maxRoomSize);
			
			// skapa en console som använder tiles från gameMap
			Console startingConsole = new ScrollingConsole(
				GameMap.Width, 
				GameMap.Height, 
				Global.FontDefault, 
				new Rectangle(0,0,Width,Height), // viewPorten
				GameMap.Tiles);

			// sätt startingConsole som currentScreen
			SadConsole.Global.CurrentScreen = startingConsole;
			
			// skapa spelare
			CreatePlayer();
			// lägg till spelare some child av nuvarande consolen
			// VIKTIGT!
			startingConsole.Children.Add(player);
		}

		// sadconsole har entities med point och andra values (tänk transform i unity)
		// ToDo: Kolla upp entities och hur lika dem är unity
		private static void CreatePlayer()
		{
			player = new Player(Color.HotPink, Color.Transparent);
			player.Position = new Point(5, 5);
		}

		// skanna sadconsoles keyboardstate och gör shit beroende på knapp
		private static void CheckKeyboard()
		{
			if (Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework
				.Input.Keys.Up))
				player.MoveBy(new Point(0, -1));
			if (Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
				player.MoveBy(new Point(0, 1));
			if (Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
				player.MoveBy(new Point(-1, 0));
			if (Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
				player.MoveBy(new Point(1, 0));
		}
	}
}