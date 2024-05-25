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
using System.Reflection;
using System.Linq;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria.GameContent.ItemDropRules;

namespace androLib
{
	public class AndroMod : Mod {
		//public static AndroServerConfig serverConfig = ModContent.GetInstance<AndroServerConfig>();
		public static Mod Instance = ModContent.GetInstance<AndroMod>();
		public static AndroClientConfig clientConfig = ModContent.GetInstance<AndroClientConfig>();
		public const string ModName = "androLib";
		public const string magicStorageName = "MagicStorage";
		public static Mod magicStroageMod;
		public static bool magicStorageEnabled = ModLoader.TryGetMod(magicStorageName, out magicStroageMod);
		public const string calamityModName = "CalamityMod";
		public static Mod calamityMod;
		public static bool calamityEnabled = ModLoader.TryGetMod(calamityModName, out calamityMod);
		public const string thoriumModName = "ThoriumMod";
		public static Mod thoriumMod;
		public static bool thoriumEnabled = ModLoader.TryGetMod(thoriumModName, out thoriumMod);
		public const string starsAboveModName = "StarsAbove";
		public static Mod starsAboveMod;
		public static bool starsAboveEnabled = ModLoader.TryGetMod(starsAboveModName, out starsAboveMod);
		public const string fargosModName = "Fargowiltas";
		public static Mod fargosMod;
		public static bool fargosEnabled = ModLoader.TryGetMod(fargosModName, out fargosMod);
		public const string fargosSoulsModName = "FargowiltasSouls";
		public static Mod fargosSoulsMod;
		public static bool fargosSoulsEnabled = ModLoader.TryGetMod(fargosSoulsModName, out fargosSoulsMod);
		public const string amuletOfManyMinionsName = "AmuletOfManyMinions";
		public static bool amuletOfManyMinionsEnabled = ModLoader.TryGetMod(amuletOfManyMinionsName, out Mod _);
		public const string vacuumBagsName = "VacuumBags";
		public static Mod vacuumBagsMod;
		public static bool vacuumBagsEnabled = ModLoader.TryGetMod(vacuumBagsName, out vacuumBagsMod);
		public const string weaponEnchantmentsName = "WeaponEnchantments";
		public static Mod weaponEnchantmentsMod;
		public static bool weaponEnchantmentsLoaded = ModLoader.TryGetMod(weaponEnchantmentsName, out weaponEnchantmentsMod);
		public const string spookyModName = "Spooky";
		public static Mod spookyMod;
		public static bool spookyModEnabled = ModLoader.TryGetMod(spookyModName, out spookyMod);
		public const string secretsOfTheShadowsName = "SOTS";
		public static Mod secretsOfTheShadowsMod;
		public static bool secretsOfTheShadowsEnabled = ModLoader.TryGetMod(secretsOfTheShadowsName, out secretsOfTheShadowsMod);
		public static bool bossChecklistEnabled = ModLoader.TryGetMod("BossChecklist", out Mod _);
		public static Mod wikiThis;
		public static bool wikiThisEnabled = ModLoader.TryGetMod("Wikithis", out wikiThis);
		public static Mod GadgetGalore;
		public static bool gadgetGaloreEnabled = ModLoader.TryGetMod("GadgetGalore", out GadgetGalore);
		public const string dbzTerrariaName = "DBZMODPORT";
		public static Mod dbzTerrariaMod;
		public static bool dbzTerrariaEnabled = ModLoader.TryGetMod(dbzTerrariaName, out dbzTerrariaMod);
		public const string warframeModName = "WarframeMod";
		public static Mod warframeMod;
		public static bool warframeModEnabled = ModLoader.TryGetMod(warframeModName, out warframeMod);
		public const string ammoToolModName = "AmmoTool";
		public static Mod ammoToolMod;
		public static bool ammoToolModEnabled = ModLoader.TryGetMod(ammoToolModName, out ammoToolMod);

		public static int VanillaRecipeCount = -1;
		private enum CallID {
			None = -1,

