using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace androLib.ModIntegration
{
	public class BossChecklistBossInfo
	{
		public string key = ""; // equal to "modSource publicName"
		public string modSource = "";
		public string internalName = "";
		public string displayName = "";
		public float progression = 0f; // See https://github.com/JavidPack/BossChecklist/blob/master/BossTracker.cs#L13 for vanilla boss values
		public Func<bool> downed = () => false;
		public bool isBoss = false;
		public bool isMiniboss = false;
		public bool isEvent = false;
		public List<int> npcIDs = new List<int>(); // Does not include minions, only npcids that count towards the NPC still being alive.
		public List<int> spawnItem = new List<int>();
		public List<int> loot = new List<int>();
		public List<int> collection = new List<int>();
	}
}
