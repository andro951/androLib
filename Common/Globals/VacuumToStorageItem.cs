using androLib.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace androLib.Common.Globals
{
	public class VacuumToStorageItem : GlobalItem {
		public bool favorited;
		private int timer;
		private const int timerReset = 15;
		public override bool InstancePerEntity => true;
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
			return !ItemID.Sets.IsAPickup[entity.type];
		}
		public override void LoadData(Item item, TagCompound tag) {
			favorited = item.favorited;
		}
		public override void SaveData(Item item, TagCompound tag) {

		}
		public override void PostUpdate(Item item) {
			if (Main.netMode != NetmodeID.Server)
				return;

			int keepTime = item.keepTime;
			if (item.playerIndexTheItemIsReservedFor != 255) {
				timer++;
				if (timer >= timerReset) {
					timer = 0;
					item.playerIndexTheItemIsReservedFor = 255;
				}
			}
		}
		public override void UpdateInventory(Item item, Player player) {
			//Track favorited
			if (item.favorited) {
				if (!favorited && AndroModSystem.FavoriteKeyDown) {
					favorited = true;
				}
			}
			else {
				if (favorited) {
					if (!AndroModSystem.FavoriteKeyDown) {
						item.favorited = true;
					}
					else {
						favorited = false;
					}
				}
			}
		}
		public override bool OnPickup(Item item, Player player) {
			if (Main.netMode == NetmodeID.Server || player.whoAmI != Main.myPlayer)
				return true;

			if (item.NullOrAir())
				return true;

			Item cloneForInfo = item.Clone();
			if (StorageManager.TryVacuumItem(ref item, player)) {
				PopupText.NewText(PopupTextContext.RegularItemPickup, cloneForInfo, cloneForInfo.stack - item.stack);
				SoundEngine.PlaySound(SoundID.Grab);
				return false;
			}

			return true;
		}
		public override bool ItemSpace(Item item, Player player) {
			if (Main.netMode == NetmodeID.Server) {
				return StorageManager.CanBeStored(item);
			}

			if (player.whoAmI != Main.myPlayer)
				return false;

			return StorageManager.CanVacuumItem(item, player);
		}
	}

	public static class ItemStaticMethods {
		public static string ModFullName(this Item item) => item.ModItem?.FullName ?? item.GetItemInternalName();
	} 
}
