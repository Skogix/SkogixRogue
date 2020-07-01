using Microsoft.Xna.Framework;

namespace GameUI.Entities
{
	public class Player : Actor
	{
		// skapar spelaren
		public Player(Color foreground, Color background) : base(foreground, background, '@')
		{
			Attack = 10;
			AttackChance = 50;
			Defense = 5;
			DefenseChance = 25;
			Name = "Skogix";
		}
	}
}