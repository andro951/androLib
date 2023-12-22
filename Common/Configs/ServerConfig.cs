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

		//Storage Settings
		[JsonIgnore]
		public const string StorageSettingsKey = "StorageSettings";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{StorageSettingsKey}")]
		[DefaultValue(true)]
		public bool RemoveItemsWhenBlacklisted;

		[DefaultValue(false)]
		public bool ClosingInventoryClosesBags;

		[DefaultValue(false)]
		public bool ReOpenBagSwitcherAutomatically;

		[DefaultValue(false)]
		[ReloadRequired]
		public bool UseAlternateRarityColors;

		[DefaultValue(false)]
		public bool StopProvidingItemsInBagsForCrafting;

		public List<StorageSizePair> StorageSizes = new();

		//Logging Information
		[JsonIgnore]
		public const string LoggingInformationKey = "LoggingInformation";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{LoggingInformationKey}")]

		[DefaultValue(false)]
		[ReloadRequired]
		public bool PrintLocalizationLists;

		[DefaultValue(false)]
		[ReloadRequired]
		public bool PrintItemDrops;

		[DefaultValue(false)]
		public bool DisableAllErrorMessagesInChat {
			set {
				if (value) {
					OnlyShowErrorMessagesInChatOnce = false;
				}
				else {
					LogMethods.LoggedChatMessagesIDs.Clear();
				}

				_disableAllErrorMessagesInChat = value;
			}

			get => _disableAllErrorMessagesInChat;
		}

		[JsonIgnore]
		private bool _disableAllErrorMessagesInChat;

		[DefaultValue(true)]
		public bool OnlyShowErrorMessagesInChatOnce {
			set {
				if (value) {
					DisableAllErrorMessagesInChat = false;
				}
				else {
					LogMethods.LoggedChatMessagesIDs.Clear();
				}

				_onlyShowErrorMessagesInChatOnce = value;
			}

			get => _onlyShowErrorMessagesInChatOnce;
		}

		private bool _onlyShowErrorMessagesInChatOnce;

		[DefaultValue(false)]
		[ReloadRequired]
		public bool LogAllPlayerWhiteAndBlackLists;

		[JsonIgnore]
		public const string ItemListsKey = "ItemLists";
		[Header($"$Mods.{AndroMod.ModName}.{L_ID_Tags.Configs}.{ClientConfigName}.{ItemListsKey}")]

		public List<ItemList> WhiteLists = new();

		public List<ItemList> BlackLists = new();

		public override void OnChanged() {
			StoragePlayer.ClientConfigChanged = true;
			if (!Main.gameMenu)
				StoragePlayer.CheckClientConfigChanged();
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

	public class StorageSizePair {
		[JsonIgnore]
		public const int MaxStorageSize = 10000;
		public string ModFullName => modFullName;
		private string modFullName;
		[Range(1, MaxStorageSize)]
		public int StorageSize;

		public StorageSizePair(string ModFullName, int storageSize) {
			modFullName = ModFullName;
			StorageSize = storageSize;
		}

		public override string ToString() {
			if (modFullName == null)
				return "";

			int underscoreIndex = modFullName.IndexOf("_");
			if (underscoreIndex == -1) {
				return modFullName;
			}
			else {
				return ModFullName.Substring(underscoreIndex + 1).AddSpaces();
			}
		}
	}
}
