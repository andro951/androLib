using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace androLib.Common.Globals
{
	//public class GenericGlobalItem : GlobalItem {

	//}

	public static class ItemStaticMethods {
		public static string ModFullName(this Item item) => item.ModItem?.FullName ?? item.Name;
	} 
}
