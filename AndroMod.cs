using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using androLib.Common.Configs;
using Terraria.ID;
using Terraria.UI;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using androLib.Common.Globals;
using androLib.UI;
using androLib.Common.Utility;
using androLib.Localization;
using androLib.ModIntegration;

namespace androLib
{
	public class AndroMod : Mod {
		//public static AndroServerConfig serverConfig = ModContent.GetInstance<AndroServerConfig>();
		public static AndroClientConfig clientConfig = ModContent.GetInstance<AndroClientConfig>();
		public static string ModName = ModContent.GetInstance<AndroMod>().Name;
		public const string magicStorageName = "MagicStorage";
		public static bool magicStorageEnabled = false;
		private enum CallID {
			None = -1,

			//Register: int id = Call("Register", Mod mod, Type VacuumStorageType, int StorageSize, Func<int> StorageItemTypeGetter, int UI_DefaultLeftLocationOnScreen, int UI_DefaultTopLocationOnScreen);
			//Example: int id = Call("Register", this, typeof(OreBag), 100, () => ModContent.ItemType<OreBag>(), 80, 675)
			//Register should be callid in your Mod.Load() method.  It needs to happen before ModPlayer.LoadData() is called.
			//The return value is the id that you can use to access your storage in the other calls.
			//StorageSize, StorageItemTypeGetter, UI_DefaultLeftLocationOnScreen, UI_DefaultTopLocationOnScreen are optional
			//StorageItemTypeGetter can be null if you aren't making an item that can be opened from your inventory or you want to take care of it yourself.
			//StorageItemTypeGetter should be a () => ModContent.ItemType<OreBag>(), where OreBag is your ModItem class.
			Register = 0,



			//The rest of the calls just need the id you got from Register.
			//GetItems: Item[] items = Call("GetItems", int storageID);
			//Example: Item[] items = Call("GetItems", storageID); where storageID is the id you got from Register Or GetStorageID.
			//Default value is null. DON'T FORGET TO CHECK FOR NULL!
			GetItems = 1,

			//Used to get the StorageId for an existing bag by internal name such as "PaintBucket"
			//Should be used after Mod.Load() such as in Mod.PostSetupContent()
			//GetStorageID: int storageID = Call("GetStorageID", string modInternalName, string bagInternalName);
			//Example: int storageID = Call("GetStorageID", "VacuumBags", "PaintBucket");
			//Default value is -1.  I check for -1, so no need to check on your end.
			GetStorageID = 2,


