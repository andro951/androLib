using androLib;
using MagicStorage;
using MagicStorage.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace androLib.ModIntegration
{
    [ExtendsFromMod(AndroMod.magicStorageName)]
    public class AndroEnvironmentModule : EnvironmentModule {
        public override string Name => "Andro's Bags";
		public override IEnumerable<Item> GetAdditionalItems(EnvironmentSandbox sandbox) {
			return StorageManager.GetMagicStorageItems.Concat(StorageManager.AllItems.SelectMany(i => i));
        }
	}
}
