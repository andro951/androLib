using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using androLib.Common.Utility;
using static androLib.Common.Globals.AndroGlobalNPC;
using androLib.Common.Globals;

namespace androLib.Common
{
	public class OneFromWeightedOptionsNotScaledWithLuckDropRule : IItemDropRule
	{
		public List<WeightedPair> dropsList;
		public float dropChance;

		public List<IItemDropRuleChainAttempt> ChainedRules {
			get;
			private set;
		}

		public OneFromWeightedOptionsNotScaledWithLuckDropRule(float chance, IEnumerable<DropData> options, ChanceMultiplier chanceMultiplier) {
			if (chanceMultiplier == null)
				chanceMultiplier = DefaultChanceMultiplier;

			dropChance = chance;
			dropsList = new();
			float totalWeight = 0f;
			float above1TotalWeight = 0f;
			float totalChanceMultiplier = 0f;
			foreach(DropData dropData in options) {
				float originalWeight = dropData.Weight;
				if (chanceMultiplier(dropData.ID, out float mult))
					dropData.UpdateWeight(mult);

				totalChanceMultiplier += mult;
				if (dropData.Weight > 0f) {
					dropsList.Add(new(dropData));
					totalWeight += dropData.Weight;
					//If any weight is over 1, track the extra.  The goal is for values past 1 to only affect the drop chance of that item, not the others.
					if (originalWeight > 1f)
						above1TotalWeight += originalWeight - 1f;
				}
			}

			if (dropsList.Count > 0) {
				if (totalWeight > 0f) {
					//Normalize weights
					for (int i = 0; i < dropsList.Count; i++) {
						WeightedPair pair = dropsList[i];
						pair.Weight /= totalWeight;
						dropsList[i] = pair;
					}
				}

				if (dropsList.Count == 1) {
					dropChance *= totalWeight;
				}
				else {
					//Make room in the drop chance for the weights over 1
					float avgChanceMultiplier = totalChanceMultiplier / dropsList.Count;
					float mult = 1f + above1TotalWeight / dropsList.Count;
					dropChance *= mult * avgChanceMultiplier;
					dropChance.Clamp();
				}
			}
			else {
				dropChance = 0f;
			}

			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info) => true;

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info) {
			ItemDropAttemptResult result;
			int item = dropsList.GetOneFromWeightedList(dropChance);
			if (item > 0) {
				CommonCode.DropItem(info, item, 1);
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;

				return result;
			}

			result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.FailedRandomRoll;

			return result;
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo) {
			float parentDropChance = dropChance * ratesInfo.parentDroprateChance;
			foreach(WeightedPair pair in dropsList) {
				float chance = parentDropChance * pair.Weight;
				chance.Clamp();
				drops.Add(new DropRateInfo(pair.ID, 1, 1, chance, ratesInfo.conditions));
			}

			Chains.ReportDroprates(ChainedRules, dropChance, drops, ratesInfo);
		}
	}
}
