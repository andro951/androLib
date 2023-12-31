﻿using System;
using System.Linq;
using Terraria;
using androLib.Common.Utility;
using static androLib.AndroMod;

namespace androLib.Common.Configs
{
	public class ConfigValues {
		public static byte UIAlpha => (byte)(byte.MaxValue - clientConfig.UITransparency);
		public static byte UIAlphaHovered => (byte)Math.Min(UIAlpha + 20, byte.MaxValue);
	}
}