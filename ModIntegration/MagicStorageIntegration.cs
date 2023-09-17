using androLib;
using MagicStorage;
using MagicStorage.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using androLib.Common.Globals;
using androLib.Common.Utility;
using androLib.UI;

namespace androLib.ModIntegration
{
    [JITWhenModsEnabled(AndroMod.magicStorageName)]
    public class MagicStorageIntegration : ModSystem
    {
		public static bool MagicStorageEnabledAndOpen => AndroMod.magicStorageEnabled ? IsOpen() : false;
        public static event Action HandleMagicStorageOnTickEvents;
        public override void PostDrawInterface(SpriteBatch spriteBatch) {
			if (AndroMod.magicStorageEnabled)
                HandleOnTickEvents();
        }
        public static bool MagicStorageIsOpen() {
            if (!AndroMod.magicStorageEnabled)
                return false;

            return IsOpen();
        }
        public static void TryClosingMagicStorage() {
            if (MagicStorageIsOpen())
                CloseMagicStorage();
        }
        public static bool DepositToMagicStorage(IEnumerable<Item> items) {
            if (!AndroMod.magicStorageEnabled)
                return false;

            return TryDepositToMagicStorage(items);
        }

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool TryDepositToMagicStorage(IEnumerable<Item> items) {
			IEnumerable<TEStorageCenter> centers = MagicStorage.Utility.GetNearbyCenters(Main.LocalPlayer);
			if (!centers.Any())
				return false;

            bool anyStored = false;
            foreach (TEStorageCenter center in centers) {
				TEStorageHeart storageHeart = center.GetHeart();
                if (storageHeart == null)
					continue;

				storageHeart.TryDeposit(items.ToList());
				//foreach (Item item in items) {
    //                if (item.NullOrAir() || item.favorited)
    //                    continue;

				//	int oldType = item.type;
				//	int oldStack = item.stack;

				//	if (oldType != item.type || oldStack != item.stack) {
				//		Chest.VisualizeChestTransfer(Main.LocalPlayer.Center, center.Position.ToWorldCoordinates(16, 16), ContentSamples.ItemsByType[oldType], oldStack - item.stack);
    //                    anyStored = true;
				//	}

				//	if (item.stack <= 0)
				//		item.TurnToAir();
				//}
			}

			return anyStored;
        }


		[MethodImpl(MethodImplOptions.NoInlining)]
        private static void CloseMagicStorage() {
            MagicStorage.StoragePlayer.LocalPlayer.CloseStorage();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsOpen() {
            return Main.playerInventory && MagicStorage.StoragePlayer.LocalPlayer.ViewingStorage().X >= 0;
        }
	    
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void HandleOnTickEvents() {
            if (IsOpen()) {
                if (Main.netMode < NetmodeID.Server)
					StorageManager.CloseAllStorageUI();

				HandleMagicStorageOnTickEvents?.Invoke();
            }
        }
	}
}
