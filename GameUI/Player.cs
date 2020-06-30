using Microsoft.Xna.Framework;

namespace GameUI
{
	public class Player : Actor
	{
		// skapar spelaren
		public Player(Color foreground, Color background) : base(foreground
			, background, '@')
		{
		}
	}
}