using androLib.Common.Configs;
using androLib.Common.Utility;
using androLib.ModIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace androLib.UI
{
	public static class MagicStorageButtonsUI {
		public static void RegisterWithMasterUIManager() {
			UI_ID.MagicStorageDepositAll_UITypeID = MasterUIManager.RegisterUI_ID();
			MasterUIManager.IsDisplayingUI.Add(() => IsOpen);
			MasterUIManager.DrawAllInterfaces += PostDrawInterface;
			MasterUIManager.UpdateInterfaces += UpdateInterface;
		}
		private static int GetUI_ID(int id) => MasterUIManager.GetUI_ID(id, UI_ID.MagicStorageDepositAll_UITypeID);
		public static int ID => GetUI_ID(UI_ID.MagicStorageButtonsUI);
		public static int MagicStorageButtonsUILeft => 280;
		public static int MagicStorageButtonsUITop => 265;
		public static Color PanelColor => new Color(44, 57, 105, ConfigValues.UIAlpha);
		private static int Spacing => 4;
		private static int ButtonBorderY => 0;
		private static int ButtonBorderX => 6;
		public static Color BackGroundColor => new Color(44, 57, 105, ConfigValues.UIAlpha);
		public static Color HoverColor => new Color(100, 118, 184, ConfigValues.UIAlphaHovered);
		public static bool IsOpen {
			get {
				isOpen = MagicStorageIntegration.MagicStorageIsOpen();

				return isOpen;
			}
		}
		private static bool isOpen = false;

		private static DrawnUIData drawnUIData;
		public class DrawnUIData {
			public UIButtonData DepositAllData;
		}
		public static void PostDrawInterface(SpriteBatch spriteBatch) {
			if (!isOpen)
				return;

			drawnUIData = new DrawnUIData();

			//Start of UI
			Color mouseColor = MasterUIManager.MouseColor;

			//Deposit All Data 1/2
			int depositAllTop = MagicStorageButtonsUITop + Spacing + ButtonBorderY;
			int depositAllLeft = MagicStorageButtonsUILeft + Spacing + ButtonBorderX;
			string depositAll = MagicStorageButtonsTextID.DepositAllFromVacuumBags.ToString().Lang(AndroMod.ModName, L_ID1.MagicStorageButtonsText);
			TextData depositAllTextData = new(depositAll);

			//Deposit All Data 2/2
			UIButtonData depositAllData = new(GetUI_ID(UI_ID.MagicStorageDepositAllButton), depositAllLeft, depositAllTop, depositAllTextData, mouseColor, ButtonBorderX, ButtonBorderY, BackGroundColor, HoverColor, true);
			drawnUIData.DepositAllData = depositAllData;

			//Deposit All Button Draw
			depositAllData.Draw(spriteBatch);
		}
		public static void UpdateInterface() {
			if (!isOpen)
				return;

			UIButtonData depositAllData = drawnUIData.DepositAllData;

			if (depositAllData.MouseHovering()) {
				if (MasterUIManager.LeftMouseClicked) {
					DepositAllToMagicStorage();
				}
			}
		}
		private static void DepositAllToMagicStorage() {
			foreach (Storage storage in StorageManager.BagUIs.Select(b => b.Storage)) {
				if (storage.HasRequiredItemToUseStorage(Main.LocalPlayer, out _, out _)) {
					MagicStorageIntegration.DepositToMagicStorage(storage.Items);
				}
			}
		}
	}
}
