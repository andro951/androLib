using androLib.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ID.TileID;
using System.Diagnostics;

namespace androLib.Common.Globals
{
	public class GenericGlobalTile : GlobalTile {
		public static int tileType = -1;
		private static SortedDictionary<int, int> tileTypeToItemType = null;
		public static SortedDictionary<int, int> TileTypeToItemType {
			get {
				if (tileTypeToItemType == null)
					SetupTileTypeToItemType();

				return tileTypeToItemType;
			}
		}
		public static bool tileTypeToItemTypesSetup = false;
		private static void SetupTileTypeToItemType() {
			//TODO: for all the failed ones, have them search for the item name in the tile name.
			tileTypeToItemType = new();
			HashSet<int> tileTypes = Main.recipe.Select(recipe => recipe.requiredTile).SelectMany(tiles => tiles).ToHashSet();
			foreach (int tileType in tileTypes) {
				int itemType = GetDroppedItem(tileType);
				if (itemType <= 0) {
					if (tileType > TileID.Count || AndroMod.magicStorageEnabled && tileType == TileID.DemonAltar) {
						if (TryGetModTileName(tileType, out string modTileName) && TryGetModTileItemType(modTileName, out int modTileItemType)) {
							itemType = modTileItemType;
						}
						else {
							switch (modTileName) {
								case "SCalAltar":
									break;//No dropped item, don't log.
								default:
									//if (Debugger.IsAttached) $"Failed to find find modded tile name for tile: {tileType.GetTileIDOrName()}, modTileName: {modTileName}".LogSimple();
									break;
							}
						}
					}
					else {
						$"Failed to find find vanilla tile type: {tileType.GetTileIDOrName()}".LogSimple();
					}
				}

				if (itemType > 0)
					tileTypeToItemType.Add(tileType, itemType);
			}

			tileTypeToItemTypesSetup = true;
			//$"\n{tileTypeToItemType.Select(t => $"{t.Key}, {(t.Key >= TileID.Count ? TileLoader.GetTile(t.Key).Name : "")}: {t.Value.CSI().S()}").JoinList("\n")}".LogSimple();
		}
		private static bool TryGetModTileName(int tileType, out string modTileName) {
			modTileName = "";
			if (AndroMod.magicStorageEnabled && tileType == TileID.DemonAltar) {
				modTileName = "MagicStorage/DemonAltar";
				return true;
			}

			if (tileType < TileID.Count)
				return false;

			ModTile modTile = TileLoader.GetTile(tileType);
			if (modTile == null)
				return false;

			modTileName = modTile.FullName;
			return true;
		}
		private static bool TryGetModTileItemType(string modTileName, out int modTileItemType) {
			modTileItemType = 0;
			for (int type = ItemID.Count; type < ItemLoader.ItemCount; type++) {
				ModItem modItem = ContentSamples.ItemsByType[type].ModItem;
				if (modItem == null)
					continue;

				bool match = modItem.FullName == modTileName;
				if (match) {
					modTileItemType = modItem.Type;
					return true;
				}
			}

			for (int type = ItemID.Count; type < ItemLoader.ItemCount; type++) {
				ModItem modItem = ContentSamples.ItemsByType[type].ModItem;
				if (modItem == null)
					continue;

				int slashIndex = modTileName.IndexOf("/");
				string searchString;
				if (slashIndex >= 0) {
					searchString = modTileName.Substring(slashIndex + 1);
				}
				else {
					searchString = modTileName;
				}

				bool match = searchString.Contains(modItem.Name) || modItem.Name.Contains(searchString);
				if (match) {
					modTileItemType = modItem.Type;
					return true;
				}
			}

			return false;
		}

