using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;
//using WeaponEnchantments.Common;
//using WeaponEnchantments.Common.Globals;
//using WeaponEnchantments.Common.Utility;
//using WeaponEnchantments.Items;
//using WeaponEnchantments.ModIntegration;
using androLib.Common.Utility;
using androLib.Common.Globals;

namespace androLib.UI
{

	public class BagUI//Time to move this and related stuff to AndroMod.
	{
		public static class Bag_UI_ID {
			//public static int Bag_UITypeID;//Set by MasterUIManager//Needs to use BagStorageID instead since this is for all bags.

			public const int Bag = 0;
			public const int BagScrollBar = 1;
			public const int BagScrollPanel = 2;
			public const int BagSearch = 3;
			public const int BagLootAll = 100;
			public const int BagDepositAll = 101;
			public const int BagQuickStack = 102;
			public const int BagSort = 103;
			public const int BagToggleVacuum = 104;
			public const int BagItemSlot = 200;
		}

		//private int BagStorageID = -1;
		//public static Item[] Inventory => (Item[])WEMod.AndroLib.Call("GetItems", BagStorageID);
		public Storage Storage { get; }
		public Item[] Inventory => Storage.Items;
		public int RegisteredUI_ID { get; }
		private int GetUI_ID(int id) => MasterUIManager.GetUI_ID(id, RegisteredUI_ID);
		public class BagButtonID {
			public const int LootAll = 0;
			public const int DepositAll = 1;
			public const int QuickStack = 2;
			public const int Sort = 3;
			public const int ToggleVacuum = 4;
			public const int Count = 5;
		}
		public int ID => GetUI_ID(Bag_UI_ID.Bag);
		public int SearchID => GetUI_ID(Bag_UI_ID.BagSearch);
		public bool Hovering => MasterUIManager.HoveringMyUIType(RegisteredUI_ID);
		public int BagUILeft => Storage.UILeft;
		public int BagUITop => Storage.UITop;
		public Color PanelColor => Storage.GetUIColor();
		public Color ScrollBarColor => Storage.GetScrollBarColor();
		public static Color SelectedTextGray => new(100, 100, 100);
		public static Color VacuumPurple => new(162, 22, 255);
		private static int Spacing => 4;
		private static int PanelBorder => 10;
		public const float buttonScaleMinimum = 0.75f;
		public const float buttonScaleMaximum = 1f;
		public float[] ButtonScale = Enumerable.Repeat(buttonScaleMinimum, BagButtonID.Count).ToArray();
		private int glowTime = 0;
		private float glowHue = 0f;
		private int scrollPanelY = int.MinValue;
		private int scrollPanelPosition = 0;
		public bool DisplayBagUI = false;

		public BagUI(Storage storage, int registeredUI_ID) {
			Storage = storage;
			RegisteredUI_ID = registeredUI_ID;
		}

