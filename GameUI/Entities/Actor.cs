using System.Collections.Generic;
using GameUI.Map.Tiles;
using GoRogue;
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
		
		public List<Item> Inventory = new List<Item>();
		

		// flyttar actor MED positionChange i x/y-dir
		// returnar true om ok, annars false
		public bool MoveBy(Point positionChange)
		{
			// kolla mappen om vi kan gå
			if (GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange))
			{
				// ToDo: Flytta all gamelogic från moveby, refactor plz
				Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + positionChange);
				Item item = GameLoop.World.CurrentMap.GetEntityAt<Item>(Position + positionChange);
				
				
				// om det står ett monster där, FIGHT!
				if (monster != null)
				{
					GameLoop.CommandManager.Attack(this, monster);
					return true;
				}
				
				// om det är ett item där, plocka upp
				else if (item != null)
				{
					GameLoop.CommandManager.Pickup(this, item);
					return true;
				}
				
				Position += positionChange;
				return true;
			}
			// situationer där vi har tiles som kan bli useade men inte walkade
			else
			{
				// kolla om det finns en dörr och i så fall försök usea
				TileDoor door = GameLoop.World.CurrentMap.GetTileAt<TileDoor>(Position + positionChange);
				if (door != null)
				{
					GameLoop.CommandManager.UseDoor(this, door);
					// öppna som ett move eller öppna OCH gå dit?
					Position += positionChange;
					return true;
				}

				return false;
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