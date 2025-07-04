﻿using androLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using androLib.Common.Utility;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;
using androLib.Common.Configs;
using System.Reflection;
using Terraria.ModLoader.Config;
using androLib.Common.Globals;

namespace androLib
{
	public class Storage {
		public int StorageID { get; }
		public Mod Mod { get; set; }
		public Type VacuumStorageType { get; set; }
		public Func<Item, bool> ItemAllowedToBeStored { get; set; }
		public string NameLocalizationKey { get; set; }
		public int StorageSize { get; set; }
		private int originalStorageSize;
		public bool? IsVacuumBag { get; }
		public bool ShouldVacuum {
			get => shouldVacuum;
			set {
				if (IsVacuumBag == false) {
					shouldVacuum = false;
					return;
				}

				shouldVacuum = value;
			}
		}
		private bool shouldVacuum = true;
		public Func<Item, bool> CanVacuumItemWhenNotContained = null;
		private List<Func<int>> StorageItemTypeGetters { get; set; } = new();
		public IEnumerable<int> StorageItemTypes => StorageItemTypeGetters.Select(g => g());
		public Func<Color> GetUIColor { get; set; }
		public Func<Color> GetScrollBarColor { get; set; }
		public Func<Color> GetButtonHoverColor { get; set; }
		public int UILeftDefault { get; }
		public int UITopDefault { get; }
		public int UILeft;
		public int UITop;
		public int UIResizePanelX;
		public int UIResizePanelY;
		public int LastUIResizePanelX;
		public int LastUIResizePanelY;
		public const int DefaultResizePanelIncrement = 100000;
		public int UIResizePanelDefaultX => DefaultResizePanelIncrement * Math.Min(originalStorageSize, 10);
		public int UIResizePanelDefaultY => DefaultResizePanelIncrement * Math.Min(originalStorageSize / 10, 3);
		public int LastUIResizePanelDefaultX;
		public int LastUIResizePanelDefaultY;
		public Func<Item[]> GetItems;
		public Item[] Items {
			get => GetItems != null ? GetItems() : items;
			set => items = value;
		}
		private Item[] items;
		public int RegisteredUI_ID { get; }
		public bool DisplayBagUI = false;
		public Action SelectItemForUIOnly { get; }
		public bool ShouldRefreshInfoAccs { get; }
		private string modFullName;
		private string name = null;
		private Func<Player, IList<Item>> ExtraStorageLocaion = null;
		private IList<Item> myLastInventoryLocation = null;
		private int myLastIndexInTheInventory = -1;
		private int foundBagType = -1;
		private int bagFoundIn = -1;
		private Action<int, bool> UpdateAllowedList;
		private bool IsBlacklistGetter;
		uint nextBagCheck = 0;
		private SortedDictionary<int, int> ItemsIHaveThisTick = new();
		public int SwitcherStorageID;
		public bool ShouldDepositToMagicStorage = false;
		public bool ShouldAddMaterialsForCrafting = true;

		#region Simple HasRequiredItemToUseStorage

		private uint nextBagItemCheckTime = 0;
		private bool found = false;
		private SortedSet<int> specificBagTypesFound = new();

		#endregion

		public Storage(
				int storageID,
				Mod mod, 
				Type vacuumStorageType, 
				Func<Item, bool> itemAllowedToBeStored, 
				string nameLocalizationKey,
				int storageSize, 
				bool? isVacuumBag,
				Func<Color> getUIColor,
				Func<Color> getScrollBarColor,
				Func<Color> getButtonHoverColor,
				List<Func<int>> storageItemTypeGetters,
				int uiLeftDefault, 
				int uiTopDefault,
				Action<int, bool> updateAllowedList,
				bool isBlackListGetter,
				Action selectItemForUIOnly,
				bool shouldRefreshInfoAccs,
				Func<Item, bool> canVacuumItem,
				Func<Player, IList<Item>> extraStorageLocaion
			) {
			StorageID = storageID;
			Mod = mod;
			VacuumStorageType = vacuumStorageType;
			modFullName = DefaultModFullName();
			ItemAllowedToBeStored = itemAllowedToBeStored;
			NameLocalizationKey = nameLocalizationKey;
			originalStorageSize = storageSize;
			StorageSize = GetBagSize(originalStorageSize);
			IsVacuumBag = isVacuumBag;
			GetUIColor = getUIColor;
			GetScrollBarColor = getScrollBarColor;
			GetButtonHoverColor = getButtonHoverColor;
			StorageItemTypeGetters = storageItemTypeGetters;
			UILeftDefault = uiLeftDefault;
			UITopDefault = uiTopDefault;
			UILeft = UILeftDefault;
			UITop = UITopDefault;
			int columns = StorageSize < 80 ? 10 : 20;
			LastUIResizePanelDefaultX = columns * DefaultResizePanelIncrement;
			LastUIResizePanelDefaultY = StorageSize < 200 ? StorageSize.CeilingDivide(columns) * DefaultResizePanelIncrement : UIResizePanelDefaultX;
			UpdateAllowedList = updateAllowedList;
			IsBlacklistGetter = isBlackListGetter;
			SelectItemForUIOnly = selectItemForUIOnly;
			ShouldRefreshInfoAccs = shouldRefreshInfoAccs;
			CanVacuumItemWhenNotContained = canVacuumItem;
			ExtraStorageLocaion = extraStorageLocaion;
			items = Enumerable.Repeat(new Item(), StorageSize).ToArray();
			ShouldVacuum = IsVacuumBag != false;
			UIResizePanelX = UIResizePanelDefaultX;
			UIResizePanelY = UIResizePanelDefaultY;
			LastUIResizePanelX = LastUIResizePanelDefaultX;
			LastUIResizePanelY = LastUIResizePanelDefaultY;
			SwitcherStorageID = StorageID;
		}

		/// <summary>
		/// Only used to save/load data when it's not loaded via the normal way to prevent deleting unloaded mod data.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="tag"></param>
		public Storage(string key, TagCompound tag) {
			modFullName = key;
			LoadData(tag);
		}

