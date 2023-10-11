using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using androLib.Common.Utility;

namespace androLib.Common
{
	public class BasicDropRule : IItemDropRule
	{
		public int itemID;
		public float dropChance;

		public List<IItemDropRuleChainAttempt> ChainedRules {
			get;
			private set;
		}

		public BasicDropRule(int id, float chance, float configChance) {
			itemID = id;
			dropChance = chance * configChance;
			dropChance.Clamp();

			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}
		public BasicDropRule(DropData dropData, float configChance) {
			itemID = dropData.ID;
			dropChance = dropData.Chance * configChance;
			dropChance.Clamp();

			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public virtual bool CanDrop(DropAttemptInfo info) => true;

		public virtual ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info) {
			ItemDropAttemptResult result = new();
			float randFloat = Main.rand.NextFloat();
			if (randFloat <= dropChance) {
				CommonCode.DropItem(info, itemID, 1);
				result.State = ItemDropAttemptResultState.Success;

				return result;
			}

			result.State = ItemDropAttemptResultState.FailedRandomRoll;

			return result;
		}

		public virtual void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo) {
			float chance = dropChance * ratesInfo.parentDroprateChance;
			DropRateInfo dropRateInfo = new DropRateInfo(itemID, 1, 1, chance);
			if (Conditions != null) {
				foreach (IItemDropRuleCondition condition in Conditions) {
					dropRateInfo.AddCondition(condition);
				}
			}

			drops.Add(dropRateInfo);
			Chains.ReportDroprates(ChainedRules, chance, drops, ratesInfo);
		}

		protected virtual IEnumerable<IItemDropRuleCondition> Conditions => null;
	}
}
