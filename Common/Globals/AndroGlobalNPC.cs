using androLib.Common.Utility;
using androLib.ModIntegration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static androLib.ModIntegration.BossChecklistIntegration;

namespace androLib.Common.Globals
{
	public class AndroGlobalNPC : GlobalNPC {
		public static readonly AndroGlobalNPC StaticInstance = new AndroGlobalNPC();

		#region Static

		private static SortedSet<int> preHardModeBossTypes = null;
		public static SortedSet<int> PreHardModeBossTypes {
			get {
				TryBossChecklistSetup();
				if (preHardModeBossTypes == null)
					DefaultBossSetup();

				return preHardModeBossTypes;
			}
		}
		private static SortedSet<int> postPlanteraBossTypes = null;
		public static SortedSet<int> PostPlanteraBossTypes {
			get {
				TryBossChecklistSetup();
				if (postPlanteraBossTypes == null)
					DefaultBossSetup();

				return postPlanteraBossTypes;
			}
		}
		private static void DefaultBossSetup() {
			if (preHardModeBossTypes == null || postPlanteraBossTypes == null) {
				preHardModeBossTypes = new() {
					NPCID.EyeofCthulhu,
					NPCID.EaterofWorldsBody,
					NPCID.EaterofWorldsHead,
					NPCID.EaterofWorldsTail,
					NPCID.BrainofCthulhu,
					NPCID.KingSlime,
					NPCID.Deerclops,
					NPCID.QueenBee,
					NPCID.SkeletronHead
				};

				postPlanteraBossTypes = new() {
					NPCID.HallowBoss,
					NPCID.CultistBoss,
					NPCID.MoonLordCore,
					NPCID.MoonLordHead,
					NPCID.Plantera,
					NPCID.Golem,
					NPCID.DukeFishron,
					NPCID.MartianSaucer
				};
			}
		}
		private static void TryBossChecklistSetup() {
			if (!AndroMod.bossChecklistEnabled) {
				if (preHardModeBossTypes == null || postPlanteraBossTypes == null)
					AndroLibGameMessages.BossChecklistNotEnabled.ToString().Lang(AndroMod.ModName, L_ID1.AndroLibGameMessages).LogSimple();

				return;
			}

			if (!ShouldSetupBossPowerBoosterDrops)
				return;

			preHardModeBossTypes = new();
			postPlanteraBossTypes = new();
			if (!BossInfoNetIDKeys.TryGetValue(NPCID.WallofFlesh, out string wallKey) || !BossInfos.TryGetValue(wallKey, out BossChecklistBossInfo wallInfo) || !BossInfoNetIDKeys.TryGetValue(NPCID.Plantera, out string planteraKey) || !BossInfos.TryGetValue(planteraKey, out BossChecklistBossInfo planteraInfo)) {
				AndroLibGameMessages.FailedDetermineProgression.ToString().Lang(AndroMod.ModName, L_ID1.GameMessages).LogSimple();
				return;
			}

			float wallOfFleshProgression = wallInfo.progression;
			float planteraProgression = planteraInfo.progression;
			foreach (KeyValuePair<string, BossChecklistBossInfo> bossInfoPair in BossInfos.Where(p => p.Value.isBoss || p.Value.isMiniboss)) {
				float progression = bossInfoPair.Value.progression;
				List<int> netIDs = bossInfoPair.Value.npcIDs;
				if (netIDs.Count < 1)
					continue;

				NPC npc = netIDs.First().CSNPC();
				if (progression < wallOfFleshProgression) {
					preHardModeBossTypes.UnionWith(netIDs);
				}
				else if (progression >= planteraProgression) {
					postPlanteraBossTypes.UnionWith(netIDs);
				}
			}

			if (Debugger.IsAttached) $"{BossInfos.OrderBy(i => i.Value.progression).Select(i => $"Key: {i.Key}, internalName: {i.Value.internalName}, progression: {i.Value.progression}, netIDs: {i.Value.npcIDs.StringList(netID => netID.GetNPCIDOrName())}").S("BossChecklist BossInfos")}".LogSimple();

			UsedBossChecklistForBossPowerBoosterDrops = true;
		}
		public static SortedDictionary<int, float> multipleSegmentBossTypes;
		public static SortedSet<int> bossPartsNotMarkedAsBoss => new () {
			NPCID.SkeletronHand,
			NPCID.PrimeCannon,
			NPCID.PrimeLaser,
			NPCID.PrimeSaw,
			NPCID.PrimeVice,
			NPCID.PlanterasHook,
			NPCID.PlanterasTentacle,
			NPCID.MartianSaucerCannon,
			NPCID.MartianSaucerTurret,
			NPCID.MartianSaucer,
			NPCID.MoonLordFreeEye,
			NPCID.MoonLordHand,
			NPCID.MoonLordHead,
			NPCID.PirateShip,
			NPCID.PirateShipCannon,

		};
		public static SortedSet<int> fakeNPCs = new() {
			NPCID.ForceBubble,
			NPCID.CultistTablet,
			NPCID.MothronEgg,
			NPCID.TargetDummy,
			NPCID.LunarTowerNebula,
			NPCID.LunarTowerSolar,
			NPCID.LunarTowerStardust,
			NPCID.LunarTowerVortex,
			NPCID.DD2EterniaCrystal,
			NPCID.DD2LanePortal,
		};
		public static SortedSet<int> npcsThatAreActuallyProjectiles = new() {
			NPCID.BurningSphere,
			NPCID.ChaosBall,
			NPCID.WaterSphere,
			NPCID.SpikeBall,
			NPCID.BlazingWheel,
			NPCID.VileSpit,
			NPCID.Bee,
			NPCID.BeeSmall,
			NPCID.FungiSpore,
			NPCID.Spore,
			NPCID.DetonatingBubble,
			NPCID.MoonLordLeechBlob,
			NPCID.SolarFlare,
			NPCID.SolarGoop,
			NPCID.AncientLight,
			NPCID.AncientDoom,
			NPCID.LarvaeAntlion,
			NPCID.WindyBalloon,
			NPCID.ChaosBall,
			NPCID.VileSpitEaterOfWorlds,
		};
		public static bool IsIregularNPC(NPC npc) => bossPartsNotMarkedAsBoss.Contains(npc.netID) || fakeNPCs.Contains(npc.netID) || npcsThatAreActuallyProjectiles.Contains(npc.netID);
		public static SortedSet<int> normalNpcsThatDropsBags;
		public static SortedSet<int> normalNpcsThatAreBosses;
		public static SortedDictionary<int, List<DropData>> npcDropTypes = new();
		public static SortedDictionary<string, List<DropData>> modNpcDropNames = new();
		public static SortedDictionary<int, List<DropData>> npcAIDrops = new();

