using androLib.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace androLib.Common.Globals
{
	public class VacuumBagTile : GlobalTile {
		private bool AppliesTo(int tileType) => StorageManager.StorageTileTypes.ContainsKey(tileType);
		public override void RightClick(int i, int j, int type) {
			if (!AppliesTo(type))
				return;

			if (Main.netMode == NetmodeID.Server)
				return;

			Player player = Main.LocalPlayer;
			Main.mouseRightRelease = false;
			player.CloseSign();
			player.SetTalkNPC(-1);
			Main.npcChatCornerItem = 0;
			Main.npcChatText = "";
			if (Main.editChest) {
				SoundEngine.PlaySound(SoundID.MenuTick);
				Main.editChest = false;
				Main.npcChatText = string.Empty;
			}

			if (player.editedChestName) {
				NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
				player.editedChestName = false;
			}

			SoundEngine.PlaySound(SoundID.Grab);

			UseBag(type);
		}

		private void UseBag(int tileType) => VacuumBag.UseBag(StorageManager.StorageTileTypes[tileType]);
		public static void QuickStackToBags(ref Item item, Player player) {
			if (Main.myPlayer != player.whoAmI)
				return;

			SortedDictionary<int, Vector2> storageIDs = new();
			int num2 = 39;
			int tileX = (int)(player.Center.X / 16f);
			int tileY = (int)(player.Center.Y / 16f);
			for (int x = tileX - num2; x <= tileX + num2; x++) {
				if (x < 0 || x >= Main.maxTilesX)
					continue;

				for (int y = tileY - num2; y <= tileY + num2; y++) {
					if (y < 0 || y >= Main.maxTilesY)
						continue;

					Tile tile = Main.tile[x, y];
					int tileType = tile.TileType;
					if (StorageManager.StorageTileTypes.ContainsKey(tileType)) {
						int storageID = StorageManager.StorageTileTypes[tileType];
						if (!storageIDs.ContainsKey(storageID))
							storageIDs.Add(storageID, new Vector2(x * 16 + 16, y * 16 + 8));
					}
				}
			}

			foreach (int storageID in storageIDs.Keys) {
				Item copy = item.Clone();
				if (StorageManager.TryVacuumItemToTile(ref item, player, storageID)) {
					Chest.VisualizeChestTransfer(player.Center, storageIDs[storageID], copy, copy.stack);
					break;
				}
			}
		}
	}
}
