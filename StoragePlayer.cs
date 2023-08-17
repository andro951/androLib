using androLib.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace androLib
{
	public class StoragePlayer : ModPlayer {
		public static StoragePlayer LocalStoragePlayer => Main.LocalPlayer.GetModPlayer<StoragePlayer>();
		public bool disableLeftShiftTrashCan = ItemSlot.Options.DisableLeftShiftTrashCan;

		public class TryReturnItemToPlayerFunc {
			private event Func<Item, Player, bool> eventHandler;

			public void Add(Func<Item, Player, bool> func) {
				eventHandler += func;
			}

			public bool Invoke(ref Item item, Player player, bool allowQuickSpawn = false) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, Player, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item, player))
						return true;
				}

				item = player.GetItem(player.whoAmI, item, GetItemSettings.InventoryEntityToPlayerInventorySettings);
				if (item.IsAir)
					return true;

				if (!allowQuickSpawn)
					return false;

				player.QuickSpawnItem(player.GetSource_Misc("PlayerDropItemCheck"), item, item.stack);

				return true;
			}
		}
		public TryReturnItemToPlayerFunc TryReturnItemToPlayer = new();


	}
}
