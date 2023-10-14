using androLib.Common.Globals;
using androLib.Common.Utility;
using androLib.Common.Utility.LogSystem;
using androLib.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace androLib
{
	public class AndroModSystem : ModSystem {
		public static bool FavoriteKeyDown => Main.keyState.IsKeyDown(Main.FavoriteKey);
		public static bool StartedPostAddRecipes { get; private set; } = false;
		public override void PostAddRecipes() {
			StartedPostAddRecipes = true;
		}
		public override void PostSetupRecipes() {
			StorageManager.PostSetupResipes();
			StoragePlayer.PostSetupRecipes();
		}
		public override void PostDrawInterface(SpriteBatch spriteBatch) {
			MasterUIManager.PostDrawInterface(spriteBatch);
		}
		private static bool printModItemName => false;
		public override void PostUpdateEverything() {
			if (Debugger.IsAttached && !Main.LocalPlayer.HeldItem.NullOrAir()) {
				Item item = Main.LocalPlayer.HeldItem;
				string temp = item.ModFullName();
				int createTile = item.createTile;
				int useStyle = item.useStyle;
				bool useTurn = item.useTurn;
				bool autoReuse = item.autoReuse;
				bool consumable = item.consumable;
				if (item.DamageType != DamageClass.Default) {
					string temp2 = item.DamageType.Name;
				}

				if (item.ModItem != null) {
					ModItem modItem = item.ModItem;
					string modItemName = modItem.Name;
					if (modItem is UnloadedItem unloadedItem) {
						string unloadedItemName = unloadedItem.Name;
						string unloadedItemFullName = unloadedItem.FullName;
						string unloadedItemItemName = unloadedItem.ItemName;
						string unloadedItemModName = unloadedItem.ModName;
					}
				}

				if (printModItemName && !Main.mouseItem.NullOrAir() && Main.mouseItem.ModItem != null)
					Main.NewText(Main.mouseItem.ModItem.Name);
			}

			if (Debugger.IsAttached && !Main.HoverItem.NullOrAir()) {
				string hoverItemName = Main.HoverItem.ModFullName();
				if (printModItemName)
					Main.NewText(hoverItemName);
			}

			SoundManager.Update();
		}
		public struct ChanceMultiplierInfo {
			public int Min {
				get {
					if (min == int.MaxValue)
						SetupMinMax();

					return min;
				}
			}
			private int min = int.MaxValue;
			public int Max {
				get {
					if (max == int.MinValue)
						SetupMinMax();

					return max;
				}
			}
			private int max = int.MinValue;
			private void SetupMinMax() {
				IEnumerable<ModItem> modItems = mod.GetContent<ModItem>();
				Wiki.GetMinMax(modItems, out min, out max);
			}
			private Mod mod;
			public Func<float> NPCMultiplier;
			public Func<float> BossMultiplier;
			public Func<float> ChestMultiplier;
			public Func<float> CrateMultiplier;
			public ChanceMultiplierInfo(Mod mod, Func<float> npcMultiplier = null, Func<float> bossMultiplier = null, Func<float> chestMultiplier = null, Func<float> crateMultiplier = null) {
				this.mod = mod;
				NPCMultiplier = npcMultiplier;
				BossMultiplier = bossMultiplier;
				ChestMultiplier = chestMultiplier;
				CrateMultiplier = crateMultiplier;
			}
		}
		public static void RegisterChestSpawnChanceMultiplier(Mod mod, Func<float> npcMultiplier = null, Func<float> bossMultiplier = null, Func<float> chestMultiplier = null, Func<float> crateMultiplier = null) {
			ChestSpawnChanceMultipliers.Add(new ChanceMultiplierInfo(mod, npcMultiplier, bossMultiplier, chestMultiplier, crateMultiplier));
		}
		private static List<ChanceMultiplierInfo> ChestSpawnChanceMultipliers = new();
		public static bool TryGetNPCSpawnChanceMultiplier(int itemType, out float mult) {
			foreach (ChanceMultiplierInfo info in ChestSpawnChanceMultipliers) {
				if (info.NPCMultiplier != null && info.Min <= itemType && info.Max >= itemType) {
					mult = info.NPCMultiplier();
					return true;
				}
			}

			mult = 1f;
			return false;
		}
		public static bool TryGetBossSpawnChanceMultiplier(int itemType, out float mult) {
			foreach (ChanceMultiplierInfo info in ChestSpawnChanceMultipliers) {
				if (info.BossMultiplier != null && info.Min <= itemType && info.Max >= itemType) {
					mult = info.BossMultiplier();
					return true;
				}
			}

			mult = 1f;
			return false;
		}
		public static bool TryGetChestSpawnChanceMultiplier(int itemType, out float mult) {
			foreach (ChanceMultiplierInfo info in ChestSpawnChanceMultipliers) {
				if (info.ChestMultiplier != null && info.Min <= itemType && info.Max >= itemType) {
					mult = info.ChestMultiplier();
					return true;
				}
			}

			mult = 1f;
			return false;
		}
		public static bool TryGetCrateSpawnChanceMultiplier(int itemType, out float mult) {
			foreach (ChanceMultiplierInfo info in ChestSpawnChanceMultipliers) {
				if (info.CrateMultiplier != null && info.Min <= itemType && info.Max >= itemType) {
					mult = info.CrateMultiplier();
					return true;
				}
			}

			mult = 1f;
			return false;
		}

		public const string AnyCommonGem = "AnyCommonGem";
		public const string AnyRareGem = "AnyRareGem";
		public const string AnyAlignedSoul = "AnyAlignedSoul";
		public const string Workbenches = "Workbenches";
		public const string StarsAboveAnyKingSlimeEssence = "AnyKingSlimeEssence";
		public const string StarsAboveAnyEyeOfCthulhuEssence = "AnyEyeOfCthulhuEssence";
		public const string StarsAboveAnyEaterOfWorldsOrBrainOfCthulhuEssence = "AnyEaterOfWorldsOrBrainOfCthulhuEssence";
		public const string StarsAboveAnyQueenBeeEssence = "AnyQueenBeeEssence";
		public const string StarsAboveAnySkeletronEssence = "AnySkeletronEssence";
		public const string CursedFlameOrIchor = "CursedFlameOrIchor";
		public const string GoldOrPlatinumBar = "GoldOrPlatinumBar";
		public override void AddRecipeGroups() {
			int[] commanGems = GemSets.CommonGems.ToArray();
			int indexOfTopax = Array.IndexOf(commanGems, ItemID.Topaz);
			if (indexOfTopax > 0) {
				commanGems[indexOfTopax] = commanGems[0];
				commanGems[0] = ItemID.Topaz;
			}
			
			RecipeGroup group = new RecipeGroup(() => AnyCommonGem.AddSpaces(), commanGems);
			RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{AnyCommonGem}", group);

			int[] rareGems = GemSets.RareGems.ToArray();
			int indexOfAmber = Array.IndexOf(rareGems, ItemID.Amber);
			if (indexOfAmber > 0) {
				rareGems[indexOfAmber] = rareGems[0];
				rareGems[0] = ItemID.Amber;
			}

			group = new RecipeGroup(() => AnyRareGem.AddSpaces(), rareGems);
			RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{AnyRareGem}", group);

			group = new RecipeGroup(() => Workbenches.AddSpaces(), new int[] {
				ItemID.WorkBench,
				ItemID.BambooWorkbench,
				ItemID.BlueDungeonWorkBench,
				ItemID.BoneWorkBench,
				ItemID.BorealWoodWorkBench,
				ItemID.CactusWorkBench,
				ItemID.CrystalWorkbench,
				ItemID.DynastyWorkBench,
				ItemID.EbonwoodWorkBench,
				ItemID.FleshWorkBench,
				ItemID.FrozenWorkBench,
				ItemID.GlassWorkBench,
				ItemID.GoldenWorkbench,
				ItemID.GothicWorkBench,
				ItemID.GraniteWorkBench,
				ItemID.GreenDungeonWorkBench,
				ItemID.HoneyWorkBench,
				ItemID.LesionWorkbench,
				ItemID.LihzahrdWorkBench,
				ItemID.LivingWoodWorkBench,
				ItemID.MarbleWorkBench,
				ItemID.MartianWorkBench,
				ItemID.MeteoriteWorkBench,
				ItemID.MushroomWorkBench,
				ItemID.NebulaWorkbench,
				ItemID.ObsidianWorkBench,
				ItemID.PalmWoodWorkBench,
				ItemID.PearlwoodWorkBench,
				ItemID.PinkDungeonWorkBench,
				ItemID.PumpkinWorkBench,
				ItemID.RichMahoganyWorkBench,
				ItemID.SandstoneWorkbench,
				ItemID.ShadewoodWorkBench,
				ItemID.SkywareWorkbench,
				ItemID.SlimeWorkBench,
				ItemID.SolarWorkbench,
				ItemID.SpiderWorkbench,
				ItemID.SpookyWorkBench,
				ItemID.StardustWorkbench,
				ItemID.SteampunkWorkBench,
				ItemID.VortexWorkbench
			});
			RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{Workbenches}", group);

			group = new RecipeGroup(() => AnyAlignedSoul.AddSpaces(), new int[] {
				ItemID.SoulofLight,
				ItemID.SoulofNight
			});
			RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{AnyAlignedSoul}", group);

			if (AndroMod.starsAboveEnabled) {
				//King Slime
				string[] kingSlimeEssences = {
					"EssenceOfTheAegis",
					"EssenceOfStyle"
				};

				List<int> kingSlimeEssenceTypes = new();
				foreach (string essenceName in kingSlimeEssences) {
					if (AndroMod.starsAboveMod.TryFind(essenceName, out ModItem essenceModItem))
						kingSlimeEssenceTypes.Add(essenceModItem.Type);
				}

				if (kingSlimeEssenceTypes.Count > 0) {
					group = new RecipeGroup(() => StarsAboveAnyKingSlimeEssence.AddSpaces(), kingSlimeEssenceTypes.ToArray());
					RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{StarsAboveAnyKingSlimeEssence}", group);
				}

				//Eye of Cthulhu
				string[] eyeOfCthulhuEssences = {
					"EssenceOfTheDarkMoon",
					"EssenceOfTheGardener"
				};

				List<int> eyeOfCthulhuEssenceTypes = new();
				foreach (string essenceName in eyeOfCthulhuEssences) {
					if (AndroMod.starsAboveMod.TryFind(essenceName, out ModItem essenceModItem))
						eyeOfCthulhuEssenceTypes.Add(essenceModItem.Type);
				}

				if (eyeOfCthulhuEssenceTypes.Count > 0) {
					group = new RecipeGroup(() => StarsAboveAnyEyeOfCthulhuEssence.AddSpaces(), eyeOfCthulhuEssenceTypes.ToArray());
					RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{StarsAboveAnyEyeOfCthulhuEssence}", group);
				}

				//Queen Bee
				string[] queenBeeEssences = {
					"EssenceOfFingers",
					"EssenceOfBitterfrost"
				};

				List<int> queenBeeEssenceTypes = new();
				foreach (string essenceName in queenBeeEssences) {
					if (AndroMod.starsAboveMod.TryFind(essenceName, out ModItem essenceModItem))
						queenBeeEssenceTypes.Add(essenceModItem.Type);
				}

				if (queenBeeEssenceTypes.Count > 0) {
					group = new RecipeGroup(() => StarsAboveAnyQueenBeeEssence.AddSpaces(), queenBeeEssenceTypes.ToArray());
					RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{StarsAboveAnyQueenBeeEssence}", group);
				}

				//Eater of Worlds or Brain of Cthulhu
				string[] eaterOfWorldsOrBrainOfCthulhuEssences = {
					"EssenceOfAsh",
					"EssenceOfTheAnomaly",
					"EssenceOfTheSoldier",
					"EssenceOfOuterGods"
				};

				List<int> eaterOfWorldsOrBrainOfCthulhuEssenceTypes = new();
				foreach (string essenceName in eaterOfWorldsOrBrainOfCthulhuEssences) {
					if (AndroMod.starsAboveMod.TryFind(essenceName, out ModItem essenceModItem))
						eaterOfWorldsOrBrainOfCthulhuEssenceTypes.Add(essenceModItem.Type);
				}

				if (eaterOfWorldsOrBrainOfCthulhuEssenceTypes.Count > 0) {
					group = new RecipeGroup(() => StarsAboveAnyEaterOfWorldsOrBrainOfCthulhuEssence.AddSpaces(), eaterOfWorldsOrBrainOfCthulhuEssenceTypes.ToArray());
					RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{StarsAboveAnyEaterOfWorldsOrBrainOfCthulhuEssence}", group);
				}

				//Skeletron
				string[] skeletronEssences = {
					"EssenceOfTheFreeshooter",
					"EssenceOfThePegasus",
					"EssenceOfTheOcean",
					"EssenceOfTheSharpshooter",
					"EssenceOfTheAutomaton",
					"EssenceOfMisery",
					"EssenceOfNanomachines",
					"EssenceOfTheHallownest"
				};

				List<int> skeletronEssenceTypes = new();
				foreach (string essenceName in skeletronEssences) {
					if (AndroMod.starsAboveMod.TryFind(essenceName, out ModItem essenceModItem))
						skeletronEssenceTypes.Add(essenceModItem.Type);
				}

				if (skeletronEssenceTypes.Count > 0) {
					group = new RecipeGroup(() => StarsAboveAnySkeletronEssence.AddSpaces(), skeletronEssenceTypes.ToArray());
					RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{StarsAboveAnySkeletronEssence}", group);
				}
			}

			group = new RecipeGroup(() => CursedFlameOrIchor.AddSpaces(), new int[] { ItemID.CursedFlame, ItemID.Ichor });
			RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{CursedFlameOrIchor}", group);

			group = new RecipeGroup(() => GoldOrPlatinumBar.AddSpaces(), new int[] { ItemID.GoldBar, ItemID.PlatinumBar });
			RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{GoldOrPlatinumBar}", group);
		}
		public static SortedDictionary<ChestID, List<DropData>> chestDrops = new();
		public override void PostWorldGen() {
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if (chest == null)
					continue;

				int itemsPlaced = 0;

				ChestID chestID = GetChestIDFromChest(chest);
				GetChestLoot(chestID, out List<DropData> options, out float chance);

				if (chance <= 0f)
					continue;

				if (options == null)
					continue;

				IEnumerable<DropData> weightedDropData = options.Where(d => d.Chance <= 0f);
				IEnumerable<DropData> chanceDropData = options.Where(d => d.Chance > 0f);
				foreach (DropData dropData in chanceDropData) {
					float randFloat = Main.rand.NextFloat();
					float dropChance = dropData.Chance;
					if (TryGetChestSpawnChanceMultiplier(dropData.ID, out float mult))
						dropChance *= mult;

					if (randFloat > dropChance)
						continue;

					for (int j = 0; j < 40; j++) {
						if (chest.item[j].type != ItemID.None)
							continue;

						int type = dropData.ID;
						for (int k = j; k >= 0; k--) {
							if (chest.item[k].type == type && chest.item[k].stack < chest.item[k].maxStack) {
								chest.item[k].stack++;
								break;
							}
						}

						chest.item[j] = new Item(type);
						break;
					}
				}

				for (int j = 0; j < 40 && itemsPlaced < chance; j++) {
					if (chest.item[j].type != ItemID.None)
						continue;

					int type = weightedDropData.GetOneFromWeightedList(chance);

					if (type > 0) {
						bool found = false;
						for (int k = j; k >= 0; k--) {
							if (chest.item[k].type == type && chest.item[k].stack < chest.item[k].maxStack) {
								chest.item[k].stack++;
								found = true;
								j--;
								break;
							}
						}

						if (!found)
							chest.item[j] = new Item(type);
					}

					itemsPlaced++;
				}
			}
		}
		public static ChestID GetChestIDFromChest(Chest chest) {
			Tile tile = Main.tile[chest.x, chest.y];
			ushort tileType = tile.TileType;
			short tileFrameX = tile.TileFrameX;
			// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest.
			// Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding.
			switch (tileType) {
				case TileID.Containers:
				case TileID.FakeContainers:
					return (ChestID)(tileFrameX / 36);
				case TileID.Containers2:
				case TileID.FakeContainers2:
					return (ChestID)(tileFrameX / 36 + 100);
				default:
					return ChestID.None;
			}
		}
		public static void GetChestLoot(ChestID chestID, out List<DropData> itemTypes, out float chance) {
			chance = 0f;
			itemTypes = chestDrops.ContainsKey(chestID) ? chestDrops[chestID] : null;
			if (itemTypes == null)
				return;

			foreach (DropData dropData in itemTypes) {
				int itemType = dropData.ID;
				if (TryGetChestSpawnChanceMultiplier(itemType, out float mult)) {
					chance = mult;
					break;
				}
			}

			if (itemTypes.Where(d => d.Chance <= 0f).Count() == 1)
				chance *= itemTypes[0].Weight;

			switch (chestID) {
				case ChestID.Chest_Normal:
					chance *= 0.7f;
					break;
				case ChestID.Gold:
					break;
				case ChestID.Gold_Locked:
					break;
				case ChestID.Shadow:
				case ChestID.Shadow_Locked:
					chance *= 2f;
					break;
				case ChestID.RichMahogany:
					break;
				case ChestID.Ivy:
					break;
				case ChestID.Frozen:
					break;
				case ChestID.LivingWood:
					break;
				case ChestID.Skyware:
					break;
				case ChestID.WebCovered:
					break;
				case ChestID.Lihzahrd:
					chance *= 2f;
					break;
				case ChestID.Water:
					break;
				case ChestID.Jungle_Dungeon:
					chance = 1f;
					break;
				case ChestID.Corruption_Dungeon:
					chance = 1f;
					break;
				case ChestID.Crimson_Dungeon:
					chance = 1f;
					break;
				case ChestID.Hallowed_Dungeon:
					chance = 1f;
					break;
				case ChestID.Ice_Dungeon:
					chance = 1f;
					break;
				case ChestID.Mushroom:
					break;
				case ChestID.Granite:
					break;
				case ChestID.Marble:
					break;
				case ChestID.Gold_DeadMans:
					break;
				case ChestID.SandStone:
					break;
				case ChestID.Desert_Dungeon:
					chance = 1f;
					break;
			}
		}
	}
}
