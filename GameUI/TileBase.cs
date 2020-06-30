using System.Drawing;
using SadConsole;
using Color = Microsoft.Xna.Framework.Color;

namespace GameUI
{
	// abstract base class, inga egna object
	public abstract class TileBase : Cell
	{
		
		protected string Name;
		// movement och LoS
		public bool IsBlockingMove;
		public bool IsBlockingLoS;
		
		// varje tilebase har en foreground/background-color och en glyph/token
		
		// verkar som cells inte har en point utan sadconsole
		// har koll på cells position, inte tvärtom
		public TileBase(
			Color foreground, 
			Color background, 
			int glyph, 
			bool blockingMove = false,
			bool blockingLoS = false,
			string name = "") : base(foreground, background, glyph)
		{
			IsBlockingMove = blockingMove;
			IsBlockingLoS = blockingLoS;
			Name = name;
		}
	}
}