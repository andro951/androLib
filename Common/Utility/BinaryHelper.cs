﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static androLib.Common.Utility.BinaryHelper;

namespace androLib.Common.Utility {
	public static class BinaryHelper {
		public static uint BitsNeeded(this byte b) => (uint)byte.Log2(b) + 1;
		public static uint BitsNeeded(this uint i) => uint.Log2(i) + 1;
		internal const int byteNum = 8;
		internal const int ushortNum = 16;
		internal const int uintNum = 32;
		private const int ulongNum = 64;
		internal static string ToBinaryString(this uint i, int length = uintNum) => Convert.ToString(i, 2).PadLeft(length, '0');
		internal static string ToBinaryString(this uint i, uint length) => i.ToBinaryString((int)length);
		internal static string ToBinaryString(this byte b, int length = byteNum) => Convert.ToString(b, 2).PadLeft(length, '0');
		internal static string ToBinaryString(this byte b, uint length) => b.ToBinaryString((int)length);
	}
	public static class BinaryWriterHelper {
		private static uint value;
		private static int shift;
		public static uint Value => value;
		static BinaryWriterHelper() {
			Reset();
		}
		public static void Finish(this BinaryWriter writer) {
			if (shift == 0)
				return;

			//$"{value.ToBinaryString()} Finish Writer; value: {value} ({value.ToBinaryString(shift)}), shift: {shift}, full value: {value.ToBinaryString()}".LogSimple();
			writer.Write(value);
			Reset();
		}
		private static void Reset() {
			value = 0;
			shift = 0;
		}
		public static void WriteBool(this BinaryWriter writer, bool v) => writer.WriteNum(v ? 1 : (uint)0, 1);
		public static void WriteNum(this BinaryWriter writer, uint num, int length) {
			//$"\n{value.ToBinaryString()} WriteNum(num: {num} ({num.ToBinaryString(length)}), length: {length}); value: {value} ({value.ToBinaryString(shift)}), shift: {shift}, full value: {value.ToBinaryString()}".LogSimple();
			value |= (num << shift);
			shift += length;
			if (shift >= uintNum) {
				//$"{value.ToBinaryString()}  Write; value: {value} ({value.ToBinaryString()})".LogSimple();
				writer.Write(value);
				value = 0;
				shift -= uintNum;
				if (shift > 0)
					value |= (num << uintNum - length) >> (uintNum - shift);
			}

			//$"{value.ToBinaryString()}-WriteNum(num: {num} ({num.ToBinaryString(length)}), length: {length}); value: {value} ({value.ToBinaryString(shift)}), shift: {shift}, full value: {value.ToBinaryString()}\n".LogSimple();
		}
		public static void WriteNum(this BinaryWriter writer, uint num, uint length) => writer.WriteNum(num, (int)length);
		public static void WriteNum(this BinaryWriter writer, uint_c num) => writer.WriteNum(num.value, num.TotalLength);
		public static void WriteNum(this BinaryWriter writer, uint_b b) => writer.WriteNum(b.Significand, b.Bits);
		public static string S(this BinaryWriter _) {
			return $"value: ({value},  {value.ToBinaryString()}), shift: {shift}";
		}
	}
	public static class BinaryReaderHelper {
		private static uint value;
		private static int shift;
		public static uint Value => value;
		static BinaryReaderHelper() {
			Reset();
		}
		public static void Finish(this BinaryReader _) {
			//$"Finish Reader; value: {value} ({value.ToBinaryString().Substring(0, uintNum - shift)}), shift: {shift}, full value: {value.ToBinaryString()}".LogSimple();
			Reset();
		}
		private static void Reset() {
			value = 0;
			shift = -1;
		}
		public static bool ReadBool(this BinaryReader reader) => reader.ReadNum(1) == 1;
		public static uint ReadNum(this BinaryReader reader, int length) {
			if (shift == -1) {
				shift = 0;
				value = reader.ReadUInt32();
			}

			//$"\n{value.ToBinaryString()} ReadNum(length: {length}); value: {value} ({value.ToBinaryString().Substring(0, uintNum - shift)}), shift: {shift}, full value: {value.ToBinaryString()}".LogSimple();
			int originalShift = shift;
			shift += length;
			uint result;
			if (shift >= uintNum) {
				if (shift > uintNum) {
					shift -= uintNum;
					uint originalValue = value;
					value = reader.ReadUInt32();
					//uint leftValue = (originalValue >> originalShift);
					int leftShift = uintNum - shift;
					//uint rightValue = ((value << (leftShift)) >> (leftShift - (uintNum - originalShift)));//(length - leftShift));
					result = (originalValue >> originalShift) | ((value << (leftShift)) >> (leftShift - (uintNum - originalShift)));//(length - leftShift));
					//if (length == 3 && result > 4) {
					//	$"{value.ToBinaryString()} leftValue: {leftValue} ({leftValue.ToBinaryString()}), rightValue: {rightValue} ({rightValue.ToBinaryString()}), result: {result} ({result.ToBinaryString()}), shiftLeft; leftShift => {leftShift} => {leftShift}, shiftRight; length - leftShift => {length} - {leftShift} => {length - leftShift}".LogSimple();
					//}
				}
				else {
					int leftShift = uintNum - shift;
					result = (value << leftShift) >> leftShift + originalShift;
					Reset();
					//shift -= uintNum;

					//if (length == 3 && result > 4) {
					//	$"{value.ToBinaryString()} result: {result} ({result.ToBinaryString()}), shift: {shift}, originalShift: {originalShift}".LogSimple();
					//}

					//value = reader.ReadUInt32();
				}
			}
			else {
				int leftShift = uintNum - shift;
				result = (value << leftShift) >> leftShift + originalShift;
				//if (length == 3 && result > 4) {
				//	$"{value.ToBinaryString()} result: {result} ({result.ToBinaryString()}), shift: {shift}, originalShift: {originalShift}".LogSimple();
				//}
			}

			//$"{value.ToBinaryString()}-ReadNum(length: {length}); value: {value} ({value.ToBinaryString().Substring(0, uintNum - shift)}), shift: {shift}, result: {result} ({result.ToBinaryString(length)}), full value: {value.ToBinaryString()}\n".LogSimple();
			return result;
		}
		public static uint ReadNum(this BinaryReader reader, uint length) => reader.ReadNum((int)length);
		public static uint ReadUintC(this BinaryReader reader) => reader.ReadNum((int)reader.ReadNum(uint_c.defaultShift));
		public static string S(this BinaryReader _) {
			return $"value: ({value},  {value.ToBinaryString()}), shift: {shift}";
		}
	}
	public struct uint_b : IComparable<uint_b> {
		public const uint MaxValue = 134217728;//2^27
		private const int significandBits = 27;
		private const uint significandMask = 0b00000111111111111111111111111111;
		private const uint bitsMask = 0b11111000000000000000000000000000;
		public uint value;
		public int Bits => unchecked((int)((value & bitsMask) >> significandBits));
		public uint Significand => value & significandMask;
		public int SignificandInt => unchecked((int)(value & significandMask));
		public uint_b(uint value, uint bits) {
			this.value = (value & significandMask) | (bits << significandBits);
		}

