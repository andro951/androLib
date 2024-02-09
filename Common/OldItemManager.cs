using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using androLib.Common.Utility;
using androLib;

namespace androLib.Common.OldItemManager
{
    public class OldItemManager
    {
        private enum OldItemContext {
            wholeNameReplaceWithItemByName,
            wholeNameReplaceWithItemByType,
            wholeNameReplaceWithCoins
		}
		public static void ReplaceAllChestIOldtems(Dictionary<string, string> wholeNameReplaceWithItemByName, Dictionary<string, int> wholeNameReplaceWithItemByType, Dictionary<string, int> wholeNameReplaceWithCoins) {
			foreach (Chest chest in Main.chest) {
				if (chest != null)
					ReplaceOldItems(chest.item, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins);
			}
		}
		public static void ReplaceAllOldItems(Dictionary<string, string> wholeNameReplaceWithItemByName, Dictionary<string, int> wholeNameReplaceWithItemByType, Dictionary<string, int> wholeNameReplaceWithCoins, Player player) {
			ReplaceOldItems(player.armor, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);

            int modSlotCount = player.GetModPlayer<ModAccessorySlotPlayer>().SlotCount;
            var loader = LoaderManager.Get<AccessorySlotLoader>();
            for (int num = 0; num < modSlotCount; num++) {
                if (loader.ModdedIsItemSlotUnlockedAndUsable(num, player)) {
                    Item accessoryClone = loader.Get(num).FunctionalItem.Clone();
                    if (!accessoryClone.NullOrAir()) {
                        ReplaceOldItem(ref accessoryClone, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
                        loader.Get(num).FunctionalItem = accessoryClone;
				    }

                    Item vanityClone = loader.Get(num).VanityItem.Clone();
                    if (!vanityClone.NullOrAir()) {
                        ReplaceOldItem(ref vanityClone, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
                        loader.Get(num).VanityItem = vanityClone;
				    }
                }
            }

            ReplaceOldItems(player.inventory, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
            ReplaceOldItems(player.bank.item, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
            ReplaceOldItems(player.bank2.item, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
            ReplaceOldItems(player.bank3.item, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
            ReplaceOldItems(player.bank4.item, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);

            if (player.TryGetModPlayer(out StoragePlayer storagePlayer)) {
                foreach (Storage storage in storagePlayer.Storages) {
                    if (storage.GetItems != null)
                        continue;

					ReplaceOldItems(storage.Items, wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
				}

				//ReplaceOldItems(wePlayer.enchantingTableEssence, player);
				//ReplaceOldItems(wePlayer.enchantmentStorageItems, player);
			}
		}
		private static void ReplaceOldItems(Item[] inventory, Dictionary<string, string> wholeNameReplaceWithItemByName, Dictionary<string, int> wholeNameReplaceWithItemByType, Dictionary<string, int> wholeNameReplaceWithCoins, Player player = null) {
            if (inventory == null)
                return;

			for (int i = 0; i < inventory.Length; i++) {
                 ReplaceOldItem(ref inventory[i], wholeNameReplaceWithItemByName, wholeNameReplaceWithItemByType, wholeNameReplaceWithCoins, player);
            }
		}
		public static void ReplaceOldItem(ref Item item, Dictionary<string, string> wholeNameReplaceWithItemByName, Dictionary<string, int> wholeNameReplaceWithItemByType, Dictionary<string, int> wholeNameReplaceWithCoins, Player player = null) {
            if (item.NullOrAir())
                return;

            if (item.ModItem is not UnloadedItem unloadedItem)
                return;

			string unloadedItemName = unloadedItem.ItemName;
			if (TryReplaceItemByName(ref item, unloadedItemName, wholeNameReplaceWithItemByName))
				return;

			if (TryReplaceItemByType(ref item, unloadedItemName, wholeNameReplaceWithItemByType))
                return;

            if (TryReplaceItemWithCoins(ref item, unloadedItemName, wholeNameReplaceWithCoins))
                return;
		}
        private static bool TryReplaceItemByName(ref Item item, string unloadedItemName, Dictionary<string, string> wholeNameReplaceWithItemByName) {
            if (!wholeNameReplaceWithItemByName.TryGetValue(unloadedItemName, out string replacementItemName))
                return false;

            for (int i = ItemID.Count; i < ItemLoader.ItemCount; i++) {
                string internalName = ContentSamples.ItemsByType[i].ModItem.Name;
                if (internalName == replacementItemName) {
                    ReplaceItem(ref item, i, unloadedItemName);
                    return true;
                }
            }

            return false;
        }
        private static bool TryReplaceItemByType(ref Item item, string unloadedItemName, Dictionary<string, int> wholeNameReplaceWithItemByType) {
            if (!wholeNameReplaceWithItemByType.TryGetValue(unloadedItemName, out int replacementItemType))
				return false;

			ReplaceItem(ref item, replacementItemType, unloadedItemName);
			return true;
        }
        private static bool TryReplaceItemWithCoins(ref Item item, string unloadedItemName, Dictionary<string, int> wholeNameReplaceWithCoins) {
            if (!wholeNameReplaceWithCoins.TryGetValue(unloadedItemName, out int coinAmount))
                return false;

            ReplaceItem(ref item, coinAmount, unloadedItemName, true);
            return true;
        }
        public static void ReplaceItem(ref Item item, int type, string unloadedItemName, bool replaceWithCoins = false, bool sellPrice = true) {
            int stack = item.stack;

            item.TurnToAir(true);
            if (replaceWithCoins) {
                int total = type * stack;
                if (sellPrice)
                    total /= 5;

				//type is coins when replaceWithCoins is true
				AndroUtilityMethods.ReplaceItemWithCoins(ref item, total);
				($"{unloadedItemName} has been removed from Weapon Enchantments.  You have received Coins equal to its sell price: {total}").LogSimple();
			}
            else {
                item = new Item(type, stack);
                ($"{unloadedItemName} has been removed from Weapon Enchantments.  It has been replaced with {item.S()}").LogSimple();

			}
		}
    }
}
