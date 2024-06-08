using androLib.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace androLib.Items.Interfaces {
	public interface IRightClickUse : ILoadable {
		public Func<int> GetItemType { get; }
		public static void Setup() {
			foreach (IRightClickUse rightClickUse in ModContent.GetContent<IRightClickUse>()) {
				ItemID.Sets.ItemsThatAllowRepeatedRightClick[rightClickUse.GetItemType()] = true;
			}
		}

		/// <summary>
		/// Allows you to make things happen when this item is used. The return value controls whether or not ApplyItemTime will be called for the player.
		/// <br/> Return true if the item actually did something, to force itemTime.
		/// <br/> Return false to keep itemTime at 0.
		/// <br/> Return null for vanilla behavior.
		/// <para/> Runs on all clients and server. Use <code>if (player.whoAmI == Main.myPlayer)</code> and <code>if (Main.netMode == NetmodeID.??)</code> if appropriate.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns></returns>
		public bool? OnUse(Player player, Item item, int tileTargetX, int tileTargetY);
		public void PreRightClickUse(Player player, Item item, Action PostUseActions);

		/// <summary>
		/// Allows you to make things happen when this item is used. The return value controls whether or not ApplyItemTime will be called for the player.
		/// <br/> Return true if the item actually did something, to force itemTime.
		/// <br/> Return false to keep itemTime at 0.
		/// <br/> Return null for vanilla behavior.
		/// <para/> Runs on all clients and server. Use <code>if (player.whoAmI == Main.myPlayer)</code> and <code>if (Main.netMode == NetmodeID.??)</code> if appropriate.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns></returns>
		public bool? OnRightClickUse(Player player, Item item, int tileTargetX, int tileTargetY);
		public bool CursorItemIConWhileHoldingItem { get; }
		public bool InInteractionRange(Player player, Item item);
		public static bool InWireRenchUseRange(Player player, Item sItem) => (player.position.X / 16f - (float)Player.tileRangeX - (float)sItem.tileBoost - (float)player.blockRange <= (float)Player.tileTargetX) && ((player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)sItem.tileBoost - 1f + (float)player.blockRange >= (float)Player.tileTargetX) && (player.position.Y / 16f - (float)Player.tileRangeY - (float)sItem.tileBoost - (float)player.blockRange <= (float)Player.tileTargetY) && ((player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)sItem.tileBoost - 2f + (float)player.blockRange >= (float)Player.tileTargetY);
		public static bool InSimpleTileRange(Player player, Item item) => player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple);
	}

	public class RightClickUseGlobalItem : GlobalItem {
		public override void Load() {
			On_Player.ItemCheck_Inner += On_Player_ItemCheck_Inner;
		}
		private static void On_Player_ItemCheck_Inner(On_Player.orig_ItemCheck_Inner orig, Player self) {
			orig(self);

			if (PostUseActions != null) {
				PostUseActions();
				PostUseActions = null;
			}
		}
		public static Action PostUseActions;
		public override bool? UseItem(Item item, Player player) {
			if (item?.ModItem is IRightClickUse rightClickUse) {
				if (!rightClickUse.InInteractionRange(player, item))
					return null;

				if (player.altFunctionUse == 2) {
					return rightClickUse.OnRightClickUse(player, item, Player.tileTargetX, Player.tileTargetY);
				}
				else {
					return rightClickUse.OnUse(player, item, Player.tileTargetX, Player.tileTargetY);
				}
			}

			return null;
		}
		public override bool AltFunctionUse(Item item, Player player) {
			Item heldItem = player.HeldItem;
			if (ItemLoader.CanUseItem(heldItem, player) && !player.mouseInterface && !heldItem.NullOrAir() && heldItem.ModItem is IRightClickUse rightClickUse) {
				//player.controlUseItem = true;

				Tile target = Main.tile[Player.tileTargetX, Player.tileTargetY];
				rightClickUse.PreRightClickUse(player, item, PostUseActions);
				return true;
			}

			return false;
		}
		public override void HoldItem(Item item, Player player) {
			if (item.ModItem is IRightClickUse rightClickUse) {
				if (rightClickUse.CursorItemIConWhileHoldingItem && rightClickUse.InInteractionRange(player, item)) {
					player.cursorItemIconEnabled = true;
					player.cursorItemIconID = item.type;
				}
			}
		}
	}
}
