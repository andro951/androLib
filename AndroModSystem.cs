using androLib.Common.Globals;
using androLib.Common.Utility;
using androLib.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public override void AddRecipeGroups() {
			RecipeGroup group = new RecipeGroup(() => AnyCommonGem.AddSpaces(), GemSets.CommonGems.ToArray());
			RecipeGroup.RegisterGroup($"{AndroMod.ModName}:{AnyCommonGem}", group);

			group = new RecipeGroup(() => AnyRareGem.AddSpaces(), GemSets.RareGems.ToArray());
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
		}
	}
}
