﻿using Humanizer;
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
using androLib.Common.Utility;
using androLib.Common.Globals;
using androLib.ModIntegration;
using Terraria.ModLoader.Default;
using Terraria.GameContent;

namespace androLib.UI
{

	public class BagUI
	{
		public static class Bag_UI_ID {
			public const int Bag = 0;
			public const int BagScrollBar = 1;
			public const int BagScrollPanel = 2;
			public const int BagSearch = 3;
			public const int BagResizePanel = 4;
			public const int BagCollapseButton = 5;
			public const int BagRename = 6;
			public const int BagButtons = 100;
			public const int BagItemSlot = 200;
			public const int BagSwitcherPanel = 201;
			public const int BagSwitcherButtons = 202;
		}

		public Item[] Inventory => Storage.Items;
		public Item[] MyInventory => MyStorage.Items;
		public Storage Storage => StoragePlayer.LocalStoragePlayer.Storages[StorageID];
		public Storage MyStorage => StoragePlayer.LocalStoragePlayer.Storages[storageID];
		public BagUI DisplayedBagUI => StorageManager.BagUIs[StorageID];
		public int RegisteredUI_ID { get; }
		public int StorageID => displayAllBagButtons ? MyStorage.SwitcherStorageID : storageID;
		private readonly int storageID;
		private int GetUI_ID(int id) => MasterUIManager.GetUI_ID(id, RegisteredUI_ID);
		public struct ButtonProperties {
			public int ButtonUI_ID { get; private set; }
			public Action<BagUI> OnClicked;
			public readonly Func<string> GetText;
			public readonly string Text => GetText();
			public Func<Color, Color> ButtonColor;
			public ButtonProperties(int uiID, Action<BagUI> onClicked, Func<string> getText, Func<Color, Color> buttonColor = null) {
				ButtonUI_ID = uiID;
				OnClicked = onClicked;
				GetText = getText;
				ButtonColor = buttonColor;
			}
		}
		public List<ButtonProperties> DisplayedAllButtonProperties => DisplayedBagUI.MyButtonProperties;
		public List<ButtonProperties> MyButtonProperties;
		public int lootAllUIIndex;
		public int depositAllUIIndex;
		public int quickStackUIIndex;
		public int restockUIIndex;
		public int sortUIIndex;
		public int toggleVacuumIndex;
		public int depositAllMagicStorageUIIndex;
		public int toggleMagicStorageDepositIndex;

		public int ID => GetUI_ID(Bag_UI_ID.Bag);
		public int SearchID => GetUI_ID(Bag_UI_ID.BagSearch);
		public int RenameID => GetUI_ID(Bag_UI_ID.BagRename);
		public bool Hovering => MasterUIManager.HoveringMyUIType(RegisteredUI_ID);
		public int BagUILeft => MyStorage.UILeft;
		public int BagUITop => MyStorage.UITop;
		public int ResizePanelX => MyStorage.UIResizePanelX;
		public int ResizePanelY => MyStorage.UIResizePanelY;
		public Color PanelColor => Storage.GetUIColor();
		public Color ScrollBarColor => Storage.GetScrollBarColor();
		public static Color SelectedTextGray => new(100, 100, 100);
		public static Color VacuumPurple => new(162, 22, 255);
		public static Color ShouldDepositGreen => new(0, 255, 0);
		public static Color NoDepositRed => new(255, 0, 0);
		private static int SpacingTiny => 1;
		private static int SpacingSmall => 2;
		private static int Spacing => 4;
		private static int TextButtonPadding => 6;
		private static int PanelBorder => 10;
		private static float MinButtonHeightMult => 0.6f;
		private static float ButtonHeightMult => 0.9f;
		public const float buttonScaleMinimum = 0.75f;
		public const float buttonScaleMaximum = 1f;
		public float[] ButtonScale;
		public void ResetButtonScaleArray() => ButtonScale = Enumerable.Repeat(buttonScaleMinimum, DisplayedAllButtonProperties.Count).ToArray();
		public int GlowTime {
			get => DisplayedBagUI.MyGlowTime;
			set => DisplayedBagUI.MyGlowTime = value;
		}
		public int MyGlowTime = 0;
		public float GlowHue {
			get => DisplayedBagUI.MyGlowHue;
			set => DisplayedBagUI.MyGlowHue = value;
		}
		public float MyGlowHue = 0;
		public int ScrollPanelY {
			get => DisplayedBagUI.MyScrollPanelY;
			set => DisplayedBagUI.MyScrollPanelY = value;
		}
		public ref int ScrollPanelY_ref => ref DisplayedBagUI.MyScrollPanelY;
		public int MyScrollPanelY = int.MinValue;
		public int ScrollPanelPosition {
			get => DisplayedBagUI.MyScrollPanelPosition;
			set => DisplayedBagUI.MyScrollPanelPosition = value;
		}
		public ref int ScrollPanelPosition_ref => ref DisplayedBagUI.MyScrollPanelPosition;
		public int MyScrollPanelPosition = 0;
		private int MyResizeUIXBeforeDoubleClick = 0;
		private int MyResizeUIYBeforeDoubleClick = 0;
		/// <summary>
		/// For changing how items are visually displayed when selected only.
		/// </summary>
		public SortedDictionary<int, int> SelectedItemSlots => DisplayedBagUI.MySelectedItemSlots;
		public SortedDictionary<int, int> MySelectedItemSlots { get; private set; } = new();
		private bool displayAllBagButtons = false;
		private static int displayAllBagButtonsCount = 0;
		private static bool DisplayingAnyAllBagButtons => displayAllBagButtonsCount > 0;
		private readonly static List<KeyValuePair<int, int>> AllBagButtonInfos = new();//bagItemType, StorageID
		private static void UpdateAllBagButtonsInfo() {
			if (!DisplayingAnyAllBagButtons)
				return;

			AllBagButtonInfos.Clear();
			Player player = Main.LocalPlayer;
			for (int i = 0; i < StorageManager.BagUIs.Count; i++) {
				Storage storage = StorageManager.BagUIs[i].MyStorage;
				if (storage.HasRequiredItemToUseStorage(player, out IList<Item> inventoryFoundIn, out int index, out _)) {
					Item bagItem = inventoryFoundIn?[index];
					if (!bagItem.NullOrAir()) {
						int bagItemType = bagItem.type;
						if (storage.ValidItemTypeGetters(out SortedSet<int> bagItemTypes) == true) {
							if (bagItemTypes.Contains(bagItemType))
								AllBagButtonInfos.Add(new(bagItemType, i));
						}
					}
				}
			}
		}
		public bool DisplayBagUI => MyStorage.DisplayBagUI;

