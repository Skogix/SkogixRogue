using System.Buffers.Text;
using System.Drawing;
using GameUI.Entities;
using Color = Microsoft.Xna.Framework.Color;

namespace GameUI
{
	public class Item : Entity
	{
		// backing field
		private int _condition;
		
		// vikt/massa/tyngd
		public int Weight { get; set; }
		
		// condition / procent av hälsa i procent
		// om <= 0 destroy
		public int Condition
		{
			get => _condition;
			set
			{
				_condition += value;
				if (_condition <= 0) Destroy();
			}
		}
		
		// default så är ett item 1x1, väger 1 och i 100% condition
		public Item(
			Color foreground,
			Color background,
			string name,
			char glyph,
			int weight = 1,
			int condition = 100,
			int width = 1,
			int height = 1)
			: base(foreground, background, glyph)
		{
			Animation.CurrentFrame[0].Foreground = foreground;
			Animation.CurrentFrame[0].Background = background;
			Animation.CurrentFrame[0].Glyph = glyph;
			Weight = weight;
			Condition = condition;
			Name = name;
		}
		
		// destroy genom att removea från MultiSpatialMaps lista med entitys
		// garbagecollectorn tar bort den från minnet när inga aktiva pekare
		// finns kvar / inte är i scope
		public void Destroy()
		{
			GameLoop.World.CurrentMap.Remove(this);
		}
	}
}