		public string GetModFullName() => modFullName;
		private string DefaultModFullName() => $"{Mod.Name}_{VacuumStorageType.Name}";
		public const string ItemsTag = "_Items";
		public const string NameTag = "_Name";
		public const string UILeftTag = "_UILeft";
		public const string UITopTag = "_UITop";
		public const string ShouldVacuumItemsTag = "_ShouldVacuumItems";
		public const string UIResizePanelXTag = "_UIResizePanelX";
		public const string UIResizePanelYTag = "_UIResizePanelY";
		public const string LastUIResizePanelXTag = "_LastUIResizePanelX";
		public const string LastUIResizePanelYTag = "_LastUIResizePanelY";
		public const string SwitcherStorageIDTag = "_SwitcherStorageID";
		public const string ShouldDepositToMagicStorageTag = "_ShouldDepositToMagicStorage";
		public void SaveData(TagCompound tag) {
			string modFullName = GetModFullName();
			if (GetItems == null)
				tag[$"{modFullName}{ItemsTag}"] = items;

			tag[$"{modFullName}{NameTag}"] = name;
			tag[$"{modFullName}{UILeftTag}"] = UILeft;
			tag[$"{modFullName}{UITopTag}"] = UITop;
			tag[$"{modFullName}{ShouldVacuumItemsTag}"] = ShouldVacuum;
			tag[$"{modFullName}{UIResizePanelXTag}"] = UIResizePanelX;
			tag[$"{modFullName}{UIResizePanelYTag}"] = UIResizePanelY;
			tag[$"{modFullName}{LastUIResizePanelXTag}"] = LastUIResizePanelX;
			tag[$"{modFullName}{LastUIResizePanelYTag}"] = LastUIResizePanelY;
			tag[$"{modFullName}{SwitcherStorageIDTag}"] = SwitcherStorageID;
			tag[$"{modFullName}{ShouldDepositToMagicStorageTag}"] = ShouldDepositToMagicStorage;
		}
		public void LoadData(TagCompound tag) {
			string modFullName = GetModFullName();
			if (!tag.TryGet($"{modFullName}{ItemsTag}", out items))
				items = Enumerable.Repeat(new Item(), StorageSize).ToArray();

			//Mod != null is checking if the bag is loaded the correct way instead of unloaded.
			if (Mod != null && items.Length > StorageSize)
				TryUpdateItemsToCurrentStorageSize();

			string loadedName = tag.Get<string>($"{modFullName}{NameTag}");
			Rename(loadedName);
			bool temp = !items.Where(i => !i.NullOrAir()).Any();
			
			int uiLeft = tag.Get<int>($"{modFullName}{UILeftTag}");
			int uiTop = tag.Get<int>($"{modFullName}{UITopTag}");
			MasterUIManager.CheckOutOfBoundsRestoreDefaultPosition(ref uiLeft, ref uiTop, UILeftDefault, UITopDefault);
			UILeft = uiLeft;
			UITop = uiTop;

			ShouldVacuum = tag.Get<bool>($"{modFullName}{ShouldVacuumItemsTag}");

			if (!tag.TryGet<int>($"{modFullName}{UIResizePanelXTag}", out UIResizePanelX))
				UIResizePanelX = UIResizePanelDefaultX;

			if (!tag.TryGet<int>($"{modFullName}{UIResizePanelYTag}", out UIResizePanelY))
				UIResizePanelY = UIResizePanelDefaultY;

			if (!tag.TryGet<int>($"{modFullName}{LastUIResizePanelXTag}", out LastUIResizePanelX))
				LastUIResizePanelX = LastUIResizePanelDefaultX;

			if (!tag.TryGet<int>($"{modFullName}{LastUIResizePanelYTag}", out LastUIResizePanelY))
				LastUIResizePanelY = LastUIResizePanelDefaultY;

			if (tag.TryGet<int>($"{modFullName}{SwitcherStorageIDTag}", out int switcherStorageID)) {
				SwitcherStorageID = switcherStorageID;
				if (SwitcherStorageID < 0)
					SwitcherStorageID = StorageID;
			}
			else {
				SwitcherStorageID = StorageID;
			}

			ShouldDepositToMagicStorage = tag.Get<bool>($"{modFullName}{ShouldDepositToMagicStorageTag}");
		}
		private void TryUpdateItemsToCurrentStorageSize() {
			if (items.Length == StorageSize || GetItems != null)
				return;

			if (items.Length < StorageSize) {
				items = items.Concat(Enumerable.Repeat(new Item(), StorageSize - items.Length)).ToArray();
				return;
			}

			IEnumerable<Item> nonAirItems = items.Where(item => !item.NullOrAir());
			int nonAirItemCount = nonAirItems.Count();
			if (nonAirItemCount >= StorageSize) {
				items = nonAirItems.ToArray();
				return;
			}
			else if (nonAirItemCount < 1) {
				items = Enumerable.Repeat(new Item(), StorageSize).ToArray();
				return;
			}

			int allowedOpenSlots = StorageSize - nonAirItemCount;
			int index = 0;
			int airCount = 0;
			while (airCount <= allowedOpenSlots && index <= StorageSize) {
				if (items[index].NullOrAir())
					airCount++;

				index++;
			}

			items = items.Take(index - 1).Concat(nonAirItems.Reverse().Take(StorageSize - (index - 1)).Reverse()).ToArray();
		}
		private bool TryGetNextOpenSlot(Item[] items, ref int nextOpenSlot, bool fromEnd = false) {
			if (!fromEnd) {
				while (nextOpenSlot < items.Length && !items[nextOpenSlot].NullOrAir()) {
					nextOpenSlot++;
				}

				return nextOpenSlot < items.Length;
			}
			else {
				while (nextOpenSlot >= 0 && !items[nextOpenSlot].NullOrAir()) {
					nextOpenSlot--;
				}

				return nextOpenSlot >= 0;
			}
		}
		public Storage Clone() {
			Storage clone = new Storage(
				StorageID,
				Mod,
				VacuumStorageType,
				ItemAllowedToBeStored,
				NameLocalizationKey,
				originalStorageSize,
				IsVacuumBag,
				GetUIColor,
				GetScrollBarColor,
				GetButtonHoverColor,
				StorageItemTypeGetters,
				UILeftDefault,
				UITopDefault,
				UpdateAllowedList,
				IsBlacklistGetter,
				SelectItemForUIOnly,
				ShouldRefreshInfoAccs,
				CanVacuumItemWhenNotContained,
				ExtraStorageLocaion
			);

			clone.name = name;
			clone.UILeft = UILeft;
			clone.UITop = UITop;
			clone.StorageSize = StorageSize;
			clone.items = new Item[StorageSize];
			Array.Copy(items, clone.items, StorageSize);
			clone.ShouldVacuum = ShouldVacuum;
			clone.UIResizePanelX = UIResizePanelX;
			clone.UIResizePanelY = UIResizePanelY;
			clone.LastUIResizePanelX = LastUIResizePanelX;
			clone.LastUIResizePanelY = LastUIResizePanelY;
			clone.SwitcherStorageID = SwitcherStorageID;
			clone.ShouldDepositToMagicStorage = ShouldDepositToMagicStorage;
			clone.ExtraStorageLocaion = ExtraStorageLocaion;

			return clone;
		}
		public string GetLocalizedName() {
			if (NameLocalizationKey != "-") {
				if (NameLocalizationKey != null) {
					string localizedName = Language.GetTextValue(NameLocalizationKey);
					if (localizedName != NameLocalizationKey)
						return localizedName;
				}
				
				NameLocalizationKey = $"Mods.{Mod.Name}.{L_ID1.Items}.{VacuumStorageType.Name}.{L_ID2.DisplayName}";
				string localizedNameModItem = Language.GetTextValue(NameLocalizationKey);
				if (localizedNameModItem != NameLocalizationKey)
					return localizedNameModItem;

				NameLocalizationKey = "-";
			}

			return VacuumStorageType.Name.AddSpaces();
		}
		public bool? ValidItemTypeGetters(out SortedSet<int> itemTypes) {
			itemTypes = new();
			bool? found = null;//null means no associated item, so it does not need to check for an associated item in the player's inventory.
			foreach (Func<int> storageItemTypeGetter in StorageItemTypeGetters) {
				int itemType = storageItemTypeGetter();
				if (itemType == -1)
					continue;

				if (itemType < 0 || itemType >= ItemLoader.ItemCount) {
					if (found == null)
						found = false;//If item type is out of range, don't add it and set found to false.

					continue;
				}

				found = true;
				itemTypes.Add(itemType);
			}

			return found;
		}
		public static readonly int ItemNotFound = -1;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index">The index in the players inventory.  If found in a bag, the index will be -index + ReuiredItemInABagStartingIndex.<\br>
		/// Use GetItemFromHasRequiredItemToUseStorageIndex() to get the item savely.</param>
		/// <returns></returns>
		public bool HasRequiredItemToUseStorage(Player player, out IList<Item> inventoryFoundIn, out int index, out int otherBagThisBagWasFoundIn, int specifiedBagType = -1) {
			bool foundResult = true;
			bool useSpecifiedBagType = specifiedBagType != -1;
			if (foundBagType == -1 || useSpecifiedBagType && foundBagType != specifiedBagType)
				foundResult = false;

			if (foundResult) {
				Item lastBagLocationItem = myLastInventoryLocation?[myLastIndexInTheInventory];
				if (foundBagType == lastBagLocationItem?.type) {
					bool foundInANonBagInventory = bagFoundIn == -1;
					if (foundInANonBagInventory || StorageManager.VacuumStorageIndexesFromBagTypes.TryGetValue(foundBagType, out int bagID) && StorageManager.BagUIs[bagID].DisplayBagUI && bagFoundIn > -1 && StorageManager.BagUIs[bagFoundIn].DisplayBagUI) {
						inventoryFoundIn = myLastInventoryLocation;
						index = myLastIndexInTheInventory;
						otherBagThisBagWasFoundIn = bagFoundIn;
						return true;
					}
				}
			}

			//Only look once per tick
			if (nextBagCheck > Main.GameUpdateCount) {
				inventoryFoundIn = null;
				index = ItemNotFound;
				otherBagThisBagWasFoundIn = -1;
				return false;
			}
			else {
				nextBagCheck = Main.GameUpdateCount + 1u;
			}

			bagFoundIn = -1;
			otherBagThisBagWasFoundIn = -1;
			index = ItemNotFound;
			inventoryFoundIn = null;
			SortedSet<int> bagItemTypes;
			if (specifiedBagType != -1) {
				bagItemTypes = new() { specifiedBagType };
			}
			else {
				bool? validItemType = ValidItemTypeGetters(out bagItemTypes);
				if (validItemType != true)
					return validItemType == null;//null means no associated item.  false means the bag type was not in the valid range and not default -1.
			}

			//Check extra storage locations for this bag.
			if (ExtraStorageLocaion != null) {
				IList<Item> extraStorageLocation = ExtraStorageLocaion(player);
				if (extraStorageLocation != null) {
					for (int i = 0; i < extraStorageLocation.Count; i++) {
						Item item = extraStorageLocation[i];
						if (bagItemTypes.Contains(item.type)) {
							index = i;
							inventoryFoundIn = extraStorageLocation;
							myLastInventoryLocation = inventoryFoundIn;
							myLastIndexInTheInventory = index;
							foundBagType = item.type;
							return true;
						}
					}
				}
			}

			//Check player inventory for this bag.
			for (int i = 0; i < player.inventory.Length; i++) {
				Item item = player.inventory[i];
				if (bagItemTypes.Contains(item.type)) {
					index = i;
					inventoryFoundIn = player.inventory;
					myLastInventoryLocation = inventoryFoundIn;
					myLastIndexInTheInventory = index;
					foundBagType = item.type;
					return true;
				}
			}

			//Fix for dynamicgearadvancements calling ItemLoader.ChooseAmmo() before Mod Players are set up.
			if (!Main.LocalPlayer.TryGetModPlayer(out StoragePlayer _))
				return false;

			//Check all other bags for this bag.
			foreach (int bagType in bagItemTypes) {
				Item bagItem = bagType.CSI();
				for (int j = 0; j < StorageManager.BagUIs.Count; j++) {
					BagUI bagUI = StorageManager.BagUIs[j];
					if (!bagUI.DisplayBagUI || !bagUI.CanBeStored(bagItem))
						continue;

					if (bagUI.MyStorage.ContainsSlow(bagType, out int myLastIndex)) {
						index = myLastIndex;
						inventoryFoundIn = bagUI.MyStorage.Items;
						myLastInventoryLocation = inventoryFoundIn;
						myLastIndexInTheInventory = index;
						foundBagType = bagType;
						bagFoundIn = j;
						otherBagThisBagWasFoundIn = bagFoundIn;
						return true;
					}
				}
			}

			foreach (int bagItemType in bagItemTypes) {
				//Check adj tiles for this bag.
				int createTile = ContentSamples.ItemsByType[bagItemType].createTile;
				if (createTile > -1 && player.adjTile[createTile]) {
					if (!useSpecifiedBagType || !found) {
						myLastInventoryLocation = null;
						myLastIndexInTheInventory = ItemNotFound;
					}
					
					return true;
				}
			}

			if (!useSpecifiedBagType) {
				myLastInventoryLocation = null;
				myLastIndexInTheInventory = ItemNotFound;
			}

			return false;
		}
		private uint GetDelay(bool found) => found ? 30u : 10u;//It can wait a little longer to stop having a bag.
		public bool HasRequiredItemToUseStorageSlow(Player player) {
			if (Main.GameUpdateCount < nextBagItemCheckTime && Main.GameUpdateCount != nextBagItemCheckTime - GetDelay(found))
				return found;

			nextBagItemCheckTime = Main.GameUpdateCount + GetDelay(found);
			found = HasRequiredItemToUseStorage(player, out _, out _, out _);
			return found;
		}
		/// <summary>
		/// Don't forget to check the type of the found item if using this method since it only checks every 10-30 ticks if that amount if time is important.
		/// </summary>
		public bool HasRequiredItemToUseStorageSlow(Player player, out IList<Item> inventoryFoundIn, out int index, out int otherBagThisBagWasFoundIn, int specifiedBagType = -1) {
			if (specifiedBagType != -1) {
				bool check = Main.GameUpdateCount < nextBagItemCheckTime || found && specifiedBagType != foundBagType && Main.GameUpdateCount == nextBagItemCheckTime - GetDelay(found) + 1;
				if (!check) {
					inventoryFoundIn = myLastInventoryLocation;
					index = myLastIndexInTheInventory;
					otherBagThisBagWasFoundIn = bagFoundIn;
					return specificBagTypesFound.Contains(specifiedBagType); ;
				}

				specificBagTypesFound.Remove(specifiedBagType);

				nextBagItemCheckTime = Main.GameUpdateCount + GetDelay(found);
				bool foundExact = HasRequiredItemToUseStorage(player, out inventoryFoundIn, out index, out otherBagThisBagWasFoundIn, specifiedBagType);
				if (foundExact)
					specificBagTypesFound.Add(specifiedBagType);

				if (foundExact || specifiedBagType == foundBagType)
					found = foundExact;

				return specificBagTypesFound.Contains(specifiedBagType);
			}

			if (Main.GameUpdateCount < nextBagItemCheckTime) {
				inventoryFoundIn = myLastInventoryLocation;
				index = myLastIndexInTheInventory;
				otherBagThisBagWasFoundIn = bagFoundIn;
				return found;
			}

			specificBagTypesFound.Clear();

			nextBagItemCheckTime = Main.GameUpdateCount + GetDelay(found);
			found = HasRequiredItemToUseStorage(player, out inventoryFoundIn, out index, out otherBagThisBagWasFoundIn, specifiedBagType);

			return found;
		}
		public void RegisterItemTypeGetter(Func<int> itemTypeGetter) {
			if (itemTypeGetter == null)
				return;

			StorageItemTypeGetters.Add(itemTypeGetter);
		}
		public void SelectItemSlotFunc() {
			if (SelectItemForUIOnly != null)
				SelectItemForUIOnly();
		}
		public bool HasSelectItemForUIOnlyFunc() => SelectItemForUIOnly != null;
		public bool TryAddToPlayerWhitelist(int type) {
			if (!HasWhiteOreBlacklistGetter)
				return false;

			UpdateAllowedList(type, true);

			return true;
		}
		public bool TryAddToPlayerBlacklist(int type) {
			if (!HasWhiteOreBlacklistGetter)
				return false;

			UpdateAllowedList(type, false);

			return true;
		}
		public bool HasWhiteListGetter => !IsBlacklistGetter && HasWhiteOreBlacklistGetter;
		public bool HasWhiteOreBlacklistGetter => UpdateAllowedList != null;
		private int GetBagSize(int bagSize) {
			if (bagSize >= 1)//Mod creator chose for the bag to not be resizable.
				return bagSize;

			bagSize = Math.Abs(bagSize);
			if (bagSize < 1)
				bagSize = 1;

			List<StorageSizePair> bagStorageSizePairs = AndroMod.clientConfig.StorageSizes;
			if (bagStorageSizePairs == null)
				return bagSize;

			foreach (StorageSizePair bagStorageSizePair in bagStorageSizePairs) {
				if (bagStorageSizePair.ModFullName != modFullName)
					continue;

				return bagStorageSizePair.StorageSize;
			}

			bagStorageSizePairs.Add(new StorageSizePair(modFullName, bagSize));
			StorageManager.SaveClientAndroConfig();

			return bagSize;
		}
		public void ResetBagSizeFromConfig() {
			if (originalStorageSize >= 1)
				return;

			int oldStorageSize = StorageSize;
			StorageSize = GetBagSize(originalStorageSize);
			if (oldStorageSize == StorageSize)
				return;

			TryUpdateItemsToCurrentStorageSize();
		}
		private uint nextContainsUpdate = 0;
		public bool Contains(Item item, out int index) => Contains(item.type, out index);
		public bool Contains(int itemType, out int index) {
			if (Main.GameUpdateCount >= nextContainsUpdate) {
				nextContainsUpdate = Main.GameUpdateCount + 1u;
				nextSlowUpdate = Main.GameUpdateCount + 10u;
				if (nextSlowUpdate < nextContainsUpdate)
					nextSlowUpdate = nextContainsUpdate;

				ItemsIHaveThisTick = new();
				for (int i = 0; i < Items.Length; i++) {
					Item myItem = Items[i];
					if (myItem.NullOrAir())
						continue;

					ItemsIHaveThisTick.TryAdd(myItem.type, i);
				}
			}
			
			return ItemsIHaveThisTick.TryGetValue(itemType, out index);
		}
		private uint nextSlowUpdate = 0;

