using androLib.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace androLib.Common.Globals
{
	//public class GenericGlobalNPC : GlobalNPC {

	//}
	public static class NPCStaticMethods
	{
		public static bool IsWorm(this NPC npc) {
			return npc.aiStyle == NPCAIStyleID.Worm || npc.aiStyle == NPCAIStyleID.TheDestroyer;
		}
		//public static bool IsDummy(this NPC npc) => npc.netID < NPCID.Count ? npc.netID == NPCID.TargetDummy : npc.ModFullName() is string modFullName && (AndroMod.calamityEnabled && modFullName == "CalamityMod/SuperDummyNPC" || AndroMod.fargosEnabled && modFullName == "Fargowiltas/SuperDummy");
		//public static bool IsBoss(this NPC npc) => npc.boss || WEGlobalNPC.multipleSegmentBossTypes.ContainsKey(npc.netID) || WEGlobalNPC.normalNpcsThatDropsBags.Contains(npc.netID);
		public static int RealNetID(this NPC npc) => npc.realLife == -1 ? npc.netID : Main.npc[npc.realLife].netID;
		public static NPC RealNPC(this NPC npc) => npc.realLife == -1 ? npc : Main.npc[npc.realLife];
		public static int RealLife(this NPC npc) => npc.realLife == -1 ? npc.life : Main.npc[npc.realLife].life;
		public static int RealLifeMax(this NPC npc) => npc.realLife == -1 ? npc.lifeMax : Main.npc[npc.realLife].lifeMax;
		public static int RealLifeRegen(this NPC npc) => npc.realLife == -1 ? npc.lifeRegen : Main.npc[npc.realLife].lifeRegen;
		public static float RealValue(this NPC npc) => npc.realLife == -1 ? npc.value : Main.npc[npc.realLife].value;
		public static void AddValue(this NPC npc, int value) {
			npc.extraValue += value;
		}

		public static string ModFullName(this NPC npc) => npc.ModNPC?.FullName ?? npc.type.GetNPCIDOrName();
	}
}
