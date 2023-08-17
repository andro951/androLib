﻿using androLib.Common.Utility;
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
	/*
	public class VacuumToStorageItem : GlobalItem {
		public bool favorited;
		public override bool InstancePerEntity => true;
		bool? canBeStored = null;
		//bool CanBeStored(Item item) {//Not used in Weapon Enchantments, copied by accident.
		//	if (canBeStored == null) {
		//		canBeStored = StorageManager.CanBeStored(item);
		//	}

		//	return (bool)canBeStored;
		//}
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
			return true;
		}
		public override void LoadData(Item item, TagCompound tag) {
			favorited = item.favorited;
		}
		public override void SaveData(Item item, TagCompound tag) {

		}
		public override void UpdateInventory(Item item, Player player) {
			//Track favorited
			if (item.favorited) {
				if (!favorited && WEModSystem.FavoriteKeyDown) {
					favorited = true;
				}
			}
			else {
				if (favorited) {
					if (!WEModSystem.FavoriteKeyDown) {
						item.favorited = true;
					}
					else {
						favorited = false;
					}
				}
			}
		}
		public override bool OnPickup(Item item, Player player) {
			if (player.whoAmI != Main.myPlayer)
				return true;

			if (item.NullOrAir())
				return true;

			Item cloneForInfo = item.Clone();
			if (OreBagUI.TryVacuumItem(player, ref item)) {
				PopupText.NewText(PopupTextContext.RegularItemPickup, cloneForInfo, cloneForInfo.stack - item.stack);
				SoundEngine.PlaySound(SoundID.Grab);
				return false;
			}

			return true;
		}
		public override bool ItemSpace(Item item, Player player) {
			if (Main.netMode == NetmodeID.Server)
				return true;

			return StorageManager.CanVacuumItem(item, player);
			//return OreBagUI.CanVacuumItem(player, item);
		}
	}
	*/

	public static class ItemStaticMethods {
		public static string ModFullName(this Item item) => item.ModItem?.FullName ?? item.Name;
	} 
}