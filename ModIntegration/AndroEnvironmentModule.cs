using androLib;
using MagicStorage;
using MagicStorage.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace androLib.ModIntegration
{
    [ExtendsFromMod(AndroMod.magicStorageName)]
    public class AndroEnvironmentModule : EnvironmentModule {
        public override string Name => "Andro's Storage";
		private IStorageManagerWrapper _storageManager;
		public AndroEnvironmentModule() {
			_storageManager = new MagicStorageWrapper();
		}

		public override IEnumerable<Item> GetAdditionalItems(EnvironmentSandbox sandbox) {
			return _storageManager.GetAdditionalItems();
        }
	}

	public interface IStorageManagerWrapper {
		IEnumerable<Item> GetAdditionalItems();
	}

	public class MagicStorageWrapper : IStorageManagerWrapper {
		public IEnumerable<Item> GetAdditionalItems() {
			return StorageManager.GetMagicStorageItems.Concat(StorageManager.AllItems.SelectMany(i => i));
		}
	}
}
