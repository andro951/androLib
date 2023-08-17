using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	public enum CommonUITextID {
		Search
	}

	/*
	public enum L_ID1_C {
		UIText
	}
	*/


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
		UIText
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
}
