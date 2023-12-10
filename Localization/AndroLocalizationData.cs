using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using static Terraria.Localization.GameCulture;
using androLib.Common.Utility;
using androLib.Common.Globals;
using androLib.Common.Configs;
using Terraria.ID;
using Terraria;

namespace androLib.Localization
{
	public class AndroLocalizationData {
		public static void RegisterSDataPackage() {
			if (Main.netMode == NetmodeID.Server)
				return;

			AndroLogModSystem.RegisterModLocalizationSDataPackage(new(ModContent.GetInstance<AndroMod>, () => AllData, () => ChangedData, () => RenamedKeys, () => RenamedFullKeys, () => SameAsEnglish));
		}

		public static Dictionary<CultureName, string> LocalizationComments = new() {
			{ CultureName.German, "Contributors: @Shiro ᵘʷᵘ#6942, @Fischstäbchen#2603  (All others Google Translated.  Needs review)" },
			{ CultureName.English, "" },
			{ CultureName.Spanish , "Contributors: @DaviReM#8740, @JoeDolca, @Haturok#8191, @Kokopai#2506  (All others Google Translated.  Needs review)" },
			{ CultureName.French , "Contributors: @Soluna#1422, @Olixx12#5354  (All others Google Translated.  Needs review)" },
			{ CultureName.Italian , "Contributors: @Tefra_K" },
			{ CultureName.Polish , "(Google Translated.  No contributions yet)" },
			{ CultureName.Portuguese , "Contributors: @Ninguém#8017, @pedro_123444#8294" },
			{ CultureName.Russian , "Contributed by @4sent4" },
			{ CultureName.Chinese , "1090549930 Kiritan - Github, @2578359679#1491, @An unilolusiality, and @huamx1#1050" }
		};

