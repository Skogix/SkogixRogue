using Microsoft.Xna.Framework;
using SadConsole;

namespace GameUI
{
	public abstract class Actor : SadConsole.Entities.Entity
	{
		// exempelattribut
		public int Health { get; set; }
		public int MaxHealth { get; set; }

		protected Actor(
			Color foreground,
			Color background,
			int glyph,
			int width = 1,
			int height = 1)
			: base(width, height)
		{
			// sadconsole har animationer så ändra bara första framen i animationen
			Animation.CurrentFrame[0].Foreground = foreground;
			Animation.CurrentFrame[0].Background = background;
			Animation.CurrentFrame[0].Glyph = glyph;
		}
		
		// flyttar actor MED positionChange i x/y-dir
		// returnar true om ok, annars false
		public bool MoveBy(Point posChange)
		{
			// kolla mappen om vi kan gå
			if (GameLoop.GameMap.IsTileWalkable(Position + posChange))
			{
				Position += posChange;
				return true;
			}
			else
			{
				return false;
			}
		}
		
		// flyttar actor TILL newPos loc
		// returnar true om ok, annars false
		public bool MoveTo(Point newPos)
		{
			Position = newPos;
			return true;
		}
		
	}
}