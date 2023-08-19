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
        public static bool Enabled { get; private set; }
		public static bool MagicStorageEnabledAndOpen => Enabled ? IsOpen() : false;
        public static event Action HandleMagicStorageOnTickEvents;
		public override void Load() {
            Enabled = ModLoader.HasMod(AndroMod.magicStorageName);

            AndroMod.magicStorageEnabled = Enabled;
        }
        public override void PostDrawInterface(SpriteBatch spriteBatch) {
			if (Enabled)
                HandleOnTickEvents();
        }
        public static bool MagicStorageIsOpen() {
            if (!Enabled)
                return false;

            return IsOpen();
        }
        public static void TryClosingMagicStorage() {
            if (MagicStorageIsOpen())
                CloseMagicStorage();
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
