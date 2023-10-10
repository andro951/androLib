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
using static androLib.Common.Utility.LogSystem.WikiStaticMethods;

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
	public enum EItemType {
		None,
		Weapons,
		Armor,
		Accessories,
		FishingPoles,
		Tools
	}
	public enum ArmorSlotSpecificID {
		Head,
		Body,
		Legs
	}

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
		AndroLibGameMessages,
		GameModeNameIDs,
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
		Switch,
		ToggleMagicStorageDeposit,
	}
	public enum MagicStorageButtonsTextID {
		DepositAllFromVacuumBags
	}
	public enum AndroLibGameMessages {
		AddedToWhitelist,
		AddedToBlacklist,
		BossChecklistNotEnabled,
		FailedDetermineProgression,
		UnableDetermineNPCDropsBossBag,
		MainUpdateCount,
		ReportErrorToAndro,
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


	public enum ChestID {
		None = -1,
		Chest_Normal,
		Gold,
		Gold_Locked,
		Shadow,
		Shadow_Locked,
		RichMahogany = 8,
		Ivy = 10,
		Frozen,
		LivingWood,
		Skyware,
		WebCovered = 15,
		Lihzahrd,
		Water,
		Jungle_Dungeon = 23,
		Corruption_Dungeon,
		Crimson_Dungeon,
		Hallowed_Dungeon,
		Ice_Dungeon,
		Mushroom = 32,
		Granite = 40,
		Marble,
		Gold_DeadMans = 104,
		SandStone = 110,
		Desert_Dungeon = 113
	}
	public static class ChestIDMethods {
		public static int GetItemType(this ChestID id) {
			switch (id) {
				case ChestID.Chest_Normal:
					return ItemID.Chest;
				case ChestID.Gold:
					return ItemID.GoldChest;
				case ChestID.Gold_Locked:
					return ItemID.Fake_GoldChest;
				case ChestID.Shadow:
					return ItemID.ShadowChest;
				case ChestID.Shadow_Locked:
					return ItemID.Fake_ShadowChest;
				case ChestID.RichMahogany:
					return ItemID.RichMahoganyChest;
				case ChestID.Ivy:
					return ItemID.IvyChest;
				case ChestID.Frozen:
					return ItemID.FrozenChest;
				case ChestID.LivingWood:
					return ItemID.LivingWoodChest;
				case ChestID.Skyware:
					return ItemID.SkywareChest;
				case ChestID.WebCovered:
					return ItemID.WebCoveredChest;
				case ChestID.Lihzahrd:
					return ItemID.LihzahrdChest;
				case ChestID.Water:
					return ItemID.WaterChest;
				case ChestID.Jungle_Dungeon:
					return ItemID.JungleChest;
				case ChestID.Corruption_Dungeon:
					return ItemID.CorruptionChest;
				case ChestID.Crimson_Dungeon:
					return ItemID.CrimsonChest;
				case ChestID.Hallowed_Dungeon:
					return ItemID.HallowedChest;
				case ChestID.Ice_Dungeon:
					return ItemID.IceChest;
				case ChestID.Mushroom:
					return ItemID.MushroomChest;
				case ChestID.Granite:
					return ItemID.GraniteChest;
				case ChestID.Marble:
					return ItemID.MarbleChest;
				case ChestID.Gold_DeadMans:
					return ItemID.DeadMansChest;
				case ChestID.SandStone:
					return ItemID.DesertChest;
				case ChestID.Desert_Dungeon:
					return ItemID.DungeonDesertChest;
				default:
					return -1;
			}
		}
	}
	public enum CrateID {
		None = -1,
		Wooden = ItemID.WoodenCrate,
		Iron = ItemID.IronCrate,
		Golden = ItemID.GoldenCrate,
		Jungle = ItemID.JungleFishingCrate,
		Sky = ItemID.FloatingIslandFishingCrate,
		Corrupt = ItemID.CorruptFishingCrate,
		Crimson = ItemID.CrimsonFishingCrate,
		Hallowed = ItemID.HallowedFishingCrate,
		Dungeon = ItemID.DungeonFishingCrate,
		Frozen = ItemID.FrozenCrate,
		Oasis = ItemID.OasisCrate,
		Obsidian = ItemID.LavaCrate,
		Ocean = ItemID.OceanCrate,
		Pearlwood_WoodenHard = ItemID.WoodenCrateHard,
		Mythril_IronHard = ItemID.IronCrateHard,
		Titanium_GoldenHard = ItemID.GoldenCrateHard,
		Bramble_JungleHard = ItemID.JungleFishingCrateHard,
		Azure_SkyHard = ItemID.FloatingIslandFishingCrateHard,
		Defiled_CorruptHard = ItemID.CorruptFishingCrateHard,
		Hematic_CrimsonHard = ItemID.CrimsonFishingCrateHard,
		Divine_HallowedHard = ItemID.HallowedFishingCrateHard,
		Stockade_DungeonHard = ItemID.DungeonFishingCrateHard,
		Boreal_FrozenHard = ItemID.FrozenCrateHard,
		Mirage_OasisHard = ItemID.OasisCrateHard,
		Hellstone_ObsidianHard = ItemID.LavaCrateHard,
		Seaside_OceanHard = ItemID.OceanCrateHard,

		Golden_LockBox = ItemID.LockBox,
		Obsidian_LockBox = ItemID.ObsidianLockbox
	}
	public enum DashID : byte {
		NinjaTabiDash = 1,
		EyeOfCthulhuShieldDash,
		SolarDash,
		CrystalNinjaDash = 5
	}
	public static class SellConditionMethods {
		public static bool CanSell(this SellCondition condition) {
			switch (condition) {
				case SellCondition.Always:
				case SellCondition.AnyTime:
				case SellCondition.AnyTimeRare:
					return true;
				case SellCondition.PostKingSlime:
					return NPC.downedSlimeKing;
				case SellCondition.PostEyeOfCthulhu:
					return NPC.downedBoss1;
				case SellCondition.PostEaterOfWorldsOrBrainOfCthulhu:
					return NPC.downedBoss2;
				case SellCondition.PostSkeletron:
					return NPC.downedBoss3;
				case SellCondition.PostQueenBee:
					return NPC.downedQueenBee;
				case SellCondition.PostQueenSlime:
					return NPC.downedQueenSlime;
				case SellCondition.PostNebulaTower:
					return NPC.downedTowerNebula;
				case SellCondition.PostSolarTower:
					return NPC.downedTowerSolar;
				case SellCondition.PostStardustTower:
					return NPC.downedTowerStardust;
				case SellCondition.PostVortexTower:
					return NPC.downedTowerVortex;
				case SellCondition.PostDeerclops:
					return NPC.downedDeerclops;
				case SellCondition.PostGoblinInvasion:
					return NPC.downedGoblins;
				case SellCondition.HardMode:
					return Main.hardMode;
				case SellCondition.PostGolem:
					return NPC.downedGolemBoss;
				case SellCondition.PostTwins:
					return NPC.downedMechBoss1;
				case SellCondition.PostDestroyer:
					return NPC.downedMechBoss2;
				case SellCondition.PostSkeletronPrime:
					return NPC.downedMechBoss3;
				case SellCondition.PostPirateInvasion:
					return NPC.downedPirates;
				case SellCondition.PostPlantera:
					return NPC.downedPlantBoss;
				case SellCondition.PostEmpressOfLight:
					return NPC.downedEmpressOfLight;
				case SellCondition.PostDukeFishron:
					return NPC.downedFishron;
				case SellCondition.PostCultist:
					return NPC.downedAncientCultist;
				case SellCondition.PostMoonLord:
					return NPC.downedMoonlord;
				case SellCondition.Never:
				default:
					return false;
			}
		}
	}
	public enum TownNPCTypeID {
		Guide = 22,
		Merchant = 17,
		Nurse = 18,
		Demolitionist = 38,
		Angler = 369,
		Dryad = 20,
		ArmsDealer = 19,
		DyeTrader = 207,
		Painter = 227,
		Stylist = 353,
		Zoologist = 633,
		Tavernkeep = 550,
		Golfer = 588,
		GoblinTinkerer = 107,
		WitchDoctor = 228,
		Mechanic = 124,
		Clothier = 54,
		Wizard = 108,
		Steampunker = 178,
		Pirate = 229,
		Truffle = 160,
		TaxCollector = 441,
		Cyborg = 209,
		PartyGirl = 208,
		Princess = 663,
		SantaClaus = 142,
		Cat = 637,
		Dog = 638,
		Bunny = 656,
		TravelingMerchant = 368,
		SkeletonMerchant = 453,
		OldMan = 37
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
					return 15f + 3f * (c - SellCondition.HardMode);
				case <= SellCondition.PostCultist:
					return 35f + 5f * (c - SellCondition.PostGolem);
				case <= SellCondition.PostVortexTower:
					return 100f;
				case <= SellCondition.PostMoonLord:
					return 150f;
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
	public enum DialogueID {
		StandardDialogue,
		BloodMoon,
		BirthdayParty,
		Storm,
		QueenBee,
		Content,
		NoHome,
		LoveSpace,
		FarFromHome,
		DislikeCrowded,
		HateCrowded,
		LikeBiome,
		LoveBiome,
		DislikeBiome,
		HateBiome,
		LikeNPC,
		LoveNPC,
		DislikeNPC,
		HateNPC
	}
	public enum BiomeID {
		Beach,
		Corrupt,
		Crimson,
		Desert,
		DirtLayerHeight,
		Dungeon,
		Forest,
		GemCave,
		Glowshroom,
		Granite,
		Graveyard,
		Hallow,
		Hive,
		Jungle,
		LihzhardTemple,
		Marble,
		Meteor,
		OldOneArmy,
		OverworldHeight,
		PeaceCandle,
		Rain,
		RockLayerHeight,
		Sandstorm,
		SkyHeight,
		Snow,
		TowerNebula,
		TowerStardust,
		TowerSolar,
		TowerVortex,
		UndergroundDesert,
		UnderworldHeight,
		WaterCandle
	}
	public enum DropRestrictionsID {
		None,
		HardModeBosses,
		PostPlanteraBosses
	}
	public enum InvasionID {
		Goblin_Army,
		Pirate_Invasion,
		Martian_Madness
	}

	public enum FloatID {
		none,
		left,
		middle,
		right,
	}

	public static class ItemIDMethods {
		public static string GetItemPNGLink(this int itemType) {
			string link = GetExternalItemPNGLink(itemType);
			if (link == "")
				link = $"Item_{itemType}".ToPNG();

			return link;
		}
		private static string GetExternalItemPNGLink(int itemType) {
			switch (itemType) {
				case ItemID.None://0 
					return "";
				case ItemID.IronPickaxe://1 Iron Pickaxe
					return "";
				case ItemID.DirtBlock://2 Dirt Block
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/55/Dirt_Block.png/revision/latest?cb=20200516211400&format=original";
				case ItemID.StoneBlock://3 Stone Block
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/37/Stone_Block.png/revision/latest?cb=20200516222613&format=original";
				case ItemID.IronBroadsword://4 Iron Broadsword
					return "";
				case ItemID.Mushroom://5 Mushroom
					return "";
				case ItemID.IronShortsword://6 Iron Shortsword
					return "";
				case ItemID.IronHammer://7 Iron Hammer
					return "";
				case ItemID.Torch://8 Torch
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b2/Torch.png/revision/latest?cb=20200516223044&format=original";
				case ItemID.Wood://9 Wood
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/df/Wood.png/revision/latest?cb=20200516223631&format=original";
				case ItemID.IronAxe://10 Iron Axe
					return "";
				case ItemID.IronOre://11 Iron Ore
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/87/Iron_Ore.png/revision/latest?cb=20200516214315&format=original";
				case ItemID.CopperOre://12 Copper Ore
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/78/Copper_Ore.png/revision/latest?cb=20200516210045&format=original";
				case ItemID.GoldOre://13 Gold Ore
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f7/Gold_Ore.png/revision/latest?cb=20200516213312&format=original";
				case ItemID.SilverOre://14 Silver Ore
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/97/Silver_Ore.png/revision/latest?cb=20200516221835&format=original";
				case ItemID.CopperWatch://15 Copper Watch
					return "";
				case ItemID.SilverWatch://16 Silver Watch
					return "";
				case ItemID.GoldWatch://17 Gold Watch
					return "";
				case ItemID.DepthMeter://18 Depth Meter
					return "";
				case ItemID.GoldBar://19 Gold Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4e/Gold_Bar.png/revision/latest?cb=20200516213006&format=original";
				case ItemID.CopperBar://20 Copper Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f1/Copper_Bar.png/revision/latest?cb=20200516210020&format=original";
				case ItemID.SilverBar://21 Silver Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Silver_Bar.png/revision/latest?cb=20200516221816&format=original";
				case ItemID.IronBar://22 Iron Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6c/Iron_Bar.png/revision/latest?cb=20200516214300&format=original";
				case ItemID.Gel://23 Gel
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3f/Gel.png/revision/latest?cb=20200519215406&format=original";
				case ItemID.WoodenSword://24 Wooden Sword
					return "";
				case ItemID.WoodenDoor://25 Wooden Door
					return "";
				case ItemID.StoneWall://26 Stone Wall
					return "";
				case ItemID.Acorn://27 Acorn
					return "";
				case ItemID.LesserHealingPotion://28 Lesser Healing Potion
					return "";
				case ItemID.LifeCrystal://29 Life Crystal
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/05/Life_Crystal.png/revision/latest?cb=20200516214721&format=original";
				case ItemID.DirtWall://30 Dirt Wall
					return "";
				case ItemID.Bottle://31 Bottle
					return "";
				case ItemID.WoodenTable://32 Wooden Table
					return "";
				case ItemID.Furnace://33 Furnace
					return "";
				case ItemID.WoodenChair://34 Wooden Chair
					return "";
				case ItemID.IronAnvil://35 Iron Anvil
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c3/Iron_Anvil.png/revision/latest?cb=20200516214257&format=original";
				case ItemID.WorkBench://36 Work Bench
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d5/Any_Work_Bench.gif/revision/latest?cb=20200627022728&format=original";
				case ItemID.Goggles://37 Goggles
					return "";
				case ItemID.Lens://38 Lens
					return "";
				case ItemID.WoodenBow://39 Wooden Bow
					return "";
				case ItemID.WoodenArrow://40 Wooden Arrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/ff/Wooden_Arrow.png/revision/latest?cb=20200516223634&format=original";
				case ItemID.FlamingArrow://41 Flaming Arrow
					return "";
				case ItemID.Shuriken://42 Shuriken
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/ac/Shuriken.png/revision/latest?cb=20200516221751&format=original";
				case ItemID.SuspiciousLookingEye://43 Suspicious Looking Eye
					return "";
				case ItemID.DemonBow://44 Demon Bow
					return "";
				case ItemID.WarAxeoftheNight://45 War Axe of the Night
					return "";
				case ItemID.LightsBane://46 Light's Bane
					return "";
				case ItemID.UnholyArrow://47 Unholy Arrow
					return "";
				case ItemID.Chest://48 Chest
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b3/Chest.png/revision/latest?cb=20200516205229&format=original";
				case ItemID.BandofRegeneration://49 Band of Regeneration
					return "";
				case ItemID.MagicMirror://50 Magic Mirror
					return "";
				case ItemID.JestersArrow://51 Jester's Arrow
					return "";
				case ItemID.AngelStatue://52 Angel Statue
					return "";
				case ItemID.CloudinaBottle://53 Cloud in a Bottle
					return "";
				case ItemID.HermesBoots://54 Hermes Boots
					return "";
				case ItemID.EnchantedBoomerang://55 Enchanted Boomerang
					return "";
				case ItemID.DemoniteOre://56 Demonite Ore
					return "";
				case ItemID.DemoniteBar://57 Demonite Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/67/Demonite_Bar.png/revision/latest?cb=20200516211316&format=original";
				case ItemID.Heart://58 Heart
					return "";
				case ItemID.CorruptSeeds://59 Corrupt Seeds
					return "";
				case ItemID.VileMushroom://60 Vile Mushroom
					return "";
				case ItemID.EbonstoneBlock://61 Ebonstone Block
					return "";
				case ItemID.GrassSeeds://62 Grass Seeds
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ef/Grass_Seeds.png/revision/latest?cb=20200516213524&format=original";
				case ItemID.Sunflower://63 Sunflower
					return "";
				case ItemID.Vilethorn://64 Vilethorn
					return "";
				case ItemID.Starfury://65 Starfury
					return "";
				case ItemID.PurificationPowder://66 Purification Powder
					return "";
				case ItemID.VilePowder://67 Vile Powder
					return "";
				case ItemID.RottenChunk://68 Rotten Chunk
					return "";
				case ItemID.WormTooth://69 Worm Tooth
					return "";
				case ItemID.WormFood://70 Worm Food
					return "";
				case ItemID.CopperCoin://71 Copper Coin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/43/Copper_Coin.png/revision/latest?cb=20200516210038&format=original";
				case ItemID.SilverCoin://72 Silver Coin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4e/Silver_Coin.png/revision/latest?cb=20200516221827&format=original";
				case ItemID.GoldCoin://73 Gold Coin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1f/Gold_Coin.png/revision/latest?cb=20200516213029&format=original";
				case ItemID.PlatinumCoin://74 Platinum Coin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2c/Platinum_Coin.png/revision/latest?cb=20200516220643&format=original";
				case ItemID.FallenStar://75 Fallen Star
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9c/Fallen_Star.png/revision/latest?cb=20200516231331&format=original";
				case ItemID.CopperGreaves://76 Copper Greaves
					return "";
				case ItemID.IronGreaves://77 Iron Greaves
					return "";
				case ItemID.SilverGreaves://78 Silver Greaves
					return "";
				case ItemID.GoldGreaves://79 Gold Greaves
					return "";
				case ItemID.CopperChainmail://80 Copper Chainmail
					return "";
				case ItemID.IronChainmail://81 Iron Chainmail
					return "";
				case ItemID.SilverChainmail://82 Silver Chainmail
					return "";
				case ItemID.GoldChainmail://83 Gold Chainmail
					return "";
				case ItemID.GrapplingHook://84 Grappling Hook
					return "";
				case ItemID.Chain://85 Chain
					return "";
				case ItemID.ShadowScale://86 Shadow Scale
					return "";
				case ItemID.PiggyBank://87 Piggy Bank
					return "";
				case ItemID.MiningHelmet://88 Mining Helmet
					return "";
				case ItemID.CopperHelmet://89 Copper Helmet
					return "";
				case ItemID.IronHelmet://90 Iron Helmet
					return "";
				case ItemID.SilverHelmet://91 Silver Helmet
					return "";
				case ItemID.GoldHelmet://92 Gold Helmet
					return "";
				case ItemID.WoodWall://93 Wood Wall
					return "";
				case ItemID.WoodPlatform://94 Wood Platform
					return "";
				case ItemID.FlintlockPistol://95 Flintlock Pistol
					return "";
				case ItemID.Musket://96 Musket
					return "";
				case ItemID.MusketBall://97 Musket Ball
					return "";
				case ItemID.Minishark://98 Minishark
					return "";
				case ItemID.IronBow://99 Iron Bow
					return "";
				case ItemID.ShadowGreaves://100 Shadow Greaves
					return "";
				case ItemID.ShadowScalemail://101 Shadow Scalemail
					return "";
				case ItemID.ShadowHelmet://102 Shadow Helmet
					return "";
				case ItemID.NightmarePickaxe://103 Nightmare Pickaxe
					return "";
				case ItemID.TheBreaker://104 The Breaker
					return "";
				case ItemID.Candle://105 Candle
					return "";
				case ItemID.CopperChandelier://106 Copper Chandelier
					return "";
				case ItemID.SilverChandelier://107 Silver Chandelier
					return "";
				case ItemID.GoldChandelier://108 Gold Chandelier
					return "";
				case ItemID.ManaCrystal://109 Mana Crystal
					return "";
				case ItemID.LesserManaPotion://110 Lesser Mana Potion
					return "";
				case ItemID.BandofStarpower://111 Band of Starpower
					return "";
				case ItemID.FlowerofFire://112 Flower of Fire
					return "";
				case ItemID.MagicMissile://113 Magic Missile
					return "";
				case ItemID.DirtRod://114 Dirt Rod
					return "";
				case ItemID.ShadowOrb://115 Shadow Orb
					return "";
				case ItemID.Meteorite://116 Meteorite
					return "";
				case ItemID.MeteoriteBar://117 Meteorite Bar
					return "";
				case ItemID.Hook://118 Hook
					return "";
				case ItemID.Flamarang://119 Flamarang
					return "";
				case ItemID.MoltenFury://120 Molten Fury
					return "";
				case ItemID.FieryGreatsword://121 Volcano
					return "";
				case ItemID.MoltenPickaxe://122 Molten Pickaxe
					return "";
				case ItemID.MeteorHelmet://123 Meteor Helmet
					return "";
				case ItemID.MeteorSuit://124 Meteor Suit
					return "";
				case ItemID.MeteorLeggings://125 Meteor Leggings
					return "";
				case ItemID.BottledWater://126 Bottled Water
					return "";
				case ItemID.SpaceGun://127 Space Gun
					return "";
				case ItemID.RocketBoots://128 Rocket Boots
					return "";
				case ItemID.GrayBrick://129 Gray Brick
					return "";
				case ItemID.GrayBrickWall://130 Gray Brick Wall
					return "";
				case ItemID.RedBrick://131 Red Brick
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/be/Red_Brick.png/revision/latest?cb=20200516221049&format=original";
				case ItemID.RedBrickWall://132 Red Brick Wall
					return "";
				case ItemID.ClayBlock://133 Clay Block
					return "";
				case ItemID.BlueBrick://134 Blue Brick
					return "";
				case ItemID.BlueBrickWall://135 Blue Brick Wall
					return "";
				case ItemID.ChainLantern://136 Chain Lantern
					return "";
				case ItemID.GreenBrick://137 Green Brick
					return "";
				case ItemID.GreenBrickWall://138 Green Brick Wall
					return "";
				case ItemID.PinkBrick://139 Pink Brick
					return "";
				case ItemID.PinkBrickWall://140 Pink Brick Wall
					return "";
				case ItemID.GoldBrick://141 Gold Brick
					return "";
				case ItemID.GoldBrickWall://142 Gold Brick Wall
					return "";
				case ItemID.SilverBrick://143 Silver Brick
					return "";
				case ItemID.SilverBrickWall://144 Silver Brick Wall
					return "";
				case ItemID.CopperBrick://145 Copper Brick
					return "";
				case ItemID.CopperBrickWall://146 Copper Brick Wall
					return "";
				case ItemID.Spike://147 Spike
					return "";
				case ItemID.WaterCandle://148 Water Candle
					return "";
				case ItemID.Book://149 Book
					return "";
				case ItemID.Cobweb://150 Cobweb
					return "";
				case ItemID.NecroHelmet://151 Necro Helmet
					return "";
				case ItemID.NecroBreastplate://152 Necro Breastplate
					return "";
				case ItemID.NecroGreaves://153 Necro Greaves
					return "";
				case ItemID.Bone://154 Bone
					return "";
				case ItemID.Muramasa://155 Muramasa
					return "";
				case ItemID.CobaltShield://156 Cobalt Shield
					return "";
				case ItemID.AquaScepter://157 Aqua Scepter
					return "";
				case ItemID.LuckyHorseshoe://158 Lucky Horseshoe
					return "";
				case ItemID.ShinyRedBalloon://159 Shiny Red Balloon
					return "";
				case ItemID.Harpoon://160 Harpoon
					return "";
				case ItemID.SpikyBall://161 Spiky Ball
					return "";
				case ItemID.BallOHurt://162 Ball O' Hurt
					return "";
				case ItemID.BlueMoon://163 Blue Moon
					return "";
				case ItemID.Handgun://164 Handgun
					return "";
				case ItemID.WaterBolt://165 Water Bolt
					return "";
				case ItemID.Bomb://166 Bomb
					return "";
				case ItemID.Dynamite://167 Dynamite
					return "";
				case ItemID.Grenade://168 Grenade
					return "";
				case ItemID.SandBlock://169 Sand Block
					return "";
				case ItemID.Glass://170 Glass
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/15/Glass.png/revision/latest?cb=20200516212938&format=original";
				case ItemID.Sign://171 Sign
					return "";
				case ItemID.AshBlock://172 Ash Block
					return "";
				case ItemID.Obsidian://173 Obsidian
					return "";
				case ItemID.Hellstone://174 Hellstone
					return "";
				case ItemID.HellstoneBar://175 Hellstone Bar
					return "";
				case ItemID.MudBlock://176 Mud Block
					return "";
				case ItemID.Sapphire://177 Sapphire
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f7/Sapphire.png/revision/latest?cb=20200516221513&format=original";
				case ItemID.Ruby://178 Ruby
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a9/Ruby.png/revision/latest?cb=20200516221330&format=original";
				case ItemID.Emerald://179 Emerald
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6a/Emerald.png/revision/latest?cb=20200814173610&format=original";
				case ItemID.Topaz://180 Topaz
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a5/Topaz.png/revision/latest?cb=20200516223043&format=original";
				case ItemID.Amethyst://181 Amethyst
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2e/Amethyst.png/revision/latest?cb=20200516184617&format=original";
				case ItemID.Diamond://182 Diamond
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ea/Diamond.png/revision/latest?cb=20200821041639&format=original";
				case ItemID.GlowingMushroom://183 Glowing Mushroom
					return "";
				case ItemID.Star://184 Star
					return "";
				case ItemID.IvyWhip://185 Ivy Whip
					return "";
				case ItemID.BreathingReed://186 Breathing Reed
					return "";
				case ItemID.Flipper://187 Flipper
					return "";
				case ItemID.HealingPotion://188 Healing Potion
					return "";
				case ItemID.ManaPotion://189 Mana Potion
					return "";
				case ItemID.BladeofGrass://190 Blade of Grass
					return "";
				case ItemID.ThornChakram://191 Thorn Chakram
					return "";
				case ItemID.ObsidianBrick://192 Obsidian Brick
					return "";
				case ItemID.ObsidianSkull://193 Obsidian Skull
					return "";
				case ItemID.MushroomGrassSeeds://194 Mushroom Grass Seeds
					return "";
				case ItemID.JungleGrassSeeds://195 Jungle Grass Seeds
					return "";
				case ItemID.WoodenHammer://196 Wooden Hammer
					return "";
				case ItemID.StarCannon://197 Star Cannon
					return "";
				case ItemID.BluePhaseblade://198 Blue Phaseblade
					return "";
				case ItemID.RedPhaseblade://199 Red Phaseblade
					return "";
				case ItemID.GreenPhaseblade://200 Green Phaseblade
					return "";
				case ItemID.PurplePhaseblade://201 Purple Phaseblade
					return "";
				case ItemID.WhitePhaseblade://202 White Phaseblade
					return "";
				case ItemID.YellowPhaseblade://203 Yellow Phaseblade
					return "";
				case ItemID.MeteorHamaxe://204 Meteor Hamaxe
					return "";
				case ItemID.EmptyBucket://205 Empty Bucket
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/64/Empty_Bucket.png/revision/latest?cb=20200516211911&format=original";
				case ItemID.WaterBucket://206 Water Bucket
					return "";
				case ItemID.LavaBucket://207 Lava Bucket
					return "";
				case ItemID.JungleRose://208 Jungle Rose
					return "";
				case ItemID.Stinger://209 Stinger
					return "";
				case ItemID.Vine://210 Vine
					return "";
				case ItemID.FeralClaws://211 Feral Claws
					return "";
				case ItemID.AnkletoftheWind://212 Anklet of the Wind
					return "";
				case ItemID.StaffofRegrowth://213 Staff of Regrowth
					return "";
				case ItemID.HellstoneBrick://214 Hellstone Brick
					return "";
				case ItemID.WhoopieCushion://215 Whoopie Cushion
					return "";
				case ItemID.Shackle://216 Shackle
					return "";
				case ItemID.MoltenHamaxe://217 Molten Hamaxe
					return "";
				case ItemID.Flamelash://218 Flamelash
					return "";
				case ItemID.PhoenixBlaster://219 Phoenix Blaster
					return "";
				case ItemID.Sunfury://220 Sunfury
					return "";
				case ItemID.Hellforge://221 Hellforge
					return "";
				case ItemID.ClayPot://222 Clay Pot
					return "";
				case ItemID.NaturesGift://223 Nature's Gift
					return "";
				case ItemID.Bed://224 Bed
					return "";
				case ItemID.Silk://225 Silk
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/83/Silk.png/revision/latest?cb=20200820022839&format=original";
				case ItemID.LesserRestorationPotion://226 Restoration Potion
					return "";
				case ItemID.RestorationPotion://227 Restoration Potion
					return "";
				case ItemID.JungleHat://228 Jungle Hat
					return "";
				case ItemID.JungleShirt://229 Jungle Shirt
					return "";
				case ItemID.JunglePants://230 Jungle Pants
					return "";
				case ItemID.MoltenHelmet://231 Molten Helmet
					return "";
				case ItemID.MoltenBreastplate://232 Molten Breastplate
					return "";
				case ItemID.MoltenGreaves://233 Molten Greaves
					return "";
				case ItemID.MeteorShot://234 Meteor Shot
					return "";
				case ItemID.StickyBomb://235 Sticky Bomb
					return "";
				case ItemID.BlackLens://236 Black Lens
					return "";
				case ItemID.Sunglasses://237 Sunglasses
					return "";
				case ItemID.WizardHat://238 Wizard Hat
					return "";
				case ItemID.TopHat://239 Top Hat
					return "";
				case ItemID.TuxedoShirt://240 Tuxedo Shirt
					return "";
				case ItemID.TuxedoPants://241 Tuxedo Pants
					return "";
				case ItemID.SummerHat://242 Summer Hat
					return "";
				case ItemID.BunnyHood://243 Bunny Hood
					return "";
				case ItemID.PlumbersHat://244 Plumber's Hat
					return "";
				case ItemID.PlumbersShirt://245 Plumber's Shirt
					return "";
				case ItemID.PlumbersPants://246 Plumber's Pants
					return "";
				case ItemID.HerosHat://247 Hero's Hat
					return "";
				case ItemID.HerosShirt://248 Hero's Shirt
					return "";
				case ItemID.HerosPants://249 Hero's Pants
					return "";
				case ItemID.FishBowl://250 Fish Bowl
					return "";
				case ItemID.ArchaeologistsHat://251 Archaeologist's Hat
					return "";
				case ItemID.ArchaeologistsJacket://252 Archaeologist's Jacket
					return "";
				case ItemID.ArchaeologistsPants://253 Archaeologist's Pants
					return "";
				case ItemID.BlackThread://254 Black Thread
					return "";
				case ItemID.GreenThread://255 Green Thread
					return "";
				case ItemID.NinjaHood://256 Ninja Hood
					return "";
				case ItemID.NinjaShirt://257 Ninja Shirt
					return "";
				case ItemID.NinjaPants://258 Ninja Pants
					return "";
				case ItemID.Leather://259 Leather
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3d/Leather.png/revision/latest?cb=20200516214635&format=original";
				case ItemID.RedHat://260 Red Hat
					return "";
				case ItemID.Goldfish://261 Goldfish
					return "";
				case ItemID.Robe://262 Robe
					return "";
				case ItemID.RobotHat://263 Robot Hat
					return "";
				case ItemID.GoldCrown://264 Gold Crown
					return "";
				case ItemID.HellfireArrow://265 Hellfire Arrow
					return "";
				case ItemID.Sandgun://266 Sandgun
					return "";
				case ItemID.GuideVoodooDoll://267 Guide Voodoo Doll
					return "";
				case ItemID.DivingHelmet://268 Diving Helmet
					return "";
				case ItemID.FamiliarShirt://269 Familiar Shirt
					return "";
				case ItemID.FamiliarPants://270 Familiar Pants
					return "";
				case ItemID.FamiliarWig://271 Familiar Wig
					return "";
				case ItemID.DemonScythe://272 Demon Scythe
					return "";
				case ItemID.NightsEdge://273 Night's Edge
					return "";
				case ItemID.DarkLance://274 Dark Lance
					return "";
				case ItemID.Coral://275 Coral
					return "";
				case ItemID.Cactus://276 Cactus
					return "";
				case ItemID.Trident://277 Trident
					return "";
				case ItemID.SilverBullet://278 Silver Bullet
					return "";
				case ItemID.ThrowingKnife://279 Throwing Knife
					return "";
				case ItemID.Spear://280 Spear
					return "";
				case ItemID.Blowpipe://281 Blowpipe
					return "";
				case ItemID.Glowstick://282 Glowstick
					return "";
				case ItemID.Seed://283 Seed
					return "";
				case ItemID.WoodenBoomerang://284 Wooden Boomerang
					return "";
				case ItemID.Aglet://285 Aglet
					return "";
				case ItemID.StickyGlowstick://286 Sticky Glowstick
					return "";
				case ItemID.PoisonedKnife://287 Poisoned Knife
					return "";
				case ItemID.ObsidianSkinPotion://288 Obsidian Skin Potion
					return "";
				case ItemID.RegenerationPotion://289 Regeneration Potion
					return "";
				case ItemID.SwiftnessPotion://290 Swiftness Potion
					return "";
				case ItemID.GillsPotion://291 Gills Potion
					return "";
				case ItemID.IronskinPotion://292 Ironskin Potion
					return "";
				case ItemID.ManaRegenerationPotion://293 Mana Regeneration Potion
					return "";
				case ItemID.MagicPowerPotion://294 Magic Power Potion
					return "";
				case ItemID.FeatherfallPotion://295 Featherfall Potion
					return "";
				case ItemID.SpelunkerPotion://296 Spelunker Potion
					return "";
				case ItemID.InvisibilityPotion://297 Invisibility Potion
					return "";
				case ItemID.ShinePotion://298 Shine Potion
					return "";
				case ItemID.NightOwlPotion://299 Night Owl Potion
					return "";
				case ItemID.BattlePotion://300 Battle Potion
					return "";
				case ItemID.ThornsPotion://301 Thorns Potion
					return "";
				case ItemID.WaterWalkingPotion://302 Water Walking Potion
					return "";
				case ItemID.ArcheryPotion://303 Archery Potion
					return "";
				case ItemID.HunterPotion://304 Hunter Potion
					return "";
				case ItemID.GravitationPotion://305 Gravitation Potion
					return "";
				case ItemID.GoldChest://306 Gold Chest
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d9/Gold_Chest.png/revision/latest?cb=20200516213027&format=original";
				case ItemID.DaybloomSeeds://307 Daybloom Seeds
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/41/Daybloom_Seeds.png/revision/latest?cb=20200516211139&format=original";
				case ItemID.MoonglowSeeds://308 Moonglow Seeds
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/46/Moonglow_Seeds.png/revision/latest?cb=20200516215434&format=original";
				case ItemID.BlinkrootSeeds://309 Blinkroot Seeds
					return "";
				case ItemID.DeathweedSeeds://310 Deathweed Seeds
					return "";
				case ItemID.WaterleafSeeds://311 Waterleaf Seeds
					return "";
				case ItemID.FireblossomSeeds://312 Fireblossom Seeds
					return "";
				case ItemID.Daybloom://313 Daybloom
					return "";
				case ItemID.Moonglow://314 Moonglow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5b/Moonglow.png/revision/latest?cb=20200516215435&format=original";
				case ItemID.Blinkroot://315 Blinkroot
					return "";
				case ItemID.Deathweed://316 Deathweed
					return "";
				case ItemID.Waterleaf://317 Waterleaf
					return "";
				case ItemID.Fireblossom://318 Fireblossom
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/92/Fireblossom.png/revision/latest?cb=20200516212309&format=original";
				case ItemID.SharkFin://319 Shark Fin
					return "";
				case ItemID.Feather://320 Feather
					return "";
				case ItemID.Tombstone://321 Tombstone
					return "";
				case ItemID.MimeMask://322 Mime Mask
					return "";
				case ItemID.AntlionMandible://323 Antlion Mandible
					return "";
				case ItemID.IllegalGunParts://324 Illegal Gun Parts
					return "";
				case ItemID.TheDoctorsShirt://325 The Doctor's Shirt
					return "";
				case ItemID.TheDoctorsPants://326 The Doctor's Pants
					return "";
				case ItemID.GoldenKey://327 Golden Key
					return "";
				case ItemID.ShadowChest://328 Shadow Chest
					return "";
				case ItemID.ShadowKey://329 Shadow Key
					return "";
				case ItemID.ObsidianBrickWall://330 Obsidian Brick Wall
					return "";
				case ItemID.JungleSpores://331 Jungle Spores
					return "";
				case ItemID.Loom://332 Loom
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3b/Loom.png/revision/latest?cb=20200516214936&format=original";
				case ItemID.Piano://333 Piano
					return "";
				case ItemID.Dresser://334 Dresser
					return "";
				case ItemID.Bench://335 Bench
					return "";
				case ItemID.Bathtub://336 Bathtub
					return "";
				case ItemID.RedBanner://337 Red Banner
					return "";
				case ItemID.GreenBanner://338 Green Banner
					return "";
				case ItemID.BlueBanner://339 Blue Banner
					return "";
				case ItemID.YellowBanner://340 Yellow Banner
					return "";
				case ItemID.LampPost://341 Lamp Post
					return "";
				case ItemID.TikiTorch://342 Tiki Torch
					return "";
				case ItemID.Barrel://343 Barrel
					return "";
				case ItemID.ChineseLantern://344 Chinese Lantern
					return "";
				case ItemID.CookingPot://345 Cooking Pot
					return "";
				case ItemID.Safe://346 Safe
					return "";
				case ItemID.SkullLantern://347 Skull Lantern
					return "";
				case ItemID.TrashCan://348 Trash Can
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/28/Trash_Can.png/revision/latest?cb=20200516223105&format=original";
				case ItemID.Candelabra://349 Candelabra
					return "";
				case ItemID.PinkVase://350 Pink Vase
					return "";
				case ItemID.Mug://351 Mug
					return "";
				case ItemID.Keg://352 Keg
					return "";
				case ItemID.Ale://353 Ale
					return "";
				case ItemID.Bookcase://354 Bookcase
					return "";
				case ItemID.Throne://355 Throne
					return "";
				case ItemID.Bowl://356 Bowl
					return "";
				case ItemID.BowlofSoup://357 Bowl of Soup
					return "";
				case ItemID.Toilet://358 Toilet
					return "";
				case ItemID.GrandfatherClock://359 Grandfather Clock
					return "";
				case ItemID.ArmorStatue://360 Armor Statue
					return "";
				case ItemID.GoblinBattleStandard://361 Goblin Battle Standard
					return "";
				case ItemID.TatteredCloth://362 Tattered Cloth
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/52/Tattered_Cloth.png/revision/latest?cb=20200516222738&format=original";
				case ItemID.Sawmill://363 Sawmill
					return "";
				case ItemID.CobaltOre://364 Cobalt Ore
					return "";
				case ItemID.MythrilOre://365 Mythril Ore
					return "";
				case ItemID.AdamantiteOre://366 Adamantite Ore
					return "";
				case ItemID.Pwnhammer://367 Pwnhammer
					return "";
				case ItemID.Excalibur://368 Excalibur
					return "";
				case ItemID.HallowedSeeds://369 Hallowed Seeds
					return "";
				case ItemID.EbonsandBlock://370 Ebonsand Block
					return "";
				case ItemID.CobaltHat://371 Cobalt Hat
					return "";
				case ItemID.CobaltHelmet://372 Cobalt Helmet
					return "";
				case ItemID.CobaltMask://373 Cobalt Mask
					return "";
				case ItemID.CobaltBreastplate://374 Cobalt Breastplate
					return "";
				case ItemID.CobaltLeggings://375 Cobalt Leggings
					return "";
				case ItemID.MythrilHood://376 Mythril Hood
					return "";
				case ItemID.MythrilHelmet://377 Mythril Helmet
					return "";
				case ItemID.MythrilHat://378 Mythril Hat
					return "";
				case ItemID.MythrilChainmail://379 Mythril Chainmail
					return "";
				case ItemID.MythrilGreaves://380 Mythril Greaves
					return "";
				case ItemID.CobaltBar://381 Cobalt Bar
					return "";
				case ItemID.MythrilBar://382 Mythril Bar
					return "";
				case ItemID.CobaltChainsaw://383 Cobalt Chainsaw
					return "";
				case ItemID.MythrilChainsaw://384 Mythril Chainsaw
					return "";
				case ItemID.CobaltDrill://385 Cobalt Drill
					return "";
				case ItemID.MythrilDrill://386 Mythril Drill
					return "";
				case ItemID.AdamantiteChainsaw://387 Adamantite Chainsaw
					return "";
				case ItemID.AdamantiteDrill://388 Adamantite Drill
					return "";
				case ItemID.DaoofPow://389 Dao of Pow
					return "";
				case ItemID.MythrilHalberd://390 Mythril Halberd
					return "";
				case ItemID.AdamantiteBar://391 Adamantite Bar
					return "";
				case ItemID.GlassWall://392 Glass Wall
					return "";
				case ItemID.Compass://393 Compass
					return "";
				case ItemID.DivingGear://394 Diving Gear
					return "";
				case ItemID.GPS://395 GPS
					return "";
				case ItemID.ObsidianHorseshoe://396 Obsidian Horseshoe
					return "";
				case ItemID.ObsidianShield://397 Obsidian Shield
					return "";
				case ItemID.TinkerersWorkshop://398 Tinkerer's Workshop
					return "";
				case ItemID.CloudinaBalloon://399 Cloud in a Balloon
					return "";
				case ItemID.AdamantiteHeadgear://400 Adamantite Headgear
					return "";
				case ItemID.AdamantiteHelmet://401 Adamantite Helmet
					return "";
				case ItemID.AdamantiteMask://402 Adamantite Mask
					return "";
				case ItemID.AdamantiteBreastplate://403 Adamantite Breastplate
					return "";
				case ItemID.AdamantiteLeggings://404 Adamantite Leggings
					return "";
				case ItemID.SpectreBoots://405 Spectre Boots
					return "";
				case ItemID.AdamantiteGlaive://406 Adamantite Glaive
					return "";
				case ItemID.Toolbelt://407 Toolbelt
					return "";
				case ItemID.PearlsandBlock://408 Pearlsand Block
					return "";
				case ItemID.PearlstoneBlock://409 Pearlstone Block
					return "";
				case ItemID.MiningShirt://410 Mining Shirt
					return "";
				case ItemID.MiningPants://411 Mining Pants
					return "";
				case ItemID.PearlstoneBrick://412 Pearlstone Brick
					return "";
				case ItemID.IridescentBrick://413 Iridescent Brick
					return "";
				case ItemID.MudstoneBlock://414 Mudstone Brick
					return "";
				case ItemID.CobaltBrick://415 Cobalt Brick
					return "";
				case ItemID.MythrilBrick://416 Mythril Brick
					return "";
				case ItemID.PearlstoneBrickWall://417 Pearlstone Brick Wall
					return "";
				case ItemID.IridescentBrickWall://418 Iridescent Brick Wall
					return "";
				case ItemID.MudstoneBrickWall://419 Mudstone Brick Wall
					return "";
				case ItemID.CobaltBrickWall://420 Cobalt Brick Wall
					return "";
				case ItemID.MythrilBrickWall://421 Mythril Brick Wall
					return "";
				case ItemID.HolyWater://422 Holy Water
					return "";
				case ItemID.UnholyWater://423 Unholy Water
					return "";
				case ItemID.SiltBlock://424 Silt Block
					return "";
				case ItemID.FairyBell://425 Fairy Bell
					return "";
				case ItemID.BreakerBlade://426 Breaker Blade
					return "";
				case ItemID.BlueTorch://427 Blue Torch
					return "";
				case ItemID.RedTorch://428 Red Torch
					return "";
				case ItemID.GreenTorch://429 Green Torch
					return "";
				case ItemID.PurpleTorch://430 Purple Torch
					return "";
				case ItemID.WhiteTorch://431 White Torch
					return "";
				case ItemID.YellowTorch://432 Yellow Torch
					return "";
				case ItemID.DemonTorch://433 Demon Torch
					return "";
				case ItemID.ClockworkAssaultRifle://434 Clockwork Assault Rifle
					return "";
				case ItemID.CobaltRepeater://435 Cobalt Repeater
					return "";
				case ItemID.MythrilRepeater://436 Mythril Repeater
					return "";
				case ItemID.DualHook://437 Dual Hook
					return "";
				case ItemID.StarStatue://438 Star Statue
					return "";
				case ItemID.SwordStatue://439 Sword Statue
					return "";
				case ItemID.SlimeStatue://440 Slime Statue
					return "";
				case ItemID.GoblinStatue://441 Goblin Statue
					return "";
				case ItemID.ShieldStatue://442 Shield Statue
					return "";
				case ItemID.BatStatue://443 Bat Statue
					return "";
				case ItemID.FishStatue://444 Fish Statue
					return "";
				case ItemID.BunnyStatue://445 Bunny Statue
					return "";
				case ItemID.SkeletonStatue://446 Skeleton Statue
					return "";
				case ItemID.ReaperStatue://447 Reaper Statue
					return "";
				case ItemID.WomanStatue://448 Woman Statue
					return "";
				case ItemID.ImpStatue://449 Imp Statue
					return "";
				case ItemID.GargoyleStatue://450 Gargoyle Statue
					return "";
				case ItemID.GloomStatue://451 Gloom Statue
					return "";
				case ItemID.HornetStatue://452 Hornet Statue
					return "";
				case ItemID.BombStatue://453 Bomb Statue
					return "";
				case ItemID.CrabStatue://454 Crab Statue
					return "";
				case ItemID.HammerStatue://455 Hammer Statue
					return "";
				case ItemID.PotionStatue://456 Potion Statue
					return "";
				case ItemID.SpearStatue://457 Spear Statue
					return "";
				case ItemID.CrossStatue://458 Cross Statue
					return "";
				case ItemID.JellyfishStatue://459 Jellyfish Statue
					return "";
				case ItemID.BowStatue://460 Bow Statue
					return "";
				case ItemID.BoomerangStatue://461 Boomerang Statue
					return "";
				case ItemID.BootStatue://462 Boot Statue
					return "";
				case ItemID.ChestStatue://463 Chest Statue
					return "";
				case ItemID.BirdStatue://464 Bird Statue
					return "";
				case ItemID.AxeStatue://465 Axe Statue
					return "";
				case ItemID.CorruptStatue://466 Corrupt Statue
					return "";
				case ItemID.TreeStatue://467 Tree Statue
					return "";
				case ItemID.AnvilStatue://468 Anvil Statue
					return "";
				case ItemID.PickaxeStatue://469 Pickaxe Statue
					return "";
				case ItemID.MushroomStatue://470 Mushroom Statue
					return "";
				case ItemID.EyeballStatue://471 Eyeball Statue
					return "";
				case ItemID.PillarStatue://472 Pillar Statue
					return "";
				case ItemID.HeartStatue://473 Heart Statue
					return "";
				case ItemID.PotStatue://474 Pot Statue
					return "";
				case ItemID.SunflowerStatue://475 Sunflower Statue
					return "";
				case ItemID.KingStatue://476 King Statue
					return "";
				case ItemID.QueenStatue://477 Queen Statue
					return "";
				case ItemID.PiranhaStatue://478 Piranha Statue
					return "";
				case ItemID.PlankedWall://479 Planked Wall
					return "";
				case ItemID.WoodenBeam://480 Wooden Beam
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3b/Wooden_Beam.png/revision/latest?cb=20200516223636&format=original";
				case ItemID.AdamantiteRepeater://481 Adamantite Repeater
					return "";
				case ItemID.AdamantiteSword://482 Adamantite Sword
					return "";
				case ItemID.CobaltSword://483 Cobalt Sword
					return "";
				case ItemID.MythrilSword://484 Mythril Sword
					return "";
				case ItemID.MoonCharm://485 Moon Charm
					return "";
				case ItemID.Ruler://486 Ruler
					return "";
				case ItemID.CrystalBall://487 Crystal Ball
					return "";
				case ItemID.DiscoBall://488 Disco Ball
					return "";
				case ItemID.SorcererEmblem://489 Sorcerer Emblem
					return "";
				case ItemID.WarriorEmblem://490 Warrior Emblem
					return "";
				case ItemID.RangerEmblem://491 Ranger Emblem
					return "";
				case ItemID.DemonWings://492 Demon Wings
					return "";
				case ItemID.AngelWings://493 Angel Wings
					return "";
				case ItemID.MagicalHarp://494 Magical Harp
					return "";
				case ItemID.RainbowRod://495 Rainbow Rod
					return "";
				case ItemID.IceRod://496 Ice Rod
					return "";
				case ItemID.NeptunesShell://497 Neptune's Shell
					return "";
				case ItemID.Mannequin://498 Mannequin
					return "";
				case ItemID.GreaterHealingPotion://499 Greater Healing Potion
					return "";
				case ItemID.GreaterManaPotion://500 Greater Mana Potion
					return "";
				case ItemID.PixieDust://501 Pixie Dust
					return "";
				case ItemID.CrystalShard://502 Crystal Shard
					return "";
				case ItemID.ClownHat://503 Clown Hat
					return "";
				case ItemID.ClownShirt://504 Clown Shirt
					return "";
				case ItemID.ClownPants://505 Clown Pants
					return "";
				case ItemID.Flamethrower://506 Flamethrower
					return "";
				case ItemID.Bell://507 Bell
					return "";
				case ItemID.Harp://508 Harp
					return "";
				case ItemID.Wrench://509 Red Wrench
					return "";
				case ItemID.WireCutter://510 Wire Cutter
					return "";
				case ItemID.ActiveStoneBlock://511 Active Stone Block
					return "";
				case ItemID.InactiveStoneBlock://512 Inactive Stone Block
					return "";
				case ItemID.Lever://513 Lever
					return "";
				case ItemID.LaserRifle://514 Laser Rifle
					return "";
				case ItemID.CrystalBullet://515 Crystal Bullet
					return "";
				case ItemID.HolyArrow://516 Holy Arrow
					return "";
				case ItemID.MagicDagger://517 Magic Dagger
					return "";
				case ItemID.CrystalStorm://518 Crystal Storm
					return "";
				case ItemID.CursedFlames://519 Cursed Flames
					return "";
				case ItemID.SoulofLight://520 Soul of Light
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3d/Soul_of_Light.gif/revision/latest?cb=20171118022839&format=original";
				case ItemID.SoulofNight://521 Soul of Night
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/02/Soul_of_Night.gif/revision/latest?cb=20171118022841&format=original";
				case ItemID.CursedFlame://522 Cursed Flame
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/19/Cursed_Flame.png/revision/latest?cb=20200516210828&format=original";
				case ItemID.CursedTorch://523 Cursed Torch
					return "";
				case ItemID.AdamantiteForge://524 Adamantite Forge
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/78/Adamantite_Forge.png/revision/latest?cb=20200516182709&format=original";
				case ItemID.MythrilAnvil://525 Mythril Anvil
					return "";
				case ItemID.UnicornHorn://526 Unicorn Horn
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/67/Unicorn_Horn.png/revision/latest?cb=20200516223250&format=original";
				case ItemID.DarkShard://527 Dark Shard
					return "";
				case ItemID.LightShard://528 Light Shard
					return "";
				case ItemID.RedPressurePlate://529 Red Pressure Plate
					return "";
				case ItemID.Wire://530 Wire
					return "";
				case ItemID.SpellTome://531 Spell Tome
					return "";
				case ItemID.StarCloak://532 Star Cloak
					return "";
				case ItemID.Megashark://533 Megashark
					return "";
				case ItemID.Shotgun://534 Shotgun
					return "";
				case ItemID.PhilosophersStone://535 Philosopher's Stone
					return "";
				case ItemID.TitanGlove://536 Titan Glove
					return "";
				case ItemID.CobaltNaginata://537 Cobalt Naginata
					return "";
				case ItemID.Switch://538 Switch
					return "";
				case ItemID.DartTrap://539 Dart Trap
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bc/Dart_Trap.png/revision/latest?cb=20200516211137&format=original";
				case ItemID.Boulder://540 Boulder
					return "";
				case ItemID.GreenPressurePlate://541 Green Pressure Plate
					return "";
				case ItemID.GrayPressurePlate://542 Gray Pressure Plate
					return "";
				case ItemID.BrownPressurePlate://543 Brown Pressure Plate
					return "";
				case ItemID.MechanicalEye://544 Mechanical Eye
					return "";
				case ItemID.CursedArrow://545 Cursed Arrow
					return "";
				case ItemID.CursedBullet://546 Cursed Bullet
					return "";
				case ItemID.SoulofFright://547 Soul of Fright
					return "";
				case ItemID.SoulofMight://548 Soul of Might
					return "";
				case ItemID.SoulofSight://549 Soul of Sight
					return "";
				case ItemID.Gungnir://550 Gungnir
					return "";
				case ItemID.HallowedPlateMail://551 Hallowed Plate Mail
					return "";
				case ItemID.HallowedGreaves://552 Hallowed Greaves
					return "";
				case ItemID.HallowedHelmet://553 Hallowed Helmet
					return "";
				case ItemID.CrossNecklace://554 Cross Necklace
					return "";
				case ItemID.ManaFlower://555 Mana Flower
					return "";
				case ItemID.MechanicalWorm://556 Mechanical Worm
					return "";
				case ItemID.MechanicalSkull://557 Mechanical Skull
					return "";
				case ItemID.HallowedHeadgear://558 Hallowed Headgear
					return "";
				case ItemID.HallowedMask://559 Hallowed Mask
					return "";
				case ItemID.SlimeCrown://560 Slime Crown
					return "";
				case ItemID.LightDisc://561 Light Disc
					return "";
				case ItemID.MusicBoxOverworldDay://562 Music Box (Overworld Day)
					return "";
				case ItemID.MusicBoxEerie://563 Music Box (Eerie)
					return "";
				case ItemID.MusicBoxNight://564 Music Box (Night)
					return "";
				case ItemID.MusicBoxTitle://565 Music Box (Title)
					return "";
				case ItemID.MusicBoxUnderground://566 Music Box (Underground)
					return "";
				case ItemID.MusicBoxBoss1://567 Music Box (Boss 1)
					return "";
				case ItemID.MusicBoxJungle://568 Music Box (Jungle)
					return "";
				case ItemID.MusicBoxCorruption://569 Music Box (Corruption)
					return "";
				case ItemID.MusicBoxUndergroundCorruption://570 Music Box (Underground Corruption)
					return "";
				case ItemID.MusicBoxTheHallow://571 Music Box (The Hallow)
					return "";
				case ItemID.MusicBoxBoss2://572 Music Box (Boss 2)
					return "";
				case ItemID.MusicBoxUndergroundHallow://573 Music Box (Underground Hallow)
					return "";
				case ItemID.MusicBoxBoss3://574 Music Box (Boss 3)
					return "";
				case ItemID.SoulofFlight://575 Soul of Flight
					return "";
				case ItemID.MusicBox://576 Music Box
					return "";
				case ItemID.DemoniteBrick://577 Demonite Brick
					return "";
				case ItemID.HallowedRepeater://578 Hallowed Repeater
					return "";
				case ItemID.Drax://579 Drax
					return "";
				case ItemID.Explosives://580 Explosives
					return "";
				case ItemID.InletPump://581 Inlet Pump
					return "";
				case ItemID.OutletPump://582 Outlet Pump
					return "";
				case ItemID.Timer1Second://583 1 Second Timer
					return "";
				case ItemID.Timer3Second://584 3 Second Timer
					return "";
				case ItemID.Timer5Second://585 5 Second Timer
					return "";
				case ItemID.CandyCaneBlock://586 Candy Cane Block
					return "";
				case ItemID.CandyCaneWall://587 Candy Cane Wall
					return "";
				case ItemID.SantaHat://588 Santa Hat
					return "";
				case ItemID.SantaShirt://589 Santa Shirt
					return "";
				case ItemID.SantaPants://590 Santa Pants
					return "";
				case ItemID.GreenCandyCaneBlock://591 Green Candy Cane Block
					return "";
				case ItemID.GreenCandyCaneWall://592 Green Candy Cane Wall
					return "";
				case ItemID.SnowBlock://593 Snow Block
					return "";
				case ItemID.SnowBrick://594 Snow Brick
					return "";
				case ItemID.SnowBrickWall://595 Snow Brick Wall
					return "";
				case ItemID.BlueLight://596 Blue Light
					return "";
				case ItemID.RedLight://597 Red Light
					return "";
				case ItemID.GreenLight://598 Green Light
					return "";
				case ItemID.BluePresent://599 Blue Present
					return "";
				case ItemID.GreenPresent://600 Green Present
					return "";
				case ItemID.YellowPresent://601 Yellow Present
					return "";
				case ItemID.SnowGlobe://602 Snow Globe
					return "";
				case ItemID.Carrot://603 Carrot
					return "";
				case ItemID.AdamantiteBeam://604 Adamantite Beam
					return "";
				case ItemID.AdamantiteBeamWall://605 Adamantite Beam Wall
					return "";
				case ItemID.DemoniteBrickWall://606 Demonite Brick Wall
					return "";
				case ItemID.SandstoneBrick://607 Sandstone Brick
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f6/Sandstone_Brick.png/revision/latest?cb=20200516221415&format=original";
				case ItemID.SandstoneBrickWall://608 Sandstone Brick Wall
					return "";
				case ItemID.EbonstoneBrick://609 Ebonstone Brick
					return "";
				case ItemID.EbonstoneBrickWall://610 Ebonstone Brick Wall
					return "";
				case ItemID.RedStucco://611 Red Stucco
					return "";
				case ItemID.YellowStucco://612 Yellow Stucco
					return "";
				case ItemID.GreenStucco://613 Green Stucco
					return "";
				case ItemID.GrayStucco://614 Gray Stucco
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/72/Gray_Stucco.png/revision/latest?cb=20200516213547&format=original";
				case ItemID.RedStuccoWall://615 Red Stucco Wall
					return "";
				case ItemID.YellowStuccoWall://616 Yellow Stucco Wall
					return "";
				case ItemID.GreenStuccoWall://617 Green Stucco Wall
					return "";
				case ItemID.GrayStuccoWall://618 Gray Stucco Wall
					return "";
				case ItemID.Ebonwood://619 Ebonwood
					return "";
				case ItemID.RichMahogany://620 Rich Mahogany
					return "";
				case ItemID.Pearlwood://621 Pearlwood
					return "";
				case ItemID.EbonwoodWall://622 Ebonwood Wall
					return "";
				case ItemID.RichMahoganyWall://623 Rich Mahogany Wall
					return "";
				case ItemID.PearlwoodWall://624 Pearlwood Wall
					return "";
				case ItemID.EbonwoodChest://625 Ebonwood Chest
					return "";
				case ItemID.RichMahoganyChest://626 Rich Mahogany Chest
					return "";
				case ItemID.PearlwoodChest://627 Pearlwood Chest
					return "";
				case ItemID.EbonwoodChair://628 Ebonwood Chair
					return "";
				case ItemID.RichMahoganyChair://629 Rich Mahogany Chair
					return "";
				case ItemID.PearlwoodChair://630 Pearlwood Chair
					return "";
				case ItemID.EbonwoodPlatform://631 Ebonwood Platform
					return "";
				case ItemID.RichMahoganyPlatform://632 Rich Mahogany Platform
					return "";
				case ItemID.PearlwoodPlatform://633 Pearlwood Platform
					return "";
				case ItemID.BonePlatform://634 Bone Platform
					return "";
				case ItemID.EbonwoodWorkBench://635 Ebonwood Work Bench
					return "";
				case ItemID.RichMahoganyWorkBench://636 Rich Mahogany Work Bench
					return "";
				case ItemID.PearlwoodWorkBench://637 Pearlwood Work Bench
					return "";
				case ItemID.EbonwoodTable://638 Ebonwood Table
					return "";
				case ItemID.RichMahoganyTable://639 Rich Mahogany Table
					return "";
				case ItemID.PearlwoodTable://640 Pearlwood Table
					return "";
				case ItemID.EbonwoodPiano://641 Ebonwood Piano
					return "";
				case ItemID.RichMahoganyPiano://642 Rich Mahogany Piano
					return "";
				case ItemID.PearlwoodPiano://643 Pearlwood Piano
					return "";
				case ItemID.EbonwoodBed://644 Ebonwood Bed
					return "";
				case ItemID.RichMahoganyBed://645 Rich Mahogany Bed
					return "";
				case ItemID.PearlwoodBed://646 Pearlwood Bed
					return "";
				case ItemID.EbonwoodDresser://647 Ebonwood Dresser
					return "";
				case ItemID.RichMahoganyDresser://648 Rich Mahogany Dresser
					return "";
				case ItemID.PearlwoodDresser://649 Pearlwood Dresser
					return "";
				case ItemID.EbonwoodDoor://650 Ebonwood Door
					return "";
				case ItemID.RichMahoganyDoor://651 Rich Mahogany Door
					return "";
				case ItemID.PearlwoodDoor://652 Pearlwood Door
					return "";
				case ItemID.EbonwoodSword://653 Ebonwood Sword
					return "";
				case ItemID.EbonwoodHammer://654 Ebonwood Hammer
					return "";
				case ItemID.EbonwoodBow://655 Ebonwood Bow
					return "";
				case ItemID.RichMahoganySword://656 Rich Mahogany Sword
					return "";
				case ItemID.RichMahoganyHammer://657 Rich Mahogany Hammer
					return "";
				case ItemID.RichMahoganyBow://658 Rich Mahogany Bow
					return "";
				case ItemID.PearlwoodSword://659 Pearlwood Sword
					return "";
				case ItemID.PearlwoodHammer://660 Pearlwood Hammer
					return "";
				case ItemID.PearlwoodBow://661 Pearlwood Bow
					return "";
				case ItemID.RainbowBrick://662 Rainbow Brick
					return "";
				case ItemID.RainbowBrickWall://663 Rainbow Brick Wall
					return "";
				case ItemID.IceBlock://664 Ice Block
					return "";
				case ItemID.RedsWings://665 Red's Wings
					return "";
				case ItemID.RedsHelmet://666 Red's Helmet
					return "";
				case ItemID.RedsBreastplate://667 Red's Breastplate
					return "";
				case ItemID.RedsLeggings://668 Red's Leggings
					return "";
				case ItemID.Fish://669 Fish
					return "";
				case ItemID.IceBoomerang://670 Ice Boomerang
					return "";
				case ItemID.Keybrand://671 Keybrand
					return "";
				case ItemID.Cutlass://672 Cutlass
					return "";
				case ItemID.BorealWoodWorkBench://673 Boreal Wood Work Bench
					return "";
				case ItemID.TrueExcalibur://674 True Excalibur
					return "";
				case ItemID.TrueNightsEdge://675 True Night's Edge
					return "";
				case ItemID.Frostbrand://676 Frostbrand
					return "";
				case ItemID.BorealWoodTable://677 Boreal Wood Table
					return "";
				case ItemID.RedPotion://678 Red Potion
					return "";
				case ItemID.TacticalShotgun://679 Tactical Shotgun
					return "";
				case ItemID.IvyChest://680 Ivy Chest
					return "";
				case ItemID.IceChest://681 Frozen Chest
					return "";
				case ItemID.Marrow://682 Marrow
					return "";
				case ItemID.UnholyTrident://683 Unholy Trident
					return "";
				case ItemID.FrostHelmet://684 Frost Helmet
					return "";
				case ItemID.FrostBreastplate://685 Frost Breastplate
					return "";
				case ItemID.FrostLeggings://686 Frost Leggings
					return "";
				case ItemID.TinHelmet://687 Tin Helmet
					return "";
				case ItemID.TinChainmail://688 Tin Chainmail
					return "";
				case ItemID.TinGreaves://689 Tin Greaves
					return "";
				case ItemID.LeadHelmet://690 Lead Helmet
					return "";
				case ItemID.LeadChainmail://691 Lead Chainmail
					return "";
				case ItemID.LeadGreaves://692 Lead Greaves
					return "";
				case ItemID.TungstenHelmet://693 Tungsten Helmet
					return "";
				case ItemID.TungstenChainmail://694 Tungsten Chainmail
					return "";
				case ItemID.TungstenGreaves://695 Tungsten Greaves
					return "";
				case ItemID.PlatinumHelmet://696 Platinum Helmet
					return "";
				case ItemID.PlatinumChainmail://697 Platinum Chainmail
					return "";
				case ItemID.PlatinumGreaves://698 Platinum Greaves
					return "";
				case ItemID.TinOre://699 Tin Ore
					return "";
				case ItemID.LeadOre://700 Lead Ore
					return "";
				case ItemID.TungstenOre://701 Tungsten Ore
					return "";
				case ItemID.PlatinumOre://702 Platinum Ore
					return "";
				case ItemID.TinBar://703 Tin Bar
					return "";
				case ItemID.LeadBar://704 Lead Bar
					return "";
				case ItemID.TungstenBar://705 Tungsten Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f3/Tungsten_Bar.png/revision/latest?cb=20200516223143&format=original";
				case ItemID.PlatinumBar://706 Platinum Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d2/Platinum_Bar.png/revision/latest?cb=20200516220632&format=original";
				case ItemID.TinWatch://707 Tin Watch
					return "";
				case ItemID.TungstenWatch://708 Tungsten Watch
					return "";
				case ItemID.PlatinumWatch://709 Platinum Watch
					return "";
				case ItemID.TinChandelier://710 Tin Chandelier
					return "";
				case ItemID.TungstenChandelier://711 Tungsten Chandelier
					return "";
				case ItemID.PlatinumChandelier://712 Platinum Chandelier
					return "";
				case ItemID.PlatinumCandle://713 Platinum Candle
					return "";
				case ItemID.PlatinumCandelabra://714 Platinum Candelabra
					return "";
				case ItemID.PlatinumCrown://715 Platinum Crown
					return "";
				case ItemID.LeadAnvil://716 Lead Anvil
					return "";
				case ItemID.TinBrick://717 Tin Brick
					return "";
				case ItemID.TungstenBrick://718 Tungsten Brick
					return "";
				case ItemID.PlatinumBrick://719 Platinum Brick
					return "";
				case ItemID.TinBrickWall://720 Tin Brick Wall
					return "";
				case ItemID.TungstenBrickWall://721 Tungsten Brick Wall
					return "";
				case ItemID.PlatinumBrickWall://722 Platinum Brick Wall
					return "";
				case ItemID.BeamSword://723 Beam Sword
					return "";
				case ItemID.IceBlade://724 Ice Blade
					return "";
				case ItemID.IceBow://725 Ice Bow
					return "";
				case ItemID.FrostStaff://726 Frost Staff
					return "";
				case ItemID.WoodHelmet://727 Wood Helmet
					return "";
				case ItemID.WoodBreastplate://728 Wood Breastplate
					return "";
				case ItemID.WoodGreaves://729 Wood Greaves
					return "";
				case ItemID.EbonwoodHelmet://730 Ebonwood Helmet
					return "";
				case ItemID.EbonwoodBreastplate://731 Ebonwood Breastplate
					return "";
				case ItemID.EbonwoodGreaves://732 Ebonwood Greaves
					return "";
				case ItemID.RichMahoganyHelmet://733 Rich Mahogany Helmet
					return "";
				case ItemID.RichMahoganyBreastplate://734 Rich Mahogany Breastplate
					return "";
				case ItemID.RichMahoganyGreaves://735 Rich Mahogany Greaves
					return "";
				case ItemID.PearlwoodHelmet://736 Pearlwood Helmet
					return "";
				case ItemID.PearlwoodBreastplate://737 Pearlwood Breastplate
					return "";
				case ItemID.PearlwoodGreaves://738 Pearlwood Greaves
					return "";
				case ItemID.AmethystStaff://739 Amethyst Staff
					return "";
				case ItemID.TopazStaff://740 Topaz Staff
					return "";
				case ItemID.SapphireStaff://741 Sapphire Staff
					return "";
				case ItemID.EmeraldStaff://742 Emerald Staff
					return "";
				case ItemID.RubyStaff://743 Ruby Staff
					return "";
				case ItemID.DiamondStaff://744 Diamond Staff
					return "";
				case ItemID.GrassWall://745 Grass Wall
					return "";
				case ItemID.JungleWall://746 Jungle Wall
					return "";
				case ItemID.FlowerWall://747 Flower Wall
					return "";
				case ItemID.Jetpack://748 Jetpack
					return "";
				case ItemID.ButterflyWings://749 Butterfly Wings
					return "";
				case ItemID.CactusWall://750 Cactus Wall
					return "";
				case ItemID.Cloud://751 Cloud
					return "";
				case ItemID.CloudWall://752 Cloud Wall
					return "";
				case ItemID.Seaweed://753 Seaweed
					return "";
				case ItemID.RuneHat://754 Rune Hat
					return "";
				case ItemID.RuneRobe://755 Rune Robe
					return "";
				case ItemID.MushroomSpear://756 Mushroom Spear
					return "";
				case ItemID.TerraBlade://757 Terra Blade
					return "";
				case ItemID.GrenadeLauncher://758 Grenade Launcher
					return "";
				case ItemID.RocketLauncher://759 Rocket Launcher
					return "";
				case ItemID.ProximityMineLauncher://760 Proximity Mine Launcher
					return "";
				case ItemID.FairyWings://761 Fairy Wings
					return "";
				case ItemID.SlimeBlock://762 Slime Block
					return "";
				case ItemID.FleshBlock://763 Flesh Block
					return "";
				case ItemID.MushroomWall://764 Mushroom Wall
					return "";
				case ItemID.RainCloud://765 Rain Cloud
					return "";
				case ItemID.BoneBlock://766 Bone Block
					return "";
				case ItemID.FrozenSlimeBlock://767 Frozen Slime Block
					return "";
				case ItemID.BoneBlockWall://768 Bone Block Wall
					return "";
				case ItemID.SlimeBlockWall://769 Slime Block Wall
					return "";
				case ItemID.FleshBlockWall://770 Flesh Block Wall
					return "";
				case ItemID.RocketI://771 Rocket I
					return "";
				case ItemID.RocketII://772 Rocket II
					return "";
				case ItemID.RocketIII://773 Rocket III
					return "";
				case ItemID.RocketIV://774 Rocket IV
					return "";
				case ItemID.AsphaltBlock://775 Asphalt Block
					return "";
				case ItemID.CobaltPickaxe://776 Cobalt Pickaxe
					return "";
				case ItemID.MythrilPickaxe://777 Mythril Pickaxe
					return "";
				case ItemID.AdamantitePickaxe://778 Adamantite Pickaxe
					return "";
				case ItemID.Clentaminator://779 Clentaminator
					return "";
				case ItemID.GreenSolution://780 Green Solution
					return "";
				case ItemID.BlueSolution://781 Blue Solution
					return "";
				case ItemID.PurpleSolution://782 Purple Solution
					return "";
				case ItemID.DarkBlueSolution://783 Dark Blue Solution
					return "";
				case ItemID.RedSolution://784 Red Solution
					return "";
				case ItemID.HarpyWings://785 Harpy Wings
					return "";
				case ItemID.BoneWings://786 Bone Wings
					return "";
				case ItemID.Hammush://787 Hammush
					return "";
				case ItemID.NettleBurst://788 Nettle Burst
					return "";
				case ItemID.AnkhBanner://789 Ankh Banner
					return "";
				case ItemID.SnakeBanner://790 Snake Banner
					return "";
				case ItemID.OmegaBanner://791 Omega Banner
					return "";
				case ItemID.CrimsonHelmet://792 Crimson Helmet
					return "";
				case ItemID.CrimsonScalemail://793 Crimson Scalemail
					return "";
				case ItemID.CrimsonGreaves://794 Crimson Greaves
					return "";
				case ItemID.BloodButcherer://795 Blood Butcherer
					return "";
				case ItemID.TendonBow://796 Tendon Bow
					return "";
				case ItemID.FleshGrinder://797 Flesh Grinder
					return "";
				case ItemID.DeathbringerPickaxe://798 Deathbringer Pickaxe
					return "";
				case ItemID.BloodLustCluster://799 Blood Lust Cluster
					return "";
				case ItemID.TheUndertaker://800 The Undertaker
					return "";
				case ItemID.TheMeatball://801 The Meatball
					return "";
				case ItemID.TheRottedFork://802 The Rotted Fork
					return "";
				case ItemID.EskimoHood://803 Snow Hood
					return "";
				case ItemID.EskimoCoat://804 Snow Coat
					return "";
				case ItemID.EskimoPants://805 Snow Pants
					return "";
				case ItemID.LivingWoodChair://806 Living Wood Chair
					return "";
				case ItemID.CactusChair://807 Cactus Chair
					return "";
				case ItemID.BoneChair://808 Bone Chair
					return "";
				case ItemID.FleshChair://809 Flesh Chair
					return "";
				case ItemID.MushroomChair://810 Mushroom Chair
					return "";
				case ItemID.BoneWorkBench://811 Bone Work Bench
					return "";
				case ItemID.CactusWorkBench://812 Cactus Work Bench
					return "";
				case ItemID.FleshWorkBench://813 Flesh Work Bench
					return "";
				case ItemID.MushroomWorkBench://814 Mushroom Work Bench
					return "";
				case ItemID.SlimeWorkBench://815 Slime Work Bench
					return "";
				case ItemID.CactusDoor://816 Cactus Door
					return "";
				case ItemID.FleshDoor://817 Flesh Door
					return "";
				case ItemID.MushroomDoor://818 Mushroom Door
					return "";
				case ItemID.LivingWoodDoor://819 Living Wood Door
					return "";
				case ItemID.BoneDoor://820 Bone Door
					return "";
				case ItemID.FlameWings://821 Flame Wings
					return "";
				case ItemID.FrozenWings://822 Frozen Wings
					return "";
				case ItemID.GhostWings://823 Spectre Wings
					return "";
				case ItemID.SunplateBlock://824 Sunplate Block
					return "";
				case ItemID.DiscWall://825 Disc Wall
					return "";
				case ItemID.SkywareChair://826 Skyware Chair
					return "";
				case ItemID.BoneTable://827 Bone Table
					return "";
				case ItemID.FleshTable://828 Flesh Table
					return "";
				case ItemID.LivingWoodTable://829 Living Wood Table
					return "";
				case ItemID.SkywareTable://830 Skyware Table
					return "";
				case ItemID.LivingWoodChest://831 Living Wood Chest
					return "";
				case ItemID.LivingWoodWand://832 Living Wood Wand
					return "";
				case ItemID.PurpleIceBlock://833 Purple Ice Block
					return "";
				case ItemID.PinkIceBlock://834 Pink Ice Block
					return "";
				case ItemID.RedIceBlock://835 Red Ice Block
					return "";
				case ItemID.CrimstoneBlock://836 Crimstone Block
					return "";
				case ItemID.SkywareDoor://837 Skyware Door
					return "";
				case ItemID.SkywareChest://838 Skyware Chest
					return "";
				case ItemID.SteampunkHat://839 Steampunk Hat
					return "";
				case ItemID.SteampunkShirt://840 Steampunk Shirt
					return "";
				case ItemID.SteampunkPants://841 Steampunk Pants
					return "";
				case ItemID.BeeHat://842 Bee Hat
					return "";
				case ItemID.BeeShirt://843 Bee Shirt
					return "";
				case ItemID.BeePants://844 Bee Pants
					return "";
				case ItemID.WorldBanner://845 World Banner
					return "";
				case ItemID.SunBanner://846 Sun Banner
					return "";
				case ItemID.GravityBanner://847 Gravity Banner
					return "";
				case ItemID.PharaohsMask://848 Pharaoh's Mask
					return "";
				case ItemID.Actuator://849 Actuator
					return "";
				case ItemID.BlueWrench://850 Blue Wrench
					return "";
				case ItemID.GreenWrench://851 Green Wrench
					return "";
				case ItemID.BluePressurePlate://852 Blue Pressure Plate
					return "";
				case ItemID.YellowPressurePlate://853 Yellow Pressure Plate
					return "";
				case ItemID.DiscountCard://854 Discount Card
					return "";
				case ItemID.LuckyCoin://855 Lucky Coin
					return "";
				case ItemID.UnicornonaStick://856 Unicorn on a Stick
					return "";
				case ItemID.SandstorminaBottle://857 Sandstorm in a Bottle
					return "";
				case ItemID.BorealWoodSofa://858 Boreal Wood Sofa
					return "";
				case ItemID.BeachBall://859 Beach Ball
					return "";
				case ItemID.CharmofMyths://860 Charm of Myths
					return "";
				case ItemID.MoonShell://861 Moon Shell
					return "";
				case ItemID.StarVeil://862 Star Veil
					return "";
				case ItemID.WaterWalkingBoots://863 Water Walking Boots
					return "";
				case ItemID.Tiara://864 Tiara
					return "";
				case ItemID.PrincessDress://865 Princess Dress
					return "";
				case ItemID.PharaohsRobe://866 Pharaoh's Robe
					return "";
				case ItemID.GreenCap://867 Green Cap
					return "";
				case ItemID.MushroomCap://868 Mushroom Cap
					return "";
				case ItemID.TamOShanter://869 Tam O' Shanter
					return "";
				case ItemID.MummyMask://870 Mummy Mask
					return "";
				case ItemID.MummyShirt://871 Mummy Shirt
					return "";
				case ItemID.MummyPants://872 Mummy Pants
					return "";
				case ItemID.CowboyHat://873 Cowboy Hat
					return "";
				case ItemID.CowboyJacket://874 Cowboy Jacket
					return "";
				case ItemID.CowboyPants://875 Cowboy Pants
					return "";
				case ItemID.PirateHat://876 Pirate Hat
					return "";
				case ItemID.PirateShirt://877 Pirate Shirt
					return "";
				case ItemID.PiratePants://878 Pirate Pants
					return "";
				case ItemID.VikingHelmet://879 Viking Helmet
					return "";
				case ItemID.CrimtaneOre://880 Crimtane Ore
					return "";
				case ItemID.CactusSword://881 Cactus Sword
					return "";
				case ItemID.CactusPickaxe://882 Cactus Pickaxe
					return "";
				case ItemID.IceBrick://883 Ice Brick
					return "";
				case ItemID.IceBrickWall://884 Ice Brick Wall
					return "";
				case ItemID.AdhesiveBandage://885 Adhesive Bandage
					return "";
				case ItemID.ArmorPolish://886 Armor Polish
					return "";
				case ItemID.Bezoar://887 Bezoar
					return "";
				case ItemID.Blindfold://888 Blindfold
					return "";
				case ItemID.FastClock://889 Fast Clock
					return "";
				case ItemID.Megaphone://890 Megaphone
					return "";
				case ItemID.Nazar://891 Nazar
					return "";
				case ItemID.Vitamins://892 Vitamins
					return "";
				case ItemID.TrifoldMap://893 Trifold Map
					return "";
				case ItemID.CactusHelmet://894 Cactus Helmet
					return "";
				case ItemID.CactusBreastplate://895 Cactus Breastplate
					return "";
				case ItemID.CactusLeggings://896 Cactus Leggings
					return "";
				case ItemID.PowerGlove://897 Power Glove
					return "";
				case ItemID.LightningBoots://898 Lightning Boots
					return "";
				case ItemID.SunStone://899 Sun Stone
					return "";
				case ItemID.MoonStone://900 Moon Stone
					return "";
				case ItemID.ArmorBracing://901 Armor Bracing
					return "";
				case ItemID.MedicatedBandage://902 Medicated Bandage
					return "";
				case ItemID.ThePlan://903 The Plan
					return "";
				case ItemID.CountercurseMantra://904 Countercurse Mantra
					return "";
				case ItemID.CoinGun://905 Coin Gun
					return "";
				case ItemID.LavaCharm://906 Lava Charm
					return "";
				case ItemID.ObsidianWaterWalkingBoots://907 Obsidian Water Walking Boots
					return "";
				case ItemID.LavaWaders://908 Lava Waders
					return "";
				case ItemID.PureWaterFountain://909 Pure Water Fountain
					return "";
				case ItemID.DesertWaterFountain://910 Desert Water Fountain
					return "";
				case ItemID.Shadewood://911 Shadewood
					return "";
				case ItemID.ShadewoodDoor://912 Shadewood Door
					return "";
				case ItemID.ShadewoodPlatform://913 Shadewood Platform
					return "";
				case ItemID.ShadewoodChest://914 Shadewood Chest
					return "";
				case ItemID.ShadewoodChair://915 Shadewood Chair
					return "";
				case ItemID.ShadewoodWorkBench://916 Shadewood Work Bench
					return "";
				case ItemID.ShadewoodTable://917 Shadewood Table
					return "";
				case ItemID.ShadewoodDresser://918 Shadewood Dresser
					return "";
				case ItemID.ShadewoodPiano://919 Shadewood Piano
					return "";
				case ItemID.ShadewoodBed://920 Shadewood Bed
					return "";
				case ItemID.ShadewoodSword://921 Shadewood Sword
					return "";
				case ItemID.ShadewoodHammer://922 Shadewood Hammer
					return "";
				case ItemID.ShadewoodBow://923 Shadewood Bow
					return "";
				case ItemID.ShadewoodHelmet://924 Shadewood Helmet
					return "";
				case ItemID.ShadewoodBreastplate://925 Shadewood Breastplate
					return "";
				case ItemID.ShadewoodGreaves://926 Shadewood Greaves
					return "";
				case ItemID.ShadewoodWall://927 Shadewood Wall
					return "";
				case ItemID.Cannon://928 Cannon
					return "";
				case ItemID.Cannonball://929 Cannonball
					return "";
				case ItemID.FlareGun://930 Flare Gun
					return "";
				case ItemID.Flare://931 Flare
					return "";
				case ItemID.BoneWand://932 Bone Wand
					return "";
				case ItemID.LeafWand://933 Leaf Wand
					return "";
				case ItemID.FlyingCarpet://934 Flying Carpet
					return "";
				case ItemID.AvengerEmblem://935 Avenger Emblem
					return "";
				case ItemID.MechanicalGlove://936 Mechanical Glove
					return "";
				case ItemID.LandMine://937 Land Mine
					return "";
				case ItemID.PaladinsShield://938 Paladin's Shield
					return "";
				case ItemID.WebSlinger://939 Web Slinger
					return "";
				case ItemID.JungleWaterFountain://940 Jungle Water Fountain
					return "";
				case ItemID.IcyWaterFountain://941 Icy Water Fountain
					return "";
				case ItemID.CorruptWaterFountain://942 Corrupt Water Fountain
					return "";
				case ItemID.CrimsonWaterFountain://943 Crimson Water Fountain
					return "";
				case ItemID.HallowedWaterFountain://944 Hallowed Water Fountain
					return "";
				case ItemID.BloodWaterFountain://945 Blood Water Fountain
					return "";
				case ItemID.Umbrella://946 Umbrella
					return "";
				case ItemID.ChlorophyteOre://947 Chlorophyte Ore
					return "";
				case ItemID.SteampunkWings://948 Steampunk Wings
					return "";
				case ItemID.Snowball://949 Snowball
					return "";
				case ItemID.IceSkates://950 Ice Skates
					return "";
				case ItemID.SnowballLauncher://951 Snowball Launcher
					return "";
				case ItemID.WebCoveredChest://952 Web Covered Chest
					return "";
				case ItemID.ClimbingClaws://953 Climbing Claws
					return "";
				case ItemID.AncientIronHelmet://954 Ancient Iron Helmet
					return "";
				case ItemID.AncientGoldHelmet://955 Ancient Gold Helmet
					return "";
				case ItemID.AncientShadowHelmet://956 Ancient Shadow Helmet
					return "";
				case ItemID.AncientShadowScalemail://957 Ancient Shadow Scalemail
					return "";
				case ItemID.AncientShadowGreaves://958 Ancient Shadow Greaves
					return "";
				case ItemID.AncientNecroHelmet://959 Ancient Necro Helmet
					return "";
				case ItemID.AncientCobaltHelmet://960 Ancient Cobalt Helmet
					return "";
				case ItemID.AncientCobaltBreastplate://961 Ancient Cobalt Breastplate
					return "";
				case ItemID.AncientCobaltLeggings://962 Ancient Cobalt Leggings
					return "";
				case ItemID.BlackBelt://963 Black Belt
					return "";
				case ItemID.Boomstick://964 Boomstick
					return "";
				case ItemID.Rope://965 Rope
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b4/Rope.png/revision/latest?cb=20200516221306&format=original";
				case ItemID.Campfire://966 Campfire
					return "";
				case ItemID.Marshmallow://967 Marshmallow
					return "";
				case ItemID.MarshmallowonaStick://968 Marshmallow on a Stick
					return "";
				case ItemID.CookedMarshmallow://969 Cooked Marshmallow
					return "";
				case ItemID.RedRocket://970 Red Rocket
					return "";
				case ItemID.GreenRocket://971 Green Rocket
					return "";
				case ItemID.BlueRocket://972 Blue Rocket
					return "";
				case ItemID.YellowRocket://973 Yellow Rocket
					return "";
				case ItemID.IceTorch://974 Ice Torch
					return "";
				case ItemID.ShoeSpikes://975 Shoe Spikes
					return "";
				case ItemID.TigerClimbingGear://976 Tiger Climbing Gear
					return "";
				case ItemID.Tabi://977 Tabi
					return "";
				case ItemID.PinkEskimoHood://978 Pink Snow Hood
					return "";
				case ItemID.PinkEskimoCoat://979 Pink Snow Coat
					return "";
				case ItemID.PinkEskimoPants://980 Pink Snow Pants
					return "";
				case ItemID.PinkThread://981 Pink Thread
					return "";
				case ItemID.ManaRegenerationBand://982 Mana Regeneration Band
					return "";
				case ItemID.SandstorminaBalloon://983 Sandstorm in a Balloon
					return "";
				case ItemID.MasterNinjaGear://984 Master Ninja Gear
					return "";
				case ItemID.RopeCoil://985 Rope Coil
					return "";
				case ItemID.Blowgun://986 Blowgun
					return "";
				case ItemID.BlizzardinaBottle://987 Blizzard in a Bottle
					return "";
				case ItemID.FrostburnArrow://988 Frostburn Arrow
					return "";
				case ItemID.EnchantedSword://989 Enchanted Sword
					return "";
				case ItemID.PickaxeAxe://990 Pickaxe Axe
					return "";
				case ItemID.CobaltWaraxe://991 Cobalt Waraxe
					return "";
				case ItemID.MythrilWaraxe://992 Mythril Waraxe
					return "";
				case ItemID.AdamantiteWaraxe://993 Adamantite Waraxe
					return "";
				case ItemID.EatersBone://994 Eater's Bone
					return "";
				case ItemID.BlendOMatic://995 Blend-O-Matic
					return "";
				case ItemID.MeatGrinder://996 Meat Grinder
					return "";
				case ItemID.Extractinator://997 Extractinator
					return "";
				case ItemID.Solidifier://998 Solidifier
					return "";
				case ItemID.Amber://999 Amber
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/01/Amber.png/revision/latest?cb=20200516184405&format=original";
				case ItemID.ConfettiGun://1000 Confetti Gun
					return "";
				case ItemID.ChlorophyteMask://1001 Chlorophyte Mask
					return "";
				case ItemID.ChlorophyteHelmet://1002 Chlorophyte Helmet
					return "";
				case ItemID.ChlorophyteHeadgear://1003 Chlorophyte Headgear
					return "";
				case ItemID.ChlorophytePlateMail://1004 Chlorophyte Plate Mail
					return "";
				case ItemID.ChlorophyteGreaves://1005 Chlorophyte Greaves
					return "";
				case ItemID.ChlorophyteBar://1006 Chlorophyte Bar
					return "";
				case ItemID.RedDye://1007 Red Dye
					return "";
				case ItemID.OrangeDye://1008 Orange Dye
					return "";
				case ItemID.YellowDye://1009 Yellow Dye
					return "";
				case ItemID.LimeDye://1010 Lime Dye
					return "";
				case ItemID.GreenDye://1011 Green Dye
					return "";
				case ItemID.TealDye://1012 Teal Dye
					return "";
				case ItemID.CyanDye://1013 Cyan Dye
					return "";
				case ItemID.SkyBlueDye://1014 Sky Blue Dye
					return "";
				case ItemID.BlueDye://1015 Blue Dye
					return "";
				case ItemID.PurpleDye://1016 Purple Dye
					return "";
				case ItemID.VioletDye://1017 Violet Dye
					return "";
				case ItemID.PinkDye://1018 Pink Dye
					return "";
				case ItemID.RedandBlackDye://1019 Red and Black Dye
					return "";
				case ItemID.OrangeandBlackDye://1020 Orange and Black Dye
					return "";
				case ItemID.YellowandBlackDye://1021 Yellow and Black Dye
					return "";
				case ItemID.LimeandBlackDye://1022 Lime and Black Dye
					return "";
				case ItemID.GreenandBlackDye://1023 Green and Black Dye
					return "";
				case ItemID.TealandBlackDye://1024 Teal and Black Dye
					return "";
				case ItemID.CyanandBlackDye://1025 Cyan and Black Dye
					return "";
				case ItemID.SkyBlueandBlackDye://1026 Sky Blue and Black Dye
					return "";
				case ItemID.BlueandBlackDye://1027 Blue and Black Dye
					return "";
				case ItemID.PurpleandBlackDye://1028 Purple and Black Dye
					return "";
				case ItemID.VioletandBlackDye://1029 Violet and Black Dye
					return "";
				case ItemID.PinkandBlackDye://1030 Pink and Black Dye
					return "";
				case ItemID.FlameDye://1031 Flame Dye
					return "";
				case ItemID.FlameAndBlackDye://1032 Flame and Black Dye
					return "";
				case ItemID.GreenFlameDye://1033 Green Flame Dye
					return "";
				case ItemID.GreenFlameAndBlackDye://1034 Green Flame and Black Dye
					return "";
				case ItemID.BlueFlameDye://1035 Blue Flame Dye
					return "";
				case ItemID.BlueFlameAndBlackDye://1036 Blue Flame and Black Dye
					return "";
				case ItemID.SilverDye://1037 Silver Dye
					return "";
				case ItemID.BrightRedDye://1038 Bright Red Dye
					return "";
				case ItemID.BrightOrangeDye://1039 Bright Orange Dye
					return "";
				case ItemID.BrightYellowDye://1040 Bright Yellow Dye
					return "";
				case ItemID.BrightLimeDye://1041 Bright Lime Dye
					return "";
				case ItemID.BrightGreenDye://1042 Bright Green Dye
					return "";
				case ItemID.BrightTealDye://1043 Bright Teal Dye
					return "";
				case ItemID.BrightCyanDye://1044 Bright Cyan Dye
					return "";
				case ItemID.BrightSkyBlueDye://1045 Bright Sky Blue Dye
					return "";
				case ItemID.BrightBlueDye://1046 Bright Blue Dye
					return "";
				case ItemID.BrightPurpleDye://1047 Bright Purple Dye
					return "";
				case ItemID.BrightVioletDye://1048 Bright Violet Dye
					return "";
				case ItemID.BrightPinkDye://1049 Bright Pink Dye
					return "";
				case ItemID.BlackDye://1050 Black Dye
					return "";
				case ItemID.RedandSilverDye://1051 Red and Silver Dye
					return "";
				case ItemID.OrangeandSilverDye://1052 Orange and Silver Dye
					return "";
				case ItemID.YellowandSilverDye://1053 Yellow and Silver Dye
					return "";
				case ItemID.LimeandSilverDye://1054 Lime and Silver Dye
					return "";
				case ItemID.GreenandSilverDye://1055 Green and Silver Dye
					return "";
				case ItemID.TealandSilverDye://1056 Teal and Silver Dye
					return "";
				case ItemID.CyanandSilverDye://1057 Cyan and Silver Dye
					return "";
				case ItemID.SkyBlueandSilverDye://1058 Sky Blue and Silver Dye
					return "";
				case ItemID.BlueandSilverDye://1059 Blue and Silver Dye
					return "";
				case ItemID.PurpleandSilverDye://1060 Purple and Silver Dye
					return "";
				case ItemID.VioletandSilverDye://1061 Violet and Silver Dye
					return "";
				case ItemID.PinkandSilverDye://1062 Pink and Silver Dye
					return "";
				case ItemID.IntenseFlameDye://1063 Intense Flame Dye
					return "";
				case ItemID.IntenseGreenFlameDye://1064 Intense Green Flame Dye
					return "";
				case ItemID.IntenseBlueFlameDye://1065 Intense Blue Flame Dye
					return "";
				case ItemID.RainbowDye://1066 Rainbow Dye
					return "";
				case ItemID.IntenseRainbowDye://1067 Intense Rainbow Dye
					return "";
				case ItemID.YellowGradientDye://1068 Yellow Gradient Dye
					return "";
				case ItemID.CyanGradientDye://1069 Cyan Gradient Dye
					return "";
				case ItemID.VioletGradientDye://1070 Violet Gradient Dye
					return "";
				case ItemID.Paintbrush://1071 Paintbrush
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Paintbrush.png/revision/latest?cb=20200516220138&format=original";
				case ItemID.PaintRoller://1072 Paint Roller
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Paint_Roller.png/revision/latest?cb=20200516220132&format=original";
				case ItemID.RedPaint://1073 Red Paint
					return "";
				case ItemID.OrangePaint://1074 Orange Paint
					return "";
				case ItemID.YellowPaint://1075 Yellow Paint
					return "";
				case ItemID.LimePaint://1076 Lime Paint
					return "";
				case ItemID.GreenPaint://1077 Green Paint
					return "";
				case ItemID.TealPaint://1078 Teal Paint
					return "";
				case ItemID.CyanPaint://1079 Cyan Paint
					return "";
				case ItemID.SkyBluePaint://1080 Sky Blue Paint
					return "";
				case ItemID.BluePaint://1081 Blue Paint
					return "";
				case ItemID.PurplePaint://1082 Purple Paint
					return "";
				case ItemID.VioletPaint://1083 Violet Paint
					return "";
				case ItemID.PinkPaint://1084 Pink Paint
					return "";
				case ItemID.DeepRedPaint://1085 Deep Red Paint
					return "";
				case ItemID.DeepOrangePaint://1086 Deep Orange Paint
					return "";
				case ItemID.DeepYellowPaint://1087 Deep Yellow Paint
					return "";
				case ItemID.DeepLimePaint://1088 Deep Lime Paint
					return "";
				case ItemID.DeepGreenPaint://1089 Deep Green Paint
					return "";
				case ItemID.DeepTealPaint://1090 Deep Teal Paint
					return "";
				case ItemID.DeepCyanPaint://1091 Deep Cyan Paint
					return "";
				case ItemID.DeepSkyBluePaint://1092 Deep Sky Blue Paint
					return "";
				case ItemID.DeepBluePaint://1093 Deep Blue Paint
					return "";
				case ItemID.DeepPurplePaint://1094 Deep Purple Paint
					return "";
				case ItemID.DeepVioletPaint://1095 Deep Violet Paint
					return "";
				case ItemID.DeepPinkPaint://1096 Deep Pink Paint
					return "";
				case ItemID.BlackPaint://1097 Black Paint
					return "";
				case ItemID.WhitePaint://1098 White Paint
					return "";
				case ItemID.GrayPaint://1099 Gray Paint
					return "";
				case ItemID.PaintScraper://1100 Paint Scraper
					return "";
				case ItemID.LihzahrdBrick://1101 Lihzahrd Brick
					return "";
				case ItemID.LihzahrdBrickWall://1102 Lihzahrd Brick Wall
					return "";
				case ItemID.SlushBlock://1103 Slush Block
					return "";
				case ItemID.PalladiumOre://1104 Palladium Ore
					return "";
				case ItemID.OrichalcumOre://1105 Orichalcum Ore
					return "";
				case ItemID.TitaniumOre://1106 Titanium Ore
					return "";
				case ItemID.TealMushroom://1107 Teal Mushroom
					return "";
				case ItemID.GreenMushroom://1108 Green Mushroom
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d0/Green_Mushroom.png/revision/latest?cb=20200516213658&format=original";
				case ItemID.SkyBlueFlower://1109 Sky Blue Flower
					return "";
				case ItemID.YellowMarigold://1110 Yellow Marigold
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/87/Yellow_Marigold.png/revision/latest?cb=20200516223751&format=original";
				case ItemID.BlueBerries://1111 Blue Berries
					return "";
				case ItemID.LimeKelp://1112 Lime Kelp
					return "";
				case ItemID.PinkPricklyPear://1113 Pink Prickly Pear
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d4/Pink_Prickly_Pear.png/revision/latest?cb=20200516220537&format=original";
				case ItemID.OrangeBloodroot://1114 Orange Bloodroot
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Orange_Bloodroot.png/revision/latest?cb=20200516220013&format=original";
				case ItemID.RedHusk://1115 Red Husk
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/ad/Red_Husk.png/revision/latest?cb=20200516221105&format=original";
				case ItemID.CyanHusk://1116 Cyan Husk
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f2/Cyan_Husk.png/revision/latest?cb=20200516211000&format=original";
				case ItemID.VioletHusk://1117 Violet Husk
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6a/Violet_Husk.png/revision/latest?cb=20200516223347&format=original";
				case ItemID.PurpleMucos://1118 Purple Mucus
					return "";
				case ItemID.BlackInk://1119 Black Ink
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/41/Black_Ink.png/revision/latest?cb=20200730171913&format=original";
				case ItemID.DyeVat://1120 Dye Vat
					return "";
				case ItemID.BeeGun://1121 Bee Gun
					return "";
				case ItemID.PossessedHatchet://1122 Possessed Hatchet
					return "";
				case ItemID.BeeKeeper://1123 Bee Keeper
					return "";
				case ItemID.Hive://1124 Hive
					return "";
				case ItemID.HoneyBlock://1125 Honey Block
					return "";
				case ItemID.HiveWall://1126 Hive Wall
					return "";
				case ItemID.CrispyHoneyBlock://1127 Crispy Honey Block
					return "";
				case ItemID.HoneyBucket://1128 Honey Bucket
					return "";
				case ItemID.HiveWand://1129 Hive Wand
					return "";
				case ItemID.Beenade://1130 Beenade
					return "";
				case ItemID.GravityGlobe://1131 Gravity Globe
					return "";
				case ItemID.HoneyComb://1132 Honey Comb
					return "";
				case ItemID.Abeemination://1133 Abeemination
					return "";
				case ItemID.BottledHoney://1134 Bottled Honey
					return "";
				case ItemID.RainHat://1135 Rain Hat
					return "";
				case ItemID.RainCoat://1136 Rain Coat
					return "";
				case ItemID.LihzahrdDoor://1137 Lihzahrd Door
					return "";
				case ItemID.DungeonDoor://1138 Dungeon Door
					return "";
				case ItemID.LeadDoor://1139 Lead Door
					return "";
				case ItemID.IronDoor://1140 Iron Door
					return "";
				case ItemID.TempleKey://1141 Temple Key
					return "";
				case ItemID.LihzahrdChest://1142 Lihzahrd Chest
					return "";
				case ItemID.LihzahrdChair://1143 Lihzahrd Chair
					return "";
				case ItemID.LihzahrdTable://1144 Lihzahrd Table
					return "";
				case ItemID.LihzahrdWorkBench://1145 Lihzahrd Work Bench
					return "";
				case ItemID.SuperDartTrap://1146 Super Dart Trap
					return "";
				case ItemID.FlameTrap://1147 Flame Trap
					return "";
				case ItemID.SpikyBallTrap://1148 Spiky Ball Trap
					return "";
				case ItemID.SpearTrap://1149 Spear Trap
					return "";
				case ItemID.WoodenSpike://1150 Wooden Spike
					return "";
				case ItemID.LihzahrdPressurePlate://1151 Lihzahrd Pressure Plate
					return "";
				case ItemID.LihzahrdStatue://1152 Lihzahrd Statue
					return "";
				case ItemID.LihzahrdWatcherStatue://1153 Lihzahrd Watcher Statue
					return "";
				case ItemID.LihzahrdGuardianStatue://1154 Lihzahrd Guardian Statue
					return "";
				case ItemID.WaspGun://1155 Wasp Gun
					return "";
				case ItemID.PiranhaGun://1156 Piranha Gun
					return "";
				case ItemID.PygmyStaff://1157 Pygmy Staff
					return "";
				case ItemID.PygmyNecklace://1158 Pygmy Necklace
					return "";
				case ItemID.TikiMask://1159 Tiki Mask
					return "";
				case ItemID.TikiShirt://1160 Tiki Shirt
					return "";
				case ItemID.TikiPants://1161 Tiki Pants
					return "";
				case ItemID.LeafWings://1162 Leaf Wings
					return "";
				case ItemID.BlizzardinaBalloon://1163 Blizzard in a Balloon
					return "";
				case ItemID.BundleofBalloons://1164 Bundle of Balloons
					return "";
				case ItemID.BatWings://1165 Bat Wings
					return "";
				case ItemID.BoneSword://1166 Bone Sword
					return "";
				case ItemID.HerculesBeetle://1167 Hercules Beetle
					return "";
				case ItemID.SmokeBomb://1168 Smoke Bomb
					return "";
				case ItemID.BoneKey://1169 Bone Key
					return "";
				case ItemID.Nectar://1170 Nectar
					return "";
				case ItemID.TikiTotem://1171 Tiki Totem
					return "";
				case ItemID.LizardEgg://1172 Lizard Egg
					return "";
				case ItemID.GraveMarker://1173 Grave Marker
					return "";
				case ItemID.CrossGraveMarker://1174 Cross Grave Marker
					return "";
				case ItemID.Headstone://1175 Headstone
					return "";
				case ItemID.Gravestone://1176 Gravestone
					return "";
				case ItemID.Obelisk://1177 Obelisk
					return "";
				case ItemID.LeafBlower://1178 Leaf Blower
					return "";
				case ItemID.ChlorophyteBullet://1179 Chlorophyte Bullet
					return "";
				case ItemID.ParrotCracker://1180 Parrot Cracker
					return "";
				case ItemID.StrangeGlowingMushroom://1181 Strange Glowing Mushroom
					return "";
				case ItemID.Seedling://1182 Seedling
					return "";
				case ItemID.WispinaBottle://1183 Wisp in a Bottle
					return "";
				case ItemID.PalladiumBar://1184 Palladium Bar
					return "";
				case ItemID.PalladiumSword://1185 Palladium Sword
					return "";
				case ItemID.PalladiumPike://1186 Palladium Pike
					return "";
				case ItemID.PalladiumRepeater://1187 Palladium Repeater
					return "";
				case ItemID.PalladiumPickaxe://1188 Palladium Pickaxe
					return "";
				case ItemID.PalladiumDrill://1189 Palladium Drill
					return "";
				case ItemID.PalladiumChainsaw://1190 Palladium Chainsaw
					return "";
				case ItemID.OrichalcumBar://1191 Orichalcum Bar
					return "";
				case ItemID.OrichalcumSword://1192 Orichalcum Sword
					return "";
				case ItemID.OrichalcumHalberd://1193 Orichalcum Halberd
					return "";
				case ItemID.OrichalcumRepeater://1194 Orichalcum Repeater
					return "";
				case ItemID.OrichalcumPickaxe://1195 Orichalcum Pickaxe
					return "";
				case ItemID.OrichalcumDrill://1196 Orichalcum Drill
					return "";
				case ItemID.OrichalcumChainsaw://1197 Orichalcum Chainsaw
					return "";
				case ItemID.TitaniumBar://1198 Titanium Bar
					return "";
				case ItemID.TitaniumSword://1199 Titanium Sword
					return "";
				case ItemID.TitaniumTrident://1200 Titanium Trident
					return "";
				case ItemID.TitaniumRepeater://1201 Titanium Repeater
					return "";
				case ItemID.TitaniumPickaxe://1202 Titanium Pickaxe
					return "";
				case ItemID.TitaniumDrill://1203 Titanium Drill
					return "";
				case ItemID.TitaniumChainsaw://1204 Titanium Chainsaw
					return "";
				case ItemID.PalladiumMask://1205 Palladium Mask
					return "";
				case ItemID.PalladiumHelmet://1206 Palladium Helmet
					return "";
				case ItemID.PalladiumHeadgear://1207 Palladium Headgear
					return "";
				case ItemID.PalladiumBreastplate://1208 Palladium Breastplate
					return "";
				case ItemID.PalladiumLeggings://1209 Palladium Leggings
					return "";
				case ItemID.OrichalcumMask://1210 Orichalcum Mask
					return "";
				case ItemID.OrichalcumHelmet://1211 Orichalcum Helmet
					return "";
				case ItemID.OrichalcumHeadgear://1212 Orichalcum Headgear
					return "";
				case ItemID.OrichalcumBreastplate://1213 Orichalcum Breastplate
					return "";
				case ItemID.OrichalcumLeggings://1214 Orichalcum Leggings
					return "";
				case ItemID.TitaniumMask://1215 Titanium Mask
					return "";
				case ItemID.TitaniumHelmet://1216 Titanium Helmet
					return "";
				case ItemID.TitaniumHeadgear://1217 Titanium Headgear
					return "";
				case ItemID.TitaniumBreastplate://1218 Titanium Breastplate
					return "";
				case ItemID.TitaniumLeggings://1219 Titanium Leggings
					return "";
				case ItemID.OrichalcumAnvil://1220 Orichalcum Anvil
					return "";
				case ItemID.TitaniumForge://1221 Titanium Forge
					return "";
				case ItemID.PalladiumWaraxe://1222 Palladium Waraxe
					return "";
				case ItemID.OrichalcumWaraxe://1223 Orichalcum Waraxe
					return "";
				case ItemID.TitaniumWaraxe://1224 Titanium Waraxe
					return "";
				case ItemID.HallowedBar://1225 Hallowed Bar
					return "";
				case ItemID.ChlorophyteClaymore://1226 Chlorophyte Claymore
					return "";
				case ItemID.ChlorophyteSaber://1227 Chlorophyte Saber
					return "";
				case ItemID.ChlorophytePartisan://1228 Chlorophyte Partisan
					return "";
				case ItemID.ChlorophyteShotbow://1229 Chlorophyte Shotbow
					return "";
				case ItemID.ChlorophytePickaxe://1230 Chlorophyte Pickaxe
					return "";
				case ItemID.ChlorophyteDrill://1231 Chlorophyte Drill
					return "";
				case ItemID.ChlorophyteChainsaw://1232 Chlorophyte Chainsaw
					return "";
				case ItemID.ChlorophyteGreataxe://1233 Chlorophyte Greataxe
					return "";
				case ItemID.ChlorophyteWarhammer://1234 Chlorophyte Warhammer
					return "";
				case ItemID.ChlorophyteArrow://1235 Chlorophyte Arrow
					return "";
				case ItemID.AmethystHook://1236 Amethyst Hook
					return "";
				case ItemID.TopazHook://1237 Topaz Hook
					return "";
				case ItemID.SapphireHook://1238 Sapphire Hook
					return "";
				case ItemID.EmeraldHook://1239 Emerald Hook
					return "";
				case ItemID.RubyHook://1240 Ruby Hook
					return "";
				case ItemID.DiamondHook://1241 Diamond Hook
					return "";
				case ItemID.AmberMosquito://1242 Amber Mosquito
					return "";
				case ItemID.UmbrellaHat://1243 Umbrella Hat
					return "";
				case ItemID.NimbusRod://1244 Nimbus Rod
					return "";
				case ItemID.OrangeTorch://1245 Orange Torch
					return "";
				case ItemID.CrimsandBlock://1246 Crimsand Block
					return "";
				case ItemID.BeeCloak://1247 Bee Cloak
					return "";
				case ItemID.EyeoftheGolem://1248 Eye of the Golem
					return "";
				case ItemID.HoneyBalloon://1249 Honey Balloon
					return "";
				case ItemID.BlueHorseshoeBalloon://1250 Blue Horseshoe Balloon
					return "";
				case ItemID.WhiteHorseshoeBalloon://1251 White Horseshoe Balloon
					return "";
				case ItemID.YellowHorseshoeBalloon://1252 Yellow Horseshoe Balloon
					return "";
				case ItemID.FrozenTurtleShell://1253 Frozen Turtle Shell
					return "";
				case ItemID.SniperRifle://1254 Sniper Rifle
					return "";
				case ItemID.VenusMagnum://1255 Venus Magnum
					return "";
				case ItemID.CrimsonRod://1256 Crimson Rod
					return "";
				case ItemID.CrimtaneBar://1257 Crimtane Bar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/ce/Crimtane_Bar.png/revision/latest?cb=20230405205757&format=original";
				case ItemID.Stynger://1258 Stynger
					return "";
				case ItemID.FlowerPow://1259 Flower Pow
					return "";
				case ItemID.RainbowGun://1260 Rainbow Gun
					return "";
				case ItemID.StyngerBolt://1261 Stynger Bolt
					return "";
				case ItemID.ChlorophyteJackhammer://1262 Chlorophyte Jackhammer
					return "";
				case ItemID.Teleporter://1263 Teleporter
					return "";
				case ItemID.FlowerofFrost://1264 Flower of Frost
					return "";
				case ItemID.Uzi://1265 Uzi
					return "";
				case ItemID.MagnetSphere://1266 Magnet Sphere
					return "";
				case ItemID.PurpleStainedGlass://1267 Purple Stained Glass
					return "";
				case ItemID.YellowStainedGlass://1268 Yellow Stained Glass
					return "";
				case ItemID.BlueStainedGlass://1269 Blue Stained Glass
					return "";
				case ItemID.GreenStainedGlass://1270 Green Stained Glass
					return "";
				case ItemID.RedStainedGlass://1271 Red Stained Glass
					return "";
				case ItemID.MulticoloredStainedGlass://1272 Multicolored Stained Glass
					return "";
				case ItemID.SkeletronHand://1273 Skeletron Hand
					return "";
				case ItemID.Skull://1274 Skull
					return "";
				case ItemID.BallaHat://1275 Balla Hat
					return "";
				case ItemID.GangstaHat://1276 Gangsta Hat
					return "";
				case ItemID.SailorHat://1277 Sailor Hat
					return "";
				case ItemID.EyePatch://1278 Eye Patch
					return "";
				case ItemID.SailorShirt://1279 Sailor Shirt
					return "";
				case ItemID.SailorPants://1280 Sailor Pants
					return "";
				case ItemID.SkeletronMask://1281 Skeletron Mask
					return "";
				case ItemID.AmethystRobe://1282 Amethyst Robe
					return "";
				case ItemID.TopazRobe://1283 Topaz Robe
					return "";
				case ItemID.SapphireRobe://1284 Sapphire Robe
					return "";
				case ItemID.EmeraldRobe://1285 Emerald Robe
					return "";
				case ItemID.RubyRobe://1286 Ruby Robe
					return "";
				case ItemID.DiamondRobe://1287 Diamond Robe
					return "";
				case ItemID.WhiteTuxedoShirt://1288 White Tuxedo Shirt
					return "";
				case ItemID.WhiteTuxedoPants://1289 White Tuxedo Pants
					return "";
				case ItemID.PanicNecklace://1290 Panic Necklace
					return "";
				case ItemID.LifeFruit://1291 Life Fruit
					return "";
				case ItemID.LihzahrdAltar://1292 Lihzahrd Altar
					return "";
				case ItemID.LihzahrdPowerCell://1293 Lihzahrd Power Cell
					return "";
				case ItemID.Picksaw://1294 Picksaw
					return "";
				case ItemID.HeatRay://1295 Heat Ray
					return "";
				case ItemID.StaffofEarth://1296 Staff of Earth
					return "";
				case ItemID.GolemFist://1297 Golem Fist
					return "";
				case ItemID.WaterChest://1298 Water Chest
					return "";
				case ItemID.Binoculars://1299 Binoculars
					return "";
				case ItemID.RifleScope://1300 Rifle Scope
					return "";
				case ItemID.DestroyerEmblem://1301 Destroyer Emblem
					return "";
				case ItemID.HighVelocityBullet://1302 High Velocity Bullet
					return "";
				case ItemID.JellyfishNecklace://1303 Jellyfish Necklace
					return "";
				case ItemID.ZombieArm://1304 Zombie Arm
					return "";
				case ItemID.TheAxe://1305 The Axe
					return "";
				case ItemID.IceSickle://1306 Ice Sickle
					return "";
				case ItemID.ClothierVoodooDoll://1307 Clothier Voodoo Doll
					return "";
				case ItemID.PoisonStaff://1308 Poison Staff
					return "";
				case ItemID.SlimeStaff://1309 Slime Staff
					return "";
				case ItemID.PoisonDart://1310 Poison Dart
					return "";
				case ItemID.EyeSpring://1311 Eye Spring
					return "";
				case ItemID.ToySled://1312 Toy Sled
					return "";
				case ItemID.BookofSkulls://1313 Book of Skulls
					return "";
				case ItemID.KOCannon://1314 KO Cannon
					return "";
				case ItemID.PirateMap://1315 Pirate Map
					return "";
				case ItemID.TurtleHelmet://1316 Turtle Helmet
					return "";
				case ItemID.TurtleScaleMail://1317 Turtle Scale Mail
					return "";
				case ItemID.TurtleLeggings://1318 Turtle Leggings
					return "";
				case ItemID.SnowballCannon://1319 Snowball Cannon
					return "";
				case ItemID.BonePickaxe://1320 Bone Pickaxe
					return "";
				case ItemID.MagicQuiver://1321 Magic Quiver
					return "";
				case ItemID.MagmaStone://1322 Magma Stone
					return "";
				case ItemID.ObsidianRose://1323 Obsidian Rose
					return "";
				case ItemID.Bananarang://1324 Bananarang
					return "";
				case ItemID.ChainKnife://1325 Chain Knife
					return "";
				case ItemID.RodofDiscord://1326 Rod of Discord
					return "";
				case ItemID.DeathSickle://1327 Death Sickle
					return "";
				case ItemID.TurtleShell://1328 Turtle Shell
					return "";
				case ItemID.TissueSample://1329 Tissue Sample
					return "";
				case ItemID.Vertebrae://1330 Vertebra
					return "";
				case ItemID.BloodySpine://1331 Bloody Spine
					return "";
				case ItemID.Ichor://1332 Ichor
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/47/Ichor.png/revision/latest?cb=20200516214220&format=original";
				case ItemID.IchorTorch://1333 Ichor Torch
					return "";
				case ItemID.IchorArrow://1334 Ichor Arrow
					return "";
				case ItemID.IchorBullet://1335 Ichor Bullet
					return "";
				case ItemID.GoldenShower://1336 Golden Shower
					return "";
				case ItemID.BunnyCannon://1337 Bunny Cannon
					return "";
				case ItemID.ExplosiveBunny://1338 Explosive Bunny
					return "";
				case ItemID.VialofVenom://1339 Vial of Venom
					return "";
				case ItemID.FlaskofVenom://1340 Flask of Venom
					return "";
				case ItemID.VenomArrow://1341 Venom Arrow
					return "";
				case ItemID.VenomBullet://1342 Venom Bullet
					return "";
				case ItemID.FireGauntlet://1343 Fire Gauntlet
					return "";
				case ItemID.Cog://1344 Cog
					return "";
				case ItemID.Confetti://1345 Confetti
					return "";
				case ItemID.Nanites://1346 Nanites
					return "";
				case ItemID.ExplosivePowder://1347 Explosive Powder
					return "";
				case ItemID.GoldDust://1348 Gold Dust
					return "";
				case ItemID.PartyBullet://1349 Party Bullet
					return "";
				case ItemID.NanoBullet://1350 Nano Bullet
					return "";
				case ItemID.ExplodingBullet://1351 Exploding Bullet
					return "";
				case ItemID.GoldenBullet://1352 Golden Bullet
					return "";
				case ItemID.FlaskofCursedFlames://1353 Flask of Cursed Flames
					return "";
				case ItemID.FlaskofFire://1354 Flask of Fire
					return "";
				case ItemID.FlaskofGold://1355 Flask of Gold
					return "";
				case ItemID.FlaskofIchor://1356 Flask of Ichor
					return "";
				case ItemID.FlaskofNanites://1357 Flask of Nanites
					return "";
				case ItemID.FlaskofParty://1358 Flask of Party
					return "";
				case ItemID.FlaskofPoison://1359 Flask of Poison
					return "";
				case ItemID.EyeofCthulhuTrophy://1360 Eye of Cthulhu Trophy
					return "";
				case ItemID.EaterofWorldsTrophy://1361 Eater of Worlds Trophy
					return "";
				case ItemID.BrainofCthulhuTrophy://1362 Brain of Cthulhu Trophy
					return "";
				case ItemID.SkeletronTrophy://1363 Skeletron Trophy
					return "";
				case ItemID.QueenBeeTrophy://1364 Queen Bee Trophy
					return "";
				case ItemID.WallofFleshTrophy://1365 Wall of Flesh Trophy
					return "";
				case ItemID.DestroyerTrophy://1366 Destroyer Trophy
					return "";
				case ItemID.SkeletronPrimeTrophy://1367 Skeletron Prime Trophy
					return "";
				case ItemID.RetinazerTrophy://1368 Retinazer Trophy
					return "";
				case ItemID.SpazmatismTrophy://1369 Spazmatism Trophy
					return "";
				case ItemID.PlanteraTrophy://1370 Plantera Trophy
					return "";
				case ItemID.GolemTrophy://1371 Golem Trophy
					return "";
				case ItemID.BloodMoonRising://1372 Blood Moon Rising
					return "";
				case ItemID.TheHangedMan://1373 The Hanged Man
					return "";
				case ItemID.GloryoftheFire://1374 Glory of the Fire
					return "";
				case ItemID.BoneWarp://1375 Bone Warp
					return "";
				case ItemID.WallSkeleton://1376 Wall Skeleton
					return "";
				case ItemID.HangingSkeleton://1377 Hanging Skeleton
					return "";
				case ItemID.BlueSlabWall://1378 Blue Slab Wall
					return "";
				case ItemID.BlueTiledWall://1379 Blue Tiled Wall
					return "";
				case ItemID.PinkSlabWall://1380 Pink Slab Wall
					return "";
				case ItemID.PinkTiledWall://1381 Pink Tiled Wall
					return "";
				case ItemID.GreenSlabWall://1382 Green Slab Wall
					return "";
				case ItemID.GreenTiledWall://1383 Green Tiled Wall
					return "";
				case ItemID.BlueBrickPlatform://1384 Blue Brick Platform
					return "";
				case ItemID.PinkBrickPlatform://1385 Pink Brick Platform
					return "";
				case ItemID.GreenBrickPlatform://1386 Green Brick Platform
					return "";
				case ItemID.MetalShelf://1387 Metal Shelf
					return "";
				case ItemID.BrassShelf://1388 Brass Shelf
					return "";
				case ItemID.WoodShelf://1389 Wood Shelf
					return "";
				case ItemID.BrassLantern://1390 Brass Lantern
					return "";
				case ItemID.CagedLantern://1391 Caged Lantern
					return "";
				case ItemID.CarriageLantern://1392 Carriage Lantern
					return "";
				case ItemID.AlchemyLantern://1393 Alchemy Lantern
					return "";
				case ItemID.DiablostLamp://1394 Diabolist Lamp
					return "";
				case ItemID.OilRagSconse://1395 Oil Rag Sconce
					return "";
				case ItemID.BlueDungeonChair://1396 Blue Dungeon Chair
					return "";
				case ItemID.BlueDungeonTable://1397 Blue Dungeon Table
					return "";
				case ItemID.BlueDungeonWorkBench://1398 Blue Dungeon Work Bench
					return "";
				case ItemID.GreenDungeonChair://1399 Green Dungeon Chair
					return "";
				case ItemID.GreenDungeonTable://1400 Green Dungeon Table
					return "";
				case ItemID.GreenDungeonWorkBench://1401 Green Dungeon Work Bench
					return "";
				case ItemID.PinkDungeonChair://1402 Pink Dungeon Chair
					return "";
				case ItemID.PinkDungeonTable://1403 Pink Dungeon Table
					return "";
				case ItemID.PinkDungeonWorkBench://1404 Pink Dungeon Work Bench
					return "";
				case ItemID.BlueDungeonCandle://1405 Blue Dungeon Candle
					return "";
				case ItemID.GreenDungeonCandle://1406 Green Dungeon Candle
					return "";
				case ItemID.PinkDungeonCandle://1407 Pink Dungeon Candle
					return "";
				case ItemID.BlueDungeonVase://1408 Blue Dungeon Vase
					return "";
				case ItemID.GreenDungeonVase://1409 Green Dungeon Vase
					return "";
				case ItemID.PinkDungeonVase://1410 Pink Dungeon Vase
					return "";
				case ItemID.BlueDungeonDoor://1411 Blue Dungeon Door
					return "";
				case ItemID.GreenDungeonDoor://1412 Green Dungeon Door
					return "";
				case ItemID.PinkDungeonDoor://1413 Pink Dungeon Door
					return "";
				case ItemID.BlueDungeonBookcase://1414 Blue Dungeon Bookcase
					return "";
				case ItemID.GreenDungeonBookcase://1415 Green Dungeon Bookcase
					return "";
				case ItemID.PinkDungeonBookcase://1416 Pink Dungeon Bookcase
					return "";
				case ItemID.Catacomb://1417 Catacomb
					return "";
				case ItemID.DungeonShelf://1418 Dungeon Shelf
					return "";
				case ItemID.SkellingtonJSkellingsworth://1419 Skellington J Skellingsworth
					return "";
				case ItemID.TheCursedMan://1420 The Cursed Man
					return "";
				case ItemID.TheEyeSeestheEnd://1421 The Eye Sees the End
					return "";
				case ItemID.SomethingEvilisWatchingYou://1422 Something Evil is Watching You
					return "";
				case ItemID.TheTwinsHaveAwoken://1423 The Twins Have Awoken
					return "";
				case ItemID.TheScreamer://1424 The Screamer
					return "";
				case ItemID.GoblinsPlayingPoker://1425 Goblins Playing Poker
					return "";
				case ItemID.Dryadisque://1426 Dryadisque
					return "";
				case ItemID.Sunflowers://1427 Sunflowers
					return "";
				case ItemID.TerrarianGothic://1428 Terrarian Gothic
					return "";
				case ItemID.Beanie://1429 Beanie
					return "";
				case ItemID.ImbuingStation://1430 Imbuing Station
					return "";
				case ItemID.StarinaBottle://1431 Star in a Bottle
					return "";
				case ItemID.EmptyBullet://1432 Empty Bullet
					return "";
				case ItemID.Impact://1433 Impact
					return "";
				case ItemID.PoweredbyBirds://1434 Powered by Birds
					return "";
				case ItemID.TheDestroyer://1435 The Destroyer
					return "";
				case ItemID.ThePersistencyofEyes://1436 The Persistency of Eyes
					return "";
				case ItemID.UnicornCrossingtheHallows://1437 Unicorn Crossing the Hallows
					return "";
				case ItemID.GreatWave://1438 Great Wave
					return "";
				case ItemID.StarryNight://1439 Starry Night
					return "";
				case ItemID.GuidePicasso://1440 Guide Picasso
					return "";
				case ItemID.TheGuardiansGaze://1441 The Guardian's Gaze
					return "";
				case ItemID.FatherofSomeone://1442 Father of Someone
					return "";
				case ItemID.NurseLisa://1443 Nurse Lisa
					return "";
				case ItemID.ShadowbeamStaff://1444 Shadowbeam Staff
					return "";
				case ItemID.InfernoFork://1445 Inferno Fork
					return "";
				case ItemID.SpectreStaff://1446 Spectre Staff
					return "";
				case ItemID.WoodenFence://1447 Wooden Fence
					return "";
				case ItemID.LeadFence://1448 Lead Fence
					return "";
				case ItemID.BubbleMachine://1449 Bubble Machine
					return "";
				case ItemID.BubbleWand://1450 Bubble Wand
					return "";
				case ItemID.MarchingBonesBanner://1451 Marching Bones Banner
					return "";
				case ItemID.NecromanticSign://1452 Necromantic Sign
					return "";
				case ItemID.RustedCompanyStandard://1453 Rusted Company Standard
					return "";
				case ItemID.RaggedBrotherhoodSigil://1454 Ragged Brotherhood Sigil
					return "";
				case ItemID.MoltenLegionFlag://1455 Molten Legion Flag
					return "";
				case ItemID.DiabolicSigil://1456 Diabolic Sigil
					return "";
				case ItemID.ObsidianPlatform://1457 Obsidian Platform
					return "";
				case ItemID.ObsidianDoor://1458 Obsidian Door
					return "";
				case ItemID.ObsidianChair://1459 Obsidian Chair
					return "";
				case ItemID.ObsidianTable://1460 Obsidian Table
					return "";
				case ItemID.ObsidianWorkBench://1461 Obsidian Work Bench
					return "";
				case ItemID.ObsidianVase://1462 Obsidian Vase
					return "";
				case ItemID.ObsidianBookcase://1463 Obsidian Bookcase
					return "";
				case ItemID.HellboundBanner://1464 Hellbound Banner
					return "";
				case ItemID.HellHammerBanner://1465 Hell Hammer Banner
					return "";
				case ItemID.HelltowerBanner://1466 Helltower Banner
					return "";
				case ItemID.LostHopesofManBanner://1467 Lost Hopes of Man Banner
					return "";
				case ItemID.ObsidianWatcherBanner://1468 Obsidian Watcher Banner
					return "";
				case ItemID.LavaEruptsBanner://1469 Lava Erupts Banner
					return "";
				case ItemID.BlueDungeonBed://1470 Blue Dungeon Bed
					return "";
				case ItemID.GreenDungeonBed://1471 Green Dungeon Bed
					return "";
				case ItemID.PinkDungeonBed://1472 Pink Dungeon Bed
					return "";
				case ItemID.ObsidianBed://1473 Obsidian Bed
					return "";
				case ItemID.Waldo://1474 Waldo
					return "";
				case ItemID.Darkness://1475 Darkness
					return "";
				case ItemID.DarkSoulReaper://1476 Dark Soul Reaper
					return "";
				case ItemID.Land://1477 Land
					return "";
				case ItemID.TrappedGhost://1478 Trapped Ghost
					return "";
				case ItemID.DemonsEye://1479 Demon's Eye
					return "";
				case ItemID.FindingGold://1480 Finding Gold
					return "";
				case ItemID.FirstEncounter://1481 First Encounter
					return "";
				case ItemID.GoodMorning://1482 Good Morning
					return "";
				case ItemID.UndergroundReward://1483 Underground Reward
					return "";
				case ItemID.ThroughtheWindow://1484 Through the Window
					return "";
				case ItemID.PlaceAbovetheClouds://1485 Place Above the Clouds
					return "";
				case ItemID.DoNotStepontheGrass://1486 Do Not Step on the Grass
					return "";
				case ItemID.ColdWatersintheWhiteLand://1487 Cold Waters in the White Land
					return "";
				case ItemID.LightlessChasms://1488 Lightless Chasms
					return "";
				case ItemID.TheLandofDeceivingLooks://1489 The Land of Deceiving Looks
					return "";
				case ItemID.Daylight://1490 Daylight
					return "";
				case ItemID.SecretoftheSands://1491 Secret of the Sands
					return "";
				case ItemID.DeadlandComesAlive://1492 Deadland Comes Alive
					return "";
				case ItemID.EvilPresence://1493 Evil Presence
					return "";
				case ItemID.SkyGuardian://1494 Sky Guardian
					return "";
				case ItemID.AmericanExplosive://1495 American Explosive
					return "";
				case ItemID.Discover://1496 Discover
					return "";
				case ItemID.HandEarth://1497 Hand Earth
					return "";
				case ItemID.OldMiner://1498 Old Miner
					return "";
				case ItemID.Skelehead://1499 Skelehead
					return "";
				case ItemID.FacingtheCerebralMastermind://1500 Facing the Cerebral Mastermind
					return "";
				case ItemID.LakeofFire://1501 Lake of Fire
					return "";
				case ItemID.TrioSuperHeroes://1502 Trio Super Heroes
					return "";
				case ItemID.SpectreHood://1503 Spectre Hood
					return "";
				case ItemID.SpectreRobe://1504 Spectre Robe
					return "";
				case ItemID.SpectrePants://1505 Spectre Pants
					return "";
				case ItemID.SpectrePickaxe://1506 Spectre Pickaxe
					return "";
				case ItemID.SpectreHamaxe://1507 Spectre Hamaxe
					return "";
				case ItemID.Ectoplasm://1508 Ectoplasm
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/36/Ectoplasm.png/revision/latest?cb=20200516211757&format=original";
				case ItemID.GothicChair://1509 Gothic Chair
					return "";
				case ItemID.GothicTable://1510 Gothic Table
					return "";
				case ItemID.GothicWorkBench://1511 Gothic Work Bench
					return "";
				case ItemID.GothicBookcase://1512 Gothic Bookcase
					return "";
				case ItemID.PaladinsHammer://1513 Paladin's Hammer
					return "";
				case ItemID.SWATHelmet://1514 SWAT Helmet
					return "";
				case ItemID.BeeWings://1515 Bee Wings
					return "";
				case ItemID.GiantHarpyFeather://1516 Giant Harpy Feather
					return "";
				case ItemID.BoneFeather://1517 Bone Feather
					return "";
				case ItemID.FireFeather://1518 Fire Feather
					return "";
				case ItemID.IceFeather://1519 Ice Feather
					return "";
				case ItemID.BrokenBatWing://1520 Broken Bat Wing
					return "";
				case ItemID.TatteredBeeWing://1521 Tattered Bee Wing
					return "";
				case ItemID.LargeAmethyst://1522 Large Amethyst
					return "";
				case ItemID.LargeTopaz://1523 Large Topaz
					return "";
				case ItemID.LargeSapphire://1524 Large Sapphire
					return "";
				case ItemID.LargeEmerald://1525 Large Emerald
					return "";
				case ItemID.LargeRuby://1526 Large Ruby
					return "";
				case ItemID.LargeDiamond://1527 Large Diamond
					return "";
				case ItemID.JungleChest://1528 Jungle Chest
					return "";
				case ItemID.CorruptionChest://1529 Corruption Chest
					return "";
				case ItemID.CrimsonChest://1530 Crimson Chest
					return "";
				case ItemID.HallowedChest://1531 Hallowed Chest
					return "";
				case ItemID.FrozenChest://1532 Ice Chest
					return "";
				case ItemID.JungleKey://1533 Jungle Key
					return "";
				case ItemID.CorruptionKey://1534 Corruption Key
					return "";
				case ItemID.CrimsonKey://1535 Crimson Key
					return "";
				case ItemID.HallowedKey://1536 Hallowed Key
					return "";
				case ItemID.FrozenKey://1537 Frozen Key
					return "";
				case ItemID.ImpFace://1538 Imp Face
					return "";
				case ItemID.OminousPresence://1539 Ominous Presence
					return "";
				case ItemID.ShiningMoon://1540 Shining Moon
					return "";
				case ItemID.LivingGore://1541 Living Gore
					return "";
				case ItemID.FlowingMagma://1542 Flowing Magma
					return "";
				case ItemID.SpectrePaintbrush://1543 Spectre Paintbrush
					return "";
				case ItemID.SpectrePaintRoller://1544 Spectre Paint Roller
					return "";
				case ItemID.SpectrePaintScraper://1545 Spectre Paint Scraper
					return "";
				case ItemID.ShroomiteHeadgear://1546 Shroomite Headgear
					return "";
				case ItemID.ShroomiteMask://1547 Shroomite Mask
					return "";
				case ItemID.ShroomiteHelmet://1548 Shroomite Helmet
					return "";
				case ItemID.ShroomiteBreastplate://1549 Shroomite Breastplate
					return "";
				case ItemID.ShroomiteLeggings://1550 Shroomite Leggings
					return "";
				case ItemID.Autohammer://1551 Autohammer
					return "";
				case ItemID.ShroomiteBar://1552 Shroomite Bar
					return "";
				case ItemID.SDMG://1553 S.D.M.G.
					return "";
				case ItemID.CenxsTiara://1554 Cenx's Tiara
					return "";
				case ItemID.CenxsBreastplate://1555 Cenx's Breastplate
					return "";
				case ItemID.CenxsLeggings://1556 Cenx's Leggings
					return "";
				case ItemID.CrownosMask://1557 Crowno's Mask
					return "";
				case ItemID.CrownosBreastplate://1558 Crowno's Breastplate
					return "";
				case ItemID.CrownosLeggings://1559 Crowno's Leggings
					return "";
				case ItemID.WillsHelmet://1560 Will's Helmet
					return "";
				case ItemID.WillsBreastplate://1561 Will's Breastplate
					return "";
				case ItemID.WillsLeggings://1562 Will's Leggings
					return "";
				case ItemID.JimsHelmet://1563 Jim's Helmet
					return "";
				case ItemID.JimsBreastplate://1564 Jim's Breastplate
					return "";
				case ItemID.JimsLeggings://1565 Jim's Leggings
					return "";
				case ItemID.AaronsHelmet://1566 Aaron's Helmet
					return "";
				case ItemID.AaronsBreastplate://1567 Aaron's Breastplate
					return "";
				case ItemID.AaronsLeggings://1568 Aaron's Leggings
					return "";
				case ItemID.VampireKnives://1569 Vampire Knives
					return "";
				case ItemID.BrokenHeroSword://1570 Broken Hero Sword
					return "";
				case ItemID.ScourgeoftheCorruptor://1571 Scourge of the Corruptor
					return "";
				case ItemID.StaffoftheFrostHydra://1572 Staff of the Frost Hydra
					return "";
				case ItemID.TheCreationoftheGuide://1573 The Creation of the Guide
					return "";
				case ItemID.TheMerchant://1574 The Merchant
					return "";
				case ItemID.CrownoDevoursHisLunch://1575 Crowno Devours His Lunch
					return "";
				case ItemID.RareEnchantment://1576 Rare Enchantment
					return "";
				case ItemID.GloriousNight://1577 Glorious Night
					return "";
				case ItemID.SweetheartNecklace://1578 Sweetheart Necklace
					return "";
				case ItemID.FlurryBoots://1579 Flurry Boots
					return "";
				case ItemID.DTownsHelmet://1580 D-Town's Helmet
					return "";
				case ItemID.DTownsBreastplate://1581 D-Town's Breastplate
					return "";
				case ItemID.DTownsLeggings://1582 D-Town's Leggings
					return "";
				case ItemID.DTownsWings://1583 D-Town's Wings
					return "";
				case ItemID.WillsWings://1584 Will's Wings
					return "";
				case ItemID.CrownosWings://1585 Crowno's Wings
					return "";
				case ItemID.CenxsWings://1586 Cenx's Wings
					return "";
				case ItemID.CenxsDress://1587 Cenx's Dress
					return "";
				case ItemID.CenxsDressPants://1588 Cenx's Dress Pants
					return "";
				case ItemID.PalladiumColumn://1589 Palladium Column
					return "";
				case ItemID.PalladiumColumnWall://1590 Palladium Column Wall
					return "";
				case ItemID.BubblegumBlock://1591 Bubblegum Block
					return "";
				case ItemID.BubblegumBlockWall://1592 Bubblegum Block Wall
					return "";
				case ItemID.TitanstoneBlock://1593 Titanstone Block
					return "";
				case ItemID.TitanstoneBlockWall://1594 Titanstone Block Wall
					return "";
				case ItemID.MagicCuffs://1595 Magic Cuffs
					return "";
				case ItemID.MusicBoxSnow://1596 Music Box (Snow)
					return "";
				case ItemID.MusicBoxSpace://1597 Music Box (Space Night)
					return "";
				case ItemID.MusicBoxCrimson://1598 Music Box (Crimson)
					return "";
				case ItemID.MusicBoxBoss4://1599 Music Box (Boss 4)
					return "";
				case ItemID.MusicBoxAltOverworldDay://1600 Music Box (Alt Overworld Day)
					return "";
				case ItemID.MusicBoxRain://1601 Music Box (Rain)
					return "";
				case ItemID.MusicBoxIce://1602 Music Box (Ice)
					return "";
				case ItemID.MusicBoxDesert://1603 Music Box (Desert)
					return "";
				case ItemID.MusicBoxOcean://1604 Music Box (Ocean Day)
					return "";
				case ItemID.MusicBoxDungeon://1605 Music Box (Dungeon)
					return "";
				case ItemID.MusicBoxPlantera://1606 Music Box (Plantera)
					return "";
				case ItemID.MusicBoxBoss5://1607 Music Box (Boss 5)
					return "";
				case ItemID.MusicBoxTemple://1608 Music Box (Temple)
					return "";
				case ItemID.MusicBoxEclipse://1609 Music Box (Eclipse)
					return "";
				case ItemID.MusicBoxMushrooms://1610 Music Box (Mushrooms)
					return "";
				case ItemID.ButterflyDust://1611 Butterfly Dust
					return "";
				case ItemID.AnkhCharm://1612 Ankh Charm
					return "";
				case ItemID.AnkhShield://1613 Ankh Shield
					return "";
				case ItemID.BlueFlare://1614 Blue Flare
					return "";
				case ItemID.AnglerFishBanner://1615 Angler Fish Banner
					return "";
				case ItemID.AngryNimbusBanner://1616 Angry Nimbus Banner
					return "";
				case ItemID.AnomuraFungusBanner://1617 Anomura Fungus Banner
					return "";
				case ItemID.AntlionBanner://1618 Antlion Banner
					return "";
				case ItemID.ArapaimaBanner://1619 Arapaima Banner
					return "";
				case ItemID.ArmoredSkeletonBanner://1620 Armored Skeleton Banner
					return "";
				case ItemID.BatBanner://1621 Cave Bat Banner
					return "";
				case ItemID.BirdBanner://1622 Bird Banner
					return "";
				case ItemID.BlackRecluseBanner://1623 Black Recluse Banner
					return "";
				case ItemID.BloodFeederBanner://1624 Blood Feeder Banner
					return "";
				case ItemID.BloodJellyBanner://1625 Blood Jelly Banner
					return "";
				case ItemID.BloodCrawlerBanner://1626 Blood Crawler Banner
					return "";
				case ItemID.BoneSerpentBanner://1627 Bone Serpent Banner
					return "";
				case ItemID.BunnyBanner://1628 Bunny Banner
					return "";
				case ItemID.ChaosElementalBanner://1629 Chaos Elemental Banner
					return "";
				case ItemID.MimicBanner://1630 Mimic Banner
					return "";
				case ItemID.ClownBanner://1631 Clown Banner
					return "";
				case ItemID.CorruptBunnyBanner://1632 Corrupt Bunny Banner
					return "";
				case ItemID.CorruptGoldfishBanner://1633 Corrupt Goldfish Banner
					return "";
				case ItemID.CrabBanner://1634 Crab Banner
					return "";
				case ItemID.CrimeraBanner://1635 Crimera Banner
					return "";
				case ItemID.CrimsonAxeBanner://1636 Crimson Axe Banner
					return "";
				case ItemID.CursedHammerBanner://1637 Cursed Hammer Banner
					return "";
				case ItemID.DemonBanner://1638 Demon Banner
					return "";
				case ItemID.DemonEyeBanner://1639 Demon Eye Banner
					return "";
				case ItemID.DerplingBanner://1640 Derpling Banner
					return "";
				case ItemID.EaterofSoulsBanner://1641 Eater of Souls Banner
					return "";
				case ItemID.EnchantedSwordBanner://1642 Enchanted Sword Banner
					return "";
				case ItemID.ZombieEskimoBanner://1643 Frozen Zombie Banner
					return "";
				case ItemID.FaceMonsterBanner://1644 Face Monster Banner
					return "";
				case ItemID.FloatyGrossBanner://1645 Floaty Gross Banner
					return "";
				case ItemID.FlyingFishBanner://1646 Flying Fish Banner
					return "";
				case ItemID.FlyingSnakeBanner://1647 Flying Snake Banner
					return "";
				case ItemID.FrankensteinBanner://1648 Frankenstein Banner
					return "";
				case ItemID.FungiBulbBanner://1649 Fungi Bulb Banner
					return "";
				case ItemID.FungoFishBanner://1650 Fungo Fish Banner
					return "";
				case ItemID.GastropodBanner://1651 Gastropod Banner
					return "";
				case ItemID.GoblinThiefBanner://1652 Goblin Thief Banner
					return "";
				case ItemID.GoblinSorcererBanner://1653 Goblin Sorcerer Banner
					return "";
				case ItemID.GoblinPeonBanner://1654 Goblin Peon Banner
					return "";
				case ItemID.GoblinScoutBanner://1655 Goblin Scout Banner
					return "";
				case ItemID.GoblinWarriorBanner://1656 Goblin Warrior Banner
					return "";
				case ItemID.GoldfishBanner://1657 Goldfish Banner
					return "";
				case ItemID.HarpyBanner://1658 Harpy Banner
					return "";
				case ItemID.HellbatBanner://1659 Hellbat Banner
					return "";
				case ItemID.HerplingBanner://1660 Herpling Banner
					return "";
				case ItemID.HornetBanner://1661 Hornet Banner
					return "";
				case ItemID.IceElementalBanner://1662 Ice Elemental Banner
					return "";
				case ItemID.IcyMermanBanner://1663 Icy Merman Banner
					return "";
				case ItemID.FireImpBanner://1664 Fire Imp Banner
					return "";
				case ItemID.JellyfishBanner://1665 Blue Jellyfish Banner
					return "";
				case ItemID.JungleCreeperBanner://1666 Jungle Creeper Banner
					return "";
				case ItemID.LihzahrdBanner://1667 Lihzahrd Banner
					return "";
				case ItemID.ManEaterBanner://1668 Man Eater Banner
					return "";
				case ItemID.MeteorHeadBanner://1669 Meteor Head Banner
					return "";
				case ItemID.MothBanner://1670 Moth Banner
					return "";
				case ItemID.MummyBanner://1671 Mummy Banner
					return "";
				case ItemID.MushiLadybugBanner://1672 Mushi Ladybug Banner
					return "";
				case ItemID.ParrotBanner://1673 Parrot Banner
					return "";
				case ItemID.PigronBanner://1674 Pigron Banner
					return "";
				case ItemID.PiranhaBanner://1675 Piranha Banner
					return "";
				case ItemID.PirateBanner://1676 Pirate Deckhand Banner
					return "";
				case ItemID.PixieBanner://1677 Pixie Banner
					return "";
				case ItemID.RaincoatZombieBanner://1678 Raincoat Zombie Banner
					return "";
				case ItemID.ReaperBanner://1679 Reaper Banner
					return "";
				case ItemID.SharkBanner://1680 Shark Banner
					return "";
				case ItemID.SkeletonBanner://1681 Skeleton Banner
					return "";
				case ItemID.SkeletonMageBanner://1682 Dark Caster Banner
					return "";
				case ItemID.SlimeBanner://1683 Blue Slime Banner
					return "";
				case ItemID.SnowFlinxBanner://1684 Snow Flinx Banner
					return "";
				case ItemID.SpiderBanner://1685 Wall Creeper Banner
					return "";
				case ItemID.SporeZombieBanner://1686 Spore Zombie Banner
					return "";
				case ItemID.SwampThingBanner://1687 Swamp Thing Banner
					return "";
				case ItemID.TortoiseBanner://1688 Giant Tortoise Banner
					return "";
				case ItemID.ToxicSludgeBanner://1689 Toxic Sludge Banner
					return "";
				case ItemID.UmbrellaSlimeBanner://1690 Umbrella Slime Banner
					return "";
				case ItemID.UnicornBanner://1691 Unicorn Banner
					return "";
				case ItemID.VampireBanner://1692 Vampire Banner
					return "";
				case ItemID.VultureBanner://1693 Vulture Banner
					return "";
				case ItemID.NypmhBanner://1694 Nymph Banner
					return "";
				case ItemID.WerewolfBanner://1695 Werewolf Banner
					return "";
				case ItemID.WolfBanner://1696 Wolf Banner
					return "";
				case ItemID.WorldFeederBanner://1697 World Feeder Banner
					return "";
				case ItemID.WormBanner://1698 Worm Banner
					return "";
				case ItemID.WraithBanner://1699 Wraith Banner
					return "";
				case ItemID.WyvernBanner://1700 Wyvern Banner
					return "";
				case ItemID.ZombieBanner://1701 Zombie Banner
					return "";
				case ItemID.GlassPlatform://1702 Glass Platform
					return "";
				case ItemID.GlassChair://1703 Glass Chair
					return "";
				case ItemID.GoldenChair://1704 Golden Chair
					return "";
				case ItemID.GoldenToilet://1705 Golden Toilet
					return "";
				case ItemID.BarStool://1706 Bar Stool
					return "";
				case ItemID.HoneyChair://1707 Honey Chair
					return "";
				case ItemID.SteampunkChair://1708 Steampunk Chair
					return "";
				case ItemID.GlassDoor://1709 Glass Door
					return "";
				case ItemID.GoldenDoor://1710 Golden Door
					return "";
				case ItemID.HoneyDoor://1711 Honey Door
					return "";
				case ItemID.SteampunkDoor://1712 Steampunk Door
					return "";
				case ItemID.GlassTable://1713 Glass Table
					return "";
				case ItemID.BanquetTable://1714 Banquet Table
					return "";
				case ItemID.Bar://1715 Bar
					return "";
				case ItemID.GoldenTable://1716 Golden Table
					return "";
				case ItemID.HoneyTable://1717 Honey Table
					return "";
				case ItemID.SteampunkTable://1718 Steampunk Table
					return "";
				case ItemID.GlassBed://1719 Glass Bed
					return "";
				case ItemID.GoldenBed://1720 Golden Bed
					return "";
				case ItemID.HoneyBed://1721 Honey Bed
					return "";
				case ItemID.SteampunkBed://1722 Steampunk Bed
					return "";
				case ItemID.LivingWoodWall://1723 Living Wood Wall
					return "";
				case ItemID.FartinaJar://1724 Fart in a Jar
					return "";
				case ItemID.Pumpkin://1725 Pumpkin
					return "";
				case ItemID.PumpkinWall://1726 Pumpkin Wall
					return "";
				case ItemID.Hay://1727 Hay
					return "";
				case ItemID.HayWall://1728 Hay Wall
					return "";
				case ItemID.SpookyWood://1729 Spooky Wood
					return "";
				case ItemID.SpookyWoodWall://1730 Spooky Wood Wall
					return "";
				case ItemID.PumpkinHelmet://1731 Pumpkin Helmet
					return "";
				case ItemID.PumpkinBreastplate://1732 Pumpkin Breastplate
					return "";
				case ItemID.PumpkinLeggings://1733 Pumpkin Leggings
					return "";
				case ItemID.CandyApple://1734 Candy Apple
					return "";
				case ItemID.SoulCake://1735 Soul Cake
					return "";
				case ItemID.NurseHat://1736 Nurse Hat
					return "";
				case ItemID.NurseShirt://1737 Nurse Shirt
					return "";
				case ItemID.NursePants://1738 Nurse Pants
					return "";
				case ItemID.WizardsHat://1739 Wizard's Hat
					return "";
				case ItemID.GuyFawkesMask://1740 Guy Fawkes Mask
					return "";
				case ItemID.DyeTraderRobe://1741 Dye Trader Robe
					return "";
				case ItemID.SteampunkGoggles://1742 Steampunk Goggles
					return "";
				case ItemID.CyborgHelmet://1743 Cyborg Helmet
					return "";
				case ItemID.CyborgShirt://1744 Cyborg Shirt
					return "";
				case ItemID.CyborgPants://1745 Cyborg Pants
					return "";
				case ItemID.CreeperMask://1746 Creeper Mask
					return "";
				case ItemID.CreeperShirt://1747 Creeper Shirt
					return "";
				case ItemID.CreeperPants://1748 Creeper Pants
					return "";
				case ItemID.CatMask://1749 Cat Mask
					return "";
				case ItemID.CatShirt://1750 Cat Shirt
					return "";
				case ItemID.CatPants://1751 Cat Pants
					return "";
				case ItemID.GhostMask://1752 Ghost Mask
					return "";
				case ItemID.GhostShirt://1753 Ghost Shirt
					return "";
				case ItemID.PumpkinMask://1754 Pumpkin Mask
					return "";
				case ItemID.PumpkinShirt://1755 Pumpkin Shirt
					return "";
				case ItemID.PumpkinPants://1756 Pumpkin Pants
					return "";
				case ItemID.RobotMask://1757 Robot Mask
					return "";
				case ItemID.RobotShirt://1758 Robot Shirt
					return "";
				case ItemID.RobotPants://1759 Robot Pants
					return "";
				case ItemID.UnicornMask://1760 Unicorn Mask
					return "";
				case ItemID.UnicornShirt://1761 Unicorn Shirt
					return "";
				case ItemID.UnicornPants://1762 Unicorn Pants
					return "";
				case ItemID.VampireMask://1763 Vampire Mask
					return "";
				case ItemID.VampireShirt://1764 Vampire Shirt
					return "";
				case ItemID.VampirePants://1765 Vampire Pants
					return "";
				case ItemID.WitchHat://1766 Witch Hat
					return "";
				case ItemID.LeprechaunHat://1767 Leprechaun Hat
					return "";
				case ItemID.LeprechaunShirt://1768 Leprechaun Shirt
					return "";
				case ItemID.LeprechaunPants://1769 Leprechaun Pants
					return "";
				case ItemID.PixieShirt://1770 Pixie Shirt
					return "";
				case ItemID.PixiePants://1771 Pixie Pants
					return "";
				case ItemID.PrincessHat://1772 Princess Hat
					return "";
				case ItemID.PrincessDressNew://1773 Princess Dress
					return "";
				case ItemID.GoodieBag://1774 Goodie Bag
					return "";
				case ItemID.WitchDress://1775 Witch Dress
					return "";
				case ItemID.WitchBoots://1776 Witch Boots
					return "";
				case ItemID.BrideofFrankensteinMask://1777 Bride of Frankenstein Mask
					return "";
				case ItemID.BrideofFrankensteinDress://1778 Bride of Frankenstein Dress
					return "";
				case ItemID.KarateTortoiseMask://1779 Karate Tortoise Mask
					return "";
				case ItemID.KarateTortoiseShirt://1780 Karate Tortoise Shirt
					return "";
				case ItemID.KarateTortoisePants://1781 Karate Tortoise Pants
					return "";
				case ItemID.CandyCornRifle://1782 Candy Corn Rifle
					return "";
				case ItemID.CandyCorn://1783 Candy Corn
					return "";
				case ItemID.JackOLanternLauncher://1784 Jack 'O Lantern Launcher
					return "";
				case ItemID.ExplosiveJackOLantern://1785 Explosive Jack 'O Lantern
					return "";
				case ItemID.Sickle://1786 Sickle
					return "";
				case ItemID.PumpkinPie://1787 Pumpkin Pie
					return "";
				case ItemID.ScarecrowHat://1788 Scarecrow Hat
					return "";
				case ItemID.ScarecrowShirt://1789 Scarecrow Shirt
					return "";
				case ItemID.ScarecrowPants://1790 Scarecrow Pants
					return "";
				case ItemID.Cauldron://1791 Cauldron
					return "";
				case ItemID.PumpkinChair://1792 Pumpkin Chair
					return "";
				case ItemID.PumpkinDoor://1793 Pumpkin Door
					return "";
				case ItemID.PumpkinTable://1794 Pumpkin Table
					return "";
				case ItemID.PumpkinWorkBench://1795 Pumpkin Work Bench
					return "";
				case ItemID.PumpkinPlatform://1796 Pumpkin Platform
					return "";
				case ItemID.TatteredFairyWings://1797 Tattered Fairy Wings
					return "";
				case ItemID.SpiderEgg://1798 Spider Egg
					return "";
				case ItemID.MagicalPumpkinSeed://1799 Magical Pumpkin Seed
					return "";
				case ItemID.BatHook://1800 Bat Hook
					return "";
				case ItemID.BatScepter://1801 Bat Scepter
					return "";
				case ItemID.RavenStaff://1802 Raven Staff
					return "";
				case ItemID.JungleKeyMold://1803 Jungle Key
					return "";
				case ItemID.CorruptionKeyMold://1804 Corruption Key
					return "";
				case ItemID.CrimsonKeyMold://1805 Crimson Key
					return "";
				case ItemID.HallowedKeyMold://1806 Hallowed Key
					return "";
				case ItemID.FrozenKeyMold://1807 Frozen Key
					return "";
				case ItemID.HangingJackOLantern://1808 Hanging Jack 'O Lantern
					return "";
				case ItemID.RottenEgg://1809 Rotten Egg
					return "";
				case ItemID.UnluckyYarn://1810 Unlucky Yarn
					return "";
				case ItemID.BlackFairyDust://1811 Black Fairy Dust
					return "";
				case ItemID.Jackelier://1812 Jackelier
					return "";
				case ItemID.JackOLantern://1813 Jack 'O Lantern
					return "";
				case ItemID.SpookyChair://1814 Spooky Chair
					return "";
				case ItemID.SpookyDoor://1815 Spooky Door
					return "";
				case ItemID.SpookyTable://1816 Spooky Table
					return "";
				case ItemID.SpookyWorkBench://1817 Spooky Work Bench
					return "";
				case ItemID.SpookyPlatform://1818 Spooky Wood Platform
					return "";
				case ItemID.ReaperHood://1819 Reaper Hood
					return "";
				case ItemID.ReaperRobe://1820 Reaper Robe
					return "";
				case ItemID.FoxMask://1821 Fox Mask
					return "";
				case ItemID.FoxShirt://1822 Fox Shirt
					return "";
				case ItemID.FoxPants://1823 Fox Pants
					return "";
				case ItemID.CatEars://1824 Cat Ears
					return "";
				case ItemID.BloodyMachete://1825 Bloody Machete
					return "";
				case ItemID.TheHorsemansBlade://1826 The Horseman's Blade
					return "";
				case ItemID.BladedGlove://1827 Bladed Glove
					return "";
				case ItemID.PumpkinSeed://1828 Pumpkin Seed
					return "";
				case ItemID.SpookyHook://1829 Spooky Hook
					return "";
				case ItemID.SpookyWings://1830 Spooky Wings
					return "";
				case ItemID.SpookyTwig://1831 Spooky Twig
					return "";
				case ItemID.SpookyHelmet://1832 Spooky Helmet
					return "";
				case ItemID.SpookyBreastplate://1833 Spooky Breastplate
					return "";
				case ItemID.SpookyLeggings://1834 Spooky Leggings
					return "";
				case ItemID.StakeLauncher://1835 Stake Launcher
					return "";
				case ItemID.Stake://1836 Stake
					return "";
				case ItemID.CursedSapling://1837 Cursed Sapling
					return "";
				case ItemID.SpaceCreatureMask://1838 Space Creature Mask
					return "";
				case ItemID.SpaceCreatureShirt://1839 Space Creature Shirt
					return "";
				case ItemID.SpaceCreaturePants://1840 Space Creature Pants
					return "";
				case ItemID.WolfMask://1841 Wolf Mask
					return "";
				case ItemID.WolfShirt://1842 Wolf Shirt
					return "";
				case ItemID.WolfPants://1843 Wolf Pants
					return "";
				case ItemID.PumpkinMoonMedallion://1844 Pumpkin Moon Medallion
					return "";
				case ItemID.NecromanticScroll://1845 Necromantic Scroll
					return "";
				case ItemID.JackingSkeletron://1846 Jacking Skeletron
					return "";
				case ItemID.BitterHarvest://1847 Bitter Harvest
					return "";
				case ItemID.BloodMoonCountess://1848 Blood Moon Countess
					return "";
				case ItemID.HallowsEve://1849 Hallow's Eve
					return "";
				case ItemID.MorbidCuriosity://1850 Morbid Curiosity
					return "";
				case ItemID.TreasureHunterShirt://1851 Treasure Hunter Shirt
					return "";
				case ItemID.TreasureHunterPants://1852 Treasure Hunter Pants
					return "";
				case ItemID.DryadCoverings://1853 Dryad Coverings
					return "";
				case ItemID.DryadLoincloth://1854 Dryad Loincloth
					return "";
				case ItemID.MourningWoodTrophy://1855 Mourning Wood Trophy
					return "";
				case ItemID.PumpkingTrophy://1856 Pumpking Trophy
					return "";
				case ItemID.JackOLanternMask://1857 Jack 'O Lantern Mask
					return "";
				case ItemID.SniperScope://1858 Sniper Scope
					return "";
				case ItemID.HeartLantern://1859 Heart Lantern
					return "";
				case ItemID.JellyfishDivingGear://1860 Jellyfish Diving Gear
					return "";
				case ItemID.ArcticDivingGear://1861 Arctic Diving Gear
					return "";
				case ItemID.FrostsparkBoots://1862 Frostspark Boots
					return "";
				case ItemID.FartInABalloon://1863 Fart in a Balloon
					return "";
				case ItemID.PapyrusScarab://1864 Papyrus Scarab
					return "";
				case ItemID.CelestialStone://1865 Celestial Stone
					return "";
				case ItemID.Hoverboard://1866 Hoverboard
					return "";
				case ItemID.CandyCane://1867 Candy Cane
					return "";
				case ItemID.SugarPlum://1868 Sugar Plum
					return "";
				case ItemID.Present://1869 Present
					return "";
				case ItemID.RedRyder://1870 Red Ryder
					return "";
				case ItemID.FestiveWings://1871 Festive Wings
					return "";
				case ItemID.PineTreeBlock://1872 Pine Tree Block
					return "";
				case ItemID.ChristmasTree://1873 Christmas Tree
					return "";
				case ItemID.StarTopper1://1874 Star Topper 1
					return "";
				case ItemID.StarTopper2://1875 Star Topper 2
					return "";
				case ItemID.StarTopper3://1876 Star Topper 3
					return "";
				case ItemID.BowTopper://1877 Bow Topper
					return "";
				case ItemID.WhiteGarland://1878 White Garland
					return "";
				case ItemID.WhiteAndRedGarland://1879 White and Red Garland
					return "";
				case ItemID.RedGardland://1880 Red Garland
					return "";
				case ItemID.RedAndGreenGardland://1881 Red and Green Garland
					return "";
				case ItemID.GreenGardland://1882 Green Garland
					return "";
				case ItemID.GreenAndWhiteGarland://1883 Green and White Garland
					return "";
				case ItemID.MulticoloredBulb://1884 Multicolored Bulb
					return "";
				case ItemID.RedBulb://1885 Red Bulb
					return "";
				case ItemID.YellowBulb://1886 Yellow Bulb
					return "";
				case ItemID.GreenBulb://1887 Green Bulb
					return "";
				case ItemID.RedAndGreenBulb://1888 Red and Green Bulb
					return "";
				case ItemID.YellowAndGreenBulb://1889 Yellow and Green Bulb
					return "";
				case ItemID.RedAndYellowBulb://1890 Red and Yellow Bulb
					return "";
				case ItemID.WhiteBulb://1891 White Bulb
					return "";
				case ItemID.WhiteAndRedBulb://1892 White and Red Bulb
					return "";
				case ItemID.WhiteAndYellowBulb://1893 White and Yellow Bulb
					return "";
				case ItemID.WhiteAndGreenBulb://1894 White and Green Bulb
					return "";
				case ItemID.MulticoloredLights://1895 Multicolored Lights
					return "";
				case ItemID.RedLights://1896 Red Lights
					return "";
				case ItemID.GreenLights://1897 Green Lights
					return "";
				case ItemID.BlueLights://1898 Blue Lights
					return "";
				case ItemID.YellowLights://1899 Yellow Lights
					return "";
				case ItemID.RedAndYellowLights://1900 Red and Yellow Lights
					return "";
				case ItemID.RedAndGreenLights://1901 Red and Green Lights
					return "";
				case ItemID.YellowAndGreenLights://1902 Yellow and Green Lights
					return "";
				case ItemID.BlueAndGreenLights://1903 Blue and Green Lights
					return "";
				case ItemID.RedAndBlueLights://1904 Red and Blue Lights
					return "";
				case ItemID.BlueAndYellowLights://1905 Blue and Yellow Lights
					return "";
				case ItemID.GiantBow://1906 Giant Bow
					return "";
				case ItemID.ReindeerAntlers://1907 Reindeer Antlers
					return "";
				case ItemID.Holly://1908 Holly
					return "";
				case ItemID.CandyCaneSword://1909 Candy Cane Sword
					return "";
				case ItemID.ElfMelter://1910 Elf Melter
					return "";
				case ItemID.ChristmasPudding://1911 Christmas Pudding
					return "";
				case ItemID.Eggnog://1912 Eggnog
					return "";
				case ItemID.StarAnise://1913 Star Anise
					return "";
				case ItemID.ReindeerBells://1914 Reindeer Bells
					return "";
				case ItemID.CandyCaneHook://1915 Candy Cane Hook
					return "";
				case ItemID.ChristmasHook://1916 Christmas Hook
					return "";
				case ItemID.CnadyCanePickaxe://1917 Candy Cane Pickaxe
					return "";
				case ItemID.FruitcakeChakram://1918 Fruitcake Chakram
					return "";
				case ItemID.SugarCookie://1919 Sugar Cookie
					return "";
				case ItemID.GingerbreadCookie://1920 Gingerbread Cookie
					return "";
				case ItemID.HandWarmer://1921 Hand Warmer
					return "";
				case ItemID.Coal://1922 Coal
					return "";
				case ItemID.Toolbox://1923 Toolbox
					return "";
				case ItemID.PineDoor://1924 Pine Door
					return "";
				case ItemID.PineChair://1925 Pine Chair
					return "";
				case ItemID.PineTable://1926 Pine Table
					return "";
				case ItemID.DogWhistle://1927 Dog Whistle
					return "";
				case ItemID.ChristmasTreeSword://1928 Christmas Tree Sword
					return "";
				case ItemID.ChainGun://1929 Chain Gun
					return "";
				case ItemID.Razorpine://1930 Razorpine
					return "";
				case ItemID.BlizzardStaff://1931 Blizzard Staff
					return "";
				case ItemID.MrsClauseHat://1932 Mrs. Claus Hat
					return "";
				case ItemID.MrsClauseShirt://1933 Mrs. Claus Shirt
					return "";
				case ItemID.MrsClauseHeels://1934 Mrs. Claus Heels
					return "";
				case ItemID.ParkaHood://1935 Parka Hood
					return "";
				case ItemID.ParkaCoat://1936 Parka Coat
					return "";
				case ItemID.ParkaPants://1937 Parka Pants
					return "";
				case ItemID.SnowHat://1938 Snow Hat
					return "";
				case ItemID.UglySweater://1939 Ugly Sweater
					return "";
				case ItemID.TreeMask://1940 Tree Mask
					return "";
				case ItemID.TreeShirt://1941 Tree Shirt
					return "";
				case ItemID.TreeTrunks://1942 Tree Trunks
					return "";
				case ItemID.ElfHat://1943 Elf Hat
					return "";
				case ItemID.ElfShirt://1944 Elf Shirt
					return "";
				case ItemID.ElfPants://1945 Elf Pants
					return "";
				case ItemID.SnowmanCannon://1946 Snowman Cannon
					return "";
				case ItemID.NorthPole://1947 North Pole
					return "";
				case ItemID.ChristmasTreeWallpaper://1948 Christmas Tree Wallpaper
					return "";
				case ItemID.OrnamentWallpaper://1949 Ornament Wallpaper
					return "";
				case ItemID.CandyCaneWallpaper://1950 Candy Cane Wallpaper
					return "";
				case ItemID.FestiveWallpaper://1951 Festive Wallpaper
					return "";
				case ItemID.StarsWallpaper://1952 Stars Wallpaper
					return "";
				case ItemID.SquigglesWallpaper://1953 Squiggles Wallpaper
					return "";
				case ItemID.SnowflakeWallpaper://1954 Snowflake Wallpaper
					return "";
				case ItemID.KrampusHornWallpaper://1955 Krampus Horn Wallpaper
					return "";
				case ItemID.BluegreenWallpaper://1956 Bluegreen Wallpaper
					return "";
				case ItemID.GrinchFingerWallpaper://1957 Grinch Finger Wallpaper
					return "";
				case ItemID.NaughtyPresent://1958 Naughty Present
					return "";
				case ItemID.BabyGrinchMischiefWhistle://1959 Baby Grinch's Mischief Whistle
					return "";
				case ItemID.IceQueenTrophy://1960 Ice Queen Trophy
					return "";
				case ItemID.SantaNK1Trophy://1961 Santa-NK1 Trophy
					return "";
				case ItemID.EverscreamTrophy://1962 Everscream Trophy
					return "";
				case ItemID.MusicBoxPumpkinMoon://1963 Music Box (Pumpkin Moon)
					return "";
				case ItemID.MusicBoxAltUnderground://1964 Music Box (Alt Underground)
					return "";
				case ItemID.MusicBoxFrostMoon://1965 Music Box (Frost Moon)
					return "";
				case ItemID.BrownPaint://1966 Brown Paint
					return "";
				case ItemID.ShadowPaint://1967 Shadow Paint
					return "";
				case ItemID.NegativePaint://1968 Negative Paint
					return "";
				case ItemID.TeamDye://1969 Team Dye
					return "";
				case ItemID.AmethystGemsparkBlock://1970 Amethyst Gemspark Block
					return "";
				case ItemID.TopazGemsparkBlock://1971 Topaz Gemspark Block
					return "";
				case ItemID.SapphireGemsparkBlock://1972 Sapphire Gemspark Block
					return "";
				case ItemID.EmeraldGemsparkBlock://1973 Emerald Gemspark Block
					return "";
				case ItemID.RubyGemsparkBlock://1974 Ruby Gemspark Block
					return "";
				case ItemID.DiamondGemsparkBlock://1975 Diamond Gemspark Block
					return "";
				case ItemID.AmberGemsparkBlock://1976 Amber Gemspark Block
					return "";
				case ItemID.LifeHairDye://1977 Life Hair Dye
					return "";
				case ItemID.ManaHairDye://1978 Mana Hair Dye
					return "";
				case ItemID.DepthHairDye://1979 Depth Hair Dye
					return "";
				case ItemID.MoneyHairDye://1980 Money Hair Dye
					return "";
				case ItemID.TimeHairDye://1981 Time Hair Dye
					return "";
				case ItemID.TeamHairDye://1982 Team Hair Dye
					return "";
				case ItemID.BiomeHairDye://1983 Biome Hair Dye
					return "";
				case ItemID.PartyHairDye://1984 Party Hair Dye
					return "";
				case ItemID.RainbowHairDye://1985 Rainbow Hair Dye
					return "";
				case ItemID.SpeedHairDye://1986 Speed Hair Dye
					return "";
				case ItemID.AngelHalo://1987 Angel Halo
					return "";
				case ItemID.Fez://1988 Fez
					return "";
				case ItemID.Womannquin://1989 Womannequin
					return "";
				case ItemID.HairDyeRemover://1990 Hair Dye Remover
					return "";
				case ItemID.BugNet://1991 Bug Net
					return "";
				case ItemID.Firefly://1992 Firefly
					return "";
				case ItemID.FireflyinaBottle://1993 Firefly in a Bottle
					return "";
				case ItemID.MonarchButterfly://1994 Monarch Butterfly
					return "";
				case ItemID.PurpleEmperorButterfly://1995 Purple Emperor Butterfly
					return "";
				case ItemID.RedAdmiralButterfly://1996 Red Admiral Butterfly
					return "";
				case ItemID.UlyssesButterfly://1997 Ulysses Butterfly
					return "";
				case ItemID.SulphurButterfly://1998 Sulphur Butterfly
					return "";
				case ItemID.TreeNymphButterfly://1999 Tree Nymph Butterfly
					return "";
				case ItemID.ZebraSwallowtailButterfly://2000 Zebra Swallowtail Butterfly
					return "";
				case ItemID.JuliaButterfly://2001 Julia Butterfly
					return "";
				case ItemID.Worm://2002 Worm
					return "";
				case ItemID.Mouse://2003 Mouse
					return "";
				case ItemID.LightningBug://2004 Lightning Bug
					return "";
				case ItemID.LightningBuginaBottle://2005 Lightning Bug in a Bottle
					return "";
				case ItemID.Snail://2006 Snail
					return "";
				case ItemID.GlowingSnail://2007 Glowing Snail
					return "";
				case ItemID.FancyGreyWallpaper://2008 Fancy Gray Wallpaper
					return "";
				case ItemID.IceFloeWallpaper://2009 Ice Floe Wallpaper
					return "";
				case ItemID.MusicWallpaper://2010 Music Wallpaper
					return "";
				case ItemID.PurpleRainWallpaper://2011 Purple Rain Wallpaper
					return "";
				case ItemID.RainbowWallpaper://2012 Rainbow Wallpaper
					return "";
				case ItemID.SparkleStoneWallpaper://2013 Sparkle Stone Wallpaper
					return "";
				case ItemID.StarlitHeavenWallpaper://2014 Starlit Heaven Wallpaper
					return "";
				case ItemID.Bird://2015 Bird
					return "";
				case ItemID.BlueJay://2016 Blue Jay
					return "";
				case ItemID.Cardinal://2017 Cardinal
					return "";
				case ItemID.Squirrel://2018 Squirrel
					return "";
				case ItemID.Bunny://2019 Bunny
					return "";
				case ItemID.CactusBookcase://2020 Cactus Bookcase
					return "";
				case ItemID.EbonwoodBookcase://2021 Ebonwood Bookcase
					return "";
				case ItemID.FleshBookcase://2022 Flesh Bookcase
					return "";
				case ItemID.HoneyBookcase://2023 Honey Bookcase
					return "";
				case ItemID.SteampunkBookcase://2024 Steampunk Bookcase
					return "";
				case ItemID.GlassBookcase://2025 Glass Bookcase
					return "";
				case ItemID.RichMahoganyBookcase://2026 Rich Mahogany Bookcase
					return "";
				case ItemID.PearlwoodBookcase://2027 Pearlwood Bookcase
					return "";
				case ItemID.SpookyBookcase://2028 Spooky Bookcase
					return "";
				case ItemID.SkywareBookcase://2029 Skyware Bookcase
					return "";
				case ItemID.LihzahrdBookcase://2030 Lihzahrd Bookcase
					return "";
				case ItemID.FrozenBookcase://2031 Frozen Bookcase
					return "";
				case ItemID.CactusLantern://2032 Cactus Lantern
					return "";
				case ItemID.EbonwoodLantern://2033 Ebonwood Lantern
					return "";
				case ItemID.FleshLantern://2034 Flesh Lantern
					return "";
				case ItemID.HoneyLantern://2035 Honey Lantern
					return "";
				case ItemID.SteampunkLantern://2036 Steampunk Lantern
					return "";
				case ItemID.GlassLantern://2037 Glass Lantern
					return "";
				case ItemID.RichMahoganyLantern://2038 Rich Mahogany Lantern
					return "";
				case ItemID.PearlwoodLantern://2039 Pearlwood Lantern
					return "";
				case ItemID.FrozenLantern://2040 Frozen Lantern
					return "";
				case ItemID.LihzahrdLantern://2041 Lihzahrd Lantern
					return "";
				case ItemID.SkywareLantern://2042 Skyware Lantern
					return "";
				case ItemID.SpookyLantern://2043 Spooky Lantern
					return "";
				case ItemID.FrozenDoor://2044 Frozen Door
					return "";
				case ItemID.CactusCandle://2045 Cactus Candle
					return "";
				case ItemID.EbonwoodCandle://2046 Ebonwood Candle
					return "";
				case ItemID.FleshCandle://2047 Flesh Candle
					return "";
				case ItemID.GlassCandle://2048 Glass Candle
					return "";
				case ItemID.FrozenCandle://2049 Frozen Candle
					return "";
				case ItemID.RichMahoganyCandle://2050 Rich Mahogany Candle
					return "";
				case ItemID.PearlwoodCandle://2051 Pearlwood Candle
					return "";
				case ItemID.LihzahrdCandle://2052 Lihzahrd Candle
					return "";
				case ItemID.SkywareCandle://2053 Skyware Candle
					return "";
				case ItemID.PumpkinCandle://2054 Pumpkin Candle
					return "";
				case ItemID.CactusChandelier://2055 Cactus Chandelier
					return "";
				case ItemID.EbonwoodChandelier://2056 Ebonwood Chandelier
					return "";
				case ItemID.FleshChandelier://2057 Flesh Chandelier
					return "";
				case ItemID.HoneyChandelier://2058 Honey Chandelier
					return "";
				case ItemID.FrozenChandelier://2059 Frozen Chandelier
					return "";
				case ItemID.RichMahoganyChandelier://2060 Rich Mahogany Chandelier
					return "";
				case ItemID.PearlwoodChandelier://2061 Pearlwood Chandelier
					return "";
				case ItemID.LihzahrdChandelier://2062 Lihzahrd Chandelier
					return "";
				case ItemID.SkywareChandelier://2063 Skyware Chandelier
					return "";
				case ItemID.SpookyChandelier://2064 Spooky Chandelier
					return "";
				case ItemID.GlassChandelier://2065 Glass Chandelier
					return "";
				case ItemID.CactusBed://2066 Cactus Bed
					return "";
				case ItemID.FleshBed://2067 Flesh Bed
					return "";
				case ItemID.FrozenBed://2068 Frozen Bed
					return "";
				case ItemID.LihzahrdBed://2069 Lihzahrd Bed
					return "";
				case ItemID.SkywareBed://2070 Skyware Bed
					return "";
				case ItemID.SpookyBed://2071 Spooky Bed
					return "";
				case ItemID.CactusBathtub://2072 Cactus Bathtub
					return "";
				case ItemID.EbonwoodBathtub://2073 Ebonwood Bathtub
					return "";
				case ItemID.FleshBathtub://2074 Flesh Bathtub
					return "";
				case ItemID.GlassBathtub://2075 Glass Bathtub
					return "";
				case ItemID.FrozenBathtub://2076 Frozen Bathtub
					return "";
				case ItemID.RichMahoganyBathtub://2077 Rich Mahogany Bathtub
					return "";
				case ItemID.PearlwoodBathtub://2078 Pearlwood Bathtub
					return "";
				case ItemID.LihzahrdBathtub://2079 Lihzahrd Bathtub
					return "";
				case ItemID.SkywareBathtub://2080 Skyware Bathtub
					return "";
				case ItemID.SpookyBathtub://2081 Spooky Bathtub
					return "";
				case ItemID.CactusLamp://2082 Cactus Lamp
					return "";
				case ItemID.EbonwoodLamp://2083 Ebonwood Lamp
					return "";
				case ItemID.FleshLamp://2084 Flesh Lamp
					return "";
				case ItemID.GlassLamp://2085 Glass Lamp
					return "";
				case ItemID.FrozenLamp://2086 Frozen Lamp
					return "";
				case ItemID.RichMahoganyLamp://2087 Rich Mahogany Lamp
					return "";
				case ItemID.PearlwoodLamp://2088 Pearlwood Lamp
					return "";
				case ItemID.LihzahrdLamp://2089 Lihzahrd Lamp
					return "";
				case ItemID.SkywareLamp://2090 Skyware Lamp
					return "";
				case ItemID.SpookyLamp://2091 Spooky Lamp
					return "";
				case ItemID.CactusCandelabra://2092 Cactus Candelabra
					return "";
				case ItemID.EbonwoodCandelabra://2093 Ebonwood Candelabra
					return "";
				case ItemID.FleshCandelabra://2094 Flesh Candelabra
					return "";
				case ItemID.HoneyCandelabra://2095 Honey Candelabra
					return "";
				case ItemID.SteampunkCandelabra://2096 Steampunk Candelabra
					return "";
				case ItemID.GlassCandelabra://2097 Glass Candelabra
					return "";
				case ItemID.RichMahoganyCandelabra://2098 Rich Mahogany Candelabra
					return "";
				case ItemID.PearlwoodCandelabra://2099 Pearlwood Candelabra
					return "";
				case ItemID.FrozenCandelabra://2100 Frozen Candelabra
					return "";
				case ItemID.LihzahrdCandelabra://2101 Lihzahrd Candelabra
					return "";
				case ItemID.SkywareCandelabra://2102 Skyware Candelabra
					return "";
				case ItemID.SpookyCandelabra://2103 Spooky Candelabra
					return "";
				case ItemID.BrainMask://2104 Brain of Cthulhu Mask
					return "";
				case ItemID.FleshMask://2105 Wall of Flesh Mask
					return "";
				case ItemID.TwinMask://2106 Twin Mask
					return "";
				case ItemID.SkeletronPrimeMask://2107 Skeletron Prime Mask
					return "";
				case ItemID.BeeMask://2108 Queen Bee Mask
					return "";
				case ItemID.PlanteraMask://2109 Plantera Mask
					return "";
				case ItemID.GolemMask://2110 Golem Mask
					return "";
				case ItemID.EaterMask://2111 Eater of Worlds Mask
					return "";
				case ItemID.EyeMask://2112 Eye of Cthulhu Mask
					return "";
				case ItemID.DestroyerMask://2113 Destroyer Mask
					return "";
				case ItemID.BlacksmithRack://2114 Blacksmith Rack
					return "";
				case ItemID.CarpentryRack://2115 Carpentry Rack
					return "";
				case ItemID.HelmetRack://2116 Helmet Rack
					return "";
				case ItemID.SpearRack://2117 Spear Rack
					return "";
				case ItemID.SwordRack://2118 Sword Rack
					return "";
				case ItemID.StoneSlab://2119 Stone Slab
					return "";
				case ItemID.SandstoneSlab://2120 Sandstone Slab
					return "";
				case ItemID.Frog://2121 Frog
					return "";
				case ItemID.MallardDuck://2122 Mallard Duck
					return "";
				case ItemID.Duck://2123 Duck
					return "";
				case ItemID.HoneyBathtub://2124 Honey Bathtub
					return "";
				case ItemID.SteampunkBathtub://2125 Steampunk Bathtub
					return "";
				case ItemID.LivingWoodBathtub://2126 Living Wood Bathtub
					return "";
				case ItemID.ShadewoodBathtub://2127 Shadewood Bathtub
					return "";
				case ItemID.BoneBathtub://2128 Bone Bathtub
					return "";
				case ItemID.HoneyLamp://2129 Honey Lamp
					return "";
				case ItemID.SteampunkLamp://2130 Steampunk Lamp
					return "";
				case ItemID.LivingWoodLamp://2131 Living Wood Lamp
					return "";
				case ItemID.ShadewoodLamp://2132 Shadewood Lamp
					return "";
				case ItemID.GoldenLamp://2133 Golden Lamp
					return "";
				case ItemID.BoneLamp://2134 Bone Lamp
					return "";
				case ItemID.LivingWoodBookcase://2135 Living Wood Bookcase
					return "";
				case ItemID.ShadewoodBookcase://2136 Shadewood Bookcase
					return "";
				case ItemID.GoldenBookcase://2137 Golden Bookcase
					return "";
				case ItemID.BoneBookcase://2138 Bone Bookcase
					return "";
				case ItemID.LivingWoodBed://2139 Living Wood Bed
					return "";
				case ItemID.BoneBed://2140 Bone Bed
					return "";
				case ItemID.LivingWoodChandelier://2141 Living Wood Chandelier
					return "";
				case ItemID.ShadewoodChandelier://2142 Shadewood Chandelier
					return "";
				case ItemID.GoldenChandelier://2143 Golden Chandelier
					return "";
				case ItemID.BoneChandelier://2144 Bone Chandelier
					return "";
				case ItemID.LivingWoodLantern://2145 Living Wood Lantern
					return "";
				case ItemID.ShadewoodLantern://2146 Shadewood Lantern
					return "";
				case ItemID.GoldenLantern://2147 Golden Lantern
					return "";
				case ItemID.BoneLantern://2148 Bone Lantern
					return "";
				case ItemID.LivingWoodCandelabra://2149 Living Wood Candelabra
					return "";
				case ItemID.ShadewoodCandelabra://2150 Shadewood Candelabra
					return "";
				case ItemID.GoldenCandelabra://2151 Golden Candelabra
					return "";
				case ItemID.BoneCandelabra://2152 Bone Candelabra
					return "";
				case ItemID.LivingWoodCandle://2153 Living Wood Candle
					return "";
				case ItemID.ShadewoodCandle://2154 Shadewood Candle
					return "";
				case ItemID.GoldenCandle://2155 Golden Candle
					return "";
				case ItemID.BlackScorpion://2156 Black Scorpion
					return "";
				case ItemID.Scorpion://2157 Scorpion
					return "";
				case ItemID.BubbleWallpaper://2158 Bubble Wallpaper
					return "";
				case ItemID.CopperPipeWallpaper://2159 Copper Pipe Wallpaper
					return "";
				case ItemID.DuckyWallpaper://2160 Ducky Wallpaper
					return "";
				case ItemID.FrostCore://2161 Frost Core
					return "";
				case ItemID.BunnyCage://2162 Bunny Cage
					return "";
				case ItemID.SquirrelCage://2163 Squirrel Cage
					return "";
				case ItemID.MallardDuckCage://2164 Mallard Duck Cage
					return "";
				case ItemID.DuckCage://2165 Duck Cage
					return "";
				case ItemID.BirdCage://2166 Bird Cage
					return "";
				case ItemID.BlueJayCage://2167 Blue Jay Cage
					return "";
				case ItemID.CardinalCage://2168 Cardinal Cage
					return "";
				case ItemID.WaterfallWall://2169 Waterfall Wall
					return "";
				case ItemID.LavafallWall://2170 Lavafall Wall
					return "";
				case ItemID.CrimsonSeeds://2171 Crimson Seeds
					return "";
				case ItemID.HeavyWorkBench://2172 Heavy Work Bench
					return "";
				case ItemID.CopperPlating://2173 Copper Plating
					return "";
				case ItemID.SnailCage://2174 Snail Cage
					return "";
				case ItemID.GlowingSnailCage://2175 Glowing Snail Cage
					return "";
				case ItemID.ShroomiteDiggingClaw://2176 Shroomite Digging Claw
					return "";
				case ItemID.AmmoBox://2177 Ammo Box
					return "";
				case ItemID.MonarchButterflyJar://2178 Monarch Butterfly Jar
					return "";
				case ItemID.PurpleEmperorButterflyJar://2179 Purple Emperor Butterfly Jar
					return "";
				case ItemID.RedAdmiralButterflyJar://2180 Red Admiral Butterfly Jar
					return "";
				case ItemID.UlyssesButterflyJar://2181 Ulysses Butterfly Jar
					return "";
				case ItemID.SulphurButterflyJar://2182 Sulphur Butterfly Jar
					return "";
				case ItemID.TreeNymphButterflyJar://2183 Tree Nymph Butterfly Jar
					return "";
				case ItemID.ZebraSwallowtailButterflyJar://2184 Zebra Swallowtail Butterfly Jar
					return "";
				case ItemID.JuliaButterflyJar://2185 Julia Butterfly Jar
					return "";
				case ItemID.ScorpionCage://2186 Scorpion Cage
					return "";
				case ItemID.BlackScorpionCage://2187 Black Scorpion Cage
					return "";
				case ItemID.VenomStaff://2188 Venom Staff
					return "";
				case ItemID.SpectreMask://2189 Spectre Mask
					return "";
				case ItemID.FrogCage://2190 Frog Cage
					return "";
				case ItemID.MouseCage://2191 Mouse Cage
					return "";
				case ItemID.BoneWelder://2192 Bone Welder
					return "";
				case ItemID.FleshCloningVaat://2193 Flesh Cloning Vat
					return "";
				case ItemID.GlassKiln://2194 Glass Kiln
					return "";
				case ItemID.LihzahrdFurnace://2195 Lihzahrd Furnace
					return "";
				case ItemID.LivingLoom://2196 Living Loom
					return "";
				case ItemID.SkyMill://2197 Sky Mill
					return "";
				case ItemID.IceMachine://2198 Ice Machine
					return "";
				case ItemID.BeetleHelmet://2199 Beetle Helmet
					return "";
				case ItemID.BeetleScaleMail://2200 Beetle Scale Mail
					return "";
				case ItemID.BeetleShell://2201 Beetle Shell
					return "";
				case ItemID.BeetleLeggings://2202 Beetle Leggings
					return "";
				case ItemID.SteampunkBoiler://2203 Steampunk Boiler
					return "";
				case ItemID.HoneyDispenser://2204 Honey Dispenser
					return "";
				case ItemID.Penguin://2205 Penguin
					return "";
				case ItemID.PenguinCage://2206 Penguin Cage
					return "";
				case ItemID.WormCage://2207 Worm Cage
					return "";
				case ItemID.Terrarium://2208 Terrarium
					return "";
				case ItemID.SuperManaPotion://2209 Super Mana Potion
					return "";
				case ItemID.EbonwoodFence://2210 Ebonwood Fence
					return "";
				case ItemID.RichMahoganyFence://2211 Rich Mahogany Fence
					return "";
				case ItemID.PearlwoodFence://2212 Pearlwood Fence
					return "";
				case ItemID.ShadewoodFence://2213 Shadewood Fence
					return "";
				case ItemID.BrickLayer://2214 Brick Layer
					return "";
				case ItemID.ExtendoGrip://2215 Extendo Grip
					return "";
				case ItemID.PaintSprayer://2216 Paint Sprayer
					return "";
				case ItemID.PortableCementMixer://2217 Portable Cement Mixer
					return "";
				case ItemID.BeetleHusk://2218 Beetle Husk
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/cb/Beetle_Husk.png/revision/latest?cb=20200730172110&format=original";
				case ItemID.CelestialMagnet://2219 Celestial Magnet
					return "";
				case ItemID.CelestialEmblem://2220 Celestial Emblem
					return "";
				case ItemID.CelestialCuffs://2221 Celestial Cuffs
					return "";
				case ItemID.PeddlersHat://2222 Peddler's Hat
					return "";
				case ItemID.PulseBow://2223 Pulse Bow
					return "";
				case ItemID.DynastyChandelier://2224 Large Dynasty Lantern
					return "";
				case ItemID.DynastyLamp://2225 Dynasty Lamp
					return "";
				case ItemID.DynastyLantern://2226 Dynasty Lantern
					return "";
				case ItemID.DynastyCandelabra://2227 Large Dynasty Candle
					return "";
				case ItemID.DynastyChair://2228 Dynasty Chair
					return "";
				case ItemID.DynastyWorkBench://2229 Dynasty Work Bench
					return "";
				case ItemID.DynastyChest://2230 Dynasty Chest
					return "";
				case ItemID.DynastyBed://2231 Dynasty Bed
					return "";
				case ItemID.DynastyBathtub://2232 Dynasty Bathtub
					return "";
				case ItemID.DynastyBookcase://2233 Dynasty Bookcase
					return "";
				case ItemID.DynastyCup://2234 Dynasty Cup
					return "";
				case ItemID.DynastyBowl://2235 Dynasty Bowl
					return "";
				case ItemID.DynastyCandle://2236 Dynasty Candle
					return "";
				case ItemID.DynastyClock://2237 Dynasty Clock
					return "";
				case ItemID.GoldenClock://2238 Golden Clock
					return "";
				case ItemID.GlassClock://2239 Glass Clock
					return "";
				case ItemID.HoneyClock://2240 Honey Clock
					return "";
				case ItemID.SteampunkClock://2241 Steampunk Clock
					return "";
				case ItemID.FancyDishes://2242 Fancy Dishes
					return "";
				case ItemID.GlassBowl://2243 Glass Bowl
					return "";
				case ItemID.WineGlass://2244 Wine Glass
					return "";
				case ItemID.LivingWoodPiano://2245 Living Wood Piano
					return "";
				case ItemID.FleshPiano://2246 Flesh Piano
					return "";
				case ItemID.FrozenPiano://2247 Frozen Piano
					return "";
				case ItemID.FrozenTable://2248 Frozen Table
					return "";
				case ItemID.HoneyChest://2249 Honey Chest
					return "";
				case ItemID.SteampunkChest://2250 Steampunk Chest
					return "";
				case ItemID.HoneyWorkBench://2251 Honey Work Bench
					return "";
				case ItemID.FrozenWorkBench://2252 Frozen Work Bench
					return "";
				case ItemID.SteampunkWorkBench://2253 Steampunk Work Bench
					return "";
				case ItemID.GlassPiano://2254 Glass Piano
					return "";
				case ItemID.HoneyPiano://2255 Honey Piano
					return "";
				case ItemID.SteampunkPiano://2256 Steampunk Piano
					return "";
				case ItemID.HoneyCup://2257 Honey Cup
					return "";
				case ItemID.SteampunkCup://2258 Chalice
					return "";
				case ItemID.DynastyTable://2259 Dynasty Table
					return "";
				case ItemID.DynastyWood://2260 Dynasty Wood
					return "";
				case ItemID.RedDynastyShingles://2261 Red Dynasty Shingles
					return "";
				case ItemID.BlueDynastyShingles://2262 Blue Dynasty Shingles
					return "";
				case ItemID.WhiteDynastyWall://2263 White Dynasty Wall
					return "";
				case ItemID.BlueDynastyWall://2264 Blue Dynasty Wall
					return "";
				case ItemID.DynastyDoor://2265 Dynasty Door
					return "";
				case ItemID.Sake://2266 Sake
					return "";
				case ItemID.PadThai://2267 Pad Thai
					return "";
				case ItemID.Pho://2268 Pho
					return "";
				case ItemID.Revolver://2269 Revolver
					return "";
				case ItemID.Gatligator://2270 Gatligator
					return "";
				case ItemID.ArcaneRuneWall://2271 Arcane Rune Wall
					return "";
				case ItemID.WaterGun://2272 Water Gun
					return "";
				case ItemID.Katana://2273 Katana
					return "";
				case ItemID.UltrabrightTorch://2274 Ultrabright Torch
					return "";
				case ItemID.MagicHat://2275 Magic Hat
					return "";
				case ItemID.DiamondRing://2276 Diamond Ring
					return "";
				case ItemID.Gi://2277 Gi
					return "";
				case ItemID.Kimono://2278 Kimono
					return "";
				case ItemID.GypsyRobe://2279 Mystic Robe
					return "";
				case ItemID.BeetleWings://2280 Beetle Wings
					return "";
				case ItemID.TigerSkin://2281 Tiger Skin
					return "";
				case ItemID.LeopardSkin://2282 Leopard Skin
					return "";
				case ItemID.ZebraSkin://2283 Zebra Skin
					return "";
				case ItemID.CrimsonCloak://2284 Crimson Cloak
					return "";
				case ItemID.MysteriousCape://2285 Mysterious Cape
					return "";
				case ItemID.RedCape://2286 Red Cape
					return "";
				case ItemID.WinterCape://2287 Winter Cape
					return "";
				case ItemID.FrozenChair://2288 Frozen Chair
					return "";
				case ItemID.WoodFishingPole://2289 Wood Fishing Pole
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Wood_Fishing_Pole.png/revision/latest?cb=20200814172805&format=original";
				case ItemID.Bass://2290 Bass
					return "";
				case ItemID.ReinforcedFishingPole://2291 Reinforced Fishing Pole
					return "";
				case ItemID.FiberglassFishingPole://2292 Fiberglass Fishing Pole
					return "";
				case ItemID.FisherofSouls://2293 Fisher of Souls
					return "";
				case ItemID.GoldenFishingRod://2294 Golden Fishing Rod
					return "";
				case ItemID.MechanicsRod://2295 Mechanic's Rod
					return "";
				case ItemID.SittingDucksFishingRod://2296 Sitting Duck's Fishing Pole
					return "";
				case ItemID.Trout://2297 Trout
					return "";
				case ItemID.Salmon://2298 Salmon
					return "";
				case ItemID.AtlanticCod://2299 Atlantic Cod
					return "";
				case ItemID.Tuna://2300 Tuna
					return "";
				case ItemID.RedSnapper://2301 Red Snapper
					return "";
				case ItemID.NeonTetra://2302 Neon Tetra
					return "";
				case ItemID.ArmoredCavefish://2303 Armored Cavefish
					return "";
				case ItemID.Damselfish://2304 Damselfish
					return "";
				case ItemID.CrimsonTigerfish://2305 Crimson Tigerfish
					return "";
				case ItemID.FrostMinnow://2306 Frost Minnow
					return "";
				case ItemID.PrincessFish://2307 Princess Fish
					return "";
				case ItemID.GoldenCarp://2308 Golden Carp
					return "";
				case ItemID.SpecularFish://2309 Specular Fish
					return "";
				case ItemID.Prismite://2310 Prismite
					return "";
				case ItemID.VariegatedLardfish://2311 Variegated Lardfish
					return "";
				case ItemID.FlarefinKoi://2312 Flarefin Koi
					return "";
				case ItemID.DoubleCod://2313 Double Cod
					return "";
				case ItemID.Honeyfin://2314 Honeyfin
					return "";
				case ItemID.Obsidifish://2315 Obsidifish
					return "";
				case ItemID.Shrimp://2316 Shrimp
					return "";
				case ItemID.ChaosFish://2317 Chaos Fish
					return "";
				case ItemID.Ebonkoi://2318 Ebonkoi
					return "";
				case ItemID.Hemopiranha://2319 Hemopiranha
					return "";
				case ItemID.Rockfish://2320 Rockfish
					return "";
				case ItemID.Stinkfish://2321 Stinkfish
					return "";
				case ItemID.MiningPotion://2322 Mining Potion
					return "";
				case ItemID.HeartreachPotion://2323 Heartreach Potion
					return "";
				case ItemID.CalmingPotion://2324 Calming Potion
					return "";
				case ItemID.BuilderPotion://2325 Builder Potion
					return "";
				case ItemID.TitanPotion://2326 Titan Potion
					return "";
				case ItemID.FlipperPotion://2327 Flipper Potion
					return "";
				case ItemID.SummoningPotion://2328 Summoning Potion
					return "";
				case ItemID.TrapsightPotion://2329 Dangersense Potion
					return "";
				case ItemID.PurpleClubberfish://2330 Purple Clubberfish
					return "";
				case ItemID.ObsidianSwordfish://2331 Obsidian Swordfish
					return "";
				case ItemID.Swordfish://2332 Swordfish
					return "";
				case ItemID.IronFence://2333 Iron Fence
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9d/Iron_Fence.png/revision/latest?cb=20200516214309&format=original";
				case ItemID.WoodenCrate://2334 Wooden Crate
					return "";
				case ItemID.IronCrate://2335 Iron Crate
					return "";
				case ItemID.GoldenCrate://2336 Golden Crate
					return "";
				case ItemID.OldShoe://2337 Old Shoe
					return "";
				case ItemID.FishingSeaweed://2338 Seaweed
					return "";
				case ItemID.TinCan://2339 Tin Can
					return "";
				case ItemID.MinecartTrack://2340 Minecart Track
					return "";
				case ItemID.ReaverShark://2341 Reaver Shark
					return "";
				case ItemID.SawtoothShark://2342 Sawtooth Shark
					return "";
				case ItemID.Minecart://2343 Minecart
					return "";
				case ItemID.AmmoReservationPotion://2344 Ammo Reservation Potion
					return "";
				case ItemID.LifeforcePotion://2345 Lifeforce Potion
					return "";
				case ItemID.EndurancePotion://2346 Endurance Potion
					return "";
				case ItemID.RagePotion://2347 Rage Potion
					return "";
				case ItemID.InfernoPotion://2348 Inferno Potion
					return "";
				case ItemID.WrathPotion://2349 Wrath Potion
					return "";
				case ItemID.RecallPotion://2350 Recall Potion
					return "";
				case ItemID.TeleportationPotion://2351 Teleportation Potion
					return "";
				case ItemID.LovePotion://2352 Love Potion
					return "";
				case ItemID.StinkPotion://2353 Stink Potion
					return "";
				case ItemID.FishingPotion://2354 Fishing Potion
					return "";
				case ItemID.SonarPotion://2355 Sonar Potion
					return "";
				case ItemID.CratePotion://2356 Crate Potion
					return "";
				case ItemID.ShiverthornSeeds://2357 Shiverthorn Seeds
					return "";
				case ItemID.Shiverthorn://2358 Shiverthorn
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/23/Shiverthorn.png/revision/latest?cb=20200516221732&format=original";
				case ItemID.WarmthPotion://2359 Warmth Potion
					return "";
				case ItemID.FishHook://2360 Fish Hook
					return "";
				case ItemID.BeeHeadgear://2361 Bee Headgear
					return "";
				case ItemID.BeeBreastplate://2362 Bee Breastplate
					return "";
				case ItemID.BeeGreaves://2363 Bee Greaves
					return "";
				case ItemID.HornetStaff://2364 Hornet Staff
					return "";
				case ItemID.ImpStaff://2365 Imp Staff
					return "";
				case ItemID.QueenSpiderStaff://2366 Queen Spider Staff
					return "";
				case ItemID.AnglerHat://2367 Angler Hat
					return "";
				case ItemID.AnglerVest://2368 Angler Vest
					return "";
				case ItemID.AnglerPants://2369 Angler Pants
					return "";
				case ItemID.SpiderMask://2370 Spider Mask
					return "";
				case ItemID.SpiderBreastplate://2371 Spider Breastplate
					return "";
				case ItemID.SpiderGreaves://2372 Spider Greaves
					return "";
				case ItemID.HighTestFishingLine://2373 High Test Fishing Line
					return "";
				case ItemID.AnglerEarring://2374 Angler Earring
					return "";
				case ItemID.TackleBox://2375 Tackle Box
					return "";
				case ItemID.BlueDungeonPiano://2376 Blue Dungeon Piano
					return "";
				case ItemID.GreenDungeonPiano://2377 Green Dungeon Piano
					return "";
				case ItemID.PinkDungeonPiano://2378 Pink Dungeon Piano
					return "";
				case ItemID.GoldenPiano://2379 Golden Piano
					return "";
				case ItemID.ObsidianPiano://2380 Obsidian Piano
					return "";
				case ItemID.BonePiano://2381 Bone Piano
					return "";
				case ItemID.CactusPiano://2382 Cactus Piano
					return "";
				case ItemID.SpookyPiano://2383 Spooky Piano
					return "";
				case ItemID.SkywarePiano://2384 Skyware Piano
					return "";
				case ItemID.LihzahrdPiano://2385 Lihzahrd Piano
					return "";
				case ItemID.BlueDungeonDresser://2386 Blue Dungeon Dresser
					return "";
				case ItemID.GreenDungeonDresser://2387 Green Dungeon Dresser
					return "";
				case ItemID.PinkDungeonDresser://2388 Pink Dungeon Dresser
					return "";
				case ItemID.GoldenDresser://2389 Golden Dresser
					return "";
				case ItemID.ObsidianDresser://2390 Obsidian Dresser
					return "";
				case ItemID.BoneDresser://2391 Bone Dresser
					return "";
				case ItemID.CactusDresser://2392 Cactus Dresser
					return "";
				case ItemID.SpookyDresser://2393 Spooky Dresser
					return "";
				case ItemID.SkywareDresser://2394 Skyware Dresser
					return "";
				case ItemID.HoneyDresser://2395 Honey Dresser
					return "";
				case ItemID.LihzahrdDresser://2396 Lihzahrd Dresser
					return "";
				case ItemID.Sofa://2397 Sofa
					return "";
				case ItemID.EbonwoodSofa://2398 Ebonwood Sofa
					return "";
				case ItemID.RichMahoganySofa://2399 Rich Mahogany Sofa
					return "";
				case ItemID.PearlwoodSofa://2400 Pearlwood Sofa
					return "";
				case ItemID.ShadewoodSofa://2401 Shadewood Sofa
					return "";
				case ItemID.BlueDungeonSofa://2402 Blue Dungeon Sofa
					return "";
				case ItemID.GreenDungeonSofa://2403 Green Dungeon Sofa
					return "";
				case ItemID.PinkDungeonSofa://2404 Pink Dungeon Sofa
					return "";
				case ItemID.GoldenSofa://2405 Golden Sofa
					return "";
				case ItemID.ObsidianSofa://2406 Obsidian Sofa
					return "";
				case ItemID.BoneSofa://2407 Bone Sofa
					return "";
				case ItemID.CactusSofa://2408 Cactus Sofa
					return "";
				case ItemID.SpookySofa://2409 Spooky Sofa
					return "";
				case ItemID.SkywareSofa://2410 Skyware Sofa
					return "";
				case ItemID.HoneySofa://2411 Honey Sofa
					return "";
				case ItemID.SteampunkSofa://2412 Steampunk Sofa
					return "";
				case ItemID.MushroomSofa://2413 Mushroom Sofa
					return "";
				case ItemID.GlassSofa://2414 Glass Sofa
					return "";
				case ItemID.PumpkinSofa://2415 Pumpkin Sofa
					return "";
				case ItemID.LihzahrdSofa://2416 Lihzahrd Sofa
					return "";
				case ItemID.SeashellHairpin://2417 Seashell Hairpin
					return "";
				case ItemID.MermaidAdornment://2418 Mermaid Adornment
					return "";
				case ItemID.MermaidTail://2419 Mermaid Tail
					return "";
				case ItemID.ZephyrFish://2420 Zephyr Fish
					return "";
				case ItemID.Fleshcatcher://2421 Fleshcatcher
					return "";
				case ItemID.HotlineFishingHook://2422 Hotline Fishing Hook
					return "";
				case ItemID.FrogLeg://2423 Frog Leg
					return "";
				case ItemID.Anchor://2424 Anchor
					return "";
				case ItemID.CookedFish://2425 Cooked Fish
					return "";
				case ItemID.CookedShrimp://2426 Cooked Shrimp
					return "";
				case ItemID.Sashimi://2427 Sashimi
					return "";
				case ItemID.FuzzyCarrot://2428 Fuzzy Carrot
					return "";
				case ItemID.ScalyTruffle://2429 Scaly Truffle
					return "";
				case ItemID.SlimySaddle://2430 Slimy Saddle
					return "";
				case ItemID.BeeWax://2431 Bee Wax
					return "";
				case ItemID.CopperPlatingWall://2432 Copper Plating Wall
					return "";
				case ItemID.StoneSlabWall://2433 Stone Slab Wall
					return "";
				case ItemID.Sail://2434 Sail
					return "";
				case ItemID.CoralstoneBlock://2435 Coralstone Block
					return "";
				case ItemID.BlueJellyfish://2436 Blue Jellyfish
					return "";
				case ItemID.GreenJellyfish://2437 Green Jellyfish
					return "";
				case ItemID.PinkJellyfish://2438 Pink Jellyfish
					return "";
				case ItemID.BlueJellyfishJar://2439 Blue Jellyfish Jar
					return "";
				case ItemID.GreenJellyfishJar://2440 Green Jellyfish Jar
					return "";
				case ItemID.PinkJellyfishJar://2441 Pink Jellyfish Jar
					return "";
				case ItemID.LifePreserver://2442 Life Preserver
					return "";
				case ItemID.ShipsWheel://2443 Ship's Wheel
					return "";
				case ItemID.CompassRose://2444 Compass Rose
					return "";
				case ItemID.WallAnchor://2445 Wall Anchor
					return "";
				case ItemID.GoldfishTrophy://2446 Goldfish Trophy
					return "";
				case ItemID.BunnyfishTrophy://2447 Bunnyfish Trophy
					return "";
				case ItemID.SwordfishTrophy://2448 Swordfish Trophy
					return "";
				case ItemID.SharkteethTrophy://2449 Sharkteeth Trophy
					return "";
				case ItemID.Batfish://2450 Batfish
					return "";
				case ItemID.BumblebeeTuna://2451 Bumblebee Tuna
					return "";
				case ItemID.Catfish://2452 Catfish
					return "";
				case ItemID.Cloudfish://2453 Cloudfish
					return "";
				case ItemID.Cursedfish://2454 Cursedfish
					return "";
				case ItemID.Dirtfish://2455 Dirtfish
					return "";
				case ItemID.DynamiteFish://2456 Dynamite Fish
					return "";
				case ItemID.EaterofPlankton://2457 Eater of Plankton
					return "";
				case ItemID.FallenStarfish://2458 Fallen Starfish
					return "";
				case ItemID.TheFishofCthulu://2459 The Fish of Cthulhu
					return "";
				case ItemID.Fishotron://2460 Fishotron
					return "";
				case ItemID.Harpyfish://2461 Harpyfish
					return "";
				case ItemID.Hungerfish://2462 Hungerfish
					return "";
				case ItemID.Ichorfish://2463 Ichorfish
					return "";
				case ItemID.Jewelfish://2464 Jewelfish
					return "";
				case ItemID.MirageFish://2465 Mirage Fish
					return "";
				case ItemID.MutantFlinxfin://2466 Mutant Flinxfin
					return "";
				case ItemID.Pengfish://2467 Pengfish
					return "";
				case ItemID.Pixiefish://2468 Pixiefish
					return "";
				case ItemID.Spiderfish://2469 Spiderfish
					return "";
				case ItemID.TundraTrout://2470 Tundra Trout
					return "";
				case ItemID.UnicornFish://2471 Unicorn Fish
					return "";
				case ItemID.GuideVoodooFish://2472 Guide Voodoo Fish
					return "";
				case ItemID.Wyverntail://2473 Wyverntail
					return "";
				case ItemID.ZombieFish://2474 Zombie Fish
					return "";
				case ItemID.AmanitaFungifin://2475 Amanita Fungifin
					return "";
				case ItemID.Angelfish://2476 Angelfish
					return "";
				case ItemID.BloodyManowar://2477 Bloody Manowar
					return "";
				case ItemID.Bonefish://2478 Bonefish
					return "";
				case ItemID.Bunnyfish://2479 Bunnyfish
					return "";
				case ItemID.CapnTunabeard://2480 Cap'n Tunabeard
					return "";
				case ItemID.Clownfish://2481 Clownfish
					return "";
				case ItemID.DemonicHellfish://2482 Demonic Hellfish
					return "";
				case ItemID.Derpfish://2483 Derpfish
					return "";
				case ItemID.Fishron://2484 Fishron
					return "";
				case ItemID.InfectedScabbardfish://2485 Infected Scabbardfish
					return "";
				case ItemID.Mudfish://2486 Mudfish
					return "";
				case ItemID.Slimefish://2487 Slimefish
					return "";
				case ItemID.TropicalBarracuda://2488 Tropical Barracuda
					return "";
				case ItemID.KingSlimeTrophy://2489 King Slime Trophy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/45/Trophies.gif/revision/latest?cb=20220122175606&format=original";
				case ItemID.ShipInABottle://2490 Ship in a Bottle
					return "";
				case ItemID.HardySaddle://2491 Hardy Saddle
					return "";
				case ItemID.PressureTrack://2492 Pressure Plate Track
					return "";
				case ItemID.KingSlimeMask://2493 King Slime Mask
					return "";
				case ItemID.FinWings://2494 Fin Wings
					return "";
				case ItemID.TreasureMap://2495 Treasure Map
					return "";
				case ItemID.SeaweedPlanter://2496 Seaweed Planter
					return "";
				case ItemID.PillaginMePixels://2497 Pillagin' Me Pixels
					return "";
				case ItemID.FishCostumeMask://2498 Fish Costume Mask
					return "";
				case ItemID.FishCostumeShirt://2499 Fish Costume Shirt
					return "";
				case ItemID.FishCostumeFinskirt://2500 Fish Costume Finskirt
					return "";
				case ItemID.GingerBeard://2501 Ginger Beard
					return "";
				case ItemID.HoneyedGoggles://2502 Honeyed Goggles
					return "";
				case ItemID.BorealWood://2503 Boreal Wood
					return "";
				case ItemID.PalmWood://2504 Palm Wood
					return "";
				case ItemID.BorealWoodWall://2505 Boreal Wood Wall
					return "";
				case ItemID.PalmWoodWall://2506 Palm Wood Wall
					return "";
				case ItemID.BorealWoodFence://2507 Boreal Wood Fence
					return "";
				case ItemID.PalmWoodFence://2508 Palm Wood Fence
					return "";
				case ItemID.BorealWoodHelmet://2509 Boreal Wood Helmet
					return "";
				case ItemID.BorealWoodBreastplate://2510 Boreal Wood Breastplate
					return "";
				case ItemID.BorealWoodGreaves://2511 Boreal Wood Greaves
					return "";
				case ItemID.PalmWoodHelmet://2512 Palm Wood Helmet
					return "";
				case ItemID.PalmWoodBreastplate://2513 Palm Wood Breastplate
					return "";
				case ItemID.PalmWoodGreaves://2514 Palm Wood Greaves
					return "";
				case ItemID.PalmWoodBow://2515 Palm Wood Bow
					return "";
				case ItemID.PalmWoodHammer://2516 Palm Wood Hammer
					return "";
				case ItemID.PalmWoodSword://2517 Palm Wood Sword
					return "";
				case ItemID.PalmWoodPlatform://2518 Palm Wood Platform
					return "";
				case ItemID.PalmWoodBathtub://2519 Palm Wood Bathtub
					return "";
				case ItemID.PalmWoodBed://2520 Palm Wood Bed
					return "";
				case ItemID.PalmWoodBench://2521 Palm Wood Bench
					return "";
				case ItemID.PalmWoodCandelabra://2522 Palm Wood Candelabra
					return "";
				case ItemID.PalmWoodCandle://2523 Palm Wood Candle
					return "";
				case ItemID.PalmWoodChair://2524 Palm Wood Chair
					return "";
				case ItemID.PalmWoodChandelier://2525 Palm Wood Chandelier
					return "";
				case ItemID.PalmWoodChest://2526 Palm Wood Chest
					return "";
				case ItemID.PalmWoodSofa://2527 Palm Wood Sofa
					return "";
				case ItemID.PalmWoodDoor://2528 Palm Wood Door
					return "";
				case ItemID.PalmWoodDresser://2529 Palm Wood Dresser
					return "";
				case ItemID.PalmWoodLantern://2530 Palm Wood Lantern
					return "";
				case ItemID.PalmWoodPiano://2531 Palm Wood Piano
					return "";
				case ItemID.PalmWoodTable://2532 Palm Wood Table
					return "";
				case ItemID.PalmWoodLamp://2533 Palm Wood Lamp
					return "";
				case ItemID.PalmWoodWorkBench://2534 Palm Wood Work Bench
					return "";
				case ItemID.OpticStaff://2535 Optic Staff
					return "";
				case ItemID.PalmWoodBookcase://2536 Palm Wood Bookcase
					return "";
				case ItemID.MushroomBathtub://2537 Mushroom Bathtub
					return "";
				case ItemID.MushroomBed://2538 Mushroom Bed
					return "";
				case ItemID.MushroomBench://2539 Mushroom Bench
					return "";
				case ItemID.MushroomBookcase://2540 Mushroom Bookcase
					return "";
				case ItemID.MushroomCandelabra://2541 Mushroom Candelabra
					return "";
				case ItemID.MushroomCandle://2542 Mushroom Candle
					return "";
				case ItemID.MushroomChandelier://2543 Mushroom Chandelier
					return "";
				case ItemID.MushroomChest://2544 Mushroom Chest
					return "";
				case ItemID.MushroomDresser://2545 Mushroom Dresser
					return "";
				case ItemID.MushroomLantern://2546 Mushroom Lantern
					return "";
				case ItemID.MushroomLamp://2547 Mushroom Lamp
					return "";
				case ItemID.MushroomPiano://2548 Mushroom Piano
					return "";
				case ItemID.MushroomPlatform://2549 Mushroom Platform
					return "";
				case ItemID.MushroomTable://2550 Mushroom Table
					return "";
				case ItemID.SpiderStaff://2551 Spider Staff
					return "";
				case ItemID.BorealWoodBathtub://2552 Boreal Wood Bathtub
					return "";
				case ItemID.BorealWoodBed://2553 Boreal Wood Bed
					return "";
				case ItemID.BorealWoodBookcase://2554 Boreal Wood Bookcase
					return "";
				case ItemID.BorealWoodCandelabra://2555 Boreal Wood Candelabra
					return "";
				case ItemID.BorealWoodCandle://2556 Boreal Wood Candle
					return "";
				case ItemID.BorealWoodChair://2557 Boreal Wood Chair
					return "";
				case ItemID.BorealWoodChandelier://2558 Boreal Wood Chandelier
					return "";
				case ItemID.BorealWoodChest://2559 Boreal Wood Chest
					return "";
				case ItemID.BorealWoodClock://2560 Boreal Wood Clock
					return "";
				case ItemID.BorealWoodDoor://2561 Boreal Wood Door
					return "";
				case ItemID.BorealWoodDresser://2562 Boreal Wood Dresser
					return "";
				case ItemID.BorealWoodLamp://2563 Boreal Wood Lamp
					return "";
				case ItemID.BorealWoodLantern://2564 Boreal Wood Lantern
					return "";
				case ItemID.BorealWoodPiano://2565 Boreal Wood Piano
					return "";
				case ItemID.BorealWoodPlatform://2566 Boreal Wood Platform
					return "";
				case ItemID.SlimeBathtub://2567 Slime Bathtub
					return "";
				case ItemID.SlimeBed://2568 Slime Bed
					return "";
				case ItemID.SlimeBookcase://2569 Slime Bookcase
					return "";
				case ItemID.SlimeCandelabra://2570 Slime Candelabra
					return "";
				case ItemID.SlimeCandle://2571 Slime Candle
					return "";
				case ItemID.SlimeChair://2572 Slime Chair
					return "";
				case ItemID.SlimeChandelier://2573 Slime Chandelier
					return "";
				case ItemID.SlimeChest://2574 Slime Chest
					return "";
				case ItemID.SlimeClock://2575 Slime Clock
					return "";
				case ItemID.SlimeDoor://2576 Slime Door
					return "";
				case ItemID.SlimeDresser://2577 Slime Dresser
					return "";
				case ItemID.SlimeLamp://2578 Slime Lamp
					return "";
				case ItemID.SlimeLantern://2579 Slime Lantern
					return "";
				case ItemID.SlimePiano://2580 Slime Piano
					return "";
				case ItemID.SlimePlatform://2581 Slime Platform
					return "";
				case ItemID.SlimeSofa://2582 Slime Sofa
					return "";
				case ItemID.SlimeTable://2583 Slime Table
					return "";
				case ItemID.PirateStaff://2584 Pirate Staff
					return "";
				case ItemID.SlimeHook://2585 Slime Hook
					return "";
				case ItemID.StickyGrenade://2586 Sticky Grenade
					return "";
				case ItemID.TartarSauce://2587 Tartar Sauce
					return "";
				case ItemID.DukeFishronMask://2588 Duke Fishron Mask
					return "";
				case ItemID.DukeFishronTrophy://2589 Duke Fishron Trophy
					return "";
				case ItemID.MolotovCocktail://2590 Molotov Cocktail
					return "";
				case ItemID.BoneClock://2591 Bone Clock
					return "";
				case ItemID.CactusClock://2592 Cactus Clock
					return "";
				case ItemID.EbonwoodClock://2593 Ebonwood Clock
					return "";
				case ItemID.FrozenClock://2594 Frozen Clock
					return "";
				case ItemID.LihzahrdClock://2595 Lihzahrd Clock
					return "";
				case ItemID.LivingWoodClock://2596 Living Wood Clock
					return "";
				case ItemID.RichMahoganyClock://2597 Rich Mahogany Clock
					return "";
				case ItemID.FleshClock://2598 Flesh Clock
					return "";
				case ItemID.MushroomClock://2599 Mushroom Clock
					return "";
				case ItemID.ObsidianClock://2600 Obsidian Clock
					return "";
				case ItemID.PalmWoodClock://2601 Palm Wood Clock
					return "";
				case ItemID.PearlwoodClock://2602 Pearlwood Clock
					return "";
				case ItemID.PumpkinClock://2603 Pumpkin Clock
					return "";
				case ItemID.ShadewoodClock://2604 Shadewood Clock
					return "";
				case ItemID.SpookyClock://2605 Spooky Clock
					return "";
				case ItemID.SkywareClock://2606 Skyware Clock
					return "";
				case ItemID.SpiderFang://2607 Spider Fang
					return "";
				case ItemID.FalconBlade://2608 Falcon Blade
					return "";
				case ItemID.FishronWings://2609 Fishron Wings
					return "";
				case ItemID.SlimeGun://2610 Slime Gun
					return "";
				case ItemID.Flairon://2611 Flairon
					return "";
				case ItemID.GreenDungeonChest://2612 Green Dungeon Chest
					return "";
				case ItemID.PinkDungeonChest://2613 Pink Dungeon Chest
					return "";
				case ItemID.BlueDungeonChest://2614 Blue Dungeon Chest
					return "";
				case ItemID.BoneChest://2615 Bone Chest
					return "";
				case ItemID.CactusChest://2616 Cactus Chest
					return "";
				case ItemID.FleshChest://2617 Flesh Chest
					return "";
				case ItemID.ObsidianChest://2618 Obsidian Chest
					return "";
				case ItemID.PumpkinChest://2619 Pumpkin Chest
					return "";
				case ItemID.SpookyChest://2620 Spooky Chest
					return "";
				case ItemID.TempestStaff://2621 Tempest Staff
					return "";
				case ItemID.RazorbladeTyphoon://2622 Razorblade Typhoon
					return "";
				case ItemID.BubbleGun://2623 Bubble Gun
					return "";
				case ItemID.Tsunami://2624 Tsunami
					return "";
				case ItemID.Seashell://2625 Seashell
					return "";
				case ItemID.Starfish://2626 Starfish
					return "";
				case ItemID.SteampunkPlatform://2627 Steampunk Platform
					return "";
				case ItemID.SkywarePlatform://2628 Skyware Platform
					return "";
				case ItemID.LivingWoodPlatform://2629 Living Wood Platform
					return "";
				case ItemID.HoneyPlatform://2630 Honey Platform
					return "";
				case ItemID.SkywareWorkbench://2631 Skyware Work Bench
					return "";
				case ItemID.GlassWorkBench://2632 Glass Work Bench
					return "";
				case ItemID.LivingWoodWorkBench://2633 Living Wood Work Bench
					return "";
				case ItemID.FleshSofa://2634 Flesh Sofa
					return "";
				case ItemID.FrozenSofa://2635 Frozen Sofa
					return "";
				case ItemID.LivingWoodSofa://2636 Living Wood Sofa
					return "";
				case ItemID.PumpkinDresser://2637 Pumpkin Dresser
					return "";
				case ItemID.SteampunkDresser://2638 Steampunk Dresser
					return "";
				case ItemID.GlassDresser://2639 Glass Dresser
					return "";
				case ItemID.FleshDresser://2640 Flesh Dresser
					return "";
				case ItemID.PumpkinLantern://2641 Pumpkin Lantern
					return "";
				case ItemID.ObsidianLantern://2642 Obsidian Lantern
					return "";
				case ItemID.PumpkinLamp://2643 Pumpkin Lamp
					return "";
				case ItemID.ObsidianLamp://2644 Obsidian Lamp
					return "";
				case ItemID.BlueDungeonLamp://2645 Blue Dungeon Lamp
					return "";
				case ItemID.GreenDungeonLamp://2646 Green Dungeon Lamp
					return "";
				case ItemID.PinkDungeonLamp://2647 Pink Dungeon Lamp
					return "";
				case ItemID.HoneyCandle://2648 Honey Candle
					return "";
				case ItemID.SteampunkCandle://2649 Steampunk Candle
					return "";
				case ItemID.SpookyCandle://2650 Spooky Candle
					return "";
				case ItemID.ObsidianCandle://2651 Obsidian Candle
					return "";
				case ItemID.BlueDungeonChandelier://2652 Blue Dungeon Chandelier
					return "";
				case ItemID.GreenDungeonChandelier://2653 Green Dungeon Chandelier
					return "";
				case ItemID.PinkDungeonChandelier://2654 Pink Dungeon Chandelier
					return "";
				case ItemID.SteampunkChandelier://2655 Steampunk Chandelier
					return "";
				case ItemID.PumpkinChandelier://2656 Pumpkin Chandelier
					return "";
				case ItemID.ObsidianChandelier://2657 Obsidian Chandelier
					return "";
				case ItemID.BlueDungeonBathtub://2658 Blue Dungeon Bathtub
					return "";
				case ItemID.GreenDungeonBathtub://2659 Green Dungeon Bathtub
					return "";
				case ItemID.PinkDungeonBathtub://2660 Pink Dungeon Bathtub
					return "";
				case ItemID.PumpkinBathtub://2661 Pumpkin Bathtub
					return "";
				case ItemID.ObsidianBathtub://2662 Obsidian Bathtub
					return "";
				case ItemID.GoldenBathtub://2663 Golden Bathtub
					return "";
				case ItemID.BlueDungeonCandelabra://2664 Blue Dungeon Candelabra
					return "";
				case ItemID.GreenDungeonCandelabra://2665 Green Dungeon Candelabra
					return "";
				case ItemID.PinkDungeonCandelabra://2666 Pink Dungeon Candelabra
					return "";
				case ItemID.ObsidianCandelabra://2667 Obsidian Candelabra
					return "";
				case ItemID.PumpkinCandelabra://2668 Pumpkin Candelabra
					return "";
				case ItemID.PumpkinBed://2669 Pumpkin Bed
					return "";
				case ItemID.PumpkinBookcase://2670 Pumpkin Bookcase
					return "";
				case ItemID.PumpkinPiano://2671 Pumpkin Piano
					return "";
				case ItemID.SharkStatue://2672 Shark Statue
					return "";
				case ItemID.TruffleWorm://2673 Truffle Worm
					return "";
				case ItemID.ApprenticeBait://2674 Apprentice Bait
					return "";
				case ItemID.JourneymanBait://2675 Journeyman Bait
					return "";
				case ItemID.MasterBait://2676 Master Bait
					return "";
				case ItemID.AmberGemsparkWall://2677 Amber Gemspark Wall
					return "";
				case ItemID.AmberGemsparkWallOff://2678 Offline Amber Gemspark Wall
					return "";
				case ItemID.AmethystGemsparkWall://2679 Amethyst Gemspark Wall
					return "";
				case ItemID.AmethystGemsparkWallOff://2680 Offline Amethyst Gemspark Wall
					return "";
				case ItemID.DiamondGemsparkWall://2681 Diamond Gemspark Wall
					return "";
				case ItemID.DiamondGemsparkWallOff://2682 Offline Diamond Gemspark Wall
					return "";
				case ItemID.EmeraldGemsparkWall://2683 Emerald Gemspark Wall
					return "";
				case ItemID.EmeraldGemsparkWallOff://2684 Offline Emerald Gemspark Wall
					return "";
				case ItemID.RubyGemsparkWall://2685 Ruby Gemspark Wall
					return "";
				case ItemID.RubyGemsparkWallOff://2686 Offline Ruby Gemspark Wall
					return "";
				case ItemID.SapphireGemsparkWall://2687 Sapphire Gemspark Wall
					return "";
				case ItemID.SapphireGemsparkWallOff://2688 Offline Sapphire Gemspark Wall
					return "";
				case ItemID.TopazGemsparkWall://2689 Topaz Gemspark Wall
					return "";
				case ItemID.TopazGemsparkWallOff://2690 Offline Topaz Gemspark Wall
					return "";
				case ItemID.TinPlatingWall://2691 Tin Plating Wall
					return "";
				case ItemID.TinPlating://2692 Tin Plating
					return "";
				case ItemID.WaterfallBlock://2693 Waterfall Block
					return "";
				case ItemID.LavafallBlock://2694 Lavafall Block
					return "";
				case ItemID.ConfettiBlock://2695 Confetti Block
					return "";
				case ItemID.ConfettiWall://2696 Confetti Wall
					return "";
				case ItemID.ConfettiBlockBlack://2697 Midnight Confetti Block
					return "";
				case ItemID.ConfettiWallBlack://2698 Midnight Confetti Wall
					return "";
				case ItemID.WeaponRack://2699 Weapon Rack
					return "";
				case ItemID.FireworksBox://2700 Fireworks Box
					return "";
				case ItemID.LivingFireBlock://2701 Living Fire Block
					return "";
				case ItemID.AlphabetStatue0://2702 '0' Statue
					return "";
				case ItemID.AlphabetStatue1://2703 '1' Statue
					return "";
				case ItemID.AlphabetStatue2://2704 '2' Statue
					return "";
				case ItemID.AlphabetStatue3://2705 '3' Statue
					return "";
				case ItemID.AlphabetStatue4://2706 '4' Statue
					return "";
				case ItemID.AlphabetStatue5://2707 '5' Statue
					return "";
				case ItemID.AlphabetStatue6://2708 '6' Statue
					return "";
				case ItemID.AlphabetStatue7://2709 '7' Statue
					return "";
				case ItemID.AlphabetStatue8://2710 '8' Statue
					return "";
				case ItemID.AlphabetStatue9://2711 '9' Statue
					return "";
				case ItemID.AlphabetStatueA://2712 'A' Statue
					return "";
				case ItemID.AlphabetStatueB://2713 'B' Statue
					return "";
				case ItemID.AlphabetStatueC://2714 'C' Statue
					return "";
				case ItemID.AlphabetStatueD://2715 'D' Statue
					return "";
				case ItemID.AlphabetStatueE://2716 'E' Statue
					return "";
				case ItemID.AlphabetStatueF://2717 'F' Statue
					return "";
				case ItemID.AlphabetStatueG://2718 'G' Statue
					return "";
				case ItemID.AlphabetStatueH://2719 'H' Statue
					return "";
				case ItemID.AlphabetStatueI://2720 'I' Statue
					return "";
				case ItemID.AlphabetStatueJ://2721 'J' Statue
					return "";
				case ItemID.AlphabetStatueK://2722 'K' Statue
					return "";
				case ItemID.AlphabetStatueL://2723 'L' Statue
					return "";
				case ItemID.AlphabetStatueM://2724 'M' Statue
					return "";
				case ItemID.AlphabetStatueN://2725 'N' Statue
					return "";
				case ItemID.AlphabetStatueO://2726 'O' Statue
					return "";
				case ItemID.AlphabetStatueP://2727 'P' Statue
					return "";
				case ItemID.AlphabetStatueQ://2728 'Q' Statue
					return "";
				case ItemID.AlphabetStatueR://2729 'R' Statue
					return "";
				case ItemID.AlphabetStatueS://2730 'S' Statue
					return "";
				case ItemID.AlphabetStatueT://2731 'T' Statue
					return "";
				case ItemID.AlphabetStatueU://2732 'U' Statue
					return "";
				case ItemID.AlphabetStatueV://2733 'V' Statue
					return "";
				case ItemID.AlphabetStatueW://2734 'W' Statue
					return "";
				case ItemID.AlphabetStatueX://2735 'X' Statue
					return "";
				case ItemID.AlphabetStatueY://2736 'Y' Statue
					return "";
				case ItemID.AlphabetStatueZ://2737 'Z' Statue
					return "";
				case ItemID.FireworkFountain://2738 Firework Fountain
					return "";
				case ItemID.BoosterTrack://2739 Booster Track
					return "";
				case ItemID.Grasshopper://2740 Grasshopper
					return "";
				case ItemID.GrasshopperCage://2741 Grasshopper Cage
					return "";
				case ItemID.MusicBoxUndergroundCrimson://2742 Music Box (Underground Crimson)
					return "";
				case ItemID.CactusTable://2743 Cactus Table
					return "";
				case ItemID.CactusPlatform://2744 Cactus Platform
					return "";
				case ItemID.BorealWoodSword://2745 Boreal Wood Sword
					return "";
				case ItemID.BorealWoodHammer://2746 Boreal Wood Hammer
					return "";
				case ItemID.BorealWoodBow://2747 Boreal Wood Bow
					return "";
				case ItemID.GlassChest://2748 Glass Chest
					return "";
				case ItemID.XenoStaff://2749 Xeno Staff
					return "";
				case ItemID.MeteorStaff://2750 Meteor Staff
					return "";
				case ItemID.LivingCursedFireBlock://2751 Living Cursed Fire Block
					return "";
				case ItemID.LivingDemonFireBlock://2752 Living Demon Fire Block
					return "";
				case ItemID.LivingFrostFireBlock://2753 Living Frost Fire Block
					return "";
				case ItemID.LivingIchorBlock://2754 Living Ichor Block
					return "";
				case ItemID.LivingUltrabrightFireBlock://2755 Living Ultrabright Fire Block
					return "";
				case ItemID.GenderChangePotion://2756 Gender Change Potion
					return "";
				case ItemID.VortexHelmet://2757 Vortex Helmet
					return "";
				case ItemID.VortexBreastplate://2758 Vortex Breastplate
					return "";
				case ItemID.VortexLeggings://2759 Vortex Leggings
					return "";
				case ItemID.NebulaHelmet://2760 Nebula Helmet
					return "";
				case ItemID.NebulaBreastplate://2761 Nebula Breastplate
					return "";
				case ItemID.NebulaLeggings://2762 Nebula Leggings
					return "";
				case ItemID.SolarFlareHelmet://2763 Solar Flare Helmet
					return "";
				case ItemID.SolarFlareBreastplate://2764 Solar Flare Breastplate
					return "";
				case ItemID.SolarFlareLeggings://2765 Solar Flare Leggings
					return "";
				case ItemID.LunarTabletFragment://2766 Solar Tablet Fragment
					return "";
				case ItemID.SolarTablet://2767 Solar Tablet
					return "";
				case ItemID.DrillContainmentUnit://2768 Drill Containment Unit
					return "";
				case ItemID.CosmicCarKey://2769 Cosmic Car Key
					return "";
				case ItemID.MothronWings://2770 Mothron Wings
					return "";
				case ItemID.BrainScrambler://2771 Brain Scrambler
					return "";
				case ItemID.VortexAxe://2772 
					return "";
				case ItemID.VortexChainsaw://2773 
					return "";
				case ItemID.VortexDrill://2774 Vortex Drill
					return "";
				case ItemID.VortexHammer://2775 
					return "";
				case ItemID.VortexPickaxe://2776 Vortex Pickaxe
					return "";
				case ItemID.NebulaAxe://2777 
					return "";
				case ItemID.NebulaChainsaw://2778 
					return "";
				case ItemID.NebulaDrill://2779 Nebula Drill
					return "";
				case ItemID.NebulaHammer://2780 
					return "";
				case ItemID.NebulaPickaxe://2781 Nebula Pickaxe
					return "";
				case ItemID.SolarFlareAxe://2782 
					return "";
				case ItemID.SolarFlareChainsaw://2783 
					return "";
				case ItemID.SolarFlareDrill://2784 Solar Flare Drill
					return "";
				case ItemID.SolarFlareHammer://2785 
					return "";
				case ItemID.SolarFlarePickaxe://2786 Solar Flare Pickaxe
					return "";
				case ItemID.HoneyfallBlock://2787 Honeyfall Block
					return "";
				case ItemID.HoneyfallWall://2788 Honeyfall Wall
					return "";
				case ItemID.ChlorophyteBrickWall://2789 Chlorophyte Brick Wall
					return "";
				case ItemID.CrimtaneBrickWall://2790 Crimtane Brick Wall
					return "";
				case ItemID.ShroomitePlatingWall://2791 Shroomite Plating Wall
					return "";
				case ItemID.ChlorophyteBrick://2792 Chlorophyte Brick
					return "";
				case ItemID.CrimtaneBrick://2793 Crimtane Brick
					return "";
				case ItemID.ShroomitePlating://2794 Shroomite Plating
					return "";
				case ItemID.LaserMachinegun://2795 Laser Machinegun
					return "";
				case ItemID.ElectrosphereLauncher://2796 Electrosphere Launcher
					return "";
				case ItemID.Xenopopper://2797 Xenopopper
					return "";
				case ItemID.LaserDrill://2798 Laser Drill
					return "";
				case ItemID.LaserRuler://2799 Mechanical Ruler
					return "";
				case ItemID.AntiGravityHook://2800 Anti-Gravity Hook
					return "";
				case ItemID.MoonMask://2801 Moon Mask
					return "";
				case ItemID.SunMask://2802 Sun Mask
					return "";
				case ItemID.MartianCostumeMask://2803 Martian Costume Mask
					return "";
				case ItemID.MartianCostumeShirt://2804 Martian Costume Shirt
					return "";
				case ItemID.MartianCostumePants://2805 Martian Costume Pants
					return "";
				case ItemID.MartianUniformHelmet://2806 Martian Uniform Helmet
					return "";
				case ItemID.MartianUniformTorso://2807 Martian Uniform Torso
					return "";
				case ItemID.MartianUniformPants://2808 Martian Uniform Pants
					return "";
				case ItemID.MartianAstroClock://2809 Martian Astro Clock
					return "";
				case ItemID.MartianBathtub://2810 Martian Bathtub
					return "";
				case ItemID.MartianBed://2811 Martian Bed
					return "";
				case ItemID.MartianHoverChair://2812 Martian Hover Chair
					return "";
				case ItemID.MartianChandelier://2813 Martian Chandelier
					return "";
				case ItemID.MartianChest://2814 Martian Chest
					return "";
				case ItemID.MartianDoor://2815 Martian Door
					return "";
				case ItemID.MartianDresser://2816 Martian Dresser
					return "";
				case ItemID.MartianHolobookcase://2817 Martian Holobookcase
					return "";
				case ItemID.MartianHoverCandle://2818 Martian Hover Candle
					return "";
				case ItemID.MartianLamppost://2819 Martian Lamppost
					return "";
				case ItemID.MartianLantern://2820 Martian Lantern
					return "";
				case ItemID.MartianPiano://2821 Martian Piano
					return "";
				case ItemID.MartianPlatform://2822 Martian Platform
					return "";
				case ItemID.MartianSofa://2823 Martian Sofa
					return "";
				case ItemID.MartianTable://2824 Martian Table
					return "";
				case ItemID.MartianTableLamp://2825 Martian Table Lamp
					return "";
				case ItemID.MartianWorkBench://2826 Martian Work Bench
					return "";
				case ItemID.WoodenSink://2827 Wooden Sink
					return "";
				case ItemID.EbonwoodSink://2828 Ebonwood Sink
					return "";
				case ItemID.RichMahoganySink://2829 Rich Mahogany Sink
					return "";
				case ItemID.PearlwoodSink://2830 Pearlwood Sink
					return "";
				case ItemID.BoneSink://2831 Bone Sink
					return "";
				case ItemID.FleshSink://2832 Flesh Sink
					return "";
				case ItemID.LivingWoodSink://2833 Living Wood Sink
					return "";
				case ItemID.SkywareSink://2834 Skyware Sink
					return "";
				case ItemID.ShadewoodSink://2835 Shadewood Sink
					return "";
				case ItemID.LihzahrdSink://2836 Lihzahrd Sink
					return "";
				case ItemID.BlueDungeonSink://2837 Blue Dungeon Sink
					return "";
				case ItemID.GreenDungeonSink://2838 Green Dungeon Sink
					return "";
				case ItemID.PinkDungeonSink://2839 Pink Dungeon Sink
					return "";
				case ItemID.ObsidianSink://2840 Obsidian Sink
					return "";
				case ItemID.MetalSink://2841 Metal Sink
					return "";
				case ItemID.GlassSink://2842 Glass Sink
					return "";
				case ItemID.GoldenSink://2843 Golden Sink
					return "";
				case ItemID.HoneySink://2844 Honey Sink
					return "";
				case ItemID.SteampunkSink://2845 Steampunk Sink
					return "";
				case ItemID.PumpkinSink://2846 Pumpkin Sink
					return "";
				case ItemID.SpookySink://2847 Spooky Sink
					return "";
				case ItemID.FrozenSink://2848 Frozen Sink
					return "";
				case ItemID.DynastySink://2849 Dynasty Sink
					return "";
				case ItemID.PalmWoodSink://2850 Palm Wood Sink
					return "";
				case ItemID.MushroomSink://2851 Mushroom Sink
					return "";
				case ItemID.BorealWoodSink://2852 Boreal Wood Sink
					return "";
				case ItemID.SlimeSink://2853 Slime Sink
					return "";
				case ItemID.CactusSink://2854 Cactus Sink
					return "";
				case ItemID.MartianSink://2855 Martian Sink
					return "";
				case ItemID.WhiteLunaticHood://2856 Solar Cultist Hood
					return "";
				case ItemID.BlueLunaticHood://2857 Lunar Cultist Hood
					return "";
				case ItemID.WhiteLunaticRobe://2858 Solar Cultist Robe
					return "";
				case ItemID.BlueLunaticRobe://2859 Lunar Cultist Robe
					return "";
				case ItemID.MartianConduitPlating://2860 Martian Conduit Plating
					return "";
				case ItemID.MartianConduitWall://2861 Martian Conduit Wall
					return "";
				case ItemID.HiTekSunglasses://2862 HiTek Sunglasses
					return "";
				case ItemID.MartianHairDye://2863 Martian Hair Dye
					return "";
				case ItemID.MartianArmorDye://2864 Martian Dye
					return "";
				case ItemID.PaintingCastleMarsberg://2865 Castle Marsberg
					return "";
				case ItemID.PaintingMartiaLisa://2866 Martia Lisa
					return "";
				case ItemID.PaintingTheTruthIsUpThere://2867 The Truth Is Up There
					return "";
				case ItemID.SmokeBlock://2868 Smoke Block
					return "";
				case ItemID.LivingFlameDye://2869 Living Flame Dye
					return "";
				case ItemID.LivingRainbowDye://2870 Living Rainbow Dye
					return "";
				case ItemID.ShadowDye://2871 Shadow Dye
					return "";
				case ItemID.NegativeDye://2872 Negative Dye
					return "";
				case ItemID.LivingOceanDye://2873 Living Ocean Dye
					return "";
				case ItemID.BrownDye://2874 Brown Dye
					return "";
				case ItemID.BrownAndBlackDye://2875 Brown and Black Dye
					return "";
				case ItemID.BrightBrownDye://2876 Bright Brown Dye
					return "";
				case ItemID.BrownAndSilverDye://2877 Brown and Silver Dye
					return "";
				case ItemID.WispDye://2878 Wisp Dye
					return "";
				case ItemID.PixieDye://2879 Pixie Dye
					return "";
				case ItemID.InfluxWaver://2880 Influx Waver
					return "";
				case ItemID.PhasicWarpEjector://2881 
					return "";
				case ItemID.ChargedBlasterCannon://2882 Charged Blaster Cannon
					return "";
				case ItemID.ChlorophyteDye://2883 Chlorophyte Dye
					return "";
				case ItemID.UnicornWispDye://2884 Unicorn Wisp Dye
					return "";
				case ItemID.InfernalWispDye://2885 Infernal Wisp Dye
					return "";
				case ItemID.ViciousPowder://2886 Vicious Powder
					return "";
				case ItemID.ViciousMushroom://2887 Vicious Mushroom
					return "";
				case ItemID.BeesKnees://2888 The Bee's Knees
					return "";
				case ItemID.GoldBird://2889 Gold Bird
					return "";
				case ItemID.GoldBunny://2890 Gold Bunny
					return "";
				case ItemID.GoldButterfly://2891 Gold Butterfly
					return "";
				case ItemID.GoldFrog://2892 Gold Frog
					return "";
				case ItemID.GoldGrasshopper://2893 Gold Grasshopper
					return "";
				case ItemID.GoldMouse://2894 Gold Mouse
					return "";
				case ItemID.GoldWorm://2895 Gold Worm
					return "";
				case ItemID.StickyDynamite://2896 Sticky Dynamite
					return "";
				case ItemID.AngryTrapperBanner://2897 Angry Trapper Banner
					return "";
				case ItemID.ArmoredVikingBanner://2898 Armored Viking Banner
					return "";
				case ItemID.BlackSlimeBanner://2899 Black Slime Banner
					return "";
				case ItemID.BlueArmoredBonesBanner://2900 Blue Armored Bones Banner
					return "";
				case ItemID.BlueCultistArcherBanner://2901 Blue Cultist Archer Banner
					return "";
				case ItemID.BlueCultistCasterBanner://2902 Lunatic Devotee Banner
					return "";
				case ItemID.BlueCultistFighterBanner://2903 Blue Cultist Fighter Banner
					return "";
				case ItemID.BoneLeeBanner://2904 Bone Lee Banner
					return "";
				case ItemID.ClingerBanner://2905 Clinger Banner
					return "";
				case ItemID.CochinealBeetleBanner://2906 Cochineal Beetle Banner
					return "";
				case ItemID.CorruptPenguinBanner://2907 Corrupt Penguin Banner
					return "";
				case ItemID.CorruptSlimeBanner://2908 Corrupt Slime Banner
					return "";
				case ItemID.CorruptorBanner://2909 Corruptor Banner
					return "";
				case ItemID.CrimslimeBanner://2910 Crimslime Banner
					return "";
				case ItemID.CursedSkullBanner://2911 Cursed Skull Banner
					return "";
				case ItemID.CyanBeetleBanner://2912 Cyan Beetle Banner
					return "";
				case ItemID.DevourerBanner://2913 Devourer Banner
					return "";
				case ItemID.DiablolistBanner://2914 Diabolist Banner
					return "";
				case ItemID.DoctorBonesBanner://2915 Doctor Bones Banner
					return "";
				case ItemID.DungeonSlimeBanner://2916 Dungeon Slime Banner
					return "";
				case ItemID.DungeonSpiritBanner://2917 Dungeon Spirit Banner
					return "";
				case ItemID.ElfArcherBanner://2918 Elf Archer Banner
					return "";
				case ItemID.ElfCopterBanner://2919 Elf Copter Banner
					return "";
				case ItemID.EyezorBanner://2920 Eyezor Banner
					return "";
				case ItemID.FlockoBanner://2921 Flocko Banner
					return "";
				case ItemID.GhostBanner://2922 Ghost Banner
					return "";
				case ItemID.GiantBatBanner://2923 Giant Bat Banner
					return "";
				case ItemID.GiantCursedSkullBanner://2924 Giant Cursed Skull Banner
					return "";
				case ItemID.GiantFlyingFoxBanner://2925 Giant Flying Fox Banner
					return "";
				case ItemID.GingerbreadManBanner://2926 Gingerbread Man Banner
					return "";
				case ItemID.GoblinArcherBanner://2927 Goblin Archer Banner
					return "";
				case ItemID.GreenSlimeBanner://2928 Green Slime Banner
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9b/Green_Slime_Banner_%28placed%29.png/revision/latest?cb=20150705170028&format=original";
				case ItemID.HeadlessHorsemanBanner://2929 Headless Horseman Banner
					return "";
				case ItemID.HellArmoredBonesBanner://2930 Hell Armored Bones Banner
					return "";
				case ItemID.HellhoundBanner://2931 Hellhound Banner
					return "";
				case ItemID.HoppinJackBanner://2932 Hoppin' Jack Banner
					return "";
				case ItemID.IceBatBanner://2933 Ice Bat Banner
					return "";
				case ItemID.IceGolemBanner://2934 Ice Golem Banner
					return "";
				case ItemID.IceSlimeBanner://2935 Ice Slime Banner
					return "";
				case ItemID.IchorStickerBanner://2936 Ichor Sticker Banner
					return "";
				case ItemID.IlluminantBatBanner://2937 Illuminant Bat Banner
					return "";
				case ItemID.IlluminantSlimeBanner://2938 Illuminant Slime Banner
					return "";
				case ItemID.JungleBatBanner://2939 Jungle Bat Banner
					return "";
				case ItemID.JungleSlimeBanner://2940 Jungle Slime Banner
					return "";
				case ItemID.KrampusBanner://2941 Krampus Banner
					return "";
				case ItemID.LacBeetleBanner://2942 Lac Beetle Banner
					return "";
				case ItemID.LavaBatBanner://2943 Lava Bat Banner
					return "";
				case ItemID.LavaSlimeBanner://2944 Lava Slime Banner
					return "";
				case ItemID.MartianBrainscramblerBanner://2945 Martian Brainscrambler Banner
					return "";
				case ItemID.MartianDroneBanner://2946 Martian Drone Banner
					return "";
				case ItemID.MartianEngineerBanner://2947 Martian Engineer Banner
					return "";
				case ItemID.MartianGigazapperBanner://2948 Martian Gigazapper Banner
					return "";
				case ItemID.MartianGreyGruntBanner://2949 Martian Gray Grunt Banner
					return "";
				case ItemID.MartianOfficerBanner://2950 Martian Officer Banner
					return "";
				case ItemID.MartianRaygunnerBanner://2951 Martian Ray Gunner Banner
					return "";
				case ItemID.MartianScutlixGunnerBanner://2952 Martian Scutlix Gunner Banner
					return "";
				case ItemID.MartianTeslaTurretBanner://2953 Martian Tesla Turret Banner
					return "";
				case ItemID.MisterStabbyBanner://2954 Mister Stabby Banner
					return "";
				case ItemID.MotherSlimeBanner://2955 Mother Slime Banner
					return "";
				case ItemID.NecromancerBanner://2956 Necromancer Banner
					return "";
				case ItemID.NutcrackerBanner://2957 Nutcracker Banner
					return "";
				case ItemID.PaladinBanner://2958 Paladin Banner
					return "";
				case ItemID.PenguinBanner://2959 Penguin Banner
					return "";
				case ItemID.PinkyBanner://2960 Pinky Banner
					return "";
				case ItemID.PoltergeistBanner://2961 Poltergeist Banner
					return "";
				case ItemID.PossessedArmorBanner://2962 Possessed Armor Banner
					return "";
				case ItemID.PresentMimicBanner://2963 Present Mimic Banner
					return "";
				case ItemID.PurpleSlimeBanner://2964 Purple Slime Banner
					return "";
				case ItemID.RaggedCasterBanner://2965 Ragged Caster Banner
					return "";
				case ItemID.RainbowSlimeBanner://2966 Rainbow Slime Banner
					return "";
				case ItemID.RavenBanner://2967 Raven Banner
					return "";
				case ItemID.RedSlimeBanner://2968 Red Slime Banner
					return "";
				case ItemID.RuneWizardBanner://2969 Rune Wizard Banner
					return "";
				case ItemID.RustyArmoredBonesBanner://2970 Rusty Armored Bones Banner
					return "";
				case ItemID.ScarecrowBanner://2971 Scarecrow Banner
					return "";
				case ItemID.ScutlixBanner://2972 Scutlix Banner
					return "";
				case ItemID.SkeletonArcherBanner://2973 Skeleton Archer Banner
					return "";
				case ItemID.SkeletonCommandoBanner://2974 Skeleton Commando Banner
					return "";
				case ItemID.SkeletonSniperBanner://2975 Skeleton Sniper Banner
					return "";
				case ItemID.SlimerBanner://2976 Slimer Banner
					return "";
				case ItemID.SnatcherBanner://2977 Snatcher Banner
					return "";
				case ItemID.SnowBallaBanner://2978 Snow Balla Banner
					return "";
				case ItemID.SnowmanGangstaBanner://2979 Snowman Gangsta Banner
					return "";
				case ItemID.SpikedIceSlimeBanner://2980 Spiked Ice Slime Banner
					return "";
				case ItemID.SpikedJungleSlimeBanner://2981 Spiked Jungle Slime Banner
					return "";
				case ItemID.SplinterlingBanner://2982 Splinterling Banner
					return "";
				case ItemID.SquidBanner://2983 Squid Banner
					return "";
				case ItemID.TacticalSkeletonBanner://2984 Tactical Skeleton Banner
					return "";
				case ItemID.TheGroomBanner://2985 The Groom Banner
					return "";
				case ItemID.TimBanner://2986 Tim Banner
					return "";
				case ItemID.UndeadMinerBanner://2987 Undead Miner Banner
					return "";
				case ItemID.UndeadVikingBanner://2988 Undead Viking Banner
					return "";
				case ItemID.WhiteCultistArcherBanner://2989 White Cultist Archer Banner
					return "";
				case ItemID.WhiteCultistCasterBanner://2990 White Cultist Caster Banner
					return "";
				case ItemID.WhiteCultistFighterBanner://2991 White Cultist Fighter Banner
					return "";
				case ItemID.YellowSlimeBanner://2992 Yellow Slime Banner
					return "";
				case ItemID.YetiBanner://2993 Yeti Banner
					return "";
				case ItemID.ZombieElfBanner://2994 Zombie Elf Banner
					return "";
				case ItemID.SparkyPainting://2995 Sparky
					return "";
				case ItemID.VineRope://2996 Vine Rope
					return "";
				case ItemID.WormholePotion://2997 Wormhole Potion
					return "";
				case ItemID.SummonerEmblem://2998 Summoner Emblem
					return "";
				case ItemID.BewitchingTable://2999 Bewitching Table
					return "";
				case ItemID.AlchemyTable://3000 Alchemy Table
					return "";
				case ItemID.StrangeBrew://3001 Strange Brew
					return "";
				case ItemID.SpelunkerGlowstick://3002 Spelunker Glowstick
					return "";
				case ItemID.BoneArrow://3003 Bone Arrow
					return "";
				case ItemID.BoneTorch://3004 Bone Torch
					return "";
				case ItemID.VineRopeCoil://3005 Vine Rope Coil
					return "";
				case ItemID.SoulDrain://3006 Life Drain
					return "";
				case ItemID.DartPistol://3007 Dart Pistol
					return "";
				case ItemID.DartRifle://3008 Dart Rifle
					return "";
				case ItemID.CrystalDart://3009 Crystal Dart
					return "";
				case ItemID.CursedDart://3010 Cursed Dart
					return "";
				case ItemID.IchorDart://3011 Ichor Dart
					return "";
				case ItemID.ChainGuillotines://3012 Chain Guillotines
					return "";
				case ItemID.FetidBaghnakhs://3013 Fetid Baghnakhs
					return "";
				case ItemID.ClingerStaff://3014 Clinger Staff
					return "";
				case ItemID.PutridScent://3015 Putrid Scent
					return "";
				case ItemID.FleshKnuckles://3016 Flesh Knuckles
					return "";
				case ItemID.FlowerBoots://3017 Flower Boots
					return "";
				case ItemID.Seedler://3018 Seedler
					return "";
				case ItemID.HellwingBow://3019 Hellwing Bow
					return "";
				case ItemID.TendonHook://3020 Tendon Hook
					return "";
				case ItemID.ThornHook://3021 Thorn Hook
					return "";
				case ItemID.IlluminantHook://3022 Illuminant Hook
					return "";
				case ItemID.WormHook://3023 Worm Hook
					return "";
				case ItemID.DevDye://3024 Skiphs' Blood
					return "";
				case ItemID.PurpleOozeDye://3025 Purple Ooze Dye
					return "";
				case ItemID.ReflectiveSilverDye://3026 Reflective Silver Dye
					return "";
				case ItemID.ReflectiveGoldDye://3027 Reflective Gold Dye
					return "";
				case ItemID.BlueAcidDye://3028 Blue Acid Dye
					return "";
				case ItemID.DaedalusStormbow://3029 Daedalus Stormbow
					return "";
				case ItemID.FlyingKnife://3030 Flying Knife
					return "";
				case ItemID.BottomlessBucket://3031 Bottomless Water Bucket
					return "";
				case ItemID.SuperAbsorbantSponge://3032 Super Absorbant Sponge
					return "";
				case ItemID.GoldRing://3033 Gold Ring
					return "";
				case ItemID.CoinRing://3034 Coin Ring
					return "";
				case ItemID.GreedyRing://3035 Greedy Ring
					return "";
				case ItemID.FishFinder://3036 Fish Finder
					return "";
				case ItemID.WeatherRadio://3037 Weather Radio
					return "";
				case ItemID.HadesDye://3038 Hades Dye
					return "";
				case ItemID.TwilightDye://3039 Twilight Dye
					return "";
				case ItemID.AcidDye://3040 Acid Dye
					return "";
				case ItemID.MushroomDye://3041 Glowing Mushroom Dye
					return "";
				case ItemID.PhaseDye://3042 Phase Dye
					return "";
				case ItemID.MagicLantern://3043 Magic Lantern
					return "";
				case ItemID.MusicBoxLunarBoss://3044 Music Box (Lunar Boss)
					return "";
				case ItemID.RainbowTorch://3045 Rainbow Torch
					return "";
				case ItemID.CursedCampfire://3046 Cursed Campfire
					return "";
				case ItemID.DemonCampfire://3047 Demon Campfire
					return "";
				case ItemID.FrozenCampfire://3048 Frozen Campfire
					return "";
				case ItemID.IchorCampfire://3049 Ichor Campfire
					return "";
				case ItemID.RainbowCampfire://3050 Rainbow Campfire
					return "";
				case ItemID.CrystalVileShard://3051 Crystal Vile Shard
					return "";
				case ItemID.ShadowFlameBow://3052 Shadowflame Bow
					return "";
				case ItemID.ShadowFlameHexDoll://3053 Shadowflame Hex Doll
					return "";
				case ItemID.ShadowFlameKnife://3054 Shadowflame Knife
					return "";
				case ItemID.PaintingAcorns://3055 Acorns
					return "";
				case ItemID.PaintingColdSnap://3056 Cold Snap
					return "";
				case ItemID.PaintingCursedSaint://3057 Cursed Saint
					return "";
				case ItemID.PaintingSnowfellas://3058 Snowfellas
					return "";
				case ItemID.PaintingTheSeason://3059 The Season
					return "";
				case ItemID.BoneRattle://3060 Bone Rattle
					return "";
				case ItemID.ArchitectGizmoPack://3061 Architect Gizmo Pack
					return "";
				case ItemID.CrimsonHeart://3062 Crimson Heart
					return "";
				case ItemID.Meowmere://3063 Meowmere
					return "";
				case ItemID.Sundial://3064 Enchanted Sundial
					return "";
				case ItemID.StarWrath://3065 Star Wrath
					return "";
				case ItemID.MarbleBlock://3066 Smooth Marble Block
					return "";
				case ItemID.HellstoneBrickWall://3067 Hellstone Brick Wall
					return "";
				case ItemID.CordageGuide://3068 Guide to Plant Fiber Cordage
					return "";
				case ItemID.WandofSparking://3069 Wand of Sparking
					return "";
				case ItemID.GoldBirdCage://3070 Gold Bird Cage
					return "";
				case ItemID.GoldBunnyCage://3071 Gold Bunny Cage
					return "";
				case ItemID.GoldButterflyCage://3072 Gold Butterfly Jar
					return "";
				case ItemID.GoldFrogCage://3073 Gold Frog Cage
					return "";
				case ItemID.GoldGrasshopperCage://3074 Gold Grasshopper Cage
					return "";
				case ItemID.GoldMouseCage://3075 Gold Mouse Cage
					return "";
				case ItemID.GoldWormCage://3076 Gold Worm Cage
					return "";
				case ItemID.SilkRope://3077 Silk Rope
					return "";
				case ItemID.WebRope://3078 Web Rope
					return "";
				case ItemID.SilkRopeCoil://3079 Silk Rope Coil
					return "";
				case ItemID.WebRopeCoil://3080 Web Rope Coil
					return "";
				case ItemID.Marble://3081 Marble Block
					return "";
				case ItemID.MarbleWall://3082 Marble Wall
					return "";
				case ItemID.MarbleBlockWall://3083 Smooth Marble Wall
					return "";
				case ItemID.Radar://3084 Radar
					return "";
				case ItemID.LockBox://3085 Golden Lock Box
					return "";
				case ItemID.Granite://3086 Granite Block
					return "";
				case ItemID.GraniteBlock://3087 Smooth Granite Block
					return "";
				case ItemID.GraniteWall://3088 Granite Wall
					return "";
				case ItemID.GraniteBlockWall://3089 Smooth Granite Wall
					return "";
				case ItemID.RoyalGel://3090 Royal Gel
					return "";
				case ItemID.NightKey://3091 Key of Night
					return "";
				case ItemID.LightKey://3092 Key of Light
					return "";
				case ItemID.HerbBag://3093 Herb Bag
					return "";
				case ItemID.Javelin://3094 Javelin
					return "";
				case ItemID.TallyCounter://3095 Tally Counter
					return "";
				case ItemID.Sextant://3096 Sextant
					return "";
				case ItemID.EoCShield://3097 Shield of Cthulhu
					return "";
				case ItemID.ButchersChainsaw://3098 Butcher's Chainsaw
					return "";
				case ItemID.Stopwatch://3099 Stopwatch
					return "";
				case ItemID.MeteoriteBrick://3100 Meteorite Brick
					return "";
				case ItemID.MeteoriteBrickWall://3101 Meteorite Brick Wall
					return "";
				case ItemID.MetalDetector://3102 Metal Detector
					return "";
				case ItemID.EndlessQuiver://3103 Endless Quiver
					return "";
				case ItemID.EndlessMusketPouch://3104 Endless Musket Pouch
					return "";
				case ItemID.ToxicFlask://3105 Toxic Flask
					return "";
				case ItemID.PsychoKnife://3106 Psycho Knife
					return "";
				case ItemID.NailGun://3107 Nail Gun
					return "";
				case ItemID.Nail://3108 Nail
					return "";
				case ItemID.NightVisionHelmet://3109 Night Vision Helmet
					return "";
				case ItemID.CelestialShell://3110 Celestial Shell
					return "";
				case ItemID.PinkGel://3111 Pink Gel
					return "";
				case ItemID.BouncyGlowstick://3112 Bouncy Glowstick
					return "";
				case ItemID.PinkSlimeBlock://3113 Pink Slime Block
					return "";
				case ItemID.PinkTorch://3114 Pink Torch
					return "";
				case ItemID.BouncyBomb://3115 Bouncy Bomb
					return "";
				case ItemID.BouncyGrenade://3116 Bouncy Grenade
					return "";
				case ItemID.PeaceCandle://3117 Peace Candle
					return "";
				case ItemID.LifeformAnalyzer://3118 Lifeform Analyzer
					return "";
				case ItemID.DPSMeter://3119 DPS Meter
					return "";
				case ItemID.FishermansGuide://3120 Fisherman's Pocket Guide
					return "";
				case ItemID.GoblinTech://3121 Goblin Tech
					return "";
				case ItemID.REK://3122 R.E.K. 3000
					return "";
				case ItemID.PDA://3123 PDA
					return "";
				case ItemID.CellPhone://3124 Cell Phone
					return "";
				case ItemID.GraniteChest://3125 Granite Chest
					return "";
				case ItemID.MeteoriteClock://3126 Meteorite Clock
					return "";
				case ItemID.MarbleClock://3127 Marble Clock
					return "";
				case ItemID.GraniteClock://3128 Granite Clock
					return "";
				case ItemID.MeteoriteDoor://3129 Meteorite Door
					return "";
				case ItemID.MarbleDoor://3130 Marble Door
					return "";
				case ItemID.GraniteDoor://3131 Granite Door
					return "";
				case ItemID.MeteoriteDresser://3132 Meteorite Dresser
					return "";
				case ItemID.MarbleDresser://3133 Marble Dresser
					return "";
				case ItemID.GraniteDresser://3134 Granite Dresser
					return "";
				case ItemID.MeteoriteLamp://3135 Meteorite Lamp
					return "";
				case ItemID.MarbleLamp://3136 Marble Lamp
					return "";
				case ItemID.GraniteLamp://3137 Granite Lamp
					return "";
				case ItemID.MeteoriteLantern://3138 Meteorite Lantern
					return "";
				case ItemID.MarbleLantern://3139 Marble Lantern
					return "";
				case ItemID.GraniteLantern://3140 Granite Lantern
					return "";
				case ItemID.MeteoritePiano://3141 Meteorite Piano
					return "";
				case ItemID.MarblePiano://3142 Marble Piano
					return "";
				case ItemID.GranitePiano://3143 Granite Piano
					return "";
				case ItemID.MeteoritePlatform://3144 Meteorite Platform
					return "";
				case ItemID.MarblePlatform://3145 Marble Platform
					return "";
				case ItemID.GranitePlatform://3146 Granite Platform
					return "";
				case ItemID.MeteoriteSink://3147 Meteorite Sink
					return "";
				case ItemID.MarbleSink://3148 Marble Sink
					return "";
				case ItemID.GraniteSink://3149 Granite Sink
					return "";
				case ItemID.MeteoriteSofa://3150 Meteorite Sofa
					return "";
				case ItemID.MarbleSofa://3151 Marble Sofa
					return "";
				case ItemID.GraniteSofa://3152 Granite Sofa
					return "";
				case ItemID.MeteoriteTable://3153 Meteorite Table
					return "";
				case ItemID.MarbleTable://3154 Marble Table
					return "";
				case ItemID.GraniteTable://3155 Granite Table
					return "";
				case ItemID.MeteoriteWorkBench://3156 Meteorite Work Bench
					return "";
				case ItemID.MarbleWorkBench://3157 Marble Work Bench
					return "";
				case ItemID.GraniteWorkBench://3158 Granite Work Bench
					return "";
				case ItemID.MeteoriteBathtub://3159 Meteorite Bathtub
					return "";
				case ItemID.MarbleBathtub://3160 Marble Bathtub
					return "";
				case ItemID.GraniteBathtub://3161 Granite Bathtub
					return "";
				case ItemID.MeteoriteBed://3162 Meteorite Bed
					return "";
				case ItemID.MarbleBed://3163 Marble Bed
					return "";
				case ItemID.GraniteBed://3164 Granite Bed
					return "";
				case ItemID.MeteoriteBookcase://3165 Meteorite Bookcase
					return "";
				case ItemID.MarbleBookcase://3166 Marble Bookcase
					return "";
				case ItemID.GraniteBookcase://3167 Granite Bookcase
					return "";
				case ItemID.MeteoriteCandelabra://3168 Meteorite Candelabra
					return "";
				case ItemID.MarbleCandelabra://3169 Marble Candelabra
					return "";
				case ItemID.GraniteCandelabra://3170 Granite Candelabra
					return "";
				case ItemID.MeteoriteCandle://3171 Meteorite Candle
					return "";
				case ItemID.MarbleCandle://3172 Marble Candle
					return "";
				case ItemID.GraniteCandle://3173 Granite Candle
					return "";
				case ItemID.MeteoriteChair://3174 Meteorite Chair
					return "";
				case ItemID.MarbleChair://3175 Marble Chair
					return "";
				case ItemID.GraniteChair://3176 Granite Chair
					return "";
				case ItemID.MeteoriteChandelier://3177 Meteorite Chandelier
					return "";
				case ItemID.MarbleChandelier://3178 Marble Chandelier
					return "";
				case ItemID.GraniteChandelier://3179 Granite Chandelier
					return "";
				case ItemID.MeteoriteChest://3180 Meteorite Chest
					return "";
				case ItemID.MarbleChest://3181 Marble Chest
					return "";
				case ItemID.MagicWaterDropper://3182 Magic Water Dropper
					return "";
				case ItemID.GoldenBugNet://3183 Golden Bug Net
					return "";
				case ItemID.MagicLavaDropper://3184 Magic Lava Dropper
					return "";
				case ItemID.MagicHoneyDropper://3185 Magic Honey Dropper
					return "";
				case ItemID.EmptyDropper://3186 Empty Dropper
					return "";
				case ItemID.GladiatorHelmet://3187 Gladiator Helmet
					return "";
				case ItemID.GladiatorBreastplate://3188 Gladiator Breastplate
					return "";
				case ItemID.GladiatorLeggings://3189 Gladiator Leggings
					return "";
				case ItemID.ReflectiveDye://3190 Reflective Dye
					return "";
				case ItemID.EnchantedNightcrawler://3191 Enchanted Nightcrawler
					return "";
				case ItemID.Grubby://3192 Grubby
					return "";
				case ItemID.Sluggy://3193 Sluggy
					return "";
				case ItemID.Buggy://3194 Buggy
					return "";
				case ItemID.GrubSoup://3195 Grub Soup
					return "";
				case ItemID.BombFish://3196 Bomb Fish
					return "";
				case ItemID.FrostDaggerfish://3197 Frost Daggerfish
					return "";
				case ItemID.SharpeningStation://3198 Sharpening Station
					return "";
				case ItemID.IceMirror://3199 Ice Mirror
					return "";
				case ItemID.SailfishBoots://3200 Sailfish Boots
					return "";
				case ItemID.TsunamiInABottle://3201 Tsunami in a Bottle
					return "";
				case ItemID.TargetDummy://3202 Target Dummy
					return "";
				case ItemID.CorruptFishingCrate://3203 Corrupt Crate
					return "";
				case ItemID.CrimsonFishingCrate://3204 Crimson Crate
					return "";
				case ItemID.DungeonFishingCrate://3205 Dungeon Crate
					return "";
				case ItemID.FloatingIslandFishingCrate://3206 Sky Crate
					return "";
				case ItemID.HallowedFishingCrate://3207 Hallowed Crate
					return "";
				case ItemID.JungleFishingCrate://3208 Jungle Crate
					return "";
				case ItemID.CrystalSerpent://3209 Crystal Serpent
					return "";
				case ItemID.Toxikarp://3210 Toxikarp
					return "";
				case ItemID.Bladetongue://3211 Bladetongue
					return "";
				case ItemID.SharkToothNecklace://3212 Shark Tooth Necklace
					return "";
				case ItemID.MoneyTrough://3213 Money Trough
					return "";
				case ItemID.Bubble://3214 Bubble
					return "";
				case ItemID.DayBloomPlanterBox://3215 Daybloom Planter Box
					return "";
				case ItemID.MoonglowPlanterBox://3216 Moonglow Planter Box
					return "";
				case ItemID.CorruptPlanterBox://3217 Deathweed Planter Box
					return "";
				case ItemID.CrimsonPlanterBox://3218 Deathweed Planter Box
					return "";
				case ItemID.BlinkrootPlanterBox://3219 Blinkroot Planter Box
					return "";
				case ItemID.WaterleafPlanterBox://3220 Waterleaf Planter Box
					return "";
				case ItemID.ShiverthornPlanterBox://3221 Shiverthorn Planter Box
					return "";
				case ItemID.FireBlossomPlanterBox://3222 Fireblossom Planter Box
					return "";
				case ItemID.BrainOfConfusion://3223 Brain of Confusion
					return "";
				case ItemID.WormScarf://3224 Worm Scarf
					return "";
				case ItemID.BalloonPufferfish://3225 Balloon Pufferfish
					return "";
				case ItemID.BejeweledValkyrieHead://3226 Lazure's Valkyrie Circlet
					return "";
				case ItemID.BejeweledValkyrieBody://3227 Lazure's Valkyrie Cloak
					return "";
				case ItemID.BejeweledValkyrieWing://3228 Lazure's Barrier Platform
					return "";
				case ItemID.RichGravestone1://3229 Golden Cross Grave Marker
					return "";
				case ItemID.RichGravestone2://3230 Golden Tombstone
					return "";
				case ItemID.RichGravestone3://3231 Golden Grave Marker
					return "";
				case ItemID.RichGravestone4://3232 Golden Gravestone
					return "";
				case ItemID.RichGravestone5://3233 Golden Headstone
					return "";
				case ItemID.CrystalBlock://3234 Crystal Block
					return "";
				case ItemID.MusicBoxMartians://3235 Music Box (Martian Madness)
					return "";
				case ItemID.MusicBoxPirates://3236 Music Box (Pirate Invasion)
					return "";
				case ItemID.MusicBoxHell://3237 Music Box (Hell)
					return "";
				case ItemID.CrystalBlockWall://3238 Crystal Block Wall
					return "";
				case ItemID.Trapdoor://3239 Trap Door
					return "";
				case ItemID.TallGate://3240 Tall Gate
					return "";
				case ItemID.SharkronBalloon://3241 Sharkron Balloon
					return "";
				case ItemID.TaxCollectorHat://3242 Tax Collector's Hat
					return "";
				case ItemID.TaxCollectorSuit://3243 Tax Collector's Suit
					return "";
				case ItemID.TaxCollectorPants://3244 Tax Collector's Pants
					return "";
				case ItemID.BoneGlove://3245 Bone Glove
					return "";
				case ItemID.ClothierJacket://3246 Clothier's Jacket
					return "";
				case ItemID.ClothierPants://3247 Clothier's Pants
					return "";
				case ItemID.DyeTraderTurban://3248 Dye Trader's Turban
					return "";
				case ItemID.DeadlySphereStaff://3249 Deadly Sphere Staff
					return "";
				case ItemID.BalloonHorseshoeFart://3250 Green Horseshoe Balloon
					return "";
				case ItemID.BalloonHorseshoeHoney://3251 Amber Horseshoe Balloon
					return "";
				case ItemID.BalloonHorseshoeSharkron://3252 Pink Horseshoe Balloon
					return "";
				case ItemID.LavaLamp://3253 Lava Lamp
					return "";
				case ItemID.CageEnchantedNightcrawler://3254 Enchanted Nightcrawler Cage
					return "";
				case ItemID.CageBuggy://3255 Buggy Cage
					return "";
				case ItemID.CageGrubby://3256 Grubby Cage
					return "";
				case ItemID.CageSluggy://3257 Sluggy Cage
					return "";
				case ItemID.SlapHand://3258 Slap Hand
					return "";
				case ItemID.TwilightHairDye://3259 Twilight Hair Dye
					return "";
				case ItemID.BlessedApple://3260 Blessed Apple
					return "";
				case ItemID.SpectreBar://3261 Spectre Bar
					return "";
				case ItemID.Code1://3262 Code 1
					return "";
				case ItemID.BuccaneerBandana://3263 Buccaneer Bandana
					return "";
				case ItemID.BuccaneerShirt://3264 Buccaneer Tunic
					return "";
				case ItemID.BuccaneerPants://3265 Buccaneer Pantaloons
					return "";
				case ItemID.ObsidianHelm://3266 Obsidian Outlaw Hat
					return "";
				case ItemID.ObsidianShirt://3267 Obsidian Longcoat
					return "";
				case ItemID.ObsidianPants://3268 Obsidian Pants
					return "";
				case ItemID.MedusaHead://3269 Medusa Head
					return "";
				case ItemID.ItemFrame://3270 Item Frame
					return "";
				case ItemID.Sandstone://3271 Sandstone Block
					return "";
				case ItemID.HardenedSand://3272 Hardened Sand Block
					return "";
				case ItemID.SandstoneWall://3273 Sandstone Wall
					return "";
				case ItemID.CorruptHardenedSand://3274 Hardened Ebonsand Block
					return "";
				case ItemID.CrimsonHardenedSand://3275 Hardened Crimsand Block
					return "";
				case ItemID.CorruptSandstone://3276 Ebonsandstone Block
					return "";
				case ItemID.CrimsonSandstone://3277 Crimsandstone Block
					return "";
				case ItemID.WoodYoyo://3278 Wooden Yoyo
					return "";
				case ItemID.CorruptYoyo://3279 Malaise
					return "";
				case ItemID.CrimsonYoyo://3280 Artery
					return "";
				case ItemID.JungleYoyo://3281 Amazon
					return "";
				case ItemID.Cascade://3282 Cascade
					return "";
				case ItemID.Chik://3283 Chik
					return "";
				case ItemID.Code2://3284 Code 2
					return "";
				case ItemID.Rally://3285 Rally
					return "";
				case ItemID.Yelets://3286 Yelets
					return "";
				case ItemID.RedsYoyo://3287 Red's Throw
					return "";
				case ItemID.ValkyrieYoyo://3288 Valkyrie Yoyo
					return "";
				case ItemID.Amarok://3289 Amarok
					return "";
				case ItemID.HelFire://3290 Hel-Fire
					return "";
				case ItemID.Kraken://3291 Kraken
					return "";
				case ItemID.TheEyeOfCthulhu://3292 The Eye of Cthulhu
					return "";
				case ItemID.RedString://3293 Red String
					return "";
				case ItemID.OrangeString://3294 Orange String
					return "";
				case ItemID.YellowString://3295 Yellow String
					return "";
				case ItemID.LimeString://3296 Lime String
					return "";
				case ItemID.GreenString://3297 Green String
					return "";
				case ItemID.TealString://3298 Teal String
					return "";
				case ItemID.CyanString://3299 Cyan String
					return "";
				case ItemID.SkyBlueString://3300 Sky Blue String
					return "";
				case ItemID.BlueString://3301 Blue String
					return "";
				case ItemID.PurpleString://3302 Purple String
					return "";
				case ItemID.VioletString://3303 Violet String
					return "";
				case ItemID.PinkString://3304 Pink String
					return "";
				case ItemID.BrownString://3305 Brown String
					return "";
				case ItemID.WhiteString://3306 White String
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3e/White_String.png/revision/latest?cb=20200516223540&format=original";
				case ItemID.RainbowString://3307 Rainbow String
					return "";
				case ItemID.BlackString://3308 Black String
					return "";
				case ItemID.BlackCounterweight://3309 Black Counterweight
					return "";
				case ItemID.BlueCounterweight://3310 Blue Counterweight
					return "";
				case ItemID.GreenCounterweight://3311 Green Counterweight
					return "";
				case ItemID.PurpleCounterweight://3312 Purple Counterweight
					return "";
				case ItemID.RedCounterweight://3313 Red Counterweight
					return "";
				case ItemID.YellowCounterweight://3314 Yellow Counterweight
					return "";
				case ItemID.FormatC://3315 Format:C
					return "";
				case ItemID.Gradient://3316 Gradient
					return "";
				case ItemID.Valor://3317 Valor
					return "";
				case ItemID.KingSlimeBossBag://3318 Treasure Bag (King Slime)
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2a/Treasure_Bag.gif/revision/latest?cb=20211119015752&format=original";
				case ItemID.EyeOfCthulhuBossBag://3319 Treasure Bag (Eye of Cthulhu)
					return "";
				case ItemID.EaterOfWorldsBossBag://3320 Treasure Bag (Eater of Worlds)
					return "";
				case ItemID.BrainOfCthulhuBossBag://3321 Treasure Bag (Brain of Cthulhu)
					return "";
				case ItemID.QueenBeeBossBag://3322 Treasure Bag (Queen Bee)
					return "";
				case ItemID.SkeletronBossBag://3323 Treasure Bag (Skeletron)
					return "";
				case ItemID.WallOfFleshBossBag://3324 Treasure Bag (Wall of Flesh)
					return "";
				case ItemID.DestroyerBossBag://3325 Treasure Bag (The Destroyer)
					return "";
				case ItemID.TwinsBossBag://3326 Treasure Bag (The Twins)
					return "";
				case ItemID.SkeletronPrimeBossBag://3327 Treasure Bag (Skeletron Prime)
					return "";
				case ItemID.PlanteraBossBag://3328 Treasure Bag (Plantera)
					return "";
				case ItemID.GolemBossBag://3329 Treasure Bag (Golem)
					return "";
				case ItemID.FishronBossBag://3330 Treasure Bag (Duke Fishron)
					return "";
				case ItemID.CultistBossBag://3331 Treasure Bag (Lunatic Cultist)
					return "";
				case ItemID.MoonLordBossBag://3332 Treasure Bag (Moon Lord)
					return "";
				case ItemID.HiveBackpack://3333 Hive Pack
					return "";
				case ItemID.YoYoGlove://3334 Yoyo Glove
					return "";
				case ItemID.DemonHeart://3335 Demon Heart
					return "";
				case ItemID.SporeSac://3336 Spore Sac
					return "";
				case ItemID.ShinyStone://3337 Shiny Stone
					return "";
				case ItemID.HallowHardenedSand://3338 Hardened Pearlsand Block
					return "";
				case ItemID.HallowSandstone://3339 Pearlsandstone Block
					return "";
				case ItemID.HardenedSandWall://3340 Hardened Sand Wall
					return "";
				case ItemID.CorruptHardenedSandWall://3341 Hardened Ebonsand Wall
					return "";
				case ItemID.CrimsonHardenedSandWall://3342 Hardened Crimsand Wall
					return "";
				case ItemID.HallowHardenedSandWall://3343 Hardened Pearlsand Wall
					return "";
				case ItemID.CorruptSandstoneWall://3344 Ebonsandstone Wall
					return "";
				case ItemID.CrimsonSandstoneWall://3345 Crimsandstone Wall
					return "";
				case ItemID.HallowSandstoneWall://3346 Pearlsandstone Wall
					return "";
				case ItemID.DesertFossil://3347 Desert Fossil
					return "";
				case ItemID.DesertFossilWall://3348 Desert Fossil Wall
					return "";
				case ItemID.DyeTradersScimitar://3349 Exotic Scimitar
					return "";
				case ItemID.PainterPaintballGun://3350 Paintball Gun
					return "";
				case ItemID.TaxCollectorsStickOfDoom://3351 Classy Cane
					return "";
				case ItemID.StylistKilLaKillScissorsIWish://3352 Stylish Scissors
					return "";
				case ItemID.MinecartMech://3353 Mechanical Cart
					return "";
				case ItemID.MechanicalWheelPiece://3354 Mechanical Wheel Piece
					return "";
				case ItemID.MechanicalWagonPiece://3355 Mechanical Wagon Piece
					return "";
				case ItemID.MechanicalBatteryPiece://3356 Mechanical Battery Piece
					return "";
				case ItemID.AncientCultistTrophy://3357 Lunatic Cultist Trophy
					return "";
				case ItemID.MartianSaucerTrophy://3358 Martian Saucer Trophy
					return "";
				case ItemID.FlyingDutchmanTrophy://3359 Flying Dutchman Trophy
					return "";
				case ItemID.LivingMahoganyWand://3360 Living Mahogany Wand
					return "";
				case ItemID.LivingMahoganyLeafWand://3361 Rich Mahogany Leaf Wand
					return "";
				case ItemID.FallenTuxedoShirt://3362 Fallen Tuxedo Shirt
					return "";
				case ItemID.FallenTuxedoPants://3363 Fallen Tuxedo Pants
					return "";
				case ItemID.Fireplace://3364 Fireplace
					return "";
				case ItemID.Chimney://3365 Chimney
					return "";
				case ItemID.YoyoBag://3366 Yoyo Bag
					return "";
				case ItemID.ShrimpyTruffle://3367 Shrimpy Truffle
					return "";
				case ItemID.Arkhalis://3368 Arkhalis
					return "";
				case ItemID.ConfettiCannon://3369 Confetti Cannon
					return "";
				case ItemID.MusicBoxTowers://3370 Music Box (The Towers)
					return "";
				case ItemID.MusicBoxGoblins://3371 Music Box (Goblin Invasion)
					return "";
				case ItemID.BossMaskCultist://3372 Lunatic Cultist Mask
					return "";
				case ItemID.BossMaskMoonlord://3373 Moon Lord Mask
					return "";
				case ItemID.FossilHelm://3374 Fossil Helmet
					return "";
				case ItemID.FossilShirt://3375 Fossil Plate
					return "";
				case ItemID.FossilPants://3376 Fossil Greaves
					return "";
				case ItemID.AmberStaff://3377 Amber Staff
					return "";
				case ItemID.BoneJavelin://3378 Bone Javelin
					return "";
				case ItemID.BoneDagger://3379 Bone Throwing Knife
					return "";
				case ItemID.FossilOre://3380 Sturdy Fossil
					return "";
				case ItemID.StardustHelmet://3381 Stardust Helmet
					return "";
				case ItemID.StardustBreastplate://3382 Stardust Plate
					return "";
				case ItemID.StardustLeggings://3383 Stardust Leggings
					return "";
				case ItemID.PortalGun://3384 Portal Gun
					return "";
				case ItemID.StrangePlant1://3385 Strange Plant
					return "";
				case ItemID.StrangePlant2://3386 Strange Plant
					return "";
				case ItemID.StrangePlant3://3387 Strange Plant
					return "";
				case ItemID.StrangePlant4://3388 Strange Plant
					return "";
				case ItemID.Terrarian://3389 Terrarian
					return "";
				case ItemID.GoblinSummonerBanner://3390 Goblin Warlock Banner
					return "";
				case ItemID.SalamanderBanner://3391 Salamander Banner
					return "";
				case ItemID.GiantShellyBanner://3392 Giant Shelly Banner
					return "";
				case ItemID.CrawdadBanner://3393 Crawdad Banner
					return "";
				case ItemID.FritzBanner://3394 Fritz Banner
					return "";
				case ItemID.CreatureFromTheDeepBanner://3395 Creature From The Deep Banner
					return "";
				case ItemID.DrManFlyBanner://3396 Dr. Man Fly Banner
					return "";
				case ItemID.MothronBanner://3397 Mothron Banner
					return "";
				case ItemID.SeveredHandBanner://3398 Severed Hand Banner
					return "";
				case ItemID.ThePossessedBanner://3399 The Possessed Banner
					return "";
				case ItemID.ButcherBanner://3400 Butcher Banner
					return "";
				case ItemID.PsychoBanner://3401 Psycho Banner
					return "";
				case ItemID.DeadlySphereBanner://3402 Deadly Sphere Banner
					return "";
				case ItemID.NailheadBanner://3403 Nailhead Banner
					return "";
				case ItemID.PoisonousSporeBanner://3404 Poisonous Spore Banner
					return "";
				case ItemID.MedusaBanner://3405 Medusa Banner
					return "";
				case ItemID.GreekSkeletonBanner://3406 Hoplite Banner
					return "";
				case ItemID.GraniteFlyerBanner://3407 Granite Elemental Banner
					return "";
				case ItemID.GraniteGolemBanner://3408 Granite Golem Banner
					return "";
				case ItemID.BloodZombieBanner://3409 Blood Zombie Banner
					return "";
				case ItemID.DripplerBanner://3410 Drippler Banner
					return "";
				case ItemID.TombCrawlerBanner://3411 Tomb Crawler Banner
					return "";
				case ItemID.DuneSplicerBanner://3412 Dune Splicer Banner
					return "";
				case ItemID.FlyingAntlionBanner://3413 Antlion Swarmer Banner
					return "";
				case ItemID.WalkingAntlionBanner://3414 Antlion Charger Banner
					return "";
				case ItemID.DesertGhoulBanner://3415 Ghoul Banner
					return "";
				case ItemID.DesertLamiaBanner://3416 Lamia Banner
					return "";
				case ItemID.DesertDjinnBanner://3417 Desert Spirit Banner
					return "";
				case ItemID.DesertBasiliskBanner://3418 Basilisk Banner
					return "";
				case ItemID.RavagerScorpionBanner://3419 Sand Poacher Banner
					return "";
				case ItemID.StardustSoldierBanner://3420 Stargazer Banner
					return "";
				case ItemID.StardustWormBanner://3421 Milkyway Weaver Banner
					return "";
				case ItemID.StardustJellyfishBanner://3422 Flow Invader Banner
					return "";
				case ItemID.StardustSpiderBanner://3423 Twinkle Popper Banner
					return "";
				case ItemID.StardustSmallCellBanner://3424 Mini Star Cell Banner
					return "";
				case ItemID.StardustLargeCellBanner://3425 Star Cell Banner
					return "";
				case ItemID.SolarCoriteBanner://3426 Corite Banner
					return "";
				case ItemID.SolarSrollerBanner://3427 Sroller Banner
					return "";
				case ItemID.SolarCrawltipedeBanner://3428 Crawltipede Banner
					return "";
				case ItemID.SolarDrakomireRiderBanner://3429 Drakomire Rider Banner
					return "";
				case ItemID.SolarDrakomireBanner://3430 Drakomire Banner
					return "";
				case ItemID.SolarSolenianBanner://3431 Selenian Banner
					return "";
				case ItemID.NebulaSoldierBanner://3432 Predictor Banner
					return "";
				case ItemID.NebulaHeadcrabBanner://3433 Brain Suckler Banner
					return "";
				case ItemID.NebulaBrainBanner://3434 Nebula Floater Banner
					return "";
				case ItemID.NebulaBeastBanner://3435 Evolution Beast Banner
					return "";
				case ItemID.VortexLarvaBanner://3436 Alien Larva Banner
					return "";
				case ItemID.VortexHornetQueenBanner://3437 Alien Queen Banner
					return "";
				case ItemID.VortexHornetBanner://3438 Alien Hornet Banner
					return "";
				case ItemID.VortexSoldierBanner://3439 Vortexian Banner
					return "";
				case ItemID.VortexRiflemanBanner://3440 Storm Diver Banner
					return "";
				case ItemID.PirateCaptainBanner://3441 Pirate Captain Banner
					return "";
				case ItemID.PirateDeadeyeBanner://3442 Pirate Deadeye Banner
					return "";
				case ItemID.PirateCorsairBanner://3443 Pirate Corsair Banner
					return "";
				case ItemID.PirateCrossbowerBanner://3444 Pirate Crossbower Banner
					return "";
				case ItemID.MartianWalkerBanner://3445 Martian Walker Banner
					return "";
				case ItemID.RedDevilBanner://3446 Red Devil Banner
					return "";
				case ItemID.PinkJellyfishBanner://3447 Pink Jellyfish Banner
					return "";
				case ItemID.GreenJellyfishBanner://3448 Green Jellyfish Banner
					return "";
				case ItemID.DarkMummyBanner://3449 Dark Mummy Banner
					return "";
				case ItemID.LightMummyBanner://3450 Light Mummy Banner
					return "";
				case ItemID.AngryBonesBanner://3451 Angry Bones Banner
					return "";
				case ItemID.IceTortoiseBanner://3452 Ice Tortoise Banner
					return "";
				case ItemID.NebulaPickup1://3453 Damage Booster
					return "";
				case ItemID.NebulaPickup2://3454 Life Booster
					return "";
				case ItemID.NebulaPickup3://3455 Mana Booster
					return "";
				case ItemID.FragmentVortex://3456 Vortex Fragment
					return "";
				case ItemID.FragmentNebula://3457 Nebula Fragment
					return "";
				case ItemID.FragmentSolar://3458 Solar Fragment
					return "";
				case ItemID.FragmentStardust://3459 Stardust Fragment
					return "";
				case ItemID.LunarOre://3460 Luminite
					return "";
				case ItemID.LunarBrick://3461 Luminite Brick
					return "";
				case ItemID.StardustAxe://3462 
					return "";
				case ItemID.StardustChainsaw://3463 
					return "";
				case ItemID.StardustDrill://3464 Stardust Drill
					return "";
				case ItemID.StardustHammer://3465 
					return "";
				case ItemID.StardustPickaxe://3466 Stardust Pickaxe
					return "";
				case ItemID.LunarBar://3467 Luminite Bar
					return "";
				case ItemID.WingsSolar://3468 Solar Wings
					return "";
				case ItemID.WingsVortex://3469 Vortex Booster
					return "";
				case ItemID.WingsNebula://3470 Nebula Mantle
					return "";
				case ItemID.WingsStardust://3471 Stardust Wings
					return "";
				case ItemID.LunarBrickWall://3472 Luminite Brick Wall
					return "";
				case ItemID.SolarEruption://3473 Solar Eruption
					return "";
				case ItemID.StardustCellStaff://3474 Stardust Cell Staff
					return "";
				case ItemID.VortexBeater://3475 Vortex Beater
					return "";
				case ItemID.NebulaArcanum://3476 Nebula Arcanum
					return "";
				case ItemID.BloodWater://3477 Blood Water
					return "";
				case ItemID.TheBrideHat://3478 Wedding Veil
					return "";
				case ItemID.TheBrideDress://3479 Wedding Dress
					return "";
				case ItemID.PlatinumBow://3480 Platinum Bow
					return "";
				case ItemID.PlatinumHammer://3481 Platinum Hammer
					return "";
				case ItemID.PlatinumAxe://3482 Platinum Axe
					return "";
				case ItemID.PlatinumShortsword://3483 Platinum Shortsword
					return "";
				case ItemID.PlatinumBroadsword://3484 Platinum Broadsword
					return "";
				case ItemID.PlatinumPickaxe://3485 Platinum Pickaxe
					return "";
				case ItemID.TungstenBow://3486 Tungsten Bow
					return "";
				case ItemID.TungstenHammer://3487 Tungsten Hammer
					return "";
				case ItemID.TungstenAxe://3488 Tungsten Axe
					return "";
				case ItemID.TungstenShortsword://3489 Tungsten Shortsword
					return "";
				case ItemID.TungstenBroadsword://3490 Tungsten Broadsword
					return "";
				case ItemID.TungstenPickaxe://3491 Tungsten Pickaxe
					return "";
				case ItemID.LeadBow://3492 Lead Bow
					return "";
				case ItemID.LeadHammer://3493 Lead Hammer
					return "";
				case ItemID.LeadAxe://3494 Lead Axe
					return "";
				case ItemID.LeadShortsword://3495 Lead Shortsword
					return "";
				case ItemID.LeadBroadsword://3496 Lead Broadsword
					return "";
				case ItemID.LeadPickaxe://3497 Lead Pickaxe
					return "";
				case ItemID.TinBow://3498 Tin Bow
					return "";
				case ItemID.TinHammer://3499 Tin Hammer
					return "";
				case ItemID.TinAxe://3500 Tin Axe
					return "";
				case ItemID.TinShortsword://3501 Tin Shortsword
					return "";
				case ItemID.TinBroadsword://3502 Tin Broadsword
					return "";
				case ItemID.TinPickaxe://3503 Tin Pickaxe
					return "";
				case ItemID.CopperBow://3504 Copper Bow
					return "";
				case ItemID.CopperHammer://3505 Copper Hammer
					return "";
				case ItemID.CopperAxe://3506 Copper Axe
					return "";
				case ItemID.CopperShortsword://3507 Copper Shortsword
					return "";
				case ItemID.CopperBroadsword://3508 Copper Broadsword
					return "";
				case ItemID.CopperPickaxe://3509 Copper Pickaxe
					return "";
				case ItemID.SilverBow://3510 Silver Bow
					return "";
				case ItemID.SilverHammer://3511 Silver Hammer
					return "";
				case ItemID.SilverAxe://3512 Silver Axe
					return "";
				case ItemID.SilverShortsword://3513 Silver Shortsword
					return "";
				case ItemID.SilverBroadsword://3514 Silver Broadsword
					return "";
				case ItemID.SilverPickaxe://3515 Silver Pickaxe
					return "";
				case ItemID.GoldBow://3516 Gold Bow
					return "";
				case ItemID.GoldHammer://3517 Gold Hammer
					return "";
				case ItemID.GoldAxe://3518 Gold Axe
					return "";
				case ItemID.GoldShortsword://3519 Gold Shortsword
					return "";
				case ItemID.GoldBroadsword://3520 Gold Broadsword
					return "";
				case ItemID.GoldPickaxe://3521 Gold Pickaxe
					return "";
				case ItemID.LunarHamaxeSolar://3522 Solar Flare Hamaxe
					return "";
				case ItemID.LunarHamaxeVortex://3523 Vortex Hamaxe
					return "";
				case ItemID.LunarHamaxeNebula://3524 Nebula Hamaxe
					return "";
				case ItemID.LunarHamaxeStardust://3525 Stardust Hamaxe
					return "";
				case ItemID.SolarDye://3526 Solar Dye
					return "";
				case ItemID.NebulaDye://3527 Nebula Dye
					return "";
				case ItemID.VortexDye://3528 Vortex Dye
					return "";
				case ItemID.StardustDye://3529 Stardust Dye
					return "";
				case ItemID.VoidDye://3530 Void Dye
					return "";
				case ItemID.StardustDragonStaff://3531 Stardust Dragon Staff
					return "";
				case ItemID.Bacon://3532 Bacon
					return "";
				case ItemID.ShiftingSandsDye://3533 Shifting Sands Dye
					return "";
				case ItemID.MirageDye://3534 Mirage Dye
					return "";
				case ItemID.ShiftingPearlSandsDye://3535 Shifting Pearlsands Dye
					return "";
				case ItemID.VortexMonolith://3536 Vortex Monolith
					return "";
				case ItemID.NebulaMonolith://3537 Nebula Monolith
					return "";
				case ItemID.StardustMonolith://3538 Stardust Monolith
					return "";
				case ItemID.SolarMonolith://3539 Solar Monolith
					return "";
				case ItemID.Phantasm://3540 Phantasm
					return "";
				case ItemID.LastPrism://3541 Last Prism
					return "";
				case ItemID.NebulaBlaze://3542 Nebula Blaze
					return "";
				case ItemID.DayBreak://3543 Daybreak
					return "";
				case ItemID.SuperHealingPotion://3544 Super Healing Potion
					return "";
				case ItemID.Detonator://3545 Detonator
					return "";
				case ItemID.FireworksLauncher://3546 Celebration
					return "";
				case ItemID.BouncyDynamite://3547 Bouncy Dynamite
					return "";
				case ItemID.PartyGirlGrenade://3548 Happy Grenade
					return "";
				case ItemID.LunarCraftingStation://3549 Ancient Manipulator
					return "";
				case ItemID.FlameAndSilverDye://3550 Flame and Silver Dye
					return "";
				case ItemID.GreenFlameAndSilverDye://3551 Green Flame and Silver Dye
					return "";
				case ItemID.BlueFlameAndSilverDye://3552 Blue Flame and Silver Dye
					return "";
				case ItemID.ReflectiveCopperDye://3553 Reflective Copper Dye
					return "";
				case ItemID.ReflectiveObsidianDye://3554 Reflective Obsidian Dye
					return "";
				case ItemID.ReflectiveMetalDye://3555 Reflective Metal Dye
					return "";
				case ItemID.MidnightRainbowDye://3556 Midnight Rainbow Dye
					return "";
				case ItemID.BlackAndWhiteDye://3557 Black and White Dye
					return "";
				case ItemID.BrightSilverDye://3558 Bright Silver Dye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f9/Bright_Silver_Dye.png/revision/latest?cb=20200516201401&format=original";
				case ItemID.SilverAndBlackDye://3559 Silver and Black Dye
					return "";
				case ItemID.RedAcidDye://3560 Red Acid Dye
					return "";
				case ItemID.GelDye://3561 Gel Dye
					return "";
				case ItemID.PinkGelDye://3562 Pink Gel Dye
					return "";
				case ItemID.SquirrelRed://3563 Red Squirrel
					return "";
				case ItemID.SquirrelGold://3564 Gold Squirrel
					return "";
				case ItemID.SquirrelOrangeCage://3565 Red Squirrel Cage
					return "";
				case ItemID.SquirrelGoldCage://3566 Gold Squirrel Cage
					return "";
				case ItemID.MoonlordBullet://3567 Luminite Bullet
					return "";
				case ItemID.MoonlordArrow://3568 Luminite Arrow
					return "";
				case ItemID.MoonlordTurretStaff://3569 Lunar Portal Staff
					return "";
				case ItemID.LunarFlareBook://3570 Lunar Flare
					return "";
				case ItemID.RainbowCrystalStaff://3571 Rainbow Crystal Staff
					return "";
				case ItemID.LunarHook://3572 Lunar Hook
					return "";
				case ItemID.LunarBlockSolar://3573 Solar Fragment Block
					return "";
				case ItemID.LunarBlockVortex://3574 Vortex Fragment Block
					return "";
				case ItemID.LunarBlockNebula://3575 Nebula Fragment Block
					return "";
				case ItemID.LunarBlockStardust://3576 Stardust Fragment Block
					return "";
				case ItemID.SuspiciousLookingTentacle://3577 Suspicious Looking Tentacle
					return "";
				case ItemID.Yoraiz0rShirt://3578 Yoraiz0r's Uniform
					return "";
				case ItemID.Yoraiz0rPants://3579 Yoraiz0r's Skirt
					return "";
				case ItemID.Yoraiz0rWings://3580 Yoraiz0r's Spell
					return "";
				case ItemID.Yoraiz0rDarkness://3581 Yoraiz0r's Scowl
					return "";
				case ItemID.JimsWings://3582 Jim's Wings
					return "";
				case ItemID.Yoraiz0rHead://3583 Yoraiz0r's Recolored Goggles
					return "";
				case ItemID.LivingLeafWall://3584 Living Leaf Wall
					return "";
				case ItemID.SkiphsHelm://3585 Skiphs' Mask
					return "";
				case ItemID.SkiphsShirt://3586 Skiphs' Skin
					return "";
				case ItemID.SkiphsPants://3587 Skiphs' Bear Butt
					return "";
				case ItemID.SkiphsWings://3588 Skiphs' Paws
					return "";
				case ItemID.LokisHelm://3589 Loki's Helmet
					return "";
				case ItemID.LokisShirt://3590 Loki's Breastplate
					return "";
				case ItemID.LokisPants://3591 Loki's Greaves
					return "";
				case ItemID.LokisWings://3592 Loki's Wings
					return "";
				case ItemID.SandSlimeBanner://3593 Sand Slime Banner
					return "";
				case ItemID.SeaSnailBanner://3594 Sea Snail Banner
					return "";
				case ItemID.MoonLordTrophy://3595 Moon Lord Trophy
					return "";
				case ItemID.MoonLordPainting://3596 Not a Kid, nor a Squid
					return "";
				case ItemID.BurningHadesDye://3597 Burning Hades Dye
					return "";
				case ItemID.GrimDye://3598 Grim Dye
					return "";
				case ItemID.LokisDye://3599 Loki's Dye
					return "";
				case ItemID.ShadowflameHadesDye://3600 Shadowflame Hades Dye
					return "";
				case ItemID.CelestialSigil://3601 Celestial Sigil
					return "";
				case ItemID.LogicGateLamp_Off://3602 Logic Gate Lamp (Off)
					return "";
				case ItemID.LogicGate_AND://3603 Logic Gate (AND)
					return "";
				case ItemID.LogicGate_OR://3604 Logic Gate (OR)
					return "";
				case ItemID.LogicGate_NAND://3605 Logic Gate (NAND)
					return "";
				case ItemID.LogicGate_NOR://3606 Logic Gate (NOR)
					return "";
				case ItemID.LogicGate_XOR://3607 Logic Gate (XOR)
					return "";
				case ItemID.LogicGate_NXOR://3608 Logic Gate (XNOR)
					return "";
				case ItemID.ConveyorBeltLeft://3609 Conveyor Belt (Clockwise)
					return "";
				case ItemID.ConveyorBeltRight://3610 Conveyor Belt (Counter Clockwise)
					return "";
				case ItemID.WireKite://3611 The Grand Design
					return "";
				case ItemID.YellowWrench://3612 Yellow Wrench
					return "";
				case ItemID.LogicSensor_Sun://3613 Logic Sensor (Day)
					return "";
				case ItemID.LogicSensor_Moon://3614 Logic Sensor (Night)
					return "";
				case ItemID.LogicSensor_Above://3615 Logic Sensor (Player Above)
					return "";
				case ItemID.WirePipe://3616 Junction Box
					return "";
				case ItemID.AnnouncementBox://3617 Announcement Box
					return "";
				case ItemID.LogicGateLamp_On://3618 Logic Gate Lamp (On)
					return "";
				case ItemID.MechanicalLens://3619 Mechanical Lens
					return "";
				case ItemID.ActuationRod://3620 Actuation Rod
					return "";
				case ItemID.TeamBlockRed://3621 Red Team Block
					return "";
				case ItemID.TeamBlockRedPlatform://3622 Red Team Platform
					return "";
				case ItemID.StaticHook://3623 Static Hook
					return "";
				case ItemID.ActuationAccessory://3624 Presserator
					return "";
				case ItemID.MulticolorWrench://3625 Multicolor Wrench
					return "";
				case ItemID.WeightedPressurePlatePink://3626 Pink Weighted Pressure Plate
					return "";
				case ItemID.EngineeringHelmet://3627 Engineering Helmet
					return "";
				case ItemID.CompanionCube://3628 Companion Cube
					return "";
				case ItemID.WireBulb://3629 Wire Bulb
					return "";
				case ItemID.WeightedPressurePlateOrange://3630 Orange Weighted Pressure Plate
					return "";
				case ItemID.WeightedPressurePlatePurple://3631 Purple Weighted Pressure Plate
					return "";
				case ItemID.WeightedPressurePlateCyan://3632 Cyan Weighted Pressure Plate
					return "";
				case ItemID.TeamBlockGreen://3633 Green Team Block
					return "";
				case ItemID.TeamBlockBlue://3634 Blue Team Block
					return "";
				case ItemID.TeamBlockYellow://3635 Yellow Team Block
					return "";
				case ItemID.TeamBlockPink://3636 Pink Team Block
					return "";
				case ItemID.TeamBlockWhite://3637 White Team Block
					return "";
				case ItemID.TeamBlockGreenPlatform://3638 Green Team Platform
					return "";
				case ItemID.TeamBlockBluePlatform://3639 Blue Team Platform
					return "";
				case ItemID.TeamBlockYellowPlatform://3640 Yellow Team Platform
					return "";
				case ItemID.TeamBlockPinkPlatform://3641 Pink Team Platform
					return "";
				case ItemID.TeamBlockWhitePlatform://3642 White Team Platform
					return "";
				case ItemID.LargeAmber://3643 Large Amber
					return "";
				case ItemID.GemLockRuby://3644 Ruby Gem Lock
					return "";
				case ItemID.GemLockSapphire://3645 Sapphire Gem Lock
					return "";
				case ItemID.GemLockEmerald://3646 Emerald Gem Lock
					return "";
				case ItemID.GemLockTopaz://3647 Topaz Gem Lock
					return "";
				case ItemID.GemLockAmethyst://3648 Amethyst Gem Lock
					return "";
				case ItemID.GemLockDiamond://3649 Diamond Gem Lock
					return "";
				case ItemID.GemLockAmber://3650 Amber Gem Lock
					return "";
				case ItemID.SquirrelStatue://3651 Squirrel Statue
					return "";
				case ItemID.ButterflyStatue://3652 Butterfly Statue
					return "";
				case ItemID.WormStatue://3653 Worm Statue
					return "";
				case ItemID.FireflyStatue://3654 Firefly Statue
					return "";
				case ItemID.ScorpionStatue://3655 Scorpion Statue
					return "";
				case ItemID.SnailStatue://3656 Snail Statue
					return "";
				case ItemID.GrasshopperStatue://3657 Grasshopper Statue
					return "";
				case ItemID.MouseStatue://3658 Mouse Statue
					return "";
				case ItemID.DuckStatue://3659 Duck Statue
					return "";
				case ItemID.PenguinStatue://3660 Penguin Statue
					return "";
				case ItemID.FrogStatue://3661 Frog Statue
					return "";
				case ItemID.BuggyStatue://3662 Buggy Statue
					return "";
				case ItemID.LogicGateLamp_Faulty://3663 Logic Gate Lamp (Faulty)
					return "";
				case ItemID.PortalGunStation://3664 Portal Gun Station
					return "";
				case ItemID.Fake_Chest://3665 Trapped Chest
					return "";
				case ItemID.Fake_GoldChest://3666 Trapped Gold Chest
					return "";
				case ItemID.Fake_ShadowChest://3667 Trapped Shadow Chest
					return "";
				case ItemID.Fake_EbonwoodChest://3668 Trapped Ebonwood Chest
					return "";
				case ItemID.Fake_RichMahoganyChest://3669 Trapped Rich Mahogany Chest
					return "";
				case ItemID.Fake_PearlwoodChest://3670 Trapped Pearlwood Chest
					return "";
				case ItemID.Fake_IvyChest://3671 Trapped Ivy Chest
					return "";
				case ItemID.Fake_IceChest://3672 Trapped Frozen Chest
					return "";
				case ItemID.Fake_LivingWoodChest://3673 Trapped Living Wood Chest
					return "";
				case ItemID.Fake_SkywareChest://3674 Trapped Skyware Chest
					return "";
				case ItemID.Fake_ShadewoodChest://3675 Trapped Shadewood Chest
					return "";
				case ItemID.Fake_WebCoveredChest://3676 Trapped Web Covered Chest
					return "";
				case ItemID.Fake_LihzahrdChest://3677 Trapped Lihzahrd Chest
					return "";
				case ItemID.Fake_WaterChest://3678 Trapped Water Chest
					return "";
				case ItemID.Fake_JungleChest://3679 Trapped Jungle Chest
					return "";
				case ItemID.Fake_CorruptionChest://3680 Trapped Corruption Chest
					return "";
				case ItemID.Fake_CrimsonChest://3681 Trapped Crimson Chest
					return "";
				case ItemID.Fake_HallowedChest://3682 Trapped Hallowed Chest
					return "";
				case ItemID.Fake_FrozenChest://3683 Trapped Ice Chest
					return "";
				case ItemID.Fake_DynastyChest://3684 Trapped Dynasty Chest
					return "";
				case ItemID.Fake_HoneyChest://3685 Trapped Honey Chest
					return "";
				case ItemID.Fake_SteampunkChest://3686 Trapped Steampunk Chest
					return "";
				case ItemID.Fake_PalmWoodChest://3687 Trapped Palm Wood Chest
					return "";
				case ItemID.Fake_MushroomChest://3688 Trapped Mushroom Chest
					return "";
				case ItemID.Fake_BorealWoodChest://3689 Trapped Boreal Wood Chest
					return "";
				case ItemID.Fake_SlimeChest://3690 Trapped Slime Chest
					return "";
				case ItemID.Fake_GreenDungeonChest://3691 Trapped Green Dungeon Chest
					return "";
				case ItemID.Fake_PinkDungeonChest://3692 Trapped Pink Dungeon Chest
					return "";
				case ItemID.Fake_BlueDungeonChest://3693 Trapped Blue Dungeon Chest
					return "";
				case ItemID.Fake_BoneChest://3694 Trapped Bone Chest
					return "";
				case ItemID.Fake_CactusChest://3695 Trapped Cactus Chest
					return "";
				case ItemID.Fake_FleshChest://3696 Trapped Flesh Chest
					return "";
				case ItemID.Fake_ObsidianChest://3697 Trapped Obsidian Chest
					return "";
				case ItemID.Fake_PumpkinChest://3698 Trapped Pumpkin Chest
					return "";
				case ItemID.Fake_SpookyChest://3699 Trapped Spooky Chest
					return "";
				case ItemID.Fake_GlassChest://3700 Trapped Glass Chest
					return "";
				case ItemID.Fake_MartianChest://3701 Trapped Martian Chest
					return "";
				case ItemID.Fake_MeteoriteChest://3702 Trapped Meteorite Chest
					return "";
				case ItemID.Fake_GraniteChest://3703 Trapped Granite Chest
					return "";
				case ItemID.Fake_MarbleChest://3704 Trapped Marble Chest
					return "";
				case ItemID.Fake_newchest1://3705 ItemName.Fake_newchest1
					return "";
				case ItemID.Fake_newchest2://3706 ItemName.Fake_newchest2
					return "";
				case ItemID.ProjectilePressurePad://3707 Teal Pressure Pad
					return "";
				case ItemID.WallCreeperStatue://3708 Wall Creeper Statue
					return "";
				case ItemID.UnicornStatue://3709 Unicorn Statue
					return "";
				case ItemID.DripplerStatue://3710 Drippler Statue
					return "";
				case ItemID.WraithStatue://3711 Wraith Statue
					return "";
				case ItemID.BoneSkeletonStatue://3712 Bone Skeleton Statue
					return "";
				case ItemID.UndeadVikingStatue://3713 Undead Viking Statue
					return "";
				case ItemID.MedusaStatue://3714 Medusa Statue
					return "";
				case ItemID.HarpyStatue://3715 Harpy Statue
					return "";
				case ItemID.PigronStatue://3716 Pigron Statue
					return "";
				case ItemID.HopliteStatue://3717 Hoplite Statue
					return "";
				case ItemID.GraniteGolemStatue://3718 Granite Golem Statue
					return "";
				case ItemID.ZombieArmStatue://3719 Armed Zombie Statue
					return "";
				case ItemID.BloodZombieStatue://3720 Blood Zombie Statue
					return "";
				case ItemID.AnglerTackleBag://3721 Angler Tackle Bag
					return "";
				case ItemID.GeyserTrap://3722 Geyser
					return "";
				case ItemID.UltraBrightCampfire://3723 Ultrabright Campfire
					return "";
				case ItemID.BoneCampfire://3724 Bone Campfire
					return "";
				case ItemID.PixelBox://3725 Pixel Box
					return "";
				case ItemID.LogicSensor_Water://3726 Liquid Sensor (Water)
					return "";
				case ItemID.LogicSensor_Lava://3727 Liquid Sensor (Lava)
					return "";
				case ItemID.LogicSensor_Honey://3728 Liquid Sensor (Honey)
					return "";
				case ItemID.LogicSensor_Liquid://3729 Liquid Sensor (Any)
					return "";
				case ItemID.PartyBundleOfBalloonsAccessory://3730 Bundled Party Balloons
					return "";
				case ItemID.PartyBalloonAnimal://3731 Balloon Animal
					return "";
				case ItemID.PartyHat://3732 Party Hat
					return "";
				case ItemID.FlowerBoyHat://3733 Silly Sunflower Petals
					return "";
				case ItemID.FlowerBoyShirt://3734 Silly Sunflower Tops
					return "";
				case ItemID.FlowerBoyPants://3735 Silly Sunflower Bottoms
					return "";
				case ItemID.SillyBalloonPink://3736 Silly Pink Balloon
					return "";
				case ItemID.SillyBalloonPurple://3737 Silly Purple Balloon
					return "";
				case ItemID.SillyBalloonGreen://3738 Silly Green Balloon
					return "";
				case ItemID.SillyStreamerBlue://3739 Blue Streamer
					return "";
				case ItemID.SillyStreamerGreen://3740 Green Streamer
					return "";
				case ItemID.SillyStreamerPink://3741 Pink Streamer
					return "";
				case ItemID.SillyBalloonMachine://3742 Silly Balloon Machine
					return "";
				case ItemID.SillyBalloonTiedPink://3743 Silly Tied Balloon (Pink)
					return "";
				case ItemID.SillyBalloonTiedPurple://3744 Silly Tied Balloon (Purple)
					return "";
				case ItemID.SillyBalloonTiedGreen://3745 Silly Tied Balloon (Green)
					return "";
				case ItemID.Pigronata://3746 Pigronata
					return "";
				case ItemID.PartyMonolith://3747 Party Center
					return "";
				case ItemID.PartyBundleOfBalloonTile://3748 Silly Tied Bundle of Balloons
					return "";
				case ItemID.PartyPresent://3749 Party Present
					return "";
				case ItemID.SliceOfCake://3750 Slice of Cake
					return "";
				case ItemID.CogWall://3751 Cog Wall
					return "";
				case ItemID.SandFallWall://3752 Sandfall Wall
					return "";
				case ItemID.SnowFallWall://3753 Snowfall Wall
					return "";
				case ItemID.SandFallBlock://3754 Sandfall Block
					return "";
				case ItemID.SnowFallBlock://3755 Snowfall Block
					return "";
				case ItemID.SnowCloudBlock://3756 Snow Cloud
					return "";
				case ItemID.PedguinHat://3757 Pedguin's Hood
					return "";
				case ItemID.PedguinShirt://3758 Pedguin's Jacket
					return "";
				case ItemID.PedguinPants://3759 Pedguin's Trousers
					return "";
				case ItemID.SillyBalloonPinkWall://3760 Silly Pink Balloon Wall
					return "";
				case ItemID.SillyBalloonPurpleWall://3761 Silly Purple Balloon Wall
					return "";
				case ItemID.SillyBalloonGreenWall://3762 Silly Green Balloon Wall
					return "";
				case ItemID.AviatorSunglasses://3763 0x33's Aviators
					return "";
				case ItemID.BluePhasesaber://3764 Blue Phasesaber
					return "";
				case ItemID.RedPhasesaber://3765 Red Phasesaber
					return "";
				case ItemID.GreenPhasesaber://3766 Green Phasesaber
					return "";
				case ItemID.PurplePhasesaber://3767 Purple Phasesaber
					return "";
				case ItemID.WhitePhasesaber://3768 White Phasesaber
					return "";
				case ItemID.YellowPhasesaber://3769 Yellow Phasesaber
					return "";
				case ItemID.DjinnsCurse://3770 Djinn's Curse
					return "";
				case ItemID.AncientHorn://3771 Ancient Horn
					return "";
				case ItemID.AntlionClaw://3772 Mandible Blade
					return "";
				case ItemID.AncientArmorHat://3773 Ancient Headdress
					return "";
				case ItemID.AncientArmorShirt://3774 Ancient Garments
					return "";
				case ItemID.AncientArmorPants://3775 Ancient Slacks
					return "";
				case ItemID.AncientBattleArmorHat://3776 Forbidden Mask
					return "";
				case ItemID.AncientBattleArmorShirt://3777 Forbidden Robes
					return "";
				case ItemID.AncientBattleArmorPants://3778 Forbidden Treads
					return "";
				case ItemID.SpiritFlame://3779 Spirit Flame
					return "";
				case ItemID.SandElementalBanner://3780 Sand Elemental Banner
					return "";
				case ItemID.PocketMirror://3781 Pocket Mirror
					return "";
				case ItemID.MagicSandDropper://3782 Magic Sand Dropper
					return "";
				case ItemID.AncientBattleArmorMaterial://3783 Forbidden Fragment
					return "";
				case ItemID.LamiaPants://3784 Lamia Tail
					return "";
				case ItemID.LamiaShirt://3785 Lamia Wraps
					return "";
				case ItemID.LamiaHat://3786 Lamia Mask
					return "";
				case ItemID.SkyFracture://3787 Sky Fracture
					return "";
				case ItemID.OnyxBlaster://3788 Onyx Blaster
					return "";
				case ItemID.SandsharkBanner://3789 Sand Shark Banner
					return "";
				case ItemID.SandsharkCorruptBanner://3790 Bone Biter Banner
					return "";
				case ItemID.SandsharkCrimsonBanner://3791 Flesh Reaver Banner
					return "";
				case ItemID.SandsharkHallowedBanner://3792 Crystal Thresher Banner
					return "";
				case ItemID.TumbleweedBanner://3793 Angry Tumbler Banner
					return "";
				case ItemID.AncientCloth://3794 Ancient Cloth
					return "";
				case ItemID.DjinnLamp://3795 Desert Spirit Lamp
					return "";
				case ItemID.MusicBoxSandstorm://3796 Music Box (Sandstorm)
					return "";
				case ItemID.ApprenticeHat://3797 Apprentice's Hat
					return "";
				case ItemID.ApprenticeRobe://3798 Apprentice's Robe
					return "";
				case ItemID.ApprenticeTrousers://3799 Apprentice's Trousers
					return "";
				case ItemID.SquireGreatHelm://3800 Squire's Great Helm
					return "";
				case ItemID.SquirePlating://3801 Squire's Plating
					return "";
				case ItemID.SquireGreaves://3802 Squire's Greaves
					return "";
				case ItemID.HuntressWig://3803 Huntress's Wig
					return "";
				case ItemID.HuntressJerkin://3804 Huntress's Jerkin
					return "";
				case ItemID.HuntressPants://3805 Huntress's Pants
					return "";
				case ItemID.MonkBrows://3806 Monk's Bushy Brow Bald Cap
					return "";
				case ItemID.MonkShirt://3807 Monk's Shirt
					return "";
				case ItemID.MonkPants://3808 Monk's Pants
					return "";
				case ItemID.ApprenticeScarf://3809 Apprentice's Scarf
					return "";
				case ItemID.SquireShield://3810 Squire's Shield
					return "";
				case ItemID.HuntressBuckler://3811 Huntress's Buckler
					return "";
				case ItemID.MonkBelt://3812 Monk's Belt
					return "";
				case ItemID.DefendersForge://3813 Defender's Forge
					return "";
				case ItemID.WarTable://3814 War Table
					return "";
				case ItemID.WarTableBanner://3815 War Table Banner
					return "";
				case ItemID.DD2ElderCrystalStand://3816 Eternia Crystal Stand
					return "";
				case ItemID.DefenderMedal://3817 Defender Medal
					return "";
				case ItemID.DD2FlameburstTowerT1Popper://3818 Flameburst Rod
					return "";
				case ItemID.DD2FlameburstTowerT2Popper://3819 Flameburst Cane
					return "";
				case ItemID.DD2FlameburstTowerT3Popper://3820 Flameburst Staff
					return "";
				case ItemID.AleThrowingGlove://3821 Ale Tosser
					return "";
				case ItemID.DD2EnergyCrystal://3822 Etherian Mana
					return "";
				case ItemID.DD2SquireDemonSword://3823 Brand of the Inferno
					return "";
				case ItemID.DD2BallistraTowerT1Popper://3824 Ballista Rod
					return "";
				case ItemID.DD2BallistraTowerT2Popper://3825 Ballista Cane
					return "";
				case ItemID.DD2BallistraTowerT3Popper://3826 Ballista Staff
					return "";
				case ItemID.DD2SquireBetsySword://3827 Flying Dragon
					return "";
				case ItemID.DD2ElderCrystal://3828 Eternia Crystal
					return "";
				case ItemID.DD2LightningAuraT1Popper://3829 Lightning Aura Rod
					return "";
				case ItemID.DD2LightningAuraT2Popper://3830 Lightning Aura Cane
					return "";
				case ItemID.DD2LightningAuraT3Popper://3831 Lightning Aura Staff
					return "";
				case ItemID.DD2ExplosiveTrapT1Popper://3832 Explosive Trap Rod
					return "";
				case ItemID.DD2ExplosiveTrapT2Popper://3833 Explosive Trap Cane
					return "";
				case ItemID.DD2ExplosiveTrapT3Popper://3834 Explosive Trap Staff
					return "";
				case ItemID.MonkStaffT1://3835 Sleepy Octopod
					return "";
				case ItemID.MonkStaffT2://3836 Ghastly Glaive
					return "";
				case ItemID.DD2GoblinBomberBanner://3837 Etherian Goblin Bomber Banner
					return "";
				case ItemID.DD2GoblinBanner://3838 Etherian Goblin Banner
					return "";
				case ItemID.DD2SkeletonBanner://3839 Old One's Skeleton Banner
					return "";
				case ItemID.DD2DrakinBanner://3840 Drakin Banner
					return "";
				case ItemID.DD2KoboldFlyerBanner://3841 Kobold Glider Banner
					return "";
				case ItemID.DD2KoboldBanner://3842 Kobold Banner
					return "";
				case ItemID.DD2WitherBeastBanner://3843 Wither Beast Banner
					return "";
				case ItemID.DD2WyvernBanner://3844 Etherian Wyvern Banner
					return "";
				case ItemID.DD2JavelinThrowerBanner://3845 Etherian Javelin Thrower Banner
					return "";
				case ItemID.DD2LightningBugBanner://3846 Etherian Lightning Bug Banner
					return "";
				case ItemID.OgreMask://3847 
					return "";
				case ItemID.GoblinMask://3848 
					return "";
				case ItemID.GoblinBomberCap://3849 
					return "";
				case ItemID.EtherianJavelin://3850 
					return "";
				case ItemID.KoboldDynamiteBackpack://3851 
					return "";
				case ItemID.BookStaff://3852 Tome of Infinite Wisdom
					return "";
				case ItemID.BoringBow://3853 ItemName.BoringBow
					return "";
				case ItemID.DD2PhoenixBow://3854 Phantom Phoenix
					return "";
				case ItemID.DD2PetGato://3855 Gato Egg
					return "";
				case ItemID.DD2PetGhost://3856 Creeper Egg
					return "";
				case ItemID.DD2PetDragon://3857 Dragon Egg
					return "";
				case ItemID.MonkStaffT3://3858 Sky Dragon's Fury
					return "";
				case ItemID.DD2BetsyBow://3859 Aerial Bane
					return "";
				case ItemID.BossBagBetsy://3860 Treasure Bag (Betsy)
					return "";
				case ItemID.BossBagOgre://3861 
					return "";
				case ItemID.BossBagDarkMage://3862 
					return "";
				case ItemID.BossMaskBetsy://3863 Betsy Mask
					return "";
				case ItemID.BossMaskDarkMage://3864 Dark Mage Mask
					return "";
				case ItemID.BossMaskOgre://3865 Ogre Mask
					return "";
				case ItemID.BossTrophyBetsy://3866 Betsy Trophy
					return "";
				case ItemID.BossTrophyDarkmage://3867 Dark Mage Trophy
					return "";
				case ItemID.BossTrophyOgre://3868 Ogre Trophy
					return "";
				case ItemID.MusicBoxDD2://3869 Music Box (Old One's Army)
					return "";
				case ItemID.ApprenticeStaffT3://3870 Betsy's Wrath
					return "";
				case ItemID.SquireAltHead://3871 Valhalla Knight's Helm
					return "";
				case ItemID.SquireAltShirt://3872 Valhalla Knight's Breastplate
					return "";
				case ItemID.SquireAltPants://3873 Valhalla Knight's Greaves
					return "";
				case ItemID.ApprenticeAltHead://3874 Dark Artist's Hat
					return "";
				case ItemID.ApprenticeAltShirt://3875 Dark Artist's Robes
					return "";
				case ItemID.ApprenticeAltPants://3876 Dark Artist's Leggings
					return "";
				case ItemID.HuntressAltHead://3877 Red Riding Hood
					return "";
				case ItemID.HuntressAltShirt://3878 Red Riding Dress
					return "";
				case ItemID.HuntressAltPants://3879 Red Riding Leggings
					return "";
				case ItemID.MonkAltHead://3880 Shinobi Infiltrator's Helmet
					return "";
				case ItemID.MonkAltShirt://3881 Shinobi Infiltrator's Torso
					return "";
				case ItemID.MonkAltPants://3882 Shinobi Infiltrator's Pants
					return "";
				case ItemID.BetsyWings://3883 Betsy's Wings
					return "";
				case ItemID.CrystalChest://3884 Crystal Chest
					return "";
				case ItemID.GoldenChest://3885 Golden Chest
					return "";
				case ItemID.Fake_CrystalChest://3886 Trapped Crystal Chest
					return "";
				case ItemID.Fake_GoldenChest://3887 Trapped Golden Chest
					return "";
				case ItemID.CrystalDoor://3888 Crystal Door
					return "";
				case ItemID.CrystalChair://3889 Crystal Chair
					return "";
				case ItemID.CrystalCandle://3890 Crystal Candle
					return "";
				case ItemID.CrystalLantern://3891 Crystal Lantern
					return "";
				case ItemID.CrystalLamp://3892 Crystal Lamp
					return "";
				case ItemID.CrystalCandelabra://3893 Crystal Candelabra
					return "";
				case ItemID.CrystalChandelier://3894 Crystal Chandelier
					return "";
				case ItemID.CrystalBathtub://3895 Crystal Bathtub
					return "";
				case ItemID.CrystalSink://3896 Crystal Sink
					return "";
				case ItemID.CrystalBed://3897 Crystal Bed
					return "";
				case ItemID.CrystalClock://3898 Crystal Clock
					return "";
				case ItemID.SkywareClock2://3899 Sunplate Clock
					return "";
				case ItemID.DungeonClockBlue://3900 Blue Dungeon Clock
					return "";
				case ItemID.DungeonClockGreen://3901 Green Dungeon Clock
					return "";
				case ItemID.DungeonClockPink://3902 Pink Dungeon Clock
					return "";
				case ItemID.CrystalPlatform://3903 Crystal Platform
					return "";
				case ItemID.GoldenPlatform://3904 Golden Platform
					return "";
				case ItemID.DynastyPlatform://3905 Dynasty Wood Platform
					return "";
				case ItemID.LihzahrdPlatform://3906 Lihzahrd Platform
					return "";
				case ItemID.FleshPlatform://3907 Flesh Platform
					return "";
				case ItemID.FrozenPlatform://3908 Frozen Platform
					return "";
				case ItemID.CrystalWorkbench://3909 Crystal Work Bench
					return "";
				case ItemID.GoldenWorkbench://3910 Golden Work Bench
					return "";
				case ItemID.CrystalDresser://3911 Crystal Dresser
					return "";
				case ItemID.DynastyDresser://3912 Dynasty Dresser
					return "";
				case ItemID.FrozenDresser://3913 Frozen Dresser
					return "";
				case ItemID.LivingWoodDresser://3914 Living Wood Dresser
					return "";
				case ItemID.CrystalPiano://3915 Crystal Piano
					return "";
				case ItemID.DynastyPiano://3916 Dynasty Piano
					return "";
				case ItemID.CrystalBookCase://3917 Crystal Bookcase
					return "";
				case ItemID.CrystalSofaHowDoesThatEvenWork://3918 Crystal Sofa
					return "";
				case ItemID.DynastySofa://3919 Dynasty Sofa
					return "";
				case ItemID.CrystalTable://3920 Crystal Table
					return "";
				case ItemID.ArkhalisHat://3921 Arkhalis' Hood
					return "";
				case ItemID.ArkhalisShirt://3922 Arkhalis' Bodice
					return "";
				case ItemID.ArkhalisPants://3923 Arkhalis' Tights
					return "";
				case ItemID.ArkhalisWings://3924 Arkhalis' Lightwings
					return "";
				case ItemID.LeinforsHat://3925 Leinfors' Hair Protector
					return "";
				case ItemID.LeinforsShirt://3926 Leinfors' Excessive Style
					return "";
				case ItemID.LeinforsPants://3927 Leinfors' Fancypants
					return "";
				case ItemID.LeinforsWings://3928 Leinfors' Prehensile Cloak
					return "";
				case ItemID.LeinforsAccessory://3929 Leinfors' Luxury Shampoo
					return "";
				case ItemID.Celeb2://3930 Celebration Mk2
					return "";
				case ItemID.SpiderBathtub://3931 Spider Bathtub
					return "";
				case ItemID.SpiderBed://3932 Spider Bed
					return "";
				case ItemID.SpiderBookcase://3933 Spider Bookcase
					return "";
				case ItemID.SpiderDresser://3934 Spider Dresser
					return "";
				case ItemID.SpiderCandelabra://3935 Spider Candelabra
					return "";
				case ItemID.SpiderCandle://3936 Spider Candle
					return "";
				case ItemID.SpiderChair://3937 Spider Chair
					return "";
				case ItemID.SpiderChandelier://3938 Spider Chandelier
					return "";
				case ItemID.SpiderChest://3939 Spider Chest
					return "";
				case ItemID.SpiderClock://3940 Spider Clock
					return "";
				case ItemID.SpiderDoor://3941 Spider Door
					return "";
				case ItemID.SpiderLamp://3942 Spider Lamp
					return "";
				case ItemID.SpiderLantern://3943 Spider Lantern
					return "";
				case ItemID.SpiderPiano://3944 Spider Piano
					return "";
				case ItemID.SpiderPlatform://3945 Spider Platform
					return "";
				case ItemID.SpiderSinkSpiderSinkDoesWhateverASpiderSinkDoes://3946 Spider Sink
					return "";
				case ItemID.SpiderSofa://3947 Spider Sofa
					return "";
				case ItemID.SpiderTable://3948 Spider Table
					return "";
				case ItemID.SpiderWorkbench://3949 Spider Work Bench
					return "";
				case ItemID.Fake_SpiderChest://3950 Trapped Spider Chest
					return "";
				case ItemID.IronBrick://3951 Iron Brick
					return "";
				case ItemID.IronBrickWall://3952 Iron Brick Wall
					return "";
				case ItemID.LeadBrick://3953 Lead Brick
					return "";
				case ItemID.LeadBrickWall://3954 Lead Brick Wall
					return "";
				case ItemID.LesionBlock://3955 Lesion Block
					return "";
				case ItemID.LesionBlockWall://3956 Lesion Block Wall
					return "";
				case ItemID.LesionPlatform://3957 Lesion Platform
					return "";
				case ItemID.LesionBathtub://3958 Lesion Bathtub
					return "";
				case ItemID.LesionBed://3959 Lesion Bed
					return "";
				case ItemID.LesionBookcase://3960 Lesion Bookcase
					return "";
				case ItemID.LesionCandelabra://3961 Lesion Candelabra
					return "";
				case ItemID.LesionCandle://3962 Lesion Candle
					return "";
				case ItemID.LesionChair://3963 Lesion Chair
					return "";
				case ItemID.LesionChandelier://3964 Lesion Chandelier
					return "";
				case ItemID.LesionChest://3965 Lesion Chest
					return "";
				case ItemID.LesionClock://3966 Lesion Clock
					return "";
				case ItemID.LesionDoor://3967 Lesion Door
					return "";
				case ItemID.LesionDresser://3968 Lesion Dresser
					return "";
				case ItemID.LesionLamp://3969 Lesion Lamp
					return "";
				case ItemID.LesionLantern://3970 Lesion Lantern
					return "";
				case ItemID.LesionPiano://3971 Lesion Piano
					return "";
				case ItemID.LesionSink://3972 Lesion Sink
					return "";
				case ItemID.LesionSofa://3973 Lesion Sofa
					return "";
				case ItemID.LesionTable://3974 Lesion Table
					return "";
				case ItemID.LesionWorkbench://3975 Lesion Work Bench
					return "";
				case ItemID.Fake_LesionChest://3976 Trapped Lesion Chest
					return "";
				case ItemID.HatRack://3977 Hat Rack
					return "";
				case ItemID.ColorOnlyDye://3978 
					return "";
				case ItemID.WoodenCrateHard://3979 Pearlwood Crate
					return "";
				case ItemID.IronCrateHard://3980 Mythril Crate
					return "";
				case ItemID.GoldenCrateHard://3981 Titanium Crate
					return "";
				case ItemID.CorruptFishingCrateHard://3982 Defiled Crate
					return "";
				case ItemID.CrimsonFishingCrateHard://3983 Hematic Crate
					return "";
				case ItemID.DungeonFishingCrateHard://3984 Stockade Crate
					return "";
				case ItemID.FloatingIslandFishingCrateHard://3985 Azure Crate
					return "";
				case ItemID.HallowedFishingCrateHard://3986 Divine Crate
					return "";
				case ItemID.JungleFishingCrateHard://3987 Bramble Crate
					return "";
				case ItemID.DeadMansChest://3988 Dead Man's Chest
					return "";
				case ItemID.GolfBall://3989 Golf Ball
					return "";
				case ItemID.AmphibianBoots://3990 Amphibian Boots
					return "";
				case ItemID.ArcaneFlower://3991 Arcane Flower
					return "";
				case ItemID.BerserkerGlove://3992 Berserker's Glove
					return "";
				case ItemID.FairyBoots://3993 Fairy Boots
					return "";
				case ItemID.FrogFlipper://3994 Frog Flipper
					return "";
				case ItemID.FrogGear://3995 Frog Gear
					return "";
				case ItemID.FrogWebbing://3996 Frog Webbing
					return "";
				case ItemID.FrozenShield://3997 Frozen Shield
					return "";
				case ItemID.HeroShield://3998 Hero Shield
					return "";
				case ItemID.LavaSkull://3999 Magma Skull
					return "";
				case ItemID.MagnetFlower://4000 Magnet Flower
					return "";
				case ItemID.ManaCloak://4001 Mana Cloak
					return "";
				case ItemID.MoltenQuiver://4002 Molten Quiver
					return "";
				case ItemID.MoltenSkullRose://4003 Molten Skull Rose
					return "";
				case ItemID.ObsidianSkullRose://4004 Obsidian Skull Rose
					return "";
				case ItemID.ReconScope://4005 Recon Scope
					return "";
				case ItemID.StalkersQuiver://4006 Stalker's Quiver
					return "";
				case ItemID.StingerNecklace://4007 Stinger Necklace
					return "";
				case ItemID.UltrabrightHelmet://4008 Ultrabright Helmet
					return "";
				case ItemID.Apple://4009 Apple
					return "";
				case ItemID.ApplePieSlice://4010 
					return "";
				case ItemID.ApplePie://4011 Apple Pie
					return "";
				case ItemID.BananaSplit://4012 Banana Split
					return "";
				case ItemID.BBQRibs://4013 BBQ Ribs
					return "";
				case ItemID.BunnyStew://4014 Bunny Stew
					return "";
				case ItemID.Burger://4015 Burger
					return "";
				case ItemID.ChickenNugget://4016 Chicken Nugget
					return "";
				case ItemID.ChocolateChipCookie://4017 Chocolate Chip Cookie
					return "";
				case ItemID.CreamSoda://4018 Cream Soda
					return "";
				case ItemID.Escargot://4019 Escargot
					return "";
				case ItemID.FriedEgg://4020 Fried Egg
					return "";
				case ItemID.Fries://4021 Fries
					return "";
				case ItemID.GoldenDelight://4022 Golden Delight
					return "";
				case ItemID.Grapes://4023 Grapes
					return "";
				case ItemID.GrilledSquirrel://4024 Grilled Squirrel
					return "";
				case ItemID.Hotdog://4025 Hotdog
					return "";
				case ItemID.IceCream://4026 Ice Cream
					return "";
				case ItemID.Milkshake://4027 Milkshake
					return "";
				case ItemID.Nachos://4028 Nachos
					return "";
				case ItemID.Pizza://4029 Pizza
					return "";
				case ItemID.PotatoChips://4030 Potato Chips
					return "";
				case ItemID.RoastedBird://4031 Roasted Bird
					return "";
				case ItemID.RoastedDuck://4032 Roasted Duck
					return "";
				case ItemID.SauteedFrogLegs://4033 Sauteed Frog Legs
					return "";
				case ItemID.SeafoodDinner://4034 Seafood Dinner
					return "";
				case ItemID.ShrimpPoBoy://4035 Shrimp Po' Boy
					return "";
				case ItemID.Spaghetti://4036 Spaghetti
					return "";
				case ItemID.Steak://4037 Steak
					return "";
				case ItemID.MoltenCharm://4038 Molten Charm
					return "";
				case ItemID.GolfClubIron://4039 Golf Club (Iron)
					return "";
				case ItemID.GolfCup://4040 Golf Cup
					return "";
				case ItemID.FlowerPacketBlue://4041 Blue Flower Seeds
					return "";
				case ItemID.FlowerPacketMagenta://4042 Magenta Flower Seeds
					return "";
				case ItemID.FlowerPacketPink://4043 Pink Flower Seeds
					return "";
				case ItemID.FlowerPacketRed://4044 Red Flower Seeds
					return "";
				case ItemID.FlowerPacketYellow://4045 Yellow Flower Seeds
					return "";
				case ItemID.FlowerPacketViolet://4046 Violet Flower Seeds
					return "";
				case ItemID.FlowerPacketWhite://4047 White Flower Seeds
					return "";
				case ItemID.FlowerPacketTallGrass://4048 Tall Grass Seeds
					return "";
				case ItemID.LawnMower://4049 Lawn Mower
					return "";
				case ItemID.CrimstoneBrick://4050 Crimstone Brick
					return "";
				case ItemID.SmoothSandstone://4051 Smooth Sandstone
					return "";
				case ItemID.CrimstoneBrickWall://4052 Crimstone Brick Wall
					return "";
				case ItemID.SmoothSandstoneWall://4053 Smooth Sandstone Wall
					return "";
				case ItemID.BloodMoonMonolith://4054 Blood Moon Monolith
					return "";
				case ItemID.SandBoots://4055 Dunerider Boots
					return "";
				case ItemID.AncientChisel://4056 Ancient Chisel
					return "";
				case ItemID.CarbonGuitar://4057 Rain Song
					return "";
				case ItemID.SkeletonBow://4058 
					return "";
				case ItemID.FossilPickaxe://4059 Fossil Pickaxe
					return "";
				case ItemID.SuperStarCannon://4060 Super Star Shooter
					return "";
				case ItemID.ThunderSpear://4061 Storm Spear
					return "";
				case ItemID.ThunderStaff://4062 Thunder Zapper
					return "";
				case ItemID.DrumSet://4063 Drum Set
					return "";
				case ItemID.PicnicTable://4064 Picnic Table
					return "";
				case ItemID.PicnicTableWithCloth://4065 Fancy Picnic Table
					return "";
				case ItemID.DesertMinecart://4066 Desert Minecart
					return "";
				case ItemID.FishMinecart://4067 Minecarp
					return "";
				case ItemID.FairyCritterPink://4068 Pink Fairy
					return "";
				case ItemID.FairyCritterGreen://4069 Green Fairy
					return "";
				case ItemID.FairyCritterBlue://4070 Blue Fairy
					return "";
				case ItemID.JunoniaShell://4071 Junonia Shell
					return "";
				case ItemID.LightningWhelkShell://4072 Lightning Whelk Shell
					return "";
				case ItemID.TulipShell://4073 Tulip Shell
					return "";
				case ItemID.PinWheel://4074 Pin Wheel
					return "";
				case ItemID.WeatherVane://4075 Weather Vane
					return "";
				case ItemID.VoidVault://4076 Void Vault
					return "";
				case ItemID.MusicBoxOceanAlt://4077 Music Box (Ocean Night)
					return "";
				case ItemID.MusicBoxSlimeRain://4078 Music Box (Slime Rain)
					return "";
				case ItemID.MusicBoxSpaceAlt://4079 Music Box (Space Day)
					return "";
				case ItemID.MusicBoxTownDay://4080 Music Box (Town Day)
					return "";
				case ItemID.MusicBoxTownNight://4081 Music Box (Town Night)
					return "";
				case ItemID.MusicBoxWindyDay://4082 Music Box (Windy Day)
					return "";
				case ItemID.GolfCupFlagWhite://4083 White Pin Flag
					return "";
				case ItemID.GolfCupFlagRed://4084 Red Pin Flag
					return "";
				case ItemID.GolfCupFlagGreen://4085 Green Pin Flag
					return "";
				case ItemID.GolfCupFlagBlue://4086 Blue Pin Flag
					return "";
				case ItemID.GolfCupFlagYellow://4087 Yellow Pin Flag
					return "";
				case ItemID.GolfCupFlagPurple://4088 Purple Pin Flag
					return "";
				case ItemID.GolfTee://4089 Golf Tee
					return "";
				case ItemID.ShellPileBlock://4090 Shell Pile
					return "";
				case ItemID.AntiPortalBlock://4091 Anti-Portal Block
					return "";
				case ItemID.GolfClubPutter://4092 Golf Club (Putter)
					return "";
				case ItemID.GolfClubWedge://4093 Golf Club (Wedge)
					return "";
				case ItemID.GolfClubDriver://4094 Golf Club (Driver)
					return "";
				case ItemID.GolfWhistle://4095 Golf Whistle
					return "";
				case ItemID.ToiletEbonyWood://4096 Ebonwood Toilet
					return "";
				case ItemID.ToiletRichMahogany://4097 Rich Mahogany Toilet
					return "";
				case ItemID.ToiletPearlwood://4098 Pearlwood Toilet
					return "";
				case ItemID.ToiletLivingWood://4099 Living Wood Toilet
					return "";
				case ItemID.ToiletCactus://4100 Cactus Toilet
					return "";
				case ItemID.ToiletBone://4101 Bone Toilet
					return "";
				case ItemID.ToiletFlesh://4102 Flesh Toilet
					return "";
				case ItemID.ToiletMushroom://4103 Mushroom Toilet
					return "";
				case ItemID.ToiletSunplate://4104 Skyware Toilet
					return "";
				case ItemID.ToiletShadewood://4105 Shadewood Toilet
					return "";
				case ItemID.ToiletLihzhard://4106 Lihzahrd Toilet
					return "";
				case ItemID.ToiletDungeonBlue://4107 Blue Dungeon Toilet
					return "";
				case ItemID.ToiletDungeonGreen://4108 Green Dungeon Toilet
					return "";
				case ItemID.ToiletDungeonPink://4109 Pink Dungeon Toilet
					return "";
				case ItemID.ToiletObsidian://4110 Obsidian Toilet
					return "";
				case ItemID.ToiletFrozen://4111 Frozen Toilet
					return "";
				case ItemID.ToiletGlass://4112 Glass Toilet
					return "";
				case ItemID.ToiletHoney://4113 Honey Toilet
					return "";
				case ItemID.ToiletSteampunk://4114 Steampunk Toilet
					return "";
				case ItemID.ToiletPumpkin://4115 Pumpkin Toilet
					return "";
				case ItemID.ToiletSpooky://4116 Spooky Toilet
					return "";
				case ItemID.ToiletDynasty://4117 Dynasty Toilet
					return "";
				case ItemID.ToiletPalm://4118 Palm Wood Toilet
					return "";
				case ItemID.ToiletBoreal://4119 Boreal Wood Toilet
					return "";
				case ItemID.ToiletSlime://4120 Slime Toilet
					return "";
				case ItemID.ToiletMartian://4121 Martian Toilet
					return "";
				case ItemID.ToiletGranite://4122 Granite Toilet
					return "";
				case ItemID.ToiletMarble://4123 Marble Toilet
					return "";
				case ItemID.ToiletCrystal://4124 Crystal Toilet
					return "";
				case ItemID.ToiletSpider://4125 Spider Toilet
					return "";
				case ItemID.ToiletLesion://4126 Lesion Toilet
					return "";
				case ItemID.ToiletDiamond://4127 Diamond Toilet
					return "";
				case ItemID.MaidHead://4128 Maid Bonnet
					return "";
				case ItemID.MaidShirt://4129 Maid Dress
					return "";
				case ItemID.MaidPants://4130 Maid Shoes
					return "";
				case ItemID.VoidLens://4131 Void Bag
					return "";
				case ItemID.MaidHead2://4132 Pink Maid Bonnet
					return "";
				case ItemID.MaidShirt2://4133 Pink Maid Dress
					return "";
				case ItemID.MaidPants2://4134 Pink Maid Shoes
					return "";
				case ItemID.GolfHat://4135 Country Club Cap
					return "";
				case ItemID.GolfShirt://4136 Country Club Vest
					return "";
				case ItemID.GolfPants://4137 Country Club Trousers
					return "";
				case ItemID.GolfVisor://4138 Country Club Visor
					return "";
				case ItemID.SpiderBlock://4139 Spider Nest Block
					return "";
				case ItemID.SpiderWall://4140 Spider Nest Wall
					return "";
				case ItemID.ToiletMeteor://4141 Meteor Toilet
					return "";
				case ItemID.LesionStation://4142 Decay Chamber
					return "";
				case ItemID.ManaCloakStar://4143 
					return "";
				case ItemID.Terragrim://4144 Terragrim
					return "";
				case ItemID.SolarBathtub://4145 Solar Bathtub
					return "";
				case ItemID.SolarBed://4146 Solar Bed
					return "";
				case ItemID.SolarBookcase://4147 Solar Bookcase
					return "";
				case ItemID.SolarDresser://4148 Solar Dresser
					return "";
				case ItemID.SolarCandelabra://4149 Solar Candelabra
					return "";
				case ItemID.SolarCandle://4150 Solar Candle
					return "";
				case ItemID.SolarChair://4151 Solar Chair
					return "";
				case ItemID.SolarChandelier://4152 Solar Chandelier
					return "";
				case ItemID.SolarChest://4153 Solar Chest
					return "";
				case ItemID.SolarClock://4154 Solar Clock
					return "";
				case ItemID.SolarDoor://4155 Solar Door
					return "";
				case ItemID.SolarLamp://4156 Solar Lamp
					return "";
				case ItemID.SolarLantern://4157 Solar Lantern
					return "";
				case ItemID.SolarPiano://4158 Solar Piano
					return "";
				case ItemID.SolarPlatform://4159 Solar Platform
					return "";
				case ItemID.SolarSink://4160 Solar Sink
					return "";
				case ItemID.SolarSofa://4161 Solar Sofa
					return "";
				case ItemID.SolarTable://4162 Solar Table
					return "";
				case ItemID.SolarWorkbench://4163 Solar Work Bench
					return "";
				case ItemID.Fake_SolarChest://4164 Trapped Solar Chest
					return "";
				case ItemID.SolarToilet://4165 Solar Toilet
					return "";
				case ItemID.VortexBathtub://4166 Vortex Bathtub
					return "";
				case ItemID.VortexBed://4167 Vortex Bed
					return "";
				case ItemID.VortexBookcase://4168 Vortex Bookcase
					return "";
				case ItemID.VortexDresser://4169 Vortex Dresser
					return "";
				case ItemID.VortexCandelabra://4170 Vortex Candelabra
					return "";
				case ItemID.VortexCandle://4171 Vortex Candle
					return "";
				case ItemID.VortexChair://4172 Vortex Chair
					return "";
				case ItemID.VortexChandelier://4173 Vortex Chandelier
					return "";
				case ItemID.VortexChest://4174 Vortex Chest
					return "";
				case ItemID.VortexClock://4175 Vortex Clock
					return "";
				case ItemID.VortexDoor://4176 Vortex Door
					return "";
				case ItemID.VortexLamp://4177 Vortex Lamp
					return "";
				case ItemID.VortexLantern://4178 Vortex Lantern
					return "";
				case ItemID.VortexPiano://4179 Vortex Piano
					return "";
				case ItemID.VortexPlatform://4180 Vortex Platform
					return "";
				case ItemID.VortexSink://4181 Vortex Sink
					return "";
				case ItemID.VortexSofa://4182 Vortex Sofa
					return "";
				case ItemID.VortexTable://4183 Vortex Table
					return "";
				case ItemID.VortexWorkbench://4184 Vortex Work Bench
					return "";
				case ItemID.Fake_VortexChest://4185 Trapped Vortex Chest
					return "";
				case ItemID.VortexToilet://4186 Vortex Toilet
					return "";
				case ItemID.NebulaBathtub://4187 Nebula Bathtub
					return "";
				case ItemID.NebulaBed://4188 Nebula Bed
					return "";
				case ItemID.NebulaBookcase://4189 Nebula Bookcase
					return "";
				case ItemID.NebulaDresser://4190 Nebula Dresser
					return "";
				case ItemID.NebulaCandelabra://4191 Nebula Candelabra
					return "";
				case ItemID.NebulaCandle://4192 Nebula Candle
					return "";
				case ItemID.NebulaChair://4193 Nebula Chair
					return "";
				case ItemID.NebulaChandelier://4194 Nebula Chandelier
					return "";
				case ItemID.NebulaChest://4195 Nebula Chest
					return "";
				case ItemID.NebulaClock://4196 Nebula Clock
					return "";
				case ItemID.NebulaDoor://4197 Nebula Door
					return "";
				case ItemID.NebulaLamp://4198 Nebula Lamp
					return "";
				case ItemID.NebulaLantern://4199 Nebula Lantern
					return "";
				case ItemID.NebulaPiano://4200 Nebula Piano
					return "";
				case ItemID.NebulaPlatform://4201 Nebula Platform
					return "";
				case ItemID.NebulaSink://4202 Nebula Sink
					return "";
				case ItemID.NebulaSofa://4203 Nebula Sofa
					return "";
				case ItemID.NebulaTable://4204 Nebula Table
					return "";
				case ItemID.NebulaWorkbench://4205 Nebula Work Bench
					return "";
				case ItemID.Fake_NebulaChest://4206 Trapped Nebula Chest
					return "";
				case ItemID.NebulaToilet://4207 Nebula Toilet
					return "";
				case ItemID.StardustBathtub://4208 Stardust Bathtub
					return "";
				case ItemID.StardustBed://4209 Stardust Bed
					return "";
				case ItemID.StardustBookcase://4210 Stardust Bookcase
					return "";
				case ItemID.StardustDresser://4211 Stardust Dresser
					return "";
				case ItemID.StardustCandelabra://4212 Stardust Candelabra
					return "";
				case ItemID.StardustCandle://4213 Stardust Candle
					return "";
				case ItemID.StardustChair://4214 Stardust Chair
					return "";
				case ItemID.StardustChandelier://4215 Stardust Chandelier
					return "";
				case ItemID.StardustChest://4216 Stardust Chest
					return "";
				case ItemID.StardustClock://4217 Stardust Clock
					return "";
				case ItemID.StardustDoor://4218 Stardust Door
					return "";
				case ItemID.StardustLamp://4219 Stardust Lamp
					return "";
				case ItemID.StardustLantern://4220 Stardust Lantern
					return "";
				case ItemID.StardustPiano://4221 Stardust Piano
					return "";
				case ItemID.StardustPlatform://4222 Stardust Platform
					return "";
				case ItemID.StardustSink://4223 Stardust Sink
					return "";
				case ItemID.StardustSofa://4224 Stardust Sofa
					return "";
				case ItemID.StardustTable://4225 Stardust Table
					return "";
				case ItemID.StardustWorkbench://4226 Stardust Work Bench
					return "";
				case ItemID.Fake_StardustChest://4227 Trapped Stardust Chest
					return "";
				case ItemID.StardustToilet://4228 Stardust Toilet
					return "";
				case ItemID.SolarBrick://4229 Solar Brick
					return "";
				case ItemID.VortexBrick://4230 Vortex Brick
					return "";
				case ItemID.NebulaBrick://4231 Nebula Brick
					return "";
				case ItemID.StardustBrick://4232 Stardust Brick
					return "";
				case ItemID.SolarBrickWall://4233 Solar Brick Wall
					return "";
				case ItemID.VortexBrickWall://4234 Vortex Brick Wall
					return "";
				case ItemID.NebulaBrickWall://4235 Nebula Brick Wall
					return "";
				case ItemID.StardustBrickWall://4236 Stardust Brick Wall
					return "";
				case ItemID.MusicBoxDayRemix://4237 Music Box (Day Remix)
					return "";
				case ItemID.CrackedBlueBrick://4238 Cracked Blue Brick
					return "";
				case ItemID.CrackedGreenBrick://4239 Cracked Green Brick
					return "";
				case ItemID.CrackedPinkBrick://4240 Cracked Pink Brick
					return "";
				case ItemID.FlowerPacketWild://4241 Wild Flower Seeds
					return "";
				case ItemID.GolfBallDyedBlack://4242 Black Golf Ball
					return "";
				case ItemID.GolfBallDyedBlue://4243 Blue Golf Ball
					return "";
				case ItemID.GolfBallDyedBrown://4244 Brown Golf Ball
					return "";
				case ItemID.GolfBallDyedCyan://4245 Cyan Golf Ball
					return "";
				case ItemID.GolfBallDyedGreen://4246 Green Golf Ball
					return "";
				case ItemID.GolfBallDyedLimeGreen://4247 Lime Golf Ball
					return "";
				case ItemID.GolfBallDyedOrange://4248 Orange Golf Ball
					return "";
				case ItemID.GolfBallDyedPink://4249 Pink Golf Ball
					return "";
				case ItemID.GolfBallDyedPurple://4250 Purple Golf Ball
					return "";
				case ItemID.GolfBallDyedRed://4251 Red Golf Ball
					return "";
				case ItemID.GolfBallDyedSkyBlue://4252 Sky Blue Golf Ball
					return "";
				case ItemID.GolfBallDyedTeal://4253 Teal Golf Ball
					return "";
				case ItemID.GolfBallDyedViolet://4254 Violet Golf Ball
					return "";
				case ItemID.GolfBallDyedYellow://4255 Yellow Golf Ball
					return "";
				case ItemID.AmberRobe://4256 Amber Robe
					return "";
				case ItemID.AmberHook://4257 Amber Hook
					return "";
				case ItemID.OrangePhaseblade://4258 Orange Phaseblade
					return "";
				case ItemID.OrangePhasesaber://4259 Orange Phasesaber
					return "";
				case ItemID.OrangeStainedGlass://4260 Orange Stained Glass
					return "";
				case ItemID.OrangePressurePlate://4261 Orange Pressure Plate
					return "";
				case ItemID.MysticCoilSnake://4262 Snake Charmer's Flute
					return "";
				case ItemID.MagicConch://4263 Magic Conch
					return "";
				case ItemID.GolfCart://4264 Golf Cart Keys
					return "";
				case ItemID.GolfChest://4265 Golf Chest
					return "";
				case ItemID.Fake_GolfChest://4266 Trapped Golf Chest
					return "";
				case ItemID.DesertChest://4267 Sandstone Chest
					return "";
				case ItemID.Fake_DesertChest://4268 Trapped Sandstone Chest
					return "";
				case ItemID.SanguineStaff://4269 Sanguine Staff
					return "";
				case ItemID.SharpTears://4270 Blood Thorn
					return "";
				case ItemID.BloodMoonStarter://4271 Bloody Tear
					return "";
				case ItemID.DripplerFlail://4272 Drippler Crippler
					return "";
				case ItemID.VampireFrogStaff://4273 Vampire Frog Staff
					return "";
				case ItemID.GoldGoldfish://4274 Gold Goldfish
					return "";
				case ItemID.GoldGoldfishBowl://4275 Gold Fish Bowl
					return "";
				case ItemID.CatBast://4276 Bast Statue
					return "";
				case ItemID.GoldStarryGlassBlock://4277 Gold Starry Block
					return "";
				case ItemID.BlueStarryGlassBlock://4278 Blue Starry Block
					return "";
				case ItemID.GoldStarryGlassWall://4279 Gold Starry Wall
					return "";
				case ItemID.BlueStarryGlassWall://4280 Blue Starry Wall
					return "";
				case ItemID.BabyBirdStaff://4281 Finch Staff
					return "";
				case ItemID.Apricot://4282 Apricot
					return "";
				case ItemID.Banana://4283 Banana
					return "";
				case ItemID.BlackCurrant://4284 Blackcurrant
					return "";
				case ItemID.BloodOrange://4285 Blood Orange
					return "";
				case ItemID.Cherry://4286 Cherry
					return "";
				case ItemID.Coconut://4287 Coconut
					return "";
				case ItemID.Dragonfruit://4288 Dragon Fruit
					return "";
				case ItemID.Elderberry://4289 Elderberry
					return "";
				case ItemID.Grapefruit://4290 Grapefruit
					return "";
				case ItemID.Lemon://4291 Lemon
					return "";
				case ItemID.Mango://4292 Mango
					return "";
				case ItemID.Peach://4293 Peach
					return "";
				case ItemID.Pineapple://4294 Pineapple
					return "";
				case ItemID.Plum://4295 Plum
					return "";
				case ItemID.Rambutan://4296 Rambutan
					return "";
				case ItemID.Starfruit://4297 Star Fruit
					return "";
				case ItemID.SandstoneBathtub://4298 Sandstone Bathtub
					return "";
				case ItemID.SandstoneBed://4299 Sandstone Bed
					return "";
				case ItemID.SandstoneBookcase://4300 Sandstone Bookcase
					return "";
				case ItemID.SandstoneDresser://4301 Sandstone Dresser
					return "";
				case ItemID.SandstoneCandelabra://4302 Sandstone Candelabra
					return "";
				case ItemID.SandstoneCandle://4303 Sandstone Candle
					return "";
				case ItemID.SandstoneChair://4304 Sandstone Chair
					return "";
				case ItemID.SandstoneChandelier://4305 Sandstone Chandelier
					return "";
				case ItemID.SandstoneClock://4306 Sandstone Clock
					return "";
				case ItemID.SandstoneDoor://4307 Sandstone Door
					return "";
				case ItemID.SandstoneLamp://4308 Sandstone Lamp
					return "";
				case ItemID.SandstoneLantern://4309 Sandstone Lantern
					return "";
				case ItemID.SandstonePiano://4310 Sandstone Piano
					return "";
				case ItemID.SandstonePlatform://4311 Sandstone Platform
					return "";
				case ItemID.SandstoneSink://4312 Sandstone Sink
					return "";
				case ItemID.SandstoneSofa://4313 Sandstone Sofa
					return "";
				case ItemID.SandstoneTable://4314 Sandstone Table
					return "";
				case ItemID.SandstoneWorkbench://4315 Sandstone Work Bench
					return "";
				case ItemID.SandstoneToilet://4316 Sandstone Toilet
					return "";
				case ItemID.BloodHamaxe://4317 Haemorrhaxe
					return "";
				case ItemID.VoidMonolith://4318 Void Monolith
					return "";
				case ItemID.ArrowSign://4319 Arrow Sign
					return "";
				case ItemID.PaintedArrowSign://4320 Painted Arrow Sign
					return "";
				case ItemID.GameMasterShirt://4321 Master Gamer's Jacket
					return "";
				case ItemID.GameMasterPants://4322 Master Gamer's Pants
					return "";
				case ItemID.StarPrincessCrown://4323 Star Princess Crown
					return "";
				case ItemID.StarPrincessDress://4324 Star Princess Dress
					return "";
				case ItemID.BloodFishingRod://4325 Chum Caster
					return "";
				case ItemID.FoodPlatter://4326 Plate
					return "";
				case ItemID.BlackDragonflyJar://4327 Black Dragonfly Jar
					return "";
				case ItemID.BlueDragonflyJar://4328 Blue Dragonfly Jar
					return "";
				case ItemID.GreenDragonflyJar://4329 Green Dragonfly Jar
					return "";
				case ItemID.OrangeDragonflyJar://4330 Orange Dragonfly Jar
					return "";
				case ItemID.RedDragonflyJar://4331 Red Dragonfly Jar
					return "";
				case ItemID.YellowDragonflyJar://4332 Yellow Dragonfly Jar
					return "";
				case ItemID.GoldDragonflyJar://4333 Gold Dragonfly Jar
					return "";
				case ItemID.BlackDragonfly://4334 Black Dragonfly
					return "";
				case ItemID.BlueDragonfly://4335 Blue Dragonfly
					return "";
				case ItemID.GreenDragonfly://4336 Green Dragonfly
					return "";
				case ItemID.OrangeDragonfly://4337 Orange Dragonfly
					return "";
				case ItemID.RedDragonfly://4338 Red Dragonfly
					return "";
				case ItemID.YellowDragonfly://4339 Yellow Dragonfly
					return "";
				case ItemID.GoldDragonfly://4340 Gold Dragonfly
					return "";
				case ItemID.PortableStool://4341 Step Stool
					return "";
				case ItemID.DragonflyStatue://4342 Dragonfly Statue
					return "";
				case ItemID.PaperAirplaneA://4343 Paper Airplane
					return "";
				case ItemID.PaperAirplaneB://4344 White Paper Airplane
					return "";
				case ItemID.CanOfWorms://4345 Can Of Worms
					return "";
				case ItemID.EncumberingStone://4346 Encumbering Stone
					return "";
				case ItemID.ZapinatorGray://4347 Gray Zapinator
					return "";
				case ItemID.ZapinatorOrange://4348 Orange Zapinator
					return "";
				case ItemID.GreenMoss://4349 Green Moss
					return "";
				case ItemID.BrownMoss://4350 Brown Moss
					return "";
				case ItemID.RedMoss://4351 Red Moss
					return "";
				case ItemID.BlueMoss://4352 Blue Moss
					return "";
				case ItemID.PurpleMoss://4353 Purple Moss
					return "";
				case ItemID.LavaMoss://4354 Lava Moss
					return "";
				case ItemID.BoulderStatue://4355 Boulder Statue
					return "";
				case ItemID.MusicBoxTitleAlt://4356 Music Box (Journey's Beginning)
					return "";
				case ItemID.MusicBoxStorm://4357 Music Box (Storm)
					return "";
				case ItemID.MusicBoxGraveyard://4358 Music Box (Graveyard)
					return "";
				case ItemID.Seagull://4359 Seagull
					return "";
				case ItemID.SeagullStatue://4360 Seagull Statue
					return "";
				case ItemID.LadyBug://4361 Ladybug
					return "";
				case ItemID.GoldLadyBug://4362 Gold Ladybug
					return "";
				case ItemID.Maggot://4363 Maggot
					return "";
				case ItemID.MaggotCage://4364 Maggot Cage
					return "";
				case ItemID.CelestialWand://4365 Celestial Wand
					return "";
				case ItemID.EucaluptusSap://4366 Eucalyptus Sap
					return "";
				case ItemID.KiteBlue://4367 Blue Kite
					return "";
				case ItemID.KiteBlueAndYellow://4368 Blue and Yellow Kite
					return "";
				case ItemID.KiteRed://4369 Red Kite
					return "";
				case ItemID.KiteRedAndYellow://4370 Red and Yellow Kite
					return "";
				case ItemID.KiteYellow://4371 Yellow Kite
					return "";
				case ItemID.IvyGuitar://4372 Ivy
					return "";
				case ItemID.Pupfish://4373 Pupfish
					return "";
				case ItemID.Grebe://4374 Grebe
					return "";
				case ItemID.Rat://4375 Rat
					return "";
				case ItemID.RatCage://4376 Rat Cage
					return "";
				case ItemID.KryptonMoss://4377 Krypton Moss
					return "";
				case ItemID.XenonMoss://4378 Xenon Moss
					return "";
				case ItemID.KiteWyvern://4379 Wyvern Kite
					return "";
				case ItemID.LadybugCage://4380 Ladybug Cage
					return "";
				case ItemID.BloodRainBow://4381 Blood Rain Bow
					return "";
				case ItemID.CombatBook://4382 Advanced Combat Techniques
					return "";
				case ItemID.DesertTorch://4383 Desert Torch
					return "";
				case ItemID.CoralTorch://4384 Coral Torch
					return "";
				case ItemID.CorruptTorch://4385 Corrupt Torch
					return "";
				case ItemID.CrimsonTorch://4386 Crimson Torch
					return "";
				case ItemID.HallowedTorch://4387 Hallowed Torch
					return "";
				case ItemID.JungleTorch://4388 Jungle Torch
					return "";
				case ItemID.ArgonMoss://4389 Argon Moss
					return "";
				case ItemID.RollingCactus://4390 Rolling Cactus
					return "";
				case ItemID.ThinIce://4391 Thin Ice
					return "";
				case ItemID.EchoBlock://4392 Echo Block
					return "";
				case ItemID.ScarabFish://4393 Scarab Fish
					return "";
				case ItemID.ScorpioFish://4394 Scorpio Fish
					return "";
				case ItemID.Owl://4395 Owl
					return "";
				case ItemID.OwlCage://4396 Owl Cage
					return "";
				case ItemID.OwlStatue://4397 Owl Statue
					return "";
				case ItemID.PupfishBowl://4398 Pupfish Bowl
					return "";
				case ItemID.GoldLadybugCage://4399 Gold Ladybug Cage
					return "";
				case ItemID.Geode://4400 Geode
					return "";
				case ItemID.Flounder://4401 Flounder
					return "";
				case ItemID.RockLobster://4402 Rock Lobster
					return "";
				case ItemID.LobsterTail://4403 Lobster Tail
					return "";
				case ItemID.FloatingTube://4404 Inner Tube
					return "";
				case ItemID.FrozenCrate://4405 Frozen Crate
					return "";
				case ItemID.FrozenCrateHard://4406 Boreal Crate
					return "";
				case ItemID.OasisCrate://4407 Oasis Crate
					return "";
				case ItemID.OasisCrateHard://4408 Mirage Crate
					return "";
				case ItemID.SpectreGoggles://4409 Spectre Goggles
					return "";
				case ItemID.Oyster://4410 Oyster
					return "";
				case ItemID.ShuckedOyster://4411 Shucked Oyster
					return "";
				case ItemID.WhitePearl://4412 White Pearl
					return "";
				case ItemID.BlackPearl://4413 Black Pearl
					return "";
				case ItemID.PinkPearl://4414 Pink Pearl
					return "";
				case ItemID.StoneDoor://4415 Stone Door
					return "";
				case ItemID.StonePlatform://4416 Stone Platform
					return "";
				case ItemID.OasisFountain://4417 Oasis Water Fountain
					return "";
				case ItemID.WaterStrider://4418 Water Strider
					return "";
				case ItemID.GoldWaterStrider://4419 Gold Water Strider
					return "";
				case ItemID.LawnFlamingo://4420 Lawn Flamingo
					return "";
				case ItemID.MusicBoxUndergroundJungle://4421 Music Box (Underground Jungle)
					return "";
				case ItemID.Grate://4422 Grate
					return "";
				case ItemID.ScarabBomb://4423 Scarab Bomb
					return "";
				case ItemID.WroughtIronFence://4424 Wrought Iron Fence
					return "";
				case ItemID.SharkBait://4425 Shark Bait
					return "";
				case ItemID.BeeMinecart://4426 Bee Minecart
					return "";
				case ItemID.LadybugMinecart://4427 Ladybug Minecart
					return "";
				case ItemID.PigronMinecart://4428 Pigron Minecart
					return "";
				case ItemID.SunflowerMinecart://4429 Sunflower Minecart
					return "";
				case ItemID.PottedForestCedar://4430 Potted Forest Cedar
					return "";
				case ItemID.PottedJungleCedar://4431 Potted Jungle Cedar
					return "";
				case ItemID.PottedHallowCedar://4432 Potted Hallow Cedar
					return "";
				case ItemID.PottedForestTree://4433 Potted Forest Tree
					return "";
				case ItemID.PottedJungleTree://4434 Potted Jungle Tree
					return "";
				case ItemID.PottedHallowTree://4435 Potted Hallow Tree
					return "";
				case ItemID.PottedForestPalm://4436 Potted Forest Palm
					return "";
				case ItemID.PottedJunglePalm://4437 Potted Jungle Palm
					return "";
				case ItemID.PottedHallowPalm://4438 Potted Hallow Palm
					return "";
				case ItemID.PottedForestBamboo://4439 Potted Forest Bamboo
					return "";
				case ItemID.PottedJungleBamboo://4440 Potted Jungle Bamboo
					return "";
				case ItemID.PottedHallowBamboo://4441 Potted Hallow Bamboo
					return "";
				case ItemID.ScarabFishingRod://4442 Scarab Fishing Rod
					return "";
				case ItemID.HellMinecart://4443 Demonic Hellcart
					return "";
				case ItemID.WitchBroom://4444 Witch's Broom
					return "";
				case ItemID.ClusterRocketI://4445 Cluster Rocket I
					return "";
				case ItemID.ClusterRocketII://4446 Cluster Rocket II
					return "";
				case ItemID.WetRocket://4447 Wet Rocket
					return "";
				case ItemID.LavaRocket://4448 Lava Rocket
					return "";
				case ItemID.HoneyRocket://4449 Honey Rocket
					return "";
				case ItemID.ShroomMinecart://4450 Shroom Minecart
					return "";
				case ItemID.AmethystMinecart://4451 Amethyst Minecart
					return "";
				case ItemID.TopazMinecart://4452 Topaz Minecart
					return "";
				case ItemID.SapphireMinecart://4453 Sapphire Minecart
					return "";
				case ItemID.EmeraldMinecart://4454 Emerald Minecart
					return "";
				case ItemID.RubyMinecart://4455 Ruby Minecart
					return "";
				case ItemID.DiamondMinecart://4456 Diamond Minecart
					return "";
				case ItemID.MiniNukeI://4457 Mini Nuke I
					return "";
				case ItemID.MiniNukeII://4458 Mini Nuke II
					return "";
				case ItemID.DryRocket://4459 Dry Rocket
					return "";
				case ItemID.SandcastleBucket://4460 Sandcastle Bucket
					return "";
				case ItemID.TurtleCage://4461 Turtle Cage
					return "";
				case ItemID.TurtleJungleCage://4462 Jungle Turtle Cage
					return "";
				case ItemID.Gladius://4463 Gladius
					return "";
				case ItemID.Turtle://4464 Turtle
					return "";
				case ItemID.TurtleJungle://4465 Jungle Turtle
					return "";
				case ItemID.TurtleStatue://4466 Turtle Statue
					return "";
				case ItemID.AmberMinecart://4467 Amber Minecart
					return "";
				case ItemID.BeetleMinecart://4468 Beetle Minecart
					return "";
				case ItemID.MeowmereMinecart://4469 Meowmere Minecart
					return "";
				case ItemID.PartyMinecart://4470 Party Wagon
					return "";
				case ItemID.PirateMinecart://4471 The Dutchman
					return "";
				case ItemID.SteampunkMinecart://4472 Steampunk Minecart
					return "";
				case ItemID.GrebeCage://4473 Grebe Cage
					return "";
				case ItemID.SeagullCage://4474 Seagull Cage
					return "";
				case ItemID.WaterStriderCage://4475 Water Strider Cage
					return "";
				case ItemID.GoldWaterStriderCage://4476 Gold Water Strider Cage
					return "";
				case ItemID.LuckPotionLesser://4477 Lesser Luck Potion
					return "";
				case ItemID.LuckPotion://4478 Luck Potion
					return "";
				case ItemID.LuckPotionGreater://4479 Greater Luck Potion
					return "";
				case ItemID.Seahorse://4480 Seahorse
					return "";
				case ItemID.SeahorseCage://4481 Seahorse Cage
					return "";
				case ItemID.GoldSeahorse://4482 Gold Seahorse
					return "";
				case ItemID.GoldSeahorseCage://4483 Gold Seahorse Cage
					return "";
				case ItemID.TimerOneHalfSecond://4484 1/2 Second Timer
					return "";
				case ItemID.TimerOneFourthSecond://4485 1/4 Second Timer
					return "";
				case ItemID.EbonstoneEcho://4486 Ebonstone Wall
					return "";
				case ItemID.MudWallEcho://4487 Mud Wall
					return "";
				case ItemID.PearlstoneEcho://4488 Pearlstone Wall
					return "";
				case ItemID.SnowWallEcho://4489 Snow Wall
					return "";
				case ItemID.AmethystEcho://4490 Amethyst Stone Wall
					return "";
				case ItemID.TopazEcho://4491 Topaz Stone Wall
					return "";
				case ItemID.SapphireEcho://4492 Sapphire Stone Wall
					return "";
				case ItemID.EmeraldEcho://4493 Emerald Stone Wall
					return "";
				case ItemID.RubyEcho://4494 Ruby Stone Wall
					return "";
				case ItemID.DiamondEcho://4495 Diamond Stone Wall
					return "";
				case ItemID.Cave1Echo://4496 Green Mossy Wall
					return "";
				case ItemID.Cave2Echo://4497 Brown Mossy Wall
					return "";
				case ItemID.Cave3Echo://4498 Red Mossy Wall
					return "";
				case ItemID.Cave4Echo://4499 Blue Mossy Wall
					return "";
				case ItemID.Cave5Echo://4500 Purple Mossy Wall
					return "";
				case ItemID.Cave6Echo://4501 Rocky Dirt Wall
					return "";
				case ItemID.Cave7Echo://4502 Old Stone Wall
					return "";
				case ItemID.SpiderEcho://4503 Spider Wall
					return "";
				case ItemID.CorruptGrassEcho://4504 Corrupt Grass Wall
					return "";
				case ItemID.HallowedGrassEcho://4505 Hallowed Grass Wall
					return "";
				case ItemID.IceEcho://4506 Ice Wall
					return "";
				case ItemID.ObsidianBackEcho://4507 Obsidian Wall
					return "";
				case ItemID.CrimsonGrassEcho://4508 Crimson Grass Wall
					return "";
				case ItemID.CrimstoneEcho://4509 Crimstone Wall
					return "";
				case ItemID.CaveWall1Echo://4510 Cave Dirt Wall
					return "";
				case ItemID.CaveWall2Echo://4511 Rough Dirt Wall
					return "";
				case ItemID.Cave8Echo://4512 Craggy Stone Wall
					return "";
				case ItemID.Corruption1Echo://4513 Corrupt Growth Wall
					return "";
				case ItemID.Corruption2Echo://4514 Corrupt Mass Wall
					return "";
				case ItemID.Corruption3Echo://4515 Corrupt Pustule Wall
					return "";
				case ItemID.Corruption4Echo://4516 Corrupt Tendril Wall
					return "";
				case ItemID.Crimson1Echo://4517 Crimson Crust Wall
					return "";
				case ItemID.Crimson2Echo://4518 Crimson Scab Wall
					return "";
				case ItemID.Crimson3Echo://4519 Crimson Teeth Wall
					return "";
				case ItemID.Crimson4Echo://4520 Crimson Blister Wall
					return "";
				case ItemID.Dirt1Echo://4521 Layered Dirt Wall
					return "";
				case ItemID.Dirt2Echo://4522 Crumbling Dirt Wall
					return "";
				case ItemID.Dirt3Echo://4523 Cracked Dirt Wall
					return "";
				case ItemID.Dirt4Echo://4524 Wavy Dirt Wall
					return "";
				case ItemID.Hallow1Echo://4525 Hallowed Prism Wall
					return "";
				case ItemID.Hallow2Echo://4526 Hallowed Cavern Wall
					return "";
				case ItemID.Hallow3Echo://4527 Hallowed Shard Wall
					return "";
				case ItemID.Hallow4Echo://4528 Hallowed Crystalline Wall
					return "";
				case ItemID.Jungle1Echo://4529 Lichen Stone Wall
					return "";
				case ItemID.Jungle2Echo://4530 Leafy Jungle Wall
					return "";
				case ItemID.Jungle3Echo://4531 Ivy Stone Wall
					return "";
				case ItemID.Jungle4Echo://4532 Jungle Vine Wall
					return "";
				case ItemID.Lava1Echo://4533 Ember Wall
					return "";
				case ItemID.Lava2Echo://4534 Cinder Wall
					return "";
				case ItemID.Lava3Echo://4535 Magma Wall
					return "";
				case ItemID.Lava4Echo://4536 Smouldering Stone Wall
					return "";
				case ItemID.Rocks1Echo://4537 Worn Stone Wall
					return "";
				case ItemID.Rocks2Echo://4538 Stalactite Stone Wall
					return "";
				case ItemID.Rocks3Echo://4539 Mottled Stone Wall
					return "";
				case ItemID.Rocks4Echo://4540 Fractured Stone Wall
					return "";
				case ItemID.TheBrideBanner://4541 The Bride Banner
					return "";
				case ItemID.ZombieMermanBanner://4542 Zombie Merman Banner
					return "";
				case ItemID.EyeballFlyingFishBanner://4543 Wandering Eye Fish Banner
					return "";
				case ItemID.BloodSquidBanner://4544 Blood Squid Banner
					return "";
				case ItemID.BloodEelBanner://4545 Blood Eel Banner
					return "";
				case ItemID.GoblinSharkBanner://4546 Hemogoblin Shark Banner
					return "";
				case ItemID.LargeBambooBlock://4547 Large Bamboo
					return "";
				case ItemID.LargeBambooBlockWall://4548 Large Bamboo Wall
					return "";
				case ItemID.DemonHorns://4549 Demon Horns
					return "";
				case ItemID.BambooLeaf://4550 Bamboo Leaf
					return "";
				case ItemID.HellCake://4551 Slice of Hell Cake
					return "";
				case ItemID.FogMachine://4552 Fog Machine
					return "";
				case ItemID.PlasmaLamp://4553 Plasma Lamp
					return "";
				case ItemID.MarbleColumn://4554 Marble Column
					return "";
				case ItemID.ChefHat://4555 Chef Hat
					return "";
				case ItemID.ChefShirt://4556 Chef Uniform
					return "";
				case ItemID.ChefPants://4557 Chef Pants
					return "";
				case ItemID.StarHairpin://4558 Star Hairpin
					return "";
				case ItemID.HeartHairpin://4559 Heart Hairpin
					return "";
				case ItemID.BunnyEars://4560 Bunny Ears
					return "";
				case ItemID.DevilHorns://4561 Devil Horns
					return "";
				case ItemID.Fedora://4562 Fedora
					return "";
				case ItemID.UnicornHornHat://4563 Fake Unicorn Horn
					return "";
				case ItemID.BambooBlock://4564 Bamboo
					return "";
				case ItemID.BambooBlockWall://4565 Bamboo Wall
					return "";
				case ItemID.BambooBathtub://4566 Bamboo Bathtub
					return "";
				case ItemID.BambooBed://4567 Bamboo Bed
					return "";
				case ItemID.BambooBookcase://4568 Bamboo Bookcase
					return "";
				case ItemID.BambooDresser://4569 Bamboo Dresser
					return "";
				case ItemID.BambooCandelabra://4570 Bamboo Candelabra
					return "";
				case ItemID.BambooCandle://4571 Bamboo Candle
					return "";
				case ItemID.BambooChair://4572 Bamboo Chair
					return "";
				case ItemID.BambooChandelier://4573 Bamboo Chandelier
					return "";
				case ItemID.BambooChest://4574 Bamboo Chest
					return "";
				case ItemID.BambooClock://4575 Bamboo Clock
					return "";
				case ItemID.BambooDoor://4576 Bamboo Door
					return "";
				case ItemID.BambooLamp://4577 Bamboo Lamp
					return "";
				case ItemID.BambooLantern://4578 Bamboo Lantern
					return "";
				case ItemID.BambooPiano://4579 Bamboo Piano
					return "";
				case ItemID.BambooPlatform://4580 Bamboo Platform
					return "";
				case ItemID.BambooSink://4581 Bamboo Sink
					return "";
				case ItemID.BambooSofa://4582 Bamboo Sofa
					return "";
				case ItemID.BambooTable://4583 Bamboo Table
					return "";
				case ItemID.BambooWorkbench://4584 Bamboo Work Bench
					return "";
				case ItemID.Fake_BambooChest://4585 Trapped Bamboo Chest
					return "";
				case ItemID.BambooToilet://4586 Bamboo Toilet
					return "";
				case ItemID.GolfClubStoneIron://4587 Worn Golf Club (Iron)
					return "";
				case ItemID.GolfClubRustyPutter://4588 Worn Golf Club (Putter)
					return "";
				case ItemID.GolfClubBronzeWedge://4589 Worn Golf Club (Wedge)
					return "";
				case ItemID.GolfClubWoodDriver://4590 Worn Golf Club (Driver)
					return "";
				case ItemID.GolfClubMythrilIron://4591 Fancy Golf Club (Iron)
					return "";
				case ItemID.GolfClubLeadPutter://4592 Fancy Golf Club (Putter)
					return "";
				case ItemID.GolfClubGoldWedge://4593 Fancy Golf Club (Wedge)
					return "";
				case ItemID.GolfClubPearlwoodDriver://4594 Fancy Golf Club (Driver)
					return "";
				case ItemID.GolfClubTitaniumIron://4595 Premium Golf Club (Iron)
					return "";
				case ItemID.GolfClubShroomitePutter://4596 Premium Golf Club (Putter)
					return "";
				case ItemID.GolfClubDiamondWedge://4597 Premium Golf Club (Wedge)
					return "";
				case ItemID.GolfClubChlorophyteDriver://4598 Premium Golf Club (Driver)
					return "";
				case ItemID.GolfTrophyBronze://4599 Bronze Golf Trophy
					return "";
				case ItemID.GolfTrophySilver://4600 Silver Golf Trophy
					return "";
				case ItemID.GolfTrophyGold://4601 Gold Golf Trophy
					return "";
				case ItemID.BloodNautilusBanner://4602 Dreadnautilus Banner
					return "";
				case ItemID.BirdieRattle://4603 Birdie Rattle
					return "";
				case ItemID.ExoticEasternChewToy://4604 Exotic Chew Toy
					return "";
				case ItemID.BedazzledNectar://4605 Bedazzled Nectar
					return "";
				case ItemID.MusicBoxJungleNight://4606 Music Box (Jungle Night)
					return "";
				case ItemID.StormTigerStaff://4607 Desert Tiger Staff
					return "";
				case ItemID.ChumBucket://4608 Chum Bucket
					return "";
				case ItemID.GardenGnome://4609 Garden Gnome
					return "";
				case ItemID.KiteBoneSerpent://4610 Bone Serpent Kite
					return "";
				case ItemID.KiteWorldFeeder://4611 World Feeder Kite
					return "";
				case ItemID.KiteBunny://4612 Bunny Kite
					return "";
				case ItemID.KitePigron://4613 Pigron Kite
					return "";
				case ItemID.AppleJuice://4614 Apple Juice
					return "";
				case ItemID.GrapeJuice://4615 Grape Juice
					return "";
				case ItemID.Lemonade://4616 Lemonade
					return "";
				case ItemID.BananaDaiquiri://4617 Frozen Banana Daiquiri
					return "";
				case ItemID.PeachSangria://4618 Peach Sangria
					return "";
				case ItemID.PinaColada://4619 Piña Colada
					return "";
				case ItemID.TropicalSmoothie://4620 Tropical Smoothie
					return "";
				case ItemID.BloodyMoscato://4621 Bloody Moscato
					return "";
				case ItemID.SmoothieofDarkness://4622 Smoothie of Darkness
					return "";
				case ItemID.PrismaticPunch://4623 Prismatic Punch
					return "";
				case ItemID.FruitJuice://4624 Fruit Juice
					return "";
				case ItemID.FruitSalad://4625 Fruit Salad
					return "";
				case ItemID.AndrewSphinx://4626 Andrew Sphinx
					return "";
				case ItemID.WatchfulAntlion://4627 Watchful Antlion
					return "";
				case ItemID.BurningSpirit://4628 Burning Spirit
					return "";
				case ItemID.JawsOfDeath://4629 Jaws of Death
					return "";
				case ItemID.TheSandsOfSlime://4630 The Sands of Slime
					return "";
				case ItemID.SnakesIHateSnakes://4631 Snakes, I Hate Snakes
					return "";
				case ItemID.LifeAboveTheSand://4632 Life Above the Sand
					return "";
				case ItemID.Oasis://4633 Oasis
					return "";
				case ItemID.PrehistoryPreserved://4634 Prehistory Preserved
					return "";
				case ItemID.AncientTablet://4635 Ancient Tablet
					return "";
				case ItemID.Uluru://4636 Uluru
					return "";
				case ItemID.VisitingThePyramids://4637 Visiting the Pyramids
					return "";
				case ItemID.BandageBoy://4638 Bandage Boy
					return "";
				case ItemID.DivineEye://4639 Divine Eye
					return "";
				case ItemID.AmethystStoneBlock://4640 Amethyst Stone Block
					return "";
				case ItemID.TopazStoneBlock://4641 Topaz Stone Block
					return "";
				case ItemID.SapphireStoneBlock://4642 Sapphire Stone Block
					return "";
				case ItemID.EmeraldStoneBlock://4643 Emerald Stone Block
					return "";
				case ItemID.RubyStoneBlock://4644 Ruby Stone Block
					return "";
				case ItemID.DiamondStoneBlock://4645 Diamond Stone Block
					return "";
				case ItemID.AmberStoneBlock://4646 Amber Stone Block
					return "";
				case ItemID.AmberStoneWallEcho://4647 Amber Stone Wall
					return "";
				case ItemID.KiteManEater://4648 Man Eater Kite
					return "";
				case ItemID.KiteJellyfishBlue://4649 Blue Jellyfish Kite
					return "";
				case ItemID.KiteJellyfishPink://4650 Pink Jellyfish Kite
					return "";
				case ItemID.KiteShark://4651 Shark Kite
					return "";
				case ItemID.SuperHeroMask://4652 Superhero Mask
					return "";
				case ItemID.SuperHeroCostume://4653 Superhero Costume
					return "";
				case ItemID.SuperHeroTights://4654 Superhero Tights
					return "";
				case ItemID.PinkFairyJar://4655 Pink Fairy Jar
					return "";
				case ItemID.GreenFairyJar://4656 Green Fairy Jar
					return "";
				case ItemID.BlueFairyJar://4657 Blue Fairy Jar
					return "";
				case ItemID.GolfPainting1://4658 The Rolling Greens
					return "";
				case ItemID.GolfPainting2://4659 Study of a Ball at Rest
					return "";
				case ItemID.GolfPainting3://4660 Fore!
					return "";
				case ItemID.GolfPainting4://4661 The Duplicity of Reflections
					return "";
				case ItemID.FogboundDye://4662 Fogbound Dye
					return "";
				case ItemID.BloodbathDye://4663 Bloodbath Dye
					return "";
				case ItemID.PrettyPinkDressSkirt://4664 Pretty Pink Dress
					return "";
				case ItemID.PrettyPinkDressPants://4665 Pretty Pink Stockings
					return "";
				case ItemID.PrettyPinkRibbon://4666 Pretty Pink Ribbon
					return "";
				case ItemID.BambooFence://4667 Bamboo Fence
					return "";
				case ItemID.GlowPaint://4668 Illuminant Coating
					return "";
				case ItemID.KiteSandShark://4669 Sand Shark Kite
					return "";
				case ItemID.KiteBunnyCorrupt://4670 Corrupt Bunny Kite
					return "";
				case ItemID.KiteBunnyCrimson://4671 Crimson Bunny Kite
					return "";
				case ItemID.BlandWhip://4672 Leather Whip
					return "";
				case ItemID.DrumStick://4673 Drumstick
					return "";
				case ItemID.KiteGoldfish://4674 Goldfish Kite
					return "";
				case ItemID.KiteAngryTrapper://4675 Angry Trapper Kite
					return "";
				case ItemID.KiteKoi://4676 Koi Kite
					return "";
				case ItemID.KiteCrawltipede://4677 Crawltipede Kite
					return "";
				case ItemID.SwordWhip://4678 Durendal
					return "";
				case ItemID.MaceWhip://4679 Morning Star
					return "";
				case ItemID.ScytheWhip://4680 Dark Harvest
					return "";
				case ItemID.KiteSpectrum://4681 Spectrum Kite
					return "";
				case ItemID.ReleaseDoves://4682 Release Doves
					return "";
				case ItemID.KiteWanderingEye://4683 Wandering Eye Kite
					return "";
				case ItemID.KiteUnicorn://4684 Unicorn Kite
					return "";
				case ItemID.UndertakerHat://4685 Gravedigger Hat
					return "";
				case ItemID.UndertakerCoat://4686 Gravedigger Coat
					return "";
				case ItemID.DandelionBanner://4687 Angry Dandelion Banner
					return "";
				case ItemID.GnomeBanner://4688 Gnome Banner
					return "";
				case ItemID.DesertCampfire://4689 Desert Campfire
					return "";
				case ItemID.CoralCampfire://4690 Coral Campfire
					return "";
				case ItemID.CorruptCampfire://4691 Corrupt Campfire
					return "";
				case ItemID.CrimsonCampfire://4692 Crimson Campfire
					return "";
				case ItemID.HallowedCampfire://4693 Hallowed Campfire
					return "";
				case ItemID.JungleCampfire://4694 Jungle Campfire
					return "";
				case ItemID.SoulBottleLight://4695 Soul of Light in a Bottle
					return "";
				case ItemID.SoulBottleNight://4696 Soul of Night in a Bottle
					return "";
				case ItemID.SoulBottleFlight://4697 Soul of Flight in a Bottle
					return "";
				case ItemID.SoulBottleSight://4698 Soul of Sight in a Bottle
					return "";
				case ItemID.SoulBottleMight://4699 Soul of Might in a Bottle
					return "";
				case ItemID.SoulBottleFright://4700 Soul of Fright in a Bottle
					return "";
				case ItemID.MudBud://4701 Mud Bud
					return "";
				case ItemID.ReleaseLantern://4702 Release Lantern
					return "";
				case ItemID.QuadBarrelShotgun://4703 Quad-Barrel Shotgun
					return "";
				case ItemID.FuneralHat://4704 Funeral Hat
					return "";
				case ItemID.FuneralCoat://4705 Funeral Coat
					return "";
				case ItemID.FuneralPants://4706 Funeral Pants
					return "";
				case ItemID.TragicUmbrella://4707 Tragic Umbrella
					return "";
				case ItemID.VictorianGothHat://4708 Victorian Goth Hat
					return "";
				case ItemID.VictorianGothDress://4709 Victorian Goth Dress
					return "";
				case ItemID.TatteredWoodSign://4710 Tattered Wood Sign
					return "";
				case ItemID.GravediggerShovel://4711 Gravedigger's Shovel
					return "";
				case ItemID.DungeonDesertChest://4712 Desert Chest
					return "";
				case ItemID.Fake_DungeonDesertChest://4713 Trapped Desert Chest
					return "";
				case ItemID.DungeonDesertKey://4714 Desert Key
					return "";
				case ItemID.SparkleGuitar://4715 Stellar Tune
					return "";
				case ItemID.MolluskWhistle://4716 Mollusk Whistle
					return "";
				case ItemID.BorealBeam://4717 Boreal Beam
					return "";
				case ItemID.RichMahoganyBeam://4718 Rich Mahogany Beam
					return "";
				case ItemID.GraniteColumn://4719 Granite Column
					return "";
				case ItemID.SandstoneColumn://4720 Sandstone Column
					return "";
				case ItemID.MushroomBeam://4721 Mushroom Beam
					return "";
				case ItemID.FirstFractal://4722 
					return "";
				case ItemID.Nevermore://4723 Nevermore
					return "";
				case ItemID.Reborn://4724 Reborn
					return "";
				case ItemID.Graveyard://4725 Graveyard
					return "";
				case ItemID.GhostManifestation://4726 Ghost Manifestation
					return "";
				case ItemID.WickedUndead://4727 Wicked Undead
					return "";
				case ItemID.BloodyGoblet://4728 Bloody Goblet
					return "";
				case ItemID.StillLife://4729 Still Life
					return "";
				case ItemID.GhostarsWings://4730 Ghostar's Infinity Eight
					return "";
				case ItemID.TerraToilet://4731 Terra Toilet
					return "";
				case ItemID.GhostarSkullPin://4732 Ghostar's Soul Jar
					return "";
				case ItemID.GhostarShirt://4733 Ghostar's Garb
					return "";
				case ItemID.GhostarPants://4734 Ghostar's Tights
					return "";
				case ItemID.BallOfFuseWire://4735 Ball O' Fuse Wire
					return "";
				case ItemID.FullMoonSqueakyToy://4736 Full Moon Squeaky Toy
					return "";
				case ItemID.OrnateShadowKey://4737 Ornate Shadow Key
					return "";
				case ItemID.DrManFlyMask://4738 Dr. Man Fly Mask
					return "";
				case ItemID.DrManFlyLabCoat://4739 Dr. Man Fly's Lab Coat
					return "";
				case ItemID.ButcherMask://4740 Butcher Mask
					return "";
				case ItemID.ButcherApron://4741 Butcher's Bloodstained Apron
					return "";
				case ItemID.ButcherPants://4742 Butcher's Bloodstained Pants
					return "";
				case ItemID.Football://4743 Football
					return "";
				case ItemID.HunterCloak://4744 Hunter Cloak
					return "";
				case ItemID.CoffinMinecart://4745 Coffin Minecart
					return "";
				case ItemID.SafemanWings://4746 Safeman's Blanket Cape
					return "";
				case ItemID.SafemanSunHair://4747 Safeman's Sunny Day
					return "";
				case ItemID.SafemanSunDress://4748 Safeman's Sun Dress
					return "";
				case ItemID.SafemanDressLeggings://4749 Safeman's Pink Leggings
					return "";
				case ItemID.FoodBarbarianWings://4750 FoodBarbarian's Tattered Dragon Wings
					return "";
				case ItemID.FoodBarbarianHelm://4751 FoodBarbarian's Horned Helm
					return "";
				case ItemID.FoodBarbarianArmor://4752 FoodBarbarian's Wild Wolf Spaulders
					return "";
				case ItemID.FoodBarbarianGreaves://4753 FoodBarbarian's Savage Greaves
					return "";
				case ItemID.GroxTheGreatWings://4754 Grox The Great's Wings
					return "";
				case ItemID.GroxTheGreatHelm://4755 Grox The Great's Horned Cowl
					return "";
				case ItemID.GroxTheGreatArmor://4756 Grox The Great's Chestplate
					return "";
				case ItemID.GroxTheGreatGreaves://4757 Grox The Great's Greaves
					return "";
				case ItemID.Smolstar://4758 Blade Staff
					return "";
				case ItemID.SquirrelHook://4759 Squirrel Hook
					return "";
				case ItemID.BouncingShield://4760 Sergeant United Shield
					return "";
				case ItemID.RockGolemHead://4761 Rock Golem Head
					return "";
				case ItemID.CritterShampoo://4762 Critter Shampoo
					return "";
				case ItemID.DiggingMoleMinecart://4763 Digging Molecart
					return "";
				case ItemID.Shroomerang://4764 Shroomerang
					return "";
				case ItemID.TreeGlobe://4765 Tree Globe
					return "";
				case ItemID.WorldGlobe://4766 World Globe
					return "";
				case ItemID.DontHurtCrittersBook://4767 Guide to Critter Companionship
					return "";
				case ItemID.DogEars://4768 Dog Ears
					return "";
				case ItemID.DogTail://4769 Dog Tail
					return "";
				case ItemID.FoxEars://4770 Fox Ears
					return "";
				case ItemID.FoxTail://4771 Fox Tail
					return "";
				case ItemID.LizardEars://4772 Lizard Ears
					return "";
				case ItemID.LizardTail://4773 Lizard Tail
					return "";
				case ItemID.PandaEars://4774 Panda Ears
					return "";
				case ItemID.BunnyTail://4775 Bunny Tail
					return "";
				case ItemID.FairyGlowstick://4776 Fairy Glowstick
					return "";
				case ItemID.LightningCarrot://4777 Lightning Carrot
					return "";
				case ItemID.HallowBossDye://4778 Prismatic Dye
					return "";
				case ItemID.MushroomHat://4779 Mushroom Hat
					return "";
				case ItemID.MushroomVest://4780 Mushroom Vest
					return "";
				case ItemID.MushroomPants://4781 Mushroom Pants
					return "";
				case ItemID.FairyQueenBossBag://4782 Treasure Bag (Empress of Light)
					return "";
				case ItemID.FairyQueenTrophy://4783 Empress of Light Trophy
					return "";
				case ItemID.FairyQueenMask://4784 Empress of Light Mask
					return "";
				case ItemID.PaintedHorseSaddle://4785 Dusty Rawhide Saddle
					return "";
				case ItemID.MajesticHorseSaddle://4786 Royal Gilded Saddle
					return "";
				case ItemID.DarkHorseSaddle://4787 Black Studded Saddle
					return "";
				case ItemID.JoustingLance://4788 Jousting Lance
					return "";
				case ItemID.ShadowJoustingLance://4789 Shadow Jousting Lance
					return "";
				case ItemID.HallowJoustingLance://4790 Hallowed Jousting Lance
					return "";
				case ItemID.PogoStick://4791 Pogo Stick
					return "";
				case ItemID.PirateShipMountItem://4792 The Black Spot
					return "";
				case ItemID.SpookyWoodMountItem://4793 Hexxed Branch
					return "";
				case ItemID.SantankMountItem://4794 Toy Tank
					return "";
				case ItemID.WallOfFleshGoatMountItem://4795 Goat Skull
					return "";
				case ItemID.DarkMageBookMountItem://4796 Dark Mage's Tome
					return "";
				case ItemID.KingSlimePetItem://4797 Royal Delight
					return "";
				case ItemID.EyeOfCthulhuPetItem://4798 Suspicious Grinning Eye
					return "";
				case ItemID.EaterOfWorldsPetItem://4799 Writhing Remains
					return "";
				case ItemID.BrainOfCthulhuPetItem://4800 Brain in a Jar
					return "";
				case ItemID.SkeletronPetItem://4801 Possessed Skull
					return "";
				case ItemID.QueenBeePetItem://4802 Sparkling Honey
					return "";
				case ItemID.DestroyerPetItem://4803 Deactivated Probe
					return "";
				case ItemID.TwinsPetItem://4804 Pair of Eyeballs
					return "";
				case ItemID.SkeletronPrimePetItem://4805 Robotic Skull
					return "";
				case ItemID.PlanteraPetItem://4806 Plantera Seedling
					return "";
				case ItemID.GolemPetItem://4807 Guardian Golem
					return "";
				case ItemID.DukeFishronPetItem://4808 Pork of the Sea
					return "";
				case ItemID.LunaticCultistPetItem://4809 Tablet Fragment
					return "";
				case ItemID.MoonLordPetItem://4810 Piece of Moon Squid
					return "";
				case ItemID.FairyQueenPetItem://4811 Jewel of Light
					return "";
				case ItemID.PumpkingPetItem://4812 Pumpkin Scented Candle
					return "";
				case ItemID.EverscreamPetItem://4813 Shrub Star
					return "";
				case ItemID.IceQueenPetItem://4814 Frozen Crown
					return "";
				case ItemID.MartianPetItem://4815 Cosmic Skateboard
					return "";
				case ItemID.DD2OgrePetItem://4816 Ogre's Club
					return "";
				case ItemID.DD2BetsyPetItem://4817 Betsy's Egg
					return "";
				case ItemID.CombatWrench://4818 Combat Wrench
					return "";
				case ItemID.DemonConch://4819 Demon Conch
					return "";
				case ItemID.BottomlessLavaBucket://4820 Bottomless Lava Bucket
					return "";
				case ItemID.FireproofBugNet://4821 Lavaproof Bug Net
					return "";
				case ItemID.FlameWakerBoots://4822 Flame Waker Boots
					return "";
				case ItemID.RainbowWings://4823 Empress Wings
					return "";
				case ItemID.WetBomb://4824 Wet Bomb
					return "";
				case ItemID.LavaBomb://4825 Lava Bomb
					return "";
				case ItemID.HoneyBomb://4826 Honey Bomb
					return "";
				case ItemID.DryBomb://4827 Dry Bomb
					return "";
				case ItemID.SuperheatedBlood://4828 Superheated Blood
					return "";
				case ItemID.LicenseCat://4829 Cat License
					return "";
				case ItemID.LicenseDog://4830 Dog License
					return "";
				case ItemID.GemSquirrelAmethyst://4831 Amethyst Squirrel
					return "";
				case ItemID.GemSquirrelTopaz://4832 Topaz Squirrel
					return "";
				case ItemID.GemSquirrelSapphire://4833 Sapphire Squirrel
					return "";
				case ItemID.GemSquirrelEmerald://4834 Emerald Squirrel
					return "";
				case ItemID.GemSquirrelRuby://4835 Ruby Squirrel
					return "";
				case ItemID.GemSquirrelDiamond://4836 Diamond Squirrel
					return "";
				case ItemID.GemSquirrelAmber://4837 Amber Squirrel
					return "";
				case ItemID.GemBunnyAmethyst://4838 Amethyst Bunny
					return "";
				case ItemID.GemBunnyTopaz://4839 Topaz Bunny
					return "";
				case ItemID.GemBunnySapphire://4840 Sapphire Bunny
					return "";
				case ItemID.GemBunnyEmerald://4841 Emerald Bunny
					return "";
				case ItemID.GemBunnyRuby://4842 Ruby Bunny
					return "";
				case ItemID.GemBunnyDiamond://4843 Diamond Bunny
					return "";
				case ItemID.GemBunnyAmber://4844 Amber Bunny
					return "";
				case ItemID.HellButterfly://4845 Hell Butterfly
					return "";
				case ItemID.HellButterflyJar://4846 Hell Butterfly Jar
					return "";
				case ItemID.Lavafly://4847 Lavafly
					return "";
				case ItemID.LavaflyinaBottle://4848 Lavafly in a Bottle
					return "";
				case ItemID.MagmaSnail://4849 Magma Snail
					return "";
				case ItemID.MagmaSnailCage://4850 Magma Snail Cage
					return "";
				case ItemID.GemTreeTopazSeed://4851 Topaz Gemcorn
					return "";
				case ItemID.GemTreeAmethystSeed://4852 Amethyst Gemcorn
					return "";
				case ItemID.GemTreeSapphireSeed://4853 Sapphire Gemcorn
					return "";
				case ItemID.GemTreeEmeraldSeed://4854 Emerald Gemcorn
					return "";
				case ItemID.GemTreeRubySeed://4855 Ruby Gemcorn
					return "";
				case ItemID.GemTreeDiamondSeed://4856 Diamond Gemcorn
					return "";
				case ItemID.GemTreeAmberSeed://4857 Amber Gemcorn
					return "";
				case ItemID.PotSuspended://4858 Hanging Pot
					return "";
				case ItemID.PotSuspendedDaybloom://4859 Hanging Daybloom
					return "";
				case ItemID.PotSuspendedMoonglow://4860 Hanging Moonglow
					return "";
				case ItemID.PotSuspendedWaterleaf://4861 Hanging Waterleaf
					return "";
				case ItemID.PotSuspendedShiverthorn://4862 Hanging Shiverthorn
					return "";
				case ItemID.PotSuspendedBlinkroot://4863 Hanging Blinkroot
					return "";
				case ItemID.PotSuspendedDeathweedCorrupt://4864 Hanging Corrupt Deathweed
					return "";
				case ItemID.PotSuspendedDeathweedCrimson://4865 Hanging Crimson Deathweed
					return "";
				case ItemID.PotSuspendedFireblossom://4866 Hanging Fireblossom
					return "";
				case ItemID.BrazierSuspended://4867 Hanging Brazier
					return "";
				case ItemID.VolcanoSmall://4868 Mini Volcano
					return "";
				case ItemID.VolcanoLarge://4869 Large Volcano
					return "";
				case ItemID.PotionOfReturn://4870 Potion of Return
					return "";
				case ItemID.VanityTreeSakuraSeed://4871 Sakura Sapling
					return "";
				case ItemID.LavaAbsorbantSponge://4872 Lava Absorbant Sponge
					return "";
				case ItemID.HallowedHood://4873 Hallowed Hood
					return "";
				case ItemID.HellfireTreads://4874 Hellfire Treads
					return "";
				case ItemID.TeleportationPylonJungle://4875 Jungle Pylon
					return "";
				case ItemID.TeleportationPylonPurity://4876 Forest Pylon
					return "";
				case ItemID.LavaCrate://4877 Obsidian Crate
					return "";
				case ItemID.LavaCrateHard://4878 Hellstone Crate
					return "";
				case ItemID.ObsidianLockbox://4879 Obsidian Lock Box
					return "";
				case ItemID.LavaFishbowl://4880 Lava Serpent Bowl
					return "";
				case ItemID.LavaFishingHook://4881 Lavaproof Fishing Hook
					return "";
				case ItemID.AmethystBunnyCage://4882 Amethyst Bunny Cage
					return "";
				case ItemID.TopazBunnyCage://4883 Topaz Bunny Cage
					return "";
				case ItemID.SapphireBunnyCage://4884 Sapphire Bunny Cage
					return "";
				case ItemID.EmeraldBunnyCage://4885 Emerald Bunny Cage
					return "";
				case ItemID.RubyBunnyCage://4886 Ruby Bunny Cage
					return "";
				case ItemID.DiamondBunnyCage://4887 Diamond Bunny Cage
					return "";
				case ItemID.AmberBunnyCage://4888 Amber Bunny Cage
					return "";
				case ItemID.AmethystSquirrelCage://4889 Amethyst Squirrel Cage
					return "";
				case ItemID.TopazSquirrelCage://4890 Topaz Squirrel Cage
					return "";
				case ItemID.SapphireSquirrelCage://4891 Sapphire Squirrel Cage
					return "";
				case ItemID.EmeraldSquirrelCage://4892 Emerald Squirrel Cage
					return "";
				case ItemID.RubySquirrelCage://4893 Ruby Squirrel Cage
					return "";
				case ItemID.DiamondSquirrelCage://4894 Diamond Squirrel Cage
					return "";
				case ItemID.AmberSquirrelCage://4895 Amber Squirrel Cage
					return "";
				case ItemID.AncientHallowedMask://4896 Ancient Hallowed Mask
					return "";
				case ItemID.AncientHallowedHelmet://4897 Ancient Hallowed Helmet
					return "";
				case ItemID.AncientHallowedHeadgear://4898 Ancient Hallowed Headgear
					return "";
				case ItemID.AncientHallowedHood://4899 Ancient Hallowed Hood
					return "";
				case ItemID.AncientHallowedPlateMail://4900 Ancient Hallowed Plate Mail
					return "";
				case ItemID.AncientHallowedGreaves://4901 Ancient Hallowed Greaves
					return "";
				case ItemID.PottedLavaPlantPalm://4902 Potted Magma Palm
					return "";
				case ItemID.PottedLavaPlantBush://4903 Potted Brimstone Bush
					return "";
				case ItemID.PottedLavaPlantBramble://4904 Potted Fire Brambles
					return "";
				case ItemID.PottedLavaPlantBulb://4905 Potted Lava Bulb
					return "";
				case ItemID.PottedLavaPlantTendrils://4906 Potted Ember Tendrils
					return "";
				case ItemID.VanityTreeYellowWillowSeed://4907 Yellow Willow Sapling
					return "";
				case ItemID.DirtBomb://4908 Dirt Bomb
					return "";
				case ItemID.DirtStickyBomb://4909 Sticky Dirt Bomb
					return "";
				case ItemID.LicenseBunny://4910 Bunny License
					return "";
				case ItemID.CoolWhip://4911 Cool Whip
					return "";
				case ItemID.FireWhip://4912 Firecracker
					return "";
				case ItemID.ThornWhip://4913 Snapthorn
					return "";
				case ItemID.RainbowWhip://4914 Kaleidoscope
					return "";
				case ItemID.TungstenBullet://4915 Tungsten Bullet
					return "";
				case ItemID.TeleportationPylonHallow://4916 Hallow Pylon
					return "";
				case ItemID.TeleportationPylonUnderground://4917 Cavern Pylon
					return "";
				case ItemID.TeleportationPylonOcean://4918 Ocean Pylon
					return "";
				case ItemID.TeleportationPylonDesert://4919 Desert Pylon
					return "";
				case ItemID.TeleportationPylonSnow://4920 Snow Pylon
					return "";
				case ItemID.TeleportationPylonMushroom://4921 Mushroom Pylon
					return "";
				case ItemID.CavernFountain://4922 Cavern Water Fountain
					return "";
				case ItemID.PiercingStarlight://4923 Starlight
					return "";
				case ItemID.EyeofCthulhuMasterTrophy://4924 Eye of Cthulhu Relic
					return "";
				case ItemID.EaterofWorldsMasterTrophy://4925 Eater of Worlds Relic
					return "";
				case ItemID.BrainofCthulhuMasterTrophy://4926 Brain of Cthulhu Relic
					return "";
				case ItemID.SkeletronMasterTrophy://4927 Skeletron Relic
					return "";
				case ItemID.QueenBeeMasterTrophy://4928 Queen Bee Relic
					return "";
				case ItemID.KingSlimeMasterTrophy://4929 King Slime Relic
					return "";
				case ItemID.WallofFleshMasterTrophy://4930 Wall of Flesh Relic
					return "";
				case ItemID.TwinsMasterTrophy://4931 Twins Relic
					return "";
				case ItemID.DestroyerMasterTrophy://4932 Destroyer Relic
					return "";
				case ItemID.SkeletronPrimeMasterTrophy://4933 Skeletron Prime Relic
					return "";
				case ItemID.PlanteraMasterTrophy://4934 Plantera Relic
					return "";
				case ItemID.GolemMasterTrophy://4935 Golem Relic
					return "";
				case ItemID.DukeFishronMasterTrophy://4936 Duke Fishron Relic
					return "";
				case ItemID.LunaticCultistMasterTrophy://4937 Lunatic Cultist Relic
					return "";
				case ItemID.MoonLordMasterTrophy://4938 Moon Lord Relic
					return "";
				case ItemID.UFOMasterTrophy://4939 Martian Saucer Relic
					return "";
				case ItemID.FlyingDutchmanMasterTrophy://4940 Flying Dutchman Relic
					return "";
				case ItemID.MourningWoodMasterTrophy://4941 Mourning Wood Relic
					return "";
				case ItemID.PumpkingMasterTrophy://4942 Pumpking Relic
					return "";
				case ItemID.IceQueenMasterTrophy://4943 Ice Queen Relic
					return "";
				case ItemID.EverscreamMasterTrophy://4944 Everscream Relic
					return "";
				case ItemID.SantankMasterTrophy://4945 Santa-NK1 Relic
					return "";
				case ItemID.DarkMageMasterTrophy://4946 Dark Mage Relic
					return "";
				case ItemID.OgreMasterTrophy://4947 Ogre Relic
					return "";
				case ItemID.BetsyMasterTrophy://4948 Betsy Relic
					return "";
				case ItemID.FairyQueenMasterTrophy://4949 Empress of Light Relic
					return "";
				case ItemID.QueenSlimeMasterTrophy://4950 Queen Slime Relic
					return "";
				case ItemID.TeleportationPylonVictory://4951 Universal Pylon
					return "";
				case ItemID.FairyQueenMagicItem://4952 Nightglow
					return "";
				case ItemID.FairyQueenRangedItem://4953 Eventide
					return "";
				case ItemID.LongRainbowTrailWings://4954 Celestial Starboard
					return "";
				case ItemID.RabbitOrder://4955 Rabbit Perch
					return "";
				case ItemID.Zenith://4956 Zenith
					return "";
				case ItemID.QueenSlimeBossBag://4957 Treasure Bag (Queen Slime)
					return "";
				case ItemID.QueenSlimeTrophy://4958 Queen Slime Trophy
					return "";
				case ItemID.QueenSlimeMask://4959 Queen Slime Mask
					return "";
				case ItemID.QueenSlimePetItem://4960 Regal Delicacy
					return "";
				case ItemID.EmpressButterfly://4961 Prismatic Lacewing
					return "";
				case ItemID.AccentSlab://4962 Stone Accent Slab
					return "";
				case ItemID.TruffleWormCage://4963 Truffle Worm Cage
					return "";
				case ItemID.EmpressButterflyJar://4964 Prismatic Lacewing Jar
					return "";
				case ItemID.RockGolemBanner://4965 Rock Golem Banner
					return "";
				case ItemID.BloodMummyBanner://4966 Blood Mummy Banner
					return "";
				case ItemID.SporeSkeletonBanner://4967 Spore Skeleton Banner
					return "";
				case ItemID.SporeBatBanner://4968 Spore Bat Banner
					return "";
				case ItemID.LarvaeAntlionBanner://4969 Antlion Larva Banner
					return "";
				case ItemID.CrimsonBunnyBanner://4970 Vicious Bunny Banner
					return "";
				case ItemID.CrimsonGoldfishBanner://4971 Vicious Goldfish Banner
					return "";
				case ItemID.CrimsonPenguinBanner://4972 Vicious Penguin Banner
					return "";
				case ItemID.BigMimicCorruptionBanner://4973 Corrupt Mimic Banner
					return "";
				case ItemID.BigMimicCrimsonBanner://4974 Crimson Mimic Banner
					return "";
				case ItemID.BigMimicHallowBanner://4975 Hallowed Mimic Banner
					return "";
				case ItemID.MossHornetBanner://4976 Moss Hornet Banner
					return "";
				case ItemID.WanderingEyeBanner://4977 Wandering Eye Banner
					return "";
				case ItemID.CreativeWings://4978 Fledgling Wings
					return "";
				case ItemID.MusicBoxQueenSlime://4979 Music Box (Queen Slime)
					return "";
				case ItemID.QueenSlimeHook://4980 Hook of Dissonance
					return "";
				case ItemID.QueenSlimeMountSaddle://4981 Gelatinous Pillion
					return "";
				case ItemID.CrystalNinjaHelmet://4982 Crystal Assassin Hood
					return "";
				case ItemID.CrystalNinjaChestplate://4983 Crystal Assassin Shirt
					return "";
				case ItemID.CrystalNinjaLeggings://4984 Crystal Assassin Pants
					return "";
				case ItemID.MusicBoxEmpressOfLight://4985 Music Box (Empress Of Light)
					return "";
				case ItemID.GelBalloon://4986 Sparkle Slime Balloon
					return "";
				case ItemID.VolatileGelatin://4987 Volatile Gelatin
					return "";
				case ItemID.QueenSlimeCrystal://4988 Gelatin Crystal
					return "";
				case ItemID.EmpressFlightBooster://4989 Soaring Insignia
					return "";
				case ItemID.MusicBoxDukeFishron://4990 Music Box (Duke Fishron)
					return "";
				case ItemID.MusicBoxMorningRain://4991 Music Box (Morning Rain)
					return "";
				case ItemID.MusicBoxConsoleTitle://4992 Music Box (Alt Title)
					return "";
				case ItemID.ChippysCouch://4993 Chippy's Couch
					return "";
				case ItemID.GraduationCapBlue://4994 Blue Graduation Cap
					return "";
				case ItemID.GraduationCapMaroon://4995 Maroon Graduation Cap
					return "";
				case ItemID.GraduationCapBlack://4996 Black Graduation Cap
					return "";
				case ItemID.GraduationGownBlue://4997 Blue Graduation Gown
					return "";
				case ItemID.GraduationGownMaroon://4998 Maroon Graduation Gown
					return "";
				case ItemID.GraduationGownBlack://4999 Black Graduation Gown
					return "";
				case ItemID.TerrasparkBoots://5000 Terraspark Boots
					return "";
				case ItemID.MoonLordLegs://5001 Moon Lord Legs
					return "";
				case ItemID.OceanCrate://5002 Ocean Crate
					return "";
				case ItemID.OceanCrateHard://5003 Seaside Crate
					return "";
				case ItemID.BadgersHat://5004 Badger's Hat
					return "";
				case ItemID.EmpressBlade://5005 Terraprisma
					return "";
				case ItemID.MusicBoxUndergroundDesert://5006 Music Box (Underground Desert)
					return "";
				case ItemID.DeadMansSweater://5007 Dead Man's Sweater
					return "";
				case ItemID.TeaKettle://5008 Teapot
					return "";
				case ItemID.Teacup://5009 Teacup
					return "";
				case ItemID.TreasureMagnet://5010 Treasure Magnet
					return "";
				case ItemID.Mace://5011 Mace
					return "";
				case ItemID.FlamingMace://5012 Flaming Mace
					return "";
				case ItemID.SleepingIcon://5013 
					return "";
				case ItemID.MusicBoxOWRain://5014 Otherworldly Music Box (Rain)
					return "";
				case ItemID.MusicBoxOWDay://5015 Otherworldly Music Box (Overworld Day)
					return "";
				case ItemID.MusicBoxOWNight://5016 Otherworldly Music Box (Night)
					return "";
				case ItemID.MusicBoxOWUnderground://5017 Otherworldly Music Box (Underground)
					return "";
				case ItemID.MusicBoxOWDesert://5018 Otherworldly Music Box (Desert)
					return "";
				case ItemID.MusicBoxOWOcean://5019 Otherworldly Music Box (Ocean)
					return "";
				case ItemID.MusicBoxOWMushroom://5020 Otherworldly Music Box (Mushrooms)
					return "";
				case ItemID.MusicBoxOWDungeon://5021 Otherworldly Music Box (Dungeon)
					return "";
				case ItemID.MusicBoxOWSpace://5022 Otherworldly Music Box (Space)
					return "";
				case ItemID.MusicBoxOWUnderworld://5023 Otherworldly Music Box (Underworld)
					return "";
				case ItemID.MusicBoxOWSnow://5024 Otherworldly Music Box (Snow)
					return "";
				case ItemID.MusicBoxOWCorruption://5025 Otherworldly Music Box (Corruption)
					return "";
				case ItemID.MusicBoxOWUndergroundCorruption://5026 Otherworldly Music Box (Underground Corruption)
					return "";
				case ItemID.MusicBoxOWCrimson://5027 Otherworldly Music Box (Crimson)
					return "";
				case ItemID.MusicBoxOWUndergroundCrimson://5028 Otherworldly Music Box (Underground Crimson)
					return "";
				case ItemID.MusicBoxOWUndergroundSnow://5029 Otherworldly Music Box (Ice)
					return "";
				case ItemID.MusicBoxOWUndergroundHallow://5030 Otherworldly Music Box (Underground Hallow)
					return "";
				case ItemID.MusicBoxOWBloodMoon://5031 Otherworldly Music Box (Eerie)
					return "";
				case ItemID.MusicBoxOWBoss2://5032 Otherworldly Music Box (Boss 2)
					return "";
				case ItemID.MusicBoxOWBoss1://5033 Otherworldly Music Box (Boss 1)
					return "";
				case ItemID.MusicBoxOWInvasion://5034 Otherworldly Music Box (Invasion)
					return "";
				case ItemID.MusicBoxOWTowers://5035 Otherworldly Music Box (The Towers)
					return "";
				case ItemID.MusicBoxOWMoonLord://5036 Otherworldly Music Box (Lunar Boss)
					return "";
				case ItemID.MusicBoxOWPlantera://5037 Otherworldly Music Box (Plantera)
					return "";
				case ItemID.MusicBoxOWJungle://5038 Otherworldly Music Box (Jungle)
					return "";
				case ItemID.MusicBoxOWWallOfFlesh://5039 Otherworldly Music Box (Wall of Flesh)
					return "";
				case ItemID.MusicBoxOWHallow://5040 Otherworldly Music Box (Hallow)
					return "";
				case ItemID.MilkCarton://5041 Carton of Milk
					return "";
				case ItemID.CoffeeCup://5042 Coffee
					return "";
				case ItemID.TorchGodsFavor://5043 Torch God's Favor
					return "";
				case ItemID.MusicBoxCredits://5044 Music Box (Journey's End)
					return "";
				case ItemID.PlaguebringerHelmet://5045 Plaguebringer's Skull
					return "";
				case ItemID.PlaguebringerChestplate://5046 Plaguebringer's Cloak
					return "";
				case ItemID.PlaguebringerGreaves://5047 Plaguebringer's Treads
					return "";
				case ItemID.RoninHat://5048 Wandering Jingasa
					return "";
				case ItemID.RoninShirt://5049 Wandering Yukata
					return "";
				case ItemID.RoninPants://5050 Wandering Geta
					return "";
				case ItemID.TimelessTravelerHood://5051 Timeless Traveler's Hood
					return "";
				case ItemID.TimelessTravelerRobe://5052 Timeless Traveler's Cloak
					return "";
				case ItemID.TimelessTravelerBottom://5053 Timeless Traveler's Footwear
					return "";
				case ItemID.FloretProtectorHelmet://5054 Floret Protector Helmet
					return "";
				case ItemID.FloretProtectorChestplate://5055 Floret Protector Shirt
					return "";
				case ItemID.FloretProtectorLegs://5056 Floret Protector Pants
					return "";
				case ItemID.CapricornMask://5057 Capricorn Helmet
					return "";
				case ItemID.CapricornChestplate://5058 Capricorn Chestplate
					return "";
				case ItemID.CapricornLegs://5059 Capricorn Hooves
					return "";
				case ItemID.CapricornTail://5060 Capricorn Tail
					return "";
				case ItemID.TVHeadMask://5061 Video Visage
					return "";
				case ItemID.TVHeadSuit://5062 Lazer Blazer
					return "";
				case ItemID.TVHeadPants://5063 Pinstripe Pants
					return "";
				case ItemID.LavaproofTackleBag://5064 Lavaproof Tackle Bag
					return "";
				case ItemID.PrincessWeapon://5065 Resonance Scepter
					return "";
				case ItemID.BeeHive://5066 Bee Hive
					return "";
				case ItemID.AntlionEggs://5067 Antlion Eggs
					return "";
				case ItemID.FlinxFurCoat://5068 Flinx Fur Coat
					return "";
				case ItemID.FlinxStaff://5069 Flinx Staff
					return "";
				case ItemID.FlinxFur://5070 Flinx Fur
					return "";
				case ItemID.RoyalTiara://5071 Royal Tiara
					return "";
				case ItemID.RoyalDressTop://5072 Royal Blouse
					return "";
				case ItemID.RoyalDressBottom://5073 Royal Dress
					return "";
				case ItemID.BoneWhip://5074 Spinal Tap
					return "";
				case ItemID.RainbowCursor://5075 Rainbow Cursor
					return "";
				case ItemID.RoyalScepter://5076 Royal Scepter
					return "";
				case ItemID.GlassSlipper://5077 Glass Slipper
					return "";
				case ItemID.PrinceUniform://5078 Prince Uniform
					return "";
				case ItemID.PrincePants://5079 Prince Pants
					return "";
				case ItemID.PrinceCape://5080 Prince Cape
					return "";
				case ItemID.PottedCrystalPlantFern://5081 Potted Crystal Fern
					return "";
				case ItemID.PottedCrystalPlantSpiral://5082 Potted Crystal Spiral
					return "";
				case ItemID.PottedCrystalPlantTeardrop://5083 Potted Crystal Teardrop
					return "";
				case ItemID.PottedCrystalPlantTree://5084 Potted Crystal Tree
					return "";
				case ItemID.Princess64://5085 Princess 64
					return "";
				case ItemID.PaintingOfALass://5086 Painting of a Lass
					return "";
				case ItemID.DarkSideHallow://5087 Dark Side of the Hallow
					return "";
				case ItemID.BerniePetItem://5088 Bernie's Button
					return "";
				case ItemID.GlommerPetItem://5089 Glommer's Flower
					return "";
				case ItemID.DeerclopsPetItem://5090 Deerclops Eyeball
					return "";
				case ItemID.PigPetItem://5091 Monster Meat
					return "";
				case ItemID.MonsterLasagna://5092 Monster Lasagna
					return "";
				case ItemID.FroggleBunwich://5093 Froggle Bunwich
					return "";
				case ItemID.TentacleSpike://5094 Tentacle Spike
					return "";
				case ItemID.LucyTheAxe://5095 Lucy the Axe
					return "";
				case ItemID.HamBat://5096 Ham Bat
					return "";
				case ItemID.BatBat://5097 Bat Bat
					return "";
				case ItemID.ChesterPetItem://5098 Eye Bone
					return "";
				case ItemID.GarlandHat://5099 Garland
					return "";
				case ItemID.BoneHelm://5100 Bone Helm
					return "";
				case ItemID.Eyebrella://5101 Eyebrella
					return "";
				case ItemID.WilsonShirt://5102 Gentleman's Vest
					return "";
				case ItemID.WilsonPants://5103 Gentleman's Trousers
					return "";
				case ItemID.WilsonBeardShort://5104 Gentleman's Beard
					return "";
				case ItemID.WilsonBeardLong://5105 Gentleman's Long Beard
					return "";
				case ItemID.WilsonBeardMagnificent://5106 Gentleman's Magnificent Beard
					return "";
				case ItemID.Magiluminescence://5107 Magiluminescence
					return "";
				case ItemID.DeerclopsTrophy://5108 Deerclops Trophy
					return "";
				case ItemID.DeerclopsMask://5109 Deerclops Mask
					return "";
				case ItemID.DeerclopsMasterTrophy://5110 Deerclops Relic
					return "";
				case ItemID.DeerclopsBossBag://5111 Treasure Bag (Deerclops)
					return "";
				case ItemID.MusicBoxDeerclops://5112 Music Box (Deerclops)
					return "";
				case ItemID.DontStarveShaderItem://5113 Radio Thing
					return "";
				case ItemID.AbigailsFlower://5114 Abigail's Flower
					return "";
				case ItemID.WillowShirt://5115 Firestarter's Sweater
					return "";
				case ItemID.WillowSkirt://5116 Firestarter's Skirt
					return "";
				case ItemID.PewMaticHorn://5117 Pew-matic Horn
					return "";
				case ItemID.WeatherPain://5118 Weather Pain
					return "";
				case ItemID.HoundiusShootius://5119 Houndius Shootius
					return "";
				case ItemID.DeerThing://5120 Deer Thing
					return "";
				case ItemID.PaintingWilson://5121 The Gentleman Scientist
					return "";
				case ItemID.PaintingWillow://5122 The Firestarter
					return "";
				case ItemID.PaintingWendy://5123 The Bereaved
					return "";
				case ItemID.PaintingWolfgang://5124 The Strongman
					return "";
				case ItemID.FartMinecart://5125 Fart Kart
					return "";
				case ItemID.HandOfCreation://5126 Hand Of Creation
					return "";
				case ItemID.VioletMoss://5127 Neon Moss
					return "";
				case ItemID.RainbowMoss://5128 Helium Moss
					return "";
				case ItemID.Flymeal://5129 Flymeal
					return "";
				case ItemID.WolfMountItem://5130 Lilith's Necklace
					return "";
				case ItemID.ResplendentDessert://5131 Resplendent Dessert
					return "";
				case ItemID.Stinkbug://5132 Stinkbug
					return "";
				case ItemID.StinkbugCage://5133 Stinkbug Cage
					return "";
				case ItemID.Clentaminator2://5134 Terraformer
					return "";
				case ItemID.VenomDartTrap://5135 Venom Dart Trap
					return "";
				case ItemID.VulkelfEar://5136 Vulkelf Ears
					return "";
				case ItemID.StinkbugHousingBlocker://5137 Stinkbug Blocker
					return "";
				case ItemID.StinkbugHousingBlockerEcho://5138 Ghostly Stinkbug Blocker
					return "";
				case ItemID.FishingBobber://5139 Fishing Bobber
					return "";
				case ItemID.FishingBobberGlowingStar://5140 Glowing Fishing Bobber
					return "";
				case ItemID.FishingBobberGlowingLava://5141 Lava Moss Fishing Bobber
					return "";
				case ItemID.FishingBobberGlowingKrypton://5142 Krypton Moss Fishing Bobber
					return "";
				case ItemID.FishingBobberGlowingXenon://5143 Xenon Moss Fishing Bobber
					return "";
				case ItemID.FishingBobberGlowingArgon://5144 Argon Moss Fishing Bobber
					return "";
				case ItemID.FishingBobberGlowingViolet://5145 Neon Moss Fishing Bobber
					return "";
				case ItemID.FishingBobberGlowingRainbow://5146 Helium Moss Fishing Bobber
					return "";
				case ItemID.WandofFrosting://5147 Wand of Frosting
					return "";
				case ItemID.CoralBathtub://5148 Reef Bathtub
					return "";
				case ItemID.CoralBed://5149 Reef Bed
					return "";
				case ItemID.CoralBookcase://5150 Reef Bookcase
					return "";
				case ItemID.CoralDresser://5151 Reef Dresser
					return "";
				case ItemID.CoralCandelabra://5152 Reef Candelabra
					return "";
				case ItemID.CoralCandle://5153 Reef Candle
					return "";
				case ItemID.CoralChair://5154 Reef Chair
					return "";
				case ItemID.CoralChandelier://5155 Reef Chandelier
					return "";
				case ItemID.CoralChest://5156 Reef Chest
					return "";
				case ItemID.CoralClock://5157 Reef Clock
					return "";
				case ItemID.CoralDoor://5158 Reef Door
					return "";
				case ItemID.CoralLamp://5159 Reef Lamp
					return "";
				case ItemID.CoralLantern://5160 Reef Lantern
					return "";
				case ItemID.CoralPiano://5161 Reef Piano
					return "";
				case ItemID.CoralPlatform://5162 Reef Platform
					return "";
				case ItemID.CoralSink://5163 Reef Sink
					return "";
				case ItemID.CoralSofa://5164 Reef Sofa
					return "";
				case ItemID.CoralTable://5165 Reef Table
					return "";
				case ItemID.CoralWorkbench://5166 Reef Work Bench
					return "";
				case ItemID.Fake_CoralChest://5167 Trapped Reef Chest
					return "";
				case ItemID.CoralToilet://5168 Reef Toilet
					return "";
				case ItemID.BalloonBathtub://5169 Balloon Bathtub
					return "";
				case ItemID.BalloonBed://5170 Balloon Bed
					return "";
				case ItemID.BalloonBookcase://5171 Balloon Bookcase
					return "";
				case ItemID.BalloonDresser://5172 Balloon Dresser
					return "";
				case ItemID.BalloonCandelabra://5173 Balloon Candelabra
					return "";
				case ItemID.BalloonCandle://5174 Balloon Candle
					return "";
				case ItemID.BalloonChair://5175 Balloon Chair
					return "";
				case ItemID.BalloonChandelier://5176 Balloon Chandelier
					return "";
				case ItemID.BalloonChest://5177 Balloon Chest
					return "";
				case ItemID.BalloonClock://5178 Balloon Clock
					return "";
				case ItemID.BalloonDoor://5179 Balloon Door
					return "";
				case ItemID.BalloonLamp://5180 Balloon Lamp
					return "";
				case ItemID.BalloonLantern://5181 Balloon Lantern
					return "";
				case ItemID.BalloonPiano://5182 Balloon Piano
					return "";
				case ItemID.BalloonPlatform://5183 Balloon Platform
					return "";
				case ItemID.BalloonSink://5184 Balloon Sink
					return "";
				case ItemID.BalloonSofa://5185 Balloon Sofa
					return "";
				case ItemID.BalloonTable://5186 Balloon Table
					return "";
				case ItemID.BalloonWorkbench://5187 Balloon Work Bench
					return "";
				case ItemID.Fake_BalloonChest://5188 Trapped Balloon Chest
					return "";
				case ItemID.BalloonToilet://5189 Balloon Toilet
					return "";
				case ItemID.AshWoodBathtub://5190 Ash Wood Bathtub
					return "";
				case ItemID.AshWoodBed://5191 Ash Wood Bed
					return "";
				case ItemID.AshWoodBookcase://5192 Ash Wood Bookcase
					return "";
				case ItemID.AshWoodDresser://5193 Ash Wood Dresser
					return "";
				case ItemID.AshWoodCandelabra://5194 Ash Wood Candelabra
					return "";
				case ItemID.AshWoodCandle://5195 Ash Wood Candle
					return "";
				case ItemID.AshWoodChair://5196 Ash Wood Chair
					return "";
				case ItemID.AshWoodChandelier://5197 Ash Wood Chandelier
					return "";
				case ItemID.AshWoodChest://5198 Ash Wood Chest
					return "";
				case ItemID.AshWoodClock://5199 Ash Wood Clock
					return "";
				case ItemID.AshWoodDoor://5200 Ash Wood Door
					return "";
				case ItemID.AshWoodLamp://5201 Ash Wood Lamp
					return "";
				case ItemID.AshWoodLantern://5202 Ash Wood Lantern
					return "";
				case ItemID.AshWoodPiano://5203 Ash Wood Piano
					return "";
				case ItemID.AshWoodPlatform://5204 Ash Wood Platform
					return "";
				case ItemID.AshWoodSink://5205 Ash Wood Sink
					return "";
				case ItemID.AshWoodSofa://5206 Ash Wood Sofa
					return "";
				case ItemID.AshWoodTable://5207 Ash Wood Table
					return "";
				case ItemID.AshWoodWorkbench://5208 Ash Wood Work Bench
					return "";
				case ItemID.Fake_AshWoodChest://5209 Trapped Ash Wood Chest
					return "";
				case ItemID.AshWoodToilet://5210 Ash Wood Toilet
					return "";
				case ItemID.BiomeSightPotion://5211 Biome Sight Potion
					return "";
				case ItemID.ScarletMacaw://5212 Scarlet Macaw
					return "";
				case ItemID.ScarletMacawCage://5213 Scarlet Macaw Cage
					return "";
				case ItemID.AshGrassSeeds://5214 Ash Grass Seeds
					return "";
				case ItemID.AshWood://5215 Ash Wood
					return "";
				case ItemID.AshWoodWall://5216 Ash Wood Wall
					return "";
				case ItemID.AshWoodFence://5217 Ash Wood Fence
					return "";
				case ItemID.Outcast://5218 Outcast
					return "";
				case ItemID.FairyGuides://5219 Fairy Guides
					return "";
				case ItemID.AHorribleNightforAlchemy://5220 A Horrible Night for Alchemy
					return "";
				case ItemID.MorningHunt://5221 Morning Hunt
					return "";
				case ItemID.SuspiciouslySparkly://5222 Suspiciously Sparkly
					return "";
				case ItemID.Requiem://5223 Requiem
					return "";
				case ItemID.CatSword://5224 Cat Sword
					return "";
				case ItemID.KargohsSummon://5225 Kargoh's Summon
					return "";
				case ItemID.HighPitch://5226 High Pitch
					return "";
				case ItemID.AMachineforTerrarians://5227 A Machine for Terrarians
					return "";
				case ItemID.TerraBladeChronicles://5228 Terra Blade Chronicles
					return "";
				case ItemID.BennyWarhol://5229 Benny Warhol
					return "";
				case ItemID.LizardKing://5230 Lizard King
					return "";
				case ItemID.MySon://5231 My Son
					return "";
				case ItemID.Duality://5232 Duality
					return "";
				case ItemID.ParsecPals://5233 Parsec Pals
					return "";
				case ItemID.RemnantsofDevotion://5234 Remnants of Devotion
					return "";
				case ItemID.NotSoLostInParadise://5235 Not So Lost In Paradise
					return "";
				case ItemID.OcularResonance://5236 Ocular Resonance
					return "";
				case ItemID.WingsofEvil://5237 Wings of Evil
					return "";
				case ItemID.Constellation://5238 Constellation
					return "";
				case ItemID.Eyezorhead://5239 Eyezorhead
					return "";
				case ItemID.DreadoftheRedSea://5240 Dread of the Red Sea
					return "";
				case ItemID.DoNotEattheVileMushroom://5241 Do Not Eat the Vile Mushroom!
					return "";
				case ItemID.YuumaTheBlueTiger://5242 Yuuma, The Blue Tiger
					return "";
				case ItemID.MoonmanandCompany://5243 Moonman & Company
					return "";
				case ItemID.SunshineofIsrapony://5244 Sunshine of Israpony
					return "";
				case ItemID.Purity://5245 Purity
					return "";
				case ItemID.SufficientlyAdvanced://5246 Sufficiently Advanced
					return "";
				case ItemID.StrangeGrowth://5247 Strange Growth
					return "";
				case ItemID.HappyLittleTree://5248 Happy Little Tree
					return "";
				case ItemID.StrangeDeadFellows://5249 Strange Dead Fellows
					return "";
				case ItemID.Secrets://5250 Secrets
					return "";
				case ItemID.Thunderbolt://5251 Thunderbolt
					return "";
				case ItemID.Crustography://5252 Crustography
					return "";
				case ItemID.TheWerewolf://5253 The Werewolf
					return "";
				case ItemID.BlessingfromTheHeavens://5254 Blessing from the Heavens
					return "";
				case ItemID.LoveisintheTrashSlot://5255 Love is in the Trash Slot
					return "";
				case ItemID.Fangs://5256 Fangs
					return "";
				case ItemID.HailtotheKing://5257 Hail to the King
					return "";
				case ItemID.SeeTheWorldForWhatItIs://5258 See The World For What It Is
					return "";
				case ItemID.WhatLurksBelow://5259 What Lurks Below
					return "";
				case ItemID.ThisIsGettingOutOfHand://5260 This Is Getting Out Of Hand
					return "";
				case ItemID.Buddies://5261 Buddies
					return "";
				case ItemID.MidnightSun://5262 Midnight Sun
					return "";
				case ItemID.CouchGag://5263 Couch Gag
					return "";
				case ItemID.SilentFish://5264 Silent Fish
					return "";
				case ItemID.TheDuke://5265 The Duke
					return "";
				case ItemID.RoyalRomance://5266 Royal Romance
					return "";
				case ItemID.Bioluminescence://5267 Bioluminescence
					return "";
				case ItemID.Wildflowers://5268 Wildflowers
					return "";
				case ItemID.VikingVoyage://5269 Viking Voyage
					return "";
				case ItemID.Bifrost://5270 Bifrost
					return "";
				case ItemID.Heartlands://5271 Heartlands
					return "";
				case ItemID.ForestTroll://5272 Forest Troll
					return "";
				case ItemID.AuroraBorealis://5273 Aurora Borealis
					return "";
				case ItemID.LadyOfTheLake://5274 Lady Of The Lake
					return "";
				case ItemID.JojaCola://5275 Joja Cola
					return "";
				case ItemID.JunimoPetItem://5276 Stardrop
					return "";
				case ItemID.SpicyPepper://5277 Spicy Pepper
					return "";
				case ItemID.Pomegranate://5278 Pomegranate
					return "";
				case ItemID.AshWoodHelmet://5279 Ash Wood Helmet
					return "";
				case ItemID.AshWoodBreastplate://5280 Ash Wood Breastplate
					return "";
				case ItemID.AshWoodGreaves://5281 Ash Wood Greaves
					return "";
				case ItemID.AshWoodBow://5282 Ash Wood Bow
					return "";
				case ItemID.AshWoodHammer://5283 Ash Wood Hammer
					return "";
				case ItemID.AshWoodSword://5284 Ash Wood Sword
					return "";
				case ItemID.MoonGlobe://5285 Moon Globe
					return "";
				case ItemID.RepairedLifeCrystal://5286 Repaired Life Crystal
					return "";
				case ItemID.RepairedManaCrystal://5287 Repaired Mana Crystal
					return "";
				case ItemID.TerraFartMinecart://5288 Terra Fart Kart
					return "";
				case ItemID.MinecartPowerup://5289 Minecart Upgrade Kit
					return "";
				case ItemID.JimsCap://5290 Jim's Cap
					return "";
				case ItemID.EchoWall://5291 Echo Wall
					return "";
				case ItemID.EchoPlatform://5292 Echo Platform
					return "";
				case ItemID.MushroomTorch://5293 Mushroom Torch
					return "";
				case ItemID.HiveFive://5294 Hive-Five
					return "";
				case ItemID.AcornAxe://5295 Axe of Regrowth
					return "";
				case ItemID.ChlorophyteExtractinator://5296 Chlorophyte Extractinator
					return "";
				case ItemID.BlueEgg://5297 Blue Chicken Egg
					return "";
				case ItemID.Trimarang://5298 Trimarang
					return "";
				case ItemID.MushroomCampfire://5299 Mushroom Campfire
					return "";
				case ItemID.BlueMacaw://5300 Blue Macaw
					return "";
				case ItemID.BlueMacawCage://5301 Blue Macaw Cage
					return "";
				case ItemID.BottomlessHoneyBucket://5302 Bottomless Honey Bucket
					return "";
				case ItemID.HoneyAbsorbantSponge://5303 Honey Absorbant Sponge
					return "";
				case ItemID.UltraAbsorbantSponge://5304 Ultra Absorbant Sponge
					return "";
				case ItemID.GoblorcEar://5305 Goblorc Ears
					return "";
				case ItemID.ReefBlock://5306 Reef Block
					return "";
				case ItemID.ReefWall://5307 Reef Wall
					return "";
				case ItemID.PlacePainting://5308 r/Terraria
					return "";
				case ItemID.DontHurtNatureBook://5309 Guide to Environmental Preservation
					return "";
				case ItemID.PrincessStyle://5310 Princess Style
					return "";
				case ItemID.Toucan://5311 Toucan
					return "";
				case ItemID.YellowCockatiel://5312 Yellow Cockatiel
					return "";
				case ItemID.GrayCockatiel://5313 Gray Cockatiel
					return "";
				case ItemID.ToucanCage://5314 Toucan Cage
					return "";
				case ItemID.YellowCockatielCage://5315 Yellow Cockatiel Cage
					return "";
				case ItemID.GrayCockatielCage://5316 Gray Cockatiel Cage
					return "";
				case ItemID.MacawStatue://5317 Macaw Statue
					return "";
				case ItemID.ToucanStatue://5318 Toucan Statue
					return "";
				case ItemID.CockatielStatue://5319 Cockatiel Statue
					return "";
				case ItemID.PlaceableHealingPotion://5320 Decorative Healing Potion
					return "";
				case ItemID.PlaceableManaPotion://5321 Decorative Mana Potion
					return "";
				case ItemID.ShadowCandle://5322 Shadow Candle
					return "";
				case ItemID.DontHurtComboBook://5323 Guide to Peaceful Coexistence
					return "";
				case ItemID.RubblemakerSmall://5324 Rubblemaker (Small)
					return "";
				case ItemID.ClosedVoidBag://5325 Closed Void Bag
					return "";
				case ItemID.ArtisanLoaf://5326 Artisan Loaf
					return "";
				case ItemID.TNTBarrel://5327 TNT Barrel
					return "";
				case ItemID.ChestLock://5328 Chest Lock
					return "";
				case ItemID.RubblemakerMedium://5329 Rubblemaker (Medium)
					return "";
				case ItemID.RubblemakerLarge://5330 Rubblemaker (Large)
					return "";
				case ItemID.HorseshoeBundle://5331 Bundle of Horseshoe Balloons
					return "";
				case ItemID.SpiffoPlush://5332 Spiffo Plush
					return "";
				case ItemID.GlowTulip://5333 Glow Tulip
					return "";
				case ItemID.MechdusaSummon://5334 Ocram's Razor
					return "";
				case ItemID.RodOfHarmony://5335 Rod of Harmony
					return "";
				case ItemID.CombatBookVolumeTwo://5336 Advanced Combat Techniques: Volume Two
					return "";
				case ItemID.AegisCrystal://5337 Vital Crystal
					return "";
				case ItemID.AegisFruit://5338 Aegis Fruit
					return "";
				case ItemID.ArcaneCrystal://5339 Arcane Crystal
					return "";
				case ItemID.GalaxyPearl://5340 Galaxy Pearl
					return "";
				case ItemID.GummyWorm://5341 Gummy Worm
					return "";
				case ItemID.Ambrosia://5342 Ambrosia
					return "";
				case ItemID.PeddlersSatchel://5343 Peddler's Satchel
					return "";
				case ItemID.EchoCoating://5344 Echo Coating
					return "";
				case ItemID.EchoMonolith://5345 Echo Chamber
					return "";
				case ItemID.GasTrap://5346 Gas Trap
					return "";
				case ItemID.ShimmerMonolith://5347 Aether Monolith
					return "";
				case ItemID.ShimmerArrow://5348 Shimmer Arrow
					return "";
				case ItemID.ShimmerBlock://5349 Aetherium Block
					return "";
				case ItemID.Shimmerfly://5350 Faeling
					return "";
				case ItemID.ShimmerflyinaBottle://5351 Faeling in a Bottle
					return "";
				case ItemID.ShimmerSlimeBanner://5352 Shimmer Slime Banner
					return "";
				case ItemID.ShimmerTorch://5353 Aether Torch
					return "";
				case ItemID.ReflectiveShades://5354 Reflective Shades
					return "";
				case ItemID.ShimmerCloak://5355 Chromatic Cloak
					return "";
				case ItemID.UsedGasTrap://5356 Used Gas Trap
					return "";
				case ItemID.ShimmerCampfire://5357 Aether Campfire
					return "";
				case ItemID.Shellphone://5358 Shellphone (Home)
					return "";
				case ItemID.ShellphoneSpawn://5359 Shellphone (Spawn)
					return "";
				case ItemID.ShellphoneOcean://5360 Shellphone (Ocean)
					return "";
				case ItemID.ShellphoneHell://5361 Shellphone (Underworld)
					return "";
				case ItemID.MusicBoxShimmer://5362 Music Box (Aether)
					return "";
				case ItemID.SpiderWallUnsafe://5363 Infested Spider Wall
					return "";
				case ItemID.BottomlessShimmerBucket://5364 Bottomless Shimmer Bucket
					return "";
				case ItemID.BlueBrickWallUnsafe://5365 Cursed Blue Brick Wall
					return "";
				case ItemID.BlueSlabWallUnsafe://5366 Cursed Blue Slab Wall
					return "";
				case ItemID.BlueTiledWallUnsafe://5367 Cursed Blue Tiled Wall
					return "";
				case ItemID.PinkBrickWallUnsafe://5368 Cursed Pink Brick Wall
					return "";
				case ItemID.PinkSlabWallUnsafe://5369 Cursed Pink Slab Wall
					return "";
				case ItemID.PinkTiledWallUnsafe://5370 Cursed Pink Tiled Wall
					return "";
				case ItemID.GreenBrickWallUnsafe://5371 Cursed Green Brick Wall
					return "";
				case ItemID.GreenSlabWallUnsafe://5372 Cursed Green Slab Wall
					return "";
				case ItemID.GreenTiledWallUnsafe://5373 Cursed Green Tiled Wall
					return "";
				case ItemID.SandstoneWallUnsafe://5374 Treacherous Sandstone Wall
					return "";
				case ItemID.HardenedSandWallUnsafe://5375 Treacherous Hardened Sand Wall
					return "";
				case ItemID.LihzahrdWallUnsafe://5376 Forbidden Lihzahrd Brick Wall
					return "";
				case ItemID.SpelunkerFlare://5377 Spelunker Flare
					return "";
				case ItemID.CursedFlare://5378 Cursed Flare
					return "";
				case ItemID.RainbowFlare://5379 Rainbow Flare
					return "";
				case ItemID.ShimmerFlare://5380 Shimmer Flare
					return "";
				case ItemID.Moondial://5381 Enchanted Moondial
					return "";
				case ItemID.WaffleIron://5382 Waffle's Iron
					return "";
				case ItemID.BouncyBoulder://5383 Bouncy Boulder
					return "";
				case ItemID.LifeCrystalBoulder://5384 Life Crystal Boulder
					return "";
				case ItemID.DizzyHat://5385 Dizzy's Rare Gecko Chester
					return "";
				case ItemID.LincolnsHoodie://5386 Raynebro's Hoodie
					return "";
				case ItemID.LincolnsPants://5387 Raynebro's Pants
					return "";
				case ItemID.SunOrnament://5388 Eye of the Sun
					return "";
				case ItemID.HoplitePizza://5389 Cheesy Pizza Poster
					return "";
				case ItemID.LincolnsHood://5390 Raynebro's Hood
					return "";
				case ItemID.UncumberingStone://5391 Uncumbering Stone
					return "";
				case ItemID.SandSolution://5392 Yellow Solution
					return "";
				case ItemID.SnowSolution://5393 White Solution
					return "";
				case ItemID.DirtSolution://5394 Brown Solution
					return "";
				case ItemID.PoopBlock://5395 Poo
					return "";
				case ItemID.PoopWall://5396 Poo Wall
					return "";
				case ItemID.ShimmerWall://5397 Aetherium Wall
					return "";
				case ItemID.ShimmerBrick://5398 Aetherium Brick
					return "";
				case ItemID.ShimmerBrickWall://5399 Aetherium Brick Wall
					return "";
				case ItemID.DirtiestBlock://5400 The Dirtiest Block
					return "";
				case ItemID.LunarRustBrick://5401 Lunar Rust Brick
					return "";
				case ItemID.DarkCelestialBrick://5402 Dark Celestial Brick
					return "";
				case ItemID.AstraBrick://5403 Astra Brick
					return "";
				case ItemID.CosmicEmberBrick://5404 Cosmic Ember Brick
					return "";
				case ItemID.CryocoreBrick://5405 Cryocore Brick
					return "";
				case ItemID.MercuryBrick://5406 Mercury Brick
					return "";
				case ItemID.StarRoyaleBrick://5407 Star Royale Brick
					return "";
				case ItemID.HeavenforgeBrick://5408 Heavenforge Brick
					return "";
				case ItemID.LunarRustBrickWall://5409 Lunar Rust Brick Wall
					return "";
				case ItemID.DarkCelestialBrickWall://5410 Dark Celestial Brick Wall
					return "";
				case ItemID.AstraBrickWall://5411 Astra Brick Wall
					return "";
				case ItemID.CosmicEmberBrickWall://5412 Cosmic Ember Brick Wall
					return "";
				case ItemID.CryocoreBrickWall://5413 Cryocore Brick Wall
					return "";
				case ItemID.MercuryBrickWall://5414 Mercury Brick Wall
					return "";
				case ItemID.StarRoyaleBrickWall://5415 Star Royale Brick Wall
					return "";
				case ItemID.HeavenforgeBrickWall://5416 Heavenforge Brick Wall
					return "";
				case ItemID.AncientBlueDungeonBrick://5417 Ancient Blue Brick
					return "";
				case ItemID.AncientBlueDungeonBrickWall://5418 Ancient Blue Brick Wall
					return "";
				case ItemID.AncientGreenDungeonBrick://5419 Ancient Green Brick
					return "";
				case ItemID.AncientGreenDungeonBrickWall://5420 Ancient Green Brick Wall
					return "";
				case ItemID.AncientPinkDungeonBrick://5421 Ancient Pink Brick
					return "";
				case ItemID.AncientPinkDungeonBrickWall://5422 Ancient Pink Brick Wall
					return "";
				case ItemID.AncientGoldBrick://5423 Ancient Gold Brick
					return "";
				case ItemID.AncientGoldBrickWall://5424 Ancient Gold Brick Wall
					return "";
				case ItemID.AncientSilverBrick://5425 Ancient Silver Brick
					return "";
				case ItemID.AncientSilverBrickWall://5426 Ancient Silver Brick Wall
					return "";
				case ItemID.AncientCopperBrick://5427 Ancient Copper Brick
					return "";
				case ItemID.AncientCopperBrickWall://5428 Ancient Copper Brick Wall
					return "";
				case ItemID.AncientCobaltBrick://5429 Ancient Cobalt Brick
					return "";
				case ItemID.AncientCobaltBrickWall://5430 Ancient Cobalt Brick Wall
					return "";
				case ItemID.AncientMythrilBrick://5431 Ancient Mythril Brick
					return "";
				case ItemID.AncientMythrilBrickWall://5432 Ancient Mythril Brick Wall
					return "";
				case ItemID.AncientObsidianBrick://5433 Ancient Obsidian Brick
					return "";
				case ItemID.AncientObsidianBrickWall://5434 Ancient Obsidian Brick Wall
					return "";
				case ItemID.AncientHellstoneBrick://5435 Ancient Hellstone Brick
					return "";
				case ItemID.AncientHellstoneBrickWall://5436 Ancient Hellstone Brick Wall
					return "";
				case ItemID.ShellphoneDummy://5437 Shellphone
					return "";
				case ItemID.Fertilizer://5438 Fertilizer
					return "";
				case ItemID.LavaMossBlock://5439 Lava Moss Brick
					return "";
				case ItemID.ArgonMossBlock://5440 Argon Moss Brick
					return "";
				case ItemID.KryptonMossBlock://5441 Krypton Moss Brick
					return "";
				case ItemID.XenonMossBlock://5442 Xenon Moss Brick
					return "";
				case ItemID.VioletMossBlock://5443 Neon Moss Brick
					return "";
				case ItemID.RainbowMossBlock://5444 Helium Moss Brick
					return "";
				case ItemID.LavaMossBlockWall://5445 Lava Moss Brick Wall
					return "";
				case ItemID.ArgonMossBlockWall://5446 Argon Moss Brick Wall
					return "";
				case ItemID.KryptonMossBlockWall://5447 Krypton Moss Brick Wall
					return "";
				case ItemID.XenonMossBlockWall://5448 Xenon Moss Brick Wall
					return "";
				case ItemID.VioletMossBlockWall://5449 Neon Moss Brick Wall
					return "";
				case ItemID.RainbowMossBlockWall://5450 Helium Moss Brick Wall
					return "";
				case ItemID.JimsDrone://5451 Kwad Racer Drone
					return "";
				case ItemID.JimsDroneVisor://5452 FPV Goggles
					return "";
				case ItemID.DontHurtCrittersBookInactive://5453 Guide to Critter Companionship (Inactive)
					return "";
				case ItemID.DontHurtNatureBookInactive://5454 Guide to Environmental Preservation (Inactive)
					return "";
				case ItemID.DontHurtComboBookInactive://5455 Guide to Peaceful Coexistence (Inactive)
					return "";
				default:
					return "";
			}
		}
		//public static string GetModItemPNGLink(this string modItemFullName) {
		//	switch (modItemFullName) {
		//		case $"{AndroMod.calamityModName}/WulfrumMetalScrap":
		//			return "https://calamitymod.wiki.gg/wiki/Wulfrum_Metal_Scrap#/media/File:Wulfrum_Metal_Scrap.png";
		//		case $"{AndroMod.calamityModName}/EnergyCore":
		//			return "https://calamitymod.wiki.gg/wiki/Energy_Core#/media/File:Energy_Core.png";
		//		case $"{AndroMod.calamityModName}/SeaPrism":
		//			return "https://calamitymod.wiki.gg/wiki/Sea_Prism#/media/File:Sea_Prism.png";
		//		case $"{AndroMod.calamityModName}/":
		//			return "";
		//		default:
		//		return "";
		//	}
		//}
		public static string GetModItemLink(this string modItemFullName) {
			switch (modItemFullName) {
				case $"{AndroMod.calamityModName}/WulfrumMetalScrap":
					return "https://calamitymod.wiki.gg/wiki/Wulfrum_Metal_Scrap";
				case $"{AndroMod.calamityModName}/EnergyCore":
					return "https://calamitymod.wiki.gg/wiki/Energy_Core";
				case $"{AndroMod.calamityModName}/SeaPrism":
					return "https://calamitymod.wiki.gg/wiki/Sea_Prism";
				case $"{AndroMod.thoriumModName}/SmoothCoal":
					return "https://thoriummod.wiki.gg/wiki/Smooth_Coal";
				case $"{AndroMod.thoriumModName}/LifeQuartz":
					return "https://thoriummod.wiki.gg/wiki/Life_Quartz";
				case $"{AndroMod.thoriumModName}/Blood":
					return "https://thoriummod.wiki.gg/wiki/Blood";
				case $"{AndroMod.thoriumModName}/ThoriumOre":
					return "https://thoriummod.wiki.gg/wiki/Thorium_Ore";
				case $"{AndroMod.secretsOfTheShadowsName}/DissolvingEarth":
					return "https://terrariamods.fandom.com/wiki/Secrets_Of_The_Shadows/Dissolving_Earth";
				case $"{AndroMod.secretsOfTheShadowsName}/FragmentOfEarth":
					return "https://terrariamods.fandom.com/wiki/Secrets_Of_The_Shadows/Fragment_of_Earth";
				case $"{AndroMod.spookyModName}/RottenSeed":
					return "https://terrariamods.wiki.gg/wiki/Spooky_Mod";
				case $"{AndroMod.spookyModName}/SpookySeedsOrange":
					return "https://terrariamods.wiki.gg/wiki/Spooky_Mod";
				case $"{AndroMod.spookyModName}/SpookySeedsGreen":
					return "https://terrariamods.wiki.gg/wiki/Spooky_Mod";
				case $"{AndroMod.spookyModName}/RottenChunk":
					return "https://terrariamods.wiki.gg/wiki/Spooky_Mod";
				case $"{AndroMod.starsAboveModName}/EssenceOfTheAegis":
				case $"{AndroMod.starsAboveModName}/EssenceOfStyle":
					return "https://terrariamods.fandom.com/wiki/The_Stars_Above/Materials/Essences";
				case $"{AndroMod.fargosModName}/SuspiciousEye":
					return "https://terrariamods.wiki.gg/wiki/Fargo%27s_Mod/Eye_That_Could_Be_Seen_As_Suspicious";
				case $"{AndroMod.fargosModName}/SlimyCrown":
					return "https://terrariamods.wiki.gg/wiki/Fargo%27s_Mod/Slimy_Crown";
				case $"{AndroMod.fargosModName}/AutoHouse":
					return "https://terrariamods.wiki.gg/wiki/Fargo%27s_Mod/InstaHouse";
				default:
					return "";
			}
		}
	}
	public static class NPCIDMethods {
		public static string GetNPCPNGLink(this int id) {
			switch (id) {
				case NPCID.BigHornetStingy://-65 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Hornet_5.png/revision/latest?cb=20170422125834&format=original";
				case NPCID.LittleHornetStingy://-64 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Hornet_5.png/revision/latest/scale-to-width-down/40?cb=20170422125834&format=original";
				case NPCID.BigHornetSpikey://-63 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2a/Spikey_Hornet.png/revision/latest?cb=20170422125808&format=original";
				case NPCID.LittleHornetSpikey://-62 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2a/Spikey_Hornet.png/revision/latest/scale-to-width-down/34?cb=20170422125808&format=original";
				case NPCID.BigHornetLeafy://-61 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Leafy_Hornet.png/revision/latest?cb=20170422125730&format=original";
				case NPCID.LittleHornetLeafy://-60 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Leafy_Hornet.png/revision/latest/scale-to-width-down/39?cb=20170422125730&format=original";
				case NPCID.BigHornetHoney://-59 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b7/Honey_Hornet.png/revision/latest?cb=20170422125704&format=original";
				case NPCID.LittleHornetHoney://-58 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b7/Honey_Hornet.png/revision/latest/scale-to-width-down/34?cb=20170422125704&format=original";
				case NPCID.BigHornetFatty://-57 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fc/Fatty_Hornet.png/revision/latest?cb=20170422125636&format=original";
				case NPCID.LittleHornetFatty://-56 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fc/Fatty_Hornet.png/revision/latest/scale-to-width-down/36?cb=20170422125636&format=original";
				case NPCID.BigRainZombie://-55 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Raincoat_Zombie.png/revision/latest?cb=20170805211221&format=original";
				case NPCID.SmallRainZombie://-54 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Raincoat_Zombie.png/revision/latest/scale-to-width-down/31?cb=20170805211221&format=original";
				case NPCID.BigPantlessSkeleton://-53 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/be/Pantless_Skeleton.png/revision/latest?cb=20170422124010&format=original";
				case NPCID.SmallPantlessSkeleton://-52 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/be/Pantless_Skeleton.png/revision/latest/scale-to-width-down/29?cb=20170422124010&format=original";
				case NPCID.BigMisassembledSkeleton://-51 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Misassembled_Skeleton.png/revision/latest?cb=20170422123940&format=original";
				case NPCID.SmallMisassembledSkeleton://-50 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Misassembled_Skeleton.png/revision/latest?cb=20170422123940&format=original";
				case NPCID.BigHeadacheSkeleton://-49 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/62/Headache_Skeleton.png/revision/latest?cb=20170422123913&format=original";
				case NPCID.SmallHeadacheSkeleton://-48 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/62/Headache_Skeleton.png/revision/latest/scale-to-width-down/32?cb=20170422123913&format=original";
				case NPCID.BigSkeleton://-47 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/23/Skeleton.png/revision/latest?cb=20170420012637&format=original";
				case NPCID.SmallSkeleton://-46 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/23/Skeleton.png/revision/latest/scale-to-width-down/27?cb=20170420012637&format=original";
				case NPCID.BigFemaleZombie://-45 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a1/Female_Zombie.png/revision/latest?cb=20170422121903&format=original";
				case NPCID.SmallFemaleZombie://-44 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a1/Female_Zombie.png/revision/latest/scale-to-width-down/30?cb=20170422121903&format=original";
				case NPCID.DemonEye2://-43 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9d/Demon_Eye.png/revision/latest?cb=20170420003551&format=original";
				case NPCID.PurpleEye2://-42 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Purple_Eye.png/revision/latest?cb=20170422122804&format=original";
				case NPCID.GreenEye2://-41 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2e/Green_Eye.png/revision/latest/scale-to-width-down/31?cb=20170422122705&format=original";
				case NPCID.DialatedEye2://-40 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/af/Dilated_Eye.png/revision/latest/scale-to-width-down/32?cb=20170422122641&format=original";
				case NPCID.SleepyEye2://-39 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/29/Sleepy_Eye.png/revision/latest?cb=20170422122738&format=original";
				case NPCID.CataractEye2://-38 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/de/Cataract_Eye.png/revision/latest?cb=20170422122610&format=original";
				case NPCID.BigTwiggyZombie://-37 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Twiggy_Zombie.png/revision/latest?cb=20170422121749&format=original";
				case NPCID.SmallTwiggyZombie://-36 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Twiggy_Zombie.png/revision/latest/scale-to-width-down/31?cb=20170422121749&format=original";
				case NPCID.BigSwampZombie://-35 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a7/Swamp_Zombie.png/revision/latest?cb=20170422121752&format=original";
				case NPCID.SmallSwampZombie://-34 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a7/Swamp_Zombie.png/revision/latest/scale-to-width-down/30?cb=20170422121752&format=original";
				case NPCID.BigSlimedZombie://-33 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/57/Slimed_Zombie.png/revision/latest?cb=20161120175703&format=original";
				case NPCID.SmallSlimedZombie://-32 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/57/Slimed_Zombie.png/revision/latest/scale-to-width-down/30?cb=20161120175703&format=original";
				case NPCID.BigPincushionZombie://-31 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/96/Pincushion_Zombie.png/revision/latest?cb=20170422122209&format=original";
				case NPCID.SmallPincushionZombie://-30 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/96/Pincushion_Zombie.png/revision/latest/scale-to-width-down/32?cb=20170422122209&format=original";
				case NPCID.BigBaldZombie://-29 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7a/Bald_Zombie.png/revision/latest?cb=20161120175629&format=original";
				case NPCID.SmallBaldZombie://-28 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7a/Bald_Zombie.png/revision/latest/scale-to-width-down/29?cb=20161120175629&format=original";
				case NPCID.BigZombie://-27 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c3/Zombie.png/revision/latest?cb=20171102011214&format=original";
				case NPCID.SmallZombie://-26 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c3/Zombie.png/revision/latest/scale-to-width-down/31?cb=20171102011214&format=original";
				case NPCID.BigCrimslime://-25 Crimslime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c9/Crimslime.png/revision/latest?cb=20150708221108&format=original";
				case NPCID.LittleCrimslime://-24 Crimslime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c9/Crimslime.png/revision/latest/scale-to-width-down/37?cb=20150708221108&format=original";
				case NPCID.BigCrimera://-23 Crimera
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3c/Crimera.png/revision/latest?cb=20200731032152&format=original";
				case NPCID.LittleCrimera://-22 Crimera
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3c/Crimera.png/revision/latest/scale-to-width-down/32?cb=20200731032152&format=original";
				case NPCID.GiantMossHornet://-21 Moss Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Moss_Hornet.png/revision/latest?cb=20170421222615&format=original";
				case NPCID.BigMossHornet://-20 Moss Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Moss_Hornet.png/revision/latest?cb=20170421222615&format=original";
				case NPCID.LittleMossHornet://-19 Moss Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Moss_Hornet.png/revision/latest/scale-to-width-down/43?cb=20170421222615&format=original";
				case NPCID.TinyMossHornet://-18 Moss Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Moss_Hornet.png/revision/latest/scale-to-width-down/38?cb=20170421222615&format=original";
				case NPCID.BigStinger://-17 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/72/Hornet.png/revision/latest?cb=20170420020349&format=original";
				case NPCID.LittleStinger://-16 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/72/Hornet.png/revision/latest/scale-to-width-down/36?cb=20170420020349&format=original";
				case NPCID.HeavySkeleton://-15 Armored Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7f/Armored_Skeleton.png/revision/latest?cb=20170421211538&format=original";
				case NPCID.BigBoned://-14 Angry Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f6/Angry_Bones_1.png/revision/latest?cb=20200530060826&format=original";
				case NPCID.ShortBones://-13 Angry Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f6/Angry_Bones_1.png/revision/latest/scale-to-width-down/27?cb=20200530060826&format=original";
				case NPCID.BigEater://-12 Eater of Souls
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ea/Eater_of_Souls.png/revision/latest?cb=20170420005422&format=original";
				case NPCID.LittleEater://-11 Eater of Souls
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ea/Eater_of_Souls.png/revision/latest/scale-to-width-down/36?cb=20170420005422&format=original";
				case NPCID.JungleSlime://-10 Jungle Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5d/Jungle_Slime.png/revision/latest?cb=20200730134909&format=original";
				case NPCID.YellowSlime://-9 Yellow Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Yellow_Slime.png/revision/latest?cb=20160925081607&format=original";
				case NPCID.RedSlime://-8 Red Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/87/Red_Slime.png/revision/latest?cb=20110828163005&format=original";
				case NPCID.PurpleSlime://-7 Purple Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/27/Purple_Slime.png/revision/latest?cb=20160925081604&format=original";
				case NPCID.BlackSlime://-6 Black Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Black_Slime.png/revision/latest?cb=20110828163020&format=original";
				case NPCID.BabySlime://-5 Baby Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6d/Baby_Slime.png/revision/latest?cb=20170121233645&format=original";
				case NPCID.Pinky://-4 Pinky
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/17/Pinky.png/revision/latest?cb=20111003022635&format=original";
				case NPCID.GreenSlime://-3 Green Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Green_Slime.png/revision/latest?cb=20141106201737&format=original";
				case NPCID.Slimer2://-2 Slimer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/45/Corrupt_Slime.png/revision/latest/scale-to-width-down/40?cb=20171130024032&format=original";
				case NPCID.Slimeling://-1 Slimeling
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ee/Slimeling.png/revision/latest?cb=20161002152710&format=original";
				case NPCID.None://0 
					return "";
				case NPCID.BlueSlime://1 Blue Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Blue_Slime.png/revision/latest?cb=20110828163020&format=original";
				case NPCID.DemonEye://2 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9d/Demon_Eye.png/revision/latest?cb=20170420003551&format=original";
				case NPCID.Zombie://3 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c3/Zombie.png/revision/latest?cb=20171102011214&format=original";
				case NPCID.EyeofCthulhu://4 Eye of Cthulhu
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/70/Eye_of_Cthulhu_%28Phase_1%29.gif/revision/latest?cb=20211114181102&format=original";
				case NPCID.ServantofCthulhu://5 Servant of Cthulhu
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/63/Servant_of_Cthulhu.png/revision/latest?cb=20170420005232&format=original";
				case NPCID.EaterofSouls://6 Eater of Souls
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f5/Eater_of_Souls.gif/revision/latest?cb=20170420010950&format=original";
				case NPCID.DevourerHead://7 Devourer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/25/Devourer_Head.png/revision/latest?cb=20170420005727&format=original";
				case NPCID.DevourerBody://8 Devourer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6a/Devourer_Body.png/revision/latest?cb=20170420005751&format=original";
				case NPCID.DevourerTail://9 Devourer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/77/Devourer_Tail.png/revision/latest?cb=20170420005829&format=original";
				case NPCID.GiantWormHead://10 Giant Worm
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b3/Giant_Worm_Head.png/revision/latest?cb=20170420005945&format=original";
				case NPCID.GiantWormBody://11 Giant Worm
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f6/Giant_Worm_Body.png/revision/latest?cb=20170420010026&format=original";
				case NPCID.GiantWormTail://12 Giant Worm
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3b/Giant_Worm_Tail.png/revision/latest?cb=20170420010057&format=original";
				case NPCID.EaterofWorldsHead://13 Eater of Worlds
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0f/Eater_of_Worlds_Head.png/revision/latest?cb=20170420010144&format=original";
				case NPCID.EaterofWorldsBody://14 Eater of Worlds
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0a/Eater_of_Worlds_Body.png/revision/latest?cb=20170420010209&format=original";
				case NPCID.EaterofWorldsTail://15 Eater of Worlds
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/92/Eater_of_Worlds_Tail.png/revision/latest?cb=20170420010233&format=original";
				case NPCID.MotherSlime://16 Mother Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Mother_Slime.png/revision/latest?cb=20160925081648&format=original";
				case NPCID.Merchant://17 Merchant
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/19/Merchant.png/revision/latest?cb=20211003230931&format=original";
				case NPCID.Nurse://18 Nurse
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/cc/Nurse.png/revision/latest?cb=20161005060102&format=original";
				case NPCID.ArmsDealer://19 Arms Dealer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9e/Arms_Dealer.png/revision/latest?cb=20161004000744&format=original";
				case NPCID.Dryad://20 Dryad
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Dryad.png/revision/latest?cb=20161004000507&format=original";
				case NPCID.Skeleton://21 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/23/Skeleton.png/revision/latest?cb=20170420012637&format=original";
				case NPCID.Guide://22 Guide
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7f/Guide.png/revision/latest?cb=20191003231144&format=original";
				case NPCID.MeteorHead://23 Meteor Head
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a1/Meteor_Head.png/revision/latest?cb=20170420013145&format=original";
				case NPCID.FireImp://24 Fire Imp
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/af/Fire_Imp.png/revision/latest?cb=20170420013443&format=original";
				case NPCID.BurningSphere://25 Burning Sphere
					return "";
				case NPCID.GoblinPeon://26 Goblin Peon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/61/Goblin_Peon.png/revision/latest?cb=20200518224850&format=original";
				case NPCID.GoblinThief://27 Goblin Thief
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/09/Goblin_Thief.png/revision/latest?cb=20200518230423&format=original";
				case NPCID.GoblinWarrior://28 Goblin Warrior
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b4/Goblin_Warrior.png/revision/latest?cb=20200518230412&format=original";
				case NPCID.GoblinSorcerer://29 Goblin Sorcerer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/43/Goblin_Sorcerer.png/revision/latest?cb=20200518230446&format=original";
				case NPCID.ChaosBall://30 Chaos Ball
					return "";
				case NPCID.AngryBones://31 Angry Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f6/Angry_Bones_1.png/revision/latest?cb=20200530060826&format=original";
				case NPCID.DarkCaster://32 Dark Caster
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c1/Dark_Caster.png/revision/latest?cb=20171104013247&format=original";
				case NPCID.WaterSphere://33 Water Sphere
					return "";
				case NPCID.CursedSkull://34 Cursed Skull
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/40/Cursed_Skull.png/revision/latest?cb=20200731025412&format=original";
				case NPCID.SkeletronHead://35 Skeletron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a3/Skeletron_Head.png/revision/latest?cb=20191003231538&format=original";
				case NPCID.SkeletronHand://36 Skeletron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0a/Skeletron_Hand_%28NPC%29.png/revision/latest?cb=20170420014655&format=original";
				case NPCID.OldMan://37 Old Man
					return "";
				case NPCID.Demolitionist://38 Demolitionist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6e/Demolitionist.png/revision/latest?cb=20200330043525&format=original";
				case NPCID.BoneSerpentHead://39 Bone Serpent
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1c/Bone_Serpent_Head.png/revision/latest?cb=20170420015651&format=original";
				case NPCID.BoneSerpentBody://40 Bone Serpent
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3d/Bone_Serpent_Body.png/revision/latest?cb=20170420015713&format=original";
				case NPCID.BoneSerpentTail://41 Bone Serpent
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/05/Bone_Serpent_Tail.png/revision/latest?cb=20170420015737&format=original";
				case NPCID.Hornet://42 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/72/Hornet.png/revision/latest?cb=20170420020349&format=original";
				case NPCID.ManEater://43 Man Eater
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a5/Man_Eater.png/revision/latest?cb=20170420020846&format=original";
				case NPCID.UndeadMiner://44 Undead Miner
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/73/Undead_Miner.png/revision/latest?cb=20170420021204&format=original";
				case NPCID.Tim://45 Tim
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ea/Tim.png/revision/latest?cb=20171104013044&format=original";
				case NPCID.Bunny://46 Bunny
					return "";
				case NPCID.CorruptBunny://47 Corrupt Bunny
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/42/Corrupt_Bunny.png/revision/latest?cb=20160929095411&format=original";
				case NPCID.Harpy://48 Harpy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1b/Harpy.png/revision/latest?cb=20200603151211&format=original";
				case NPCID.CaveBat://49 Cave Bat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0d/Cave_Bat.gif/revision/latest?cb=20211209033748&format=original";
				case NPCID.KingSlime://50 King Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/93/King_Slime.gif/revision/latest?cb=20200523113558&format=original";
				case NPCID.JungleBat://51 Jungle Bat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Jungle_Bat.png/revision/latest?cb=20161130084556&format=original";
				case NPCID.DoctorBones://52 Doctor Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/24/Doctor_Bones.png/revision/latest?cb=20170420102814&format=original";
				case NPCID.TheGroom://53 The Groom
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/cd/The_Groom.png/revision/latest?cb=20170420102844&format=original";
				case NPCID.Clothier://54 Clothier
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d2/Clothier.png/revision/latest?cb=20161009093143&format=original";
				case NPCID.Goldfish://55 Goldfish
					return "";
				case NPCID.Snatcher://56 Snatcher
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2b/Snatcher.png/revision/latest?cb=20170420104001&format=original";
				case NPCID.CorruptGoldfish://57 Corrupt Goldfish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6e/Corrupt_Goldfish.png/revision/latest?cb=20161012150337&format=original";
				case NPCID.Piranha://58 Piranha
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/43/Piranha.png/revision/latest?cb=20161126013943&format=original";
				case NPCID.LavaSlime://59 Lava Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c5/Lava_Slime.png/revision/latest?cb=20160925082050&format=original";
				case NPCID.Hellbat://60 Hellbat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/26/Hellbat.png/revision/latest?cb=20161113060925&format=original";
				case NPCID.Vulture://61 Vulture
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c7/Vulture_%28flying%29.png/revision/latest?cb=20210725202149&format=original";
				case NPCID.Demon://62 Demon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Demon.png/revision/latest?cb=20200526231750&format=original";
				case NPCID.BlueJellyfish://63 Blue Jellyfish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/98/Blue_Jellyfish.png/revision/latest?cb=20200803163235&format=original";
				case NPCID.PinkJellyfish://64 Pink Jellyfish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5d/Pink_Jellyfish.png/revision/latest?cb=20200517031622&format=original";
				case NPCID.Shark://65 Shark
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/14/Shark.png/revision/latest?cb=20170420104945&format=original";
				case NPCID.VoodooDemon://66 Voodoo Demon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/00/Voodoo_Demon.png/revision/latest?cb=20200517031810&format=original";
				case NPCID.Crab://67 Crab
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/63/Crab.png/revision/latest?cb=20170420105630&format=original";
				case NPCID.DungeonGuardian://68 Dungeon Guardian
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a4/Dungeon_Guardian.png/revision/latest?cb=20150227080514&format=original";
				case NPCID.Antlion://69 Antlion
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/ff/Antlion.png/revision/latest?cb=20191128180152&format=original";
				case NPCID.SpikeBall://70 Spike Ball
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9f/Spike_Ball.png/revision/latest?cb=20200807032629&format=original";
				case NPCID.DungeonSlime://71 Dungeon Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/50/Dungeon_Slime_%28Key%29.png/revision/latest?cb=20210430065342&format=original";
				case NPCID.BlazingWheel://72 Blazing Wheel
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/65/Blazing_Wheel.png/revision/latest?cb=20170421211040&format=original";
				case NPCID.GoblinScout://73 Goblin Scout
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/27/Goblin_Scout.png/revision/latest?cb=20200517031944&format=original";
				case NPCID.Bird://74 Bird
					return "";
				case NPCID.Pixie://75 Pixie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/22/Pixie.png/revision/latest?cb=20170421211325&format=original";
				case NPCID.None2://76 NPCName.None2
					return "";
				case NPCID.ArmoredSkeleton://77 Armored Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7f/Armored_Skeleton.png/revision/latest?cb=20170421211538&format=original";
				case NPCID.Mummy://78 Mummy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/70/Mummy.png/revision/latest?cb=20211207225057&format=original";
				case NPCID.DarkMummy://79 Dark Mummy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f1/Dark_Mummy_%28old%29.png/revision/latest?cb=20210205225723&format=original";
				case NPCID.LightMummy://80 Light Mummy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2a/Light_Mummy_%28old%29.png/revision/latest?cb=20210205225744&format=original";
				case NPCID.CorruptSlime://81 Corrupt Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/45/Corrupt_Slime.png/revision/latest?cb=20171130024032&format=original";
				case NPCID.Wraith://82 Wraith
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7c/Wraith.png/revision/latest?cb=20191128174013&format=original";
				case NPCID.CursedHammer://83 Cursed Hammer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/77/Cursed_Hammer.png/revision/latest?cb=20170420181531&format=original";
				case NPCID.EnchantedSword://84 Enchanted Sword
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ed/Enchanted_Sword_%28NPC%29.gif/revision/latest?cb=20180309231258&format=original";
				case NPCID.Mimic://85 Mimic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a8/Mimic.png/revision/latest?cb=20170421213035&format=original";
				case NPCID.Unicorn://86 Unicorn
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7e/Unicorn.png/revision/latest?cb=20200821032944&format=original";
				case NPCID.WyvernHead://87 Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1d/Wyvern_Head.png/revision/latest?cb=20170421215825&format=original";
				case NPCID.WyvernLegs://88 Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a0/Wyvern_Legs.png/revision/latest?cb=20170421215904&format=original";
				case NPCID.WyvernBody://89 Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/10/Wyvern_Body.png/revision/latest?cb=20170421220400&format=original";
				case NPCID.WyvernBody2://90 Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/82/Wyvern_Body_2.png/revision/latest?cb=20170421220323&format=original";
				case NPCID.WyvernBody3://91 Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/aa/Wyvern_Body_3.png/revision/latest?cb=20170421220433&format=original";
				case NPCID.WyvernTail://92 Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/35/Wyvern_Tail.png/revision/latest?cb=20170421220524&format=original";
				case NPCID.GiantBat://93 Giant Bat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3f/Giant_Bat.png/revision/latest?cb=20191128181401&format=original";
				case NPCID.Corruptor://94 Corruptor
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0e/Corruptor.gif/revision/latest?cb=20211216225509&format=original";
				case NPCID.DiggerHead://95 Digger
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3f/Digger_Head.png/revision/latest?cb=20170421222702&format=original";
				case NPCID.DiggerBody://96 Digger
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/91/Digger_Body.png/revision/latest?cb=20170421222727&format=original";
				case NPCID.DiggerTail://97 Digger
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/27/Digger_Tail.png/revision/latest?cb=20170421222751&format=original";
				case NPCID.SeekerHead://98 World Feeder
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c8/World_Feeder_Head.png/revision/latest?cb=20170421223257&format=original";
				case NPCID.SeekerBody://99 World Feeder
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e8/World_Feeder_Body.png/revision/latest?cb=20170421223327&format=original";
				case NPCID.SeekerTail://100 World Feeder
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a2/World_Feeder_Tail.png/revision/latest?cb=20170421223351&format=original";
				case NPCID.Clinger://101 Clinger
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/34/Clinger.png/revision/latest?cb=20170421224440&format=original";
				case NPCID.AnglerFish://102 Angler Fish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/af/Angler_Fish.png/revision/latest?cb=20170421222605&format=original";
				case NPCID.GreenJellyfish://103 Green Jellyfish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b0/Green_Jellyfish.png/revision/latest?cb=20200517031702&format=original";
				case NPCID.Werewolf://104 Werewolf
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f5/Werewolf.png/revision/latest?cb=20170420210112&format=original";
				case NPCID.BoundGoblin://105 Bound Goblin
					return "";
				case NPCID.BoundWizard://106 Bound Wizard
					return "";
				case NPCID.GoblinTinkerer://107 Goblin Tinkerer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/86/Goblin_Tinkerer.png/revision/latest?cb=20150705070124&format=original";
				case NPCID.Wizard://108 Wizard
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c7/Wizard.png/revision/latest?cb=20151018113651&format=original";
				case NPCID.Clown://109 Clown
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/ff/Clown.png/revision/latest?cb=20170421224900&format=original";
				case NPCID.SkeletonArcher://110 Skeleton Archer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Skeleton_Archer.png/revision/latest?cb=20170421225035&format=original";
				case NPCID.GoblinArcher://111 Goblin Archer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d4/Goblin_Archer.png/revision/latest?cb=20200517032400&format=original";
				case NPCID.VileSpit://112 Vile Spit
					return "";
				case NPCID.WallofFlesh://113 Wall of Flesh
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fd/Wall_of_Flesh_Mouth.png/revision/latest?cb=20170422001243&format=original";
				case NPCID.WallofFleshEye://114 Wall of Flesh
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/01/Wall_of_Flesh_Eye.png/revision/latest?cb=20170422001207&format=original";
				case NPCID.TheHungry://115 The Hungry
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/80/The_Hungry.png/revision/latest?cb=20210728032344&format=original";
				case NPCID.TheHungryII://116 The Hungry
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a1/The_Hungry_II.png/revision/latest?cb=20210728032519&format=original";
				case NPCID.LeechHead://117 Leech
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8f/Leech_Head.png/revision/latest?cb=20170422001834&format=original";
				case NPCID.LeechBody://118 Leech
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/90/Leech_Body.png/revision/latest?cb=20170422001857&format=original";
				case NPCID.LeechTail://119 Leech
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4b/Leech_Tail.png/revision/latest?cb=20170422001928&format=original";
				case NPCID.ChaosElemental://120 Chaos Elemental
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a9/Chaos_Elemental.png/revision/latest?cb=20170421223414&format=original";
				case NPCID.Slimer://121 Slimer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c6/Slimer.png/revision/latest?cb=20170422002239&format=original";
				case NPCID.Gastropod://122 Gastropod
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/73/Gastropod.png/revision/latest?cb=20211210033410&format=original";
				case NPCID.BoundMechanic://123 Bound Mechanic
					return "";
				case NPCID.Mechanic://124 Mechanic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/55/Mechanic.png/revision/latest?cb=20151018120500&format=original";
				case NPCID.Retinazer://125 Retinazer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/af/Retinazer_%28Second_Form%29.png/revision/latest?cb=20170421155457&format=original";
				case NPCID.Spazmatism://126 Spazmatism
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b9/Spazmatism_%28Second_Form%29.png/revision/latest?cb=20220227045610&format=original";
				case NPCID.SkeletronPrime://127 Skeletron Prime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/60/Skeletron_Prime.png/revision/latest?cb=20170928200428&format=original";
				case NPCID.PrimeCannon://128 Prime Cannon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5e/Prime_Cannon.png/revision/latest?cb=20170421151706&format=original";
				case NPCID.PrimeSaw://129 Prime Saw
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3b/Prime_Saw.png/revision/latest?cb=20170421151701&format=original";
				case NPCID.PrimeVice://130 Prime Vice
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/30/Prime_Vice.png/revision/latest?cb=20170421151703&format=original";
				case NPCID.PrimeLaser://131 Prime Laser
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1e/Prime_Laser.png/revision/latest?cb=20170421151658&format=original";
				case NPCID.BaldZombie://132 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7a/Bald_Zombie.png/revision/latest?cb=20161120175629&format=original";
				case NPCID.WanderingEye://133 Wandering Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a1/Wandering_Eye.png/revision/latest?cb=20161120175931&format=original";
				case NPCID.TheDestroyer://134 The Destroyer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/27/The_Destroyer_Head.png/revision/latest?cb=20150708221902&format=original";
				case NPCID.TheDestroyerBody://135 The Destroyer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/20/The_Destroyer_Body.png/revision/latest?cb=20150708221907&format=original";
				case NPCID.TheDestroyerTail://136 The Destroyer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b9/The_Destroyer_Tail.png/revision/latest?cb=20150708221909&format=original";
				case NPCID.IlluminantBat://137 Illuminant Bat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2f/Illuminant_Bat.gif/revision/latest?cb=20211115004135&format=original";
				case NPCID.IlluminantSlime://138 Illuminant Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/55/Illuminant_Slime.png/revision/latest?cb=20171130024631&format=original";
				case NPCID.Probe://139 Probe
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Probe.png/revision/latest?cb=20150708201700&format=original";
				case NPCID.PossessedArmor://140 Possessed Armor
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/55/Possessed_Armor.png/revision/latest?cb=20140526012145&format=original";
				case NPCID.ToxicSludge://141 Toxic Sludge
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/cf/Toxic_Sludge.gif/revision/latest?cb=20200808162634&format=original";
				case NPCID.SantaClaus://142 Santa Claus
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/58/Santa_Claus.png/revision/latest?cb=20201013025452&format=original";
				case NPCID.SnowmanGangsta://143 Snowman Gangsta
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7a/Snowman_Gangsta.png/revision/latest?cb=20210620022429&format=original";
				case NPCID.MisterStabby://144 Mister Stabby
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e5/Mister_Stabby.png/revision/latest?cb=20170422003215&format=original";
				case NPCID.SnowBalla://145 Snow Balla
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/dc/Snow_Balla.png/revision/latest?cb=20170422003414&format=original";
				case NPCID.None3://146 NPCName.None3
					return "";
				case NPCID.IceSlime://147 Ice Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5a/Ice_Slime.png/revision/latest?cb=20170422004036&format=original";
				case NPCID.Penguin://148 Penguin
					return "";
				case NPCID.PenguinBlack://149 Penguin
					return "";
				case NPCID.IceBat://150 Ice Bat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8f/Ice_Bat.png/revision/latest?cb=20170420001843&format=original";
				case NPCID.Lavabat://151 Lava Bat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c5/Lava_Bat.png/revision/latest?cb=20170420001653&format=original";
				case NPCID.GiantFlyingFox://152 Giant Flying Fox
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/44/Giant_Flying_Fox.png/revision/latest?cb=20170421020011&format=original";
				case NPCID.GiantTortoise://153 Giant Tortoise
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/03/Giant_Tortoise.png/revision/latest?cb=20170421020231&format=original";
				case NPCID.IceTortoise://154 Ice Tortoise
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f1/Ice_Tortoise.png/revision/latest?cb=20170421020410&format=original";
				case NPCID.Wolf://155 Wolf
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c8/Wolf.png/revision/latest?cb=20200517032614&format=original";
				case NPCID.RedDevil://156 Red Devil
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/06/Red_Devil.png/revision/latest?cb=20170422150043&format=original";
				case NPCID.Arapaima://157 Arapaima
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/06/Arapaima.png/revision/latest?cb=20170422005426&format=original";
				case NPCID.VampireBat://158 Vampire
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/89/Vampire_Bat.png/revision/latest?cb=20170421221803&format=original";
				case NPCID.Vampire://159 Vampire
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4e/Vampire.png/revision/latest?cb=20170422005918&format=original";
				case NPCID.Truffle://160 Truffle
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f2/Truffle.png/revision/latest?cb=20200704192524&format=original";
				case NPCID.ZombieEskimo://161 Frozen Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/cf/Frozen_Zombie.png/revision/latest?cb=20170422010132&format=original";
				case NPCID.Frankenstein://162 Frankenstein
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d9/Frankenstein.png/revision/latest?cb=20220304235913&format=original";
				case NPCID.BlackRecluse://163 Black Recluse
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a9/Black_Recluse_%28ground%29.png/revision/latest?cb=20191019095158&format=original";
				case NPCID.WallCreeper://164 Wall Creeper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8c/Wall_Creeper_%28ground%29.png/revision/latest?cb=20210620011544&format=original";
				case NPCID.WallCreeperWall://165 Wall Creeper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/89/Wall_Creeper.png/revision/latest?cb=20200804000512&format=original";
				case NPCID.SwampThing://166 Swamp Thing
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fe/Swamp_Thing.png/revision/latest?cb=20191019095727&format=original";
				case NPCID.UndeadViking://167 Undead Viking
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/83/Undead_Viking.png/revision/latest?cb=20220305001117&format=original";
				case NPCID.CorruptPenguin://168 Corrupt Penguin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Corrupt_Penguin.png/revision/latest?cb=20200730150902&format=original";
				case NPCID.IceElemental://169 Ice Elemental
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/35/Ice_Elemental.png/revision/latest?cb=20170422120617&format=original";
				case NPCID.PigronCorruption://170 Pigron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/18/Corruption_Pigron.png/revision/latest?cb=20150714194850&format=original";
				case NPCID.PigronHallow://171 Pigron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8e/Hallow_Pigron.png/revision/latest?cb=20150714194917&format=original";
				case NPCID.RuneWizard://172 Rune Wizard
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Rune_Wizard.png/revision/latest?cb=20170524102422&format=original";
				case NPCID.Crimera://173 Crimera
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4d/Crimera.gif/revision/latest?cb=20211216224331&format=original";
				case NPCID.Herpling://174 Herpling
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5e/Herpling.png/revision/latest?cb=20170421162114&format=original";
				case NPCID.AngryTrapper://175 Angry Trapper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a6/Angry_Trapper.png/revision/latest?cb=20200808223845&format=original";
				case NPCID.MossHornet://176 Moss Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c4/Moss_Hornet.png/revision/latest?cb=20170421222615&format=original";
				case NPCID.Derpling://177 Derpling
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/34/Derpling.png/revision/latest?cb=20170421203737&format=original";
				case NPCID.Steampunker://178 Steampunker
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/82/Steampunker.png/revision/latest?cb=20200702150220&format=original";
				case NPCID.CrimsonAxe://179 Crimson Axe
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/df/Crimson_Axe.png/revision/latest?cb=20170420181950&format=original";
				case NPCID.PigronCrimson://180 Pigron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Crimson_Pigron.png/revision/latest?cb=20210210223359&format=original";
				case NPCID.FaceMonster://181 Face Monster
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f6/Face_Monster.png/revision/latest?cb=20170421204044&format=original";
				case NPCID.FloatyGross://182 Floaty Gross
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/87/Floaty_Gross.png/revision/latest?cb=20170422121522&format=original";
				case NPCID.Crimslime://183 Crimslime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c9/Crimslime.png/revision/latest?cb=20150708221108&format=original";
				case NPCID.SpikedIceSlime://184 Spiked Ice Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bc/Spiked_Ice_Slime.png/revision/latest?cb=20170422121847&format=original";
				case NPCID.SnowFlinx://185 Snow Flinx
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/cb/Snow_Flinx.png/revision/latest?cb=20170421233536&format=original";
				case NPCID.PincushionZombie://186 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/96/Pincushion_Zombie.png/revision/latest?cb=20170422122209&format=original";
				case NPCID.SlimedZombie://187 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/57/Slimed_Zombie.png/revision/latest?cb=20161120175703&format=original";
				case NPCID.SwampZombie://188 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a7/Swamp_Zombie.png/revision/latest?cb=20170422121752&format=original";
				case NPCID.TwiggyZombie://189 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Twiggy_Zombie.png/revision/latest?cb=20170422121749&format=original";
				case NPCID.CataractEye://190 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/de/Cataract_Eye.png/revision/latest?cb=20170422122610&format=original";
				case NPCID.SleepyEye://191 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/29/Sleepy_Eye.png/revision/latest?cb=20170422122738&format=original";
				case NPCID.DialatedEye://192 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/af/Dilated_Eye.png/revision/latest?cb=20170422122641&format=original";
				case NPCID.GreenEye://193 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2e/Green_Eye.png/revision/latest?cb=20170422122705&format=original";
				case NPCID.PurpleEye://194 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Purple_Eye.png/revision/latest?cb=20170422122804&format=original";
				case NPCID.LostGirl://195 Lost Girl
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b3/Lost_Girl.png/revision/latest?cb=20170421233129&format=original";
				case NPCID.Nymph://196 Nymph
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/62/Nymph.png/revision/latest?cb=20170421233126&format=original";
				case NPCID.ArmoredViking://197 Armored Viking
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Armored_Viking.png/revision/latest?cb=20220305001234&format=original";
				case NPCID.Lihzahrd://198 Lihzahrd
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ee/Lihzahrd.png/revision/latest?cb=20170422123231&format=original";
				case NPCID.LihzahrdCrawler://199 Lihzahrd
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c5/Lihzahrd_%28crawler%29.png/revision/latest?cb=20211222232440&format=original";
				case NPCID.FemaleZombie://200 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a1/Female_Zombie.png/revision/latest?cb=20170422121903&format=original";
				case NPCID.HeadacheSkeleton://201 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/62/Headache_Skeleton.png/revision/latest?cb=20170422123913&format=original";
				case NPCID.MisassembledSkeleton://202 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Misassembled_Skeleton.png/revision/latest?cb=20170422123940&format=original";
				case NPCID.PantlessSkeleton://203 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/be/Pantless_Skeleton.png/revision/latest?cb=20170422124010&format=original";
				case NPCID.SpikedJungleSlime://204 Spiked Jungle Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fe/Spiked_Jungle_Slime.png/revision/latest?cb=20171130030805&format=original";
				case NPCID.Moth://205 Moth
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/09/Moth.png/revision/latest?cb=20170422123155&format=original";
				case NPCID.IcyMerman://206 Icy Merman
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4f/Icy_Merman.png/revision/latest?cb=20170421020536&format=original";
				case NPCID.DyeTrader://207 Dye Trader
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/51/Dye_Trader.png/revision/latest?cb=20161009093013&format=original";
				case NPCID.PartyGirl://208 Party Girl
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a8/Party_Girl.png/revision/latest?cb=20161130010012&format=original";
				case NPCID.Cyborg://209 Cyborg
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a3/Cyborg.png/revision/latest?cb=20161004001101&format=original";
				case NPCID.Bee://210 Bee
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/56/Bee.png/revision/latest?cb=20170422124353&format=original";
				case NPCID.BeeSmall://211 Bee
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b3/Bee_%28Small%29.png/revision/latest?cb=20170422124331&format=original";
				case NPCID.PirateDeckhand://212 Pirate Deckhand
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b8/Pirate_Deckhand.png/revision/latest?cb=20210625214940&format=original";
				case NPCID.PirateCorsair://213 Pirate Corsair
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e5/Pirate_Corsair.png/revision/latest?cb=20210625214836&format=original";
				case NPCID.PirateDeadeye://214 Pirate Deadeye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Pirate_Deadeye.png/revision/latest?cb=20170421234512&format=original";
				case NPCID.PirateCrossbower://215 Pirate Crossbower
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Pirate_Crossbower.png/revision/latest?cb=20170421234501&format=original";
				case NPCID.PirateCaptain://216 Pirate Captain
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/37/Pirate_Captain.png/revision/latest?cb=20210914050020&format=original";
				case NPCID.CochinealBeetle://217 Cochineal Beetle
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9b/Cochineal_Beetle.png/revision/latest?cb=20200523235211&format=original";
				case NPCID.CyanBeetle://218 Cyan Beetle
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6c/Cyan_Beetle.png/revision/latest?cb=20200523235324&format=original";
				case NPCID.LacBeetle://219 Lac Beetle
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/61/Lac_Beetle.png/revision/latest?cb=20200808224342&format=original";
				case NPCID.SeaSnail://220 Sea Snail
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/41/Sea_Snail.png/revision/latest?cb=20171104195216&format=original";
				case NPCID.Squid://221 Squid
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/81/Squid.png/revision/latest?cb=20140905021230&format=original";
				case NPCID.QueenBee://222 Queen Bee
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/60/Queen_Bee.gif/revision/latest?cb=20200523215116&format=original";
				case NPCID.ZombieRaincoat://223 Raincoat Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Raincoat_Zombie.png/revision/latest?cb=20170805211221&format=original";
				case NPCID.FlyingFish://224 Flying Fish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bc/Flying_Fish.png/revision/latest?cb=20170305130407&format=original";
				case NPCID.UmbrellaSlime://225 Umbrella Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d9/Umbrella_Slime.png/revision/latest?cb=20160726020032&format=original";
				case NPCID.FlyingSnake://226 Flying Snake
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/82/Flying_Snake.png/revision/latest?cb=20161024041552&format=original";
				case NPCID.Painter://227 Painter
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/24/Painter.png/revision/latest?cb=20150705103620&format=original";
				case NPCID.WitchDoctor://228 Witch Doctor
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/ac/Witch_Doctor.png/revision/latest?cb=20170108122024&format=original";
				case NPCID.Pirate://229 Pirate
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7d/Pirate.png/revision/latest?cb=20170421220847&format=original";
				case NPCID.GoldfishWalker://230 Goldfish
					return "";
				case NPCID.HornetFatty://231 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fc/Fatty_Hornet.png/revision/latest?cb=20170422125636&format=original";
				case NPCID.HornetHoney://232 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b7/Honey_Hornet.png/revision/latest?cb=20170422125704&format=original";
				case NPCID.HornetLeafy://233 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Leafy_Hornet.png/revision/latest?cb=20170422125730&format=original";
				case NPCID.HornetSpikey://234 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2a/Spikey_Hornet.png/revision/latest?cb=20170422125808&format=original";
				case NPCID.HornetStingy://235 Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Hornet_5.png/revision/latest?cb=20170422125834&format=original";
				case NPCID.JungleCreeper://236 Jungle Creeper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9e/Jungle_Creeper_%28ground%29.png/revision/latest?cb=20201014050951&format=original";
				case NPCID.JungleCreeperWall://237 Jungle Creeper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e9/Jungle_Creeper.png/revision/latest?cb=20201014050936&format=original";
				case NPCID.BlackRecluseWall://238 Black Recluse
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/26/Black_Recluse.png/revision/latest?cb=20210620010827&format=original";
				case NPCID.BloodCrawler://239 Blood Crawler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/81/Blood_Crawler_%28ground%29.png/revision/latest?cb=20200521181748&format=original";
				case NPCID.BloodCrawlerWall://240 Blood Crawler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7e/Blood_Crawler.png/revision/latest?cb=20200804000419&format=original";
				case NPCID.BloodFeeder://241 Blood Feeder
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/69/Blood_Feeder.png/revision/latest?cb=20200521181749&format=original";
				case NPCID.BloodJelly://242 Blood Jelly
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7c/Blood_Jelly.png/revision/latest?cb=20200803162612&format=original";
				case NPCID.IceGolem://243 Ice Golem
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8a/Ice_Golem.png/revision/latest?cb=20200708133205&format=original";
				case NPCID.RainbowSlime://244 Rainbow Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/87/Rainbow_Slime.gif/revision/latest?cb=20210921162633&format=original";
				case NPCID.Golem://245 Golem
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/37/Golem_Core.png/revision/latest?cb=20200517202317&format=original";
				case NPCID.GolemHead://246 Golem Head
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9f/Golem_Head.png/revision/latest?cb=20200602093043&format=original";
				case NPCID.GolemFistLeft://247 Golem Fist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f5/Left_Golem_Fist.png/revision/latest?cb=20200529235418&format=original";
				case NPCID.GolemFistRight://248 Golem Fist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f7/Right_Golem_Fist.png/revision/latest?cb=20200602093336&format=original";
				case NPCID.GolemHeadFree://249 Golem Head
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f9/Free_Golem_Head.png/revision/latest?cb=20200602094652&format=original";
				case NPCID.AngryNimbus://250 Angry Nimbus
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/db/Angry_Nimbus.png/revision/latest?cb=20171130030045&format=original";
				case NPCID.Eyezor://251 Eyezor
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b6/Eyezor.png/revision/latest?cb=20210611165016&format=original";
				case NPCID.Parrot://252 Parrot
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/16/Parrot.png/revision/latest?cb=20170421215823&format=original";
				case NPCID.Reaper://253 Reaper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6a/Reaper.png/revision/latest?cb=20171130031709&format=original";
				case NPCID.ZombieMushroom://254 Spore Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/13/Spore_Zombie.png/revision/latest?cb=20200521181815&format=original";
				case NPCID.ZombieMushroomHat://255 Spore Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b1/Mushroom_Zombie.png/revision/latest?cb=20200521181747&format=original";
				case NPCID.FungoFish://256 Fungo Fish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/be/Fungo_Fish.png/revision/latest?cb=20200521181744&format=original";
				case NPCID.AnomuraFungus://257 Anomura Fungus
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f0/Anomura_Fungus.png/revision/latest?cb=20200521181927&format=original";
				case NPCID.MushiLadybug://258 Mushi Ladybug
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/85/Mushi_Ladybug.png/revision/latest?cb=20200521181746&format=original";
				case NPCID.FungiBulb://259 Fungi Bulb
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f5/Fungi_Bulb.png/revision/latest?cb=20200521175303&format=original";
				case NPCID.GiantFungiBulb://260 Giant Fungi Bulb
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5f/Giant_Fungi_Bulb.png/revision/latest?cb=20200521175310&format=original";
				case NPCID.FungiSpore://261 Fungi Spore
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/99/Fungi_Spore.png/revision/latest?cb=20200521174943&format=original";
				case NPCID.Plantera://262 Plantera
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9e/Plantera_%28First_form%29.gif/revision/latest?cb=20200706212844&format=original";
				case NPCID.PlanterasHook://263 Plantera's Hook
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/30/Plantera%27s_Hook.png/revision/latest?cb=20200521175019&format=original";
				case NPCID.PlanterasTentacle://264 Plantera's Tentacle
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a5/Plantera%27s_Tentacle.png/revision/latest?cb=20200521174809&format=original";
				case NPCID.Spore://265 Spore
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a9/Spore.png/revision/latest?cb=20200521174616&format=original";
				case NPCID.BrainofCthulhu://266 Brain of Cthulhu
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/18/Brain_of_Cthulhu_%28First_Phase%29.gif/revision/latest?cb=20211114173255&format=original";
				case NPCID.Creeper://267 Creeper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0a/Creeper.png/revision/latest?cb=20200521174446&format=original";
				case NPCID.IchorSticker://268 Ichor Sticker
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8e/Ichor_Sticker.png/revision/latest?cb=20200521181745&format=original";
				case NPCID.RustyArmoredBonesAxe://269 Rusty Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/74/Rusty_Armored_Bones_1.png/revision/latest?cb=20200811222335&format=original";
				case NPCID.RustyArmoredBonesFlail://270 Rusty Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/54/Rusty_Armored_Bones_2.png/revision/latest?cb=20200811222519&format=original";
				case NPCID.RustyArmoredBonesSword://271 Rusty Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d8/Rusty_Armored_Bones_3.png/revision/latest?cb=20200811222656&format=original";
				case NPCID.RustyArmoredBonesSwordNoArmor://272 Rusty Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8b/Rusty_Armored_Bones_4.png/revision/latest?cb=20200811222738&format=original";
				case NPCID.BlueArmoredBones://273 Blue Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d7/Blue_Armored_Bones_1.png/revision/latest?cb=20200530060830&format=original";
				case NPCID.BlueArmoredBonesMace://274 Blue Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9f/Blue_Armored_Bones_2.png/revision/latest?cb=20200530060831&format=original";
				case NPCID.BlueArmoredBonesNoPants://275 Blue Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/05/Blue_Armored_Bones_3.png/revision/latest?cb=20200530060833&format=original";
				case NPCID.BlueArmoredBonesSword://276 Blue Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/53/Blue_Armored_Bones_4.png/revision/latest?cb=20200530060834&format=original";
				case NPCID.HellArmoredBones://277 Hell Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/56/Hell_Armored_Bones_1.png/revision/latest?cb=20200518132724&format=original";
				case NPCID.HellArmoredBonesSpikeShield://278 Hell Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2e/Hell_Armored_Bones_2.png/revision/latest?cb=20200518132721&format=original";
				case NPCID.HellArmoredBonesMace://279 Hell Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d7/Hell_Armored_Bones_3.png/revision/latest?cb=20200518132718&format=original";
				case NPCID.HellArmoredBonesSword://280 Hell Armored Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/05/Hell_Armored_Bones_4.png/revision/latest?cb=20200518132730&format=original";
				case NPCID.RaggedCaster://281 Ragged Caster
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/40/Ragged_Caster_1.png/revision/latest?cb=20220312055614&format=original";
				case NPCID.RaggedCasterOpenCoat://282 Ragged Caster
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fc/Ragged_Caster_2.png/revision/latest?cb=20220312055704&format=original";
				case NPCID.Necromancer://283 Necromancer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4e/Necromancer_1.png/revision/latest?cb=20220312053825&format=original";
				case NPCID.NecromancerArmored://284 Necromancer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3a/Necromancer_2.png/revision/latest?cb=20220312053710&format=original";
				case NPCID.DiabolistRed://285 Diabolist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6e/Diabolist_1.png/revision/latest?cb=20220312060327&format=original";
				case NPCID.DiabolistWhite://286 Diabolist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Diabolist_2.png/revision/latest?cb=20220312060429&format=original";
				case NPCID.BoneLee://287 Bone Lee
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Bone_Lee.png/revision/latest?cb=20200803161640&format=original";
				case NPCID.DungeonSpirit://288 Dungeon Spirit
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b7/Dungeon_Spirit.png/revision/latest?cb=20171130033849&format=original";
				case NPCID.GiantCursedSkull://289 Giant Cursed Skull
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/97/Giant_Cursed_Skull.png/revision/latest?cb=20200521174503&format=original";
				case NPCID.Paladin://290 Paladin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b2/Paladin.png/revision/latest?cb=20200521173908&format=original";
				case NPCID.SkeletonSniper://291 Skeleton Sniper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4d/Skeleton_Sniper.png/revision/latest?cb=20200518190711&format=original";
				case NPCID.TacticalSkeleton://292 Tactical Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1b/Skeleton_Commando.png/revision/latest?cb=20200604133711&format=original";
				case NPCID.SkeletonCommando://293 Skeleton Commando
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1b/Skeleton_Commando.png/revision/latest?cb=20200604133711&format=original";
				case NPCID.AngryBonesBig://294 Angry Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/33/Angry_Bones_2.png/revision/latest?cb=20200530060827&format=original";
				case NPCID.AngryBonesBigMuscle://295 Angry Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b9/Angry_Bones_3.png/revision/latest?cb=20200530060828&format=original";
				case NPCID.AngryBonesBigHelmet://296 Angry Bones
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/56/Angry_Bones_4.png/revision/latest?cb=20200530060829&format=original";
				case NPCID.BirdBlue://297 Blue Jay
					return "";
				case NPCID.BirdRed://298 Cardinal
					return "";
				case NPCID.Squirrel://299 Squirrel
					return "";
				case NPCID.Mouse://300 Mouse
					return "";
				case NPCID.Raven://301 Raven
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/77/Raven_%28flying%29.png/revision/latest?cb=20210914024621&format=original";
				case NPCID.SlimeMasked://302 Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a4/Bunny_Slime.png/revision/latest?cb=20151229024026&format=original";
				case NPCID.BunnySlimed://303 Bunny
					return "";
				case NPCID.HoppinJack://304 Hoppin' Jack
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/43/Hoppin%27_Jack.png/revision/latest?cb=20131025174453&format=original";
				case NPCID.Scarecrow1://305 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e6/Scarecrow_1.png/revision/latest?cb=20141219002114&format=original";
				case NPCID.Scarecrow2://306 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bd/Scarecrow_2.png/revision/latest?cb=20141219002212&format=original";
				case NPCID.Scarecrow3://307 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/74/Scarecrow_3.png/revision/latest?cb=20141219002229&format=original";
				case NPCID.Scarecrow4://308 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/ff/Scarecrow_4.png/revision/latest?cb=20141219002245&format=original";
				case NPCID.Scarecrow5://309 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2d/Scarecrow_5.png/revision/latest?cb=20141219002324&format=original";
				case NPCID.Scarecrow6://310 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Scarecrow.png/revision/latest?cb=20141219002655&format=original";
				case NPCID.Scarecrow7://311 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4b/Scarecrow_7.png/revision/latest?cb=20141219002347&format=original";
				case NPCID.Scarecrow8://312 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/34/Scarecrow_8.png/revision/latest?cb=20141219002435&format=original";
				case NPCID.Scarecrow9://313 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f7/Scarecrow_9.png/revision/latest?cb=20141219002459&format=original";
				case NPCID.Scarecrow10://314 Scarecrow
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/62/Scarecrow_10.png/revision/latest?cb=20131025174054&format=original";
				case NPCID.HeadlessHorseman://315 Headless Horseman
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3d/Headless_Horseman.png/revision/latest?cb=20210625213223&format=original";
				case NPCID.Ghost://316 Ghost
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/70/Ghost_%28enemy%29.png/revision/latest?cb=20131025175525&format=original";
				case NPCID.DemonEyeOwl://317 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d7/Demon_Eye_Halloween_Variant_1.png/revision/latest?cb=20170422125859&format=original";
				case NPCID.DemonEyeSpaceship://318 Demon Eye
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6a/Demon_Eye_Halloween_Variant_2.png/revision/latest?cb=20170422125929&format=original";
				case NPCID.ZombieDoctor://319 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6c/Zombie_Halloween_Variant_1.png/revision/latest?cb=20170422121435&format=original";
				case NPCID.ZombieSuperman://320 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/26/Zombie_Halloween_Variant_2.png/revision/latest?cb=20170422121433&format=original";
				case NPCID.ZombiePixie://321 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0c/Zombie_Halloween_Variant_3.png/revision/latest?cb=20170422121437&format=original";
				case NPCID.SkeletonTopHat://322 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9e/Skeleton_Halloween_Variant_1.png/revision/latest?cb=20171104202519&format=original";
				case NPCID.SkeletonAstonaut://323 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/65/Skeleton_Halloween_Variant_2.png/revision/latest?cb=20200808224709&format=original";
				case NPCID.SkeletonAlien://324 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/68/Skeleton_Halloween_Variant_3.png/revision/latest?cb=20171104202707&format=original";
				case NPCID.MourningWood://325 Mourning Wood
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/90/Mourning_Wood.gif/revision/latest?cb=20190125130555&format=original";
				case NPCID.Splinterling://326 Splinterling
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/88/Splinterling.png/revision/latest?cb=20171104210227&format=original";
				case NPCID.Pumpking://327 Pumpking
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d8/Pumpking.png/revision/latest?cb=20210914051940&format=original";
				case NPCID.PumpkingBlade://328 Pumpking
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c5/Pumpking_Hand.png/revision/latest?cb=20141218234517&format=original";
				case NPCID.Hellhound://329 Hellhound
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3e/Hellhound.png/revision/latest?cb=20171213013510&format=original";
				case NPCID.Poltergeist://330 Poltergeist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bb/Poltergeist.png/revision/latest?cb=20131025170412&format=original";
				case NPCID.ZombieXmas://331 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Zombie_Christmas_Variant_1.png/revision/latest?cb=20170422121212&format=original";
				case NPCID.ZombieSweater://332 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b2/Zombie_Christmas_Variant_2.png/revision/latest?cb=20170422121209&format=original";
				case NPCID.SlimeRibbonWhite://333 Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/59/White_Present_Slime.png/revision/latest?cb=20170217233818&format=original";
				case NPCID.SlimeRibbonYellow://334 Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c9/Yellow_Present_Slime.png/revision/latest?cb=20170217233853&format=original";
				case NPCID.SlimeRibbonGreen://335 Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1d/Green_Present_Slime.png/revision/latest?cb=20170217233925&format=original";
				case NPCID.SlimeRibbonRed://336 Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bf/Red_Present_Slime.png/revision/latest?cb=20170217234001&format=original";
				case NPCID.BunnyXmas://337 Bunny
					return "";
				case NPCID.ZombieElf://338 Zombie Elf
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fb/Zombie_Elf.png/revision/latest?cb=20200702150744&format=original";
				case NPCID.ZombieElfBeard://339 Zombie Elf
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9e/Zombie_Elf_Beard.png/revision/latest?cb=20200702150736&format=original";
				case NPCID.ZombieElfGirl://340 Zombie Elf
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e0/Zombie_Elf_Girl.png/revision/latest?cb=20200702150727&format=original";
				case NPCID.PresentMimic://341 Present Mimic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8b/Present_Mimic_Forms.png/revision/latest?cb=20200730152725&format=original";
				case NPCID.GingerbreadMan://342 Gingerbread Man
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2e/Gingerbread_Man.png/revision/latest?cb=20200708164201&format=original";
				case NPCID.Yeti://343 Yeti
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7d/Yeti.png/revision/latest?cb=20200518194832&format=original";
				case NPCID.Everscream://344 Everscream
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Everscream.gif/revision/latest?cb=20200605002358&format=original";
				case NPCID.IceQueen://345 Ice Queen
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9f/Ice_Queen.png/revision/latest?cb=20171213014122&format=original";
				case NPCID.SantaNK1://346 Santa-NK1
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ee/Santa-NK1.png/revision/latest?cb=20131220135050&format=original";
				case NPCID.ElfCopter://347 Elf Copter
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/45/Elf_Copter.png/revision/latest?cb=20200518194558&format=original";
				case NPCID.Nutcracker://348 Nutcracker
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/38/Nutcracker.png/revision/latest?cb=20200911122316&format=original";
				case NPCID.NutcrackerSpinning://349 Nutcracker
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ec/Nutcracker_2.png/revision/latest?cb=20200911122308&format=original";
				case NPCID.ElfArcher://350 Elf Archer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d3/Elf_Archer.png/revision/latest?cb=20200803161148&format=original";
				case NPCID.Krampus://351 Krampus
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8f/Krampus.png/revision/latest?cb=20200702145123&format=original";
				case NPCID.Flocko://352 Flocko
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/62/Flocko.png/revision/latest?cb=20200518194055&format=original";
				case NPCID.Stylist://353 Stylist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/16/Stylist.png/revision/latest?cb=20151031152652&format=original";
				case NPCID.WebbedStylist://354 Webbed Stylist
					return "";
				case NPCID.Firefly://355 Firefly
					return "";
				case NPCID.Butterfly://356 Butterfly
					return "";
				case NPCID.Worm://357 Worm
					return "";
				case NPCID.LightningBug://358 Lightning Bug
					return "";
				case NPCID.Snail://359 Snail
					return "";
				case NPCID.GlowingSnail://360 Glowing Snail
					return "";
				case NPCID.Frog://361 Frog
					return "";
				case NPCID.Duck://362 Duck
					return "";
				case NPCID.Duck2://363 Duck
					return "";
				case NPCID.DuckWhite://364 Duck
					return "";
				case NPCID.DuckWhite2://365 Duck
					return "";
				case NPCID.ScorpionBlack://366 Scorpion
					return "";
				case NPCID.Scorpion://367 Scorpion
					return "";
				case NPCID.TravellingMerchant://368 Traveling Merchant
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/37/Traveling_Merchant.png/revision/latest?cb=20150704081454&format=original";
				case NPCID.Angler://369 Angler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bf/Angler.png/revision/latest?cb=20200702150720&format=original	";
				case NPCID.DukeFishron://370 Duke Fishron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0b/Duke_Fishron.png/revision/latest?cb=20180705150806&format=original";
				case NPCID.DetonatingBubble://371 Detonating Bubble
					return "";
				case NPCID.Sharkron://372 Sharkron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ef/Sharkron.png/revision/latest?cb=20140508212606&format=original";
				case NPCID.Sharkron2://373 Sharkron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ef/Sharkron.png/revision/latest?cb=20140508212606&format=original";
				case NPCID.TruffleWorm://374 Truffle Worm
					return "";
				case NPCID.TruffleWormDigger://375 Truffle Worm
					return "";
				case NPCID.SleepingAngler://376 Sleeping Angler
					return "";
				case NPCID.Grasshopper://377 Grasshopper
					return "";
				case NPCID.ChatteringTeethBomb://378 Chattering Teeth Bomb
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/60/Chattering_Teeth_Bomb.gif/revision/latest?cb=20210627211221&format=original";
				case NPCID.CultistArcherBlue://379 Cultist Archer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/25/Cultist_Archer.png/revision/latest?cb=20150701095329&format=original";
				case NPCID.CultistArcherWhite://380 Cultist Archer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/30/White_Cultist_Archer.png/revision/latest?cb=20150701095459&format=original";
				case NPCID.BrainScrambler://381 Brain Scrambler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/38/Brain_Scrambler.png/revision/latest?cb=20200704161941&format=original";
				case NPCID.RayGunner://382 Ray Gunner
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d7/Ray_Gunner.png/revision/latest?cb=20150701095421&format=original";
				case NPCID.MartianOfficer://383 Martian Officer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b7/Martian_Officer.png/revision/latest?cb=20150701010423&format=original";
				case NPCID.ForceBubble://384 Bubble Shield
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Bubble_Shield.png/revision/latest?cb=20150701010521&format=original";
				case NPCID.GrayGrunt://385 Gray Grunt
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8e/Gray_Grunt.png/revision/latest?cb=20150701095347&format=original";
				case NPCID.MartianEngineer://386 Martian Engineer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/15/Martian_Engineer.png/revision/latest?cb=20210914031152&format=original";
				case NPCID.MartianTurret://387 Tesla Turret
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9f/Tesla_Turret.gif/revision/latest?cb=20211204005336&format=original";
				case NPCID.MartianDrone://388 Martian Drone
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ea/Martian_Drone.png/revision/latest?cb=20150701135039&format=original";
				case NPCID.GigaZapper://389 Gigazapper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/29/Gigazapper.png/revision/latest?cb=20210914030825&format=original";
				case NPCID.ScutlixRider://390 Scutlix Gunner
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5a/Scutlix_Gunner.png/revision/latest?cb=20150701010937&format=original";
				case NPCID.Scutlix://391 Scutlix
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e7/Scutlix_%28creature%29.png/revision/latest?cb=20150701171708&format=original";
				case NPCID.MartianSaucer://392 Martian Saucer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6f/Martian_Saucer.png/revision/latest?cb=20150702004716&format=original";
				case NPCID.MartianSaucerTurret://393 Martian Saucer Turret
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4c/Martian_Saucer_Turret.png/revision/latest?cb=20150701095504&format=original";
				case NPCID.MartianSaucerCannon://394 Martian Saucer Cannon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Martian_Saucer_Cannon.png/revision/latest?cb=20150701095409&format=original";
				case NPCID.MartianSaucerCore://395 Martian Saucer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/cf/Martian_Saucer_Core.png/revision/latest?cb=20150701010958&format=original";
				case NPCID.MoonLordHead://396 Moon Lord
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/84/Moon_Lord%27s_Head.gif/revision/latest?cb=20160712055422&format=original";
				case NPCID.MoonLordHand://397 Moon Lord's Hand
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/11/Moon_Lord%27s_Hand.gif/revision/latest?cb=20160712053505&format=original";
				case NPCID.MoonLordCore://398 Moon Lord's Core
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ea/Moon_Lord%27s_Core.gif/revision/latest?cb=20160712055841&format=original";
				case NPCID.MartianProbe://399 Martian Probe
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f8/Martian_Probe.png/revision/latest?cb=20150710011512&format=original";
				case NPCID.MoonLordFreeEye://400 True Eye of Cthulhu
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/56/True_Eye_of_Cthulhu.png/revision/latest?cb=20150804093912&format=original";
				case NPCID.MoonLordLeechBlob://401 Moon Leech Clot
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/16/Moon_Leech_Clot.png/revision/latest?cb=20150701095300&format=original";
				case NPCID.StardustWormHead://402 Milkyway Weaver
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0b/Milkyway_Weaver_%28Head%29.png/revision/latest?cb=20150701095415&format=original";
				case NPCID.StardustWormBody://403 NPCName.StardustWormBody
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/91/Milkyway_Weaver_%28Body%29.png/revision/latest?cb=20150701095433&format=original";
				case NPCID.StardustWormTail://404 NPCName.StardustWormTail
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/38/Milkyway_Weaver_%28Tail%29.png/revision/latest?cb=20150701095427&format=original";
				case NPCID.StardustCellBig://405 Star Cell
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/43/Star_Cell.png/revision/latest?cb=20150701095341&format=original";
				case NPCID.StardustCellSmall://406 Star Cell
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/43/Star_Cell.png/revision/latest?cb=20150701095341&format=original";
				case NPCID.StardustJellyfishBig://407 Flow Invader
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/95/Flow_Invader.png/revision/latest?cb=20150701141020&format=original";
				case NPCID.StardustJellyfishSmall://408 NPCName.StardustJellyfishSmall
					return "";
				case NPCID.StardustSpiderBig://409 Twinkle Popper
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4c/Twinkle_Popper.png/revision/latest?cb=20150701095448&format=original";
				case NPCID.StardustSpiderSmall://410 Twinkle
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Twinkle.png/revision/latest?cb=20150701095454&format=original";
				case NPCID.StardustSoldier://411 Stargazer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2b/Stargazer.png/revision/latest?cb=20150701135045&format=original";
				case NPCID.SolarCrawltipedeHead://412 Crawltipede
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/61/Crawltipede_%28Head%29.png/revision/latest?cb=20150701100750&format=original";
				case NPCID.SolarCrawltipedeBody://413 Crawltipede
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b8/Crawltipede_%28Body%29.png/revision/latest?cb=20150701100745&format=original";
				case NPCID.SolarCrawltipedeTail://414 Crawltipede
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/71/Crawltipede_%28Tail%29.png/revision/latest?cb=20150701100755&format=original";
				case NPCID.SolarDrakomire://415 Drakomire
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1b/Drakomire.png/revision/latest?cb=20150701100807&format=original";
				case NPCID.SolarDrakomireRider://416 Drakomire Rider
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/28/Drakomire_Rider.png/revision/latest?cb=20150701100801&format=original";
				case NPCID.SolarSroller://417 Sroller
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/51/Sroller.png/revision/latest?cb=20150701100850&format=original";
				case NPCID.SolarCorite://418 Corite
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/05/Corite.png/revision/latest?cb=20150701100738&format=original";
				case NPCID.SolarSolenian://419 Selenian
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/48/Selenian.png/revision/latest?cb=20150701100845&format=original";
				case NPCID.NebulaBrain://420 Nebula Floater
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5a/Nebula_Floater.png/revision/latest?cb=20150701135041&format=original";
				case NPCID.NebulaHeadcrab://421 Brain Suckler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fb/Brain_Suckler.png/revision/latest?cb=20201017034837&format=original";
				case NPCID.LunarTowerVortex://422 Vortex Pillar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8d/Vortex_Pillar.png/revision/latest?cb=20150701100906&format=original";
				case NPCID.NebulaBeast://423 Evolution Beast
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/86/Evolution_Beast.png/revision/latest?cb=20201017035139&format=original";
				case NPCID.NebulaSoldier://424 Predictor
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/30/Predictor.png/revision/latest?cb=20150701135043&format=original";
				case NPCID.VortexRifleman://425 Storm Diver
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b8/Storm_Diver.png/revision/latest?cb=20150701135046&format=original";
				case NPCID.VortexHornetQueen://426 Alien Queen
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d8/Alien_Queen.png/revision/latest?cb=20150701100644&format=original";
				case NPCID.VortexHornet://427 Alien Hornet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4d/Alien_Hornet.png/revision/latest?cb=20150701100634&format=original";
				case NPCID.VortexLarva://428 Alien Larva
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5d/Alien_Larva.png/revision/latest?cb=20150701100639&format=original";
				case NPCID.VortexSoldier://429 Vortexian
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a0/Vortexian.png/revision/latest?cb=20150701100912&format=original";
				case NPCID.ArmedZombie://430 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d9/Armed_Zombie.png/revision/latest?cb=20170422131025&format=original";
				case NPCID.ArmedZombieEskimo://431 Frozen Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3c/Armed_Frozen_Zombie.png/revision/latest?cb=20170422131256&format=original";
				case NPCID.ArmedZombiePincussion://432 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e4/Armed_Pincushion_Zombie.png/revision/latest?cb=20170422131048&format=original";
				case NPCID.ArmedZombieSlimed://433 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/64/Armed_Slimed_Zombie.png/revision/latest?cb=20170422131112&format=original";
				case NPCID.ArmedZombieSwamp://434 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/48/Armed_Swamp_Zombie.png/revision/latest?cb=20170422131135&format=original";
				case NPCID.ArmedZombieTwiggy://435 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Armed_Twiggy_Zombie.png/revision/latest?cb=20170422131200&format=original";
				case NPCID.ArmedZombieCenx://436 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/2c/Armed_Female_Zombie.png/revision/latest?cb=20170422131225&format=original";
				case NPCID.CultistTablet://437 Mysterious Tablet
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/53/Mysterious_Tablet.gif/revision/latest?cb=20190228020510&format=original";
				case NPCID.CultistDevote://438 Lunatic Devotee
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3a/Lunatic_Devotee.gif/revision/latest?cb=20150814182852&format=original";
				case NPCID.CultistBoss://439 Lunatic Cultist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1c/Lunatic_Cultist.gif/revision/latest?cb=20190125125946&format=original";
				case NPCID.CultistBossClone://440 Lunatic Cultist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/94/Ancient_Cultist.png/revision/latest?cb=20160318233643&format=original";
				case NPCID.TaxCollector://441 Tax Collector
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Tax_Collector.png/revision/latest?cb=20150701011232&format=original";
				case NPCID.GoldBird://442 Gold Bird
					return "";
				case NPCID.GoldBunny://443 Gold Bunny
					return "";
				case NPCID.GoldButterfly://444 Gold Butterfly
					return "";
				case NPCID.GoldFrog://445 Gold Frog
					return "";
				case NPCID.GoldGrasshopper://446 Gold Grasshopper
					return "";
				case NPCID.GoldMouse://447 Gold Mouse
					return "";
				case NPCID.GoldWorm://448 Gold Worm
					return "";
				case NPCID.BoneThrowingSkeleton://449 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/23/Skeleton.png/revision/latest?cb=20170420012637&format=original";
				case NPCID.BoneThrowingSkeleton2://450 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/62/Headache_Skeleton.png/revision/latest?cb=20170422123913&format=original";
				case NPCID.BoneThrowingSkeleton3://451 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7b/Misassembled_Skeleton.png/revision/latest?cb=20170422123940&format=original";
				case NPCID.BoneThrowingSkeleton4://452 Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/be/Pantless_Skeleton.png/revision/latest?cb=20170422124010&format=original";
				case NPCID.SkeletonMerchant://453 Skeleton Merchant
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/37/Skeleton_Merchant.png/revision/latest?cb=20150701011353&format=original";
				case NPCID.CultistDragonHead://454 Phantasm Dragon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/30/Phantasm_Dragon_%28Head%29.png/revision/latest?cb=20150701103037&format=original";
				case NPCID.CultistDragonBody1://455 Phantasm Dragon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d0/Phantasm_Dragon_%28Body1%29.png/revision/latest?cb=20150701103013&format=original";
				case NPCID.CultistDragonBody2://456 Phantasm Dragon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b2/Phantasm_Dragon_%28Body2%29.png/revision/latest?cb=20150701103019&format=original";
				case NPCID.CultistDragonBody3://457 Phantasm Dragon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/53/Phantasm_Dragon_%28Body3%29.png/revision/latest?cb=20150701103024&format=original";
				case NPCID.CultistDragonBody4://458 Phantasm Dragon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4b/Phantasm_Dragon_%28Body4%29.png/revision/latest?cb=20150701103031&format=original";
				case NPCID.CultistDragonTail://459 Phantasm Dragon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d7/Phantasm_Dragon_%28Tail%29.png/revision/latest?cb=20150709000911&format=original";
				case NPCID.Butcher://460 Butcher
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fb/Butcher.png/revision/latest?cb=20150701102715&format=original";
				case NPCID.CreatureFromTheDeep://461 Creature from the Deep
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/91/Creature_from_the_Deep.png/revision/latest?cb=20150629182153&format=original";
				case NPCID.Fritz://462 Fritz
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e8/Fritz.png/revision/latest?cb=20150629214920&format=original";
				case NPCID.Nailhead://463 Nailhead
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/73/Nailhead.png/revision/latest?cb=20150701102948&format=original";
				case NPCID.CrimsonBunny://464 Vicious Bunny
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/99/Vicious_Bunny.png/revision/latest?cb=20171124231053&format=original";
				case NPCID.CrimsonGoldfish://465 Vicious Goldfish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4d/Vicious_Goldfish.png/revision/latest?cb=20150701102758&format=original";
				case NPCID.Psycho://466 Psycho
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9c/Psycho.png/revision/latest?cb=20210620015106&format=original";
				case NPCID.DeadlySphere://467 Deadly Sphere
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6e/Deadly_Sphere.png/revision/latest?cb=20211218000054&format=original";
				case NPCID.DrManFly://468 Dr. Man Fly
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d9/Dr._Man_Fly.png/revision/latest?cb=20150701102809&format=original";
				case NPCID.ThePossessed://469 The Possessed
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/The_Possessed.png/revision/latest?cb=20150629184855&format=original";
				case NPCID.CrimsonPenguin://470 Vicious Penguin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/77/Vicious_Penguin.png/revision/latest?cb=20200730170604&format=original";
				case NPCID.GoblinSummoner://471 Goblin Summoner
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/24/Goblin_Warlock.png/revision/latest?cb=20200708163902&format=original";
				case NPCID.ShadowFlameApparition://472 Shadowflame Apparition
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b1/Shadowflame_Apparition.png/revision/latest?cb=20150701103137&format=original";
				case NPCID.BigMimicCorruption://473 Corrupt Mimic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9c/Corrupt_Mimic2.png/revision/latest?cb=20150723182451&format=original";
				case NPCID.BigMimicCrimson://474 Crimson Mimic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Crimson_Mimic2.png/revision/latest?cb=20150723182523&format=original";
				case NPCID.BigMimicHallow://475 Hallowed Mimic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5e/Hallowed_Mimic2.png/revision/latest?cb=20150723182552&format=original";
				case NPCID.BigMimicJungle://476 Jungle Mimic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6b/Jungle_Mimic2.png/revision/latest?cb=20150723182625&format=original";
				case NPCID.Mothron://477 Mothron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1a/Mothron.png/revision/latest?cb=20150629182437&format=original";
				case NPCID.MothronEgg://478 Mothron Egg
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/14/Mothron_Egg.png/revision/latest?cb=20150701102942&format=original";
				case NPCID.MothronSpawn://479 Baby Mothron
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/87/Baby_Mothron.png/revision/latest?cb=20150701102702&format=original";
				case NPCID.Medusa://480 Medusa
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c3/Medusa.png/revision/latest?cb=20150701102935&format=original";
				case NPCID.GreekSkeleton://481 Hoplite
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6b/Hoplite.png/revision/latest?cb=20171209201110&format=original";
				case NPCID.GraniteGolem://482 Granite Golem
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4e/Granite_Golem.png/revision/latest?cb=20150701102858&format=original";
				case NPCID.GraniteFlyer://483 Granite Elemental
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/04/Granite_Elemental.gif/revision/latest?cb=20150501004036&format=original";
				case NPCID.EnchantedNightcrawler://484 Enchanted Nightcrawler
					return "";
				case NPCID.Grubby://485 Grubby
					return "";
				case NPCID.Sluggy://486 Sluggy
					return "";
				case NPCID.Buggy://487 Buggy
					return "";
				case NPCID.TargetDummy://488 Target Dummy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/06/Target_Dummy_%28placed%29.gif/revision/latest?cb=20150701180254&format=original";
				case NPCID.BloodZombie://489 Blood Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/59/Blood_Zombie.png/revision/latest?cb=20171104201710&format=original";
				case NPCID.Drippler://490 Drippler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/69/Drippler.gif/revision/latest?cb=20200831184238&format=original";
				case NPCID.PirateShip://491 Flying Dutchman
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/ba/Flying_Dutchman.png/revision/latest/scale-to-width-down/260?cb=20180705152731&format=original";
				case NPCID.PirateShipCannon://492 Dutchman Cannon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/ce/Dutchman_Cannon.png/revision/latest?cb=20150701102833&format=original";
				case NPCID.LunarTowerStardust://493 Stardust Pillar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9f/Stardust_Pillar.png/revision/latest?cb=20180125013518&format=original";
				case NPCID.Crawdad://494 Crawdad
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/53/Crawdad.png/revision/latest?cb=20150701102733&format=original";
				case NPCID.Crawdad2://495 Crawdad
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9e/Crawdad2.png/revision/latest?cb=20150701102739&format=original";
				case NPCID.GiantShelly://496 Giant Shelly
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/dd/Giant_Shelly.png/revision/latest?cb=20150701102840&format=original";
				case NPCID.GiantShelly2://497 Giant Shelly
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8d/Giant_Shelly2.png/revision/latest?cb=20150701102846&format=original";
				case NPCID.Salamander://498 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/53/Salamander.png/revision/latest?cb=20150701103048&format=original";
				case NPCID.Salamander2://499 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/00/Salamander_2.png/revision/latest?cb=20180831035120&format=original";
				case NPCID.Salamander3://500 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/88/Salamander_3.png/revision/latest?cb=20180831035112&format=original";
				case NPCID.Salamander4://501 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fb/Salamander_4.png/revision/latest?cb=20180831035103&format=original";
				case NPCID.Salamander5://502 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8a/Salamander_5.png/revision/latest?cb=20180831035036&format=original";
				case NPCID.Salamander6://503 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4e/Salamander_6.png/revision/latest?cb=20180831035047&format=original";
				case NPCID.Salamander7://504 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/75/Salamander_7.png/revision/latest?cb=20180831035142&format=original";
				case NPCID.Salamander8://505 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e9/Salamander_8.png/revision/latest?cb=20180831035151&format=original";
				case NPCID.Salamander9://506 Salamander
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c1/Salamander_9.png/revision/latest?cb=20180831035203&format=original";
				case NPCID.LunarTowerNebula://507 Nebula Pillar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a9/Nebula_Pillar.png/revision/latest?cb=20150701102641&format=original";
				case NPCID.GiantWalkingAntlion://508 Giant Antlion Charger
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/01/Giant_Antlion_Charger.png/revision/latest?cb=20191112012549&format=original";
				case NPCID.GiantFlyingAntlion://509 Giant Antlion Swarmer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9c/Giant_Antlion_Swarmer.png/revision/latest?cb=20150701102655&format=original";
				case NPCID.DuneSplicerHead://510 Dune Splicer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/da/Dune_Splicer_%28Head%29.png/revision/latest?cb=20150701102822&format=original";
				case NPCID.DuneSplicerBody://511 Dune Splicer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ef/Dune_Splicer_%28Body%29.png/revision/latest?cb=20150701102816&format=original";
				case NPCID.DuneSplicerTail://512 Dune Splicer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b6/Dune_Splicer_%28Tail%29.png/revision/latest?cb=20150701102828&format=original";
				case NPCID.TombCrawlerHead://513 Tomb Crawler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8f/Tomb_Crawler_%28Head%29.png/revision/latest?cb=20200530045928&format=original";
				case NPCID.TombCrawlerBody://514 Tomb Crawler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0f/Tomb_Crawler_%28Body%29.png/revision/latest?cb=20200530050002&format=original";
				case NPCID.TombCrawlerTail://515 Tomb Crawler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/70/Tomb_Crawler_%28Tail%29.png/revision/latest?cb=20200530045949&format=original";
				case NPCID.SolarFlare://516 Solar Flare
					return "";
				case NPCID.LunarTowerSolar://517 Solar Pillar
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/50/Solar_Pillar.png/revision/latest?cb=20180125005222&format=original";
				case NPCID.SolarSpearman://518 Drakanian
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f2/Drakanian.png/revision/latest?cb=20150701103316&format=original";
				case NPCID.SolarGoop://519 Solar Fragment
					return "";
				case NPCID.MartianWalker://520 Martian Walker
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f1/Martian_Walker.png/revision/latest?cb=20150701103346&format=original";
				case NPCID.AncientCultistSquidhead://521 Ancient Vision
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e0/Ancient_Vision.png/revision/latest?cb=20150701103258&format=original";
				case NPCID.AncientLight://522 Ancient Light
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/12/Ancient_Light.png/revision/latest?cb=20150701103252&format=original";
				case NPCID.AncientDoom://523 Ancient Doom
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/26/Ancient_Doom.png/revision/latest?cb=20150701103247&format=original";
				case NPCID.DesertGhoul://524 Ghoul
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e3/Ghoul.png/revision/latest?cb=20150701103329&format=original";
				case NPCID.DesertGhoulCorruption://525 Vile Ghoul
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/fa/Vile_Ghoul.png/revision/latest?cb=20150701103435&format=original";
				case NPCID.DesertGhoulCrimson://526 Tainted Ghoul
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/89/Tainted_Ghoul.png/revision/latest?cb=20150701103417&format=original";
				case NPCID.DesertGhoulHallow://527 Dreamer Ghoul
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/01/Dreamer_Ghoul.png/revision/latest?cb=20150701103323&format=original";
				case NPCID.DesertLamiaLight://528 Lamia
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Lamia.png/revision/latest?cb=20150701103335&format=original";
				case NPCID.DesertLamiaDark://529 Lamia
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e5/Lamia2.png/revision/latest?cb=20150701103341&format=original";
				case NPCID.DesertScorpionWalk://530 Sand Poacher
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1e/Sand_Poacher.png/revision/latest?cb=20150701103353&format=original";
				case NPCID.DesertScorpionWall://531 Sand Poacher
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/9d/Sand_Poacher2.png/revision/latest?cb=20150701103358&format=original";
				case NPCID.DesertBeast://532 Basilisk
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b1/Basilisk.png/revision/latest?cb=20150701103303&format=original";
				case NPCID.DesertDjinn://533 Desert Spirit
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/35/Desert_Spirit.png/revision/latest?cb=20150701103311&format=original";
				case NPCID.DemonTaxCollector://534 Tortured Soul
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/83/Tortured_Soul.png/revision/latest?cb=20150701103428&format=original";
				case NPCID.SlimeSpiked://535 Spiked Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/10/Spiked_Slime.png/revision/latest?cb=20150701103411&format=original";
				case NPCID.TheBride://536 The Bride
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3f/The_Bride.png/revision/latest?cb=20210620013648&format=original";
				case NPCID.SandSlime://537 Sand Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f2/Sand_Slime.png/revision/latest?cb=20160224071115&format=original";
				case NPCID.SquirrelRed://538 Red Squirrel
					return "";
				case NPCID.SquirrelGold://539 Gold Squirrel
					return "";
				case NPCID.PartyBunny://540 Bunny
					return "";
				case NPCID.SandElemental://541 Sand Elemental
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/aa/Sand_Elemental.png/revision/latest?cb=20160909175739&format=original";
				case NPCID.SandShark://542 Sand Shark
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/95/Sand_Shark.png/revision/latest?cb=20160909154452&format=original";
				case NPCID.SandsharkCorrupt://543 Bone Biter
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/7c/Bone_Biter.png/revision/latest?cb=20160909201152&format=original";
				case NPCID.SandsharkCrimson://544 Flesh Reaver
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/85/Flesh_Reaver.png/revision/latest?cb=20160909201148&format=original";
				case NPCID.SandsharkHallow://545 Crystal Thresher
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5a/Crystal_Thresher.png/revision/latest?cb=20160909201150&format=original";
				case NPCID.Tumbleweed://546 Angry Tumbler
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/40/Angry_Tumbler.png/revision/latest?cb=20190205230614&format=original";
				case NPCID.DD2AttackerTest://547 ???
					return "";
				case NPCID.DD2EterniaCrystal://548 Eternia Crystal
					return "";
				case NPCID.DD2LanePortal://549 Mysterious Portal
					return "";
				case NPCID.DD2Bartender://550 Tavernkeep
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/81/Tavernkeep.png/revision/latest?cb=20161115191006&format=original";
				case NPCID.DD2Betsy://551 Betsy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/ec/Betsy.png/revision/latest?cb=20161117001359&format=original";
				case NPCID.DD2GoblinT1://552 Etherian Goblin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8c/Etherian_Goblin.png/revision/latest?cb=20161116123728&format=original";
				case NPCID.DD2GoblinT2://553 Etherian Goblin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/42/Etherian_Goblin_2.png/revision/latest?cb=20161116123746&format=original";
				case NPCID.DD2GoblinT3://554 Etherian Goblin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/3a/Etherian_Goblin_3.png/revision/latest?cb=20161116123808&format=original";
				case NPCID.DD2GoblinBomberT1://555 Etherian Goblin Bomber
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/79/Etherian_Goblin_Bomber.png/revision/latest?cb=20161116124106&format=original";
				case NPCID.DD2GoblinBomberT2://556 Etherian Goblin Bomber
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5c/Etherian_Goblin_Bomber_2.png/revision/latest?cb=20161116124101&format=original";
				case NPCID.DD2GoblinBomberT3://557 Etherian Goblin Bomber
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/21/Etherian_Goblin_Bomber_3.png/revision/latest?cb=20161116124109&format=original";
				case NPCID.DD2WyvernT1://558 Etherian Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/8b/Etherian_Wyvern.png/revision/latest?cb=20210625221330&format=original";
				case NPCID.DD2WyvernT2://559 Etherian Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/34/Etherian_Wyvern_2.png/revision/latest?cb=20210625221449&format=original";
				case NPCID.DD2WyvernT3://560 Etherian Wyvern
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/14/Etherian_Wyvern_3.png/revision/latest?cb=20210625221540&format=original";
				case NPCID.DD2JavelinstT1://561 Etherian Javelin Thrower
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/06/Etherian_Javelin_Thrower.png/revision/latest?cb=20161116125303&format=original";
				case NPCID.DD2JavelinstT2://562 Etherian Javelin Thrower
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/55/Etherian_Javelin_Thrower_2.png/revision/latest?cb=20161116125322&format=original";
				case NPCID.DD2JavelinstT3://563 Etherian Javelin Thrower
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/ae/Etherian_Javelin_Thrower_3.png/revision/latest?cb=20161116125327&format=original";
				case NPCID.DD2DarkMageT1://564 Dark Mage
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/33/Dark_Mage.png/revision/latest?cb=20161116171947&format=original";
				case NPCID.DD2DarkMageT3://565 Dark Mage
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/3/33/Dark_Mage.png/revision/latest?cb=20161116171947&format=original";
				case NPCID.DD2SkeletonT1://566 Old One's Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f5/Old_One%27s_Skeleton.png/revision/latest?cb=20161116124228&format=original";
				case NPCID.DD2SkeletonT3://567 Old One's Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f5/Old_One%27s_Skeleton.png/revision/latest?cb=20161116124228&format=original";
				case NPCID.DD2WitherBeastT2://568 Wither Beast
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/00/Wither_Beast.png/revision/latest?cb=20161116124330&format=original";
				case NPCID.DD2WitherBeastT3://569 Wither Beast
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/00/Wither_Beast.png/revision/latest?cb=20161116124330&format=original";
				case NPCID.DD2DrakinT2://570 Drakin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f5/Drakin.png/revision/latest?cb=20161116124454&format=original";
				case NPCID.DD2DrakinT3://571 Drakin
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0e/Drakin_2.png/revision/latest?cb=20161116124450&format=original";
				case NPCID.DD2KoboldWalkerT2://572 Kobold
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d2/Kobold.png/revision/latest?cb=20161116125025&format=original";
				case NPCID.DD2KoboldWalkerT3://573 Kobold
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d2/Kobold.png/revision/latest?cb=20161116125025&format=original";
				case NPCID.DD2KoboldFlyerT2://574 Kobold Glider
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/49/Kobold_Glider.png/revision/latest?cb=20161116123143&format=original";
				case NPCID.DD2KoboldFlyerT3://575 Kobold Glider
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/49/Kobold_Glider.png/revision/latest?cb=20161116123143&format=original";
				case NPCID.DD2OgreT2://576 Ogre
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c5/Ogre.png/revision/latest?cb=20161115192150&format=original";
				case NPCID.DD2OgreT3://577 Ogre
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c5/Ogre.png/revision/latest?cb=20161115192150&format=original";
				case NPCID.DD2LightningBugT3://578 Etherian Lightning Bug
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1d/Etherian_Lightning_Bug.png/revision/latest?cb=20161116123152&format=original";
				case NPCID.BartenderUnconscious://579 Unconscious Man
					return "";
				case NPCID.WalkingAntlion://580 Antlion Charger
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6c/Antlion_Charger.png/revision/latest?cb=20200517034253&format=original";
				case NPCID.FlyingAntlion://581 Antlion Swarmer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/11/Antlion_Swarmer.png/revision/latest?cb=20200521230735&format=original";
				case NPCID.LarvaeAntlion://582 Antlion Larva
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c8/Antlion_Larva.png/revision/latest?cb=20200517034039&format=original";
				case NPCID.FairyCritterPink://583 Pink Fairy
					return "";
				case NPCID.FairyCritterGreen://584 Green Fairy
					return "";
				case NPCID.FairyCritterBlue://585 Blue Fairy
					return "";
				case NPCID.ZombieMerman://586 Zombie Merman
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/d/d9/Zombie_Merman.png/revision/latest?cb=20220305001421&format=original";
				case NPCID.EyeballFlyingFish://587 Wandering Eye Fish
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/53/Wandering_Eye_Fish.png/revision/latest?cb=20200516195725&format=original";
				case NPCID.Golfer://588 Golfer
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1a/Golfer.png/revision/latest?cb=20200516183144&format=original";
				case NPCID.GolferRescue://589 Golfer
					return "";
				case NPCID.TorchZombie://590 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f8/Torch_Zombie.png/revision/latest?cb=20200516195725&format=original";
				case NPCID.ArmedTorchZombie://591 Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f6/Armed_Torch_Zombie.png/revision/latest?cb=20200516195727&format=original";
				case NPCID.GoldGoldfish://592 Gold Goldfish
					return "";
				case NPCID.GoldGoldfishWalker://593 Gold Goldfish
					return "";
				case NPCID.WindyBalloon://594 Windy Balloon
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/1a/Windy_Balloon.png/revision/latest?cb=20200516200240&format=original";
				case NPCID.BlackDragonfly://595 Dragonfly
					return "";
				case NPCID.BlueDragonfly://596 Dragonfly
					return "";
				case NPCID.GreenDragonfly://597 Dragonfly
					return "";
				case NPCID.OrangeDragonfly://598 Dragonfly
					return "";
				case NPCID.RedDragonfly://599 Dragonfly
					return "";
				case NPCID.YellowDragonfly://600 Dragonfly
					return "";
				case NPCID.GoldDragonfly://601 Gold Dragonfly
					return "";
				case NPCID.Seagull://602 Seagull
					return "";
				case NPCID.Seagull2://603 Seagull
					return "";
				case NPCID.LadyBug://604 Ladybug
					return "";
				case NPCID.GoldLadyBug://605 Gold Ladybug
					return "";
				case NPCID.Maggot://606 Maggot
					return "";
				case NPCID.Pupfish://607 Pupfish
					return "";
				case NPCID.Grebe://608 Grebe
					return "";
				case NPCID.Grebe2://609 Grebe
					return "";
				case NPCID.Rat://610 Rat
					return "";
				case NPCID.Owl://611 Owl
					return "";
				case NPCID.WaterStrider://612 Water Strider
					return "";
				case NPCID.GoldWaterStrider://613 Gold Water Strider
					return "";
				case NPCID.ExplosiveBunny://614 Explosive Bunny
					return "";
				case NPCID.Dolphin://615 Dolphin
					return "";
				case NPCID.Turtle://616 Turtle
					return "";
				case NPCID.TurtleJungle://617 Jungle Turtle
					return "";
				case NPCID.BloodNautilus://618 Dreadnautilus
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/59/Dreadnautilus.png/revision/latest?cb=20200517123621&format=original";
				case NPCID.BloodSquid://619 Blood Squid
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a3/Blood_Squid.png/revision/latest?cb=20200516201131&format=original";
				case NPCID.GoblinShark://620 Hemogoblin Shark
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/a9/Hemogoblin_Shark.png/revision/latest?cb=20200516201155&format=original";
				case NPCID.BloodEelHead://621 Blood Eel
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/6c/Blood_Eel_Head.png/revision/latest?cb=20200516201211&format=original";
				case NPCID.BloodEelBody://622 Blood Eel
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/0/0e/Blood_Eel_Body.png/revision/latest?cb=20200516201210&format=original";
				case NPCID.BloodEelTail://623 Blood Eel
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/bb/Blood_Eel_Tail.png/revision/latest?cb=20200516201212&format=original";
				case NPCID.Gnome://624 Gnome
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/78/Gnome.png/revision/latest?cb=20200516203041&format=original";
				case NPCID.SeaTurtle://625 Sea Turtle
					return "";
				case NPCID.Seahorse://626 Seahorse
					return "";
				case NPCID.GoldSeahorse://627 Gold Seahorse
					return "";
				case NPCID.Dandelion://628 Angry Dandelion
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/e/e2/Angry_Dandelion.png/revision/latest?cb=20200516192425&format=original";
				case NPCID.IceMimic://629 Ice Mimic
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/1/18/Ice_Mimic.png/revision/latest?cb=20170421213009&format=original";
				case NPCID.BloodMummy://630 Blood Mummy
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/2/28/Blood_Mummy.png/revision/latest?cb=20200516192426&format=original";
				case NPCID.RockGolem://631 Rock Golem
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/80/Rock_Golem.png/revision/latest?cb=20200516192425&format=original";
				case NPCID.MaggotZombie://632 Maggot Zombie
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/a/ab/Maggot_Zombie.png/revision/latest?cb=20200516193041&format=original";
				case NPCID.BestiaryGirl://633 Zoologist
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/6/61/Zoologist.png/revision/latest?cb=20200516192903&format=original";
				case NPCID.SporeBat://634 Spore Bat
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b7/Spore_Bat.png/revision/latest?cb=20200516193907&format=original";
				case NPCID.SporeSkeleton://635 Spore Skeleton
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/b/b5/Spore_Skeleton.png/revision/latest?cb=20200516193020&format=original";
				case NPCID.HallowBoss://636 Empress of Light
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/c/c9/Empress_of_Light.gif/revision/latest?cb=20210501121017&format=original";
				case NPCID.TownCat://637 Cat
					return "";
				case NPCID.TownDog://638 Dog
					return "";
				case NPCID.GemSquirrelAmethyst://639 Amethyst Squirrel
					return "";
				case NPCID.GemSquirrelTopaz://640 Topaz Squirrel
					return "";
				case NPCID.GemSquirrelSapphire://641 Sapphire Squirrel
					return "";
				case NPCID.GemSquirrelEmerald://642 Emerald Squirrel
					return "";
				case NPCID.GemSquirrelRuby://643 Ruby Squirrel
					return "";
				case NPCID.GemSquirrelDiamond://644 Diamond Squirrel
					return "";
				case NPCID.GemSquirrelAmber://645 Amber Squirrel
					return "";
				case NPCID.GemBunnyAmethyst://646 Amethyst Bunny
					return "";
				case NPCID.GemBunnyTopaz://647 Topaz Bunny
					return "";
				case NPCID.GemBunnySapphire://648 Sapphire Bunny
					return "";
				case NPCID.GemBunnyEmerald://649 Emerald Bunny
					return "";
				case NPCID.GemBunnyRuby://650 Ruby Bunny
					return "";
				case NPCID.GemBunnyDiamond://651 Diamond Bunny
					return "";
				case NPCID.GemBunnyAmber://652 Amber Bunny
					return "";
				case NPCID.HellButterfly://653 Hell Butterfly
					return "";
				case NPCID.Lavafly://654 Lavafly
					return "";
				case NPCID.MagmaSnail://655 Magma Snail
					return "";
				case NPCID.TownBunny://656 Bunny
					return "";
				case NPCID.QueenSlimeBoss://657 Queen Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/7/79/Queen_Slime.png/revision/latest?cb=20200524022713&format=original";
				case NPCID.QueenSlimeMinionBlue://658 Crystal Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/4/4f/Crystal_Slime.png/revision/latest?cb=20200516194836&format=original";
				case NPCID.QueenSlimeMinionPink://659 Bouncy Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5a/Bouncy_Slime.png/revision/latest?cb=20200516194817&format=original";
				case NPCID.QueenSlimeMinionPurple://660 Heavenly Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/81/Heavenly_Slime.png/revision/latest?cb=20200712033312&format=original";
				case NPCID.EmpressButterfly://661 Prismatic Lacewing
					return "";
				case NPCID.PirateGhost://662 Pirate's Curse
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/59/Pirate%27s_Curse.png/revision/latest?cb=20200730145829&format=original";
				case NPCID.Princess://663 Princess
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f2/Princess.png/revision/latest?cb=20201013172546&format=original";
				case NPCID.TorchGod://664 The Torch God
					return "";
				case NPCID.ChaosBallTim://665 Chaos Ball
					return "";
				case NPCID.VileSpitEaterOfWorlds://666 Vile Spit
					return "";
				case NPCID.GoldenSlime://667 Golden Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/f/f9/Golden_Slime.png/revision/latest?cb=20210516134412&format=original";
				case NPCID.Deerclops://668 Deerclops
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/8/85/Deerclops.png/revision/latest?cb=20211118192944&format=original";
				case NPCID.Stinkbug://669 Stinkbug
					return "";
				case NPCID.TownSlimeBlue://670 Nerdy Slime
					return "";
				case NPCID.ScarletMacaw://671 Scarlet Macaw
					return "";
				case NPCID.BlueMacaw://672 Blue Macaw
					return "";
				case NPCID.Toucan://673 Toucan
					return "";
				case NPCID.YellowCockatiel://674 Yellow Cockatiel
					return "";
				case NPCID.GrayCockatiel://675 Gray Cockatiel
					return "";
				case NPCID.ShimmerSlime://676 Shimmering Slime
					return "https://static.wikia.nocookie.net/terraria_gamepedia/images/9/96/Shimmer_Slime.png/revision/latest?cb=20221028202234&format=original";
				case NPCID.Shimmerfly://677 Faeling
					return "";
				case NPCID.TownSlimeGreen://678 Cool Slime
					return "";
				case NPCID.TownSlimeOld://679 Elder Slime
					return "";
				case NPCID.TownSlimePurple://680 Clumsy Slime
					return "";
				case NPCID.TownSlimeRainbow://681 Diva Slime
					return "";
				case NPCID.TownSlimeRed://682 Surly Slime
					return "";
				case NPCID.TownSlimeYellow://683 Mystic Slime
					return "";
				case NPCID.TownSlimeCopper://684 Squire Slime
					return "";
				case NPCID.BoundTownSlimeOld://685 Old Shaking Chest
					return "";
				case NPCID.BoundTownSlimePurple://686 Clumsy Balloon Slime
					return "";
				case NPCID.BoundTownSlimeYellow://687 Mystic Frog
					return "";
				default:
					return "";
			}
		}
		public static string GetModNpcLink(this string modNpcFullName) {
			switch (modNpcFullName) {
				case "CalamityMod/DesertScourgeHead":
					return "https://calamitymod.wiki.gg/wiki/Desert_Scourge";
				case "ThoriumMod/TheGrandThunderBird":
					return "https://thoriummod.wiki.gg/wiki/The_Grand_Thunder_Bird";
				case "ThoriumMod/BuriedChampion":
					return "https://thoriummod.wiki.gg/wiki/Buried_Champion";
				case "ThoriumMod/GraniteEnergyStorm":
					return "https://thoriummod.wiki.gg/wiki/Granite_Energy_Storm";
				case "ThoriumMod/QueenJellyfish":
					return "https://thoriummod.wiki.gg/wiki/Queen_Jellyfish";
				default:
					return "";
			}
		}
	}
    public enum GameModeNameID {
		Normal,
		Expert,
		Master,
		Journey
	}
	public static class GameModeMethods {
		public static string ToGameModeIDName(this short id) {
			switch (id) {
				case GameModeID.Normal:
					return GameModeNameID.Normal.ToString().Lang(AndroMod.ModName, L_ID1.GameModeNameIDs);
				case GameModeID.Expert:
					return GameModeNameID.Expert.ToString().Lang(AndroMod.ModName, L_ID1.GameModeNameIDs);
				case GameModeID.Master:
					return GameModeNameID.Master.ToString().Lang(AndroMod.ModName, L_ID1.GameModeNameIDs);
				case GameModeID.Creative:
					return GameModeNameID.Journey.ToString().Lang(AndroMod.ModName, L_ID1.GameModeNameIDs);
				default:
					return "";
			}
		}
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
			if (lowerName.EndsWith("trophy")) {
				if (item.useStyle == ItemUseStyleID.Swing
				&& item.useTurn == true
				&& item.autoReuse == true
				&& item.consumable == true
				&& item.createTile > -1
				&& item.value == bossTrophyValue
				&& item.rare == ItemRarityID.Blue
				) {
					return true;
				}
			}

			return false;
		}
		public static bool IsBossRelic(this Item item, string lowerName = null) {
			if (item.NullOrAir())
				return false;

			if (lowerName == null)
				lowerName = item.GetItemInternalName().ToLower();

			if (lowerName.EndsWith("mastertrophy")
				&& item.useStyle == ItemUseStyleID.Swing
				&& item.useTurn == true
				&& item.autoReuse == true
				&& item.consumable == true
				&& item.createTile > -1
				&& item.rare == ItemRarityID.Master
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

					for (int i = TileID.Count; i < TileLoader.TileCount; i++) {
						ModTile modTile = TileLoader.GetTile(i);
						if (modTile == null)
							continue;

						int[] adjTiles = modTile.AdjTiles;
						for (int j = 0; j < adjTiles.Length; j++) {
							if (requiredTiles.Contains(adjTiles[j])) {
								requiredTiles.Add(i);
								break;
							}
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
