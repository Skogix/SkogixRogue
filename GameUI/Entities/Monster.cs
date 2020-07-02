using System;
using Microsoft.Xna.Framework;

namespace GameUI.Entities
{
	public class Monster : Actor
	{
		public Monster(Color foreground, Color background) : base(foreground, background, 'M')
		{
			Random random = new Random();
			int goldLootNum = random.Next(1, 5);

			for (int i = 0; i < goldLootNum; i++)
			{
				Item newLoot = new Item(Color.HotPink, Color.Transparent, "lewt", 'L', 2);
				newLoot.Components.Add(new SadConsole.Components.EntityViewSyncComponent());
				Inventory.Add(newLoot);
			}
		}
		
		
	}
}