using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;

namespace GameUI
{
	// ContainerConsole håller alla consoler som används och gör det
	// enkelt att komma åt från ett ställe
	public class UIManager : ContainerConsole
	{
		// skapa / förstöra sadconsoles
		// vilken console används
		// håller relationen mellan consoles children och parent
		// map-scrolling
		// skapa och hålla reda på kontroller för ui-element
		// hanterar inputs från tangentbord/mus(?) och ui
		
		public ScrollingConsole MapConsole;
		public Window MapWindow;

		public UIManager()
		{
			// måste vara true för att kunna calla draw-metoder
			IsVisible = true;
			// måste vara true för att läsa av inputs
			IsFocused = true;

			// UIManager är den enda consolen som SadConsole bryr sig om
			Parent = Global.CurrentScreen;
		}

		// inita alla windows och consoler
		public void Init()
		{
			CreateConsoles();
			CreateMapWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "Game Map");
		}

		// gör ett window som håller en map-console och visar
		// en title i mitten
		// viktigt att den läggs till som child av UIManager så
		// den uppdateras och draws
		public void CreateMapWindow(int width, int height, string title)
		{
			MapWindow = new Window(width, height);
			MapWindow.CanDrag = true;

			// gör det kort nog för att dra titel och borders
			// flytta iväg från borders
			var mapConsoleWidth = width - 2;
			var mapConsoleHeight = height - 2;

			// resizea viewporten så den får plats i windows borders
			MapConsole.ViewPort =
				new Rectangle(0, 0, mapConsoleWidth, mapConsoleHeight);

			// flytta MapConsole så den inte överlappar
			MapConsole.Position = new Point(1, 1);

			// "close window"-knapp
			var closeButton = new Button(3);
			closeButton.Position = new Point(0, 0);
			closeButton.Text = "[x]";

			// lägg till button som component till MapWindows ui-element
			MapWindow.Add(closeButton);

			// centrera titeltexten
			MapWindow.Title =
				title.Align(HorizontalAlignment.Center, mapConsoleWidth);


			// VIKTIGT
			// lägg till player som något som ska renderas
			MapConsole.Children.Add(GameLoop.World.Player);
			// lägg till mapviewer till window
			MapWindow.Children.Add(MapConsole);
			// gör den en child till UIManager
			Children.Add(MapWindow);
			// om den inte sätts som show så visas den aldrig
			MapWindow.Show();
		}

		// skapar alla child-consoles som vi hanterar
		public void CreateConsoles()
		{
			MapConsole = new ScrollingConsole(
				GameLoop.World.CurrentMap.Width, 
				GameLoop.World.CurrentMap.Height, 
				Global.FontDefault, 
				new Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), 
				GameLoop.World.CurrentMap.Tiles);

		}

		// centrerar viewporten på actor
		public void CenterOnActor(Actor actor)
		{
			MapConsole.CenterViewPortOnPoint(actor.Position);
		}

		// hanterar "time.deltatime" och lägger till vår egna checkkeyboard
		// är fixedupdate
		public override void Update(TimeSpan timeElapsed)
		{
			CheckKeyboard();
			base.Update(timeElapsed);
		}

		// skanna sadconsoles keyboardstate och gör shit beroende på knapp
		private void CheckKeyboard()
		{
			// lättare att skapa en teleport än att faktiskt se till att man spawnar i 
			// ett rum
			if (Global.KeyboardState.IsKeyPressed(Keys.Space))
			{
				var random = new Random();
				GameLoop.World.Player.MoveTo(new Point(
					random.Next(GameLoop.GameWidth - 1)
					, random.Next(GameLoop.GameHeight - 1)));
				CenterOnActor(GameLoop.World.Player);
			}

			if (Global.KeyboardState.IsKeyPressed(Keys.Up))
			{
				GameLoop.World.Player.MoveBy(new Point(0, -1));
				CenterOnActor(GameLoop.World.Player);
			}

			if (Global.KeyboardState.IsKeyPressed(Keys.Down))
			{
				GameLoop.World.Player.MoveBy(new Point(0, 1));
				CenterOnActor(GameLoop.World.Player);
			}

			if (Global.KeyboardState.IsKeyPressed(Keys.Left))
			{
				GameLoop.World.Player.MoveBy(new Point(-1, 0));
				CenterOnActor(GameLoop.World.Player);
			}

			if (Global.KeyboardState.IsKeyPressed(Keys.Right))
			{
				GameLoop.World.Player.MoveBy(new Point(1, 0));
				CenterOnActor(GameLoop.World.Player);
			}
		}
	}
}