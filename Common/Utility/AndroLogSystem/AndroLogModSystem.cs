using Hjson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
//using WeaponEnchantments.Common.Globals;
//using WeaponEnchantments.Items;
//using WeaponEnchantments.Localization;
using static Terraria.Localization.GameCulture;
using static androLib.Common.Utility.AndroLogModSystem;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
//using WeaponEnchantments.Common.Utility.LogSystem;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
//using static WeaponEnchantments.Common.Globals.EnchantedItemStaticMethods;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
using System.Reflection.Emit;
using androLib.Common.Utility;
using androLib.Localization;

namespace androLib.Common.Utility
{
    public class AndroLogModSystem : ModSystem {
        public static bool printListOfContributors = false;
        public static bool printLocalization => AndroMod.clientConfig.PrintLocalizationLists && !Debugger.IsAttached;
        public static readonly bool zzzLocalizationForTesting = false;
        public static bool printLocalizationKeysAndValues => AndroMod.clientConfig.PrintLocalizationLists && Debugger.IsAttached;
        private static int localizationValuesCharacterCount = 0;
        //public static readonly bool printWiki = AndroMod.serverConfig.PrintWikiInfo;

        //Only used to print the full list of contributors.
        public static Dictionary<string, string> androLibContributorLinks = new Dictionary<string, string>() {
            //{ "Zorutan", "https://twitter.com/ZorutanMesuta" }
        };

        public static SortedDictionary<string, Contributors> contributorsData = new SortedDictionary<string, Contributors>();
        public static List<string> namesAddedToContributorDictionary = new List<string>();
        private static string localization = "";
        private static string localizationValues = "";
        private static string localizationKeys = "";
        private static int tabs = 0;
        private static List<string> labels;
        private static SortedDictionary<int, SortedDictionary<string, string>> translations;
        private static int culture;
        private static JDataManager jDataManager;
        private static SortedDictionary<string, ModLocalizationSDataPackage> modLocalizationSDataPackages = new();
        private static ModLocalizationSDataPackage ActivePackage;
		public static void RegisterModLocalizationSDataPackage(ModLocalizationSDataPackage package) {
            string name = package.ModName;
            if (modLocalizationSDataPackages.ContainsKey(name)) {
                modLocalizationSDataPackages[name] = package;
			}
            else {
                modLocalizationSDataPackages.Add(name, package);
            }
		}

		public override void OnWorldLoad() {
            PrintContributorsList();

            PrintAllLocalization();

            //Wiki.PrintWiki();
        }
        public static void UpdateContributorsList<T>(T modTypeWithTexture, string sharedName = null) {
            if (!printListOfContributors)
                return;

            //Already added
            if (sharedName != null && namesAddedToContributorDictionary.Contains(sharedName))
                return;

            Type thisObjectsType = modTypeWithTexture.GetType();
            string texture = (string)thisObjectsType.GetProperty("Texture").GetValue(modTypeWithTexture);
            string artist = (string)thisObjectsType.GetProperty("Artist").GetValue(modTypeWithTexture);
            string designer = (string)thisObjectsType.GetProperty("Designer").GetValue(modTypeWithTexture);

            if (!contributorsData.ContainsKey(texture))
                contributorsData.Add(texture, new Contributors(artist, designer));

            if (sharedName != null)
                namesAddedToContributorDictionary.Add(sharedName);
        }
        public struct ModLocalizationSDataPackage {
            public ModLocalizationSDataPackage(Func<Mod> mod, Func<SortedDictionary<string, SData>> allData, Func<List<string>> changedData, Func<Dictionary<string, string>> renamedKeys, Func<Dictionary<string, string>> renamedFullKeys, Func<Dictionary<CultureName, List<string>>> sameAsEnglish) {
				this.mod = mod;
				this.allData = allData;
                this.changedData = changedData;
                this.renamedKeys = renamedKeys;
                this.renamedFullKeys = renamedFullKeys;
                this.sameAsEnglish = sameAsEnglish;
			}

