using System.Drawing;
using SadConsole.Components;
using Color = Microsoft.Xna.Framework.Color;

namespace GameUI.Entities
{
	// lägg till ID från GoRogue till alla entities från sadconsole
	public class Entity : SadConsole.Entities.Entity, GoRogue.IHasID
	{
		// unikt entity-id som GoRogue använder
		public uint ID { get; private set; }

		protected Entity(Color foreground, Color background, int glyph, int width = 1, int height = 1) : base(width, height)
		{
			Animation.CurrentFrame[0].Foreground = foreground;
			Animation.CurrentFrame[0].Background = background;
			Animation.CurrentFrame[0].Glyph = glyph;
			
			// skapa nytt UNIKT id
			ID = Map.Map.IDGenerator.UseID();
			// adda alltid component viewsync så scrollingconsole alltid trackar
			Components.Add(new EntityViewSyncComponent());
		}
	}
}