		private static SortedDictionary<int, List<(int, float)>> allItemDropsFromNpcs = null;
		public static SortedDictionary<int, List<(int, float)>> AllItemDropsFromNpcs {
			get {
				if (allItemDropsFromNpcs == null)
					GetAllNpcDrops();

				return allItemDropsFromNpcs;
			}
		}
		public static bool UsedBossChecklist = false;

		#endregion

		public override void Load() {
			multipleSegmentBossTypes = new SortedDictionary<int, float>() {
				{ NPCID.EaterofWorldsHead, 100f },
				{ NPCID.EaterofWorldsBody, 100f },
				{ NPCID.EaterofWorldsTail, 100f },
				{ NPCID.TheDestroyer, 1f },
				{ NPCID.TheDestroyerBody, 1f },
				{ NPCID.TheDestroyerTail, 1f },
			};

			normalNpcsThatAreBosses = new() {
				NPCID.DD2Betsy
			};

			if (!AndroMod.thoriumEnabled)
				normalNpcsThatDropsBags = new(normalNpcsThatAreBosses);

			normalNpcsThatAreBosses.Add(NPCID.DD2DarkMageT1);
			normalNpcsThatAreBosses.Add(NPCID.DD2DarkMageT3);
			normalNpcsThatAreBosses.Add(NPCID.DD2OgreT2);
			normalNpcsThatAreBosses.Add(NPCID.DD2OgreT3);
			normalNpcsThatAreBosses.Add(NPCID.PirateShip);

			if (AndroMod.thoriumEnabled) {
				normalNpcsThatDropsBags = new(normalNpcsThatAreBosses);
			}

		}
		public static float GetMultiSegmentBossMultiplier(int npcType) {
			if (multipleSegmentBossTypes.ContainsKey(npcType))
				return multipleSegmentBossTypes[npcType];

			return 1f;
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			TryGetLoot(npcLoot, npc);
		}
		protected virtual bool CanDropLoot(NPC npc) => !npc.friendly && !npc.townNPC && !npc.SpawnedFromStatue;
		public void TryGetLoot(ILoot loot, NPC npc, bool bossBag = false) {
			if (!CanDropLoot(npc))
				return;

			bool normalNpcThatDropsBag = normalNpcsThatDropsBags.Contains(npc.netID);
			bool multipleSegmentBoss = multipleSegmentBossTypes.ContainsKey(npc.netID);
			float multipleSegmentBossMultiplier = GetMultiSegmentBossMultiplier(npc.netID);
			bool boss = npc.boss || multipleSegmentBoss || normalNpcThatDropsBag;
			GetDefaultDropStats(npc, normalNpcThatDropsBag, out float hp, out float value, out float total);
			if (multipleSegmentBoss && bossBag)
				total *= multipleSegmentBossMultiplier;

			GetLoot(loot, npc, hp, value, total, boss, bossBag);
		}
		protected static Action<ILoot, NPC, float, float, float, bool, bool> GetLootActions;
		public virtual void GetLoot(ILoot loot, NPC npc, float hp, float value, float total, bool boss, bool bossBag = false) {
			GetLootActions?.Invoke(loot, npc, hp, value, total, boss, bossBag);

			if (total <= 0f)
				return;

			if (boss) {
				//Enchantments
				if (npcDropTypes.ContainsKey(npc.netID)) {
					//Enchantment drop chance
					AddBossAttributeItemLoot(npc, loot, bossBag);
				}

				if (npc.netID > NPCID.Count && npc.ModFullName() is string modFullName && modNpcDropNames.ContainsKey(modFullName)) {
					//Enchantment drop chance for modded npcs
					AddBossAttributeItemLoot(npc, loot, bossBag, modFullName);
				}
			}
			else {
				//Non-boss drops

				//defaultDenom is the denominator of the drop rate.  The numerator is always 1 (This is part of how Terraria calculates drop rates, I can't change it)
				//  hp is the npc's max hp
				//  total is calculated based on npc max hp.  Use total = hp + 0.2 * value
				//Example: defaultDenom = 5, numerator is 1.  Drop rate = 1/5 = 20%
				//Aproximate drop rate = (hp + 0.2 * value)/(5000 + hp * 5) * config multiplier
				bool useDefaultChance = true;
				switch (npc.aiStyle) {
					case NPCAIStyleID.Mimic:
					case NPCAIStyleID.BiomeMimic:
						useDefaultChance = false;
						break;
				}

				float chance;
				if (useDefaultChance) {
					chance = (total) / (5000f + hp * 0.5f);
				}
				else {
					chance = 1f;
				}

				if (npcDropTypes.ContainsKey(npc.netID)) {
					List<IItemDropRule> dropRules = GetDropRules(chance, npcDropTypes[npc.netID], AndroModSystem.TryGetNPCSpawnChanceMultiplier);
					foreach (IItemDropRule rule in dropRules) {
						loot.Add(rule);
					}

					if (AndroLogModSystem.printItemDrops)
						AndroLogModSystem.npcItemDrops.AddOrCombine(npc.netID, npcDropTypes[npc.netID]);
				}

				if (npcAIDrops.ContainsKey(npc.aiStyle)) {
					List<IItemDropRule> dropRules = GetDropRules(chance, npcAIDrops[npc.aiStyle], AndroModSystem.TryGetNPCSpawnChanceMultiplier);
					foreach (IItemDropRule rule in dropRules) {
						loot.Add(rule);
					}

					if (AndroLogModSystem.printItemDrops)
						AndroLogModSystem.npcItemDrops.AddOrCombine(npc.netID, npcAIDrops[npc.aiStyle]);
				}
			}
		}
		public static void AddBossAttributeItemLoot(NPC npc, ILoot loot, bool bossBag = false, string modFullName = null) {
			List<DropData> dropData = modFullName != null ? modNpcDropNames[modFullName] : npcDropTypes[npc.netID];
			List<IItemDropRule> dropRules = GetDropRules(1f, dropData, AndroModSystem.TryGetBossSpawnChanceMultiplier);
			foreach (IItemDropRule rule in dropRules) {
				AddBossLoot(loot, npc, rule, bossBag);
			}

			if (AndroLogModSystem.printItemDrops && (bossBag || AllItemDropsFromNpcs != null && !AllItemDropsFromNpcs.Values.SelectMany(l => l).Select(p => p.Item1).Contains(npc.netID)))
				AndroLogModSystem.npcItemDrops.AddOrCombine(npc.netID, dropData);
		}
		public virtual bool UseDefaultDropChance(NPC npc) => true;
		public virtual float DropRateMultiplier => 1f;
		protected virtual void GetDefaultDropStats(NPC npc, bool normalNPCThatDropsBossBag, out float hp, out float value, out float total) {
			//Defense
			float defenseMultiplier = 1f + (float)npc.defDefense / 40f;

			//HP
			int lifeMax = npc.RealLifeMax();
			hp = (float)lifeMax * defenseMultiplier;

			//Value
			value = npc.RealValue();
			if (value <= 0 && hp <= 10) {
				total = 0;
				return;
			}

			//Total
			if (value > 0) {
				total = hp + 0.2f * value;
			}
			else {
				total = hp * 2.6f;
				//Thorium bags for Dark Mage and Ogre only drop at a 25% rate.
				if (AndroMod.thoriumEnabled && (npc.type == NPCID.DD2OgreT2 || npc.type == NPCID.DD2OgreT3 || npc.type == NPCID.DD2DarkMageT1 || npc.type == NPCID.DD2DarkMageT3))
					total *= 4f;
			}

			//Hp reduction factor
			float hpReductionFactor = NPCStaticMethods.GetReductionFactor((int)hp);
			total /= hpReductionFactor;

			//NPC Characteristics Factors
			float noGravityFactor = npc.noGravity ? 0.2f : 0f;
			float noTileCollideFactor = npc.noTileCollide ? 0.2f : 0f;
			float knockBackResistFactor = 0.2f * npc.knockBackResist;
			float npcCharacteristicsFactor = 1f + noGravityFactor + noTileCollideFactor + knockBackResistFactor;
			total *= npcCharacteristicsFactor;

			//Balance Multiplier (Extra multiplier for us to control the values manually)
			float balanceMultiplier = 0.2f;
			total *= balanceMultiplier;

			//Modify total for specific enemies.
			switch (npc.netID) {
				case NPCID.DungeonGuardian:
					total /= 50f;
					break;
				case NPCID.EaterofWorldsHead:
				case NPCID.EaterofWorldsBody:
				case NPCID.EaterofWorldsTail:
					total /= 8f;
					break;
			}
		}
		public delegate bool ChanceMultiplier(int itemType, out float mult);
		public static bool DefaultChanceMultiplier(int itemType, out float mult) {
			mult = 1f;
			return true;
		}
		public static List<IItemDropRule> GetDropRules(float chance, IEnumerable<DropData> dropData, ChanceMultiplier GetChanceMultiplier = null) {
			List<IItemDropRule> itemDropRules = new();
			if (chance > 0f) {
				if (GetChanceMultiplier == null)
					GetChanceMultiplier = DefaultChanceMultiplier;

				IEnumerable<DropData> weightedDrops = dropData.Where(d => d.Chance <= 0f);
				foreach (DropData data in weightedDrops) {
					if (GetChanceMultiplier(data.ID, out float chanceMultiplier))
						data.UpdateWeight(chanceMultiplier);
				}

				IEnumerable<DropData> basicDrops = dropData.Where(d => d.Chance > 0f);
				if (weightedDrops.Count() > 0) {
					var rule = new OneFromWeightedOptionsNotScaledWithLuckDropRule(chance, weightedDrops, GetChanceMultiplier);
					if (rule.dropChance > 0f)
						itemDropRules.Add(rule);
				}

				foreach (DropData data in basicDrops) {
					float chanceMultiplier = GetChanceMultiplier(data.ID, out float mult) ? mult : 1f;
					itemDropRules.Add(new BasicDropRule(data, chanceMultiplier));
				}
			}

			return itemDropRules;
		}
		protected static void AddBossLoot(ILoot loot, NPC npc, IItemDropRule dropRule, bool bossBag) {
			bool bossCantDropBossBags = false;

			switch (npc.netID) {
				//UnobtainableBossBags
				case NPCID.CultistBoss:
				case NPCID.DD2DarkMageT1:
				case NPCID.DD2OgreT2:
					bossCantDropBossBags = !AndroMod.thoriumEnabled;
					break;
			}

			if (bossBag || bossCantDropBossBags) {
				loot.Add(dropRule);
			}
			else {
				loot.Add(new DropBasedOnExpertMode(dropRule, ItemDropRule.DropNothing()));
			}
		}
		private static void GetAllNpcDrops() {
			if (!AndroModSystem.StartedPostAddRecipes)
				return;

			allItemDropsFromNpcs = new();
			SortedDictionary<string, (int, float)> manuallySetModBossBags = new();
			foreach (KeyValuePair<int, NPC> npcPair in ContentSamples.NpcsByNetId) {
				int netID = npcPair.Key;
				NPC npc = npcPair.Value;
				IEnumerable<IItemDropRule> dropRules = Main.ItemDropsDB.GetRulesForNPCID(netID, false);
				foreach (IItemDropRule dropRule in dropRules) {
					List<DropRateInfo> dropRates = new();
					DropRateInfoChainFeed dropRateInfoChainFeed = new(1f);
					dropRule.ReportDroprates(dropRates, dropRateInfoChainFeed);
					foreach (DropRateInfo dropRate in dropRates) {
						int itemType = dropRate.itemId;
						float chance = dropRate.dropRate;
						//Item item = itemType.CSI();
						allItemDropsFromNpcs.AddOrCombineTouple(itemType, (netID, chance));
					}
				}

				if (GlobalBossBags.ManuallySetModBossBags.TryGetValue(npc.ModFullName(), out (string bagModName, float chance) value))
					manuallySetModBossBags.Add(value.bagModName, (netID, value.chance));
			}

			for (int i = 0; i < ItemLoader.ItemCount; i++) {
				if (manuallySetModBossBags.Count <= 0)
					break;

				string modFullName = i.CSI().ModFullName();
				if (manuallySetModBossBags.TryGetValue(modFullName, out (int netID, float chance) value)) {
					allItemDropsFromNpcs.AddOrCombineTouple(i, (value.netID, value.chance));
					manuallySetModBossBags.Remove(modFullName);
				}
			}

			//if (Debugger.IsAttached) allItemDropsFromNpcs.Select(p => p.Value.StringList(n => $"{n.Item1.CSNPC().S()} : {n.Item2.PercentString()}" , p.Key.CSI().S())).S("allItemDropsFromNpcs").LogSimple();
		}
	}
	public static class NPCStaticMethods {
		public static bool IsDummy(this NPC npc) => npc.netID < NPCID.Count ? npc.netID == NPCID.TargetDummy : npc.ModFullName() is string modFullName && (AndroMod.calamityEnabled && modFullName == "CalamityMod/SuperDummyNPC" || AndroMod.fargosEnabled && modFullName == "Fargowiltas/SuperDummy");
		public static bool IsBoss(this NPC npc) => npc.boss || AndroGlobalNPC.multipleSegmentBossTypes.ContainsKey(npc.netID) || AndroGlobalNPC.normalNpcsThatAreBosses.Contains(npc.netID);
		public static bool IsMiniBoss(this NPC npc) {
			switch (npc.netID) {
				case NPCID.Everscream:
				case NPCID.SantaNK1:
				case NPCID.IceQueen:
				case NPCID.Mothron:
				case NPCID.MourningWood:
				case NPCID.Pumpking:
				case NPCID.PumpkingBlade:
				case NPCID.DungeonGuardian:
				case NPCID.BigMimicCorruption:
				case NPCID.BigMimicCrimson:
				case NPCID.BigMimicHallow:
				case NPCID.BigMimicJungle:
				case NPCID.Paladin:
					return true;
				default:
					string modFullName = npc.ModFullName();
					if (AndroMod.calamityEnabled) {
						switch (modFullName) {
							case "CalamityMod/DesertNuisanceHeadYoung":
							case "CalamityMod/DesertNuisanceHead":
							case "CalamityMod/PerforatorHeadLarge":
							case "CalamityMod/PerforatorHeadMedium":
							case "CalamityMod/PerforatorHeadSmall":
							case "CalamityMod/AquaticScourgeHead":
							case "CalamityMod/ProfanedGuardianHealer":
							case "CalamityMod/ProfanedGuardianDefender":
							case "CalamityMod/SupremeCatastrophe":
							case "CalamityMod/SupremeCataclysm":
							case "CalamityMod/PlaguebringerMiniboss":
							case "CalamityMod/EidolonWyrmHead":
							case "CalamityMod/GiantClam":
							case "CalamityMod/Horse":
							case "CalamityMod/ThiccWaifu":
							case "CalamityMod/CragmawMire":
							case "CalamityMod/ArmoredDiggerHead":
							case "CalamityMod/GreatSandShark":
							case "CalamityMod/ColossalSquid":
							case "CalamityMod/ReaperShark":
							case "CalamityMod/Mauler":
							case "CalamityMod/NuclearTerror":
								return true;
							default:
								return false;
						}
					}

					//if (AndroMod.starsAboveEnabled) {
					//    switch(modFullName) {

					//    }
					//}

					if (AndroMod.thoriumEnabled) {
						switch (modFullName) {
							case "ThoriumMod/PatchWerk":
							case "ThoriumMod/CorpseBloom":
							case "ThoriumMod/Illusionist":
								return true;
						}
					}

					return false;
			}
		}
		public static bool ModBossPart(this NPC npc) {
			string modFullName = npc.ModFullName();
			if (AndroMod.calamityEnabled) {
				switch (modFullName) {
					case "CalamityMod/DesertNuisanceBodyYoung":
					case "CalamityMod/DesertNuisanceTailYoung":
					case "CalamityMod/DesertNuisanceBody":
					case "CalamityMod/DesertNuisanceTail":
					case "CalamityMod/PerforatorCyst":
					case "CalamityMod/PerforatorBodyLarge":
					case "CalamityMod/PerforatorBodyMedium":
					case "CalamityMod/PerforatorBodySmall":
					case "CalamityMod/PerforatorTailLarge":
					case "CalamityMod/PerforatorTailMedium":
					case "CalamityMod/PerforatorTailSmall":
					case "CalamityMod/AquaticScourgeBodyAlt":
					case "CalamityMod/AquaticScourgeBody":
					case "CalamityMod/AquaticScourgeTail":
					case "CalamityMod/EidolonWyrmBody":
					case "CalamityMod/EidolonWyrmBodyAlt":
					case "CalamityMod/EidolonWyrmTail":
					case "CalamityMod/ArmoredDiggerBody":
					case "CalamityMod/ArmoredDiggerTail":
						return true;
					default:
						return false;
				}
			}

			//if (AndroMod.starsAboveEnabled) {
			//    switch(modFullName) {

			//    }
			//}

			if (AndroMod.thoriumEnabled) {
				switch (modFullName) {
					case "ThoriumMod/IllusionGlass":
					case "ThoriumMod/IllusionistDecoy":
						return true;
				}
			}

			return false;
		}
		public static bool IsWorm(this NPC npc) {
			return npc.aiStyle == NPCAIStyleID.Worm || npc.aiStyle == NPCAIStyleID.TheDestroyer;
		}
		//public static bool IsDummy(this NPC npc) => npc.netID < NPCID.Count ? npc.netID == NPCID.TargetDummy : npc.ModFullName() is string modFullName && (AndroMod.calamityEnabled && modFullName == "CalamityMod/SuperDummyNPC" || AndroMod.fargosEnabled && modFullName == "Fargowiltas/SuperDummy");
		//public static bool IsBoss(this NPC npc) => npc.boss || WEGlobalNPC.multipleSegmentBossTypes.ContainsKey(npc.netID) || WEGlobalNPC.normalNpcsThatDropsBags.Contains(npc.netID);
		public static int RealNetID(this NPC npc) => npc.realLife == -1 ? npc.netID : Main.npc[npc.realLife].netID;
		public static NPC RealNPC(this NPC npc) => npc.realLife == -1 ? npc : Main.npc[npc.realLife];
		public static int RealLife(this NPC npc) => npc.realLife == -1 ? npc.life : Main.npc[npc.realLife].life;
		public static int RealLifeMax(this NPC npc) => npc.realLife == -1 ? npc.lifeMax : Main.npc[npc.realLife].lifeMax;
		public static int RealLifeRegen(this NPC npc) => npc.realLife == -1 ? npc.lifeRegen : Main.npc[npc.realLife].lifeRegen;
		public static float RealValue(this NPC npc) => npc.realLife == -1 ? npc.value : Main.npc[npc.realLife].value;
		public static void AddValue(this NPC npc, int value) {
			npc.extraValue += value;
		}

		public static string ModFullName(this NPC npc) => npc.ModNPC?.FullName ?? npc.type.GetNPCIDOrName();

		//Fix for Draedon boss having a null reference error in Calamity code when sampleNPC.FullName is called.  (Error in NPC.ToString())
		public static string FullName(this NPC npc) {
			if (AndroMod.calamityEnabled) {
				string sampleModNPCFullName = npc.ModNPC?.FullName;
				switch (sampleModNPCFullName) {
					case "CalamityMod/ThanatosBody1":
					case "CalamityMod/ThanatosBody2":
						return "Draedon";
					case "CalamityMod/ThanatosTail":
						return "XM-05 Thanatos";
					default:
						return npc.FullName;
				}
			}
			else {
				return npc.FullName;
			}
		}
		public static float GetReductionFactor(int hp) {
			float factor = hp < 7000 ? hp / 1000f + 1f : 8f;
			return factor;
		}
	}
}