		public void PostDrawInterface(SpriteBatch spriteBatch) {
			StoragePlayer storagePlayer = StoragePlayer.LocalStoragePlayer;
			if (!DisplayBagUI || !Main.playerInventory)
				return;

			#region Pre UI

			if (storagePlayer.Player.chest != -1)
				return;

			if (ItemSlot.ShiftInUse && MasterUIManager.NoUIBeingHovered && CanBeStored(Main.HoverItem)) {
				if (!Main.mouseItem.IsAir || !RoomInStorage(Main.HoverItem)) {
					Main.cursorOverride = -1;
				}
				else {
					Main.cursorOverride = CursorOverrideID.InventoryToChest;
				}
			}

			#endregion

			#region Data

			Color mouseColor = MasterUIManager.MouseColor;
			if (glowTime > 0) {
				glowTime--;
				if (glowTime <= 0)
					glowHue = 0f;
			}

			//ItemSlots Data 1/2
			//Item[] inventory = wePlayer.oreBagItems;
			Item[] inventory = Inventory;

			int itemSlotColumns = 10;
			int itemSlotRowsDisplayed = 3;
			int itemSlotTotalRows = inventory.Length.CeilingDivide(itemSlotColumns);
			int itemSlotSpaceWidth = MasterUIManager.ItemSlotSize + Spacing;
			int itemSlotSpaceHeight = MasterUIManager.ItemSlotSize + Spacing;
			int itemSlotsWidth = (itemSlotColumns - 1) * itemSlotSpaceWidth + MasterUIManager.ItemSlotSize;
			int itemSlotsHeight = (itemSlotRowsDisplayed - 1) * itemSlotSpaceHeight + MasterUIManager.ItemSlotSize;
			int itemSlotsLeft = BagUILeft + PanelBorder;

			//Name Data
			int nameLeft = itemSlotsLeft;//itemSlotsLeft + (itemSlotsWidth - nameWidth) / 2;
			int nameTop = BagUITop + PanelBorder;
			//string name = EnchantmentStorageTextID.OreBag.ToString().Lang(L_ID1.EnchantmentStorageText);
			string name = Storage.GetLocalizedName();
			UITextData nameData = new(UI_ID.None, nameLeft, nameTop, name, 1f, mouseColor);

			//Panel Data 1/2
			int panelBorderTop = nameData.Height + Spacing + 2;

			//Search Bar Data
			int searchBarMinWidth = 100;
			TextData searchBarTextData = new(MasterUIManager.DisplayedSearchBarString(SearchID));
			Color temp = Storage.GetButtonHoverColor();//TODO: DELETE ME!!!
			UIButtonData searchBarData = new(SearchID, nameData.BottomRight.X + Spacing * 10, nameTop - 6, searchBarTextData, mouseColor, Math.Max(6, (searchBarMinWidth - searchBarTextData.Width) / 2), 0, PanelColor, Storage.GetButtonHoverColor());

			//ItemSlots Data 2/2
			int itemSlotsTop = BagUITop + panelBorderTop;

			//Scroll Bar Data 1/2
			int scrollBarLeft = itemSlotsLeft + itemSlotsWidth + Spacing;
			int scrollBarWidth = 30;

			//Text buttons Data
			int buttonsLeft = scrollBarLeft + scrollBarWidth + Spacing;
			int currentButtonTop = nameTop;
			UITextData[] textButtons = new UITextData[BagButtonID.Count];
			int longestButtonNameWidth = 0;
			for (int buttonIndex = 0; buttonIndex < BagButtonID.Count; buttonIndex++) {
				string text = ((StorageTextID)buttonIndex).ToString().Lang(AndroMod.ModName, L_ID1.StorageText);//TODO: change EnchantmentStorageTextID to be in androLib or separate
				//string text = $"Button {buttonIndex}";
				float scale = ButtonScale[buttonIndex];
				Color color;
				if (buttonIndex == BagButtonID.ToggleVacuum && Storage.ShouldVacuum) {
					color = VacuumPurple;
				}
				else {
					color = mouseColor;
				}

				UITextData thisButton = new(GetUI_ID(Bag_UI_ID.BagLootAll) + buttonIndex, buttonsLeft, currentButtonTop, text, scale, color, ancorBotomLeft: true);
				textButtons[buttonIndex] = thisButton;
				longestButtonNameWidth = Math.Max(longestButtonNameWidth, thisButton.Width);
				currentButtonTop += (int)(thisButton.BaseHeight * 0.95f);
			}

			//Panel Data 2/2
			int panelBorderRightOffset = Spacing + longestButtonNameWidth + PanelBorder;
			int panelWidth = itemSlotsWidth + Spacing + scrollBarWidth + PanelBorder + panelBorderRightOffset;
			int panelHeight = itemSlotsHeight + PanelBorder + panelBorderTop;
			UIPanelData panel = new(ID, BagUILeft, BagUITop, panelWidth, panelHeight, PanelColor);
			
			//Scroll Bar Data 2/2
			int scrollBarTop = BagUITop + PanelBorder;
			UIPanelData scrollBarData = new(GetUI_ID(Bag_UI_ID.BagScrollBar), scrollBarLeft, scrollBarTop, scrollBarWidth, panelHeight - PanelBorder * 2, ScrollBarColor);

			//Scroll Panel Data 1/2
			int scrollPanelXOffset = 1;
			int scrollPanelSize = scrollBarWidth - scrollPanelXOffset * 2;
			int scrollPanelMinY = scrollBarData.TopLeft.Y + scrollPanelXOffset;
			int scrollPanelMaxY = scrollBarData.BottomRight.Y - scrollPanelSize - scrollPanelXOffset;
			int possiblePanelPositions = itemSlotTotalRows - itemSlotRowsDisplayed;
			if (possiblePanelPositions < 1)
				possiblePanelPositions = 1;

			scrollPanelPosition.Clamp(0, possiblePanelPositions);

			#endregion

			//Panel Draw
			panel.Draw(spriteBatch);

			//Scroll Bar Draw
			scrollBarData.Draw(spriteBatch);

			//ItemSlots Draw
			int startRow = scrollPanelPosition;
			bool UsingSearchBar = MasterUIManager.UsingSearchBar(SearchID);
			int inventoryIndexStart = startRow * itemSlotColumns;
			int slotsToDisplay = itemSlotRowsDisplayed * itemSlotColumns;
			int slotNum = 0;
			int itemSlotX = itemSlotsLeft;
			int itemSlotY = itemSlotsTop;
			for (int inventoryIndex = inventoryIndexStart; inventoryIndex < inventory.Length && slotNum < slotsToDisplay; inventoryIndex++) {
				if (inventoryIndex >= inventory.Length)
					break;

				ref Item item = ref inventory[inventoryIndex];
				if (!UsingSearchBar || item.Name.ToLower().Contains(MasterUIManager.SearchBarString.ToLower())) {
					UIItemSlotData slotData = new(GetUI_ID(Bag_UI_ID.BagItemSlot), itemSlotX, itemSlotY);
					if (slotData.MouseHovering()) {
						if (AndroModSystem.FavoriteKeyDown) {
							Main.cursorOverride = CursorOverrideID.FavoriteStar;
							if (MasterUIManager.LeftMouseClicked) {
								item.favorited = !item.favorited;
								SoundEngine.PlaySound(SoundID.MenuTick);
								//if (item.TryGetGlobalItem(out VacuumToOreBagItems vacummItem2))
								if (item.TryGetGlobalItem(out VacuumToStorageItem vacummItem2))
									vacummItem2.favorited = item.favorited;
							}
						}
						else if (Main.mouseItem.NullOrAir() || CanBeStored(Main.mouseItem)) {
							slotData.ClickInteractions(ref item);
						}
					}

					if (!item.IsAir && !item.favorited && item.TryGetGlobalItem(out VacuumToStorageItem vacummItem) && vacummItem.favorited)
						item.favorited = true;

					slotData.Draw(spriteBatch, item, ItemSlotContextID.Normal, glowHue, glowTime);

					slotNum++;
					if (slotNum % itemSlotColumns == 0) {
						itemSlotX = itemSlotsLeft;
						itemSlotY += itemSlotSpaceHeight;
					}
					else {
						itemSlotX += itemSlotSpaceWidth;
					}
				}
			}

			//Name Draw
			nameData.Draw(spriteBatch);

			//Search Bar Draw
			searchBarData.Draw(spriteBatch);

			bool mouseHoveringSearchBar = searchBarData.MouseHovering();
			if (mouseHoveringSearchBar) {
				if (MasterUIManager.LeftMouseClicked)
					MasterUIManager.ClickSearchBar(SearchID);
			}

			if (MasterUIManager.TypingOnSearchBar(SearchID)) {
				if (MasterUIManager.LeftMouseClicked && !mouseHoveringSearchBar || Main.mouseRight || !Main.playerInventory) {
					MasterUIManager.StopTypingOnSearchBar();
				}
				else {
					PlayerInput.WritingText = true;
					Main.instance.HandleIME();
					MasterUIManager.SearchBarString = Main.GetInputText(MasterUIManager.SearchBarString);
				}
			}

			//Text Buttons Draw
			for (int buttonIndex = 0; buttonIndex < BagButtonID.Count; buttonIndex++) {
				UITextData textButton = textButtons[buttonIndex];
				textButton.Draw(spriteBatch);
				if (MasterUIManager.MouseHovering(textButton, true)) {
					ButtonScale[buttonIndex] += 0.05f;

					if (ButtonScale[buttonIndex] > buttonScaleMaximum)
						ButtonScale[buttonIndex] = buttonScaleMaximum;

					if (MasterUIManager.LeftMouseClicked) {
						switch (buttonIndex) {
							case BagButtonID.LootAll:
								LootAll();
								break;
							case BagButtonID.DepositAll:
								DepositAll(Main.LocalPlayer.inventory);
								break;
							case BagButtonID.QuickStack:
								QuickStack(Main.LocalPlayer.inventory);
								break;
							case BagButtonID.Sort:
								Sort();
								break;
							case BagButtonID.ToggleVacuum:
								ToggleVacuum();
								break;
						}

						SoundEngine.PlaySound(SoundID.MenuTick);
					}
				}
				else {
					ButtonScale[buttonIndex] -= 0.05f;

					if (ButtonScale[buttonIndex] < buttonScaleMinimum)
						ButtonScale[buttonIndex] = buttonScaleMinimum;
				}
			}

			//Scroll Panel Data 2/2
			bool draggingScrollPanel = MasterUIManager.PanelBeingDragged == GetUI_ID(Bag_UI_ID.BagScrollPanel);
			if (MasterUIManager.PanelBeingDragged == GetUI_ID(Bag_UI_ID.BagScrollPanel)) {
				scrollPanelY.Clamp(scrollPanelMinY, scrollPanelMaxY);
			}
			else {
				int scrollPanelRange = scrollPanelMaxY - scrollPanelMinY;
				int offset = scrollPanelPosition * scrollPanelRange / possiblePanelPositions;
				scrollPanelY = offset + scrollPanelMinY;
			}

			//Scroll Bar Hover
			if (scrollBarData.MouseHovering()) {
				if (MasterUIManager.LeftMouseClicked) {
					MasterUIManager.UIBeingHovered = UI_ID.None;
					scrollPanelY = Main.mouseY - scrollPanelSize / 2;
					scrollPanelY.Clamp(scrollPanelMinY, scrollPanelMaxY);
				}
			}

			//Scroll Panel Hover and Drag
			UIPanelData scrollPanelData = new(GetUI_ID(Bag_UI_ID.BagScrollPanel), scrollBarLeft + scrollPanelXOffset, scrollPanelY, scrollPanelSize, scrollPanelSize, mouseColor);
			if (scrollPanelData.MouseHovering()) {
				scrollPanelData.TryStartDraggingUI();
			}

			if (scrollPanelData.ShouldDragUI()) {
				MasterUIManager.DragUI(out _, out scrollPanelY);
			}
			else if (draggingScrollPanel) {
				int scrollPanelRange = scrollPanelMaxY - scrollPanelMinY;
				scrollPanelPosition = ((scrollPanelY - scrollPanelMinY) * possiblePanelPositions).RoundDivide(scrollPanelRange);
			}

			scrollPanelData.Draw(spriteBatch);

			//Panel Hover and Drag
			if (panel.MouseHovering()) {
				panel.TryStartDraggingUI();
			}

			if (panel.ShouldDragUI())
				MasterUIManager.DragUI(out Storage.UILeft, out Storage.UITop);

			int scrollWheelTicks = MasterUIManager.ScrollWheelTicks;
			if (scrollWheelTicks != 0 && Hovering && MasterUIManager.NoPanelBeingDragged) {
				if (scrollPanelPosition > 0 && scrollWheelTicks < 0 || scrollPanelPosition < possiblePanelPositions && scrollWheelTicks > 0) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					scrollPanelPosition += scrollWheelTicks;
				}
			}
		}
		
