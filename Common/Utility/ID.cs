using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.Localization.GameCulture;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria;
using androLib.Common.Globals;
using Terraria.ModLoader;
using System.Diagnostics;
using static Terraria.ID.ContentSamples.CreativeHelper;
using rail;
using Terraria.Audio;

namespace androLib.Common.Utility
{
	public enum DamageClassID
	{
		Default,
		Generic,
		Melee,
		MeleeNoSpeed,
		Ranged,
		Magic,
		Summon,
		Whip,
		MagicSummonHybrid,
		Throwing,
		Rogue,
		Ki
	} //Located in DamageClassLoader.cs

	public static class L_ID_Tags {
		public const string Configs = "Configs";
	}
	public enum L_ID1
	{
		Items,
		Tooltip,
		Buffs,
		Dialogue,
		NPCNames,
		Bestiary,
		TownNPCMood,
		Ores,
		TableText,
		EnchantmentStorageText,
		Configs,
		UIText,
		StorageText,
		MagicStorageButtonsText,
		GameMessages,
		AndroLibGameMessages
	}
	public enum L_ID2
	{
		None,
		Witch,
		EffectDisplayName,
		EnchantmentEffects,
		EnchantmentCustomTooltips,
		EnchantmentTypeNames,
		EnchantmentShortTooltip,
		EnchantmentGeneralTooltips,
		ItemType,
		ArmorSlotNames,
		DamageClassNames,
		VanillaBuffs,
		Header,
		DisplayName
	}
	public enum L_ID3
	{
		Label,
		Tooltip
	}
	public enum L_ID_V
	{
		Item,
		Projectile,
		NPC,
		Buff,
		BuffDescription
	}

	public enum StorageTextID {
		LootAll,
		DepositAll,
		QuickStack,
		Sort,
		ToggleVacuum,
		DepositAllMagicStorage,
		CloseBag,
		//ToggleMarkTrash,
		//UncraftAllTrash,
		//RevertAllToBasic,
		//ManageTrash,
		//ManageOfferedItems,
		//QuickCraft,
		//Do not place anything besides buttons before this
		//EnchantmentStorage,
		Search,
		//OreBag,
		//EnchantmentLoadouts,
		//All,
		//HeldItem,
		//NoHeldItem,
		//LoadoutSizeChanged,
		//NotHighEnoughLevel,
		//NoArmor,
		//NoAccessories,
		//NoItems,
		//NotEnoughEnchantments,
		//Add,
		//Loadout
		ClearTrash,
	}
	public enum MagicStorageButtonsTextID {
		DepositAllFromVacuumBags
	}
	public enum AndroLibGameMessages {
		AddedToWhitelist,
		AddedToBlacklist,
	}