			SetUIPosition = 100,
			SetShouldVacuum = 101,
		}
		public override object Call(params object[] args) {
			if (args.Length < 2)
				return -1;

			CallID id;
			int idIndex = 0;
			if (args[idIndex] is int callID) {
				id = (CallID)callID;
			}
			else if (args[idIndex] is not string callIDName || !Enum.TryParse(callIDName, out id)) {
				return -1;
			}

			switch(id) {
				case CallID.Register:
					if (args.Length < 3 || args.Length > 13)
						return -1;

					int modIndex = idIndex + 1;
					if (args[modIndex] is not Mod mod)
						return -1;

					int typeIndex = modIndex + 1;
					if (args[typeIndex] is not Type VacuumStorageType)
						return -1;

					int allowedIndex = typeIndex + 1;
					Func<Item, bool> ItemAllowedToBeStored = (Item item) => true;
					if (args.Length >= allowedIndex + 1 && args[allowedIndex] is Func<Item, bool> ItemAllowedToBeStoredArg)
						ItemAllowedToBeStored = ItemAllowedToBeStoredArg;

					int localizationNameKeyIndex = allowedIndex + 1;
					string localizationNameKey = null;
					if (args.Length >= localizationNameKeyIndex + 1 && args[localizationNameKeyIndex] is string localizationNameKeyArg)
						localizationNameKey = localizationNameKeyArg;

					int StorageSizeIndex = localizationNameKeyIndex + 1;
					int StorageSize = StorageManager.DefaultStorageSize;
					if (args.Length >= StorageSizeIndex + 1 && args[StorageSizeIndex] is int StorageSizeArg)
						StorageSize = StorageSizeArg;

					int ShouldVacuumIndex = StorageSizeIndex + 1;
					bool ShouldVacuum = true;
					if (args.Length >= ShouldVacuumIndex + 1 && args[ShouldVacuumIndex] is bool ShouldVacuumArg)
						ShouldVacuum = ShouldVacuumArg;

					int GetColorIndex = ShouldVacuumIndex + 1;
					Func<Microsoft.Xna.Framework.Color> GetColor = () => new Color(255, 255, 255, androLib.Common.Configs.ConfigValues.UIAlpha);
					if (args.Length >= GetColorIndex + 1 && args[GetColorIndex] is Func<Microsoft.Xna.Framework.Color> GetColorArg)
						GetColor = GetColorArg;

					int ScrollColorIndex = GetColorIndex + 1;
					Func<Microsoft.Xna.Framework.Color> GetScrollBarColor = () => new Color(255, 255, 255, androLib.Common.Configs.ConfigValues.UIAlpha);
					if (args.Length >= ScrollColorIndex + 1 && args[ScrollColorIndex] is Func<Microsoft.Xna.Framework.Color> GetScrollBarColorArg)
						GetScrollBarColor = GetScrollBarColorArg;

					int ButtonHoverColorIndex = ScrollColorIndex + 1;
					Func<Microsoft.Xna.Framework.Color> GetButtonHoverColor = () => new Color(255, 255, 255, androLib.Common.Configs.ConfigValues.UIAlpha);
					if (args.Length >= ButtonHoverColorIndex + 1 && args[ButtonHoverColorIndex] is Func<Microsoft.Xna.Framework.Color> GetButtonHoverColorArg)
						GetButtonHoverColor = GetButtonHoverColorArg;

					int StorageItemTypeGetterIndex = ButtonHoverColorIndex + 1;
					Func<int> StorageItemTypeGetter = () => -1;
					if (args.Length >= StorageItemTypeGetterIndex + 1 && args[StorageItemTypeGetterIndex] is Func<int> StorageItemTypeGetterArg)
						StorageItemTypeGetter = StorageItemTypeGetterArg;

					int leftIndex = StorageItemTypeGetterIndex + 1;
					int UI_DefaultLeftLocationOnScreen = StorageManager.DefaultLeftLocationOnScreen;
					if (args.Length >= leftIndex + 1 && args[leftIndex] is int UI_DefaultLeftLocationOnScreenArg)
						UI_DefaultLeftLocationOnScreen = UI_DefaultLeftLocationOnScreenArg;

					int topIndex = leftIndex + 1;
					int UI_DefaultTopLocationOnScreen = StorageManager.DefaultTopLocationOnScreen;
					if (args.Length >= topIndex + 1 && args[topIndex] is int UI_DefaultTopLocationOnScreenArg)
						UI_DefaultTopLocationOnScreen = UI_DefaultTopLocationOnScreenArg;

					return StorageManager.RegisterVacuumStorageClass(
						mod, 
						VacuumStorageType, 
						ItemAllowedToBeStored,
						localizationNameKey,
						StorageSize, 
						ShouldVacuum, 
						GetColor,
						GetScrollBarColor,
						GetButtonHoverColor,
						StorageItemTypeGetter,
						UI_DefaultLeftLocationOnScreen, 
						UI_DefaultTopLocationOnScreen
					);

				case CallID.GetItems:
					if (args.Length < 2 || args.Length > 2)
						return null;

					if (args[1] is not int modID)
						return null;

					return StorageManager.GetItems(modID);

				case CallID.GetStorageID:
					if (args.Length < 3 || args.Length > 3)
						return -1;

					if (args[1] is not string modName)
						return -1;

					if (args[2] is not string bagName)
						return -1;

					return StorageManager.GetStorageID(modName, bagName);

				case CallID.SetUIPosition:
					if (args.Length < 4)
						return false;

					if (args[1] is not int modID2)
						return false;

					if (args[2] is not int left)
						return false;

					if (args[3] is not int top)
						return false;

					StorageManager.SetUIPosition(modID2, (left, top));
					return true;

				case CallID.SetShouldVacuum:
					if (args.Length < 3)
						return false;

					if (args[1] is not int modID3)
						return false;

					if (args[2] is not bool shouldVacuum)
						return false;

					StorageManager.SetShouldVacuum(modID3, shouldVacuum);
					return true;

				default:
					return -1;
			}
		}
		List<Hook> hooks = new();
		public override void Load() {
			On_ChestUI.LootAll += OnChestUI_LootAll;
			On_ChestUI.Restock += On_ChestUI_Restock;
			On_Player.QuickStackAllChests += On_Player_QuickStackAllChests;
			On_Chest.AskForChestToEatItem += On_Chest_AskForChestToEatItem;
			//hooks.Add();
			//foreach (Hook hook in hooks) {
			//	hook.Apply();
			//}

			MagicStorageButtonsUI.RegisterWithMasterUIManager();
			AndroLocalizationData.RegisterSDataPackage();
		}
		public override void Unload() {
			BossChecklistIntegration.UnloadBossChecklistIntegration();
		}

		private void On_Chest_AskForChestToEatItem(On_Chest.orig_AskForChestToEatItem orig, Vector2 worldPosition, int duration) {
			orig(worldPosition, duration);

			VacuumBagTile.AskForBagToEatItem(worldPosition, duration);
		}

		private void On_Player_QuickStackAllChests(On_Player.orig_QuickStackAllChests orig, Player self) {
			orig(self);

			for (int i = 0; i < self.inventory.Length; i++) {
				ref Item item = ref self.inventory[i];
				if (item.favorited)
					continue;

				if (!StorageManager.TryVacuumItem(ref item, self))
					VacuumBagTile.QuickStackToBags(ref item, self);
			}
		}

		private void On_ChestUI_Restock(On_ChestUI.orig_Restock orig) {
			StoragePlayer storagePlayer = StoragePlayer.LocalStoragePlayer;
			int chest = storagePlayer.Player.chest;
			if (chest != -1) {
				Item[] chestItmes = storagePlayer.Player.GetChestItems();
				bool synchChest = chest > -1 && Main.netMode == NetmodeID.MultiplayerClient;
				for (int i = 0; i < chestItmes.Length; i++) {
					ref Item item = ref chestItmes[i];
					if (item.favorited)
						continue;

					if (StorageManager.TryQuickStack(ref item)) {
						if (synchChest)
							NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, chest, i);
					}
				}
			}

			orig();
		}

		private void OnChestUI_LootAll(On_ChestUI.orig_LootAll orig) {//TODO: Make this work with essence
			StoragePlayer storagePlayer = StoragePlayer.LocalStoragePlayer;
			int chest = storagePlayer.Player.chest;
			if (chest != -1) {
				Item[] chestItmes = storagePlayer.Player.GetChestItems();
				bool synchChest = chest > -1 && Main.netMode == NetmodeID.MultiplayerClient;
				for (int i = 0; i < chestItmes.Length; i++) {
					ref Item item = ref chestItmes[i];
					if (StorageManager.TryVacuumItem(ref item, Main.LocalPlayer)) {
						if (synchChest)
							NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, chest, i);
					}
				}
			}

			orig();
		}
	}
}