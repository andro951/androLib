using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Terraria.ID;
using androLib.Common.Globals;
using androLib.Common.Utility;
using Terraria;

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

		[JsonIgnore]
		public const string StorageSettingsKey = "StorageSettings";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{StorageSettingsKey}")]
		[DefaultValue(true)]
		public bool RemoveItemsWhenBlacklisted;

		[DefaultValue(false)]
		public bool ClosingInventoryClosesBags;

		//Logging Information
		[JsonIgnore]
		public const string LoggingInformationKey = "LoggingInformation";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{LoggingInformationKey}")]

		[DefaultValue(false)]
		[ReloadRequired]
		public bool PrintLocalizationLists;

		[DefaultValue(false)]
		[ReloadRequired]
		public bool LogAllPlayerWhiteAndBlackLists;

		[JsonIgnore]
		public const string ItemListsKey = "ItemLists";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{ItemListsKey}")]

		[DefaultValue(false)]
		public bool ForceAllowedListUpdate;

		public List<ItemList> WhiteLists = new();

		public List<ItemList> BlackLists = new();

		public override void OnChanged() {
			if  (ForceAllowedListUpdate) {
				StoragePlayer.ClientConfigChanged = true;
				ForceAllowedListUpdate = false;
				if (!Main.gameMenu)
					StoragePlayer.CheckClientConfigChanged();

			}
		}
	}

	public class ItemList
	{
		public ItemList(string ModFullName) {
			modFullName = ModFullName;
		}

		public List<ItemDefinition> ItemDefinitions => itemDefinitions;
		private List<ItemDefinition> itemDefinitions = new();

		public string ModFullName => modFullName;
		private string modFullName;

		public override string ToString() {
			return modFullName;//TODO:
		}
	}
}
