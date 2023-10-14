using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Terraria;

namespace androLib.Common.Utility {
	public static class StaticMethodInfos {
		public static readonly MethodInfo itemSlot_TryOpenContainer = typeof(ItemSlot).GetMethod("TryOpenContainer", BindingFlags.Static | BindingFlags.NonPublic);
		public delegate void ItemSlot_TryOpenContainerDelegate(Item item, Player player);
		public static ItemSlot_TryOpenContainerDelegate ItemSlot_TryOpenContainer = (ItemSlot_TryOpenContainerDelegate)Delegate.CreateDelegate(typeof(ItemSlot_TryOpenContainerDelegate), itemSlot_TryOpenContainer);
	}
}
