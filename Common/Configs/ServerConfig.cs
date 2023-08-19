using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Terraria.ID;
using androLib.Common.Globals;
using androLib.Common.Utility;

namespace androLib.Common.Configs
{
	//public class AndroServerConfig : ModConfig
	//{
	//	public override ConfigScope Mode => ConfigScope.ServerSide;

		
	//}

	public class AndroClientConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;

		//Display Settings
		[Header("$Mods.androLib.Config.DisplaySettings")]

		[Label("$Mods.androLib.Config.UITransparency.Label")]
		[DefaultValue(100)]
		[Range(0, (int)byte.MaxValue)]
		public int UITransparency;

		[Header("$Mods.androLib.Config.LoggingInformation")]

		[Label("$Mods.androLib.Config.PrintLocalizationLists.Label")]
		[Tooltip("$Mods.androLib.Config.PrintLocalizationLists.Tooltip")]
		[DefaultValue(false)]
		[ReloadRequired]
		public bool PrintLocalizationLists;
	}
}