		public BagUI(int storageID, int registeredUI_ID) {
			this.storageID = storageID;
			RegisteredUI_ID = registeredUI_ID;

			CanDepositFunc = CanDeposit;
			CanVacuumItemFunc = CanVacuumItem;
			DepositFunc = Deposit;
			RestockFunc = Restock;
			MyUpdateItemSlotFunc = UpdateItemSlot;
		}
		private DrawnUIData drawnUIData;
		public class DrawnUIData {
			public UITextData nameData;
			public UIButtonData searchBarData;
			public List<UITextData> textButtons;
			public UIPanelData panel;
			public UIPanelData scrollBarData;
			public UIItemSlotData[] slotData;
			public UIPanelData scrollPanelData;
			public UIPanelData resizePanelData;
			public UIButtonData collapseButtonData;

			public int inventoryIndexStart;
			public int itemSlotsLeft;
			public int itemSlotsTop;
			public int slotsToDisplay;
			public bool usingSearchBar;
			public int itemSlotColumns;
			public int itemSlotSpaceWidth;
			public int itemSlotSpaceHeight;
			public int scrollPanelMinY;
			public int scrollPanelMaxY;
			public int possiblePanelPositions;
			public int scrollPanelWidth;
			public int scrollPanelHeight;
			public bool draggingScrollPanel;
			public bool displayScrollbar;
		}
		public void AddButton(Action<BagUI> OnClicked, Func<string> GetText, Func<Color, Color> buttonColor = null) {
			MyButtonProperties.Add(new(MyButtonProperties.Count, OnClicked, GetText, buttonColor));
		}
		public Func<IEnumerable<Item>> GetPlayersInventory = null;
		public Func<IList<Item>> GetLootAllTargetInventory = null;
		public void PreSetup() {
			MyButtonProperties = new();
			AddButton((bagUI) => bagUI.LootAll(), () => StorageTextID.LootAll.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			lootAllUIIndex = MyButtonProperties.Count - 1;
			AddButton((bagUI) => bagUI.DepositAll(GetPlayersInventory), () => StorageTextID.DepositAll.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			depositAllUIIndex = MyButtonProperties.Count - 1;
			AddButton((bagUI) => bagUI.QuickStackAll(GetPlayersInventory, Main.LocalPlayer), () => StorageTextID.QuickStack.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			quickStackUIIndex = MyButtonProperties.Count - 1;
			AddButton((bagUI) => bagUI.Sort(), () => StorageTextID.Sort.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			sortUIIndex = MyButtonProperties.Count - 1;
			
			if (StorageManager.RegisteredStorages[storageID].IsVacuumBag != false) {
				AddButton((bagUI) => bagUI.ToggleVacuum(), () => StorageTextID.ToggleVacuum.ToString().Lang(AndroMod.ModName, L_ID1.StorageText), (defaultColor) => MyStorage.ShouldVacuum ? VacuumPurple : defaultColor);
				toggleVacuumIndex = MyButtonProperties.Count - 1;
			}
			
			if (AndroMod.magicStorageEnabled) {
				AddButton((bagUI) => MagicStorageIntegration.DepositToMagicStorage(bagUI.MyInventory.ToList()), () => StorageTextID.DepositAllMagicStorage.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
				depositAllMagicStorageUIIndex = MyButtonProperties.Count - 1;
				AddButton((bagUI) => MyStorage.ShouldDepositToMagicStorage = !MyStorage.ShouldDepositToMagicStorage, () => StorageTextID.ToggleMagicStorageDeposit.ToString().Lang(AndroMod.ModName, L_ID1.StorageText), (defaultColor) => MyStorage.ShouldDepositToMagicStorage ? ShouldDepositGreen : NoDepositRed);
				toggleMagicStorageDepositIndex = MyButtonProperties.Count - 1;
			}
		}
		public void PostSetup() {
			AddButton((bagUI) => bagUI.CloseBag(), () => StorageTextID.CloseBag.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			ResetButtonScaleArray();
		}
		public static void StaticPostSetup() {
			MasterUIManager.PreDrawUIStaticFunctions += UpdateAllBagButtonsInfo;
		}
		public void PostDrawInterface(SpriteBatch spriteBatch) {
			StoragePlayer storagePlayer = StoragePlayer.LocalStoragePlayer;
			if (!Main.playerInventory) {
				if (AndroMod.clientConfig.ClosingInventoryClosesBags)
					CloseBag();

				return;
			}

			if (!DisplayBagUI)
				return;

			#region Pre UI

			UpdateSelectedItemSlots();

			#endregion

			#region Data

			drawnUIData = new DrawnUIData();

			Color mouseColor = MasterUIManager.MouseColor;
			if (GlowTime > 0) {
				GlowTime--;
				if (GlowTime <= 0)
					GlowHue = 0f;
			}

			//ItemSlots Data 1/2
			Item[] inventory = Inventory;

			int minPanelHeight = PanelBorder * 2;
			int minPanelWidth = PanelBorder * 2;

			//Button Texts
			string[] buttonTexts = DisplayedAllButtonProperties.Select(p => p.Text).ToArray();
			Vector2[] buttonSizes = buttonTexts.Select(t => UITextData.GetBaseSize(t)).ToArray();
			int longestButtonNameBaseWidth = (int)(buttonSizes[0].X * buttonScaleMinimum);
			int longestButtonNameWidth = (int)(buttonSizes[0].X * ButtonScale[0]);
			for (int i = 1; i < buttonSizes.Length; i++) {
				int currentButtonNameBaseWidth = (int)(buttonSizes[i].X * buttonScaleMinimum);
				if (currentButtonNameBaseWidth > longestButtonNameBaseWidth)
					longestButtonNameBaseWidth = currentButtonNameBaseWidth;

				int currentButtonNameWidth = (int)(buttonSizes[i].X * ButtonScale[i]);
				if (currentButtonNameWidth > longestButtonNameWidth)
					longestButtonNameWidth = currentButtonNameWidth;
			}

			int allButtonsHeight = buttonSizes.Select(v => (int)(v.Y * MinButtonHeightMult)).Sum();
			minPanelWidth += longestButtonNameWidth;
			minPanelHeight += allButtonsHeight;

			//Name Data
			int nameLeft = BagUILeft + PanelBorder;
			int nameTop = BagUITop + PanelBorder;
			string name = MasterUIManager.DisplayedRenameBarString(RenameID, Storage.DisplayedName);
			drawnUIData.nameData = new(RenameID, nameLeft, nameTop, name, 1f, mouseColor);
			UITextData nameData = drawnUIData.nameData;
			int nameWidth = nameData.Width;
			int nameHeight = nameData.Height;
			minPanelWidth += nameWidth;

			//Search Bar Data 1/2
			int searchBarMinWidth = 100;
			TextData searchBarTextData = new(MasterUIManager.DisplayedSearchBarString(SearchID));
			int searchBarWidth = Math.Max(searchBarMinWidth, searchBarTextData.Width + TextButtonPadding * 2);
			minPanelWidth += searchBarWidth + Spacing;

			//Collapse Button Data 1/2
			TextData collapseButtonTextData = new(StorageTextID.Switch.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			int collapseButtonWidth = collapseButtonTextData.Width + TextButtonPadding * 2 + Spacing;
			minPanelWidth += collapseButtonWidth;

			//Panel Data 1/2
			int panelBorderTopOffset = nameHeight + Spacing + SpacingSmall;

			int scrollBarWidth = 30;
			int scrollBarWidthSpacing = scrollBarWidth + Spacing;

			//Resize Panel Data
			int resizePanelWidth = 18;
			int resizePanelHeight = 18;
			int resizePanelWidthSpace = resizePanelWidth + SpacingSmall;
			int resizePanelHeightSpace = resizePanelHeight + SpacingSmall;

			//ItemSlots and Resize panel
			int itemSlotSpaceWidth = MasterUIManager.ItemSlotSize + Spacing;
			int itemSlotSpaceHeight = MasterUIManager.ItemSlotSize + Spacing;

			bool checkForLargerDefault = false;
			int heightFromResizePanel;
			int measuredPanelHeight;
			int itemSlotsHeight;
			int itemSlotRowsDisplayed;
			int checkCount = 0;
			do {
				checkCount++;
				bool changed = false;
				if (MyStorage.UIResizePanelY >= Storage.DefaultResizePanelIncrement / 2) {
					MyStorage.UIResizePanelY = BagUITop + nameHeight - Spacing + MasterUIManager.ItemSlotSize + itemSlotSpaceHeight * (MyStorage.UIResizePanelY / Storage.DefaultResizePanelIncrement - 1);
					changed = true;
				}

				heightFromResizePanel = MyStorage.UIResizePanelY - BagUITop + resizePanelHeightSpace;
				measuredPanelHeight = heightFromResizePanel < minPanelHeight ? minPanelHeight : heightFromResizePanel;
				itemSlotsHeight = measuredPanelHeight - panelBorderTopOffset - PanelBorder;
				itemSlotRowsDisplayed = (itemSlotsHeight - MasterUIManager.ItemSlotSize) / itemSlotSpaceHeight + 1;

				if (MyStorage.UIResizePanelX >= Storage.DefaultResizePanelIncrement / 2) {
					int columns = MyStorage.UIResizePanelX / Storage.DefaultResizePanelIncrement;
					MyStorage.UIResizePanelX = BagUILeft + longestButtonNameBaseWidth + MasterUIManager.ItemSlotSize + itemSlotSpaceWidth * (columns - 1);
					if (columns * itemSlotRowsDisplayed < inventory.Length)
						MyStorage.UIResizePanelX += scrollBarWidthSpacing;

					changed = true;
				}

				if (checkCount > 1)
					break;

				if (changed) {
					if (MyStorage.UIResizePanelX == MyResizeUIXBeforeDoubleClick && MyStorage.UIResizePanelY == MyResizeUIYBeforeDoubleClick) {
						checkForLargerDefault = true;
						MyStorage.UIResizePanelY = Storage.LastUIResizePanelDefaultY;
						MyStorage.UIResizePanelX = Storage.LastUIResizePanelDefaultX;
					}
				}
			}
			while (checkForLargerDefault);

			int widthFromResizePanel = MyStorage.UIResizePanelX - BagUILeft + longestButtonNameWidth - longestButtonNameBaseWidth + resizePanelWidth + SpacingSmall;
			int measuredPanelWidth = widthFromResizePanel < minPanelWidth ? minPanelWidth : widthFromResizePanel;
			int itemSlotsAndScrollbarWidth = measuredPanelWidth - longestButtonNameWidth - PanelBorder * 2;
			int itemSlotsWidth = itemSlotsAndScrollbarWidth;
			int panelInnerHeight = Math.Max(allButtonsHeight, nameHeight + MasterUIManager.ItemSlotSize);
			int itemSlotColumns = (itemSlotsWidth - MasterUIManager.ItemSlotSize) / itemSlotSpaceWidth + 1;
			int itemSlotTotalRows = inventory.Length.CeilingDivide(itemSlotColumns);

			//Scroll Bar Data 1/2
			int possiblePanelPositions = itemSlotTotalRows - itemSlotRowsDisplayed;
			bool displayScrollbar = possiblePanelPositions > 0;
			if (displayScrollbar) {
				drawnUIData.displayScrollbar = true;
				minPanelWidth += scrollBarWidthSpacing;
				measuredPanelWidth = widthFromResizePanel < minPanelWidth ? minPanelWidth : widthFromResizePanel;
				itemSlotsAndScrollbarWidth = measuredPanelWidth - longestButtonNameWidth - PanelBorder * 2;
				itemSlotsWidth = itemSlotsAndScrollbarWidth - scrollBarWidthSpacing;
				itemSlotColumns = (itemSlotsWidth - MasterUIManager.ItemSlotSize) / itemSlotSpaceWidth + 1;
			}

			//Search Bar Data 2/2
			int searchBarTop = nameTop - TextButtonPadding;
			int totalTopBarSpaceRemaining = itemSlotsWidth - searchBarWidth - nameWidth - collapseButtonWidth;
			int nameToSearchBarSpace = Math.Min(totalTopBarSpaceRemaining, Spacing * 10);
			drawnUIData.searchBarData = new(SearchID, nameData.BottomRight.X + nameToSearchBarSpace, searchBarTop, searchBarTextData, mouseColor, Math.Max(TextButtonPadding, (searchBarMinWidth - searchBarTextData.Width) / 2), 0, PanelColor, Storage.GetButtonHoverColor());
			UIButtonData searchBarData = drawnUIData.searchBarData;

			//Collapse Button Data 2/2
			int searchBarToCollapseButtonSpace = Math.Min(totalTopBarSpaceRemaining - nameToSearchBarSpace + Spacing, Spacing * 6);
			drawnUIData.collapseButtonData = new(Bag_UI_ID.BagCollapseButton, (int)searchBarData.BottomRight.X + searchBarToCollapseButtonSpace, searchBarTop, collapseButtonTextData, mouseColor, TextButtonPadding, 0, MyStorage.GetUIColor(), MyStorage.GetButtonHoverColor());
			UIButtonData collapseButtonData = drawnUIData.collapseButtonData;

			int itemSlotsLeft = nameLeft;

			drawnUIData.itemSlotColumns = itemSlotColumns;
			drawnUIData.itemSlotSpaceWidth = itemSlotsAndScrollbarWidth;
			drawnUIData.itemSlotSpaceHeight = panelInnerHeight;
			drawnUIData.itemSlotsLeft = itemSlotsLeft;

			//ItemSlots Data 2/2
			int itemSlotsTop = BagUITop + panelBorderTopOffset;
			drawnUIData.itemSlotsTop = itemSlotsTop;

			//Text buttons Data
			int buttonsLeft = itemSlotsLeft + itemSlotsAndScrollbarWidth + Spacing;

			int scrollBarLeft = buttonsLeft - scrollBarWidthSpacing;

			int currentButtonTop = nameTop;
			drawnUIData.textButtons = new();
			List<UITextData> textButtons = drawnUIData.textButtons;
			float buttonHeightMult = Math.Min(MinButtonHeightMult * (measuredPanelHeight - PanelBorder * 2) / allButtonsHeight, ButtonHeightMult);
			for (int buttonIndex = 0; buttonIndex < DisplayedAllButtonProperties.Count; buttonIndex++) {
				ButtonProperties buttonProperties = DisplayedAllButtonProperties[buttonIndex];
				string text = buttonTexts[buttonIndex];
				float scale = ButtonScale[buttonIndex];
				Color color = buttonProperties.ButtonColor?.Invoke(mouseColor) ?? mouseColor;

				UITextData thisButton = new(GetUI_ID(Bag_UI_ID.BagButtons) + buttonIndex, buttonsLeft, currentButtonTop, text, scale, color, ancorBotomLeft: true);
				textButtons.Add(thisButton);
				currentButtonTop += (int)(thisButton.BaseHeight * buttonHeightMult);
			}

			//Panel Data 2/2
			int panelBorderRightOffset = Spacing + longestButtonNameWidth + PanelBorder;
			int panelWidth = measuredPanelWidth;

			int panelHeight = measuredPanelHeight;
			drawnUIData.panel = new(ID, BagUILeft, BagUITop, panelWidth, panelHeight, PanelColor);
			UIPanelData panel = drawnUIData.panel;

			//Scroll Bar Data 2/2
			int scrollBarTop = BagUITop + PanelBorder;
			if (displayScrollbar) {
				drawnUIData.scrollBarData = new(GetUI_ID(Bag_UI_ID.BagScrollBar), scrollBarLeft, scrollBarTop, scrollBarWidth, panelHeight - PanelBorder * 2, ScrollBarColor);
				UIPanelData scrollBarData = drawnUIData.scrollBarData;

				//Scroll Panel Data 1/2
				int scrollPanelWidth = scrollBarWidth - SpacingTiny * 2;
				int maxHeight = scrollBarData.Height - SpacingTiny * 2;
				int numerator = itemSlotTotalRows - (possiblePanelPositions > 0 ? possiblePanelPositions : 0);
				int scrollPanelHeight = maxHeight * numerator / itemSlotTotalRows;
				int scrollPanelMinY = scrollBarData.TopLeft.Y + SpacingTiny;
				int scrollPanelMaxY = scrollBarData.BottomRight.Y - scrollPanelHeight - SpacingTiny;
				if (possiblePanelPositions < 0)
					possiblePanelPositions = 0;

				drawnUIData.scrollPanelWidth = scrollPanelWidth;
				drawnUIData.scrollPanelHeight = scrollPanelHeight;
				drawnUIData.scrollPanelMinY = scrollPanelMinY;
				drawnUIData.scrollPanelMaxY = scrollPanelMaxY;
				drawnUIData.possiblePanelPositions = possiblePanelPositions;
			}

			#endregion

			 //Panel Draw
			 panel.Draw(spriteBatch);

			//Scroll Bar Draw
			if (displayScrollbar) {
				drawnUIData.scrollBarData.Draw(spriteBatch);

				int lastScrollPanelPosition = ScrollPanelPosition;
				//If panel is being dragged, force scrollPanelY to be in it's correct position by calculating with scrollPanelPosition.
				bool draggingScrollPanel = MasterUIManager.PanelBeingDragged == GetUI_ID(Bag_UI_ID.BagScrollPanel);
				drawnUIData.draggingScrollPanel = draggingScrollPanel;
				if (draggingScrollPanel) {
					int scrollPanelRange = drawnUIData.scrollPanelMaxY - drawnUIData.scrollPanelMinY;
					ScrollPanelPosition = ((ScrollPanelY - drawnUIData.scrollPanelMinY) * possiblePanelPositions).RoundDivide(scrollPanelRange);
				}

				ScrollPanelPosition_ref.Clamp(0, possiblePanelPositions);
				if (lastScrollPanelPosition != ScrollPanelPosition)
					SoundEngine.PlaySound(SoundID.MenuTick);
			}
			else {
				ScrollPanelPosition = 0;
			}

			//ItemSlots Draw
			drawnUIData.slotData = new UIItemSlotData[inventory.Length];
			UIItemSlotData[] slotData = drawnUIData.slotData;
			int startRow = ScrollPanelPosition;
			bool usingSearchBar = MasterUIManager.UsingTypingBar(SearchID);
			int inventoryIndexStart = startRow * itemSlotColumns;
			int slotsToDisplay = itemSlotRowsDisplayed * itemSlotColumns;
			int slotNum = 0;
			int itemSlotX = itemSlotsLeft;
			int itemSlotY = itemSlotsTop;
			drawnUIData.usingSearchBar = usingSearchBar;
			drawnUIData.inventoryIndexStart = inventoryIndexStart;
			drawnUIData.slotsToDisplay = slotsToDisplay;
			for (int inventoryIndex = inventoryIndexStart; inventoryIndex < inventory.Length && slotNum < slotsToDisplay; inventoryIndex++) {
				if (inventoryIndex >= inventory.Length)
					break;

				ref Item item = ref inventory[inventoryIndex];
				if (!usingSearchBar || item.Name.ToLower().Contains(MasterUIManager.TypingBarString.ToLower())) {
					slotData[inventoryIndex] = new(GetUI_ID(Bag_UI_ID.BagItemSlot), itemSlotX, itemSlotY);
					int context = SelectedItemSlots.TryGetValue(inventoryIndex, out int selectedContext) ? selectedContext : ItemSlotContextID.Normal;
					slotData[inventoryIndex].Draw(spriteBatch, item, context, GlowHue, GlowTime);

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

			//Collapse Button Draw
			collapseButtonData.Draw(spriteBatch);

			//Text Buttons Draw
			for (int buttonIndex = 0; buttonIndex < textButtons.Count; buttonIndex++) {
				UITextData textButton = textButtons[buttonIndex];
				textButton.Draw(spriteBatch);
			}

			if (displayScrollbar) {
				//Scroll Panel Data 2 / 2
				if (MasterUIManager.PanelBeingDragged == GetUI_ID(Bag_UI_ID.BagScrollPanel)) {
					ScrollPanelY_ref.Clamp(drawnUIData.scrollPanelMinY, drawnUIData.scrollPanelMaxY);
				}
				else {
					int scrollPanelRange = drawnUIData.scrollPanelMaxY - drawnUIData.scrollPanelMinY;
					int offset = possiblePanelPositions > 0 ? ScrollPanelPosition * scrollPanelRange / possiblePanelPositions : 0;
					ScrollPanelY = offset + drawnUIData.scrollPanelMinY;
				}

				drawnUIData.scrollPanelData = new(GetUI_ID(Bag_UI_ID.BagScrollPanel), scrollBarLeft + SpacingTiny, ScrollPanelY, drawnUIData.scrollPanelWidth, drawnUIData.scrollPanelHeight, mouseColor);

				drawnUIData.scrollPanelData.Draw(spriteBatch);
			}

			//Resize Panel Draw
			drawnUIData.resizePanelData = new(GetUI_ID(Bag_UI_ID.BagResizePanel), panel.BottomRight.X - resizePanelWidthSpace, panel.BottomRight.Y - resizePanelHeightSpace, resizePanelWidth, resizePanelHeight, mouseColor);
			UIPanelData resizePanelData = drawnUIData.resizePanelData;
			resizePanelData.Draw(spriteBatch);

			if (displayAllBagButtons)
				DrawBagButtonsPanel(spriteBatch);
		}
		public void UpdateInterface() {
			StoragePlayer storagePlayer = StoragePlayer.LocalStoragePlayer;
			if (!DisplayBagUI || !Main.playerInventory || drawnUIData == null)
				return;

			if (displayAllBagButtons)
				UpdateAllBagButtons();

			if (ItemSlot.ShiftInUse && (MasterUIManager.NoUIBeingHovered && DisplayedBagUI.CanBeStored(Main.HoverItem) || MasterUIManager.HovingUIByID(GetUI_ID(depositAllUIIndex)))) {
				if (!Main.mouseItem.IsAir || Main.HoverItem.favorited || !CanShiftClickNonBagItemToBag(Main.HoverItem)) {
					Main.cursorOverride = -1;
				}
				else {
					Main.cursorOverride = CursorOverrideID.InventoryToChest;
				}
			}

			UITextData nameData = drawnUIData.nameData;
			UIButtonData searchBarData = drawnUIData.searchBarData;
			List<UITextData> textButtons = drawnUIData.textButtons;
			UIPanelData panel = drawnUIData.panel;
			UIPanelData scrollBarData = drawnUIData.scrollBarData;
			UIItemSlotData[] slotDatas = drawnUIData.slotData;
			UIPanelData scrollPanelData = drawnUIData.scrollPanelData;
			UIPanelData resizePanelData = drawnUIData.resizePanelData;
			UIButtonData collapseButtonData = drawnUIData.collapseButtonData;
			int inventoryIndexStart = drawnUIData.inventoryIndexStart;
			int itemSlotsLeft = drawnUIData.itemSlotsLeft;
			int itemSlotsTop = drawnUIData.itemSlotsTop;
			int slotsToDisplay = drawnUIData.slotsToDisplay;
			bool usingSearchBar = drawnUIData.usingSearchBar;
			int itemSlotColumns = drawnUIData.itemSlotColumns;
			int itemSlotSpaceWidth = drawnUIData.itemSlotSpaceWidth;
			int itemSlotSpaceHeight = drawnUIData.itemSlotSpaceHeight;
			int scrollPanelMinY = drawnUIData.scrollPanelMinY;
			int scrollPanelMaxY = drawnUIData.scrollPanelMaxY;
			int possiblePanelPositions = drawnUIData.possiblePanelPositions;
			int scrollPanelWidth = drawnUIData.scrollPanelWidth;
			int scrollPanelHeight = drawnUIData.scrollPanelHeight;
			bool draggingScrollPanel = drawnUIData.draggingScrollPanel;
			bool displayScrollbar = drawnUIData.displayScrollbar;

			//Itemslots
			Item[] inventory = Inventory;
			int itemSlotX = itemSlotsLeft;
			int itemSlotY = itemSlotsTop;
			int slotNum = 0;
			for (int inventoryIndex = inventoryIndexStart; inventoryIndex < inventory.Length && slotNum < slotsToDisplay; inventoryIndex++) {
				if (inventoryIndex >= inventory.Length || inventoryIndex >= slotDatas.Length)
					break;

				if (!usingSearchBar || inventory[inventoryIndex].Name.ToLower().Contains(MasterUIManager.TypingBarString.ToLower())) {
					UpdateItemSlotFunc(inventoryIndex, this, inventory, slotDatas);

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

			//Rename
			bool mouseHoveringRename = nameData.MouseHovering();
			if (mouseHoveringRename) {
				if (MasterUIManager.LeftMouseClicked) {
					MasterUIManager.ClickTypingBar(RenameID);
					MasterUIManager.TypingBarString = Storage.DisplayedName;
				}
			}

			if (MasterUIManager.TypingOnBar(RenameID)) {
				if (MasterUIManager.LeftMouseClicked && !mouseHoveringRename || MasterUIManager.ShouldStopTypingOnBar()) {
					MasterUIManager.StopTypingOnBar();
				}
				else {
					PlayerInput.WritingText = true;
					Main.instance.HandleIME();
					MasterUIManager.TypingBarString = Main.GetInputText(MasterUIManager.TypingBarString);
					Storage.Rename(MasterUIManager.TypingBarString);
				}
			}

			//Search bar
			bool mouseHoveringSearchBar = searchBarData.MouseHovering();
			if (mouseHoveringSearchBar) {
				if (MasterUIManager.LeftMouseClicked)
					MasterUIManager.ClickTypingBar(SearchID);
			}

			if (MasterUIManager.TypingOnBar(SearchID)) {
				if (MasterUIManager.LeftMouseClicked && !mouseHoveringSearchBar || MasterUIManager.ShouldStopTypingOnBar()) {
					MasterUIManager.StopTypingOnBar();
				}
				else {
					PlayerInput.WritingText = true;
					Main.instance.HandleIME();
					MasterUIManager.TypingBarString = Main.GetInputText(MasterUIManager.TypingBarString);
				}
			}

			//Collapse Button
			if (collapseButtonData.MouseHovering()) {
				if (MasterUIManager.LeftMouseClicked) {
					ToggleCollapse();
					SoundEngine.PlaySound(SoundID.MenuTick);
				}
			}

			//Text Buttons Draw
			int bagUIID = GetUI_ID(Bag_UI_ID.BagButtons);
			for (int buttonIndex = 0; buttonIndex <  textButtons.Count; buttonIndex++) {
				UITextData textButton = textButtons[buttonIndex];
				int allButtonPropertiesIndex = textButton.ID - bagUIID;
				if (MasterUIManager.MouseHovering(textButton, true)) {
					ButtonScale[allButtonPropertiesIndex] += 0.05f;

					if (ButtonScale[allButtonPropertiesIndex] > buttonScaleMaximum)
						ButtonScale[allButtonPropertiesIndex] = buttonScaleMaximum;

					if (MasterUIManager.LeftMouseClicked) {
						DisplayedAllButtonProperties[allButtonPropertiesIndex].OnClicked(allButtonPropertiesIndex < DisplayedAllButtonProperties.Count - 1 ? DisplayedBagUI : this);
						SoundEngine.PlaySound(SoundID.MenuTick);
					}
				}
				else {
					ButtonScale[allButtonPropertiesIndex] -= 0.05f;

					if (ButtonScale[allButtonPropertiesIndex] < buttonScaleMinimum)
						ButtonScale[allButtonPropertiesIndex] = buttonScaleMinimum;
				}
			}

			if (displayScrollbar) {
				//Scroll Bar Hover
				if (scrollBarData.MouseHovering()) {
					if (MasterUIManager.LeftMouseClicked) {
						MasterUIManager.UIBeingHovered = Bag_UI_ID.BagScrollPanel;
						ScrollPanelY = Main.mouseY - scrollPanelHeight / 2;
						ScrollPanelY_ref.Clamp(scrollPanelMinY, scrollPanelMaxY);
						scrollPanelData.SetCenterY(ScrollPanelY);

						scrollPanelData.TryStartDraggingUI();
					}
				}

				if (scrollPanelData.ShouldDragUI()) {
					MasterUIManager.DragUI(out _, out ScrollPanelY_ref);
				}
				else if (draggingScrollPanel) {
					int scrollPanelRange = scrollPanelMaxY - scrollPanelMinY;
					ScrollPanelPosition = ((ScrollPanelY - scrollPanelMinY) * possiblePanelPositions).RoundDivide(scrollPanelRange);
				}
			}

			//Resize Panel Hover and Drag
			if (resizePanelData.MouseHovering()) {
				if (MasterUIManager.DoubleClick) {
					MyResizeUIXBeforeDoubleClick = MyStorage.UIResizePanelX;
					MyResizeUIYBeforeDoubleClick = MyStorage.UIResizePanelY;
					MyStorage.UIResizePanelX = Storage.UIResizePanelDefaultX;
					MyStorage.UIResizePanelY = Storage.UIResizePanelDefaultY;
				}
				else {
					resizePanelData.TryStartDraggingUI();
				}
			}

			if (resizePanelData.ShouldDragUI()) {
				MasterUIManager.DragUI(out MyStorage.UIResizePanelX, out MyStorage.UIResizePanelY);
			}

			//Panel Hover and Drag
			if (panel.MouseHovering()) {
				MainPanelClickInteractions();
			}

			if (panel.ShouldDragUI()) {
				int left = MyStorage.UILeft;
				int top = MyStorage.UITop;
				MasterUIManager.DragUI(out MyStorage.UILeft, out MyStorage.UITop);
				MyStorage.UIResizePanelX += MyStorage.UILeft - left;
				MyStorage.UIResizePanelY += MyStorage.UITop - top;
			}

			if (displayScrollbar) {
				int scrollWheelTicks = MasterUIManager.ScrollWheelTicks;
				if (scrollWheelTicks != 0 && Hovering && MasterUIManager.NoPanelBeingDragged) {
					if (ScrollPanelPosition > 0 && scrollWheelTicks < 0 || ScrollPanelPosition < possiblePanelPositions && scrollWheelTicks > 0) {
						SoundEngine.PlaySound(SoundID.MenuTick);
						ScrollPanelPosition += scrollWheelTicks;
					}
				}
			}
			
			SelectedItemSlots.Clear();
		}
		public Action<int, BagUI, Item[], UIItemSlotData[]> MyUpdateItemSlotFunc;
		public Action<int, BagUI, Item[], UIItemSlotData[]> UpdateItemSlotFunc => DisplayedBagUI.MyUpdateItemSlotFunc;
		private void UpdateItemSlot(int inventoryIndex, BagUI bagUI, Item[] inventory, UIItemSlotData[] slotDatas) {
			ref Item item = ref inventory[inventoryIndex];
			UIItemSlotData slotData = slotDatas[inventoryIndex];
			if (slotData.MouseHovering()) {
				if (AndroModSystem.FavoriteKeyDown) {
					Main.cursorOverride = CursorOverrideID.FavoriteStar;
					if (MasterUIManager.LeftMouseClicked) {
						item.favorited = !item.favorited;
						SoundEngine.PlaySound(SoundID.MenuTick);
						if (item.TryGetGlobalItem(out VacuumToStorageItem vacuumItem2))
							vacuumItem2.favorited = item.favorited;
					}
				}
				else {
					if (ItemSlot.ShiftInUse && Main.mouseRight && Main.mouseItem.NullOrAir()) {
						bool unloaded = item.ModItem is UnloadedItem;
						if (MasterUIManager.RightMouseClicked && !item.NullOrAir() && !item.favorited) {
							int type = item.type;
							bool allowed = DisplayedBagUI.CanBeStored(item);
							if (unloaded || !allowed || TryAddToPlayerBlacklist(type)) {
								if (!unloaded && allowed)
									SoundEngine.PlaySound(SoundID.Research);

								if (unloaded || AndroMod.clientConfig.RemoveItemsWhenBlacklisted) {
									for (int i = 0; i < inventory.Length; i++) {
										ref Item checkItem = ref inventory[i];
										if (checkItem.type == type) {
											if (!StorageManager.TryReturnItemToPlayer(ref checkItem, Main.LocalPlayer))
												break;
										}
									}
								}
							}
						}
					}
					else {
						bool doClickInteractions = Main.mouseItem.NullOrAir();
						bool unloaded = Main.mouseItem.ModItem is UnloadedItem;
						if (MasterUIManager.LeftMouseClicked) {
							if (!doClickInteractions && (item.NullOrAir() || Main.mouseItem.type == item.type)) {
								if (!DisplayedBagUI.CanBeStored(Main.mouseItem) || Storage.IsVacuumBag == null && Storage.HasWhiteListGetter && Storage.CanVacuumItemWhenNotContained != null && !Storage.CanVacuumItemWhenNotContained(Main.mouseItem)) {
									if (!unloaded && TryAddToPlayerWhitelist(Main.mouseItem.type))
										SoundEngine.PlaySound(SoundID.ResearchComplete);
								}
							}
						}

						if (doClickInteractions || DisplayedBagUI.CanBeStored(Main.mouseItem) || Main.mouseItem.type == item.type || unloaded)
							slotData.ClickInteractions(ref item);
					}
				}
			}

			if (!item.IsAir && !item.favorited && item.TryGetGlobalItem(out VacuumToStorageItem vacuumItem) && vacuumItem.favorited)
				item.favorited = true;
		}
		private void MainPanelClickInteractions() {
			UIPanelData panel = drawnUIData.panel;
			if (MasterUIManager.DoubleClick) {
				int x = MyStorage.UIResizePanelX - MyStorage.UILeft;
				int y = MyStorage.UIResizePanelY - MyStorage.UITop;
				MyStorage.UIResizePanelX = MyStorage.LastUIResizePanelX + MyStorage.UILeft;
				MyStorage.UIResizePanelY = MyStorage.LastUIResizePanelY + MyStorage.UITop;
				MyStorage.LastUIResizePanelX = x;
				MyStorage.LastUIResizePanelY = y;
			}
			else {
				panel.TryStartDraggingUI();
			}
		}
		private DrawnSwitcherUIData drawnSwitcherUIData;
		public class DrawnSwitcherUIData
		{
			public UIPanelData switcherPanel;
			public List<UIItemButtonData> uIImageButtonDatas;
		}
		private void DrawBagButtonsPanel(SpriteBatch spriteBatch) {
			UIPanelData panel = drawnUIData.panel;

			drawnSwitcherUIData = new();

			//Switcher Panel Data 1/2
			int bagSwitcherWidth = panel.Width;
			int buttonSize = MasterUIManager.ItemSlotSize;
			int buttonSizeSpace = buttonSize + Spacing;
			int buttonsPerRow = (bagSwitcherWidth - PanelBorder * 2 - buttonSize) / buttonSizeSpace + 1;
			int rows = AllBagButtonInfos.Count.CeilingDivide(buttonsPerRow);
			int bagSwitcherHeight = buttonSizeSpace * (rows - 1) + buttonSize;

			//Image Button Data
			drawnSwitcherUIData.uIImageButtonDatas = new(AllBagButtonInfos.Count);
			List<UIItemButtonData> uIImageButtonDatas = drawnSwitcherUIData.uIImageButtonDatas;

			int columnNum = 0;
			int buttonsLeft = panel.TopLeft.X + PanelBorder;
			int buttonsTop = panel.TopLeft.Y - bagSwitcherHeight - PanelBorder;
			int buttonLeft = buttonsLeft;
			int buttonTop = buttonsTop;
			int storageID = StorageID;
			for (int i = 0; i < AllBagButtonInfos.Count; i++) {
				KeyValuePair<int, int> pair = AllBagButtonInfos[i];//bag Item type, Storage ID
				uIImageButtonDatas.Add(new(Bag_UI_ID.BagSwitcherButtons + i, buttonLeft, buttonTop, pair.Key, () => MyStorage.SwitcherStorageID = pair.Value, storageID == pair.Value));
				columnNum++;
				if (columnNum == buttonsPerRow) {
					buttonLeft = buttonsLeft;
					buttonTop += buttonSizeSpace;
					columnNum = 0;
				}
				else {
					buttonLeft += buttonSizeSpace;
				}
			}
			
			//Switcher Panel Data 2/2
			int bagSwitcherHeightBorder = bagSwitcherHeight + PanelBorder * 2;
			drawnSwitcherUIData.switcherPanel = new(Bag_UI_ID.BagSwitcherPanel, panel.TopLeft.X, panel.TopLeft.Y - bagSwitcherHeightBorder, bagSwitcherWidth, bagSwitcherHeightBorder, panel.Color);
			UIPanelData switcherPanel = drawnSwitcherUIData.switcherPanel;

			//Switcher Panel Draw
			switcherPanel.Draw(spriteBatch);

			//Image Button Draw
			for (int i = 0; i < uIImageButtonDatas.Count; i++) {
				UIItemButtonData uIImageButtonData = uIImageButtonDatas[i];
				uIImageButtonData.Draw(spriteBatch);
			}
		}
		private void UpdateAllBagButtons() {
			UIPanelData switcherPanel = drawnSwitcherUIData.switcherPanel;
			List<UIItemButtonData> uIImageButtonDatas = drawnSwitcherUIData.uIImageButtonDatas;

			for (int i = 0; i < uIImageButtonDatas.Count; i++) {
				UIItemButtonData uIImageButtonData = uIImageButtonDatas[i];
				if (uIImageButtonData.MouseHovering()) {
					if (MasterUIManager.LeftMouseClicked) {
						uIImageButtonData.OnClick();
						SoundEngine.PlaySound(SoundID.MenuTick);
					}
				}
			}

			if (switcherPanel.MouseHovering()) {
				MainPanelClickInteractions();
			}
		}
		public void OpenBag() {
			ScrollPanelY = int.MinValue;
			Main.playerInventory = true;
			MyStorage.DisplayBagUI = true;
		}
		public void CloseBag(bool noSound = false) {
			ScrollPanelY = int.MinValue;
			MyStorage.DisplayBagUI = false;
			MasterUIManager.TryResetTypingBar(RenameID);
			MasterUIManager.TryResetTypingBar(SearchID);
			if (!noSound)
				SoundEngine.PlaySound(SoundID.Grab);

			if (!AndroMod.clientConfig.ReOpenBagSwitcherAutomatically)
				displayAllBagButtons = false;
		}
		
		public void LootAll() {
			Item[] inv = MyInventory;
			IList<Item> lootAllTargetInventory = GetLootAllTargetInventory?.Invoke();
			bool doSound = false;
			for (int i = 0; i < inv.Length; i++) {
				Item item = inv[i];
				if (lootAllTargetInventory?.Deposit(item, out int index) == true)
					doSound |= index != lootAllTargetInventory.Count;

				if (item.type > ItemID.None && !item.favorited) {
					inv[i] = Main.LocalPlayer.GetItem(Main.myPlayer, inv[i], GetItemSettings.LootAllSettings);
				}
			}

			if (doSound)
				SoundEngine.PlaySound(SoundID.Grab);
		}
		
		#region Single

		public bool CanBeStored(Item item) => !item.NullOrAir() && MyStorage.ItemAllowedToBeStored(item);
		public bool VacuumAllowed(Item item) => MyStorage.IsVacuumBag == true || MyStorage.IsVacuumBag == null && (MyStorage.CanVacuumItemWhenNotContained != null && MyStorage.CanVacuumItemWhenNotContained(item) || ContainsItem(item));
		public bool ContainsItem(Item item) => MyStorage.ContainsSlow(item);
		public bool RoomInStorage(Item item) {
			Item[] inv = MyInventory;
			int stack = item.stack;
			for (int i = inv.Length - 1; i >= 0; i--) {
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

		public Func<Item, bool> CanDepositFunc;
		public bool CanDeposit(Item item) {
			if (item.NullOrAir())
				return false;

			if (!CanBeStored(item))
				return false;

			if (!RoomInStorage(item))
				return false;

			return true;
		}
		public Func<Item, Player, bool, bool> CanVacuumItemFunc;
		public bool CanVacuumItem(Item item, Player player, bool ignoreNeedBagInInventory = false) {
			if (item.NullOrAir())
				return false;

			if (!MyStorage.ShouldVacuum)
				return false;

			if (!CanBeStored(item))
				return false;

			//If bag is a "Quick Stack" only style bag, check if item is already in inventory
			if (!VacuumAllowed(item))
				return false;

			if (!ignoreNeedBagInInventory && !MyStorage.HasRequiredItemToUseStorageSlow(player))
				return false;
			
			if (!RoomInStorage(item))
				return false;

			return true;
		}
		public bool TryVacuumItem(Item item, Player player, bool ignoreNeedBagInInventory = false, bool playSound = true) {
			if (CanVacuumItemFunc(item, player, ignoreNeedBagInInventory))
				return DepositFunc(item, playSound, false);

			return false;
		}
		public Func<Item, bool, bool, bool> DepositFunc;
		public bool Deposit(Item item, bool playSound = true, bool displayedInventory = false) {
			Item[] inv = displayedInventory ? Inventory : MyInventory;
			if (inv.Deposit(item, out int _)) {
				if (playSound)
					SoundEngine.PlaySound(SoundID.Grab);

				return true;
			}

			return false;
		}
		public Func<Item, bool, bool, bool> RestockFunc;
		public bool Restock(Item item, bool playSound = true, bool displayedInventory = false) {
			Item[] bagInventory = displayedInventory ? Inventory : MyInventory;
			if (bagInventory.Restock(item, out int _)) {
				if (playSound)
					SoundEngine.PlaySound(SoundID.Grab);

				return true;
			}

			return false;
		}
		public bool CanShiftClickNonBagItemToBag(Item item) {
			if (!DisplayBagUI)
				return false;

			if (!CanDepositFunc(item))
				return false;

			return true;
		}
		public bool TryShiftClickNonBagItemToBag(ref Item item) {
			if (CanShiftClickNonBagItemToBag(item))
				return DepositFunc(item, true, true);

			return false;
		}

		#endregion

		#region Multiple

		public bool DepositAll(Func<IEnumerable<Item>> inv, bool playSound = true) => DepositAll(inv?.Invoke(), playSound);

		/// <summary>
		/// Do not use for anything besides a button function unless you add an optional Func<> to make it possible to replace.
		/// </summary>
		public bool DepositAll(IEnumerable<Item> otherInv, bool playSound = true) {
			IEnumerable<Item> inv = Main.LocalPlayer.inventory.TakePlayerInventory40();
			if (otherInv != null)
				inv = inv.Concat(otherInv);

			IEnumerable<Item> items = inv.Where(i => !i.NullOrAir() && !i.favorited && CanBeStored(i));
			Item[] arr = inv.ToArray();
			bool transferredAnyItem = RestockAll(items, false);
			int index = 0;
			Item[] bagInventory = MyInventory;
			foreach (Item item in items) {
				if (item.NullOrAir())
					continue;

				while (index < bagInventory.Length && !bagInventory[index].IsAir) {
					index++;
				}

				if (index >= bagInventory.Length)
					break;

				ref Item bagInventoryItem = ref bagInventory[index];
				bagInventoryItem = item.Clone();
				if (bagInventoryItem.stack == bagInventoryItem.maxStack)
					bagInventory.DoCoins(index);

				item.TurnToAir();
				transferredAnyItem = true;
			}

			if (transferredAnyItem) {
				if (playSound)
					SoundEngine.PlaySound(SoundID.Grab);

				Recipe.FindRecipes(true);
			}

			return transferredAnyItem;
		}

		/// <summary>
		/// Do not use for anything besides a button function unless you add an optional Func<> to make it possible to replace.
		/// </summary>
		public bool RestockAll(IEnumerable<Item> inv, bool playSound = true) {
			Item[] bagInventory = MyInventory;
			bool transferredAnyItem = false;
			SortedDictionary<int, List<int>> nonAirItemsInStorage = new();
			for (int i = 0; i < bagInventory.Length; i++) {
				Item bagItem = bagInventory[i];
				if (!bagItem.NullOrAir() && bagItem.stack < bagItem.maxStack)
					nonAirItemsInStorage.AddOrCombine(bagItem.type, i);
			}

			foreach (Item item in inv) {
				if (nonAirItemsInStorage.TryGetValue(item.type, out List<int> storageIndexes)) {
					foreach (int bagIndex in storageIndexes) {
						ref Item bagItem = ref bagInventory[bagIndex];
						if (bagItem.stack < item.maxStack) {
							if (ItemLoader.TryStackItems(bagItem, item, out _)) {
								transferredAnyItem = true;
								if (item.stack < 1) {
									item.TurnToAir();
									break;
								}
							}
						}
					}
				}
			}

			if (playSound && transferredAnyItem) {
				bagInventory.DoCoins();
				SoundEngine.PlaySound(SoundID.Grab);
				Recipe.FindRecipes(true);
			}

			return transferredAnyItem;
		}

		#endregion

		public void QuickStackAll(Func<IEnumerable<Item>> inv, Player player) => QuickStackAll(inv?.Invoke(), player);
		public bool QuickStack(Item item, Player player, bool ignoreTile = false, bool playSound = true) {
			if (ContainsItem(item))
				return TryVacuumItem(item, player, ignoreTile, playSound);

			return false;
		}
		public void QuickStackAll(IEnumerable<Item> otherInv, Player player) {
			IEnumerable<Item> inv = Main.LocalPlayer.inventory.TakePlayerInventory40();
			if (otherInv != null)
				inv = inv.Concat(otherInv);

			foreach (Item item in inv) {
				if (item.NullOrAir())
					continue;

				if (item.favorited)
					continue;

				QuickStack(item, player);
			}
		}
		
		private void Sort() {
			if (MyStorage.GetItems != null)
				return;

			Item[] storageItems = MyStorage.Items;
			MasterUIManager.SortItems(ref storageItems);
			MyStorage.Items = storageItems;
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

			//prevent glow on bags that select items.
			if (MySelectedItemSlots.Count == 0) {
				GlowTime = 300;
				GlowHue = 0.5f;
			}
		}
		private void ToggleVacuum() {
			MyStorage.ShouldVacuum = !MyStorage.ShouldVacuum;
		}
		public void AddSelectedItemSlots(IEnumerable<KeyValuePair<int, int>> selectedItemSlots) {
			foreach (KeyValuePair<int, int> selectedItemSlot in selectedItemSlots) {
				AddSelectedItemSlot(selectedItemSlot.Key, selectedItemSlot.Value);
			}
		}
		public void AddSelectedItemSlot(int selectedItemSlot, int context) => MySelectedItemSlots.TryAdd(selectedItemSlot, context);
		private void UpdateSelectedItemSlots() {
			Storage.SelectItemSlotFunc();
			if (MySelectedItemSlots.Count == 1) {
				Item selectedItem = Inventory[MySelectedItemSlots.First().Key];
				if (selectedItem.stack < 1)
					selectedItem.favorited = false;
			}
		}
		public bool TryAddToPlayerWhitelist(int type) {
			if (Storage.HasWhiteOreBlacklistGetter) {
				bool updated = StorageManager.AddToPlayerWhitelist(StorageID, type);
				Storage.TryAddToPlayerWhitelist(type);
				return updated;
			}

			return false;
		}
		public bool TryAddToPlayerBlacklist(int type) {
			if (Storage.HasWhiteOreBlacklistGetter) {
				bool updated = StorageManager.AddToPlayerBlacklist(StorageID, type);
				Storage.TryAddToPlayerBlacklist(type);
				return updated;
			}

			return false;
		}
		public void ToggleCollapse() {
			displayAllBagButtons = !displayAllBagButtons;
			displayAllBagButtonsCount += displayAllBagButtons ? 1 : -1;
			ResetButtonScaleArray();
		}
	}
}
