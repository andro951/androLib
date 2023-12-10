using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using System.Security.Cryptography.X509Certificates;

namespace androLib.Common.Utility {
	public static class StaticMethodInfos {
		public static readonly MethodInfo itemSlot_TryOpenContainer = typeof(ItemSlot).GetMethod("TryOpenContainer", BindingFlags.Static | BindingFlags.NonPublic);
		public delegate void ItemSlot_TryOpenContainerDelegate(Item item, Player player);
		public static ItemSlot_TryOpenContainerDelegate ItemSlot_TryOpenContainer = (ItemSlot_TryOpenContainerDelegate)Delegate.CreateDelegate(typeof(ItemSlot_TryOpenContainerDelegate), itemSlot_TryOpenContainer);

		public static readonly MethodInfo extractinatorUse = typeof(Player).GetMethod("ExtractinatorUse", BindingFlags.NonPublic | BindingFlags.Instance);
		public delegate void ExtractinatorUseDelegate(Player player, int extractType, int extractinatorBlockType);
		public static ExtractinatorUseDelegate ExtractinatorUseMethod = (ExtractinatorUseDelegate)Delegate.CreateDelegate(typeof(ExtractinatorUseDelegate), extractinatorUse);
		/// <summary>
		/// Needs to be paired with a detour around ItemLoader.ExtractinatorUse() and set stack to zero.
		/// </summary>
		/// <param name="extractType"></param>
		/// <param name="extractinatorBlockType"></param>
		public static void ExtractinatorUse(int extractType, int extractinatorBlockType) {
			Extracting = true;
			ExtractinatorUseMethod(null, extractType, extractinatorBlockType);
			Extracting = false;
		}

		public static bool Extracting = false;
		public delegate void orig_ItemLoader_ExtractinatorUse(ref int resultType, ref int resultStack, int extractType, int extractinatorBlockType);
		public delegate void hook_ItemLoader_ExtractinatorUse(orig_ItemLoader_ExtractinatorUse orig, ref int resultType, ref int resultStack, int extractType, int extractinatorBlockType);
		public static readonly MethodInfo ItemLoaderExtractinatorUse = typeof(ItemLoader).GetMethod("ExtractinatorUse", BindingFlags.Public | BindingFlags.Static);
		public static void ItemLoader_ExtractinatorUse_Detour(orig_ItemLoader_ExtractinatorUse orig, ref int resultType, ref int resultStack, int extractType, int extractinatorBlockType) {
			orig(ref resultType, ref resultStack, extractType, extractinatorBlockType);
			if (Extracting)
				resultStack = 0;
		}
	}
}