		public bool ContainsSlow(Item item, out int index) => ContainsSlow(item.type, out index);
		public bool ContainsSlow(int itemType, out int index) {
			if (Main.GameUpdateCount >= nextContainsUpdate) {
				nextContainsUpdate = Main.GameUpdateCount + 10u;
				ItemsIHaveThisTick = new();
				for (int i = 0; i < Items.Length; i++) {
					Item myItem = Items[i];
					if (myItem.NullOrAir())
						continue;

					ItemsIHaveThisTick.TryAdd(myItem.type, i);
				}
			}


			return ItemsIHaveThisTick.TryGetValue(itemType, out index);
		}
		public bool ContainsSlow(Item item) => ContainsSlow(item.type);
		public bool ContainsSlow(int itemType) {
			if (Main.GameUpdateCount >= nextContainsUpdate) {
				nextContainsUpdate = Main.GameUpdateCount + 10u;
				ItemsIHaveThisTick = new();
				for (int i = 0; i < Items.Length; i++) {
					Item myItem = Items[i];
					if (myItem.NullOrAir())
						continue;

					ItemsIHaveThisTick.TryAdd(myItem.type, i);
				}
			}

			return ItemsIHaveThisTick.ContainsKey(itemType);
		}
		public string DisplayedName => name ?? GetLocalizedName();
		public string Name => name;
		public void Rename(string newName) {
			if (newName == "") {
				newName = null;
			}
			else if (newName != null) {
				bool allSpaces = true;
				foreach (char c in newName) {
					if (c != ' ') {
						allSpaces = false;
						break;
					}
				}

				if (allSpaces)
					newName = null;
			}

			name = newName;
		}
		public override string ToString() {
			return $"{GetModFullName()} ({StorageID})";
		}

