using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.Localization.GameCulture;
using Terraria.GameContent.Personalities;
using Terraria.ID;

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
		StorageText
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
}
