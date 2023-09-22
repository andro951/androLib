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
		public const string ClientConfigName = "AndroClientConfig";
		public override ConfigScope Mode => ConfigScope.ClientSide;

		//Display Settings
		[JsonIgnore]
		public const string DisplaySettingsKey = "DisplaySettings";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{DisplaySettingsKey}")]

		[DefaultValue(100)]
		[Range(0, (int)byte.MaxValue)]
		public int UITransparency;

		//Logging Information
		[JsonIgnore]
		public const string LoggingInformationKey = "LoggingInformation";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{LoggingInformationKey}")]

		[DefaultValue(false)]
		[ReloadRequired]
		public bool PrintLocalizationLists;
	}
}
