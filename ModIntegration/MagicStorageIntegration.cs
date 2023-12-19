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
using Terraria.Audio;

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

            return TryDepositToMagicStorage(items.Where(i => !i.favorited));
        }

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool TryDepositToMagicStorage(IEnumerable<Item> items) {
            IEnumerable<TEStorageCenter> centers;
            bool isOpen = false;
            if (Main.LocalPlayer.TryGetModPlayer(out MagicStorage.StoragePlayer storagePlayer) && MagicStorage.Utility.GetHeartFromAccess(storagePlayer.ViewingStorage()) is TEStorageHeart heart) {
                centers = new List<TEStorageCenter>() { heart };
				isOpen = true;
			}
            else {
                centers = MagicStorage.Utility.GetNearbyCenters(Main.LocalPlayer);
			}

			if (!centers.Any())
				return false;

            bool anyStored = false;
            List<Item> allItems = items.ToList();
            List<Item> itemsCopy = items.Select(i => i.Clone()).ToList();
			foreach (TEStorageCenter center in centers) {
				TEStorageHeart storageHeart = center.GetHeart();
                if (storageHeart == null)
					continue;

				storageHeart.TryDeposit(allItems);
                for (int i = 0; i < allItems.Count; i++) {
                    Item item = allItems[i];
                    Item copy = itemsCopy[i];
                    if (item.stack < copy.stack || item.type != copy.stack) {
                        if (isOpen) {
                            SoundEngine.PlaySound(SoundID.Grab);
                        }
                        else {
							Chest.VisualizeChestTransfer(Main.LocalPlayer.Center, center.Position.ToWorldCoordinates(16, 16), copy, copy.stack - item.stack);
						}

                        if (item.stack <= 0)
                            item.TurnToAir();
					}
                }
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
					StorageManager.OnOpenMagicStorageCloseAllStorageUI();

				HandleMagicStorageOnTickEvents?.Invoke();
            }
        }

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SetBiomeGlobe(Player player) {
            if (player.TryGetModPlayer(out MagicStorage.Items.BiomePlayer biomePlayer))
                biomePlayer.biomeGlobe = true;
		}
	}
}
