using androLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace androLib.Common.Globals
{
	public class VacuumBag : GlobalItem
	{
		private bool AppliesTo(Item item) => StorageManager.StorageItemTypes.ContainsKey(item.type);
		public override bool CanRightClick(Item item) => AppliesTo(item);
		public override void RightClick(Item item, Player player) {
			if (!AppliesTo(item))
				return;

			item.stack++;
			UseBag(item, player);
		}
		private static void UseBag(Item item, Player player) => UseBag(StorageManager.StorageItemTypes[item.type]);

		public static void UseBag(int bagStorageID) {
			if (!StorageManager.TryGetBagUI(bagStorageID, out BagUI bagUI))
				return;

			if (StorageManager.BagUIs[bagStorageID].DisplayBagUI && Main.LocalPlayer.chest == -1) {
				bagUI.CloseBag(true);
			}
			else {
				bagUI.OpenBag();
			}
		}
	}
}