            private Func<Mod> mod;
            public Mod Mod => mod();
            public string ModName => Mod.Name;
            private Func<SortedDictionary<string, SData>> allData;
            public SortedDictionary<string, SData> AllData => allData();
            private Func<List<string>> changedData;
            public List<string> ChangedData => changedData();
            private Func<Dictionary<string, string>> renamedKeys;
            public Dictionary<string, string> RenamedKeys => renamedKeys();
            private Func<Dictionary<string, string>> renamedFullKeys;
            public Dictionary<string, string> RenamedFullKeys => renamedFullKeys();
            private Func<Dictionary<CultureName, List<string>>> sameAsEnglish;
            public Dictionary<CultureName, List<string>> SameAsEnglish => sameAsEnglish();
		}
        private static void PrintAllLocalization() {
            if (!printLocalization && !printLocalizationKeysAndValues)
                return;

            foreach (ModLocalizationSDataPackage package in modLocalizationSDataPackages.Values) {
                ActivePackage = package;
				jDataManager = new();
                Mod mod = ActivePackage.Mod;
				TmodFile file = (TmodFile)typeof(Mod).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(mod);
				translations = new();
				IEnumerable<int> cultures = Enum.GetValues(typeof(CultureName)).Cast<CultureName>().Where(n => n != CultureName.Unknown).Select(n => (int)n);
				MethodInfo loadTranslationsInfo = typeof(LocalizationLoader).GetMethod("LoadTranslations", BindingFlags.NonPublic | BindingFlags.Static);
				foreach (int i in cultures) {
					SortedDictionary<string, string> cultureTranslations = new();
					GameCulture gameCulture = FromLegacyId(i);
					List<(string, string)> loadedTranslationsList = (List<(string, string)>)loadTranslationsInfo.Invoke(null, new object[] { mod, gameCulture });
					foreach ((string key, string value) t in loadedTranslationsList) {
						cultureTranslations.Add(t.key, t.value);
					}

					translations.Add(i, cultureTranslations);
				}

				foreach (int i in cultures) {
					PrintLocalization(i);
				}
			}
        }
        private class JDataManager {
            private JData master = new();
            private JData active;
            private CultureName cultureName;
            public void Start(CultureName CultureName) {
                master.Clear();
                active = master;
                cultureName = CultureName;
                culture = (int)cultureName;
                AddKey("Mods");
                AddKey(ActivePackage.ModName);
            }
            public void AddKey(string key) {
                active.Children.Add(key, new(parent: active));
                active = active.Children[key];
            }
            public void Add(string key, string value) => active.Dict.Add(key, value);
            public void FinishedKey() {
                active = active.Parent;
            }
            public void End() {
                PrintStart();

                PrintData(master);

                PrintEnd();
            }
            private void PrintStart() {
                if (printLocalization) {
                    string label = $"\n\n{cultureName}\n#{AndroLocalizationData.LocalizationComments[cultureName]}";
                    localization += label;
                }

                if (printLocalizationKeysAndValues) {
                    localizationValuesCharacterCount = 0;
                    string keyLabel = $"#{AndroLocalizationData.LocalizationComments[cultureName]}";
                    localizationKeys += keyLabel;

                    string valueLabel = "";
                    localizationValues += valueLabel;
                }

                labels = new();
            }
            private void PrintKey(string label) {
                string tabsString = $"\n{Tabs(tabs)}{label}: {"{"}";
                if (printLocalization)
                    localization += tabsString;

                if (printLocalizationKeysAndValues)
                    localizationKeys += tabsString;

                tabs++;
                labels.Add(label);
            }
            private void PrintData(JData jData) {
                PrintDict(jData.Dict);
                foreach (KeyValuePair<string, JData> child in jData.Children) {
                    PrintKey(child.Key);
                    PrintData(child.Value);
                    PrintFinishedKey();
                }
            }
            private void PrintDict(SortedDictionary<string, string> dict) {
                string tabString = Tabs(tabs);
                string allLabels = string.Join(".", labels.ToArray());
                foreach (KeyValuePair<string, string> p in dict) {
                    string key = $"{allLabels}.{p.Key}";
                    string s = null;
                    if (translations[culture].ContainsKey(key)) {
                        s = translations[culture][key];
                        if (culture == (int)CultureName.English) {
                            if (s != p.Value)
								ActivePackage.ChangedData.Add(key);
                        }

                        if (ActivePackage.ChangedData.Contains(key))
                            s = p.Value;
                    }
                    else {
                        if (culture == (int)CultureName.English) {
                            if (ActivePackage.RenamedKeys.ContainsKey(p.Key)) {
                                string renamedKey = ActivePackage.RenamedKeys[p.Key];
                                string newKey = $"{allLabels}.{renamedKey}";
                                string newS = translations[culture][newKey];
                                if (newS != renamedKey.AddSpaces())
									ActivePackage.RenamedFullKeys.Add(key, newKey);
                            }
                        }

                        if (ActivePackage.RenamedFullKeys.ContainsKey(key)) {
                            string newKey = ActivePackage.RenamedFullKeys[key];
                            string newS = translations[culture][newKey];
                            if (newS != newKey) {
                                key = newKey;
                                s = translations[culture][key];
                            }
                        }

                        //Try pulling from WeaponEnchantments.
                        //if (s == null) {
                        //    string weaponEnchantmentsKey = key.Replace("androLib", "WeaponEnchantments").Replace(L_ID1.StorageText.ToString(), L_ID1.EnchantmentStorageText.ToString());
						//	if (translations[culture].ContainsKey(weaponEnchantmentsKey))
                        //        s = translations[culture][weaponEnchantmentsKey];
						//}
                    }

                    s ??= key;

                    //$"{key}: {s}".Log();

                    if (s == key)
                        s = p.Value;

                    bool noLocalizationFound = s == p.Value && (culture == (int)CultureName.English || !ActivePackage.SameAsEnglish[(CultureName)culture].Contains(s));

                    s = s.Replace("\"", "\\\"");
                    if ((s.Contains("{") || s.Contains("\"")) && s[0] != '"' && s[0] != '“' && s[0] != '”' && !s.Contains('\n'))
                        s = $"\"{s}\"";

                    if (zzzLocalizationForTesting) {
                        if (s[s.Length - 1] == '"') {
                            s = $"{s.Substring(0, s.Length - 1)}zzz\"";
                        }
                        else {
                            s += "zzz";
                        }
                    }

                    s = CheckTabOutLocalization(s);
                    if (printLocalization)
                        localization += $"\n{tabString}{p.Key}: {s}";

                    if (printLocalizationKeysAndValues) {
                        localizationKeys += $"\n{tabString}{p.Key}: {(!noLocalizationFound ? s : "")}";

                        if (noLocalizationFound) {
                            string valueString = s.Replace("\t", "");
                            int length = valueString.Length;
                            if (localizationValuesCharacterCount + length > 5000) {
                                localizationValues += $"\n{'_'.FillString(4999 - localizationValuesCharacterCount)}";
                                localizationValuesCharacterCount = 0;
                                int newLineIndex = valueString.IndexOf("\n");
                                string checkString = newLineIndex > -1 ? valueString.Substring(0, newLineIndex) : valueString;
                                if (checkString.Contains("'''"))
                                    localizationValues += "\n";
                            }

                            localizationValuesCharacterCount += length + 1;

                            localizationValues += $"{(localizationValues != "" ? "\n" : "")}{valueString}";
                        }
                    }
                }
            }
            private void PrintFinishedKey() {
                tabs--;
                if (tabs < 0)
                    return;

                string tabsString = $"\n{Tabs(tabs)}{"}"}";
                if (printLocalization)
                    localization += tabsString;

                if (printLocalizationKeysAndValues)
                    localizationKeys += tabsString;

                labels.RemoveAt(labels.Count - 1);
            }
            private void PrintEnd() {
                while (tabs >= 0) {
                    PrintFinishedKey();
                }

                tabs = 0;
                if (printLocalization) {
                    localization.LogSimple();
                    localization = "";
                }

                if (printLocalizationKeysAndValues) {
                    string cultureName = ((CultureName)culture).ToLanguageName();
                    localizationKeys = localizationKeys.ReplaceLineEndings();
                    string keyFilePath = @$"C:\Users\Isaac\Desktop\TerrariaDev\Localization Merger\{ActivePackage.ModName}\Keys\{cultureName}.txt";
                    File.WriteAllText(keyFilePath, localizationKeys);
                    localizationKeys = "";

                    string valueFilePath = @$"C:\Users\Isaac\Desktop\TerrariaDev\Localization Merger\{ActivePackage.ModName}\In\{cultureName}.txt";
                    File.WriteAllText(valueFilePath, localizationValues);
                    localizationValues = "";
                }
            }
        }
        private class JData {
            public SortedDictionary<string, string> Dict;
            public SortedDictionary<string, JData> Children;
            public JData Parent;
            public int Count => Dict.Count + Children.Select(c => c.Value.Count).Sum();

