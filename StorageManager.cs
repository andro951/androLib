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

namespace androLib
{

	public class Storage {
		public Mod Mod { get; set; }
		public Type VacuumStorageType { get; set; }
		public Func<Item, bool> ItemAllowedToBeStored { get; set; }
		public string NameLocalizationKey { get; set; }
		public int StorageSize { get; set; }
		public bool ShouldVacuum { get; set; }
		private Func<int> StorageItemTypeGetter { get; set; }
		public Func<Color> GetUIColor { get; set; }
		public Func<Color> GetScrollBarColor { get; set; }
		public Func<Color> GetButtonHoverColor { get; set; }
		public int UILeftDefault { get; }
		public int UITopDefault { get; }
		public int UILeft;
		public int UITop;
		public Item[] Items;

		public Storage(
				Mod mod, 
				Type vacuumStorageType, 
				Func<Item, bool> itemAllowedToBeStored, 
				string nameLocalizationKey,
				int storageSize, 
				bool shouldVacuum, 
				Func<int> storageItemTypeGetter, 
				Func<Color> getUIColor,
				Func<Color> getScrollBarColor,
				Func<Color> getButtonHoverColor,
				int uiLeftDefault, 
				int uiTopDefault
			) {
			Mod = mod;
			VacuumStorageType = vacuumStorageType;
			ItemAllowedToBeStored = itemAllowedToBeStored;
			NameLocalizationKey = nameLocalizationKey;
			StorageSize = storageSize;
			ShouldVacuum = shouldVacuum;
			StorageItemTypeGetter = storageItemTypeGetter;
			GetUIColor = getUIColor;
			GetScrollBarColor = getScrollBarColor;
			GetButtonHoverColor = getButtonHoverColor;
			UILeftDefault = uiLeftDefault;
			UITopDefault = uiTopDefault;
			UILeft = UILeftDefault;
			UITop = UITopDefault;
			Items = new Item[storageSize];
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
		public static List<BagUI> BagUIs { get; } = new();

		public static SortedDictionary<int, int> StorageItemTypes {
			get {
				if (storageItemTypes == null) {
					storageItemTypes = new();
					for (int i = 0; i < BagUIs.Count; i++) {
						if (BagUIs[i].Storage.ValidItemTypeGetter(out int itemType) == true)
							storageItemTypes.Add(itemType, i);
					}
				}

				return storageItemTypes;
			}
		}
		private static SortedDictionary<int, int> storageItemTypes = null;

		#endregion

		#region Calls

		public static int RegisterVacuumStorageClass(
				Mod mod, 
				Type vacuumStorageType, 
				Func<Item, bool> itemAllowedToBeStored,
				string nameLocalizationKey, 
				int storageSize, 
				bool shouldVacuum,
				Func<int> storageItemTypeGetter, 
				Func<Color> getUIColor,
				Func<Color> getScrollBarColor,
				Func<Color> getButtonHoverColor,
				int uiLeft, 
				int uiTop
			) {
			Storage storage = new Storage(
				mod, 
				vacuumStorageType, 
				itemAllowedToBeStored, 
				nameLocalizationKey,
				storageSize, 
				shouldVacuum, 
				storageItemTypeGetter, 
				getUIColor,
				getScrollBarColor,
				getButtonHoverColor,
				uiLeft, 
				uiTop
			);

			int registeredUI_ID = MasterUIManager.RegisterUI_ID();
			BagUI bagUI = new BagUI(storage, registeredUI_ID);
			BagUIs.Add(bagUI);

			vacuumStorageIndexes.Add(storage.GetModFullName(), BagUIs.Count - 1);
			CanVacuumItemHandler.Add(bagUI.CanVacuumItem);
			TryVacuumItemHandler.Add((Item item, Player player) => bagUI.TryVacuumItem(ref item, player));
			//TryReturnItemToPlayerHandler.Add((Item item, Player player) => bagUI.TryVacuumItem(ref item, player));
			CloseAllStorageUIEvent += () => {
				if (bagUI.DisplayBagUI)
					bagUI.CloseBag();
			};

			MasterUIManager.IsDisplayingUI.Add(() => bagUI.DisplayBagUI);
			MasterUIManager.DrawAllInterfaces += bagUI.PostDrawInterface;
			MasterUIManager.ShouldPreventRecipeScrolling.Add(() => bagUI.Hovering);
			

			/*
				MasterUIManager.IsDisplayingUI.Add(() => WEPlayer.LocalWEPlayer.displayOreBagUI);//OreBag-Delete
			MasterUIManager.ShouldPreventTrashingItem.Add(() => OreBagUI.CanBeStored(Main.HoverItem));//OreBag-Delete
				MasterUIManager.DrawAllInterfaces += OreBagUI.PostDrawInterface;//OreBag-Delete
				MasterUIManager.ShouldPreventRecipeScrolling.Add(() => MasterUIManager.HoveringMyUIType(WE_UI_ID.OreBag_UITypeID));//OreBag-Delete
			StorageManager.AllowedToStoreInStorage.Add(OreBagUI.CanBeStored);//OreBag-Delete
				StorageManager.TryVacuumItemHandler.Add((Item item, Player player) => OreBagUI.TryVacuumItem(ref item, player));//OreBag-Delete
				StorageManager.CanVacuumItemHandler.Add(OreBagUI.CanVacuumItem);
			StorageManager.TryReturnItemToPlayerHandler.Add((Item item, Player player) => OreBagUI.TryVacuumItem(ref item, player));
				StorageManager.CloseAllStorageUIEvent += () => {
					if (WEPlayer.LocalWEPlayer.displayOreBagUI)
						OreBagUI.CloseOreBag();
				};
			*/

			return BagUIs.Count - 1;
		}
		public static Item[] GetItems(int modID) {
			if (!ValidModID(modID))
				return null;

			return BagUIs[modID].Storage.Items;
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
		public class MagicStorageItemsGatherer {
			private event Func<IEnumerable<Item>> eventHandler;

			public void Add(Func<IEnumerable<Item>> func) {
				eventHandler += func;
			}

			public IEnumerable<Item> Invoke() {
				//IEnumerable<Item> items = new List<Item>();

				if (eventHandler == null)
					return new List<Item>();

				return eventHandler.GetInvocationList().SelectMany(i => ((Func<IEnumerable<Item>>)i).Invoke());

				//foreach (Func<IEnumerable<Item>> func in eventHandler.GetInvocationList()) {
				//	items = items.Concat(func.Invoke()).ToList();
				//}

				//return ref items;
			}
		}
		public static MagicStorageItemsGatherer MagicStorageItemsHandler = new();
		public static IEnumerable<Item> GetMagicStorageItems => MagicStorageItemsHandler.Invoke();
		public static void SaveData(TagCompound tag) {
			for (int i = 0; i < BagUIs.Count; i++) {
				BagUIs[i].Storage.SaveData(tag);
			}
		}
		public static void LoadData(TagCompound tag) {
			for(int i = 0; i < BagUIs.Count; i++) {
				BagUIs[i].Storage.LoadData(tag);
			}
		}




		public class AllowedToStoreInStorageConditions {
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
		public static AllowedToStoreInStorageConditions AllowedToStoreInStorage = new();
		public static bool CanBeStored(Item item) {
			return AllowedToStoreInStorage.Invoke(item);
		}

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

		public class TryVacuumItemFunc {
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
		public static void TryUpdateMouseOverrideForDeposit(Item item) {
			if (item.IsAir)
				return;

			if (CanVacuumItemHandler.Invoke(item, Main.LocalPlayer))
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

		public static event Action CloseAllStorageUIEvent;
		public static void CloseAllStorageUI() {
			CloseAllStorageUIEvent?.Invoke();
		}


	}
}
