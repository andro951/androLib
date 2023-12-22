using androLib.Common.Globals;
using androLib.Common.Utility;
using androLib.Items;
using androLib.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace androLib
{
	public class StoragePlayer : ModPlayer {
		public Vector2 CenterBeforeMoveUpdate = Vector2.Zero;
		public static StoragePlayer LocalStoragePlayer => Main.LocalPlayer.GetModPlayer<StoragePlayer>();
		public bool disableLeftShiftTrashCan = false;
		public List<Storage> Storages {
			get {
				if (storages == null)
					StorageManager.PopulateStorages(ref storages);

				return storages;
			}
		}
		private List<Storage> storages = null;
		private List<Storage> unloadedStorages = new();
		public override void SaveData(TagCompound tag) {
			for (int i = 0; i < Storages.Count; i++) {
				Storages[i].SaveData(tag);
			}

			foreach (Storage storage in unloadedStorages) {
				storage.SaveData(tag);
			}
		}
		public override void LoadData(TagCompound tag) {
			for (int i = 0; i < Storages.Count; i++) {
				Storages[i].LoadData(tag);
			}

			foreach (string key in tag.AsEnumerable().Where(p => p.Key.EndsWith(Storage.ItemsTag)).Select(k => k.Key.Substring(0, k.Key.Length - Storage.ItemsTag.Length))) {
				bool found = false;
				foreach (Storage storage in Storages) {
					if (storage.GetModFullName() == key) {
						found = true;
						break;
					}
				}

				if (!found)
					unloadedStorages.Add(new Storage(key, tag));
			}
		}
		public override IEnumerable<Item> AddMaterialsForCrafting(out ItemConsumedCallback itemConsumedCallback) {
			itemConsumedCallback = null;
			if (AndroMod.clientConfig.StopProvidingItemsInBagsForCrafting)
				return null;

			List<Item> items = new();
			foreach (Storage storage in StorageManager.BagUIs.Select(b => b.MyStorage)) {
				if (storage.HasRequiredItemToUseStorage(Main.LocalPlayer, out _, out _)) {
					for (int i = 0; i < storage.Items.Length; i++) {
						ref Item item = ref storage.Items[i];
						if (!item.NullOrAir() && item.stack > 0)
							items.Add(item);
					}
				}
			}

			return items.Count > 0 ? items : null;
		}

		public override bool ShiftClickSlot(Item[] inventory, int context, int slot) {
			//shop
			if (context == 15 || Main.npcShop != 0)
				return false;

			ref Item item = ref inventory[slot];
			if (context == 29) {
				item = item.Clone();
				item.stack = item.maxStack;
				item.OnCreated(new JourneyDuplicationItemCreationContext());
			}

			if (MasterUIManager.NoUIBeingHovered) {
				bool openAndCouldStore = false;
				foreach (BagUI bagUI in StorageManager.BagUIs) {
					if (bagUI.DisplayBagUI && bagUI.CanBeStored(item)) {
						openAndCouldStore = true;
						if (bagUI.TryShiftClickNonBagItemToBag(ref item))
							return true;
					}
				}

				if (openAndCouldStore || !Main.mouseItem.NullOrAir()) {
					MasterUIManager.SwapMouseItem(ref item);
					return true;
				}
			}

			return false;
		}
		public override void OnEnterWorld() {
			StorageManager.CanVacuumItem(new(1), Player);//Sets up all allowed Lists
			CheckClientConfigChanged();
		}
		public override void PreUpdateMovement() {
			CenterBeforeMoveUpdate = Player.Center;
		}
		public override void ResetInfoAccessories() {
			if (Main.netMode == NetmodeID.Server)
				return;

			if (Main.LocalPlayer == null)
				return;

			if (Main.gameMenu)
				return;

			foreach (Storage storage in StorageManager.BagUIs.Select(ui => ui.MyStorage)) {
				if (!storage.ShouldRefreshInfoAccs)
					continue;

				foreach (Item item in storage.Items) {
					if (!item.favorited)
						continue;

					Player.RefreshInfoAccsFromItemType(item);
				}
			}
		}
		public static void PostSetupRecipes() {
			SetupAllAllowedItemManagers();
		}

		#region Allowed Lists

		public static bool PrintDevOnlyAllowedItemListInfo => Debugger.IsAttached && AndroMod.clientConfig.LogAllPlayerWhiteAndBlackLists;
		private static void SetupAllAllowedItemManagers() {
			if (Main.netMode == NetmodeID.Server)
				return;

			IEnumerable<AllowedItemsManager> allowedItemManagers = INeedsSetUpAllowedList.AllowedItemsManagers.Values;
			SortedDictionary<int, SortedSet<int>> enchantedItemsAllowedInBags = new();
			foreach (AllowedItemsManager allowedItemsManager in allowedItemManagers) {
				allowedItemsManager.Setup();
				if (PrintDevOnlyAllowedItemListInfo)
					enchantedItemsAllowedInBags.TryAdd(allowedItemsManager.OwningBagItemType, new());
			}

			List<int> itemsNotAdded = new List<int>();

			for (int i = 0; i < ItemLoader.ItemCount; i++) {
				ItemSetInfo info = new(i);
				if (info.NullOrAir())
					continue;

				if (StorageManager.AllBagTypesSorted.Contains(info.Type))
					continue;

				bool forWhitelistOnlyCheck = false;
				foreach (AllowedItemsManager allowedItemsManager in allowedItemManagers) {
					if (allowedItemsManager.TryAddToAllowedItems(info, forWhitelistOnlyCheck)) {
						forWhitelistOnlyCheck = true;
						if (PrintDevOnlyAllowedItemListInfo && ContentSamples.ItemsByType[i].IsEnchantable())
							enchantedItemsAllowedInBags[allowedItemsManager.OwningBagItemType].Add(info.Type);
					}
				}

				if (!forWhitelistOnlyCheck)
					itemsNotAdded.Add(info.Type);
			}

			if (PrintDevOnlyAllowedItemListInfo) {
				IEnumerable<Item> itemsNotSelected = itemsNotAdded.Select(t => ContentSamples.ItemsByType[t]);
				//itemsNotSelected.Select(i => i.S()).S("All Items that don't fit in bags:").LogSimple();
				itemsNotSelected.Where(i => !i.IsEnchantable()).Select(i => i.S()).S("All Non-Enchantable Items that don't fit in bags:").LogSimple();
				SortedSet<int> sortedItemsNotSelected = new(itemsNotAdded);
				IEnumerable<Item> allOtherItems = ContentSamples.ItemsByType.Select(p => p.Value).Where(i => !sortedItemsNotSelected.Contains(i.type));
				enchantedItemsAllowedInBags.Select(p => p.Value.Select(i => ContentSamples.ItemsByType[i].S()).S(ContentSamples.ItemsByType[p.Key].Name)).S("All Enchantable Items that fit in bags:").LogSimple();
			}

			foreach (AllowedItemsManager allowedItemsManager in allowedItemManagers) {
				allowedItemsManager.ClearSetupLists();
				allowedItemsManager.PostSetup();
			}
		}
		public static bool ClientConfigChanged = false;
		public static Action OnAndroLibClientConfigChangedInGame;
		public static void CheckClientConfigChanged() {
			if (ClientConfigChanged && !Main.gameMenu) {
				SetupAllAllowedItemManagers();
				OnAndroLibClientConfigChangedInGame?.Invoke();
				StorageManager.ResetAllBagSizesFromConfig();
				ClientConfigChanged = false;
			}
		}

		#endregion
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
		//public static bool MouseOutOfTilePlaceRange(this Item item) {//Copied from Vanilla Player.PlaceThing_Tiles()
		//	Vector2 position = Main.LocalPlayer.position;
		//	int tileRangeX = Player.tileRangeX;
		//	int tileRangeY = Player.tileRangeY;
		//	int blockRange = Main.LocalPlayer.blockRange;
		//	int tileTargetX = Player.tileTargetX;
		//	int tileTargetY = Player.tileTargetY;
		//	int width = Main.LocalPlayer.width;
		//	int height = Main.LocalPlayer.height;
		//	return !(position.X / 16f - (float)tileRangeX - (float)item.tileBoost - (float)blockRange <= (float)tileTargetX) || !((position.X + (float)width) / 16f + (float)tileRangeX + (float)item.tileBoost - 1f + (float)blockRange >= (float)tileTargetX) || !(position.Y / 16f - (float)tileRangeY - (float)item.tileBoost - (float)blockRange <= (float)tileTargetY) || !((position.Y + (float)height) / 16f + (float)tileRangeY + (float)item.tileBoost - 2f + (float)blockRange >= (float)tileTargetY);
		//}
	}
}