	public enum WikiTypeID
	{
		CraftingMaterial,
		Containments,
		CursedEssence,
		EnchantingTables,
		Enchantments,
		EnchantmentEssence,
		Furniture,
		CraftingStation,
		Storage,
		Armor,
		Set,
		Weapon,
		Tool,
		Mechanism,
		LightSource,
		PowerBooster,
		NPC
	}
	public static class WikiExtensionMethods
	{
		public static string GetLinkText(this WikiTypeID id, out bool external) {
			external = true;
			switch (id) {
				case WikiTypeID.CraftingMaterial:
					return "https://terraria.fandom.com/wiki/Category:Crafting_material_items";
				case WikiTypeID.Furniture:
					return "https://terraria.fandom.com/wiki/Furniture";
				case WikiTypeID.CraftingStation:
					return "https://terraria.fandom.com/wiki/Crafting_stations";
				case WikiTypeID.Storage:
					return "https://terraria.fandom.com/wiki/Storage_items";
				case WikiTypeID.Armor:
					return "https://terraria.fandom.com/wiki/Armor";
				case WikiTypeID.Set:
					return "https://terraria.fandom.com/wiki/Armor";
				case WikiTypeID.Weapon:
					return "https://terraria.fandom.com/wiki/Weapons";
				case WikiTypeID.Tool:
					return "https://terraria.fandom.com/wiki/Tools";
				case WikiTypeID.Mechanism:
					return "https://terraria.fandom.com/wiki/Mechanisms";
				case WikiTypeID.LightSource:
					return "https://terraria.fandom.com/wiki/Light_sources";
				case WikiTypeID.NPC:
					return "https://terraria.fandom.com/wiki/NPCs";
				default:
					external = false;
					return id.ToString().AddSpaces();
			}
		}
		public static string GetPNGLink(this IShoppingBiome shoppingBiome) {
			switch (shoppingBiome.NameKey) {
				case "Jungle":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a8/Bestiary_The_Jungle.png";
				case "Hallow":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b7/Bestiary_The_Hallow.png";
				case "Dungeon":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/81/Bestiary_The_Dungeon.png";
				case "Corruption":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/ab/Bestiary_The_Corruption.png";
				case "Crimson":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/63/Bestiary_The_Crimson.png";
				case "Glowing Mushroom":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/66/Bestiary_Surface_Mushroom.png";
				case "Snow":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fa/Bestiary_Snow.png";
				case "Ocean":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/aa/Bestiary_Ocean.png";
				case "Desert":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a8/Bestiary_Desert.png";
				case "Underground":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/79/Bestiary_Underground.png";
				case "Cavern":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/52/Bestiary_Caverns.png";
				case "The Underworld":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/30/Bestiary_The_Underworld.png";
				case "Forest":
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/42/Bestiary_Surface.png";
				default:
					return $"{shoppingBiome.NameKey} Not Found";
			}
		}
		public static string GetLinkText(this IShoppingBiome shoppingBiome) {
			return $"https://terraria.fandom.com/wiki/{shoppingBiome.NameKey}";
		}
		public static string ToLanguageName(this CultureName id) {
			switch (id) {
				case CultureName.English:
					return "en-US";
				case CultureName.French:
					return "fr-FR";
				case CultureName.German:
					return "de-DE";
				case CultureName.Italian:
					return "it-IT";
				case CultureName.Spanish:
					return "es-ES";
				case CultureName.Russian:
					return "ru-RU";
				case CultureName.Chinese:
					return "zh-Hans";
				case CultureName.Portuguese:
					return "pt-BR";
				case CultureName.Polish:
					return "pl-PL";
				default:
					return "CultureNameNotFound";
			}
		}
	}

	public enum SellCondition
	{
		IgnoreCondition,
		Never,
		Always,
		AnyTime,
		AnyTimeRare,
		PostKingSlime,
		PostEyeOfCthulhu,
		PostEaterOfWorldsOrBrainOfCthulhu,
		PostSkeletron,
		PostQueenBee,
		PostDeerclops,
		PostGoblinInvasion,
		Luck,
		HardMode,
		PostQueenSlime,
		PostPirateInvasion,
		PostTwins,
		PostDestroyer,
		PostSkeletronPrime,
		PostPlantera,
		PostGolem,
		PostMartianInvasion,
		PostDukeFishron,
		PostEmpressOfLight,
		PostCultist,
		PostSolarTower,
		PostNebulaTower,
		PostStardustTower,
		PostVortexTower,
		PostMoonLord,
	}

	public static class SellConditionStaticMethods {

		public static float GetSellPriceModifier(this SellCondition c) {
			switch (c) {
				case <= SellCondition.Always:
					return 1f;
				case SellCondition.AnyTime:
					return 2f;
				case < SellCondition.HardMode:
					return 5f + (float)c;
				case <= SellCondition.PostPlantera:
					return 20f + 10f * (c - SellCondition.HardMode);
				case <= SellCondition.PostCultist:
					return 100f + 50f * (c - SellCondition.PostGolem);
				case <= SellCondition.PostVortexTower:
					return 500f;
				case <= SellCondition.PostMoonLord:
					return 1000f;
				default:
					return 1f;
			}
		}
	}