		public static int GetDroppedItem(int type, int frame = 0, bool forMining = false, bool ignoreError = false) {
			int dropItem = 0;
			if (TileID.Sets.Ore[type])
				ignoreError = false;

			switch (type) {
				//Coin Piles
				case TileID.CopperCoinPile:
					dropItem = ItemID.CopperCoin;
					break;
				case TileID.SilverCoinPile:
					dropItem = ItemID.SilverCoin;
					break;
				case TileID.GoldCoinPile:
					dropItem = ItemID.GoldCoin;
					break;
				case TileID.PlatinumCoinPile:
					dropItem = ItemID.PlatinumCoin;
					break;

				//Ores
				case TileID.Iron:
					dropItem = ItemID.IronOre;
					break;
				case TileID.Copper:
					dropItem = ItemID.CopperOre;
					break;
				case TileID.Gold:
					dropItem = ItemID.GoldOre;
					break;
				case TileID.Silver:
					dropItem = ItemID.SilverOre;
					break;
				case TileID.Palladium:
					dropItem = ItemID.PalladiumOre;
					break;
				case TileID.Orichalcum:
					dropItem = ItemID.OrichalcumOre;
					break;
				case TileID.Titanium:
					dropItem = ItemID.TitaniumOre;
					break;
				case TileID.Demonite:
					dropItem = ItemID.DemoniteOre;
					break;
				case TileID.Meteorite:
					dropItem = ItemID.Meteorite;
					break;
				case TileID.Hellstone:
					dropItem = ItemID.Hellstone;
					break;
				case TileID.Tin:
					dropItem = ItemID.TinOre;
					break;
				case TileID.Lead:
					dropItem = ItemID.LeadOre;
					break;
				case TileID.Tungsten:
					dropItem = ItemID.TungstenOre;
					break;
				case TileID.Platinum:
					dropItem = ItemID.PlatinumOre;
					break;
				case TileID.Crimtane:
					dropItem = ItemID.CrimtaneOre;
					break;
				case TileID.Cobalt:
					dropItem = ItemID.CobaltOre;
					break;
				case TileID.Mythril:
					dropItem = ItemID.MythrilOre;
					break;
				case TileID.Adamantite:
					dropItem = ItemID.AdamantiteOre;
					break;
				case TileID.Chlorophyte:
					dropItem = ItemID.ChlorophyteOre;
					break;
				case TileID.LunarOre:
					dropItem = ItemID.LunarOre;
					break;
				case TileID.DesertFossil:
					dropItem = ItemID.DesertFossil;
					break;
				case TileID.FossilOre:
					dropItem = ItemID.FossilOre;
					break;

				//Gems and crystals
				case TileID.Crystals:
					if (frame >= 324)
						dropItem = ItemID.QueenSlimeCrystal;
					else
						dropItem = ItemID.CrystalShard;
					break;
				case TileID.Sapphire:
				case TileID.Ruby:
				case TileID.Emerald:
				case TileID.Topaz:
				case TileID.Amethyst:
				case TileID.Diamond:
					dropItem = tileType - TileID.Sapphire + ItemID.Sapphire;
					break;
				case TileID.AmberStoneBlock:
					dropItem = ItemID.Amber;
					break;
				case TileID.TreeTopaz:
					dropItem = ItemID.Topaz;
					break;
				case TileID.TreeAmethyst:
					dropItem = ItemID.Amethyst;
					break;
				case TileID.TreeSapphire:
					dropItem = ItemID.Sapphire;
					break;
				case TileID.TreeEmerald:
					dropItem = ItemID.Emerald;
					break;
				case TileID.TreeRuby:
					dropItem = ItemID.Ruby;
					break;
				case TileID.TreeDiamond:
					dropItem = ItemID.Diamond;
					break;
				case TileID.TreeAmber:
					dropItem = ItemID.Amber;
					break;
				case TileID.ExposedGems:
					switch (frame) {
						case 0:
							dropItem = ItemID.Amethyst;
							break;
						case 1:
							dropItem = ItemID.Topaz;
							break;
						case 2:
							dropItem = ItemID.Sapphire;
							break;
						case 3:
							dropItem = ItemID.Emerald;
							break;
						case 4:
							dropItem = ItemID.Ruby;
							break;
						case 5:
							dropItem = ItemID.Diamond;
							break;
						case 6:
							dropItem = ItemID.Amber;
							break;
					}
					break;
				case TileID.CrystalBall:
					dropItem = ItemID.CrystalBall;
					break;
				case TileID.Loom:
					dropItem = ItemID.Loom;
					break;
				case TileID.Anvils:
					dropItem = ItemID.IronAnvil;
					break;
				case TileID.Furnaces:
					dropItem = ItemID.Furnace;
					break;
				case TileID.Kegs:
					dropItem = ItemID.Keg;
					break;
				case TileID.CookingPots:
					dropItem = ItemID.CookingPot;
					break;
				case TileID.WorkBenches:
					dropItem = ItemID.WorkBench;
					break;
				case TileID.TeaKettle:
					dropItem = ItemID.TeaKettle;
					break;
				case TileID.Bottles:
					dropItem = ItemID.Bottle;
					break;
				case TileID.ImbuingStation:
					dropItem = ItemID.ImbuingStation;
					break;
				case TileID.MythrilAnvil:
					dropItem = ItemID.MythrilAnvil;
					break;
				case TileID.Autohammer:
					dropItem = ItemID.Autohammer;
					break;
				case TileID.LivingLoom:
					dropItem = ItemID.LivingLoom;
					break;
				case TileID.HeavyWorkBench:
					dropItem = ItemID.HeavyWorkBench;
					break;
				case TileID.Blendomatic:
					dropItem = ItemID.BlendOMatic;
					break;
				case TileID.Sawmill:
					dropItem = ItemID.Sawmill;
					break;
				case TileID.LunarCraftingStation:
					dropItem = ItemID.LunarCraftingStation;
					break;
				case TileID.AdamantiteForge:
					dropItem = ItemID.AdamantiteForge;
					break;
				case TileID.Solidifier:
					dropItem = ItemID.Solidifier;
					break;
				case TileID.MeatGrinder:
					dropItem = ItemID.MeatGrinder;
					break;
				case TileID.LesionStation:
					dropItem = ItemID.LesionStation;
					break;
				case TileID.GlassKiln:
					dropItem = ItemID.GlassKiln;
					break;
				case TileID.HoneyDispenser:
					dropItem = ItemID.HoneyDispenser;
					break;
				case TileID.SkyMill:
					dropItem = ItemID.SkyMill;
					break;
				case TileID.LihzahrdFurnace:
					dropItem = ItemID.LihzahrdFurnace;
					break;
				case TileID.IceMachine:
					dropItem = ItemID.IceMachine;
					break;
				case TileID.SteampunkBoiler:
					dropItem = ItemID.SteampunkBoiler;
					break;
				case TileID.FleshCloningVat:
					dropItem = ItemID.FleshCloningVaat;
					break;
				case TileID.BoneWelder:
					dropItem = ItemID.BoneWelder;
					break;
				case TileID.Hellforge:
					dropItem = ItemID.Hellforge;
					break;
				case TileID.DyeVat:
					dropItem = ItemID.DyeVat;
					break;
				case TileID.TinkerersWorkbench:
					dropItem = ItemID.TinkerersWorkshop;
					break;
				case TileID.Tables:
					dropItem = ItemID.WoodenTable;
					break;
				case TileID.Chairs:
					dropItem = ItemID.WoodenChair;
					break;
				case TileID.Bookcases:
					dropItem = ItemID.Bookcase;
					break;
				case TileID.AlchemyTable:
					dropItem = ItemID.AlchemyTable;
					break;
				case TileID.DemonAltar:
					if (AndroMod.magicStorageEnabled && TileTypeToItemType.Keys.Contains(type))
						dropItem = TileTypeToItemType[type];

					break;
				case TileID.Tombstones:
					dropItem = ItemID.Tombstone;
					break;
				case TileID.Presents:
					dropItem = ItemID.Present;
					break;
				default:
					ModTile modTile = TileLoader.GetTile(type);
					//Get item dropped by the tile
					if ((!forMining || TileID.Sets.Ore[type])) {
						if (modTile != null) {
							int itemDrop = TileLoader.GetItemDropFromTypeAndStyle(tileType);
							if (itemDrop > 0) {
								dropItem = itemDrop;
							}
							else if (TileTypeToItemType.Keys.Contains(type)) {
								dropItem = TileTypeToItemType[type];
							}
							else if (tileTypeToItemTypesSetup && !ignoreError) {
								switch (modTile.Name) {
									case "SCalAltar":
										break;//No dropped item, don't log.
									default:
										//if (Debugger.IsAttached) $"Failed to determine the dropItem of tile: {type.GetTileIDOrName()}, modTile.Name: {modTile.Name}, modTile.ItemDrop: {itemDrop}.".LogSimple();
										break;
								}
							}
						}
						else if (!ignoreError) {
							//if (Debugger.IsAttached) $"Failed to determine the dropItem of tile: {type.GetTileIDOrName()}.".LogSimple();
						}
					}

					break;
			}

			return dropItem;
		}
		public static int GetRequiredPickaxePower(int type, bool forIndusionPower = false) {
			ModTile modTile = TileLoader.GetTile(type);
			if (modTile != null)
				return modTile.MinPick;

			switch (type) {
				case Meteorite:
					if (forIndusionPower)
						return 60;

					return 50;
				case Demonite:
				case Crimtane:
				case Obsidian:
					return 55;
				case Ebonstone:
				case Crimstone:
				case Pearlstone:
				case Hellstone:
				case Hellforge:
					return 65;
				case Cobalt:
				case Palladium:
					return 100;
				case Mythril:
				case Orichalcum:
					return 110;
				case Adamantite:
				case Titanium:
					return 150;
				case Chlorophyte:
					return 200;
				case LihzahrdBrick:
				case LihzahrdAltar:
					return 210;
				default:
					if (Main.tileDungeon[type])
						return 100;

					return 0;
			}
		}
	}
}
