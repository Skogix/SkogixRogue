using Microsoft.Xna.Framework;

namespace GameUI.Entities
{
	public abstract class Actor : Entity
	{
		protected Actor(
			Color foreground,
			Color background,
			int glyph,
			int width = 1,
			int height = 1)
			: base(foreground,
				background,
				glyph,
				width,
				height)
		{
			// sadconsole har animationer så ändra bara första framen i animationen
			Animation.CurrentFrame[0].Foreground = foreground;
			Animation.CurrentFrame[0].Background = background;
			Animation.CurrentFrame[0].Glyph = glyph;
		}

		// exempelattribut
		public int Health { get; set; }
		public int MaxHealth { get; set; }
		public int Attack { get; set; }
		public int AttackChance { get; set; }
		public int Defense { get; set; }
		public int DefenseChance { get; set; }
		public int Gold { get; set; }
		

		// flyttar actor MED positionChange i x/y-dir
		// returnar true om ok, annars false
		public bool MoveBy(Point posChange)
		{
			// kolla mappen om vi kan gå
			if (GameLoop.World.CurrentMap.IsTileWalkable(Position + posChange))
			{
				// om det står ett monster där, FIGHT!
				Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + posChange);
				if (monster != null)
				{
					GameLoop.CommandManager.Attack(this, monster);
					return true;
				}
				Position += posChange;
				return true;
			}

			return false;
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