			//Register: int id = Call("Register", Mod mod, Type VacuumStorageType, int StorageSize, Func<int> StorageItemTypeGetter, int UI_DefaultLeftLocationOnScreen, int UI_DefaultTopLocationOnScreen);
			//Example: int id = Call("Register", this, typeof(OreBag), 100, () => ModContent.ItemType<OreBag>(), 80, 675)
			//Register should be callid in your Mod.Load() method.  It needs to happen before ModPlayer.LoadData() is called.
			//The return value is the id that you can use to access your storage in the other calls.
			//StorageSize, StorageItemTypeGetter, UI_DefaultLeftLocationOnScreen, UI_DefaultTopLocationOnScreen are optional
			//Setting StorageSize to a negative number will add a config option to the androLib Client Config to allow players to change the storage size. (default is the absolute value of number you provide)
			//StorageItemTypeGetter can be null if you aren't making an item that can be opened from your inventory or you want to take care of it yourself.
			//StorageItemTypeGetter should be a () => ModContent.ItemType<OreBag>(), where OreBag is your ModItem class.
			Register = 0,



			//The rest of the calls just need the id you got from Register.
			//GetItems: Item[] items = Call("GetItems", int storageID);
			//Example: Item[] items = Call("GetItems", storageID); where storageID is the id you got from Register Or GetStorageID.
			//Default value is an empty array if it fails.
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
					if (args.Length < 3 || args.Length > 15)
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

					int updateAllowedListIndex = topIndex + 1;
					Action<int, bool> UpdateAllowedList = null;
					if (args.Length >= updateAllowedListIndex + 1 && args[updateAllowedListIndex] is Action<int, bool> UpdateAllowedListArg)
						UpdateAllowedList = UpdateAllowedListArg;

					int isBlacklistGetterIndex = updateAllowedListIndex + 1;
					bool IsBlacklistGetter = false;
					if (args.Length >= isBlacklistGetterIndex + 1 && args[isBlacklistGetterIndex] is bool IsBlacklistGetterArg)
						IsBlacklistGetter = IsBlacklistGetterArg;

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
						UI_DefaultTopLocationOnScreen,
						UpdateAllowedList,
						IsBlacklistGetter
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
			VanillaRecipeCount = Recipe.numRecipes;
			hooks.Add(new(ItemDropDatabaseRegisterToItem, RegisterToItemDetour));
			hooks.Add(new(ItemLoaderRightClickMethodInfo, ItemLoaderRightClickDetour));
			foreach (Hook hook in hooks) {
				hook.Apply();
			}

			On_ChestUI.LootAll += OnChestUI_LootAll;
			On_ChestUI.Restock += On_ChestUI_Restock;
			On_Player.QuickStackAllChests += On_Player_QuickStackAllChests;
			On_Chest.AskForChestToEatItem += On_Chest_AskForChestToEatItem;
			On_Main.GetBuffTooltip += On_Main_GetBuffTooltip;
			IL_ItemSlot.RightClick_ItemArray_int_int += IL_ItemSlot_RightClick_ItemArray_int_int;
			IL_SceneMetrics.ScanAndExportToMain += IL_SceneMetrics_ScanAndExportToMain;

			MagicStorageButtonsUI.RegisterWithMasterUIManager();
			AndroLocalizationData.RegisterSDataPackage();
		}


