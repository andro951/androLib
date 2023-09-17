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
		Config,
		UIText,
		StorageText,
		MagicStorageButtonsText,
		GameMessages
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
	}
	public enum MagicStorageButtonsTextID {
		DepositAllFromVacuumBags
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
			{ Light, ItemSets.IsTorch },
			{ Hammer, (item) => item.hammer > 0 },
			{ Axe, (item) => item.axe > 0 },
			{ Pickaxe, (item) => item.pick > 0 },
			{ WetLight, (item) => ItemSets.IsWaterTorch(item) || ItemSets.IsGlowstick(item) },
			{ WetLongDistanceThrow, ItemSets.IsGlowstick },
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
		public static bool IsBanner(this Item item, out int banner) => IsBannerItem(item.NullOrAir() ? item.type : ItemID.None, out banner);
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
				if (bannerItemID >= 0 && bannerItemID < ItemLoader.ItemCount) {
					if (!itemToBanner.TryAdd(bannerItemID, bannerID) && Debugger.IsAttached)
						$"banner {bannerID} already exists.  current [{itemToBanner[bannerItemID].GetItemIDOrName()}], new [{bannerItemID.GetItemIDOrName()}, npc: {npc.S()}]".LogSimple();
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
	}
}
