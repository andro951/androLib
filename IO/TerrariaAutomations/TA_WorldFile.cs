using androLib.Common.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using androLib.Tiles.TileData;

namespace androLib.IO.TerrariaAutomations {
	public static class TA_WorldFile {
		internal static void Load() {
			WorldFileManager.RegisterWorldFileData(WorldFileManager.WorldFileID.TerrariaAutomations, Save, Load);
		}
		private static void Save(BinaryWriter writer) {
			writer.Write((ushort)Main.maxTilesX);
			writer.Write((ushort)Main.maxTilesY);
			writer.Write((ushort)Main.tile.Height);

			SortedSet<byte> pipeTypesSet = new();
			uint inARowCounter = 0;
			uint inARowHighest = 0;
			byte tileBeingCounted = 0;
			TilePipeData[] tilePipeData = Main.tile.GetData<TilePipeData>();
			for (uint x = 0; x < Main.maxTilesX; x++) {
				for (uint y = 0; y < Main.maxTilesY; y++) {
					uint z = x * Main.tile.Height + y;
					byte tileData = tilePipeData[z].PipeData;
					pipeTypesSet.Add(tileData);
					if (tileData == tileBeingCounted) {
						inARowCounter++;
					}
					else {
						if (inARowHighest < inARowCounter)
							inARowHighest = inARowCounter;

						tileBeingCounted = tileData;
						inARowCounter = 1;
					}
				}
			}

			if (inARowHighest < inARowCounter)
				inARowHighest = inARowCounter;

			byte pipeTypes = (byte)(pipeTypesSet.Count - 1);
			writer.Write(pipeTypes);
			uint numBitsNeededForPipeId = pipeTypes.BitsNeeded();
			uint maxBitsInTileCountRepresentation = inARowHighest.BitsNeeded();
			writer.Write(maxBitsInTileCountRepresentation);

			byte[] pipeTypesArr = pipeTypesSet.ToArray();
			bool usesIdLookup = numBitsNeededForPipeId < 8;
			writer.Write(usesIdLookup);
			//$"SavingWorld; numBitsNeededForPipeId: {numBitsNeededForPipeId}, maxBitsInTileCountRepresentation: {maxBitsInTileCountRepresentation}, usesIdLookup: {usesIdLookup}, pipeTypes: {pipeTypes}".LogSimple();
			if (usesIdLookup) {
				Dictionary<byte, uint_b> pipeIds = new();
				for (uint i = 0; i <= pipeTypes; i++) {
					byte pipeType = pipeTypesArr[i];
					pipeIds.Add(pipeType, new(i, numBitsNeededForPipeId));
					writer.Write(pipeType);
					//$"pipeIds[{pipeType} ({pipeType.ToBinaryString()})]: {i} ({i.ToBinaryString(numBitsNeededForPipeId)})".LogSimple();
				}

				uint tileCount = 1;
				byte pipeData = tilePipeData[0].PipeData;
				//$"pipeData: {pipeData.ToBinaryString()}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: 0, x: 0, y: 0".LogSimple();
				uint x = 0;
				uint y = 1;
				uint z = 0;
				for (; x < Main.maxTilesX; x++) {
					for (; y < Main.maxTilesY; y++) {
						z = x * Main.tile.Height + y;
						byte otherPipeData = tilePipeData[z].PipeData;
						if (otherPipeData == pipeData) {
							tileCount++;
						}
						else {
							//$"-pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeIds[pipeData]}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z - 1}, x: {(y == 0 ? x - 1 : x)}, y: {(y == 0 ? Main.maxTilesY - 1 : y - 1)}".LogSimple();
							writer.WriteNum(pipeIds[pipeData]);
							writer.WriteNum(tileCount, maxBitsInTileCountRepresentation);
							pipeData = otherPipeData;
							tileCount = 1;
							//$"pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeIds[pipeData]}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
						}

						//if (x == Main.maxTilesX - 1 && y == Main.maxTilesY - 1) $"-pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeIds[pipeData]}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
					}

					y = 0;
				}

				writer.WriteNum(pipeIds[pipeData]);
				writer.WriteNum(tileCount, maxBitsInTileCountRepresentation);
			}
			else {
				uint tileCount = 1;
				byte pipeData = tilePipeData[0].PipeData;
				//$"pipeData: {pipeData.ToBinaryString()}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: 0, x: 0, y: 0".LogSimple();
				uint x = 0;
				uint y = 1;
				uint z = 0;
				for (; x < Main.maxTilesX; x++) {
					for (; y < Main.maxTilesY; y++) {
						z = x * Main.tile.Height + y;
						byte otherPipeData = tilePipeData[z].PipeData;
						if (otherPipeData == pipeData) {
							tileCount++;
						}
						else {
							//$"-pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeIds[pipeData]}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z - 1}, x: {(y == 0 ? x - 1 : x)}, y: {(y == 0 ? Main.maxTilesY - 1 : y - 1)}".LogSimple();
							writer.WriteNum(pipeData, numBitsNeededForPipeId);
							writer.WriteNum(tileCount, maxBitsInTileCountRepresentation);
							pipeData = otherPipeData;
							tileCount = 1;
							//$"pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeIds[pipeData]}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
						}

						//if (x == Main.maxTilesX - 1 && y == Main.maxTilesY - 1) $"-pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeIds[pipeData]}, tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
					}

					y = 0;
				}

				writer.WriteNum(pipeData, numBitsNeededForPipeId);
				writer.WriteNum(tileCount, maxBitsInTileCountRepresentation);
			}

			writer.Finish();
		}
		private static void Load(BinaryReader reader) {
			uint maxTilesX = reader.ReadUInt16();
			uint maxTilesY = reader.ReadUInt16();
			uint tileHeight = reader.ReadUInt16();
			byte pipeTypes = reader.ReadByte();
			uint numBitsNeededForPipeId = pipeTypes.BitsNeeded();
			uint maxBitsInTileCountRepresentation = reader.ReadUInt32();
			bool usesIdLookup = reader.ReadBoolean();
			TilePipeData[] tilePipeData = Main.tile.GetData<TilePipeData>();
			//$"\n\nLoadingWorld; numBitsNeededForPipeId: {numBitsNeededForPipeId}, maxBitsInTileCountRepresentation: {maxBitsInTileCountRepresentation}, usesIdLookup: {usesIdLookup}, pipeTypes: {pipeTypes}".LogSimple();
			if (usesIdLookup) {
				Dictionary<uint, byte> pipeIds = new();//reverse of reader
				for (uint i = 0; i <= pipeTypes; i++) {
					byte pipeId2 = reader.ReadByte();
					pipeIds.Add(i, pipeId2);
					//$"pipeIds[{i} ({i.ToBinaryString(numBitsNeededForPipeId)})]: {pipeId2} ({pipeId2.ToBinaryString()})".LogSimple();
				}

				byte pipeData = 0;
				uint tileCount = 0;
				uint pipeId = 0;
				for (uint x = 0; x < maxTilesX; x++) {
					for (uint y = 0; y < maxTilesY; y++) {
						uint z = x * tileHeight + y;
						if (tileCount == 0) {
							pipeId = reader.ReadNum(numBitsNeededForPipeId);
							pipeData = pipeIds[pipeId];
							tileCount = reader.ReadNum(maxBitsInTileCountRepresentation);
							//$"pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeId} ({pipeId.ToBinaryString(numBitsNeededForPipeId)}), tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
						}

						tilePipeData[z].PipeData = pipeData;
						tileCount--;

						//if (tileCount == 0) $"-pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeId} ({pipeId.ToBinaryString(numBitsNeededForPipeId)}), tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
					}
				}
			}
			else {
				byte pipeData = 0;
				uint tileCount = 0;
				for (uint x = 0; x < maxTilesX; x++) {
					for (uint y = 0; y < maxTilesY; y++) {
						uint z = x * tileHeight + y;
						if (tileCount == 0) {
							pipeData = (byte)reader.ReadNum(numBitsNeededForPipeId);
							tileCount = reader.ReadNum(maxBitsInTileCountRepresentation);
							//$"pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeId} ({pipeId.ToBinaryString(numBitsNeededForPipeId)}), tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
						}

						tilePipeData[z].PipeData = pipeData;
						tileCount--;

						//if (tileCount == 0) $"-pipeData: {pipeData.ToBinaryString()}, pipeID: {pipeId} ({pipeId.ToBinaryString(numBitsNeededForPipeId)}), tileCount: {tileCount} ({tileCount.ToBinaryString(maxBitsInTileCountRepresentation)}), z: {z}, x: {x}, y: {y}".LogSimple();
					}
				}
			}

			reader.Finish();
		}
		private static bool RandonPipesOnPlayerEnterWorld => false;
		private static void TestingPopulateTiles() {
			TilePipeData[] tilePipeData = Main.tile.GetData<TilePipeData>();
			if (Debugger.IsAttached && RandonPipesOnPlayerEnterWorld) {
				for (int x = 0; x < Main.maxTilesX; x++) {
					for (int y = 0; y < Main.maxTilesY; y++) {
						int z = x * Main.tile.Height + y;
						if (Main.rand.Next(10) == 1) {
							tilePipeData[z].PipeData = (byte)Main.rand.Next(256);
						}

						worldDataBeforeSave[x, y] = tilePipeData[z].PipeData;
					}
				}
			}

			worldDataBeforeSave = new byte[Main.maxTilesX, Main.maxTilesY];
			for (int x = 0; x < Main.maxTilesX; x++) {
				for (int y = 0; y < Main.maxTilesY; y++) {
					int z = x * Main.tile.Height + y;
					worldDataBeforeSave[x, y] = tilePipeData[z].PipeData;
				}
			}
		}
		private static byte[,] worldDataBeforeSave;
		internal static void CheckLoadVsBeforeSave() {
			TilePipeData[] tilePipeData = Main.tile.GetData<TilePipeData>();
			List<uint> diff = new();
			if (worldDataBeforeSave == null) {
				$"LoadVsBeforeSave failed; worldDataBeforeSave is null".LogSimple();
				return;
			}

			for (int x = 0; x < Main.maxTilesX; x++) {
				for (int y = 0; y < Main.maxTilesY; y++) {
					int z = x * Main.tile.Height + y;
					if (worldDataBeforeSave[x, y] != tilePipeData[z].PipeData) {
						//$"LoadVsBeforeSave failed at {x}, {y}".LogSimple();
						diff.Add((uint)z);
					}
				}
			}

			if (diff.Count > 0) {
				$"LoadVsBeforeSave failed at {diff.Count} locations".LogSimple();
			}
			else {
				$"LoadVsBeforeSave passed".LogSimple();
			}
		}

		internal static void OnEnterWorld() {
			TestingPopulateTiles();
		}

		#region Net

		public static void WriteAllPipeData(BinaryWriter writer) => Save(writer);
		public static void RecieveAllPipeData(BinaryReader reader) => Load(reader);

		#endregion
	}
}