		public void OpenBag() {
			Main.playerInventory = true;
			DisplayBagUI = true;
			Main.LocalPlayer.chest = -1;
			//if (MagicStorageIntegration.MagicStorageIsOpen())//TODO: Move magic storage integration to a androLib
			//	MagicStorageIntegration.TryClosingMagicStorage();
		}
		public void CloseBag(bool noSound = false) {
			DisplayBagUI = false;
			MasterUIManager.TryResetSearch(SearchID);
			if (Main.LocalPlayer.chest == -1) {
				if (!noSound)
					SoundEngine.PlaySound(SoundID.Grab);
			}
		}
		public bool CanBeStored(Item item) => Storage.ItemAllowedToBeStored(item);
		public bool RoomInStorage(Item item, Player player = null) {
			if (Main.netMode == NetmodeID.Server)
				return false;

			if (player == null)
				player = Main.LocalPlayer;

			if (player.whoAmI != Main.myPlayer)
				return false;

			//Item[] inv = player.GetWEPlayer().oreBagItems;
			Item[] inv = Inventory;
			int stack = item.stack;
			for (int i = 0; i < inv.Length; i++) {
				Item invItem = inv[i];
				if (invItem.IsAir) {
					return true;
				}
				else if (invItem.type == item.type) {
					stack -= invItem.maxStack - invItem.stack;
					if (stack <= 0)
						return true;
				}
			}

			return false;
		}
		private void LootAll() {
			Item[] inv = Inventory;
			for (int i = 0; i < inv.Length; i++) {
				Item item = inv[i];
				if (item.type > ItemID.None && !item.favorited) {
					inv[i] = Main.LocalPlayer.GetItem(Main.myPlayer, inv[i], GetItemSettings.LootAllSettings);
				}
			}
		}
		public bool CanVacuumItem(Item item, Player player) {
			if (item.NullOrAir())
				return false;

			if (!Storage.ShouldVacuum)
				return false;

			if (!CanBeStored(item))
				return false;

			if (!Storage.HasRequiredItemToUseStorage(player))
				return false;

			if (!RoomInStorage(item))
				return false;

			return true;
		}
		public bool TryVacuumItem(ref Item item, Player player) {
			if (CanVacuumItem(item, player))
				return DepositAll(ref item);

			return false;
		}
		public bool DepositAll(ref Item item) => DepositAll(new Item[] { item });
		public bool DepositAll(Item[] inv) {
			bool transferedAnyItem = QuickStack(inv, false);
			int storageIndex = 0;
			Item[] oreBagInventory = Inventory;
			for (int i = 0; i < inv.Length; i++) {
				if (i == 58)//Skip Mouse Item
					continue;

				ref Item item = ref inv[i];
				if (!item.favorited && CanBeStored(item)) {
					while (storageIndex < oreBagInventory.Length && oreBagInventory[storageIndex].type > ItemID.None) {
						storageIndex++;
					}

					if (storageIndex < oreBagInventory.Length) {
						oreBagInventory[storageIndex] = item.Clone();
						item.TurnToAir();
						transferedAnyItem = true;
					}
					else {
						break;
					}
				}
			}

			if (transferedAnyItem) {
				SoundEngine.PlaySound(SoundID.Grab);
				Recipe.FindRecipes(true);
			}

			return transferedAnyItem;
		}
		public bool QuickStack(ref Item item) => QuickStack(new Item[] { item });
		public bool QuickStack(Item[] inv, bool playSound = true) {
			Item[] oreBagInventory = Inventory;
			bool transferedAnyItem = false;
			SortedDictionary<int, List<int>> nonAirItemsInStorage = new();
			for (int i = 0; i < oreBagInventory.Length; i++) {
				int type = oreBagInventory[i].type;
				if (type > ItemID.None)
					nonAirItemsInStorage.AddOrCombine(type, i);
			}

			for (int i = 0; i < inv.Length; i++) {
				ref Item item = ref inv[i];
				if (!item.favorited && CanBeStored(item) && nonAirItemsInStorage.TryGetValue(item.type, out List<int> storageIndexes)) {
					foreach (int storageIndex in storageIndexes) {
						ref Item storageItem = ref oreBagInventory[storageIndex];
						if (storageItem.stack < item.maxStack) {
							if (ItemLoader.TryStackItems(storageItem, item, out int transfered)) {
								transferedAnyItem = true;
								if (item.stack < 1) {
									item.TurnToAir();
									break;
								}
							}
						}
					}
				}
			}

			if (playSound && transferedAnyItem) {
				SoundEngine.PlaySound(SoundID.Grab);
				Recipe.FindRecipes(true);
			}

			return transferedAnyItem;
		}
		private void Sort() {
			MasterUIManager.SortItems(ref Storage.Items);
			Type itemSlotType = typeof(ItemSlot);
			int[] inventoryGlowTime = (int[])itemSlotType.GetField("inventoryGlowTime", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
			for (int i = 0; i < inventoryGlowTime.Length; i++) {
				inventoryGlowTime[i] = 0;
			}

			if (Main.LocalPlayer.chest != -1) {
				int[] inventoryGlowTimeChest = (int[])itemSlotType.GetField("inventoryGlowTimeChest", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
				for (int i = 0; i < inventoryGlowTimeChest.Length; i++) {
					inventoryGlowTimeChest[i] = 0;
				}
			}

			glowTime = 300;
			glowHue = 0.5f;
		}
		private void ToggleVacuum() {
			Storage.ShouldVacuum = !Storage.ShouldVacuum;
		}
		
	}
	
}