		private void IL_ItemSlot_RightClick_ItemArray_int_int(ILContext il) {
			//IL_0053: brfalse.s IL_0089

			//// if (Main.mouseRightRelease)
			//IL_0055: ldsfld bool Terraria.Main::mouseRightRelease
			//IL_005a: brfalse.s IL_0088

			var c = new ILCursor(il);

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchBrfalse(out _),
				i => i.MatchLdsfld(typeof(Main), nameof(Main.mouseRightRelease))
				)) {
				throw new Exception("Failed to find instructions IL_ItemSlot_RightClick_ItemArray_int_int");
			}

			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldarg, 2);
			c.EmitDelegate((bool mouseRightReleased, Item[] inv, int slot) => {
				Item item = inv[slot];
				if (item.NullOrAir() || !ItemSets.Sets.ContinuousRightClickItems.Contains(item.type))
					return mouseRightReleased;

				if (Main.stackSplit > 1)
					return false;

				int num = Main.superFastStack + 1;
				Player player = Main.LocalPlayer;
				bool anyRules = Main.ItemDropsDB.GetRulesForItemID(inv[slot].type).Any();
				for (int i = 0; i < num; i++) {
					if (anyRules)
						StaticMethodInfos.ItemSlot_TryOpenContainer(inv[slot], player);
					else
						ItemLoader.RightClick(inv[slot], player);

					ItemSlot.RefreshStackSplitCooldown();
				}

				return false;
			});
		}

		public override void Unload() {
			BossChecklistIntegration.UnloadBossChecklistIntegration();
		}
		private void On_Chest_AskForChestToEatItem(On_Chest.orig_AskForChestToEatItem orig, Vector2 worldPosition, int duration) {
			orig(worldPosition, duration);

			GlobalVacuumBagTile.AskForBagToEatItem(worldPosition, duration);
		}
		private void On_Player_QuickStackAllChests(On_Player.orig_QuickStackAllChests orig, Player self) {
			orig(self);

			if (Main.netMode != NetmodeID.SinglePlayer)
				return;

			//Dupplicates the item in multiplayer because TryQuickStack will fail.
			Item[] inv = self.inventory.TakePlayerInventory40();
			for (int i = 0; i < inv.Length; i++) {
				ref Item item = ref inv[i];
				if (item.favorited)
					continue;

				if (!StorageManager.TryQuickStack(ref item, self))
					GlobalVacuumBagTile.QuickStackToBags(ref item, self);
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

					if (StorageManager.TryRestock(ref item)) {
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
		private delegate IItemDropRule orig_RegisterToItem(ItemDropDatabase instance, int type, IItemDropRule entry);
		private delegate void hook_RegisterToItem(orig_RegisterToItem orig, ItemDropDatabase instance, int type, IItemDropRule entry);
		private static readonly MethodInfo ItemDropDatabaseRegisterToItem = typeof(ItemDropDatabase).GetMethod("RegisterToItem");
		private IItemDropRule RegisterToItemDetour(orig_RegisterToItem orig, ItemDropDatabase instance, int type, IItemDropRule entry) {
			if (!StorageManager.AllBagTypesSorted.Contains(type))
				return orig(instance, type, entry);

			return entry;
		}

		private delegate void orig_RightClick(Item item, Player player);
		private delegate void hook_RightClick(orig_RightClick orig, Item item, Player player);
		private static readonly MethodInfo ItemLoaderRightClickMethodInfo = typeof(ItemLoader).GetMethod("RightClick");
		public static Action<Item, Player> PostRightClickActions;
		private void ItemLoaderRightClickDetour(orig_RightClick orig, Item item, Player player) {
			orig(item, player);

			PostRightClickActions?.Invoke(item, player);
		}
		public static Action<SceneMetrics, SceneMetricsScanSettings> ScenemetrictBeforeAnyCheck = null;

		/// <summary>
		/// The input int and output int are the tile type at the location.
		/// </summary>
		public static List<Func<int, SceneMetrics, SceneMetricsScanSettings, int>> ScenemetricsOnNearbyEffects = new();
		public static Action<SceneMetrics, SceneMetricsScanSettings> ScenemetrictAfterTileCheck = null;
		private void IL_SceneMetrics_ScanAndExportToMain(ILContext il) {
			var c = new ILCursor(il);

			//IL_0035: ldarga.s settings
			//IL_0037: ldflda valuetype[System.Runtime]System.Nullable`1 < valuetype[FNA]Microsoft.Xna.Framework.Vector2 > Terraria.SceneMetricsScanSettings::BiomeScanCenterPositionInWorld
			//IL_003c: call instance !0 valuetype[System.Runtime]System.Nullable`1 < valuetype[FNA]Microsoft.Xna.Framework.Vector2 >::get_Value()
			//IL_0041: call valuetype[FNA]Microsoft.Xna.Framework.Point Terraria.Utils::ToTileCoordinates(valuetype[FNA]Microsoft.Xna.Framework.Vector2)
			//IL_0046: stloc.3

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchLdarga(1),
				i => i.MatchLdflda<SceneMetricsScanSettings>("BiomeScanCenterPositionInWorld"),
				i => i.MatchCall(out _),
				i => i.MatchCall(out _),
				i => i.MatchStloc(3)
			)) { throw new Exception("Failed to find instructions IL_SceneMetrics_ScanAndExportToMain 1/4"); }


			c.Emit(OpCodes.Ldarga, 0);
			c.Emit(OpCodes.Ldarg, 1);
			c.EmitDelegate((ref SceneMetrics sceneMetrics, SceneMetricsScanSettings settings) => {
				ScenemetrictBeforeAnyCheck?.Invoke(sceneMetrics, settings);
				//BannerBag.PreScanAndExportToMain();
				//PortableStation.PreScanAndExportToMain();
			});

			//IL_0322: ldloc.s 5
			//IL_0324: ldloc.s 6
			//IL_0326: ldloca.s 7
			//IL_0328: call instance uint16 & Terraria.Tile::get_type()
			//IL_032d: ldind.u2
			//IL_032e: ldc.i4.0
			//IL_032f: call void Terraria.ModLoader.TileLoader::NearbyEffects(int32, int32, int32, bool)

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchLdloc(5),
				i => i.MatchLdloc(6),
				i => i.MatchLdloca(7),
				i => i.MatchCall(out _),
				i => i.MatchLdindU2(),
				i => i.MatchLdcI4(0)
			)) { throw new Exception("Failed to find instructions IL_SceneMetrics_ScanAndExportToMain 2/4"); }

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchLdcI4(0)
			)) { throw new Exception("Failed to find instructions IL_SceneMetrics_ScanAndExportToMain 3/4"); }

			//c.EmitDelegate((int type) => { return type; });

			c.Emit(OpCodes.Ldarga, 0);
			c.Emit(OpCodes.Ldarg, 1);

			c.EmitDelegate((int num, ref SceneMetrics sceneMetrics, SceneMetricsScanSettings settings) => {
				foreach (Func<int, SceneMetrics, SceneMetricsScanSettings, int> func in ScenemetricsOnNearbyEffects) {
					num = func(num, sceneMetrics, settings);
				}

				return num;
			});

			//c.EmitDelegate(GlobalBagTile.NearbyEffects);

			//IL_0677: ldarg.0
			//IL_0678: call instance void Terraria.SceneMetrics::ExportTileCountsToMain()

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchLdarg(0),
				i => i.MatchCall<SceneMetrics>("ExportTileCountsToMain")
			)) { throw new Exception("Failed to find instructions IL_SceneMetrics_ScanAndExportToMain 4/4"); }

			c.Emit(OpCodes.Ldarga, 0);
			c.Emit(OpCodes.Ldarg, 1);

			c.EmitDelegate((ref SceneMetrics sceneMetrics, SceneMetricsScanSettings settings) => {
				ScenemetrictAfterTileCheck?.Invoke(sceneMetrics, settings);
				//BannerBag.PostScanAndExportToMain(ref sceneMetrics);
				//PortableStation.PostScanAndExportToMain(ref sceneMetrics);
			});
		}

		private static SortedDictionary<int, Func<Player, string>> BuffDescriptions = new();
		public static void RegisterBuffDescription(int buffID, Func<Player, string> description) => BuffDescriptions.TryAdd(buffID, description);
		private string On_Main_GetBuffTooltip(On_Main.orig_GetBuffTooltip orig, Player player, int buffType) {
			if (BuffDescriptions.TryGetValue(buffType, out Func<Player, string> descriptionFunc))
				return descriptionFunc(player);

			return orig(player, buffType);
		}
	}
}