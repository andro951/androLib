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
using androLib.Common.Utility;
using androLib.Common.Globals;
using androLib.ModIntegration;
using Terraria.ModLoader.Default;

namespace androLib.UI
{

	public class BagUI
	{
		public static class Bag_UI_ID {
			public const int Bag = 0;
			public const int BagScrollBar = 1;
			public const int BagScrollPanel = 2;
			public const int BagSearch = 3;
			public const int BagResizePannel = 4;
			public const int BagButtons = 100;
			//public const int BagDepositAll = 101;
			//public const int BagQuickStack = 102;
			//public const int BagSort = 103;
			//public const int BagToggleVacuum = 104;
			//public const int BagDepostAllMagicStorage = 105;
			//public const int BagCloseBag = 106;
			public const int BagItemSlot = 200;
		}

		public Item[] Inventory => Storage.Items;
		public Storage Storage => StoragePlayer.LocalStoragePlayer.Storages[StorageID];
		public int RegisteredUI_ID { get; }
		public int StorageID { get; }
		private int GetUI_ID(int id) => MasterUIManager.GetUI_ID(id, RegisteredUI_ID);
		//public class BagButtonID {
		//	public const int LootAll = 0;
		//	public const int DepositAll = 1;
		//	public const int QuickStack = 2;
		//	public const int Sort = 3;
		//	public const int ToggleVacuum = 4;
		//	public const int DepositAllMagicStorage = 5;
		//	public const int CloseBag = 6;
		//	public const int Count = 7;
		//	public const int LastButtonIndex = 100000;
		//}
		public struct ButtonProperties {
			public int ButtonUI_ID { get; private set; }
			public Action<BagUI> OnClicked;
			private Func<string> GetText;
			public string Text => GetText();
			public Func<Color, Color> ButtonColor;
			public ButtonProperties(int uiID, Action<BagUI> onClicked, Func<string> getText, Func<Color, Color> buttonColor = null) {
				ButtonUI_ID = uiID;
				OnClicked = onClicked;
				GetText = getText;
				ButtonColor = buttonColor;
			}
		}
		public List<ButtonProperties> AllButtonProperites;
		public int depositAllUIIndex;
		public int ID => GetUI_ID(Bag_UI_ID.Bag);
		public int SearchID => GetUI_ID(Bag_UI_ID.BagSearch);
		public bool Hovering => MasterUIManager.HoveringMyUIType(RegisteredUI_ID);
		public int BagUILeft => Storage.UILeft;
		public int BagUITop => Storage.UITop;
		public int ResizePanelX => Storage.UIResizePanelX;
		public int ResizePanelY => Storage.UIResizePanelY;
		public Color PanelColor => Storage.GetUIColor();
		public Color ScrollBarColor => Storage.GetScrollBarColor();
		public static Color SelectedTextGray => new(100, 100, 100);
		public static Color VacuumPurple => new(162, 22, 255);
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
		private int glowTime = 0;
		private float glowHue = 0f;
		private int scrollPanelY = int.MinValue;
		private int scrollPanelPosition = 0;
		/// <summary>
		/// For changing how items are visually displayed when selected only.
		/// </summary>
		private SortedDictionary<int, int> selectedItemSlots = new();
		public bool DisplayBagUI => Storage.DisplayBagUI && Main.LocalPlayer.chest == -1;

