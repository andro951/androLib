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
		public override void Load() {
			AndroMod.PostRightClickActions += PostRightClick;
		}
		private bool AppliesTo(Item item) => StorageManager.StorageItemTypes.ContainsKey(item.type);
		public override bool CanRightClick(Item item) => AppliesTo(item);
		public override void RightClick(Item item, Player player) {
			if (!AppliesTo(item))
				return;

			justRightClicked = true;
			UseBag(item, player);
		}
		private static bool justRightClicked = false;
		private void PostRightClick(Item item, Player player) {
			justRightClicked = false;
		}
		public override bool ConsumeItem(Item item, Player player) {
			return !AppliesTo(item) || !justRightClicked;
		}
		private static void UseBag(Item item, Player player) => UseBag(StorageManager.StorageItemTypes[item.type]);

		public static void UseBag(int bagStorageID) {
			if (!StorageManager.TryGetBagUI(bagStorageID, out BagUI bagUI))
				return;

			if (StorageManager.BagUIs[bagStorageID].DisplayBagUI) {
				if (Main.playerInventory) {
					bagUI.CloseBag(true);
				}
				else {
					Main.playerInventory = true;
				}
			}
			else {
				bagUI.OpenBag();
			}
		}
	}
}
