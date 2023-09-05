using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace androLib.Common.Utility
{
	public static class BossBagsData
	{
		public static bool qwertyModEnabled = ModLoader.TryGetMod("QwertyMod", out Mod _);

		private static SortedSet<int> bossBags = null;
		public static SortedSet<int> BossBags {
			get {
				if (bossBags == null)
					SetupModBossBagIntegration();

				return bossBags;
			}
		}
		private static void SetupModBossBagIntegration() {
			bossBags = new();
			SortedDictionary<string, int> supportedNPCsThatDropBags = new SortedDictionary<string, int>();
			SortedSet<int> bossBagsNotIncludedInItemIdSets = new();
			if (qwertyModEnabled)
				bossBagsNotIncludedInItemIdSets.UnionWith(ContentSamples.ItemsByType.Where(p => p.Key > ItemID.Count).Select(p => p.Value.ModItem).Where(m => m != null && m.Mod.Name == "QwertyMod" && m.FullName.Contains("Bag")).Select(m => m.Type));

			for (int i = 0; i < ItemLoader.ItemCount; i++) {
				Item sampleItem = ContentSamples.ItemsByType[i];
				bool itemIsBossBag = ItemID.Sets.BossBag[i] || bossBagsNotIncludedInItemIdSets.Contains(i);

				if (!itemIsBossBag)
					continue;

				bossBags.Add(i);
			}
		}
	}
}