		public BagUI(int storageID, int registeredUI_ID) {
			StorageID = storageID;
			RegisteredUI_ID = registeredUI_ID;
		}
		private DrawnUIData drawnUIData;
		public class DrawnUIData {
			public UIButtonData searchBarData;
			public List<UITextData> textButtons;
			public UIPanelData panel;
			public UIPanelData scrollBarData;
			public UIItemSlotData[] slotData;
			public UIPanelData scrollPanelData;
			public UIPanelData resizePanelData;

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
			AllButtonProperites.Add(new(AllButtonProperites.Count, OnClicked, GetText, buttonColor));
		}
		public void PreSetup() {
			AllButtonProperites = new();
			AddButton((bagUI) => bagUI.LootAll(), () => StorageTextID.LootAll.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			AddButton((bagUI) => bagUI.DepositAll(Main.LocalPlayer.inventory.TakePlayerInventory40()), () => StorageTextID.DepositAll.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			depositAllUIIndex = AllButtonProperites.Count - 1;
			AddButton((bagUI) => bagUI.QuickStack(Main.LocalPlayer.inventory.TakePlayerInventory40(), Main.LocalPlayer), () => StorageTextID.QuickStack.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			AddButton((bagUI) => bagUI.Sort(), () => StorageTextID.Sort.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			
			if (StorageManager.RegisteredStorages[StorageID].IsVacuumBag != false)
				AddButton((bagUI) => bagUI.ToggleVacuum(), () => StorageTextID.ToggleVacuum.ToString().Lang(AndroMod.ModName, L_ID1.StorageText), (defaultColor) => Storage.ShouldVacuum ? VacuumPurple : defaultColor);
			
			if (AndroMod.magicStorageEnabled)
				AddButton((bagUI) => MagicStorageIntegration.DepositToMagicStorage(bagUI.Inventory.ToList()), () => StorageTextID.DepositAllMagicStorage.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
		}
		public void PostSetup() {
			AddButton((bagUI) => bagUI.CloseBag(), () => StorageTextID.CloseBag.ToString().Lang(AndroMod.ModName, L_ID1.StorageText));
			ButtonScale = Enumerable.Repeat(buttonScaleMinimum, AllButtonProperites.Count).ToArray();
		}
		public void PostDrawInterface(SpriteBatch spriteBatch) {
			StoragePlayer storagePlayer = StoragePlayer.LocalStoragePlayer;
			if (!DisplayBagUI || !Main.playerInventory)
				return;

			#region Pre UI

			UpdateSelectedItemSlots();

			#endregion

			#region Data

			drawnUIData = new DrawnUIData();

			Color mouseColor = MasterUIManager.MouseColor;
			if (glowTime > 0) {
				glowTime--;
				if (glowTime <= 0)
					glowHue = 0f;
			}

			//ItemSlots Data 1/2
			Item[] inventory = Inventory;

			int minPanelHeight = PanelBorder * 2;
			int minPanelWidth = PanelBorder * 2;

			//Button Texts
			string[] buttonTexts = AllButtonProperites.Select(p => p.Text).ToArray();
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
			string name = Storage.GetLocalizedName();
			UITextData nameData = new(UI_ID.None, nameLeft, nameTop, name, 1f, mouseColor);
			int nameWidth = nameData.Width;
			int nameHeight = nameData.Height;
			minPanelWidth += nameWidth;

			//Search Bar Data 1/2
			int searchBarMinWidth = 100;
			TextData searchBarTextData = new(MasterUIManager.DisplayedSearchBarString(SearchID));
			int searchBarWidth = Math.Max(searchBarMinWidth, searchBarTextData.Width + TextButtonPadding * 2);
			minPanelWidth += searchBarWidth + Spacing;

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
			if (Storage.UIResizePanelY >= Storage.DefaultResizePanelIncrement / 2)
				Storage.UIResizePanelY = BagUITop + nameHeight - Spacing + MasterUIManager.ItemSlotSize + itemSlotSpaceHeight * (Storage.UIResizePanelY / Storage.DefaultResizePanelIncrement - 1);

			int heightFromResizePanel = Storage.UIResizePanelY - BagUITop + resizePanelHeightSpace;
			int measuredPanelHeight = heightFromResizePanel < minPanelHeight ? minPanelHeight : heightFromResizePanel;
			int itemSlotsHeight = measuredPanelHeight - panelBorderTopOffset - PanelBorder;
			int itemSlotRowsDisplayed = (itemSlotsHeight - MasterUIManager.ItemSlotSize) / itemSlotSpaceHeight + 1;

			if (Storage.UIResizePanelX >= Storage.DefaultResizePanelIncrement / 2) {
				int columns = Storage.UIResizePanelX / Storage.DefaultResizePanelIncrement;
				Storage.UIResizePanelX = BagUILeft + longestButtonNameBaseWidth + MasterUIManager.ItemSlotSize + itemSlotSpaceWidth * (columns - 1);
				if (columns * itemSlotRowsDisplayed < inventory.Length)
					Storage.UIResizePanelX += scrollBarWidthSpacing;
			}

			int widthFromResizePanel = Storage.UIResizePanelX - BagUILeft + longestButtonNameWidth - longestButtonNameBaseWidth + resizePanelWidth + SpacingSmall;
			int measuredPanelWidth = widthFromResizePanel < minPanelWidth ? minPanelWidth : widthFromResizePanel;
			int itemSlotsAndScrollbarWidth = measuredPanelWidth - longestButtonNameWidth - PanelBorder * 2;
			int itemSlotWidth = itemSlotsAndScrollbarWidth;
			int itemSlotsWidth = itemSlotsAndScrollbarWidth;
			int pannelInnerHeight = Math.Max(allButtonsHeight, nameHeight + MasterUIManager.ItemSlotSize);
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
				itemSlotWidth = itemSlotsAndScrollbarWidth - scrollBarWidthSpacing;
				if (widthFromResizePanel < minPanelWidth - SpacingSmall) {
					itemSlotColumns = (itemSlotsWidth - MasterUIManager.ItemSlotSize) / itemSlotSpaceWidth + 1;
				}
				else {
					itemSlotColumns = (itemSlotsWidth - scrollBarWidthSpacing - MasterUIManager.ItemSlotSize) / itemSlotSpaceWidth + 1;
				}
			}

			//Search Bar Data 2/2
			int nameToSearchBarSpace = Math.Min(itemSlotWidth - searchBarWidth - nameWidth, Spacing * 10);
			drawnUIData.searchBarData = new(SearchID, nameData.BottomRight.X + nameToSearchBarSpace, nameTop - TextButtonPadding, searchBarTextData, mouseColor, Math.Max(TextButtonPadding, (searchBarMinWidth - searchBarTextData.Width) / 2), 0, PanelColor, Storage.GetButtonHoverColor());
			UIButtonData searchBarData = drawnUIData.searchBarData;

			int itemSlotsLeft = nameLeft;

			drawnUIData.itemSlotColumns = itemSlotColumns;
			drawnUIData.itemSlotSpaceWidth = itemSlotsAndScrollbarWidth;
			drawnUIData.itemSlotSpaceHeight = pannelInnerHeight;
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
			for (int buttonIndex = 0; buttonIndex < AllButtonProperites.Count; buttonIndex++) {
				ButtonProperties buttonProperties = AllButtonProperites[buttonIndex];
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

				int lastScrollPanelPosition = scrollPanelPosition;
				//If panel is being dragged, force scrollPanelY to be in it's correct position by calculating with scrollPanelPosition.
				bool draggingScrollPanel = MasterUIManager.PanelBeingDragged == GetUI_ID(Bag_UI_ID.BagScrollPanel);
				drawnUIData.draggingScrollPanel = draggingScrollPanel;
				if (draggingScrollPanel) {
					int scrollPanelRange = drawnUIData.scrollPanelMaxY - drawnUIData.scrollPanelMinY;
					scrollPanelPosition = ((scrollPanelY - drawnUIData.scrollPanelMinY) * possiblePanelPositions).RoundDivide(scrollPanelRange);
				}

				scrollPanelPosition.Clamp(0, possiblePanelPositions);
				if (lastScrollPanelPosition != scrollPanelPosition)
					SoundEngine.PlaySound(SoundID.MenuTick);
			}

			//ItemSlots Draw
			drawnUIData.slotData = new UIItemSlotData[inventory.Length];
			UIItemSlotData[] slotData = drawnUIData.slotData;
			int startRow = scrollPanelPosition;
			bool usingSearchBar = MasterUIManager.UsingSearchBar(SearchID);
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
				if (!usingSearchBar || item.Name.ToLower().Contains(MasterUIManager.SearchBarString.ToLower())) {
					slotData[inventoryIndex] = new(GetUI_ID(Bag_UI_ID.BagItemSlot), itemSlotX, itemSlotY);
					int context = selectedItemSlots.TryGetValue(inventoryIndex, out int selectedContext) ? selectedContext : ItemSlotContextID.Normal;
					slotData[inventoryIndex].Draw(spriteBatch, item, context, glowHue, glowTime);

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

			//Text Buttons Draw
			for (int buttonIndex = 0; buttonIndex < textButtons.Count; buttonIndex++) {
				UITextData textButton = textButtons[buttonIndex];
				textButton.Draw(spriteBatch);
			}

			if (displayScrollbar) {
				//Scroll Panel Data 2 / 2
				if (MasterUIManager.PanelBeingDragged == GetUI_ID(Bag_UI_ID.BagScrollPanel)) {
					scrollPanelY.Clamp(drawnUIData.scrollPanelMinY, drawnUIData.scrollPanelMaxY);
				}
				else {
					int scrollPanelRange = drawnUIData.scrollPanelMaxY - drawnUIData.scrollPanelMinY;
					int offset = possiblePanelPositions > 0 ? scrollPanelPosition * scrollPanelRange / possiblePanelPositions : 0;
					scrollPanelY = offset + drawnUIData.scrollPanelMinY;
				}

				drawnUIData.scrollPanelData = new(GetUI_ID(Bag_UI_ID.BagScrollPanel), scrollBarLeft + SpacingTiny, scrollPanelY, drawnUIData.scrollPanelWidth, drawnUIData.scrollPanelHeight, mouseColor);

				drawnUIData.scrollPanelData.Draw(spriteBatch);
			}

			//Resize Panel Draw
			drawnUIData.resizePanelData = new(GetUI_ID(Bag_UI_ID.BagResizePannel), panel.BottomRight.X - resizePanelWidthSpace, panel.BottomRight.Y - resizePanelHeightSpace, resizePanelWidth, resizePanelHeight, mouseColor);
			UIPanelData resizePanelData = drawnUIData.resizePanelData;
			resizePanelData.Draw(spriteBatch);
		}
		public void UpdateInterface() {
			StoragePlayer storagePlayer = StoragePlayer.LocalStoragePlayer;
			if (!DisplayBagUI || !Main.playerInventory || drawnUIData == null)
				return;

			if (ItemSlot.ShiftInUse && (MasterUIManager.NoUIBeingHovered && CanBeStored(Main.HoverItem) || MasterUIManager.HovingUIByID(GetUI_ID(depositAllUIIndex)))) {
				if (!Main.mouseItem.IsAir || Main.HoverItem.favorited || !CanShiftClickNonBagItemToBag(Main.HoverItem)) {
					Main.cursorOverride = -1;
				}
				else {
					Main.cursorOverride = CursorOverrideID.InventoryToChest;
				}
			}

			UIButtonData searchBarData = drawnUIData.searchBarData;
			List<UITextData> textButtons = drawnUIData.textButtons;
			UIPanelData panel = drawnUIData.panel;
			UIPanelData scrollBarData = drawnUIData.scrollBarData;
			UIItemSlotData[] slotDatas = drawnUIData.slotData;
			UIPanelData scrollPanelData = drawnUIData.scrollPanelData;
			UIPanelData resizePanelData = drawnUIData.resizePanelData;
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
			for (int inventoryIndex = inventoryIndexStart; inventoryIndex < inventory.Length && slotNum <  slotsToDisplay; inventoryIndex++) {
				if (inventoryIndex >= inventory.Length)
					break;

				ref Item item = ref inventory[inventoryIndex];
				if (!usingSearchBar || item.Name.ToLower().Contains(MasterUIManager.SearchBarString.ToLower())) {
					UIItemSlotData slotData = slotDatas[inventoryIndex];
					if (slotData.MouseHovering()) {
						if (AndroModSystem.FavoriteKeyDown) {
							Main.cursorOverride = CursorOverrideID.FavoriteStar;
							if (MasterUIManager.LeftMouseClicked) {
								item.favorited = !item.favorited;
								SoundEngine.PlaySound(SoundID.MenuTick);
								if (item.TryGetGlobalItem(out VacuumToStorageItem vacummItem2))
									vacummItem2.favorited = item.favorited;
							}
						}
						else {
							if (ItemSlot.ShiftInUse && Main.mouseRight && Main.mouseItem.NullOrAir()) {
								bool unloaded = item.ModItem is UnloadedItem;
								if (MasterUIManager.RightMouseClicked && !item.NullOrAir() && !item.favorited) {
									int type = item.type;
									bool allowed = CanBeStored(item);
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
								if (!doClickInteractions && MasterUIManager.LeftMouseClicked && (item.NullOrAir() || Main.mouseItem.type == item.type) && !CanBeStored(Main.mouseItem)) {
									if (!unloaded && TryAddToPlayerWhitelist(Main.mouseItem.type))
										SoundEngine.PlaySound(SoundID.ResearchComplete);
								}

								if (doClickInteractions || CanBeStored(Main.mouseItem) || Main.mouseItem.type == item.type || unloaded)
									slotData.ClickInteractions(ref item);
							}
						}
					}

					if (!item.IsAir && !item.favorited && item.TryGetGlobalItem(out VacuumToStorageItem vacummItem) && vacummItem.favorited)
						item.favorited = true;

					slotNum++;
					if (slotNum %  itemSlotColumns == 0) {
						itemSlotX = itemSlotsLeft;
						itemSlotY += itemSlotSpaceHeight;
					}
					else {
						itemSlotX += itemSlotSpaceWidth;
					}
				}
			}

			//Search bar
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
			int bagUIID = GetUI_ID(Bag_UI_ID.BagButtons);
			for (int buttonIndex = 0; buttonIndex <  textButtons.Count; buttonIndex++) {
				UITextData textButton = textButtons[buttonIndex];
				int allButtonPropertiesIndex = textButton.ID - bagUIID;
				if (MasterUIManager.MouseHovering(textButton, true)) {
					ButtonScale[allButtonPropertiesIndex] += 0.05f;

					if (ButtonScale[allButtonPropertiesIndex] > buttonScaleMaximum)
						ButtonScale[allButtonPropertiesIndex] = buttonScaleMaximum;

					if (MasterUIManager.LeftMouseClicked) {
						AllButtonProperites[allButtonPropertiesIndex].OnClicked(this);
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
						scrollPanelY = Main.mouseY - scrollPanelHeight / 2;
						scrollPanelY.Clamp(scrollPanelMinY, scrollPanelMaxY);
						scrollPanelData.SetCenterY(scrollPanelY);

						scrollPanelData.TryStartDraggingUI();
					}
				}

				if (scrollPanelData.ShouldDragUI()) {
					MasterUIManager.DragUI(out _, out scrollPanelY);
				}
				else if (draggingScrollPanel) {
					int scrollPanelRange = scrollPanelMaxY - scrollPanelMinY;
					scrollPanelPosition = ((scrollPanelY - scrollPanelMinY) * possiblePanelPositions).RoundDivide(scrollPanelRange);
				}
			}

			//Resize Panel Hover and Drag
			if (resizePanelData.MouseHovering()) {
				if (MasterUIManager.DoubleClick) {
					Storage.UIResizePanelX = Storage.UIResizePanelDefaultX;
					Storage.UIResizePanelY = Storage.UIResizePanelDefaultY;
				}
				else {
					resizePanelData.TryStartDraggingUI();
				}
			}

			if (resizePanelData.ShouldDragUI()) {
				MasterUIManager.DragUI(out Storage.UIResizePanelX, out Storage.UIResizePanelY);
			}

			//Panel Hover and Drag
			if (panel.MouseHovering()) {
				if (MasterUIManager.DoubleClick) {
					int x = Storage.UIResizePanelX - Storage.UILeft;
					int y = Storage.UIResizePanelY - Storage.UITop;
					Storage.UIResizePanelX = Storage.LastUIResizePanelX + Storage.UILeft;
					Storage.UIResizePanelY = Storage.LastUIResizePanelY + Storage.UITop;
					Storage.LastUIResizePanelX = x;
					Storage.LastUIResizePanelY = y;
				}
				else {
					panel.TryStartDraggingUI();
				}
			}

			if (panel.ShouldDragUI()) {
				int left = Storage.UILeft;
				int top = Storage.UITop;
				MasterUIManager.DragUI(out Storage.UILeft, out Storage.UITop);
				Storage.UIResizePanelX += Storage.UILeft - left;
				Storage.UIResizePanelY += Storage.UITop - top;
			}

			if (displayScrollbar) {
				int scrollWheelTicks = MasterUIManager.ScrollWheelTicks;
				if (scrollWheelTicks != 0 && Hovering && MasterUIManager.NoPanelBeingDragged) {
					if (scrollPanelPosition > 0 && scrollWheelTicks < 0 || scrollPanelPosition < possiblePanelPositions && scrollWheelTicks > 0) {
						SoundEngine.PlaySound(SoundID.MenuTick);
						scrollPanelPosition += scrollWheelTicks;
					}
				}
			}
			
			selectedItemSlots.Clear();
		}
		public void OpenBag() {
			scrollPanelY = int.MinValue;
			Main.playerInventory = true;
			Storage.DisplayBagUI = true;
			Main.LocalPlayer.chest = -1;
			if (MagicStorageIntegration.MagicStorageIsOpen())
				MagicStorageIntegration.TryClosingMagicStorage();
		}
		public void CloseBag(bool noSound = false) {
			scrollPanelY = int.MinValue;
			Storage.DisplayBagUI = false;
			MasterUIManager.TryResetSearch(SearchID);
			if (Main.LocalPlayer.chest == -1) {
				if (!noSound)
					SoundEngine.PlaySound(SoundID.Grab);
			}
		}
		
		public void LootAll() {
			Item[] inv = Inventory;
			for (int i = 0; i < inv.Length; i++) {
				Item item = inv[i];
				if (item.type > ItemID.None && !item.favorited) {
					inv[i] = Main.LocalPlayer.GetItem(Main.myPlayer, inv[i], GetItemSettings.LootAllSettings);
				}
			}
		}
		
		#region Single

		public bool CanBeStored(Item item) => !item.NullOrAir() && Storage.ItemAllowedToBeStored(item);
		public bool VacuumAllowed(Item item) => Storage.IsVacuumBag == true || Storage.IsVacuumBag == null && ContainsItem(item);
		public bool ContainsItem(Item item) => Storage.ContainsSlow(item);
		public bool RoomInStorage(Item item) {
			Item[] inv = Inventory;
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
		public bool CanDeposit(Item item) {
			if (item.NullOrAir())
				return false;

			if (!CanBeStored(item))
				return false;

			if (!RoomInStorage(item))
				return false;

			return true;
		}
		public bool CanVacuumItem(Item item, Player player, bool ignoreNeedBagInInventory = false) {
			if (item.NullOrAir())
				return false;

			if (!Storage.ShouldVacuum)
				return false;

			if (!CanBeStored(item))
				return false;

			//If bag is a "Quick Stack" only style bag, check if item is already in inventory
			if (!VacuumAllowed(item))
				return false;

			if (!ignoreNeedBagInInventory && !Storage.HasRequiredItemToUseStorageSlow(player))
				return false;
			//return false;
			if (!RoomInStorage(item))
				return false;

			return true;
		}
		public bool TryVacuumItem(ref Item item, Player player, bool ignoreNeedBagInInventory = false, bool playSound = true) {
			if (CanVacuumItem(item, player, ignoreNeedBagInInventory))
				return Deposit(ref item, playSound);

			return false;
		}
		public bool Deposit(ref Item item, bool playSound = true) {
			if (item.NullOrAir())
				return false;

			if (item.favorited)
				return false;

			if (Restock(ref item))
				return true;

			int index = 0;
			Item[] inv = Inventory;
			while (!inv[index].IsAir && index < inv.Length) {
				index++;
			}

			if (index == inv.Length)
				return false;

			inv[index] = item.Clone();
			item.TurnToAir();
			if (playSound)
				SoundEngine.PlaySound(SoundID.Grab);

			return true;
		}
		public bool Restock(ref Item item, bool playSound = true) {
			Item[] bagInventory = Inventory;
			for (int i = 0; i < bagInventory.Length; i++) {
				Item bagItem = bagInventory[i];
				if (!bagItem.NullOrAir() && bagItem.type == item.type && bagItem.stack < bagItem.maxStack) {
					if (ItemLoader.TryStackItems(bagItem, item, out _)) {
						if (item.stack < 1) {
							item.TurnToAir();
							if (playSound)
								SoundEngine.PlaySound(SoundID.Grab);

							return true;
						}
					}
				}
			}

			return false;
		}
		public bool CanShiftClickNonBagItemToBag(Item item) {
			if (!DisplayBagUI)
				return false;

			if (!CanDeposit(item))
				return false;

			return true;
		}
		public bool TryShiftClickNonBagItemToBag(ref Item item) {
			if (CanShiftClickNonBagItemToBag(item))
				return Deposit(ref item);

			return false;
		}

		#endregion

		#region Multiple

		public bool DepositAll(IEnumerable<Item> inv, bool playSound = true) {
			IEnumerable<Item> items = inv.Where(i => !i.NullOrAir() && !i.favorited && CanBeStored(i));
			bool transferedAnyItem = Restock(items, false);
			int index = 0;
			Item[] bagInventory = Inventory;
			foreach (Item item in items) {
				if (item.NullOrAir())
					continue;

				while (!bagInventory[index].IsAir && index < bagInventory.Length) {
					index++;
				}

				if (index >= bagInventory.Length)
					break;

				bagInventory[index] = item.Clone();
				item.TurnToAir();
				transferedAnyItem = true;
			}

			if (transferedAnyItem) {
				if (playSound)
					SoundEngine.PlaySound(SoundID.Grab);

				Recipe.FindRecipes(true);
			}

			return transferedAnyItem;
		}
		public bool Restock(IEnumerable<Item> inv, bool playSound = true) {
			Item[] bagInventory = Inventory;
			bool transferedAnyItem = false;
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

		#endregion

		public bool QuickStack(ref Item item, Player player, bool ignoreTile = false, bool playSound = true) {
			if (ContainsItem(item))
				return TryVacuumItem(ref item, player, ignoreTile, playSound);

			return false;
		}
		public void QuickStack(Item[] inv, Player player) {
			for (int i = 0; i < inv.Length; i++) {
				ref Item item = ref inv[i];
				if (item.NullOrAir())
					continue;

				if (item.favorited)
					continue;

				QuickStack(ref item, player);
			}
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

			//prevent glow on bags that select items.
			if (selectedItemSlots.Count == 0) {
				glowTime = 300;
				glowHue = 0.5f;
			}
		}
		private void ToggleVacuum() {
			Storage.ShouldVacuum = !Storage.ShouldVacuum;
		}
		public void AddSelectedItemSlots(IEnumerable<KeyValuePair<int, int>> selectedItemSlots) {
			foreach (KeyValuePair<int, int> selectedItemSlot in selectedItemSlots) {
				AddSelectedItemSlot(selectedItemSlot.Key, selectedItemSlot.Value);
			}
		}
		public void AddSelectedItemSlot(int selectedItemSlot, int context) => selectedItemSlots.TryAdd(selectedItemSlot, context);
		private void UpdateSelectedItemSlots() {
			Storage.SelectItemSlotFunc();
			if (selectedItemSlots.Count == 1) {
				Item selectedItem = Inventory[selectedItemSlots.First().Key];
				if (selectedItem.stack < 1)
					selectedItem.favorited = false;
			}
		}
		public bool TryAddToPlayerWhitelist(int type) {
			if (Storage.TryAddToPlayerWhitelist(type)) {
				StorageManager.AddToPlayerWhitelist(StorageID, type);
				return true;
			}

			return false;
		}
		public bool TryAddToPlayerBlacklist(int type) {
			if (Storage.TryAddToPlayerBlacklist(type)) {
				StorageManager.AddToPlayerBlacklist(StorageID, type);
				return true;
			}

			return false;
		}
	}
}
