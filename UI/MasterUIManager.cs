﻿using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;
//using androLib.Common.Configs;
using androLib.Common.Globals;
using androLib.Common.Utility;
//using androLib.Content.NPCs;
using androLib;
using static androLib.UI.MasterUIManager;

namespace androLib.UI
{
	public static class MasterUIManager {
		//TODO: Re-evaluate the need for any of this.  Probably taking care of everything via StorageManager and BagUI.

		public static event Action UpdateUIAlpha;

		public static BoolCheck IsDisplayingUI = new();
		public static BoolCheck ShouldPreventTrashingItem = new();
		public static BoolCheck ShouldPreventRecipeScrolling = new();

		public delegate void DrawInterface(SpriteBatch spriteBatch);
		public static event DrawInterface DrawAllInterfaces;

		public delegate void UpdateInterface();
		public static event UpdateInterface UpdateInterfaces;

		private static int UI_ID_Counter = 0;
		public static int RegisterUI_ID() {
			int ui_id = UI_ID_Counter;

			UI_ID_Counter++;

			return ui_id;
		}

		public static int GetUI_ID(int ui_id, int UITypeID) => ui_id + UITypeID * 1000;
		public static bool HoveringMyUIType(int uiTypeID) => UI_IDToTypeID(UIBeingHovered) == uiTypeID;
		public static bool HovingUIByID(int ui_id) => UIBeingHovered == ui_id;
		public static int UI_IDToTypeID(int ui_id) => ui_id / 1000;

