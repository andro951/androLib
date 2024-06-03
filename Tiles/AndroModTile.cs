using androLib.Common.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;

namespace androLib.Tiles {
	public abstract class AndroModTile : ModTile {
		private static SortedSet<int> ValidSolidReplaceTiles = new();
		protected virtual bool IsValidSolidReplaceTile => false;
		public override void SetStaticDefaults() {
			if (IsValidSolidReplaceTile)
				ValidSolidReplaceTiles.Add(Type);
		}
		private static bool updatePlacementPreview = false;
		internal static void On_Player_UpdatePlacementPreview(On_Player.orig_UpdatePlacementPreview orig, Player self, Item sItem) {
			if (sItem.createTile != -1) {
				if (ValidSolidReplaceTiles.Contains(sItem.createTile))
					updatePlacementPreview = true;
			}

			orig(self, sItem);
			updatePlacementPreview = false;
		}

		internal static bool On_TileObjectData_CustomPlace(On_TileObjectData.orig_CustomPlace orig, int type, int style) {
			if (updatePlacementPreview)
				return true;

			return orig(type, style);
		}
		internal static void On_TileObject_DrawPreview(On_TileObject.orig_DrawPreview orig, SpriteBatch sb, TileObjectPreviewData op, Vector2 position) {
			ModTile modTile = ModContent.GetModTile(op.Type);
			if (modTile is AndroModTile androModTile && androModTile.IsValidSolidReplaceTile) {
				androModTile.OnTileObjectDrawPreview(op.Coordinates.X, op.Coordinates.Y, op);
			}

			orig(sb, op, position);
		}

		protected virtual void OnTileObjectDrawPreview(short x, short y, TileObjectPreviewData op) {}

		internal static void IL_TileObject_DrawPreview(ILContext il) {

			ILCursor c = new(il);

			//// int num12 = op[i, j];
			//IL_019a: ldloc.2
			//IL_019b: callvirt instance int32 Terraria.ObjectData.TileObjectData::get_DrawYOffset()

			//IL_01a0: stloc.s 7

			//IL_01a2: ldarg.1
			//IL_01a3: ldloc.s 12
			//IL_01a5: ldloc.s 15
			//IL_01a7: callvirt instance int32 Terraria.DataStructures.TileObjectPreviewData::get_Item(int32, int32)
			//IL_01ac: stloc.s 18
			//// if (num12 != 1)
			//IL_01ae: ldloc.s 18
			//IL_01b0: ldc.i4.1
			//IL_01b1: beq.s IL_01ce

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchStloc(18),
				i => i.MatchLdloc(18)
				)) {
				throw new Exception("Failed to find instructions for IL_TileObject_DrawPreview");
			}

			c.EmitDelegate((int num12) => {
				if (StopObjectPreviewReset) {
					StopObjectPreviewReset = false;
					return 1;
				}

				return num12;
			});
		}
		internal static void On_TileObjectPreviewData_Reset(On_TileObjectPreviewData.orig_Reset orig, TileObjectPreviewData self) {
			if (StopObjectPreviewReset)
				return;

			orig(self);
		}
		internal static bool On_WorldGen_ReplaceTile(On_WorldGen.orig_ReplaceTile orig, int x, int y, ushort targetType, int targetStyle) {
			Tile tileSafely = Framing.GetTileSafely(x, y);
			bool wouldWork = WorldGen.WouldTileReplacementWork(targetType, x, y);
			bool result = orig(x, y, targetType, targetStyle);
			if (result) {
				Tile tile = Main.tile[x, y];
				ModTile modTile = TileLoader.GetTile(tile.TileType);
				if (modTile is AndroModTile androModTyle) {
					androModTyle.PlaceInWorld(x, y, Main.LocalPlayer.HeldItem);
					WorldGen.TileFrame(x, y);
				}
			}

			StopObjectPreviewReset = false;

			return result;
		}
		private static bool StopObjectPreviewReset = false;
		internal static bool On_Player_PlaceThing_ValidTileForReplacement(On_Player.orig_PlaceThing_ValidTileForReplacement orig, Player self) {
			int createTile = self.HeldItem.createTile;
			int tileTargetX = Player.tileTargetX;
			int tileTargetY = Player.tileTargetY;
			Tile tile = Main.tile[tileTargetX, tileTargetY];
			if (Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && !Main.tileRope[tile.TileType]) {
				if (Main.tileSolid[createTile] && !Main.tileSolidTop[createTile] && !Main.tileRope[createTile] && ValidSolidReplaceTiles.Contains(createTile)) {
					StopObjectPreviewReset = true;
					return true;
				}
			}

			return orig(self);
		}
		internal static bool On_WorldGen_WouldTileReplacementWork(On_WorldGen.orig_WouldTileReplacementWork orig, ushort attemptingToReplaceWith, int x, int y) {
			Tile tile = Main.tile[x, y];
			if (Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && !Main.tileRope[tile.TileType]) {
				if (Main.tileSolid[attemptingToReplaceWith] && !Main.tileSolidTop[attemptingToReplaceWith] && !Main.tileRope[attemptingToReplaceWith] && ValidSolidReplaceTiles.Contains(attemptingToReplaceWith))
					return true;
			}

			return orig(attemptingToReplaceWith, x, y);
		}
	}
}
