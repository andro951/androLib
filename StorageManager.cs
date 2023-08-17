using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace androLib
{
	public static class StorageManager {
		public class AllowedToStoreInStorageConditions {
			private event Func<Item, bool> eventHandler;

			public void Add(Func<Item, bool> func) {
				eventHandler += func;
			}

			public bool Invoke(Item item) {
				if (eventHandler == null)
					return false;

				foreach (Func<Item, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item))
						return true;
				}

				return false;
			}
		}
		public static AllowedToStoreInStorageConditions AllowedToStoreInStorage = new();
		public static bool CanBeStored(Item item) {
			return AllowedToStoreInStorage.Invoke(item);
		}

		public class CanVacuumItemConditions
		{
			private event Func<Item, Player, bool> eventHandler;
			public void Add(Func<Item, Player, bool> func) {
				eventHandler += func;
			}
			public bool Invoke(Item item, Player player) {
				if (eventHandler == null)
					return false;
				foreach (Func<Item, Player, bool> func in eventHandler.GetInvocationList()) {
					if (func.Invoke(item, player))
						return true;
				}
				return false;
			}
		}
		public static CanVacuumItemConditions CanVacuumItemHandler = new();
		public static bool CanVacuumItem(Item item, Player player) => CanVacuumItemHandler.Invoke(item, player);

		public static void TryUpdateMouseOverrideForDeposit(Item item) {
			if (item.IsAir)
				return;

			if (CanVacuumItemHandler.Invoke(item, Main.LocalPlayer))
				Main.cursorOverride = CursorOverrideID.InventoryToChest;
		}
	}
}