		internal void ResetTimers() {
			nextBagCheck = 0;
			nextContainsUpdate = 0;
			nextBagItemCheckTime = 0;
			nextSlowUpdate = 0;
		}
	}
	public static class StorageManager {
		public static int DefaultLeftLocationOnScreen => 80;
		public static int DefaultTopLocationOnScreen => 675;
		public static int DefaultStorageSize => 100;

		#region Lists and Dictionaries

		private static SortedDictionary<string, int> vacuumStorageIndexes = new();
		public static SortedDictionary<int, int> VacuumStorageIndexesFromBagTypes {
			get {
				if (vacuumStorageIndexesFromBagTypes == null)
					PopulateVacuumStorageIndexesFromBagTypes();

				return vacuumStorageIndexesFromBagTypes;
			}
		}
		private static SortedDictionary<int, int> vacuumStorageIndexesFromBagTypes = null;
		public static List<BagUI> BagUIs = new();
		public static List<Storage> RegisteredStorages = new();
		public static void Load() {
			AndroMod.OnResetGameCounter += () => ResetAllStorageTimers();
		}
		private static void ResetAllStorageTimers() {
			foreach (Storage storage in RegisteredStorages) {
				storage.ResetTimers();
			}

			if (Main.LocalPlayer?.TryGetModPlayer(out StoragePlayer storagePlayer) == true && storagePlayer != null) {
				foreach (Storage storage in storagePlayer.Storages) {
					storage.ResetTimers();
				}
			}
		}