	public static class GemSets {
		public static SortedSet<int> CommonGems = new() {
			ItemID.Topaz,
			ItemID.Sapphire,
			ItemID.Ruby,
			ItemID.Emerald,
			ItemID.Amethyst
		};
		public static SortedSet<int> RareGems = new() {
			ItemID.Amber,
			ItemID.Diamond
		};
	}

	public static class ToolStrategyID {
		public static readonly int Light2ndPassGlowStickOnly = -2;
		public static readonly int None = -1;
		public static readonly int Light = 0;
		public static readonly int Hammer = 1;
		public static readonly int Axe = 2;
		public static readonly int Pickaxe = 3;
		public static readonly int WetLight = 4;
		public static readonly int WetLongDistanceThrow = 5;
		public static readonly int Cannon = 6;
		public static readonly int Extractinator = 7;
		public static readonly int PaintScraper = 8;
		public static readonly int Count = 9;

		public static readonly SortedDictionary<int, Func<Item, bool>> ToolStrategyConditions = new() {
			{ Light2ndPassGlowStickOnly, (item) => ItemSets.IsGlowstick(item) },
			{ Light, (item) => ItemSets.IsTorch(item) },
			{ Hammer, (item) => item.hammer > 0 },
			{ Axe, (item) => item.axe > 0 },
			{ Pickaxe, (item) => item.pick > 0 },
			{ WetLight, (item) => ItemSets.IsGlowstick(item) || ItemSets.ISFlareGun(item) || ItemSets.IsWaterTorch(item) },
			{ WetLongDistanceThrow, (item) => ItemSets.ISFlareGun(item) || ItemSets.IsGlowstick(item) },
			{ Cannon, (item) => false },//Requires checking the hover tile
			{ Extractinator, (item) => false },//Requires checking the hover tile
			{ PaintScraper, (item) => ItemID.Sets.IsPaintScraper[item.type] },
		};
	}

