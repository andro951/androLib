using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using androLib.Common.Utility;

namespace androLib.Common.Globals
{
    public class GlobalCrates : GlobalItem {
        public static SortedDictionary<int, List<DropData>> crateDrops = new();
		
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
            if (crateDrops.ContainsKey(item.type)) {
                float chance = GetCrateEnchantmentDropChance(item.type);
				List<IItemDropRule> dropRules = AndroGlobalNPC.GetDropRules(chance, crateDrops[item.type], AndroModSystem.TryGetCrateSpawnChanceMultiplier);
				foreach (IItemDropRule rule in dropRules) {
					itemLoot.Add(rule);
				}
            }
        }

        public static float GetCrateEnchantmentDropChance(int id) {
            float chance;
			switch (id) {
                case ItemID.WoodenCrate:
                case ItemID.IronCrate:
                case ItemID.GoldenCrate:
                    chance = (float)(id - ItemID.WoodenCrate + 4f) / 20f;
                    break;
                case ItemID.WoodenCrateHard:
                case ItemID.IronCrateHard:
                case ItemID.GoldenCrateHard:
                    chance = (float)(id - ItemID.WoodenCrateHard + 6f) / 20f;
                    break;
                case ItemID.LockBox:
                    chance = 0.5f;
                    break;
                case ItemID.ObsidianLockbox:
                    chance = 1f;
                    break;
                case < ItemID.WoodenCrateHard:
                case ItemID.FrozenCrate:
                case ItemID.OasisCrate:
                case ItemID.LavaCrate:
                case ItemID.OceanCrate:
                    chance = 0.25f;
                    break;
                default:
                    chance = 0.35f;
                    break;
			}

            if (crateDrops[id].Count == 1)
                chance *= crateDrops[id][0].Weight;

            return chance;
        }
    }
}
