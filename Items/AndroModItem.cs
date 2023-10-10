using androLib.Common.Globals;
using androLib.Common.Utility;
using androLib.Items.Interfaces;
using androLib.Localization;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace androLib.Items
{
	public abstract class AndroModItem : ModItem {
		public abstract List<WikiTypeID> WikiItemTypes { get; }
		public abstract string Artist { get; }
		public virtual string ArtModifiedBy => null;
		public abstract string Designer { get; }
		public virtual string WikiDescription => null;
		public virtual string LocalizationTooltip { protected set; get; }
		public virtual bool ConfigOnlyDrop => false;
		public virtual string LocalizationDisplayName => null;
		protected string localizationTooltip;
		protected abstract Action<ModItem, string, string> AddLocalizationTooltipFunc { get; }
		public virtual bool IsEquivenantForCondensingWikiCraftingRecipes(ModItem other) => other.Type == Type;
		public virtual Type GroupingType => GetType().BaseType;
		public override void SetStaticDefaults() {
			if (Tooltip != LocalizedText.Empty)
				AddLocalizationTooltipFunc(this, LocalizationTooltip, LocalizationDisplayName);

			if (this is IHasDropRates hasDropRates) {
				if (hasDropRates.NpcDropTypes != null) {
					foreach (DropData dropData in hasDropRates.NpcDropTypes) {
						int npcType = dropData.ID;
						DropData enchantmentDropData = new(Type, dropData.Weight, dropData.Chance);
						AndroGlobalNPC.npcDropTypes.AddOrCombine(npcType, enchantmentDropData);
					}
				}

				if (hasDropRates.ModNpcDropNames != null) {
					foreach (ModDropData dropData in hasDropRates.ModNpcDropNames) {
						string modNpcName = dropData.Name;
						DropData enchantmentDropData = new(Type, dropData.Weight, dropData.Chance);
						AndroGlobalNPC.modNpcDropNames.AddOrCombine(modNpcName, enchantmentDropData);
					}
				}

				if (hasDropRates.NpcAIDrops != null) {
					foreach (DropData dropData in hasDropRates.NpcAIDrops) {
						int npcAIStyle = dropData.ID;
						DropData enchantmentDropData = new(Type, dropData.Weight, dropData.Chance);
						AndroGlobalNPC.npcAIDrops.AddOrCombine(npcAIStyle, enchantmentDropData);
					}
				}

				if (hasDropRates.ChestDrops != null) {
					foreach (DropData dropData in hasDropRates.ChestDrops) {
						ChestID chestID = (ChestID)dropData.ID;
						DropData enchantmentDropData = new(Type, dropData.Weight, dropData.Chance);
						AndroModSystem.chestDrops.AddOrCombine(chestID, enchantmentDropData);
					}
				}

				if (hasDropRates.CrateDrops != null) {
					foreach (DropData dropData in hasDropRates.CrateDrops) {
						int crateID = dropData.ID;
						DropData enchantmentDropData = new(Type, dropData.Weight, dropData.Chance);
						GlobalCrates.crateDrops.AddOrCombine(crateID, enchantmentDropData);
					}
				}
			}
		}
	}
}