		public override string ToString() {
			return $"value: {value} ({value.ToBinaryString()}), Significand: {Significand}, Bits: {Bits}, WrittenBits: ({Significand.ToBinaryString(Bits)})";
		}

		public int CompareTo(uint_b other) {
			int diff = Bits - other.Bits;
			if (diff != 0)
				return diff;

			return SignificandInt - other.SignificandInt;
		}
		public override int GetHashCode() => unchecked((int)value);
	}
	public struct uint_c : IComparable<uint_c> {
		public const uint MaxValue = 134217728;//2^27
		private const int maxSignificandBits = 27;
		public const int defaultShift = 5;
		private const uint defaultSignificandMask = 0b11111111111111111111111111100000;
		private const uint defaultBitsMask = 0b00000000000000000000000000011111;
		public uint value;
		/// <summary>
		/// SignificandLength is the value that will be read first, 5 bits.  It determines how many bits to read for the actual value.
		/// </summary>
		public int SignificandLength => unchecked((int)(value & defaultBitsMask));
		/// <summary>
		/// Use TotalLength when writing the full value to be stored/sent
		/// </summary>
		public int TotalLength => SignificandLength + defaultShift;
		public uint Significand => value >> defaultShift;
		public uint_c(uint value) {
			this.value = (value << defaultShift) | (defaultBitsMask & value.BitsNeeded());
		}

		public override string ToString() {
			return $"value: {value} ({value.ToBinaryString()}), Significand: {Significand}, SignificandLength: {SignificandLength}, TotalLength: {TotalLength}, WrittenBits: ({Significand.ToBinaryString(TotalLength - defaultShift)})";
		}

		public int CompareTo(uint_c other) {
			int diff = unchecked((int)SignificandLength) - unchecked((int)other.SignificandLength);
			if (diff != 0)
				return diff;

			return unchecked((int)Significand) - unchecked((int)other.Significand);
		}
		public override int GetHashCode() => unchecked((int)value);
	}
}
