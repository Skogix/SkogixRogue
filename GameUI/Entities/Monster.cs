using Microsoft.Xna.Framework;

namespace GameUI.Entities
{
	public class Monster : Actor
	{
		public Monster(Color foreground, Color background, int glyph, int width = 1, int height = 1) : base(foreground, background, glyph, width, height)
		{
		}

		public Monster(Color foreground, Color background) : base(foreground, background, 'M')
		{
		}
	}
}