	public static class ItemSets {
		public static bool IsEnchantable(this Item item) {
			if (item.IsArmorItem() || item.IsWeaponItem() || item.IsAccessoryItem() || item.IsFishingPole() || item.IsTool()) {
				return true;
			}
			else {
				return false;
			}
		}
		public static bool IsWeaponItem(this Item item) {
			if (item.NullOrAir())
				return false;

			if (IsArmorItem(item))
				return false;

			if (item.ModItem != null) {
				string modName = item.ModItem.Mod.Name;
				//Manually prevent calamity items from being weapons
				if (AndroMod.calamityEnabled && modName == AndroMod.calamityModName) {
					switch (item.ModFullName()) {
						case "CalamityMod/WulfrumFusionCannon":
							return false;
					}
				}

				//Manually prevent magic storage items from being weapons
				if (AndroMod.magicStorageEnabled && modName == AndroMod.magicStorageName) {
					switch (item.ModFullName()) {
						case "MagicStorage/BiomeGlobe":
							return false;
					}
				}

				if (AndroMod.thoriumEnabled && modName == AndroMod.thoriumModName) {
					string modItemFullName = item.ModFullName();
					switch (modItemFullName) {
						case "ThoriumMod/HiveMind":
						case "ThoriumMod/PiousBanner":
						case "ThoriumMod/PrecisionBanner":
							return false;
						case "ThoriumMod/TechniqueBloodLotus":
						case "ThoriumMod/TechniqueCobraBite":
						case "ThoriumMod/TechniqueHiddenBlade":
						case "ThoriumMod/TechniquePaperExplosive":
						case "ThoriumMod/TechniqueShadowClone":
						case "ThoriumMod/Gauze":
							return true;
						default:
							if (modItemFullName.Contains("InspirationNote") || modItemFullName.Contains("Tester"))
								return false;

							break;
					}

					//Some Thorium non-weapon consumables were counting as weapons.
					if (item.consumable && item.damage <= 0 && item.mana <= 0)
						return false;
				}

				if (AndroMod.fargosEnabled && modName == AndroMod.fargosModName) {
					switch (item.ModFullName()) {
						case "Fargowiltas/BrittleBone":
							return false;
					}
				}

				if (AndroMod.amuletOfManyMinionsEnabled && modName == AndroMod.amuletOfManyMinionsName) {
					List<string> aOMMNonItemNames = new() {
						"AmuletOfManyMinions/AxolotlMinionItem",
						"AmuletOfManyMinions/CinderHenMinionItem",
						"AmuletOfManyMinions/WyvernFlyMinionItem",
						"AmuletOfManyMinions/TruffleTurtleMinionItem",
						"AmuletOfManyMinions/SmolederMinionItem",
						"AmuletOfManyMinions/PlantPupMinionItem",
						"AmuletOfManyMinions/LilGatorMinionItem",
						"AmuletOfManyMinions/CloudiphantMinionItem"
					};
					string modItemFullName = item.ModFullName();
					if (aOMMNonItemNames.Contains(modItemFullName) || modItemFullName.Contains("Replica"))
						return false;
				}
			}

			bool isWeapon;
			switch (item.type) {
				case ItemID.ExplosiveBunny:
				case ItemID.TreeGlobe:
				case ItemID.WorldGlobe:
				case ItemID.MoonGlobe:
					isWeapon = false;
					break;
				case ItemID.LawnMower:
					isWeapon = true;
					break;
				default:
					isWeapon = (item.DamageType != DamageClass.Default || item.damage > 0 || item.crit > 0) && (item.ammo == 0 || item.ammo == ItemID.Grenade);
					break;
			}

			return isWeapon && !item.accessory;
		}
		public static bool IsArmorItem(this Item item) {
			if (item.NullOrAir())
				return false;

			return !item.vanity && (item.headSlot > -1 || item.bodySlot > -1 || item.legSlot > -1);
		}
		public static bool IsAccessoryItem(this Item item) {
			if (item.NullOrAir())
				return false;

			//Check for armor item is a fix for Reforge-able armor mod setting armor to accessories
			return item.accessory && !IsArmorItem(item);
		}
		public static bool IsFishingPole(this Item item) {
			if (item.NullOrAir())
				return false;

			return item.fishingPole > 0;
		}
		public static bool IsTool(this Item item) {
			if (item.NullOrAir())
				return false;

			switch (item.type) {
				case ItemID.Clentaminator:
				case ItemID.BugNet:
				case ItemID.GoldenBugNet:
				case ItemID.FireproofBugNet:
				case ItemID.BottomlessBucket:
				case ItemID.BottomlessLavaBucket:
				case ItemID.SuperAbsorbantSponge:
				case ItemID.LavaAbsorbantSponge:
				case ItemID.Clentaminator2:
					return true;
				default:
					return item.mana > 0 && !IsWeaponItem(item);
			}
		}
		public static bool IsBanner(this Item item, out int banner) => IsBannerItem(!item.NullOrAir() ? item.type : ItemID.None, out banner);
		public static bool IsBannerItem(this int itemType, out int banner) => ItemToBanner.TryGetValue(itemType, out banner);
		public static bool IsBanner(this Item item) => !item.NullOrAir() && IsBannerItem(item.type);
		public static bool IsBannerItem(this int itemType) => ItemToBanner.ContainsKey(itemType);
		public static SortedDictionary<int, int> ItemToBanner {
			get {
				if (itemToBanner == null)
					SetupItemToBanner();

				return itemToBanner;
			}
		}
		private static SortedDictionary<int, int> itemToBanner = null;
		public static IEnumerable<int> AllBanners => ItemToBanner.Select(p => p.Key);
		private static void SetupItemToBanner() {
			itemToBanner = new();

			for (int npcid = NPCID.NegativeIDCount + 1; npcid < NPCLoader.NPCCount; npcid++) {
				NPC npc = ContentSamples.NpcsByNetId[npcid];
				int bannerID = Item.NPCtoBanner(npc.BannerID());
				int bannerItemID = Item.BannerToItem(bannerID);
				if (bannerItemID > ItemID.None && bannerItemID < ItemLoader.ItemCount && bannerID > 0) {
					if (!itemToBanner.TryAdd(bannerItemID, bannerID) && Debugger.IsAttached)
						$"banner {bannerID} already exists.  current [{ContentSamples.NpcsByNetId[Item.BannerToNPC(itemToBanner[bannerItemID])].S()}], new [{bannerItemID.GetItemIDOrName()}, npc: {npc.S()}]".LogSimple();
				}
			}
		}
		public static bool IsBossTrophy(this Item item, string lowerName = null) {
			if (item.NullOrAir())
				return false;

			if (lowerName == null)
				lowerName = item.GetItemInternalName().ToLower();

			int bossTrophyValue = Item.sellPrice(0, 1);
			if (lowerName.EndsWith("mastertrophy")
				&& item.useStyle == ItemUseStyleID.Swing
				&& item.useTurn == true
				&& item.autoReuse == true
				&& item.consumable == true
				&& item.createTile > -1
				&& item.value == bossTrophyValue
				&& item.rare == ItemRarityID.Master
				) {
				return true;
			}

			return false;
		}
		public static bool IsBossRelic(this Item item, string lowerName = null) {
			if (item.NullOrAir())
				return false;

			if (lowerName == null)
				lowerName = item.GetItemInternalName().ToLower();

			if (lowerName.Contains("relic")
					&& item.useStyle == ItemUseStyleID.Swing
					&& item.useTurn
					&& item.autoReuse
					&& item.consumable
					) {
				return true;
			}

			return false;
		}
		public static bool IsBossSpawner(this Item item) {
			if (ItemID.Sets.SortingPriorityBossSpawns[item.type] > -1
					&& item.useStyle == ItemUseStyleID.HoldUp
					&& item.consumable
					&& item.useAnimation == 45
					&& item.useTime == 45
					) {
				return true;
			}

			ItemGroupAndOrderInGroup group = new ItemGroupAndOrderInGroup(item);
			return group.Group == ItemGroup.BossSpawners;
		}
		public static bool IsRope(this Item item) => !item.NullOrAir() && (ItemID.Sets.SortingPriorityRopes[item.type] != -1 && item.type != ItemID.Vine || item.type == ItemID.VineRope);
		public static bool IsTorch(this Item item) => !item.NullOrAir() && ItemID.Sets.Torches[item.type];
		public static bool IsWaterTorch(this Item item) => !item.NullOrAir() && ItemID.Sets.WaterTorches[item.type];
		public static bool IsGlowstick(this Item item) => !item.NullOrAir() && ItemID.Sets.Glowsticks[item.type];
		public static bool ISFlareGun(this Item item) => !item.NullOrAir() && item.useAmmo == ItemID.Flare;
		public static SortedSet<int> RequiredTiles {
			get {
				if (requiredTiles == null) {
					requiredTiles = new();

					for (int i = 0; i < Main.recipe.Length; i++) {
						Recipe r = Main.recipe[i];

						if (r.createItem.NullOrAir())
							continue;

						foreach (int tile in r.requiredTile) {
							if (tile < TileID.Dirt)
								continue;

							requiredTiles.Add(tile);
						}
					}
				}

				return requiredTiles;
			}
		}
		private static SortedSet<int> requiredTiles = null;
		public static bool IsRequiredTile(this Item item) => !item.NullOrAir() && item.createTile > -1 && RequiredTiles.Contains(item.createTile);

