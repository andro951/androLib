using androLib.UI;
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
		public static int DefaultResizePanelIncrement = 100000;
		public static int UIResizePanelDefaultX = DefaultResizePanelIncrement * 10;
		public static int UIResizePanelDefaultY = DefaultResizePanelIncrement * 3;
		public int LastUIResizePanelDefaultX;
		public int LastUIResizePanelDefaultY;
		public Item[] Items;
		public int RegisteredUI_ID { get; }
		public bool DisplayBagUI = false;
		public Action SelectItemForUIOnly { get; }
		public bool ShouldRefreshInfoAccs { get; }
		private string modFullName;
		private int myLastBagLocation = -1;
		private int myLastIndexInTheBag = -1;
		private int foundBagType = -1;
		private Func<SortedSet<int>> GetAllowedList;
		private bool IsBlacklistGetter;
		uint nextBagCheck = 0;
		private SortedDictionary<int, int> ItemsIHaveThisTick = new();
		public int SwitcherStorageID;
		public bool ShouldDepositToMagicStorage = false;

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
				Func<SortedSet<int>> getAllowedList,
				bool isBlackListGetter,
				Action selectItemForUIOnly,
				bool shouldRefreshInfoAccs
			) {
			StorageID = storageID;
			Mod = mod;
			VacuumStorageType = vacuumStorageType;
			modFullName = DefaultModFullName();
			ItemAllowedToBeStored = itemAllowedToBeStored;
			NameLocalizationKey = nameLocalizationKey;
			StorageSize = GetBagSize(storageSize);
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
			GetAllowedList = getAllowedList;
			IsBlacklistGetter = isBlackListGetter;
			SelectItemForUIOnly = selectItemForUIOnly;
			ShouldRefreshInfoAccs = shouldRefreshInfoAccs;
			Items = Enumerable.Repeat(new Item(), StorageSize).ToArray();
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
			tag[$"{modFullName}{ItemsTag}"] = Items;
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
			int itemCount = StorageSize;
			string modFullName = GetModFullName();
			if (!tag.TryGet($"{modFullName}{ItemsTag}", out Item[] items))
				items = Enumerable.Repeat(new Item(), itemCount).ToArray();

			if (items.Length < itemCount)
				items = items.Concat(Enumerable.Repeat(new Item(), itemCount - items.Length)).ToArray();

			//Mod != null is checking if the bag is loaded the correct way instead of unloaded.
			if (Mod != null && items.Length > itemCount)
				TryShiftDownAndReduceToMaxSize(ref items, itemCount);

			bool temp = !items.Where(i => !i.NullOrAir()).Any();
			Items = items;
			
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
			}
			else {
				SwitcherStorageID = StorageID;
			}

			ShouldDepositToMagicStorage = tag.Get<bool>($"{modFullName}{ShouldDepositToMagicStorageTag}");
		}
		private void TryShiftDownAndReduceToMaxSize(ref Item[] items, int itemCount) {
			IEnumerable<Item> nonAirItems = items.Where(item => !item.NullOrAir());
			int nonAirItemCount = nonAirItems.Count();
			if (nonAirItemCount >= itemCount) {
				items = nonAirItems.ToArray();
				return;
			}
			else if (nonAirItemCount < 1) {
				items = Enumerable.Repeat(new Item(), itemCount).ToArray();
				return;
			}

			int allowedOpenSlots = itemCount - nonAirItemCount;
			int index = 0;
			int airCount = 0;
			while (airCount <= allowedOpenSlots && index <= itemCount) {
				if (items[index].NullOrAir())
					airCount++;

				index++;
			}

			items = items.Take(index - 1).Concat(nonAirItems.Reverse().Take(itemCount -(index - 1)).Reverse()).ToArray();
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
				StorageSize,
				IsVacuumBag,
				GetUIColor,
				GetScrollBarColor,
				GetButtonHoverColor,
				StorageItemTypeGetters,
				UILeftDefault,
				UITopDefault,
				GetAllowedList,
				IsBlacklistGetter,
				SelectItemForUIOnly,
				ShouldRefreshInfoAccs
			);

			clone.UILeft = UILeft;
			clone.UITop = UITop;
			clone.Items = new Item[StorageSize];
			Array.Copy(Items, clone.Items, StorageSize);
			clone.ShouldVacuum = ShouldVacuum;
			clone.UIResizePanelX = UIResizePanelX;
			clone.UIResizePanelY = UIResizePanelY;
			clone.LastUIResizePanelX = LastUIResizePanelX;
			clone.LastUIResizePanelY = LastUIResizePanelY;
			clone.SwitcherStorageID = SwitcherStorageID;
			clone.ShouldDepositToMagicStorage = ShouldDepositToMagicStorage;

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
		public static readonly int RequiredItemNotFound = -1;
		public static readonly int ReuiredItemInABagStartingIndex = -2;
		public static Item GetItemFromHasRequiredItemToUseStorageIndex(Player player, int lastBagUIFoundIn, int hasRequiredItemToUseStorageIndex) {
			if (hasRequiredItemToUseStorageIndex > RequiredItemNotFound)
				return player.inventory[hasRequiredItemToUseStorageIndex];

			if (lastBagUIFoundIn < 0)
				return null;

			int index = ItemsIndexFromHasRequiredItemToUseStorageIndex(hasRequiredItemToUseStorageIndex);
			if (index < 0)
				return null;

			return StoragePlayer.LocalStoragePlayer.Storages[lastBagUIFoundIn].Items[index];
		}
		public static int ItemsIndexFromHasRequiredItemToUseStorageIndex(int hasRequiredItemToUseStorageIndex) => -(hasRequiredItemToUseStorageIndex - ReuiredItemInABagStartingIndex);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index">The index in the players inventory.  If found in a bag, the index will be -index + ReuiredItemInABagStartingIndex.<\br>
		/// Use GetItemFromHasRequiredItemToUseStorageIndex() to get the item savely.</param>
		/// <returns></returns>
		public bool HasRequiredItemToUseStorage(Player player, out int bagFoundInID, out int index, int specifiedBagType = -1) {
			bool foundResult = true;
			bool useSpecifiedBagType = specifiedBagType != -1;
			if (foundBagType == -1 || useSpecifiedBagType && foundBagType != specifiedBagType)
				foundResult = false;

			if (foundResult) {
				Item lastBagLocationItem = GetItemFromHasRequiredItemToUseStorageIndex(player, myLastBagLocation, myLastIndexInTheBag);
				if (foundBagType == lastBagLocationItem?.type) {
					bool inPlayerInventory = myLastIndexInTheBag > RequiredItemNotFound;
					if (inPlayerInventory || StorageManager.VacuumStorageIndexesFromBagTypes.TryGetValue(foundBagType, out int bagID) && StorageManager.BagUIs[bagID].DisplayBagUI || myLastBagLocation > -1 && StorageManager.BagUIs[myLastBagLocation].DisplayBagUI) {
						bagFoundInID = myLastBagLocation;
						index = myLastIndexInTheBag;
						return true;
					}
				}
			}

			//Only look once per tick
			if (nextBagCheck > Main.GameUpdateCount) {
				bagFoundInID = -1;
				index = RequiredItemNotFound;
				return false;
			}
			else {
				nextBagCheck = Main.GameUpdateCount + 1u;
			}

			index = RequiredItemNotFound;
			bagFoundInID = -1;
			SortedSet<int> bagItemTypes;
			if (specifiedBagType != -1) {
				bagItemTypes = new() { specifiedBagType };
			}
			else {
				bool? validItemType = ValidItemTypeGetters(out bagItemTypes);
				if (validItemType != true)
					return validItemType == null;//null means no associated item.  false means the bag type was not in the valid range and not default -1.
			}
			
			//Check player inventory for this bag.
			for (int i = 0; i < player.inventory.Length; i++) {
				Item item = player.inventory[i];
				if (bagItemTypes.Contains(item.type)) {
					index = i;
					myLastBagLocation = bagFoundInID;
					myLastIndexInTheBag = index;
					foundBagType = item.type;
					return true;
				}
			}

			//Check all other bags for this bag.
			foreach (int bagType in bagItemTypes) {
				Item bagItem = bagType.CSI();
				for (int j = 0; j < StorageManager.BagUIs.Count; j++) {
					BagUI bagUI = StorageManager.BagUIs[j];
					if (!bagUI.DisplayBagUI || !bagUI.CanBeStored(bagItem))
						continue;

					if (bagUI.MyStorage.ContainsSlow(bagType, out int myLastIndex)) {
						index = -myLastIndex + ReuiredItemInABagStartingIndex;
						bagFoundInID = j;
						myLastBagLocation = bagFoundInID;
						myLastIndexInTheBag = index;
						foundBagType = bagType;
						return true;
					}
				}
			}

			foreach (int bagItemType in bagItemTypes) {
				//Check adj tiles for this bag.
				int createTile = ContentSamples.ItemsByType[bagItemType].createTile;
				if (createTile > -1 && player.adjTile[createTile]) {
					if (!useSpecifiedBagType || !found) {
						myLastBagLocation = -1;
						myLastIndexInTheBag = RequiredItemNotFound;
					}
					
					return true;
				}
			}

			if (!useSpecifiedBagType) {
				myLastBagLocation = -1;
				myLastIndexInTheBag = RequiredItemNotFound;
			}

			return false;
		}
		private uint GetDelay(bool found) => found ? 30u : 10u;//It can wait a little longer to stop having a bag.
		public bool HasRequiredItemToUseStorageSlow(Player player) {
			if (Main.GameUpdateCount < nextBagItemCheckTime && Main.GameUpdateCount != nextBagItemCheckTime - GetDelay(found))
				return found;

			nextBagItemCheckTime = Main.GameUpdateCount + GetDelay(found);
			found = HasRequiredItemToUseStorage(player, out _, out _);
			return found;
		}
		/// <summary>
		/// Don't forget to check the type of the found item if using this method since it only checks every 10-30 ticks if that amount if time is important.
		/// </summary>
		public bool HasRequiredItemToUseStorageSlow(Player player, out int bagFoundInID, out int index, int specifiedBagType = -1) {
			if (specifiedBagType != -1) {
				bool check = Main.GameUpdateCount < nextBagItemCheckTime || found && specifiedBagType != foundBagType && Main.GameUpdateCount == nextBagItemCheckTime - GetDelay(found) + 1;
				if (!check) {
					bagFoundInID = myLastBagLocation;
					index = myLastIndexInTheBag;
					return specificBagTypesFound.Contains(specifiedBagType); ;
				}

				specificBagTypesFound.Remove(specifiedBagType);

				nextBagItemCheckTime = Main.GameUpdateCount + GetDelay(found);
				bool foundExact = HasRequiredItemToUseStorage(player, out bagFoundInID, out index, specifiedBagType);
				if (foundExact)
					specificBagTypesFound.Add(specifiedBagType);

				if (foundExact || specifiedBagType == foundBagType)
					found = foundExact;

				return specificBagTypesFound.Contains(specifiedBagType);
			}

			if (Main.GameUpdateCount < nextBagItemCheckTime) {
				bagFoundInID = myLastBagLocation;
				index = myLastIndexInTheBag;
				return found;
			}

			specificBagTypesFound.Clear();

			nextBagItemCheckTime = Main.GameUpdateCount + GetDelay(found);
			found = HasRequiredItemToUseStorage(player, out bagFoundInID, out index, specifiedBagType);

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

			SortedSet<int> allowedList = GetAllowedList();
			if (IsBlacklistGetter) {
				allowedList.Remove(type);
			}
			else {
				allowedList.Add(type);
			}

			return true;
		}
		public bool TryAddToPlayerBlacklist(int type) {
			if (!HasWhiteOreBlacklistGetter)
				return false;

			SortedSet<int> allowedList = GetAllowedList();
			if (IsBlacklistGetter) {
				allowedList.Add(type);
			}
			else {
				allowedList.Remove(type);
			}

			return true;
		}
		public bool HasWhiteListGetter => !IsBlacklistGetter && HasWhiteOreBlacklistGetter;
		public bool HasWhiteOreBlacklistGetter => GetAllowedList != null;
		private int GetBagSize(int bagSize) {
			if (bagSize >= 0)
				return bagSize;

			bagSize = -bagSize;

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
		public override string ToString() {
			return GetModFullName();
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
		public static bool HasRequiredItemToUseStorageFromBagType(Player player, int bagType, out int bagInventoryIndex, bool onlyThisBagType = false) {
			if (VacuumStorageIndexesFromBagTypes.TryGetValue(bagType, out int storageID)) {
				Storage storage = BagUIs[storageID].MyStorage;
				if (storage.HasRequiredItemToUseStorage(player, out _, out bagInventoryIndex, onlyThisBagType ? bagType : -1))
					return true;
			}

			bagInventoryIndex = Storage.RequiredItemNotFound;
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
		public static bool HasRequiredItemToUseStorageFromBagTypeSlow(Player player, int bagType, out int bagInventoryIndex, bool onlyThisBagType = false) {
			if (VacuumStorageIndexesFromBagTypes.TryGetValue(bagType, out int storageID)) {
				Storage storage = BagUIs[storageID].MyStorage;
				if (storage.HasRequiredItemToUseStorageSlow(player, out _, out bagInventoryIndex, onlyThisBagType ? bagType : -1))
					return true;
			}

			bagInventoryIndex = Storage.RequiredItemNotFound;
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
		public static void AddToPlayerWhitelist(int storageID, int itemType) {
			if (TryGetPlayerWhitelist(storageID, out ItemList whiteList)) {
				for (int i = 0; i < whiteList.ItemDefinitions.Count; i++) {
					if (whiteList.ItemDefinitions[i].Type == itemType)
						break;
				}

				whiteList.ItemDefinitions.Add(new(itemType));
			}

			if (RegisteredStorages[storageID].ValidItemTypeGetters(out SortedSet<int> bagItemTypes) == true) {
				string bagName = bagItemTypes.First().CSI().Name;
				Main.NewText(AndroLibGameMessages.AddedToWhitelist.ToString().Lang(AndroMod.ModName, L_ID1.AndroLibGameMessages, new object[] { itemType.CSI().Name, bagName }));
			}

			TryRemoveFromPlayerBlacklist(storageID, itemType, true);
		}
		public static void AddToPlayerBlacklist(int storageID, int itemType) {
			if (TryGetPlayerBlacklist(storageID, out ItemList blackList)) {
				for (int i = 0; i < blackList.ItemDefinitions.Count; i++) {
					if (blackList.ItemDefinitions[i].Type == itemType)
						break;
				}

				blackList.ItemDefinitions.Add(new(itemType));
			}

			if (RegisteredStorages[storageID].ValidItemTypeGetters(out SortedSet<int> bagItemTypes) == true) {
				string bagName = bagItemTypes.First().CSI().Name;
				Main.NewText(AndroLibGameMessages.AddedToBlacklist.ToString().Lang(AndroMod.ModName, L_ID1.AndroLibGameMessages, new object[] { itemType.CSI().Name, bagName }));
			}

			TryRemoveFromPlayerWhitelist(storageID, itemType, true);
		}
		public static void TryRemoveFromPlayerWhitelist(int storageID, int itemType, bool forseSave = false) {
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
				return;

			SaveClientAndroConfig();
		}
		public static void TryRemoveFromPlayerBlacklist(int storageID, int itemType, bool forseSave = false) {
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
				return;

			SaveClientAndroConfig();
		}
		public static SortedSet<int> GetPlayerWhiteListSortedSet(int storageID) => TryGetPlayerWhitelist(storageID, out ItemList whiteList) ? new (whiteList.ItemDefinitions.Select(d => d.Type)) : new();
		public static SortedSet<int> GetPlayerBlackListSortedSet(int storageID) => TryGetPlayerBlacklist(storageID, out ItemList blackList) ? new (blackList.ItemDefinitions.Select(d => d.Type)) : new();
		public static void SaveClientConfig(ModConfig clientConfig) {
			typeof(ConfigManager).GetMethod("Save", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { clientConfig });
		}
		public static void SaveClientAndroConfig() {
			SaveClientConfig(AndroMod.clientConfig);
		}
		public static bool TryQuickStackItemToTile(ref Item item, Player player, int storageID) {
			if (!ValidModID(storageID))
				return false;

			return BagUIs[storageID].QuickStack(ref item, player, true, false);
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
				Func<SortedSet<int>> getAllowedList = null,
				bool isBlacklistGetter = false,
				Action selectItemForUIOnly = null,
				bool shouldRefreshInfoAccs = false
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
				getAllowedList,
				isBlacklistGetter,
				selectItemForUIOnly,
				shouldRefreshInfoAccs
			);

			RegisteredStorages.Add(storage);
			vacuumStorageIndexes.Add(storage.GetModFullName(), storageID);
			BagUI bagUI = new(storageID, registeredUI_ID);

			CanVacuumItemHandler.Add((Item item, Player player) => bagUI.CanVacuumItem(item, player));
			TryVacuumItemHandler.Add((Item item, Player player) => bagUI.TryVacuumItem(ref item, player));
			TryRestockItemHandler.Add((Item item) => bagUI.Restock(ref item));
			TryQuickStackItemHandler.Add((Item item, Player player) => bagUI.QuickStack(ref item, player));

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
		public static IEnumerable<Item[]> AllItems => BagUIs.Select(bagUI => bagUI.MyStorage.Items);
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
			if (TryVacuumItem(ref item, player))
				return true;

			item = player.GetItem(player.whoAmI, item, GetItemSettings.InventoryEntityToPlayerInventorySettings);
			if (item.IsAir)
				return true;

			if (!allowQuickSpawn)
				return false;

			player.QuickSpawnItem(player.GetSource_Misc("PlayerDropItemCheck"), item, item.stack);

			return true;
		}
		public static void GiveNewItemToPlayer(int itemType, Player player) {
			Item item = new Item(itemType);
			TryReturnItemToPlayer(ref item, player, true);
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