		public static bool DisplayingAnyUI => IsDisplayingUI.Invoke();
		public static bool LastDisplayingAnyUI = false;
		public static bool NoPanelBeingDragged => PanelBeingDragged == UI_ID.None;
		public static bool NoUIBeingHovered => UIBeingHovered == UI_ID.None;
		private static int mouseOffsetX = 0;
		private static int mouseOffsetY = 0;
		public static bool lastMouseLeft = false;
		public static bool lastMouseRight = false;
		public static bool LeftMouseClicked => Main.mouseLeft && !lastMouseLeft;
		public static bool RightMouseClicked => Main.mouseRight && !lastMouseRight;
		public static bool LeftMouseDown = false;
		public static Color MouseColor {
			get {
				Color mouseColor = Color.White * (1f - (255f - (float)(int)Main.mouseTextColor) / 255f * 0.5f);
				mouseColor.A = byte.MaxValue;
				return mouseColor;
			}
		}
		public static readonly Asset<Texture2D>[] uiTextures = { Main.Assets.Request<Texture2D>("Images/UI/PanelBackground"), Main.Assets.Request<Texture2D>("Images/UI/PanelBorder") };
		public static readonly int ItemSlotSize = 44;
		public static int ItemSlotInteractContext => Main.mouseRight && !ItemSlot.ShiftInUse ? ItemSlot.Context.InventoryItem : ItemSlot.Context.BankItem;
		public static int PanelBeingDragged { get; private set; } = UI_ID.None;
		public static int UIBeingHovered = UI_ID.None;
		public static int LastUIBeingHovered { get; private set; } = UI_ID.None;
		public static int HoverTime = 0;
		public static int ScrollWheel = 0;
		public static int LastScrollWheel = 0;
		public static int ScrollWheelTicks => (LastScrollWheel - ScrollWheel) / 120;
		public static int FocusRecipe = Main.focusRecipe;
		public static int LastFocusRecipe = Main.focusRecipe;
		public static int SearchBarTimer = 0;
		public static bool ShouldShowSearchBarHeartbeat => SearchBarTimer % 60 >= 30;
		public static string SearchBarString = "";
		public static int SearchBarInUse = UI_ID.None;
		public static bool TypingOnAnySearchBar = false;
		public static void PostDrawInterface(SpriteBatch spriteBatch) {
			StoragePlayer genericModPlayer = StoragePlayer.LocalStoragePlayer;
			if (genericModPlayer.disableLeftShiftTrashCan) {
				ItemSlot.Options.DisableLeftShiftTrashCan = true;
				genericModPlayer.disableLeftShiftTrashCan = false;
			}

			if (DisplayingAnyUI) {
				if (NoPanelBeingDragged) {
					if (!NoUIBeingHovered && UIBeingHovered == LastUIBeingHovered) {
						HoverTime++;
					}
					else {
						HoverTime = 0;
					}

					LastUIBeingHovered = UIBeingHovered;
					UIBeingHovered = UI_ID.None;
				}

				if (TypingOnAnySearchBar) {
					SearchBarTimer++;
				}
				else {
					SearchBarTimer = 0;
				}

				LastScrollWheel = ScrollWheel;
				ScrollWheel = (int)typeof(Mouse).GetField("INTERNAL_MouseWheel", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
				LastFocusRecipe = FocusRecipe;
				FocusRecipe = Main.focusRecipe;
				float savedInventoryScale = Main.inventoryScale;
				Main.inventoryScale = 0.86f;

				//If UI just opened, reset scroll wheel to prevent sudden scroll wheel change on opening
				if (!LastDisplayingAnyUI)
					LastScrollWheel = ScrollWheel;

				bool preventTrashingItem = ShouldPreventTrashingItem.Invoke();
				if (preventTrashingItem) {
					//Disable Left Shift to Quick trash
					if (ItemSlot.Options.DisableLeftShiftTrashCan) {
						genericModPlayer.disableLeftShiftTrashCan = ItemSlot.Options.DisableLeftShiftTrashCan;
						ItemSlot.Options.DisableLeftShiftTrashCan = false;
					}
				}

				DrawAllInterfaces?.Invoke(spriteBatch);
				if (UpdateInterfaces != null) {
					Delegate[] invocationList = UpdateInterfaces.GetInvocationList();
					for (int i = invocationList.Length - 1; i >= 0; i--) {
						UpdateInterface updateInterface = (UpdateInterface)invocationList[i];
						updateInterface?.Invoke();
					}
				}

				Main.inventoryScale = savedInventoryScale;
			}
			else {
				UIBeingHovered = UI_ID.None;
			}

			lastMouseLeft = Main.mouseLeft;
			lastMouseRight = Main.mouseRight;
			LastDisplayingAnyUI = DisplayingAnyUI;
		}
		
		public static void PostUpdateEverything() {
			if (Main.focusRecipe != FocusRecipe && ShouldPreventRecipeScrolling.Invoke())
				Main.focusRecipe = FocusRecipe;
		}
		public static string DisplayedSearchBarString(int SearchBarID) {
			if (!UsingSearchBar(SearchBarID))
				return StorageTextID.Search.ToString().Lang(AndroMod.ModName, L_ID1.StorageText);

			return $"{(SearchBarString.Length > 15 ? SearchBarString.Substring(SearchBarString.Length - 15) : SearchBarString)}{Main.chatText}{(ShouldShowSearchBarHeartbeat ? "|" : SearchBarString != "" ? "" : " ")}";
		}
		public static bool UsingSearchBar(int ID) => SearchBarInUse == ID;
		public static bool TypingOnSearchBar(int ID) => UsingSearchBar(ID) && TypingOnAnySearchBar;
		public static void TryResetSearch(int ID) {
			if (SearchBarInUse == ID)
				ResetSearch();
		}
		public static void ResetSearch() {
			SearchBarString = "";
			SearchBarInUse = UI_ID.None;
			TypingOnAnySearchBar = false;
		}
		public static void StopTypingOnSearchBar() {
			TypingOnAnySearchBar = false;
			if (SearchBarString == "")
				SearchBarInUse = UI_ID.None;
		}
		public static void ClickSearchBar(int ID) {
			if (UsingSearchBar(ID)) {
				if (TypingOnAnySearchBar) {
					StopTypingOnSearchBar();
				}
				else {
					TypingOnAnySearchBar = true;
				}
			}
			else {
				if (SearchBarInUse != UI_ID.None)
					TryResetSearch(SearchBarInUse);

				StartTypingOnSearchBar(ID);
			}
		}
		public static void StartTypingOnSearchBar(int ID) {
			TypingOnAnySearchBar = true;
			SearchBarInUse = ID;
		}
		public static bool MouseHovering(UIPanel panel, int ID, bool playSound = false) {
			if (NoUIBeingHovered && panel.IsMouseHovering) {
				SetMouseHovering(ID, playSound);
				return true;
			}

			return false;
		}
		public static bool MouseHovering(UIPanelData panel, bool playSound = false) {
			if (NoUIBeingHovered && panel.IsMouseHovering) {
				SetMouseHovering(panel.ID, playSound);
				return true;
			}

			return false;
		}
		public static bool MouseHovering(UITextData text, bool playSound = false) {
			if (NoUIBeingHovered && text.IsMouseHovering) {
				SetMouseHovering(text.ID, playSound);
				return true;
			}

			return false;
		}
		public static bool MouseHovering(UIButtonData button, bool playSound = false, bool shouldSet = true) {
			if (NoUIBeingHovered && button.IsMouseHovering) {
				if (shouldSet)
					SetMouseHovering(button.ID, playSound);

				return true;
			}

			return false;
		}
		private static void SetMouseHovering(int ID, bool playSound = false) {
			UIBeingHovered = ID;
			Main.LocalPlayer.mouseInterface = true;
			if (playSound && LastUIBeingHovered != ID)
				SoundEngine.PlaySound(SoundID.MenuTick);
		}
		public static void TryStartDraggingUI(UIPanelData panel) {
			if (PanelBeingDragged == UI_ID.None) {
				if (LeftMouseClicked)
					StartDraggingUI(panel);
			}
		}
		public static void StartDraggingUI(UIPanel panel, int UI_ID) {
			StartDraggingUI((int)panel.Left.Pixels, (int)panel.Top.Pixels, UI_ID);
		}
		public static void StartDraggingUI(int panelX, int panelY, int UI_ID) {
			PanelBeingDragged = UI_ID;
			mouseOffsetX = panelX - Main.mouseX;
			mouseOffsetY = panelY - Main.mouseY;
		}
		public static void StartDraggingUI(UIPanelData panel) {
			PanelBeingDragged = panel.ID;
			Point topLeft = panel.TopLeft;
			mouseOffsetX = topLeft.X - Main.mouseX;
			mouseOffsetY = topLeft.Y - Main.mouseY;
		}
		public static bool ShouldDragUI(UIPanelData panel) => ShouldDragUI(panel.ID);
		public static bool ShouldDragUI(int ID) {
			if (PanelBeingDragged == ID) {
				if (!Main.mouseLeft) {
					PanelBeingDragged = UI_ID.None;
				}
				else {
					return true;
				}
			}

			return false;
		}
		public static void DragUI(out int panelX, out int panelY) {
			panelX = Main.mouseX + mouseOffsetX;
			panelY = Main.mouseY + mouseOffsetY;
		}
		public static void DragUI(UIPanel panel) {
			DragUI(out int panelX, out int panelY);
			panel.Left.Pixels = panelX;
			panel.Top.Pixels = panelY;
		}
		public static void CheckOutOfBoundsRestoreDefaultPosition(ref int uiX, ref int uiY, int defaultX, int defaultY) {
			if (uiX <= 10 || uiX >= Main.screenWidth - 10 || uiY <= 10 || uiY >= Main.screenHeight - 10) {
				uiX = defaultX;
				uiY = defaultY;
			}
		}
		public static void DrawUIPanel(SpriteBatch spriteBatch, UIPanelData panel, Color panelColor) {
			DrawUIPanel(spriteBatch, panel.TopLeft.X, panel.TopLeft.Y, panel.BottomRight.X, panel.BottomRight.Y, panelColor);
		}
		public static void DrawUIPanel(SpriteBatch spriteBatch, Point panelTopLeft, Point panelBottomRight, Color panelColor) {
			DrawUIPanel(spriteBatch, panelTopLeft.X, panelTopLeft.Y, panelBottomRight.X, panelBottomRight.Y, panelColor);
		}
		public static void DrawUIPanel(SpriteBatch spriteBatch, Vector2 panelTopLeft, Vector2 panelBottomRight, Color panelColor) {
			DrawUIPanel(spriteBatch, (int)panelTopLeft.X, (int)panelTopLeft.Y, (int)panelBottomRight.X, (int)panelBottomRight.Y, panelColor);
		}
		public static void DrawUIPanel(SpriteBatch spriteBatch, int Left, int Top, int Right, int Bottom, Color panelColor) {
			int _barSize = 4;
			int cornerSize = 12;
			Right -= cornerSize;
			Bottom -= cornerSize;
			int width = Right - Left - cornerSize;
			int height = Bottom - Top - cornerSize;
			Color[] colors = { panelColor, Color.Black };
			for (int i = 0; i < uiTextures.Length; i++) {
				Texture2D texture = uiTextures[i].Value;
				Color color = colors[i];
				spriteBatch.Draw(texture, new Rectangle(Left, Top, cornerSize, cornerSize), new Rectangle(0, 0, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(Right, Top, cornerSize, cornerSize), new Rectangle(cornerSize + _barSize, 0, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(Left, Bottom, cornerSize, cornerSize), new Rectangle(0, cornerSize + _barSize, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(Right, Bottom, cornerSize, cornerSize), new Rectangle(cornerSize + _barSize, cornerSize + _barSize, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(Left + cornerSize, Top, width, cornerSize), new Rectangle(cornerSize, 0, _barSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(Left + cornerSize, Bottom, width, cornerSize), new Rectangle(cornerSize, cornerSize + _barSize, _barSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(Left, Top + cornerSize, cornerSize, height), new Rectangle(0, cornerSize, cornerSize, _barSize), color);
				spriteBatch.Draw(texture, new Rectangle(Right, Top + cornerSize, cornerSize, height), new Rectangle(cornerSize + _barSize, cornerSize, cornerSize, _barSize), color);
				spriteBatch.Draw(texture, new Rectangle(Left + cornerSize, Top + cornerSize, width, height), new Rectangle(cornerSize, cornerSize, _barSize, _barSize), color);
			}
		}
		public static void DrawItemSlot(SpriteBatch spriteBatch, Item item, UIItemSlotData slot, int context = ItemSlotContextID.Normal, float hue = 0f, int glowTime = 0, int stack = int.MinValue) {
			DrawItemSlot(spriteBatch, item, slot.TopLeft.X, slot.TopLeft.Y, context, hue, glowTime, stack);
		}
		public static void DrawItemSlot(SpriteBatch spriteBatch, Item item, int itemSlotX, int itemSlotY, int context = ItemSlotContextID.Normal, float hue = 0f, int glowTime = 0, int stack = int.MinValue) {
			//ItemSlot.Draw(spriteBatch, ref item, context, new Vector2(itemSlotX, itemSlotY));
			if (stack == int.MinValue)
				stack = item.stack;

			float inventoryScale = Main.inventoryScale;
			Color color = Color.White;
			Vector2 position = new(itemSlotX, itemSlotY);

			Texture2D texture;
			Color color2 = Main.inventoryBack;

			switch (context) {
				case ItemSlotContextID.Purple when !item.favorited:
					texture = TextureAssets.InventoryBack4.Value;//Purple
					break;
				case ItemSlotContextID.Purple when item.favorited:
					texture = (Texture2D)ModContent.Request<Texture2D>("WeaponEnchantments/UI/Sprites/Inventory_Back4(Favorited)");
					break;
				case ItemSlotContextID.Red:
					texture = TextureAssets.InventoryBack5.Value;
					break;
				case 6:
					texture = TextureAssets.InventoryBack6.Value;
					break;
				case 7:
					texture = TextureAssets.InventoryBack7.Value;
					break;
				case 8:
					texture = TextureAssets.InventoryBack8.Value;
					break;
				case 9:
					texture = TextureAssets.InventoryBack9.Value;
					break;
				case ItemSlotContextID.Favorited:
					texture = TextureAssets.InventoryBack10.Value;
					break;
				case 11:
					texture = TextureAssets.InventoryBack11.Value;
					break;
				case 12:
					texture = TextureAssets.InventoryBack12.Value;
					break;
				case 13:
					texture = TextureAssets.InventoryBack13.Value;
					break;
				case 14:
					texture = TextureAssets.InventoryBack14.Value;
					break;
				case 15:
					texture = TextureAssets.InventoryBack15.Value;
					break;
				case 16:
					texture = TextureAssets.InventoryBack16.Value;
					break;
				case ItemSlotContextID.Gold:
					texture = TextureAssets.InventoryBack17.Value;
					break;
				case 18:
					texture = TextureAssets.InventoryBack18.Value;
					break;
				default:
					texture = item.favorited ? TextureAssets.InventoryBack10.Value : TextureAssets.InventoryBack.Value;
					break;
			}

			//Glow
			if (hue != 0f && !item.favorited && !item.IsAir) {
				float num5 = Main.invAlpha / 255f;
				Color value2 = new Color(63, 65, 151, 255) * num5;
				Color value3 = Main.hslToRgb(hue, 1f, 0.5f) * num5;
				float num6 = (float)glowTime / 300f;
				num6 *= num6;
				color2 = Color.Lerp(value2, value3, num6 / 2f);
				texture = TextureAssets.InventoryBack13.Value;
			}

			//Draw ItemSlot
			spriteBatch.Draw(texture, position, null, color2, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);

			Vector2 vector = texture.Size() * inventoryScale;
			if (item.type > ItemID.None && item.stack > 0) {
				//Trash Can
				if (context == ItemSlotContextID.MarkedTrash) {
					Texture2D value11 = TextureAssets.Trash.Value;
					Vector2 position4 = position + texture.Size() * inventoryScale / 2f - value11.Size() * inventoryScale / 2f * 1.5f;
					spriteBatch.Draw(value11, position4, null, new Color(100, 100, 100, 100), 0f, default(Vector2), inventoryScale * 1.5f, SpriteEffects.None, 0f);
				}

				//Draw Item
				ItemSlot.DrawItemIcon(item, 5, spriteBatch, position + vector / 2f, inventoryScale, 32f, color);

				//Draw Stack
				if (item.stack > 1 || stack != item.stack)
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, stack.ToString(), position + new Vector2(10f, 26f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
			}
		}
		public static bool MouseHoveringItemSlot(int itemSlotX, int itemSlotY, int ID) {
			if (NoUIBeingHovered && Main.mouseX >= itemSlotX && Main.mouseX <= itemSlotX + ItemSlotSize && Main.mouseY >= itemSlotY && Main.mouseY <= itemSlotY + ItemSlotSize && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
				UIBeingHovered = ID;
				return true;
			}

			return false;
		}
		public static void ItemSlotClickInteractions(ref Item item, int context = -1) {
			if (context == -1)
				context = ItemSlotInteractContext;

			ItemSlot.Handle(ref item, context);
		}
		public static void SwapMouseItem(ref Item item1) {
			Item stored = item1.Clone();
			item1 = Main.mouseItem.Clone();
			if (!item1.NullOrAir() && item1.stack <= 0)
				item1.TurnToAir();

			Main.mouseItem = stored;
			if (!Main.mouseItem.NullOrAir() && Main.mouseItem.stack <= 0)
				Main.mouseItem.TurnToAir();

			SoundEngine.PlaySound(SoundID.Grab);
		}
		public static void SortItems(ref Item[] inv, bool updateOrder = true) {
			for (int i = 0; i < inv.Length; i++) {
				ref Item item = ref inv[i];
				if (item == null) {
					item = new();
					continue;
				}

				if (item.IsAir || item.favorited || item.stack < 1 || item.maxStack <= item.stack)
					continue;

				int num = item.maxStack - item.stack;
				for (int j = i + 1; j < inv.Length; j++) {
					ref Item item2 = ref inv[j];
					if (item2 == null) {
						item2 = new();
						continue;
					}

					if (item2.stack < 1 || item2.IsAir || item2.favorited)
						continue;

					if (item.type == item2.type && item2.stack < item2.maxStack) {
						
						if (!ItemLoader.TryStackItems(item, item2, out int numTransfered))
							continue;
						
						num -= numTransfered;

						if (item2.stack <= 0) {
							item2 = new();
							Item temp = inv[j];
							continue;
						}

						if (num <= 0)
							break;
					}
				}
			}

			if (updateOrder)
				inv = inv.OrderByDescending(i => i.type).ToArray();
		}
	}
	
	public struct UIPanelData {
		public Point TopLeft;
		public Point BottomRight;
		public int Width => BottomRight.X - TopLeft.X;
		public int Height => BottomRight.Y - TopLeft.Y;
		public int ID;
		public Color Color;
		public UIPanelData(int id, int left, int top, int width, int height, Color color) {
			ID = id;
			TopLeft = new Point(left, top);
			BottomRight = new Point(left + width, top + height);
			Color = color;
		}
		public UIPanelData(int id, Point topLeft, Point bottomRight, Color color) {
			ID = id;
			TopLeft = topLeft;
			BottomRight = bottomRight;
			Color = color;
		}
		public bool IsMouseHovering => Main.mouseX >= TopLeft.X && Main.mouseX <= BottomRight.X && Main.mouseY >= TopLeft.Y && Main.mouseY <= BottomRight.Y && !PlayerInput.IgnoreMouseInterface;
		public Point Center => new((TopLeft.X + BottomRight.X) / 2, (TopLeft.Y + BottomRight.Y) / 2);
		public void Draw(SpriteBatch spriteBatch) => MasterUIManager.DrawUIPanel(spriteBatch, this, Color);
		public bool MouseHovering() => MasterUIManager.MouseHovering(this);
		public void TryStartDraggingUI() => MasterUIManager.TryStartDraggingUI(this);
		public bool ShouldDragUI() => MasterUIManager.ShouldDragUI(this);
		public void SetCenterY(int centerY) {
			int height = Height;
			TopLeft.Y = centerY;
			BottomRight.Y = centerY + height;
		} 
	}
	public struct UITextData {
		public int ID;
		public string Text;
		public float Scale;
		public Color Color;
		public bool Center;
		public bool AncorBotomLeft;
		Vector2 BaseTextSize;
		Vector2 TextSize;
		Vector2 Position => AncorBotomLeft ? new(0, BaseTextSize.Y / 2) : Vector2.Zero;
		public int Width => (int)TextSize.X;
		public int Height => (int)TextSize.Y;
		public int BaseWidth => (int)BaseTextSize.X;
		public int BaseHeight => (int)BaseTextSize.Y;
		public Point TopLeft;
		public Point BottomRight;
		public UITextData(int id, int left, int top, string text, float scale, Color color, bool center = false, bool ancorBotomLeft = false) {
			ID = id;
			Text = text;
			Scale = scale;
			Center = center;
			Color = color;
			AncorBotomLeft = ancorBotomLeft;
			Vector2 baseSize = text != null ? FontAssets.MouseText.Value.MeasureString(Text) : Vector2.Zero;
			BaseTextSize = baseSize;
			Vector2 size = baseSize * scale;
			TextSize = size;
			int heightOffset = ancorBotomLeft ? (int)baseSize.Y / 2 : 0;
			TopLeft = new Point(left, top + heightOffset);
			BottomRight = new Point(left + (int)size.X, top + (int)size.Y + heightOffset);
		}
		public UITextData(int id, int left, int top, TextData textData, Color color, bool center = false, bool ancorBotomLeft = false) {
			ID = id;
			Text = textData.Text;
			Scale = textData.Scale;
			Center = center;
			Color = color;
			AncorBotomLeft = ancorBotomLeft;
			Vector2 baseSize = textData.Text != null ? FontAssets.MouseText.Value.MeasureString(Text) : Vector2.Zero;
			BaseTextSize = baseSize;
			Vector2 size = baseSize * textData.Scale;
			TextSize = size;
			int heightOffset = ancorBotomLeft ? (int)baseSize.Y / 2 : 0;
			TopLeft = new Point(left, top + heightOffset);
			BottomRight = new Point(left + (int)size.X, top + (int)size.Y + heightOffset);
		}
		public bool IsMouseHovering => Main.mouseX >= TopLeft.X && Main.mouseX <= BottomRight.X && Main.mouseY >= TopLeft.Y - Position.Y && Main.mouseY <= BottomRight.Y - Position.Y && !PlayerInput.IgnoreMouseInterface;
		public bool MouseHovering() => MasterUIManager.MouseHovering(this, true);
		public void Draw(SpriteBatch spriteBatch) {
			int left = Center ? TopLeft.X - Width / 2: TopLeft.X;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, Text, new Vector2(left, TopLeft.Y), Color, 0f, Position, new Vector2(Scale), -1f, 1.5f);
		}
	}
	public struct TextData {
		public string Text;
		public float Scale;
		public int Width => (int)TextSize.X;
		public int Height => (int)TextSize.Y;
		public int BaseWidth => (int)BaseTextSize.X;
		public int BaseHeight => (int)BaseTextSize.Y;
		public Vector2 BaseTextSize;
		public Vector2 TextSize;
		public TextData(string text, float scale = 1f) {
			Text = text;
			Scale = scale;
			BaseTextSize = text != null ? FontAssets.MouseText.Value.MeasureString(text) : Vector2.Zero;
			TextSize = BaseTextSize * Scale;
		}
	}
	public struct UIButtonData
	{
		public int ID;
		public string Text;
		public float Scale;
		public Color Color;
		Vector2 TextSize;
		Vector2 Borders;
		Vector2 Position => new(0, -2);// TextSize / 2;
		public int Width;
		public int Height;
		public Vector2 TopLeft;
		public Vector2 BottomRight;
		public Color PanelColor;
		public Color HoverColor;
		public UIButtonData(int id, int X, int top, string text, float scale, Color color, int borderWidth, int borderHeight, Color panelColor, Color hoverColor, bool fromRight = false) {
			ID = id;
			Text = text;
			Scale = scale;
			Color = color;
			TextSize = FontAssets.MouseText.Value.MeasureString(text) * scale;
			Borders = new Vector2(borderWidth, borderHeight);
			Width = (int)TextSize.X + borderWidth * 2;
			Height = (int)TextSize.Y + borderHeight * 2;
			TopLeft = new Vector2(X - (fromRight ? Width : 0), top);
			BottomRight = new Vector2(X + (fromRight ? 0 : Width), top + Height);
			PanelColor = panelColor;
			HoverColor = hoverColor;
		}
		public UIButtonData(int id, int X, int top, TextData textData, Color color, int borderWidth, int borderHeight, Color panelColor, Color hoverColor, bool fromRight = false) {
			ID = id;
			Text = textData.Text;
			Scale = textData.Scale;
			Color = color;
			TextSize = textData.TextSize;
			Borders = new Vector2(borderWidth, borderHeight * 2);
			Width = (int)TextSize.X + borderWidth * 2;
			Height = (int)TextSize.Y + borderHeight * 2;
			TopLeft = new Vector2(X - (fromRight ? Width : 0), top);
			BottomRight = new Vector2(X + (fromRight ? 0 : Width), top + Height);
			PanelColor = panelColor;
			HoverColor = hoverColor;
		}
		public bool IsMouseHovering {
			get {
				if (isMouseHovering == null)
					isMouseHovering = Main.mouseX >= TopLeft.X && Main.mouseX <= BottomRight.X && Main.mouseY >= TopLeft.Y - Position.Y && Main.mouseY <= BottomRight.Y - Position.Y && !PlayerInput.IgnoreMouseInterface;

				return (bool)isMouseHovering;
			}
		}
		private bool? isMouseHovering = null;
		public bool MouseHovering() => MasterUIManager.MouseHovering(this, true);
		public void Draw(SpriteBatch spriteBatch) {
			MasterUIManager.DrawUIPanel(spriteBatch, TopLeft, BottomRight, MasterUIManager.MouseHovering(this, false, false) ? HoverColor : PanelColor);
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, Text, TopLeft + Borders, Color, 0f, Position, new Vector2(Scale), -1f, 1.5f);
		}
	}
	public struct UIItemSlotData {
		public Point TopLeft;
		public Point BottomRight;
		public int ID;
		public UIItemSlotData(int id, int left, int top) {
			ID = id;
			TopLeft = new Point(left, top);
			BottomRight = new Point(left + MasterUIManager.ItemSlotSize, top + MasterUIManager.ItemSlotSize);
		}
		public UIItemSlotData(int id, Point topLeft) {
			ID = id;
			TopLeft = topLeft;
			BottomRight = new Point(TopLeft.X + MasterUIManager.ItemSlotSize, TopLeft.Y + MasterUIManager.ItemSlotSize);
		}
		public bool IsMouseHovering => Main.mouseX >= TopLeft.X && Main.mouseX <= BottomRight.X && Main.mouseY >= TopLeft.Y && Main.mouseY <= BottomRight.Y && !PlayerInput.IgnoreMouseInterface;
		public Point Center => new((TopLeft.X + BottomRight.X) / 2, (TopLeft.Y + BottomRight.Y) / 2);
		public void Draw(SpriteBatch spriteBatch, Item item, int context = ItemSlotContextID.Normal, float hue = 0f, int glowTime = 0, int stack = int.MinValue) {
			MasterUIManager.DrawItemSlot(spriteBatch, item, this, context, hue, glowTime, stack);
		}
		public bool MouseHovering() => MasterUIManager.MouseHoveringItemSlot(TopLeft.X, TopLeft.Y, ID);
		public void ClickInteractions(ref Item item, int context = -1) {
			if (context == -1)
				context = ItemSlotInteractContext;

			ItemSlotClickInteractions(ref item, context);
		}
	}
	public static class UI_ID {
		public const int None = -1000;
		public static int MagicStorageDepositAll_UITypeID;//Set by MasterUIManager
		public const int MagicStorageButtonsUI = 0;
		public const int MagicStorageDepositAllButton = 1;

	}
	public static class ItemSlotContextID
	{
		public const int MarkedTrash = -1;
		public const int Normal = 0;
		public const int Purple = 4;
		public const int Red = 5;
		public const int Favorited = 10;
		public const int Gold = 17;
	}
}
