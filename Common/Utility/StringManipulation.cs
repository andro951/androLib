﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using androLib.Common.Globals;
/*
using androLib.Common.Configs;
using androLib.Items;
*/

namespace androLib.Common.Utility
{
	public static class StringManipulation
    {
        private static readonly char[] upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] lowerCase = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] numbers = "0123456789".ToCharArray();
        private static readonly string[] apla = { "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWKYZ" };

        #region S() Methods

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this StatModifier statModifier) => "<A: " + statModifier.Additive + ", M: " + statModifier.Multiplicative + ", B: " + statModifier.Base + ", F: " + statModifier.Flat + ">";

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this Item item) => item != null ? !item.IsAir ? $"{item.Name}, ID: {item.type.GetItemIDOrName()}, type: {item.type}{(item.ModItem != null ? $", {item.ModItem?.Mod?.Name}" : "")}" : "<Air>" : "null";

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this Projectile projectile) => projectile != null ? $"name: {projectile.Name}, id: {projectile.type}" : "null";

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this Player player) => player != null ? player.name : "null";

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this NPC npc, bool stats = false) => npc != null ? $"id: {npc.type.GetNPCIDOrName()}, netID: {npc.netID}{(npc.type >= NPCID.Count ? $" name: {npc.ModFullName()}" : "")} whoAmI: {npc.whoAmI}{(stats ? $"defense: {npc.defense}, defDefense: {npc.defDefense}, lifeMax: {npc.lifeRegen}, life: {npc.RealLife()}" : "")}" : "null";

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this Dictionary<int, int> dictionary, int key) => "contains " + key + ": " + dictionary.ContainsKey(key) + " count: " + dictionary.Count + (dictionary.ContainsKey(key) ? " value: " + dictionary[key] : "");
        
        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this Dictionary<short, int> dictionary, short key) => "contains " + key + ": " + dictionary.ContainsKey(key) + " count: " + dictionary.Count + (dictionary.ContainsKey(key) ? " value: " + dictionary[key] : "");

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this Dictionary<string, StatModifier> dictionary, string key) => "contains " + key + ": " + dictionary.ContainsKey(key) + " count: " + dictionary.Count + (dictionary.ContainsKey(key) ? " value: " + dictionary[key].S() : "");

        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this bool b) => b ? "true" : "false";
		
        /// <summary>
        /// Convert to a string
        /// </summary>
        public static string S(this DamageClass dc) => dc != null ? dc.Type != (int)DamageClassID.Generic ? ((DamageClassID)dc.Type).ToString() + " " : "" : "";

        public static string S(this float f, int decimals = 4) {
            float correction = (float)Math.Pow(0.1f, decimals + 1);
            if (1f - f + (int)f <= correction)
                f += correction;

            string s = f.ToString($"F{decimals + 1}");

            int dot = s.IndexOf('.');
            if (dot == -1) {
                dot = s.IndexOf(',');
                if (dot == -1)
					return s;
			}

            int length = s.Length;
            int end = length - 1;
            for (; end > dot; end--) {
                char c = s[end - 1];
				if (c != '0' && c != '.' && c != ',') {
                    break;
                }
            }

            if (end == length - 1 && length - dot - 1 > 2) {
                char last = s[end];
                char lastM1 = (char)(last - 1);
                int i = end + 1;
				for (; i > dot + 2; i--) {
                    char c = s[i - 2];
                    if (c != last && c != lastM1 && c != '.' && c != ',') {
                        break;
                    }
                }

                if (i < end) {
					string newStr1 = $"{s.Substring(0, i)}{last}";
					return newStr1;
				}
			}

			string newStr = s.Substring(0, end);

			return newStr;
		}
		public static string S(this (int[], bool[]) spawnInfoCode) => $"(new int[]{"{"} {spawnInfoCode.Item1.Select(i => $"{i}").JoinList(", ")} {"}"}, new bool[]{"{"} {spawnInfoCode.Item2.Select(b => b.S()).JoinList(", ")} {"}"})";
        public static string S(this IEnumerable<string> strings, string nameAndType = "", int tabs = 2, bool isArgument = false) => $"{(isArgument ? "," : "")}\n{tabs.Tabs()}{nameAndType} {"{"}\n{tabs.Tabs(1)}{strings.JoinList($",\n{tabs.Tabs(1)}")}\n{tabs.Tabs()}{"}"}{(isArgument ? "" : ";")}";
        public delegate string ToStringDelegate<T>(T t);
        public static string S<T>(this IEnumerable<T> list, ToStringDelegate<T> toString, string nameAndType = "", int tabs = 2) => list.Select(i => toString(i)).S(nameAndType, tabs);
        public static string NPCListString(this IEnumerable<int> list, string name = null, bool isArgument = false, int tabs = 2) =>
			$"{list.Where(i => i < NPCID.Count).Select(i => $"{i.GetNPCIDName()}").S($"{(isArgument ? $"{name}Types:" : $"SortedSet<int> {name}Types =")} new SortedSet<int>()", tabs, isArgument)}" +
			$"{list.Where(i => i >= NPCID.Count).Select(i => $"{i.GetNPCNameString()}").OrderBy(i => i).S($"{(isArgument ? $"{name}Names:" : $"SortedSet<string> {name}Types =")} new SortedSet<string>()", tabs, isArgument)}";
		public static string NPCDictionaryStrings<T>(this IEnumerable<KeyValuePair<int, T>> dict, string name, ToStringDelegate<T> toString, string typeStringOverride = null, int tabs = 2) =>
			$"{dict.Where(i => i.Key < NPCID.Count).Select(i => $"{i.Key.GetNPCIDName()}, {toString(i.Value)}".Brackets()).S($"SortedDictionary<int, {typeStringOverride ?? typeof(T).Name}> {name}Types = new()", tabs)}" +
			$"{dict.Where(i => i.Key >= NPCID.Count).OrderBy(i => i.Key.GetNPCNameString()).Select(i => $"{i.Key.GetNPCNameString()}, {toString(i.Value)}".Brackets()).S($"SortedDictionary<string, {typeStringOverride ?? typeof(T).Name}> Mod{name}Names = new()", tabs)}";
		public static string NPCDictionaryStrings<T>(this IEnumerable<KeyValuePair<int, IEnumerable<T>>> dict, string name, ToStringDelegate<T> toString, string typeStringOverride = null, int tabs = 2) =>
			$"{dict.Where(i => i.Key < NPCID.Count).Select(i => $"{i.Key.GetNPCIDName()}, {i.Value.Select(i => toString(i)).JoinList(", ")}".Brackets()).S($"SortedDictionary<int, {typeStringOverride ?? typeof(T).Name}> {name}Types = new()", tabs)}" +
			$"{dict.Where(i => i.Key >= NPCID.Count).OrderBy(i => i.Key.GetNPCNameString()).Select(i => $"{i.Key.GetNPCNameString()}, {i.Value.Select(i => toString(i)).JoinList(", ")}".Brackets()).S($"SortedDictionary<string, {typeStringOverride ?? typeof(T).Name}> Mod{name}Names = new()", tabs)}";
		public static string ItemDictionaryStrings<T>(this IEnumerable<KeyValuePair<int, T>> dict, string name, ToStringDelegate<T> toString, string typeStringOverride = null, int tabs = 2) =>
			$"{dict.Where(i => i.Key < ItemID.Count).Select(i => $"{i.Key.GetItemIDName()}, {toString(i.Value)}".Brackets()).S($"SortedDictionary<int, {typeStringOverride ?? typeof(T).Name}> {name}Types = new()", tabs)}" +
			$"{dict.Where(i => i.Key >= ItemID.Count).OrderBy(i => i.Key.GetItemNameString()).Select(i => $"{i.Key.GetItemNameString()}, {toString(i.Value)}".Brackets()).S($"SortedDictionary<string, {typeStringOverride ?? typeof(T).Name}> Mod{name}Names = new()", tabs)}";
		public static string ItemDictionaryStrings<T>(this IEnumerable<KeyValuePair<int, IEnumerable<T>>> dict, string name, ToStringDelegate<T> toString, string typeStringOverride = null, int tabs = 2) =>
			$"{dict.Where(i => i.Key < ItemID.Count).Select(i => $"{i.Key.GetItemIDName()}, {i.Value.Select(i => toString(i)).JoinList(", ")}".Brackets()).S($"SortedDictionary<int, {typeStringOverride ?? typeof(T).Name}> {name}Types = new()", tabs)}" +
			$"{dict.Where(i => i.Key >= ItemID.Count).OrderBy(i => i.Key.GetItemNameString()).Select(i => $"{i.Key.GetItemNameString()}, {i.Value.Select(i => toString(i)).JoinList(", ")}".Brackets()).S($"SortedDictionary<string, {typeStringOverride ?? typeof(T).Name}> Mod{name}Names = new()", tabs)}";
        public static string DictionaryOfItemLists<T>(this IEnumerable<KeyValuePair<T, SortedSet<int>>> dict, string name, ToStringDelegate<T> toString, string typeStringOverride = null, int tabs = 2) =>
            $"{(dict.SelectMany(p => p.Value).Where(i => i < ItemID.Count).Any() ? dict.Where(p => p.Value.Where(i => i < ItemID.Count).Any()).Select(p => $"{toString(p.Key)}, {p.Value.Where(i => i < ItemID.Count).Select(i => i.GetItemIDName()).JoinList(", ")}".Brackets()).S($"SortedDictionary<{typeStringOverride ?? typeof(T).Name}, SortedSet<int>> {name}Types = new()", tabs) : "")}" +
			$"{(dict.SelectMany(p => p.Value).Where(i => i >= ItemID.Count).Any() ? dict.Where(p => p.Value.Where(i => i >= ItemID.Count).Any()).Select(p => $"{toString(p.Key)}, {p.Value.Where(i => i >= ItemID.Count).Select(i => i.GetItemNameString()).JoinList(", ")}".Brackets()).S($"SortedDictionary<{typeStringOverride ?? typeof(T).Name}, SortedSet<string>> Mod{name}Names = new()", tabs) : "")}";
		public static string Brackets(this string s, bool spaces = true) => "{" + (spaces ? " " : "") + s + (spaces ? " " : "") + "}";
		public static string Quotes(this string s) => "\"" + s + "\"";
        public static string GetItemIDName(this int itemType) => ItemID.Search.TryGetName(itemType, out string name) ? $"ItemID.{name}" : $"FailedToFindItemName{itemType}";
        public static string GetNPCIDName(this int netId) => NPCID.Search.TryGetName(netId, out string name) ? $"NPCID.{name}" : $"FailedToFindNPCName{netId}";
        public static string GetItemNameString(this int itemType) => ContentSamples.ItemsByType[itemType].ModFullName().Quotes();
		//public static string ModFullName(this Item item) => item.ModItem?.FullName ?? item.Name;
		public static string GetNPCNameString(this int netId) => ContentSamples.NpcsByNetId[netId].ModFullName().Quotes();
		//public static string ModFullName(this NPC npc) => npc.ModNPC?.FullName ?? npc.type.GetNPCIDOrName();
		public static string GetItemIDOrName(this int itemType) => itemType < ItemID.Count ? itemType.GetItemIDName() : itemType.GetItemNameString();
        public static string GetItemInternalName(this int itemType) => GetItemInternalName(ContentSamples.ItemsByType[itemType]);
		public static string GetItemInternalName(this Item item) => item.type < ItemID.Count ? ItemID.Search.TryGetName(item.type, out string name) ? name : $"FailedToFindItemName{item.S()}" : item.ModFullName();
        public static string GetNPCIDOrName(this int netId) => netId < NPCID.Count ? netId.GetNPCIDName() : netId.GetNPCNameString();
        public static string GetTilIDName(this int tileType) => TileID.Search.TryGetName(tileType, out string name) ? $"TileID.{name}" : $"FailedToFindTileNae{tileType}";
        public static string GetTileNameString(this int tileType) => TileLoader.GetTile(tileType) is ModTile modTile && modTile != null ? modTile.FullName : $"NullModTile{tileType}";
		public static string GetTileIDOrName(this int tileType) => tileType < TileID.Count ? tileType.GetTilIDName() : tileType.GetTileNameString();
		public static string StringList<T>(this IEnumerable<T> enumerable, ToStringDelegate<T> toString, string name = null) => $"{(name != null ? $"{name} " : "")}{enumerable.Select(v => toString(v)).JoinList(", ").Brackets()}";

		#endregion

		public static bool IsUpper(this char c) {
            foreach (char upper in upperCase) {
                if (upper == c)
                    return true;
            }

            return false;
        }
        public static bool IsLower(this char c) {
            foreach (char lower in lowerCase) {
                if (lower == c)
                    return true;
            }

            return false;
        }
        public static bool IsNumber(this char c) {
            foreach (char number in numbers) {
                if (number == c)
                    return true;
            }

            return false;
        }
        public static bool IsUpperOrNumber(this char c) => c.IsUpper() || c.IsNumber();

        /// <summary>
        /// Create a list of words from a string, splitting them when encountering capital letters<br/>
        /// </summary>
        /// <param name="s"></param>
        /// <returns>list of words</returns>
        public static List<string> SplitString(this string s) {
            List<string> list = new List<string>();
            int start = 0;
            int end;
            for (int i = 1; i < s.Length; i++) {
                if (s[i].IsUpper()) {
                    end = i - 1;
                    list.Add(s.Substring(start, end - start + 1));
                    start = end + 1;
                }
                else if (i == s.Length - 1) {
                    end = i;
                    list.Add(s.Substring(start, end - start + 1));
                }
            }

            return list;
        }

        /// <summary>
        /// Gets a folder name from a string, s.
        /// </summary>
        /// <param name="s">File path</param>
        /// <param name="numberOfFolders">How many times to find the searchChar and remove everything before it.  1 will find the folder of the file.</param>
        /// <param name="searchChar">The character that seperates the folders and files in the path.  Default '.'</param>
        /// <returns>New file path after removing the file name (and folders if numberOfFolders > 1)</returns>
        public static string GetFolderName(this string s, int numberOfFolders = 1, char searchChar = '.') {
            int i = s.Length - 1;
            for (int j = 0; j < numberOfFolders; j++) {
                i = s.FindChar(searchChar, false);

                //Not last time loop will run
                if (j != numberOfFolders - 1) {
                    //Remove last folder from the string and continue the loop
                    s = s.Substring(0, i);
                }
            }

            return s.Substring(i + 1);
        }
        public static string GetFileName(this string s, char searchChar = '.') {
            int i = s.FindChar(searchChar, false);

            return s.Substring(i + 1);
        }
        public static int FindChar(this string s, char searchChar, bool startLeft = true) {
            int length = s.Length;
            int i = startLeft ? 0 : length - 1;
            if (startLeft) {
                for (; i < length; i++) {
                    char c = s[i];
                    if (c == searchChar) {
                        return i;
                    }
                }
            }
            else {
                for (; i >= 0; i--) {
                    char c = s[i];
                    if (c == searchChar) {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Used only to first part of a file path.<br/>
        /// Example: androLib.Common.Utility -> Common.Utility
        /// </summary>
        /// <param name="s">File path or namespace</param>
        /// <param name="searchChar">Charachter that seperates the folders and file.  Default '.'</param>
        /// <param name="removeSearchChar">Wether to remove the sperating char.  .Common.Utility vs Common.Utility</param>
        /// <returns>New file path after removing the first part of the file path.</returns>
        public static string RemoveFirstFolder(this string s, char searchChar = '.', bool removeSearchChar = true) {
            int i = s.FindChar(searchChar);

            if (removeSearchChar)
                i++;

            return s.Substring(i);
        }

        private static List<string> LowerCaseAddSpacesStringWords = new() { "of" };

        /// <summary>
        /// Add spaces before capitals and numbers.<br/>
        /// (multiple capatials or numbers in a row will split only the last one.  It assumes there is an abriviation.)<br/>
        /// </summary>
        /// <param name="s"></param>
        /// <returns>String with spaces added.</returns>
        public static string AddSpaces(this string s, bool checkLowerCaseWords = false, string space = " ") {
            if (s == null)
                return s;

            int length = s.Length;
            if (length < 2)
                return s;

            int start = 0;
            int end;
            string finalString = "";
            char previous = s[0];
            char c = s[1];
            for (int i = 2; i < length; i++) {
                char previous2 = previous;
                previous = c;
                c = s[i];
                bool previousUpperOrNumber = previous.IsUpperOrNumber();
                bool currentUppderOrNumber = c.IsUpperOrNumber();
                
                if (!previousUpperOrNumber && currentUppderOrNumber) {
                    if (previous == ' ')
                        continue;

                    end = i - 1;
                    finalString += s.Substring(start, end - start + 1) + space;
                    start = i;
                }
                else if (previousUpperOrNumber && previous2.IsUpperOrNumber() && !currentUppderOrNumber) {
                    if (c == ' ')
                        continue;

                    end = i - 2;
                    finalString += s.Substring(start, end - start + 1) + space;
                    start = i - 1;
                }
            }

            if (start != -1)
                finalString += s.Substring(start);

            if (checkLowerCaseWords) {
                foreach (string word in LowerCaseAddSpacesStringWords) {
                    int index = finalString.IndexOf($"{word} ");
                    if (index > 0) {
                        if (finalString[index - 1] != ' ')
                            finalString = finalString.Substring(0, index - 1) + space + finalString.Substring(index);
                    }
                }
            }

            return finalString;
        }

        /// <summary>
        /// Remove all spaces from a string.
        /// </summary>
        public static string RemoveSpaces(this string s) {
            bool started = false;
            int start = 0;
            int end;
            string finalString = "";
            for (int i = 0; i < s.Length; i++) {
                char c = s[i];
                if (started) {
                    if (c == ' ') {
                        started = false;
                        end = i;
                        finalString += s.Substring(start, end - start);
                    }
                }
                else {
                    if (c != ' ') {
                        started = true;
                        start = i;
                    }
                }
            }
            if (started)
                finalString += s.Substring(start, s.Length - start);

            return finalString;
        }

        /// <summary>
        /// Capitalize the first character in a string.
        /// </summary>
        public static string CapitalizeFirst(this string s) {
            if (s.Length > 0) {
                if (s[0].IsLower())
                    for (int i = 0; i < apla[0].Length; i++) {
                        if (s[0] == apla[0][i]) {
                            char c = apla[1][i];
                            return c + s.Substring(1);
                        }
                    }
            }
            return s;
        }

        /// <summary>
        /// Set the first character in the string to lowercase.
        /// </summary>
        public static string ToFieldName(this string s) {
            if (s.Length > 0) {
                if (s[0].IsUpper())
                    for (int i = 0; i < apla[0].Length; i++) {
                        if (s[0] == apla[1][i]) {
                            char c = apla[0][i];
                            return c + s.Substring(1);
                        }
                    }
            }

            return s;
        }

        /// <summary>
        /// Removes "ProjectileName." from the start of the string.
        /// </summary>
        public static string RemoveProjectileName(this string s) {
            int i = s.IndexOf("ProjectileName.");
            return i == 0 ? s.Substring(15) : s;
        }

        /// <summary>
        /// Counts the number of list L2 strings that are contained in L1 strings.
        /// </summary>
        /// <returns>Number of matches.</returns>
        public static int CheckMatches(this List<string> l1, List<string> l2) {
            int matches = 0;
            foreach (string s in l1) {
                foreach (string s2 in l2) {
                    if (s2.IndexOf(s) > -1) {
                        matches++;
                    }
                }
            }

            return matches;
        }
        public static string CommonToAll<T>(this List<T> list) where T : class {
            if (list == null || list.Count <= 0)
                return "";

            List<string> original = list.Select(t => t.ToString()).ToList();
            List<string> edited = new();
            List<string> matches = new();
            
            //"original".LogSimple();
            //foreach(string s in original) {
            //    s.LogSimple();
			//}
            
            string rS = list[0].ToString();
            string result = "";
            int listCount = list.Count;
            string matchString = "";
            for(int i = 0; i < rS.Length; i++) {
                char c = rS[i];
                bool match = true;
                for (int k = 0; k < listCount; k++) {
                    string orig = original[k];
                    if (!orig.Contains(matchString + c)) {
                        match = false;
                        break;
                    }
                }

                if (match) {
                    if (c != ' ' || matchString != " ")
                        matchString += c;
                }
                else if (matchString != "") {
                    i--;
                    matches.Add(matchString);
                    matchString = "";
                }
            }
            /*
            for(int i = 0; i < rS.Length; i++) {
                for(int j = i; j < rS.Length; j++) {
                    char c = rS[j];
                    bool match = true;
                    for (int k = 0; k < listCount; k++) {
                        string orig = original[k];
                        if (!orig.Contains(matchString + c)) {
                            match = false;
                            break;
                        }
                    }

                    if (match) {
                        //if (c != ' ' || matchString != " ")
                            matchString += c;
                    }
                    else if (matchString != "") {
                        //j--;
                        matches.Add(matchString);
                        matchString = "";
                        break;
                    }
                }
            }
            */
            if (matchString != "") {
                matches.Add(matchString);
            }
            
            //"\nmatches".LogSimple();
            //foreach(string s in matches) {
            //    s.LogSimple();
			//}
            
            int count = matches.Count;
            for (int i = 0; i < count; i++) {
                string s = matches[i];
                bool match = false;
                for(int k = 0; k < count; k++) {
                    if (i == k)
                        continue;

                    matchString = matches[k];
                    if (s != matchString && matchString.Contains(s)) {
                        match = true;
                        break;
					}
				}

                if (!match)
                    result += s;
			}
            
            //"/nresult".LogSimple();
            //result.LogSimple();

            return result;
		}
        public static string FillString(this char c, int num) => num > 0 ? new string(c, num) : "";
        public static string FillString(this string s, int num) {
            string text = "";
            for (int i = 0; i < num; i++) {
                text += s;
            }

            return text;
        }
        public static string JoinLists(this IEnumerable<IEnumerable<string>> lists, string joinString = "<br/>or<br/>") {
            string text = "";
            bool first = true;
            foreach (IEnumerable<string> list in lists) {
                if (first) {
                    first = false;
                }
                else {
                    text += joinString;
                }

                text += list.JoinList();
            }

            return text;
        }
        public static string JoinList(this IEnumerable<string> list, string joinString = "<br/>", string last = null) {
            string text = "";
            bool firstString = true;
            int count = list.Count();
            int i = 0;
            foreach (string s in list) {
                if (firstString) {
                    firstString = false;
                }
                else {
                    text += i == count - 1 ? last ?? joinString : joinString;
                }

                text += s;
                i++;
            }

            return text;
        }
        public static string ToEnchantmentTypeName(this string enchantmentName) => enchantmentName.Substring(0, enchantmentName.IndexOf("Enchantment"));
        public static void PadStrings(this List<string> strings) {
            int max = 0;
            foreach(string s in strings) {
                if (s.Length > max)
                    max = s.Length;
			}

            for(int i = 0; i < strings.Count; i++) {
                string s = strings[i];
                int length = s.Length;
                float leftFloat = (max - length) / 2f;
                int right = (int)leftFloat;
                int left = (int)Math.Round(leftFloat);
                strings[i] = $"{' '.FillString(left)}{s}{' '.FillString(right)}";
            }
		}
        public static bool StartsWith(this string original, string startString) => original.Length >= startString.Length && original.Substring(0, startString.Length) == startString;
		public static string Tabs(this int num) => num > 0 ? new string('\t', num) : "";
		public static string Tabs(this int num, int numAdd) => num + numAdd > 0 ? new string('\t', num + numAdd) : "";
	}
}
