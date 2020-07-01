using Microsoft.Xna.Framework;
using Game = SadConsole.Game;

namespace GameUI
{
	internal class GameLoop
	{
		public const int GameWidth = 80;

		public const int GameHeight = 25;

		// managers
		public static UIManager UIManager;
		public static World World;

		private static void Main()
		{
			// skapa mainwindow med engine
			Game.Create(GameWidth, GameHeight);

			// hooka starteventet så vi kan lägga till consoler till systemet
			Game.OnInitialize = Init;

			// hooka frameupdate till en egen logikupdate
			Game.OnUpdate = Update;

			// gogogo
			Game.Instance.Run();

			// här körs först efter terminalen stängs
			Game.Instance.Dispose();
		}

		private static void Update(GameTime time)
		{
		}

		private static void Init()
		{
			UIManager = new UIManager();
			World = new World();

			// skapa consolerna vi behöver
			UIManager.Init();
		}
	}
}