		#region Passive Tile Buffs

		private static void Campfire(SceneMetrics sceneMetrics) => sceneMetrics.HasCampfire = true;
		private static void CatBast(SceneMetrics sceneMetrics) => sceneMetrics.HasCatBast = true;
		private static void HeartLantern(SceneMetrics sceneMetrics) => sceneMetrics.HasHeartLantern = true;
		private static void StarInBottle(SceneMetrics sceneMetrics) => sceneMetrics.HasStarInBottle = true;
		private static void Sunflower(SceneMetrics sceneMetrics) => sceneMetrics.HasSunflower = true;
		private static void GardenGnome(SceneMetrics sceneMetrics) => sceneMetrics.HasGardenGnome = true;
		private static void PeaceCandle(SceneMetrics sceneMetrics) => sceneMetrics.PeaceCandleCount++;
		private static void WaterCandle(SceneMetrics sceneMetrics) => sceneMetrics.WaterCandleCount++;
		private static void ShadowCandle(SceneMetrics sceneMetrics) =>	sceneMetrics.ShadowCandleCount++;
		private static bool HasCampfire(SceneMetrics sceneMetrics) => sceneMetrics.HasCampfire;
		private static bool HasCatBast(SceneMetrics sceneMetrics) => sceneMetrics.HasCatBast;
		private static bool HasHeartLantern(SceneMetrics sceneMetrics) => sceneMetrics.HasHeartLantern;
		private static bool HasStarInBottle(SceneMetrics sceneMetrics) => sceneMetrics.HasStarInBottle;
		private static bool HasSunflower(SceneMetrics sceneMetrics) => sceneMetrics.HasSunflower;
		private static bool HasGardenGnome(SceneMetrics sceneMetrics) => sceneMetrics.HasGardenGnome;
		private static bool HasPeaceCandle(SceneMetrics sceneMetrics) => sceneMetrics.PeaceCandleCount > 0;
		private static bool HasWaterCandle(SceneMetrics sceneMetrics) => sceneMetrics.WaterCandleCount > 0;
		private static bool HasShadowCandle(SceneMetrics sceneMetrics) => sceneMetrics.ShadowCandleCount > 0;
		private static void SetupPassiveBuffTileEffectDictionaries() {
			passiveBuffTileEffects = new() {
				{ ItemID.CatBast, CatBast },
				{ ItemID.HeartLantern, HeartLantern },
				{ ItemID.StarinaBottle, StarInBottle },
				{ ItemID.Sunflower, Sunflower },
				{ ItemID.GardenGnome, GardenGnome },
				{ ItemID.PeaceCandle, PeaceCandle },
				{ ItemID.WaterCandle, WaterCandle },
				{ ItemID.ShadowCandle, ShadowCandle },
			};

			passiveBuffTileEffectsAlreadyActive = new() {
				{ ItemID.CatBast, HasCatBast },
				{ ItemID.HeartLantern, HasHeartLantern },
				{ ItemID.StarinaBottle, HasStarInBottle },
				{ ItemID.Sunflower, HasSunflower },
				{ ItemID.GardenGnome, HasGardenGnome },
				{ ItemID.PeaceCandle, HasPeaceCandle },
				{ ItemID.WaterCandle, HasWaterCandle },
				{ ItemID.ShadowCandle, HasShadowCandle },
			};

			int setCount = TileID.Sets.Campfire.Length;
			foreach (Item item in ContentSamples.ItemsByType.Select(p => p.Value)) {
				if (item.createTile > -1 && item.createTile < setCount) {
					if (TileID.Sets.Campfire[item.createTile]) {
						passiveBuffTileEffects.Add(item.type, Campfire);
						passiveBuffTileEffectsAlreadyActive.Add(item.type, HasCampfire);
					}
				}
			}
		}
		public static SortedDictionary<int, Action<SceneMetrics>> PassiveBuffTileEffects {
			get {
				if (passiveBuffTileEffects == null)
					SetupPassiveBuffTileEffectDictionaries();

				return passiveBuffTileEffects;
			}
		}
		public static SortedDictionary<int, Action<SceneMetrics>> passiveBuffTileEffects = null;
		public static SortedDictionary<int, Func<SceneMetrics, bool>> PassiveBuffTileEffectsAlreadyActive {
			get {
				if (passiveBuffTileEffectsAlreadyActive == null)
					SetupPassiveBuffTileEffectDictionaries();

				return passiveBuffTileEffectsAlreadyActive;
			}
		}
		public static SortedDictionary<int, Func<SceneMetrics, bool>> passiveBuffTileEffectsAlreadyActive = null;
		public static bool IsPassiveBuffTile(this Item item, out Action<SceneMetrics> buff) {
			if (!item.NullOrAir() && PassiveBuffTileEffects.TryGetValue(item.type, out buff))
				return true;

			buff = null;
			return false;
		}
		public static bool IsPassiveBuffTile(this Item item) => PassiveBuffTileEffects.ContainsKey(item.type);
		private static SortedSet<int> passiveBuffCandles = new() {
			ItemID.WaterCandle,
			ItemID.PeaceCandle,
			ItemID.ShadowCandle,
		};
		public static bool IsPassiveBuffCandle(this Item item) => passiveBuffCandles.Contains(item.type);
		public static bool PassiveBuffTileIsActive(this Item item, SceneMetrics sceneMetrics) {
			if (!PassiveBuffTileEffectsAlreadyActive.TryGetValue(item.type, out Func<SceneMetrics, bool> active))
				return false;

			if (!active(sceneMetrics))
				return false;

			return true;
		}
		public static SortedSet<int> Buckets = new() {
			ItemID.EmptyBucket,
			ItemID.WaterBucket,
			ItemID.LavaBucket,
			ItemID.HoneyBucket,
			ItemID.BottomlessBucket,
			ItemID.BottomlessLavaBucket,
			ItemID.BottomlessHoneyBucket,
			ItemID.BottomlessShimmerBucket
		};
		public static bool IsBucket(this Item item) => !item.NullOrAir() && Buckets.Contains(item.type);

