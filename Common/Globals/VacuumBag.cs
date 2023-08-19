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
		public override bool CanRightClick(Item item) => AppliesTo(item) && !ItemSlot.ShiftInUse;
		public override void RightClick(Item item, Player player) {
			if (!AppliesTo(item))
				return;

			item.stack++;
			UseBag(item, player);
		}
		public override bool? UseItem(Item item, Player player) {
			if (!AppliesTo(item))
				return null;

			if (Main.myPlayer == player.whoAmI && Main.netMode != NetmodeID.Server)
				UseBag(item, player);

			return null;
		}
		private void UseBag(Item item, Player player) {
			int bagStroageID = StorageManager.StorageItemTypes[item.type];
			if (!StorageManager.TryGetBagUI(bagStroageID, out BagUI bagUI))
				return;

			if (StorageManager.BagUIs[bagStroageID].DisplayBagUI && Main.LocalPlayer.chest == -1) {
				bagUI.CloseBag(true);
			}
			else {
				bagUI.OpenBag();
			}
		}
	}
}