		private static SortedDictionary<string, SData> allData;
		public static SortedDictionary<string, SData> AllData {
			get {
				if (allData == null) {
					allData = new() {
						{ L_ID1.Items.ToString(), new(children: new() {
							//Intentionally empty.  Filled automatically
						}) },
						{ L_ID1.StorageText.ToString(), new(
							values: new() {
								//Filled Automatically
							},
							dict: new() {
								
						}) },
						{ L_ID1.MagicStorageButtonsText.ToString(), new(
							values: new() {
								//Filled Automatically
							},
							dict: new() {

						}) },
						{ L_ID1.Configs.ToString(), new(children: new() {
							{ nameof(AndroClientConfig), new(children: new() {
								{ nameof(AndroClientConfig.UITransparency), new(dict: new() {
										{ L_ID3.Label.ToString(), nameof(AndroClientConfig.UITransparency).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "The transparency of all UIs that depend on androLib.  0 is invisible, 255 fully saturated." }
								}) },
								{ nameof(AndroClientConfig.UseAlternateRarityColors), new(dict: new() {
									{ L_ID3.Label.ToString(), "Use Alternate Rarity Colors and Textures" },
									{ L_ID3.Tooltip.ToString(), "The default colors are color blind friendly.  The alternate textures have minor differences, but were voted to be kept." }
								}) },
								{ nameof(AndroClientConfig.StorageSizes), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(AndroClientConfig.StorageSizes).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "Allows you to change the size of storages.  \n" +
										"Changing any of the sizes requires you to manually reload mods for the change to take effect." +
										"(The mod creator of the storage must choose for their storage to be configurable for it to show up in this list.  Using the + button will not do anything.)" }
								}) },
								{ nameof(AndroClientConfig.PrintLocalizationLists), new(dict: new() {
										{ L_ID3.Label.ToString(), "Log all translation lists" },
									{ L_ID3.Tooltip.ToString(), "The lists are printed to the client.log when you enter a world.\nThe client.log default location is C:\\Steam\\SteamApps\\common\\tModLoader\\tModLoader-Logs" }
								}) },
								{ nameof(AndroClientConfig.PrintItemDrops), new(dict: new() {
									{ L_ID3.Label.ToString(), "Log a List of Item Drop sources" },
									{ L_ID3.Tooltip.ToString(), "The list is printed to the client.log when you enter a world.\nThe client.log default location is C:\\Steam\\SteamApps\\common\\tModLoader\\tModLoader-Logs" }
								}) },
								{ nameof(AndroClientConfig.LogAllPlayerWhiteAndBlackLists), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(AndroClientConfig.LogAllPlayerWhiteAndBlackLists).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "If true, all player white lists and black lists will be logged to the client.log.\n" +
										"If you make changes to the white/black lists that you think should be standard changes for everyone, please print them and send me your client.log. -andro951" }
								}) },
								{ nameof(AndroClientConfig.RemoveItemsWhenBlacklisted), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(AndroClientConfig.RemoveItemsWhenBlacklisted).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "If true, items will be removed from the storage when blacklisted.\n" +
										"Items can be blacklisted by right clicking on them in the storage with the shift key held." }
								}) },
								{ nameof(AndroClientConfig.ClosingInventoryClosesBags), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(AndroClientConfig.ClosingInventoryClosesBags).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "If true, closing the players inventory will close all bags." }
								}) },
								{ nameof(AndroClientConfig.ReOpenBagSwitcherAutomatically), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(AndroClientConfig.ReOpenBagSwitcherAutomatically).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "Switching to a bag with the bag switcher then closing and reopening the bag will have the original bag \n" +
										"open (bag switcher closed) instead of opening the previously switched to bag." }
								}) },
								{ nameof(AndroClientConfig.DisableAllErrorMessagesInChat), new(dict: new() {
									{ L_ID3.Label.ToString(), "Disable All Error Messages In Chat" },
									{ L_ID3.Tooltip.ToString(), "Prevents messages showing up in your chat that ask you to \n" +
														"Please report this to andro951(Weapon Enchantments) along with a description of what you were doing at the time." }
								}) },
								{ nameof(AndroClientConfig.OnlyShowErrorMessagesInChatOnce), new(dict: new() {
									{ L_ID3.Label.ToString(), "Only show error messages in chat once" },
									{ L_ID3.Tooltip.ToString(), "Messages will continue to show up in your chat, but only once during a game session.\n" +
														"(The error message must be the exact same as a previous message to be prevented.)" }
								}) },
								{ nameof(AndroClientConfig.ForceAllowedListUpdate), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(AndroClientConfig.ForceAllowedListUpdate).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "This will force changes made to the Whitelists and Blacklists that you made by manually adjusting the config to be updated.\n" +
										"This will be immediately turned back off when the update is complete.  It is not required if updating the white/black lists in game.\n" +
										"This is prevents requiring a reload when changing the lists." }
								}) },
								{ nameof(AndroClientConfig.WhiteLists), new(dict: new() {
									{ L_ID3.Label.ToString(), $"{nameof(AndroClientConfig.WhiteLists).AddSpaces()} (Manage in game instead!)" },
									{ L_ID3.Tooltip.ToString(), "Items can be whitelisted in game by manually clicking them into a storage bag with your mouse.\n" +
										"Changing the Mod Full Name will prevent that list from working and a new blank one will be made." }
								}) },
								{ nameof(AndroClientConfig.BlackLists), new(dict: new() {
									{ L_ID3.Label.ToString(), $"{nameof(AndroClientConfig.BlackLists).AddSpaces()} (Manage in game instead!)" },
									{ L_ID3.Tooltip.ToString(), "Items can be blacklisted in game by right clicking on an item in a storage bag with the shift key held.\n" +
										"Changing the Mod Full Name will prevent that list from working and a new blank one will be made." }
								}) },
							},
							dict: new() {
								{ L_ID2.DisplayName.ToString(), "Client Config" },
								{ AndroClientConfig.DisplaySettingsKey, AndroClientConfig.DisplaySettingsKey.AddSpaces() },
								{ AndroClientConfig.LoggingInformationKey, AndroClientConfig.LoggingInformationKey.AddSpaces() },
								{ AndroClientConfig.StorageSettingsKey, AndroClientConfig.StorageSettingsKey.AddSpaces() },
								{ AndroClientConfig.ItemListsKey, "Allowed Lists (Should be modified in game instead!)" }
							}) },
							{ nameof(ItemList), new(children: new() {
								{ nameof(ItemList.ItemDefinitions), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(ItemList.ItemDefinitions).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "Items in the list.  This will override my white/black lists." }
								}) },
								{ nameof(ItemList.ModFullName), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(ItemList.ModFullName).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "Editing this will prevent the list from working.  It is the key used to determine which storage the list belongs to." }
								}) },
							},
							dict: new() {
								{ L_ID3.Tooltip.ToString(), "Allows you to edit the whitelist/blacklist of the bags." },
							}) },
							{ nameof(StorageSizePair), new(children: new() {
								{ nameof(StorageSizePair.ModFullName), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(StorageSizePair.ModFullName).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "Editing this will prevent the size from working.  It is the key used to determine which storage the list belongs to." }
								}) },
								{ nameof(StorageSizePair.StorageSize), new(dict: new() {
									{ L_ID3.Label.ToString(), nameof(StorageSizePair.StorageSize).AddSpaces() },
									{ L_ID3.Tooltip.ToString(), "The size of the storage.  This is the number of items that can be stored in the storage." }
								}) }
							},
							dict: new() {
								{ L_ID3.Tooltip.ToString(), "Allows you to change the size of the storage." }
							}) },
						}) },
						{ L_ID1.AndroLibGameMessages.ToString(), new(
							dict: new() {
								{ AndroLibGameMessages.AddedToWhitelist.ToString(), "{0} whitelisted for the {1}.  Items can be blacklisted by right clicking on them in the {1} with the shift key held." },
								{ AndroLibGameMessages.AddedToBlacklist.ToString(), "{0} blacklisted for the {1}.  Items can be whitelisted by placing them into the {1} with the mouse." },
								{ AndroLibGameMessages.BossChecklistNotEnabled.ToString(), "BossChecklist mod is not enabled.  Weapon Enchantments uses BossChecklist to determine which bosses determine Power Booster drops from Modded bosses.  Since BossChecklist is not enabled, all Modded bosses will drop the regular Power Booster." },
								{ AndroLibGameMessages.FailedDetermineProgression.ToString(), "Failed to determine the progression of Wall of Flesh and Plantera from BossChecklistData" },
								{ AndroLibGameMessages.UnableDetermineNPCDropsBossBag.ToString(), "Unable to determine the npc that drops this boss bag:" },
								{ AndroLibGameMessages.MainUpdateCount.ToString(), "Main.GameUpdateCount: {0}" },
								{ AndroLibGameMessages.ReportErrorToAndro.ToString(), "Please report this to andro951(Weapon Enchantments) along with a description of what you were doing at the time." },
						}) },
						{ L_ID1.GameModeNameIDs.ToString(), new(
							values: new() {
								//Filled Automatically
							},
							dict: new() {

						}) },
					};

					//Mod androMod = ModContent.GetInstance<AndroMod>();
					//IEnumerable<ModItem> modItems = androMod.GetContent<ModItem>();
					//foreach (ModItem modItem in modItems) {
					//	allData[L_ID1.Items.ToString()].Children.Add(modItem.Name, new(dict: new() { { L_ID2.DisplayName.ToString(), modItem.Name.AddSpaces() } }));
					//}

					string StorageTextKey = L_ID1.StorageText.ToString();
					foreach (string stroageText in Enum.GetNames(typeof(StorageTextID))) {
						if (!allData[StorageTextKey].Dict.ContainsKey(stroageText))
							allData[StorageTextKey].Values.Add(stroageText);
					}

					string MagicStorageButtonsTextKey = L_ID1.MagicStorageButtonsText.ToString();
					foreach (string magicStorageButtonsText in Enum.GetNames(typeof(MagicStorageButtonsTextID))) {
						if (!allData[MagicStorageButtonsTextKey].Dict.ContainsKey(magicStorageButtonsText))
							allData[MagicStorageButtonsTextKey].Values.Add(magicStorageButtonsText);
					}

					string GameModeNameIDsKey = L_ID1.GameModeNameIDs.ToString();
					foreach (string gameModeNameIDKey in Enum.GetNames(typeof(GameModeNameID))) {
						if (!allData[GameModeNameIDsKey].Dict.ContainsKey(gameModeNameIDKey))
							allData[GameModeNameIDsKey].Values.Add(gameModeNameIDKey);
					}
				}

				return allData;
			}
		}

		private static List<string> changedData;
		public static List<string> ChangedData {
			get {
				if (changedData == null)
					changedData = new();

				return changedData;
			}

			set => changedData = value;
		}

		private static Dictionary<string, string> renamedFullKeys;
		public static Dictionary<string, string> RenamedFullKeys {
			get {
				if (renamedFullKeys == null)
					renamedFullKeys = new();

				return renamedFullKeys;
			}

			set => renamedFullKeys = value;
		}

		public static Dictionary<string, string> RenamedKeys = new() {
			//{ typeof(ItemCooldown).Name, "AllForOne" },
			//{ DialogueID.HateCrowded.ToString(), "HateCrouded" }
		};

		public static Dictionary<CultureName, List<string>> SameAsEnglish = new() {
			{ CultureName.German,
				new() {
					"Normal",
				}
			},
			{
				CultureName.Spanish,
				new() {
					"Normal",
					"Main.GameUpdateCount: {0}",
				}
			},
			{
				CultureName.French,
				new() {
					"Expert"
				}
			},
			{
				CultureName.Italian,
				new() {
					"Main.GameUpdateCount: {0}"
				}
			},
			{
				CultureName.Polish,
				new() {
					
				}
			},
			{
				CultureName.Portuguese,
				new() {
					"Normal",
					"Main.GameUpdateCount: {0}"
				}
			},
			{
				CultureName.Russian,
				new() {
					"Main.GameUpdateCount: {0}"
				}
			},
			{
				CultureName.Chinese,
				new() {
					
				}
			},
		};
	}
	public class SData
	{
		public List<string> Values;
		public SortedDictionary<string, string> Dict;
		public SortedDictionary<string, SData> Children;
		public SData(List<string> values = null, SortedDictionary<string, string> dict = null, SortedDictionary<string, SData> children = null) {
			Values = values;
			Dict = dict;
			Children = children;
		}
	}
	public static class LocalizationDataStaticMethods
	{
		/// <summary>
		/// Should only be used for items directly in androLib, not items derived from AndroModItem, or the localization will end up in androLib localization.
		/// </summary>
		public static void AddLocalizationTooltip(this ModItem modItem, string tooltip, string name = null) {
			SortedDictionary<string, SData> all = AndroLocalizationData.AllData;
			if (AndroLogModSystem.printLocalization || AndroLogModSystem.printLocalizationKeysAndValues) {
				AndroLocalizationData.AllData[L_ID1.Items.ToString()].Children.Add(modItem.Name, new(dict: new()));
				AndroLocalizationData.AllData[L_ID1.Items.ToString()].Children[modItem.Name].Dict.Add(L_ID1.Tooltip.ToString(), tooltip);
				AndroLocalizationData.AllData[L_ID1.Items.ToString()].Children[modItem.Name].Dict.Add(L_ID2.DisplayName.ToString(), name ?? modItem.Name.AddSpaces());
			}
		}
	}
}