		public static void Unload() {
			RegisteredStorages.Clear();
			BagUIs.Clear();
			vacuumStorageIndexes.Clear();
			vacuumStorageIndexesFromBagTypes = null;
			allBagTypes = null;
			allBagTypesFirstForEachInventory = null;
			allBagTypesSorted = null;
			storageItemTypes = null;
			storageTileTypes = null;
			whitelistIndexes = null;
			blacklistIndexes = null;
		}
		public static void PopulateStorages(ref List<Storage> storages) {
			storages = new();
			for (int i = 0; i < RegisteredStorages.Count; i++) {
				Storage storage = RegisteredStorages[i].Clone();
				storages.Add(storage);
			}
		}
		public static void PopulateVacuumStorageIndexesFromBagTypes() {
			vacuumStorageIndexesFromBagTypes = new();
			for (int i = 0; i < RegisteredStorages.Count; i++) {
				Storage storage = RegisteredStorages[i];
				if (storage.ValidItemTypeGetters(out SortedSet<int> bagTypes) != true)
					continue;

				foreach (int bagType in bagTypes) {
					vacuumStorageIndexesFromBagTypes.Add(bagType, i);
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bagType"></param>
		/// <param name="storageItems">Storage items for the storage for bagType, not the bag containing the bagType item if stored in one.</param>
		/// <param name="storageBagFoundIn">Storage items that the bag is in.  storageBagFoundIn[index] is the bag of type bagType.  Make sure to null check storageBagFoundIn before using it or check index >= 0.</param>
		/// <param name="indexFoundAt"></param>
		/// <returns></returns>
		public static bool HasRequiredItemToUseStorageFromBagType(Player player, int bagType, out IList<Item> inventoryFoundIn, out int index, out int otherBagThisBagWasFoundIn, bool onlyThisBagType = false) {
			if (VacuumStorageIndexesFromBagTypes.TryGetValue(bagType, out int storageID)) {
				Storage storage = BagUIs[storageID].MyStorage;
				if (storage.HasRequiredItemToUseStorage(player, out inventoryFoundIn, out index, out otherBagThisBagWasFoundIn, onlyThisBagType ? bagType : -1))
					return true;
			}

			inventoryFoundIn = null;
			index = Storage.ItemNotFound;
			otherBagThisBagWasFoundIn = -1;

			return false;
		}
		public static bool HasRequiredItemToUseStorageFromBagTypeSlow(Player player, int bagType) {
			if (VacuumStorageIndexesFromBagTypes.TryGetValue(bagType, out int storageID)) {
				Storage storage = BagUIs[storageID].MyStorage;
				if (storage.HasRequiredItemToUseStorageSlow(player))
					return true;
			}

			return false;
		}
		public static bool HasRequiredItemToUseStorageFromBagTypeSlow(Player player, int bagType, out IList<Item> inventoryFoundIn, out int index, out int otherBagThisBagWasFoundIn, bool onlyThisBagType = false) {
			if (VacuumStorageIndexesFromBagTypes.TryGetValue(bagType, out int storageID)) {
				Storage storage = BagUIs[storageID].MyStorage;
				if (storage.HasRequiredItemToUseStorageSlow(player, out inventoryFoundIn, out index, out otherBagThisBagWasFoundIn, onlyThisBagType ? bagType : -1))
					return true;
			}

			inventoryFoundIn = null;
			index = Storage.ItemNotFound;
			otherBagThisBagWasFoundIn = -1;

			return false;
		}

		public static List<int> AllBagTypes {
			get {
				if (allBagTypes == null)
					allBagTypes = RegisteredStorages.Select(s => s.StorageItemTypes).SelectMany(s => s).ToList();

				return allBagTypes;
			}
		}
		private static List<int> allBagTypes = null;
		public static List<int> AllBagTypesFirstForEachInventory {
			get {
				if (allBagTypesFirstForEachInventory == null)
					allBagTypesFirstForEachInventory = RegisteredStorages.Select(s => s.StorageItemTypes).Select(s => s.First()).ToList();

				return allBagTypesFirstForEachInventory;
			}
		}
		private static List<int> allBagTypesFirstForEachInventory = null;

		public static SortedSet<int> AllBagTypesSorted {
			get {
				if (allBagTypesSorted == null)
					allBagTypesSorted = new(AllBagTypes);

				return allBagTypesSorted;
			}
		}
		private static SortedSet<int> allBagTypesSorted = null;

		/// <summary>
		/// <int itemType, int bagID>
		/// </summary>
		public static SortedDictionary<int, int> StorageItemTypes {
			get {
				if (storageItemTypes == null) {
					SetUpStorageItemAndTileTypes();
				}

				return storageItemTypes;
			}
		}
		private static SortedDictionary<int, int> storageItemTypes = null;
		public static SortedDictionary<int, int> StorageTileTypes {
			get {
				if (storageTileTypes == null) {
					SetUpStorageItemAndTileTypes();
				}

				return storageTileTypes;
			}
		}
		private static SortedDictionary<int, int> storageTileTypes = null;
		private static void SetUpStorageItemAndTileTypes() {
			storageItemTypes = new();
			for (int i = 0; i < BagUIs.Count; i++) {
				if (BagUIs[i].MyStorage.ValidItemTypeGetters(out SortedSet<int> itemTypes) == true) {
					foreach (int itemType in itemTypes) {
						storageItemTypes.Add(itemType, i);
					}
				}
			}

			storageTileTypes = new();
			foreach (int itemType in storageItemTypes.Keys) {
				int createTile = ContentSamples.ItemsByType[itemType].createTile;
				if (createTile > 0)
					storageTileTypes.Add(createTile, storageItemTypes[itemType]);
			}
		}

		private static SortedDictionary<int, int> WhitelistIndexes {//StorageID, WhitelistIndex
			get {
				if (whitelistIndexes == null)
					SetupWhitelistAndBlacklistIndexes();

				return whitelistIndexes;
			}
		}
		private static SortedDictionary<int, int> whitelistIndexes = null;
		private static SortedDictionary<int, int> BlacklistIndexes {//StorageID, BlacklistIndex
			get {
				if (blacklistIndexes == null)
					SetupWhitelistAndBlacklistIndexes();

				return blacklistIndexes;
			}
		}
		private static SortedDictionary<int, int> blacklistIndexes = null;
		private static void SetupWhitelistAndBlacklistIndexes() {
			whitelistIndexes = new();
			List<ItemList> whitelists = AndroMod.clientConfig.WhiteLists;
			for (int i = 0; i < whitelists.Count; i++) {
				ItemList whitelist = whitelists[i];
				if (whitelist.ModFullName == null) {
					whitelists.RemoveAt(i);
					i--;
					continue;
				}

				if (vacuumStorageIndexes.TryGetValue(whitelist.ModFullName, out int storageID))
					whitelistIndexes.Add(storageID, i);
			}

			foreach (KeyValuePair<string, int> pair in vacuumStorageIndexes) {
				if (!RegisteredStorages[pair.Value].HasWhiteListGetter)
					continue;

				if (!whitelistIndexes.ContainsKey(pair.Value)) {
					whitelists.Add(new(pair.Key));
					whitelistIndexes.Add(pair.Value, whitelists.Count - 1);
				}
			}

			blacklistIndexes = new();
			List<ItemList> blacklists = AndroMod.clientConfig.BlackLists;
			for (int i = 0; i < blacklists.Count; i++) {
				ItemList blacklist = blacklists[i];
				if (blacklist.ModFullName == null) {
					blacklists.RemoveAt(i);
					i--;
					continue;
				}

				if (vacuumStorageIndexes.TryGetValue(blacklist.ModFullName, out int storageID))
					blacklistIndexes.Add(storageID, i);
			}

			foreach (KeyValuePair<string, int> pair in vacuumStorageIndexes) {
				if (!RegisteredStorages[pair.Value].HasWhiteOreBlacklistGetter)
					continue;

				if (!blacklistIndexes.ContainsKey(pair.Value)) {
					blacklists.Add(new(pair.Key));
					blacklistIndexes.Add(pair.Value, blacklists.Count - 1);
				}
			}

			SaveClientAndroConfig();
		}
		public static ItemList GetPlayerWhitelist(int storageID) {
			//Need to check for HasAllowedListGetter first!
			List<ItemList> whitelists = AndroMod.clientConfig.WhiteLists;
			if (!WhitelistIndexes.TryGetValue(storageID, out int whitelistIndex)) {
				whitelists.Add(new(RegisteredStorages[storageID].GetModFullName()));
				whitelistIndex = whitelists.Count - 1;
				whitelistIndexes.Add(storageID, whitelistIndex);
			}

			if (whitelistIndex >= whitelists.Count || whitelists[whitelistIndex].ModFullName != RegisteredStorages[storageID].GetModFullName()) {
				SetupWhitelistAndBlacklistIndexes();
				whitelists = AndroMod.clientConfig.WhiteLists;
			}

			return whitelists[whitelistIndex];
		}
		public static ItemList GetPlayerBlacklist(int storageID) {
			//Need to check for HasAllowedListGetter first!
			List<ItemList> blacklists = AndroMod.clientConfig.BlackLists;
			if (!BlacklistIndexes.TryGetValue(storageID, out int blacklistIndex)) {
				blacklists.Add(new(RegisteredStorages[storageID].GetModFullName()));
				blacklistIndex = blacklists.Count - 1;
				blacklistIndexes.Add(storageID, blacklistIndex);
			}
			
			if (blacklistIndex >= blacklists.Count || blacklists[blacklistIndex].ModFullName != RegisteredStorages[storageID].GetModFullName()) {
				SetupWhitelistAndBlacklistIndexes();
				blacklists = AndroMod.clientConfig.BlackLists;
			}

			return blacklists[blacklistIndex];
		}
		public static bool TryGetPlayerWhitelist(int storageID, out ItemList itemList) {
			if (!RegisteredStorages[storageID].HasWhiteListGetter) {
				itemList = null;
				return false;
			}
			
			itemList = GetPlayerWhitelist(storageID);
			return true;
		}
		public static bool TryGetPlayerBlacklist(int storageID, out ItemList itemList) {
			if (!RegisteredStorages[storageID].HasWhiteOreBlacklistGetter) {
				itemList = null;
				return false;
			}
			
			itemList = GetPlayerBlacklist(storageID);
			return true;
		}
		public static bool AddToPlayerWhitelist(int storageID, int itemType) {
			bool message = false;
			if (TryGetPlayerWhitelist(storageID, out ItemList whiteList)) {
				for (int i = 0; i < whiteList.ItemDefinitions.Count; i++) {
					if (whiteList.ItemDefinitions[i].Type == itemType)
						break;
				}

				whiteList.ItemDefinitions.Add(new(itemType));
				message = true;
			}

			message |= TryRemoveFromPlayerBlacklist(storageID, itemType, true);

			if (message) {
				if (RegisteredStorages[storageID].ValidItemTypeGetters(out SortedSet<int> bagItemTypes) == true) {
					string bagName = bagItemTypes.First().CSI().Name;
					Main.NewText(AndroLibGameMessages.AddedToWhitelist.ToString().Lang(AndroMod.ModName, L_ID1.AndroLibGameMessages, new object[] { itemType.CSI().Name, bagName }));
				}
			}

			return message;
		}
		public static bool AddToPlayerBlacklist(int storageID, int itemType) {
			bool message = false;
			Storage storage = BagUIs[storageID].Storage;
			bool skipAddingToBlacklist = storage.CanVacuumItemWhenNotContained != null && storage.HasWhiteListGetter && storage.CanVacuumItemWhenNotContained(itemType.CSI());
			if (!skipAddingToBlacklist && TryGetPlayerBlacklist(storageID, out ItemList blackList)) {
				for (int i = 0; i < blackList.ItemDefinitions.Count; i++) {
					if (blackList.ItemDefinitions[i].Type == itemType)
						break;
				}

				blackList.ItemDefinitions.Add(new(itemType));
				message = true;
			}

			message |= TryRemoveFromPlayerWhitelist(storageID, itemType, true);

			if (message) {
				if (RegisteredStorages[storageID].ValidItemTypeGetters(out SortedSet<int> bagItemTypes) == true) {
					string bagName = bagItemTypes.First().CSI().Name;
					if (skipAddingToBlacklist) {
						Main.NewText(AndroLibGameMessages.RemovedFromWhitelist.ToString().Lang(AndroMod.ModName, L_ID1.AndroLibGameMessages, new object[] { itemType.CSI().Name, bagName }));
					}
					else {
						Main.NewText(AndroLibGameMessages.AddedToBlacklist.ToString().Lang(AndroMod.ModName, L_ID1.AndroLibGameMessages, new object[] { itemType.CSI().Name, bagName }));
					}
				}
			}

			return message && !skipAddingToBlacklist;
		}
		public static bool TryRemoveFromPlayerWhitelist(int storageID, int itemType, bool forseSave = false) {
			bool shouldSave = forseSave;
			if (TryGetPlayerWhitelist(storageID, out ItemList whiteList)) {
				for (int i = 0; i < whiteList.ItemDefinitions.Count; i++) {
					if (whiteList.ItemDefinitions[i].Type == itemType) {
						whiteList.ItemDefinitions.RemoveAt(i);
						shouldSave = true;
						break;
					}
				}
			}

			if (!shouldSave)
				return shouldSave;

			SaveClientAndroConfig();

			return shouldSave;
		}
		public static bool TryRemoveFromPlayerBlacklist(int storageID, int itemType, bool forseSave = false) {
			bool shouldSave = forseSave;
			if (TryGetPlayerBlacklist(storageID, out ItemList blackList)) {
				for (int i = 0; i < blackList.ItemDefinitions.Count; i++) {
					if (blackList.ItemDefinitions[i].Type == itemType) {
						blackList.ItemDefinitions.RemoveAt(i);
						shouldSave = true;
						break;
					}
				}
			}

			if (!shouldSave)
				return shouldSave;

			SaveClientAndroConfig();

			return shouldSave;
		}
		public static SortedSet<int> GetPlayerWhiteListSortedSet(int storageID) => TryGetPlayerWhitelist(storageID, out ItemList whiteList) ? new (whiteList.ItemDefinitions.Select(d => d.Type)) : new();
		public static SortedSet<int> GetPlayerBlackListSortedSet(int storageID) => TryGetPlayerBlacklist(storageID, out ItemList blackList) ? new (blackList.ItemDefinitions.Select(d => d.Type)) : new();
		public static void SaveClientConfig(ModConfig clientConfig) {
			typeof(ConfigManager).GetMethod("Save", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { clientConfig });
		}
		public static void SaveClientAndroConfig() {
			SaveClientConfig(AndroMod.clientConfig);
		}
		public static bool TryQuickStackItemToTile(Item item, Player player, int storageID) {
			if (!ValidModID(storageID))
				return false;

			return BagUIs[storageID].QuickStack(item, player, true, false);
		}

		#endregion

		#region Calls

		public static int RegisterVacuumStorageClass(
				Mod mod,
				Type vacuumStorageType,
				Func<Item, bool> itemAllowedToBeStored,
				string nameLocalizationKey,
				int storageSize,
				bool? isVacuumBag,
				Func<Color> getUIColor,
				Func<Color> getScrollBarColor,
				Func<Color> getButtonHoverColor,
				Func<int> storageItemTypeGetter,
				int uiLeft,
				int uiTop,
				Action<int, bool> updateAllowedList = null,
				bool isBlacklistGetter = false,
				Action selectItemForUIOnly = null,
				bool shouldRefreshInfoAccs = false,
				Func<Item, bool> canVacuumItem = null,
				Func<Player, IList<Item>> extraStorageLocaion = null
			) {
			if (Main.netMode == NetmodeID.Server)
				return 0;

			int storageID = BagUIs.Count;

			int registeredUI_ID = MasterUIManager.RegisterUI_ID();

			Storage storage = new Storage(
				storageID,
				mod, 
				vacuumStorageType, 
				itemAllowedToBeStored, 
				nameLocalizationKey,
				storageSize, 
				isVacuumBag, 
				getUIColor,
				getScrollBarColor,
				getButtonHoverColor,
				new() { storageItemTypeGetter },
				uiLeft, 
				uiTop,
				updateAllowedList,
				isBlacklistGetter,
				selectItemForUIOnly,
				shouldRefreshInfoAccs,
				canVacuumItem,
				extraStorageLocaion
			);

			RegisteredStorages.Add(storage);
			vacuumStorageIndexes.Add(storage.GetModFullName(), storageID);
			BagUI bagUI = new(storageID, registeredUI_ID);

			CanVacuumItemHandler.Add((Item item, Player player) => bagUI.CanVacuumItem(item, player));
			CanBeStoredHandler.Add(bagUI.CanBeStored);
			TryVacuumItemHandler.Add((Item item, Player player) => bagUI.TryVacuumItem(item, player));
			TryRestockItemHandler.Add((Item item) => bagUI.RestockFunc(item, true, false));
			TryQuickStackItemHandler.Add((Item item, Player player) => bagUI.QuickStack(item, player));

			MasterUIManager.IsDisplayingUI.Add(() => bagUI.DisplayBagUI);
			MasterUIManager.DrawAllInterfaces += bagUI.PostDrawInterface;
			MasterUIManager.UpdateInterfaces += bagUI.UpdateInterface;
			MasterUIManager.ShouldPreventRecipeScrolling.Add(() => bagUI.Hovering);

			BagUIs.Add(bagUI);

			return storageID;
		}
		public static void RegisterVacuumStorageClassItemTypeOnly(Func<int> itemTypeGetter, int storageID) {
			RegisteredStorages[storageID].RegisterItemTypeGetter(itemTypeGetter);
		}
		public static Item[] GetItems(int modID) {
			if (!ValidModID(modID))
				return new Item[0];

			return BagUIs[modID].MyStorage.Items;
		}
		public static int GetStorageID(string modName, string bagName) {
			if (vacuumStorageIndexes.TryGetValue($"{modName}_{bagName}", out int storageID))
				return storageID;

			return -1;
		}
		public static void CloseBag(int modID) {
			if (!ValidModID(modID))
				return;

			BagUIs[modID].CloseBag();
		}

		#region Sets

		public static bool SetShouldVacuum(int modID, bool shouldVacuum) {
			if (!ValidModID(modID))
				return false;

			BagUIs[modID].MyStorage.ShouldVacuum = shouldVacuum;

			return true;
		}
		public static bool SetUIPosition(int modID, (int left, int top) newPosition) {
			if (!ValidModID(modID))
				return false;

			BagUIs[modID].MyStorage.UILeft = newPosition.left;
			BagUIs[modID].MyStorage.UITop = newPosition.top;

			return true;
		}

		#endregion

		#endregion

		private static bool ValidModID(int storageID) => storageID >= 0 && storageID < BagUIs.Count;
		public static bool TryGetBagUI(int storageID, out BagUI bagUI) {
			if (!ValidModID(storageID)) {
				bagUI = null;
				return false;
			}

			bagUI = BagUIs[storageID];
			return true;
		}
		public static IEnumerable<Item[]> AllItems => BagUIs.Where(bagUI => bagUI.MyStorage.GetItems == null).Select(bagUI => bagUI.MyStorage.Items);
		private static SortedDictionary<int, List<Action<BagUI>>> BagUIEdits = new();
		public static void AddBagUIEdit(int storageID, Action<BagUI> edit) {
			BagUIEdits.AddOrCombine(storageID, edit);
		}
		public static void PostSetupResipes() {
			if (Main.netMode == NetmodeID.Server)
				return;

			foreach (BagUI bagUI in BagUIs) {
				bagUI.PreSetup();
			}

			foreach (KeyValuePair<int, List<Action<BagUI>>> edits in BagUIEdits) {
				foreach (Action<BagUI> edit in edits.Value) {
					edit(BagUIs[edits.Key]);
				}
			}

			BagUIEdits.Clear();

			foreach (BagUI bagUI in BagUIs) {
				bagUI.PostSetup();
			}

			BagUI.StaticPostSetup();
		}
		public static void TryUpdateMouseOverrideForDeposit(Item item) {
			if (item.IsAir)
				return;

			if (CanVacuumItem(item, Main.LocalPlayer))
				Main.cursorOverride = CursorOverrideID.InventoryToChest;
		}
		public static bool TryReturnItemToPlayer(ref Item item, Player player, bool allowQuickSpawn = false) {
			if (TryVacuumItem(ref item, player)) {
				Recipe.FindRecipes();
				return true;
			}

			item = player.GetItem(player.whoAmI, item, GetItemSettings.InventoryEntityToPlayerInventorySettings);
			if (item.IsAir) {
				Recipe.FindRecipes();
				return true;
			}

			if (!allowQuickSpawn)
				return false;

			player.QuickSpawnItem(player.GetSource_Misc("PlayerDropItemCheck"), item, item.stack);

			return true;
		}
		public static void GiveNewItemToPlayer(int itemType, Player player) {
			Item item = new Item(itemType);
			TryReturnItemToPlayer(ref item, player, true);
		}
		public static void ResetAllBagSizesFromConfig() {
			foreach (BagUI bagUI in BagUIs) {
				bagUI.MyStorage.ResetBagSizeFromConfig();
			}
		}


		public class MagicStorageItemsGatherer
		{
			private event Func<IEnumerable<Item>> eventHandler;

			public void Add(Func<IEnumerable<Item>> func) {
				eventHandler += func;
			}

			public IEnumerable<Item> Invoke() {
				if (eventHandler == null)
					return new List<Item>();

				return eventHandler.GetInvocationList().SelectMany(i => ((Func<IEnumerable<Item>>)i).Invoke());
			}
		}
		public static MagicStorageItemsGatherer MagicStorageItemsHandler = new();
		public static IEnumerable<Item> GetMagicStorageItems => MagicStorageItemsHandler.Invoke();

		public class CanVacuumItemConditions
		{
			private event Func<Item, Player, bool> eventHandler;
			public void Add(Func<Item, Player, bool> func) {
				eventHandler += func;
			}
			public bool Invoke(Item item, Player player) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, Player, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item, player))
						return true;
				}

				return false;
			}
		}
		public static CanVacuumItemConditions CanVacuumItemHandler = new();
		public static bool CanVacuumItem(Item item, Player player) => CanVacuumItemHandler.Invoke(item, player);

