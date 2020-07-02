using Microsoft.Xna.Framework;

namespace GameUI.Map.Tiles
{
	public class TileWall : TileBase
	{
		public TileWall(
			bool blocksMovement = true,
			bool blocksLoS = true)
			: base(
				Color.LightGray,
				Color.Transparent,
				'#',
				blocksMovement,
				blocksLoS)
		{
			Name = "Wall";
		}
	}
}