using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Console = SadConsole.Console;
using SadConsole;

namespace GameUI
{
	internal class Program
	{
		private const int Width = 80;
		private const int Height = 25;
		private static SadConsole.Entities.Entity player;
		
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
		}

		private static void Init()
		{
			// loada och preppa saker
			
			// testfest
			// console är nu "using" sadconsole.console
			Console startingConsole = new Console(Width, Height);
			
			// startingConsole.FillWithRandomGarbage();
			startingConsole.Fill(
				new Rectangle(3, 3, 27, 5),
				null,
				Color.Black,
				0,
				SpriteEffects.None);
			startingConsole.Print(5, 5, "Skogix!", ColorAnsi.CyanBright);

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
	}
}