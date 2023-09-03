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

namespace androLib.Localization
{
	public class AndroLocalizationData {
		public static void RegisterSDataPackage() {
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
						{ L_ID1.Config.ToString(), new(children: new() {
							{ nameof(AndroClientConfig.UITransparency), new(dict: new() {
								{ L_ID3.Label.ToString(), nameof(AndroClientConfig.UITransparency).AddSpaces() }
							}) },
							{ nameof(AndroClientConfig.PrintLocalizationLists), new(dict: new() {
								{ L_ID3.Label.ToString(), "Log all translation lists" },
								{ L_ID3.Tooltip.ToString(), "The lists are printed to the client.log when you enter a world.\nThe client.log default location is C:\\Steam\\SteamApps\\common\\tModLoader\\tModLoader-Logs" }
							}) },
							}, dict: new() {
								{ "DisplaySettings", "Display Settings" },
								{ "LoggingInformation", "Logging Information" }/*,
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" },
								{ "", "" }
								*/
						}) }
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
					
				}
			},
			{
				CultureName.Spanish,
				new() {
					
				}
			},
			{
				CultureName.French,
				new() {
					
				}
			},
			{
				CultureName.Italian,
				new() {
					
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
					
				}
			},
			{
				CultureName.Russian,
				new() {

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