		#endregion

		#region Active Tile Buffs

		public const int InfiniteBuffDuration = 108000;
		public const int SliceOfCakeBuffDuration = 7200;
		private static void AmmoBox(Player player) => player.AddBuff(BuffID.AmmoBox, InfiniteBuffDuration);
		private static void BewitchingTable(Player player) => player.AddBuff(BuffID.Bewitched, InfiniteBuffDuration);
		private static void CrystalBall(Player player) => player.AddBuff(BuffID.Clairvoyance, InfiniteBuffDuration);
		private static void SharpeningStation(Player player) => player.AddBuff(BuffID.Sharpened, InfiniteBuffDuration);
		private static void WarTable(Player player) => player.AddBuff(BuffID.WarTable, InfiniteBuffDuration);
		private static void SliceOfCake(Player player) => player.AddBuff(BuffID.SugarRush, SliceOfCakeBuffDuration);
		private static bool HasAmmoBox(Player player) => player.HasBuff(BuffID.AmmoBox);
		private static bool HasBewitchingTable(Player player) => player.HasBuff(BuffID.Bewitched);
		private static bool HasCrystalBall(Player player) => player.HasBuff(BuffID.Clairvoyance);
		private static bool HasSharpeningStation(Player player) => player.HasBuff(BuffID.Sharpened);
		private static bool HasWarTable(Player player) => player.HasBuff(BuffID.WarTable);
		private static bool HasSliceOfCake(Player player) => player.HasBuff(BuffID.SugarRush);
		private static void AmmoBoxSound(Player player) => SoundEngine.PlaySound(SoundID.Item149, player.position);
		private static void BewitchingTableSound(Player player) => SoundEngine.PlaySound(SoundID.Item4, player.position);
		private static void CrystalBallSound(Player player) => SoundEngine.PlaySound(SoundID.Item4, player.position);
		private static void SharpeningStationSound(Player player) => SoundEngine.PlaySound(SoundID.Item37, player.position);
		private static void WarTableSound(Player player) => SoundEngine.PlaySound(SoundID.Item4, player.position);
		private static void SliceOfCakeSound(Player player) => SoundEngine.PlaySound(SoundID.Item2, player.position);
		private static void SetupActiveBuffTileEffectDictionaries() {
			activeBuffTileEffects = new() {
				{ ItemID.AmmoBox, AmmoBox },
				{ ItemID.BewitchingTable, BewitchingTable },
				{ ItemID.CrystalBall, CrystalBall },
				{ ItemID.SharpeningStation, SharpeningStation },
				{ ItemID.WarTable, WarTable },
				{ ItemID.SliceOfCake, SliceOfCake },
			};

			activeBuffTileEffectsAlreadyActive = new() {
				{ ItemID.AmmoBox, HasAmmoBox },
				{ ItemID.BewitchingTable, HasBewitchingTable },
				{ ItemID.CrystalBall, HasCrystalBall },
				{ ItemID.SharpeningStation, HasSharpeningStation },
				{ ItemID.WarTable, HasWarTable },
				{ ItemID.SliceOfCake, HasSliceOfCake },
			};

			activeBuffTileSounds = new() {
				{ ItemID.AmmoBox, AmmoBoxSound },
				{ ItemID.BewitchingTable, BewitchingTableSound },
				{ ItemID.CrystalBall, CrystalBallSound },
				{ ItemID.SharpeningStation, SharpeningStationSound },
				{ ItemID.WarTable, WarTableSound },
				{ ItemID.SliceOfCake, SliceOfCakeSound },
			};
		}
		public static SortedDictionary<int, Action<Player>> ActiveBuffTileEffects {
			get {
				if (activeBuffTileEffects == null)
					SetupActiveBuffTileEffectDictionaries();

				return activeBuffTileEffects;
			}
		}
		public static SortedDictionary<int, Action<Player>> activeBuffTileEffects = null;
		public static SortedDictionary<int, Func<Player, bool>> ActiveBuffTileEffectsAlreadyActive {
			get {
				if (activeBuffTileEffectsAlreadyActive == null)
					SetupActiveBuffTileEffectDictionaries();

				return activeBuffTileEffectsAlreadyActive;
			}
		}
		public static SortedDictionary<int, Func<Player, bool>> activeBuffTileEffectsAlreadyActive = null;
		public static SortedDictionary<int, Action<Player>> ActiveBuffTileSounds {
			get {
				if (activeBuffTileSounds == null)
					SetupActiveBuffTileEffectDictionaries();

				return activeBuffTileSounds;
			}
		}
		public static SortedDictionary<int, Action<Player>> activeBuffTileSounds = null;
		public static bool IsActiveBuffTile(this Item item, out Action<Player> buff) {
			if (!item.NullOrAir() && ActiveBuffTileEffects.TryGetValue(item.type, out buff))
				return true;

			buff = null;
			return false;
		}
		public static bool IsActiveBuffTile(this Item item) => !item.NullOrAir() && ActiveBuffTileEffects.ContainsKey(item.type);
		public static bool IsActiveBuffTileAndHasBuff(this Item item, Player player) => IsActiveBuffTile(item) && HasActiveTileBuff(item, player);
		public static bool HasActiveTileBuff(this Item item, Player player) => !item.NullOrAir() && ActiveBuffTileEffectsAlreadyActive.TryGetValue(item.type, out Func<Player, bool> func) && func(player);
		public static void PlayActiveBuffTileSound(this Item item, Player player) {
			if (!item.NullOrAir() && ActiveBuffTileSounds.TryGetValue(item.type, out Action<Player> sound))
				sound(player);
		}

		#endregion

		public static SortedSet<int> PotionBuffs {
			get {
				if (potionBuffs == null)
					SetupBuffToItem();

				return potionBuffs;
			}
		}
		public static SortedSet<int> potionBuffs = null;
		private static void SetupBuffToItem() {
			potionBuffs = new();
			for (int i = 0; i < ItemLoader.ItemCount; i++) {
				Item item = ContentSamples.ItemsByType[i];
				if (item.buffType > 0 && item.consumable && !Main.debuff[item.buffType])
					potionBuffs.Add(item.buffType);
			}

			potionBuffs.Remove(BuffID.Honey);
		}
		public static bool IsPotionBuff(int buffType) => PotionBuffs.Contains(buffType);
	}
}
