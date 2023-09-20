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

namespace androLib
{

	public class Storage {
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
		public Item[] Items;
		public int RegisteredUI_ID { get; }
		public bool DisplayBagUI = false;
		public Action SelectItemForUIOnly { get; }
		public bool ShouldRefreshInfoAccs { get; }
		private string modFullName;

		public Storage(
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
				Action selectItemForUIOnly,
				bool shouldRefreshInfoAccs
			) {
			Mod = mod;
			VacuumStorageType = vacuumStorageType;
			ItemAllowedToBeStored = itemAllowedToBeStored;
			NameLocalizationKey = nameLocalizationKey;
			StorageSize = storageSize;
			IsVacuumBag = isVacuumBag;
			GetUIColor = getUIColor;
			GetScrollBarColor = getScrollBarColor;
			GetButtonHoverColor = getButtonHoverColor;
			StorageItemTypeGetters = storageItemTypeGetters;
			UILeftDefault = uiLeftDefault;
			UITopDefault = uiTopDefault;
			UILeft = UILeftDefault;
			UITop = UITopDefault;
			SelectItemForUIOnly = selectItemForUIOnly;
			ShouldRefreshInfoAccs = shouldRefreshInfoAccs;
			Items = Enumerable.Repeat(new Item(), StorageSize).ToArray();
			ShouldVacuum = IsVacuumBag != false;
			modFullName = DefaultModFullName();
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
		public void SaveData(TagCompound tag) {
			string modFullName = GetModFullName();
			tag[$"{modFullName}{ItemsTag}"] = Items;
			tag[$"{modFullName}{UILeftTag}"] = UILeft;
			tag[$"{modFullName}{UITopTag}"] = UITop;
			tag[$"{modFullName}{ShouldVacuumItemsTag}"] = ShouldVacuum; 
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
				SelectItemForUIOnly,
				ShouldRefreshInfoAccs
			);

			clone.UILeft = UILeft;
			clone.UITop = UITop;
			clone.Items = new Item[StorageSize];
			Array.Copy(Items, clone.Items, StorageSize);
			ShouldVacuum = IsVacuumBag != false;

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
		public Item GetItemFromHasRequiredItemToUseStorageIndex(Player player, int hasRequiredItemToUseStorageIndex) {
			if (hasRequiredItemToUseStorageIndex > RequiredItemNotFound)
				return player.inventory[hasRequiredItemToUseStorageIndex];

			int index = ItemsIndexFromHasRequiredItemToUseStorageIndex(hasRequiredItemToUseStorageIndex);
			return Items[index];
		}
		public static int ItemsIndexFromHasRequiredItemToUseStorageIndex(int hasRequiredItemToUseStorageIndex) => -(hasRequiredItemToUseStorageIndex - ReuiredItemInABagStartingIndex);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index">The index in the players inventory.  If found in a bag, the index will be -index + ReuiredItemInABagStartingIndex.<\br>
		/// Use GetItemFromHasRequiredItemToUseStorageIndex() to get the item savely.</param>
		/// <returns></returns>
		public bool HasRequiredItemToUseStorage(Player player, out int bagFoundInID, out int index) {
			index = RequiredItemNotFound;
			bagFoundInID = -1;
			bool? validItemType = ValidItemTypeGetters(out SortedSet<int> bagItemTypes);
			if (validItemType != true)
				return validItemType == null;//null means no associated item.  false means the bag type was not in the valid range and not default -1.

			//Check player inventory for this bag.
			for (int i = 0; i < player.inventory.Length; i++) {
				if (bagItemTypes.Contains(player.inventory[i].type)) {
					index = i;
					return true;
				}
			}

			//Check all other bags for this bag.
			for (int j = 0; j < StorageManager.BagUIs.Count; j++) {
				BagUI bagUI = StorageManager.BagUIs[j];
				if (!bagUI.DisplayBagUI)
					continue;

				for (int i = 0; i < bagUI.Storage.Items.Length; i++) {
					if (bagItemTypes.Contains(bagUI.Storage.Items[i].type)) {
						index = -i + ReuiredItemInABagStartingIndex;
						bagFoundInID = j;
						return true;
					}
				}
			}

			foreach (int bagItemType in bagItemTypes) {
				//Check adj tiles for this bag.
				int createTile = ContentSamples.ItemsByType[bagItemType].createTile;
				if (createTile > -1 && player.adjTile[createTile])
					return true;

				//Check if UI is open
				if (StorageManager.VacuumStorageIndexesFromBagTypes.TryGetValue(bagItemType, out int bagID) && StorageManager.BagUIs[bagID].DisplayBagUI)
					return true;
			}

			return false;
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
			foreach (BagUI bagUI in BagUIs) {
				Storage storage = bagUI.Storage;
				if (storage.ValidItemTypeGetters(out SortedSet<int> bagTypes) != true)
					continue;

				foreach (int bagType in bagTypes) {
					vacuumStorageIndexesFromBagTypes.Add(bagType, bagUI.StorageID);
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
		public static bool HasRequiredItemToUseStorageFromBagType(Player player, int bagType, out int bagInventoryIndex) {
			if (VacuumStorageIndexesFromBagTypes.TryGetValue(bagType, out int storageID)) {
				Storage storage = BagUIs[storageID].Storage;
				if (storage.HasRequiredItemToUseStorage(Main.LocalPlayer, out _, out bagInventoryIndex))
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
				if (BagUIs[i].Storage.ValidItemTypeGetters(out SortedSet<int> itemTypes) == true) {
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
				Action selectItemForUIOnly = null,
				bool shouldRefreshInfoAccs = false
			) {
			int storageID = BagUIs.Count;

			int registeredUI_ID = MasterUIManager.RegisterUI_ID();

			Storage storage = new Storage(
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
			CloseAllStorageUIEvent += () => {
				if (bagUI.DisplayBagUI)
					bagUI.CloseBag();
			};

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

			return BagUIs[modID].Storage.Items;
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

			BagUIs[modID].Storage.ShouldVacuum = shouldVacuum;

			return true;
		}
		public static bool SetUIPosition(int modID, (int left, int top) newPosition) {
			if (!ValidModID(modID))
				return false;

			BagUIs[modID].Storage.UILeft = newPosition.left;
			BagUIs[modID].Storage.UITop = newPosition.top;

			return true;
		}

		#endregion

		#endregion

		private static bool ValidModID(int modID) => modID >= 0 && modID < BagUIs.Count;
		public static bool TryGetBagUI(int modID, out BagUI bagUI) {
			if (!ValidModID(modID)) {
				bagUI = null;
				return false;
			}

			bagUI = BagUIs[modID];
			return true;
		}
		public static IEnumerable<Item[]> AllItems => BagUIs.Select(bagUI => bagUI.Storage.Items);
		


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

		public static event Action CloseAllStorageUIEvent;
		public static void CloseAllStorageUI() {
			CloseAllStorageUIEvent?.Invoke();
		}
	}
}
