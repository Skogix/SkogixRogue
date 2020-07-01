using Microsoft.Xna.Framework;

namespace GameUI
{
	public class TileFloor : TileBase
	{
		public TileFloor(
			bool blocksMovement = false,
			bool blocksLoS = false)
			: base(
				Color.DarkGray,
				Color.Transparent,
				'.',
				blocksMovement,
				blocksLoS)
		{
			Name = "Floor";
		}
	}
}