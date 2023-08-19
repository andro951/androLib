using androLib.Common.Utility;
using androLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace androLib
{
	public class StoragePlayer : ModPlayer {
		public static StoragePlayer LocalStoragePlayer => Main.LocalPlayer.GetModPlayer<StoragePlayer>();
		public bool disableLeftShiftTrashCan = ItemSlot.Options.DisableLeftShiftTrashCan;

		public override void SaveData(TagCompound tag) {
			StorageManager.SaveData(tag);
		}
		public override void LoadData(TagCompound tag) {
			StorageManager.LoadData(tag);
		}

		public override bool ShiftClickSlot(Item[] inventory, int context, int slot) {
			ref Item item = ref inventory[slot];
			if (MasterUIManager.NoUIBeingHovered) {
				bool openAndCouldStore = false;
				foreach (BagUI bagUI in StorageManager.BagUIs) {
					if (bagUI.DisplayBagUI && bagUI.CanBeStored(item)) {
						openAndCouldStore = true;
						if (bagUI.TryVacuumItem(ref item, Main.LocalPlayer))
							return true;
					}
				}

				if (openAndCouldStore) {
					MasterUIManager.SwapMouseItem(ref item);
					return true;
				}
			}

			return false;
		}
	}
}
