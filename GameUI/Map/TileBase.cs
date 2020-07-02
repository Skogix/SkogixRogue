using Microsoft.Xna.Framework;
using SadConsole;

namespace GameUI.Map
{
	// abstract base class, inga egna object
	public abstract class TileBase : Cell
	{
		public bool IsBlockingLoS;

		// movement och LoS
		public bool IsBlockingMove;

		protected string Name;

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