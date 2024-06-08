using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.TileDataPacking;

namespace androLib.Tiles.TileData {
	public struct TilePipeData : ITileData {
		public const byte Empty = 0;
		private byte pipeData;
		private static int hasPipeBit = 0;
		private static int pipeTypeOffset = 1;
		private static int pipeTypeLength = 7;
		public bool HasPipe { get => GetBit(pipeData, hasPipeBit); set => pipeData = (byte)SetBit(value, pipeData, hasPipeBit); }
		public byte PipeType { get => (byte)Unpack(pipeData, pipeTypeOffset, pipeTypeLength); set => pipeData = (byte)Pack(value, pipeData, pipeTypeOffset, pipeTypeLength); }
		public byte PipeData { get => pipeData; set => pipeData = value; }
	}
	public static class ES_TileDataStaticMethods {
		public static byte PipeData(this Tile tile) => tile.Get<TilePipeData>().PipeData;
		public static void PipeData(this Tile tile, byte pipeData) => tile.Get<TilePipeData>().PipeData = pipeData;
	}
}
