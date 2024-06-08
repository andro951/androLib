using androLib.Common.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
		public override bool InstancePerEntity => true;
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
			return !ItemID.Sets.IsAPickup[entity.type];
		}
		public override void LoadData(Item item, TagCompound tag) {
			favorited = item.favorited;
		}
		public override void SaveData(Item item, TagCompound tag) { }
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

		#region ItemSpace
		
		private static (uint blockRequestResetTime, Dictionary<int, uint> requests)[] playerItemSpaceStatus;
		private static SortedSet<int> MyRequests = new();
		private static bool PlayerHasSpace(int playerWhoAmI, int itemWhoAmI) {
			(uint blockRequestResetTime, Dictionary<int, uint> requests) info = playerItemSpaceStatus[itemWhoAmI];
			if (info.requests.TryGetValue(playerWhoAmI, out uint expirationTime)) {
				if (expirationTime > Main.GameUpdateCount) {
					return true;
				}
				else {
					info.requests.Remove(playerWhoAmI);
				}
			}

			return false;
		}
		private static void ResetItemSpaceStatus(int itemWhoAmI) {
			(uint blockRequestResetTime, Dictionary<int, uint> requests) info = playerItemSpaceStatus[itemWhoAmI];
			info.blockRequestResetTime = Main.GameUpdateCount + 5;
			info.requests.Clear();
		}
		private static void UpdateItemSpaceStatus(int playerWhoAmI, int itemWhoAmI) {//-itemWhoAmI means remove.  -itemWhoAmI - 1 => itemWhoAmI
			if (itemWhoAmI >= 0) {
				(uint blockRequestResetTime, Dictionary<int, uint> requests) info = playerItemSpaceStatus[itemWhoAmI];
				if (RequestBlocked(itemWhoAmI))
					return;

				uint reset = (uint)(playerWhoAmI == Main.myPlayer ? 10 : 22);
				info.requests.AddOrSet(playerWhoAmI, Main.GameUpdateCount + reset);
				if (Main.netMode == NetmodeID.MultiplayerClient)
					MyRequests.Add(itemWhoAmI);
			}
			else {
				itemWhoAmI = -itemWhoAmI - 1;
				(uint blockRequestResetTime, Dictionary<int, uint> requests) info = playerItemSpaceStatus[itemWhoAmI];
				info.requests.Remove(playerWhoAmI);
			}
		}
		private static bool RequestBlocked(int itemWhoAmI) => playerItemSpaceStatus[itemWhoAmI].blockRequestResetTime > Main.GameUpdateCount;
		private static bool RequestOnCooldown(int itemWhoAmI) => playerItemSpaceStatus[itemWhoAmI].requests.TryGetValue(Main.myPlayer, out uint canAcceptNewRequestTime) && canAcceptNewRequestTime > Main.GameUpdateCount;
		private static void SendItemSpaceStatusToServer(int itemWhoAmI, int ignoreClient = -1) {
			UpdateItemSpaceStatus(Main.myPlayer, itemWhoAmI);
			ModPacket modPacket = AndroMod.Instance.GetPacket();
			modPacket.Write((byte)AndroMod.AndroModPacketID.ItemSpaceSync);
			modPacket.Write((short)itemWhoAmI);
			modPacket.Send(ignoreClient: ignoreClient);
			//$"TellServerIHaveSpaceForItem; itemWhoAmI: {itemWhoAmI}, item: {Main.item[(itemWhoAmI >= 0 ? itemWhoAmI : -(itemWhoAmI + 1))].S()}".LogSimpleNT();
		}
		internal static void ReceiveItemSpaceStatus(int clientWhoAmI, BinaryReader reader) {
			short itemWhoAmI = reader.ReadInt16();
			UpdateItemSpaceStatus(clientWhoAmI, itemWhoAmI);
			if (Main.netMode == NetmodeID.Server)
				SendItemSpaceStatusToServer(itemWhoAmI, clientWhoAmI);
			//$"ReceiveItemSpaceStatusFromClient; clientWhoAmI: {clientWhoAmI}, itemWhoAmI: {itemWhoAmI}".LogSimpleNT();
		}
		private static bool CheckingVanillaItemSpace = false;
		public static void Update() {
			if (Main.netMode != NetmodeID.MultiplayerClient)
				return;

			CheckingVanillaItemSpace = true;
			for (int itemSpaceStatusIndex = 0; itemSpaceStatusIndex < Main.item.Length; itemSpaceStatusIndex++) {
				Item item = Main.item[itemSpaceStatusIndex];
				if (!item.active)
					continue;

				if (item.ownIgnore == Main.myPlayer || item.playerIndexTheItemIsReservedFor == Main.myPlayer)
					continue;

				if (RequestOnCooldown(item.whoAmI))
					continue;

				Player player = Main.LocalPlayer;

				if (player.ItemSpace(item).CanTakeItem)
					continue;

				if (!ItemLoader.CanPickup(item, player) || !player.CanPullItem(item, new Player.ItemSpaceStatus(true)))
					continue;

				float distanceFromGrabRange = Math.Abs(player.position.X + (float)(player.width / 2) - item.position.X - (float)(item.width / 2)) + Math.Abs(player.position.Y + (float)(player.height / 2) - item.position.Y - (float)item.height);
				distanceFromGrabRange -= Math.Min(player.GetItemGrabRange(item) * 4, NPC.sWidth);

				//Uses 4x grab range to be similar to vanilla but not over clog with messages.
				if (distanceFromGrabRange > 0)
					continue;

				if (WEItemSpaceInternal(item, Main.LocalPlayer))
					SendItemSpaceStatusToServer(item.whoAmI);
			}

			CheckingVanillaItemSpace = false;

			List<int> toRemove = new();
			foreach (int itemWhoAmI in MyRequests) {
				Item item = Main.item[itemWhoAmI];
				if (!item.active)
					goto RemoveItem;

				if (WEItemSpaceInternal(item, Main.LocalPlayer))
					continue;

				RemoveItem:
				toRemove.Add(itemWhoAmI);
				SendItemSpaceStatusToServer(-(item.whoAmI + 1));
			}

			foreach (int itemWhoAmI in toRemove)
				MyRequests.Remove(itemWhoAmI);
		}
		private static void ResetPlayerItemSpaceStatus() {
			playerItemSpaceStatus = new (uint blockRequestResetTime, Dictionary<int, uint> requests)[Main.item.Length];
			for (int i = 0; i < Main.item.Length; i++) {
				playerItemSpaceStatus[i] = (0, new());
			}
		}
		public override void Load() {
			ResetPlayerItemSpaceStatus();

			On_Player.ItemSpace += On_Player_ItemSpace;
			On_Item.PickAnItemSlotToSpawnItemOn += On_Item_PickAnItemSlotToSpawnItemOn;
			AndroMod.OnResetGameCounter += ResetPlayerItemSpaceStatus;
		}

		private int On_Item_PickAnItemSlotToSpawnItemOn(On_Item.orig_PickAnItemSlotToSpawnItemOn orig, bool reverseLookup, int nextItem) {
			int num = orig(reverseLookup, nextItem);
			ResetItemSpaceStatus(num);
			return num;
		}

		private Player.ItemSpaceStatus On_Player_ItemSpace(On_Player.orig_ItemSpace orig, Player self, Item newItem) {
			Player.ItemSpaceStatus itemSpaceStatus = orig(self, newItem);
			if (!itemSpaceStatus.CanTakeItem) {
				if (WEItemSpace(newItem, self))
					itemSpaceStatus = new Player.ItemSpaceStatus(CanTakeItem: true);
			}

			return itemSpaceStatus;
		}
		private static bool WEItemSpace(Item item, Player player) {
			if (Main.netMode != NetmodeID.SinglePlayer) {
				return PlayerHasSpace(player.whoAmI, item.whoAmI);
			}

			if (CheckingVanillaItemSpace)//Only happens on MultiplayerClient
				return false;

			return WEItemSpaceInternal(item, player);
		}
		private static bool WEItemSpaceInternal(Item item, Player player) {
			if (player.whoAmI != Main.myPlayer)
				return false;
			bool result = StorageManager.CanVacuumItem(item, player);
			return result;
		}

		#endregion
	}

	public static class ItemStaticMethods {
		public static string ModFullName(this Item item) => item.ModItem?.FullName ?? item.GetItemInternalName();
	} 
}
