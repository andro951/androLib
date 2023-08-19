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
		public List<Storage> Storages {
			get {
				if (storages == null)
					StorageManager.PopulateStorages(ref storages);

				return storages;
			}
		}
		private List<Storage> storages = null;
		public override void SaveData(TagCompound tag) {
			string name = Player.name;
			for (int i = 0; i < Storages.Count; i++) {
				Storages[i].SaveData(tag);
			}
		}
		public override void LoadData(TagCompound tag) {
			string name = Player.name;//, PlayerStorageManager);

			for (int i = 0; i < Storages.Count; i++) {
				Storages[i].LoadData(tag);
			}
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
	public static class StoragePlayerFunctions {
		public static Item[] GetChestItems(this Player player, int chest = int.MinValue) {
			if (chest == int.MinValue)
				chest = player.chest;

			switch (chest) {
				case > -1:
					return Main.chest[chest].item;
				case -2:
					return player.bank.item;
				case -3:
					return player.bank2.item;
				case -4:
					return player.bank3.item;
				case -5:
					return player.bank4.item;
				default:
					return new Item[0];
			}
		}
		public static bool ItemWillBeTrashedFromShiftClick(this Player player, Item item) {
			int stack = item.stack;
			for (int i = 49; i >= 0; i--) {
				//Any open invenotry space or a stack of the same item in the inventory can hold the 
				Item inventoryItem = player.inventory[i];
				if (inventoryItem.IsAir) {
					return false;
				}
				else if (inventoryItem.type == item.type) {
					int availableStack = Math.Max(inventoryItem.maxStack - inventoryItem.stack, 0);
					stack -= availableStack;
					if (stack < 1)
						return false;
				}
			}

			return true;
		}
	}
}
