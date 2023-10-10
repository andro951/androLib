using androLib.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace androLib.Items.Interfaces {
	public interface IHasDropRates {
		/// <summary>
		/// Chance will be multiplied by the config values which is 0.5 by default for bosses and 1f by default for other NPCs, so boss chances should be double.
		/// </summary>
		public List<DropData> NpcDropTypes { get; }
		public List<ModDropData> ModNpcDropNames { get; }
		public List<DropData> NpcAIDrops { get; }
		public List<DropData> ChestDrops { get; }
		public List<DropData> CrateDrops { get; }
	}
}
