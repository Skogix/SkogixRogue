using TinkerWorX.AccidentalNoiseLibrary;
using Microsoft.Xna.Framework;

namespace GameUI.Map.Tiles
{
	// kan vara locked, opened elle closed
	public class TileDoor : TileBase
	{
		// ToDo: Kolla om det är värt att använda enums, kan bli messy annars
		public bool Locked; // låst = 1, upplåst = 0
		public bool IsOpen; // öppen = 1, stängd = 0

		public TileDoor(bool locked, bool open) : base(Color.Gray, Color.Transparent, '+')
		{
			// + är closed
			Glyph = '+';

			Locked = locked;
			IsOpen = open;
			
			// ändra symbol om dörren är öppen
			if (!Locked && IsOpen)
				Open();
			else if (Locked || !IsOpen)
				Close();
		}
		
		// stänger en dörr
		public void Close()
		{
			IsOpen = false;
			Glyph = '+';
			IsBlockingLoS = true;
			IsBlockingMove = true;
		}
		
		// öppnar en dörr
		public void Open()
		{
			IsOpen = true;
			Glyph = '-';
			IsBlockingLoS = false;
			IsBlockingMove = false;
		}
	}
}