            public JData(SortedDictionary<string, string> dict = null, SortedDictionary<string, JData> children = null, JData parent = null) {
                Dict = dict ?? new();
                Children = children ?? new();
                Parent = parent;
            }

            public bool HasParent(out JData parent) {
                parent = Parent;

                return parent != null;
            }
            public void Clear() {
                Dict.Clear();
                Children.Clear();
            }
        }
        public static void PrintLocalization(int cultureName) {
            jDataManager.Start((CultureName)cultureName);

            FromLocalizationData();

            jDataManager.End();
        }
        private static void FromLocalizationData() => GetFromSDataDict(ActivePackage.AllData);
        private static void GetFromSDataDict(SortedDictionary<string, SData> dict) {
            foreach (KeyValuePair<string, SData> pair in dict) {
                jDataManager.AddKey(pair.Key);
                GetFromSData(pair.Value);
                jDataManager.FinishedKey();
            }
        }
        private static void GetFromSData(SData d) {
            if (d.Values != null)
                GetLocalizationFromList(null, d.Values);

            if (d.Dict != null)
                GetLocalizationFromDict(null, d.Dict);

            if (d.Children != null)
                GetFromSDataDict(d.Children);
        }
        private static void GetLocalizationFromCommonLabelList(string label, IEnumerable<string> uniqueLabels, string commonLabel, bool ignoreLabel = false, bool printMaster = false) {
            SortedDictionary<string, string> dict = new();
            foreach (string s in uniqueLabels) {
                dict.Add($"{s}.{commonLabel}", $"{s.AddSpaces()}");
            }

            GetLocalizationFromDict(label, dict, ignoreLabel, printMaster);
        }
        private static void GetLocalizationFromList(string label, IEnumerable<ModType> list, bool ignoreLabel = false, bool printMaster = false) {
            IEnumerable<string> listNames = list.Select(l => l.Name);
            GetLocalizationFromList(label, listNames, ignoreLabel, printMaster);
        }
        private static void GetLocalizationFromList(string label, IEnumerable<string> list, bool ignoreLabel = false, bool printMaster = false) {
            SortedDictionary<string, string> dict = new();
            foreach (string s in list) {
                dict.Add($"{s}", $"{s.AddSpaces()}");
            }

            GetLocalizationFromDict(label, dict, ignoreLabel, printMaster);
        }
        private static void GetLocalizationFromDict(string label, SortedDictionary<string, string> dict, bool ignoreLabel = false, bool printMaster = false) {
            ignoreLabel = ignoreLabel || label == null || label == "";
            if (!ignoreLabel)
                jDataManager.AddKey(label);

            foreach (KeyValuePair<string, string> p in dict) {
                jDataManager.Add(p.Key, p.Value);
            }

            if (!ignoreLabel)
                jDataManager.FinishedKey();
        }
        private static void GetLocalizationFromListAddToEnd(string label, IEnumerable<string> list, string addString, int tabsNum) {
            List<string> newList = ListAddToEnd(list, addString);
            GetLocalizationFromList(label, newList);
        }
        private static List<string> ListAddToEnd(IEnumerable<string> iEnumerable, string addString) {
            List<string> list = iEnumerable.ToList();
            for (int i = 0; i < list.Count; i++) {
                list[i] += addString;
            }

            return list;
        }
        private static string Tabs(int num) => num > 0 ? new string('\t', num) : "";
        private static string CheckTabOutLocalization(string s) {
            if (s.Contains("'''"))
                return s;

            if (!s.Contains('\n'))
                return s;

            tabs++;
            string newString = $"\n{Tabs(tabs)}'''\n";
            int start = 0;
            int i = 0;
            for (; i < s.Length; i++) {
                if (s[i] == '\n') {
                    newString += Tabs(tabs) + s.Substring(start, i - start + 1);
                    start = i + 1;
                }
            }

            if (s[s.Length - 1] != '\n')
				newString += Tabs(tabs) + s.Substring(start, i - start);

			newString += $"\n{Tabs(tabs)}'''";
            tabs--;

            return newString;
        }
        private static void PrintContributorsList() {
            if (!printListOfContributors)
                return;

            if (contributorsData.Count <= 0)
                return;

            //New dictionary with artist names as the key
            SortedDictionary<string, List<string>> artistCredits = new SortedDictionary<string, List<string>>();
            foreach (string key in contributorsData.Keys) {
                string artistName = contributorsData[key].Artist;
                if (artistName != null) {
                    if (artistCredits.ContainsKey(artistName)) {
                        artistCredits[artistName].Add(key);
                    }
                    else {
                        artistCredits.Add(artistName, new List<string>() { key });
                    }
                }
            }

            //Create and print the GitHub Artist credits.
            string artistsMessage = "";
            foreach (string artistName in artistCredits.Keys) {
                artistsMessage += $"\n{artistName}: ";
                if (androLibContributorLinks.ContainsKey(artistName))
                    artistsMessage += androLibContributorLinks[artistName];

                artistsMessage += "\n\n";
                foreach (string texture in artistCredits[artistName]) {
                    artistsMessage += $"![{texture.GetFileName('/')}]({texture.RemoveFirstFolder('/', false)}.png)\n";
                }
            }

            artistsMessage.Log();

            namesAddedToContributorDictionary.Clear();
            contributorsData.Clear();
        }
	}

	public struct Contributors {
		public Contributors(string artist, string designer) {
			Artist = artist;
			Designer = designer;
		}

		public string Artist;
		public string Designer;
	}
}