		public class CanBeStoredConditions {
			private event Func<Item, bool> eventHandler;
			public void Add(Func<Item, bool> func) {
				eventHandler += func;
			}
			public bool Invoke(Item item) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item))
						return true;
				}

				return false;
			}
		}
		public static CanBeStoredConditions CanBeStoredHandler = new();
		public static bool CanBeStored(Item item) => CanBeStoredHandler.Invoke(item);

		public class TryVacuumItemFunc
		{
			private event Func<Item, Player, bool> eventHandler;
			public void Add(Func<Item, Player, bool> func) {
				eventHandler += func;
			}
			public bool Invoke(ref Item item, Player player) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, Player, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item, player))
						return true;
				}

				return false;
			}
		}
		public static TryVacuumItemFunc TryVacuumItemHandler = new();
		public static bool TryVacuumItem(ref Item item, Player player) => TryVacuumItemHandler.Invoke(ref item, player);
		public class TryRestockItemFunc
		{
			private event Func<Item, bool> eventHandler;
			public void Add(Func<Item, bool> func) {
				eventHandler += func;
			}
			public bool Invoke(ref Item item) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item) && (item.NullOrAir() || item.stack <= 0))
						return true;
				}

				return false;
			}
		}
		public static TryRestockItemFunc TryRestockItemHandler = new();
		public static bool TryRestock(ref Item item) => TryRestockItemHandler.Invoke(ref item);

		public class TryQuickStackItemFunc
		{
			private event Func<Item, Player, bool> eventHandler;
			public void Add(Func<Item, Player, bool> func) {
				eventHandler += func;
			}
			public bool Invoke(ref Item item, Player player) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, Player, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item, player) && (item.NullOrAir() || item.stack <= 0))
						return true;
				}

				return false;
			}
		}
		public static TryQuickStackItemFunc TryQuickStackItemHandler = new();
		public static bool TryQuickStack(ref Item item, Player player) => TryQuickStackItemHandler.Invoke(ref item, player);

		public static event Action OnOpenMagicStorageCloseAllStorageUIEvent;
		public static void OnOpenMagicStorageCloseAllStorageUI() {
			OnOpenMagicStorageCloseAllStorageUIEvent?.Invoke();
		}
	}
}
