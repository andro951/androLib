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
		private Func<int> StorageItemTypeGetter { get; set; }
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
				Func<int> storageItemTypeGetter,
				int uiLeftDefault, 
				int uiTopDefault
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
			StorageItemTypeGetter = storageItemTypeGetter;
			UILeftDefault = uiLeftDefault;
			UITopDefault = uiTopDefault;
			UILeft = UILeftDefault;
			UITop = UITopDefault;
			Items = Enumerable.Repeat(new Item(), StorageSize).ToArray();
			ShouldVacuum = IsVacuumBag != false;
		}

		public string GetModFullName() => $"{Mod.Name}_{VacuumStorageType.Name}";
		public void SaveData(TagCompound tag) {
			string modFullName = GetModFullName();
			tag[$"{modFullName}_Items"] = Items;
			tag[$"{modFullName}_UILeft"] = UILeft;
			tag[$"{modFullName}_UITop"] = UITop;
			tag[$"{modFullName}_ShouldVacuumItems"] = ShouldVacuum;
		}
		public void LoadData(TagCompound tag) {
			int itemCount = StorageSize;
			string modFullName = GetModFullName();
			if (!tag.TryGet($"{modFullName}_Items", out Item[] items))
				items = Enumerable.Repeat(new Item(), itemCount).ToArray();

			if (items.Length < itemCount)
				items = items.Concat(Enumerable.Repeat(new Item(), itemCount - items.Length)).ToArray();

			Items = items;
			
			int uiLeft = tag.Get<int>($"{modFullName}_UILeft");
			int uiTop = tag.Get<int>($"{modFullName}_UITop");
			MasterUIManager.CheckOutOfBoundsRestoreDefaultPosition(ref uiLeft, ref uiTop, UILeftDefault, UITopDefault);
			UILeft = uiLeft;
			UITop = uiTop;

			ShouldVacuum = tag.Get<bool>($"{modFullName}_ShouldVacuumItems");
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
				StorageItemTypeGetter,
				UILeftDefault,
				UITopDefault
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
		public bool? ValidItemTypeGetter(out int itemType) {
			itemType = StorageItemTypeGetter();
			if (itemType == -1)
				return null;

			return itemType >= 0 && itemType < ItemLoader.ItemCount;
		}
		public bool HasRequiredItemToUseStorage(Player player) {
			bool? validItemType = ValidItemTypeGetter(out int bagItemType);
			if (validItemType != null) {//-1 = null, No associated item
				if (validItemType != true)//false, Not in valid range and not default -1.
					return false;

				if (!player.HasItem(bagItemType))
					return false;
			}

			return true;
		}
	}
	public static class StorageManager {
		public static int DefaultLeftLocationOnScreen => 80;
		public static int DefaultTopLocationOnScreen => 675;
		public static int DefaultStorageSize => 100;

		#region Lists and Dictionaries

		private static SortedDictionary<string, int> vacuumStorageIndexes = new();
		public static List<BagUI> BagUIs = new();
		public static List<Storage> RegisteredStorages = new();
		public static void PopulateStorages(ref List<Storage> storages) {
			storages = new();
			for (int i = 0; i < RegisteredStorages.Count; i++) {
				Storage storage = RegisteredStorages[i].Clone();
				storages.Add(storage);
			}
		}

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
		private static List<(Func<int>, int)> otherStorageItemTypeOnly = new();
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
				if (BagUIs[i].Storage.ValidItemTypeGetter(out int itemType) == true)
					storageItemTypes.Add(itemType, i);
			}

			foreach ((Func<int> itemTypeGetter, int storageID) in otherStorageItemTypeOnly) {
				storageItemTypes.Add(itemTypeGetter(), storageID);
			}

			otherStorageItemTypeOnly.Clear();

			storageTileTypes = new();
			foreach (int itemType in storageItemTypes.Keys) {
				int createTile = ContentSamples.ItemsByType[itemType].createTile;
				if (createTile > ItemID.None)
					storageTileTypes.Add(createTile, storageItemTypes[itemType]);
			}
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
				int uiTop
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
				storageItemTypeGetter,
				uiLeft, 
				uiTop
			);

			RegisteredStorages.Add(storage);
			vacuumStorageIndexes.Add(storage.GetModFullName(), storageID);
			BagUI bagUI = new(storageID, registeredUI_ID);

			CanVacuumItemHandler.Add((Item item, Player player) => bagUI.CanVacuumItem(item, player));
			TryVacuumItemHandler.Add((Item item, Player player) => bagUI.TryVacuumItem(ref item, player));
			TryQuickStackItemHandler.Add((Item item) => bagUI.QuickStack(ref item));
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
			otherStorageItemTypeOnly.Add((itemTypeGetter, storageID));
		}
		public static Item[] GetItems(int modID) {
			if (!ValidModID(modID))
				return null;

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
		public static bool TryVacuumItemToTile(ref Item item, Player player, int storageID) {
			if (!ValidModID(storageID))
				return false;

			return BagUIs[storageID].TryVacuumItem(ref item, player, true);
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
		public class TryQuickStackItemFunc
		{
			private event Func<Item, bool> eventHandler;
			public void Add(Func<Item, bool> func) {
				eventHandler += func;
			}
			public bool Invoke(ref Item item) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item) && item.NullOrAir() || item.stack <= 0)
						return true;
				}

				return false;
			}
		}
		public static TryQuickStackItemFunc TryQuickStackItemHandler = new();
		public static bool TryQuickStack(ref Item item) => TryQuickStackItemHandler.Invoke(ref item);

		public static event Action CloseAllStorageUIEvent;
		public static void CloseAllStorageUI() {
			CloseAllStorageUIEvent?.Invoke();
		}
	}
}
