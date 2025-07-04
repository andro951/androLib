﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using androLib.Common.Utility;
using androLib.Items;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Terraria.Audio;
using ReLogic.Content;

namespace androLib.Common.Utility
{
    public static class AndroUtilityMethods {

		#region ModSpecific


		#endregion

		#region General

		public static Item CSI(this int type) => ContentSamples.ItemsByType[type];
        public static Item CSI(this short type) => ContentSamples.ItemsByType[type];
		public static NPC CSNPC(this int netID) => ContentSamples.NpcsByNetId[netID];
        public static NPC CSNPC(this short netID) => ContentSamples.NpcsByNetId[netID];
		public static void ReplaceItemWithCoins(ref Item item, int coins) {
            int coinType = ItemID.PlatinumCoin;
            int coinValue = 1000000;
            while (coins > 0) {
                int numCoinsToSpawn = coins / coinValue;
                if (numCoinsToSpawn > 0) {
                    item = new Item(coinType, numCoinsToSpawn + 1);
                    return;
                }

                coins %= coinValue;
                coinType--;
                coinValue /= 100;
            }
        }
        public static void SpawnCoins(this Player player, int coinValue) {
            int valuePerCoin = 1000000;
            for (int i = 3; i >= 0; i--) {
                int coins = coinValue / valuePerCoin;
                coinValue %= valuePerCoin;
                valuePerCoin /= 100;
                if (coins > 0)
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), ItemID.CopperCoin + i, coins);
            }
        }
		public static bool AnyFavoritedItem(this IEnumerable<Item> items, Func<Item, bool> doesntCountTowardsTotal = null) {
			foreach (Item item in items) {
				if (item.NullOrAir())
					continue;

				if (item.favorited == true && (doesntCountTowardsTotal == null || !doesntCountTowardsTotal(item)))
					return true;
			}

			return false;
		}
        public static bool AnyFavoritedItem(this IEnumerable<KeyValuePair<int, Item>> indexItemsPairs, Func<Item, bool> doesntCountTowardsTotal) => indexItemsPairs.Select(p => p.Value).AnyFavoritedItem();

        /// <summary>
		/// Randomly selects an item from the list if the chance is higher than the randomly generated float.<br/>
        /// <c>This can be done with var rand = new WeightedRandom<Item>(Main.rand)<br/>
        /// rand.Add(item, chance)<br/>
        /// rand.Add(new Item(), chanceN) //chance to get nothing<br/>
        /// Item chosen = rand.Get()<br/></c>
		/// </summary>
		/// <param name="options">Posible items to be selected.</param>
		/// <param name="chance">Chance to select an item from the list.</param>
		/// <returns>Item selected or null if chance was less than the generated float.</returns>
		public static T GetOneFromList<T>(this List<T> options, float chance = 1f) where T : new() {
            if (options.Count == 0)
                return new T();

            if (chance <= 0f)
                return new T();

            if (chance > 1f)
                chance = 1f;

            //Example: items contains 4 items and chance = 0.4f (40%)
            float randFloat = Main.rand.NextFloat();//Example randFloat = 0.24f
            if (randFloat < chance) {
                float count = options.Count;// = 4f
                float chancePerItem = chance / count;// chancePerItem = 0.4f / 4f = 0.1f.  (10% chance each item)  
                int chosenItemNum = (int)(randFloat / chancePerItem);// chosenItemNum = (int)(0.24f / 0.1f) = (int)(2.4f) = 2.

                return options[chosenItemNum];// items[2] being the 3rd item in the list.
            }

            //If the chance is less than the generated float, return new.
            return new T();
        }
        public static T GetOneFromWeightedList<T>(this List<(float, T)> options, float chance) where T : new() {
            if (options.Count == 0)
                return new T();

            if (chance <= 0f)
                return new T();

            if (chance > 1f)
                chance = 1f;

            float randFloat = Main.rand.NextFloat();
            if (randFloat <= chance) {
                float total = 0f;
                foreach ((float, T) pair in options) {
                    total += pair.Item1;
                }

                total *= randFloat / chance;

                foreach ((float, T) pair in options) {
                    total -= pair.Item1;
                    if (total <= 0f)
                        return pair.Item2;
                }
            }

            return new T();
        }
        public static T GetOneFromWeightedList<T>(this SortedList<int, T> options, float chance) where T : new() {
            List<(float, T)> newList = new List<(float, T)>();
            foreach (KeyValuePair<int, T> pair in options) {
                newList.Add(((float)pair.Key, pair.Value));
            }

            return newList.GetOneFromWeightedList(chance);
		}
		public static int GetOneFromWeightedList(this IEnumerable<WeightedPair> options, float chance) {
			if (options.Count() == 0)
				return 0;

			if (chance <= 0f)
				return 0;

			if (chance > 1f)
				chance = 1f;

			float randFloat = Main.rand.NextFloat();
			if (randFloat <= chance) {
				float total = 0f;
				foreach (WeightedPair pair in options) {
					total += pair.Weight;
				}

				total *= randFloat / chance;

				foreach (WeightedPair pair in options) {
					total -= pair.Weight;
					if (total <= 0f)
						return pair.ID;
				}
			}

			return 0;
		}
		public static int GetOneFromWeightedList(this IEnumerable<DropData> options, float chance) {
			if (options.Count() == 0)
				return 0;

			if (chance <= 0f)
				return 0;

			if (chance > 1f)
				chance = 1f;

			float randFloat = Main.rand.NextFloat();
			if (randFloat <= chance) {
				float total = 0f;
				foreach (DropData pair in options) {
					total += pair.Weight;
				}

				total *= randFloat / chance;

				foreach (DropData pair in options) {
					total -= pair.Weight;
					if (total <= 0f)
						return pair.ID;
				}
			}

			return 0;
		}
		public static float Percent(this float value) => value * 100f;
        public static string PercentString(this float value, int decimals = 0) => $"{(value * 100).S(decimals + 2)}%";
		public static float ToSign(this bool value) => value ? 1f : -1f;
		public static string Lang(this string s, string modName, string m) => s.Lang(modName, out string result, m) ? result : "";
		public static bool Lang(this string s, string modName, out string result, string m) {
			string key = $"Mods.{modName}.{m}.{s}";
			result = Language.GetTextValue(key);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string Lang(this string s, string modName, L_ID1 id = L_ID1.Tooltip) => s.Lang(modName, out string result, id) ? result : "";
		public static bool Lang(this string s, string modName, out string result, L_ID1 id = L_ID1.Tooltip) {
			string key = $"Mods.{modName}.{id}.{s}";
			result = Language.GetTextValue(key);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string Lang(this string s, string modName, L_ID1 id, string m) => s.Lang(modName, out string result, id, m) ? result : "";
		public static bool Lang(this string s, string modName, out string result, L_ID1 id, string m) {
			string key = $"Mods.{modName}.{id}.{m}.{s}";
			result = Language.GetTextValue(key);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string Lang(this string s, string modName, L_ID1 id, L_ID2 id2) => s.Lang(modName, out string result, id, id2) ? result : "";
		public static bool Lang(this string s, string modName, out string result, L_ID1 id, L_ID2 id2) {
			string key = $"Mods.{modName}.{id}.{id2}.{s}";
			result = Language.GetTextValue(key);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string Lang(this string s, string modName, L_ID1 id, L_ID2 id2, string m) => s.Lang(modName, out string result, id, id2, m) ? result : "";
		public static bool Lang(this string s, string modName, out string result, L_ID1 id, L_ID2 id2, string m) {
			string key = $"Mods.{modName}.{id}.{id2}.{m}.{s}";
			result = Language.GetTextValue(key);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string Lang(this int i, L_ID_V id) {
			switch (id) {
				case L_ID_V.Item:
					return Terraria.Lang.GetItemNameValue(i);
				case L_ID_V.NPC:
					return Terraria.Lang.GetNPCNameValue(i);
				case L_ID_V.Buff:
					return Terraria.Lang.GetBuffName(i);
				case L_ID_V.BuffDescription:
					return Terraria.Lang.GetBuffDescription(i);
			}

			return null;
		}

		public static string Lang(this string s, string modName, string m, IEnumerable<string> args) => s.Lang(modName, out string result, m, args) ? result : "";
		public static bool Lang(this string s, string modName, out string result, string m, IEnumerable<string> args) {
			string key = $"Mods.{modName}.{m}.{s}";
			result = Language.GetTextValue(key, args);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string Lang(this string s, string modName, L_ID1 id, L_ID2 id2, IEnumerable<object> args) => s.Lang(modName, out string result, id, id2, args) ? result : "";
		public static bool Lang(this string s, string modName, out string result, L_ID1 id, L_ID2 id2, IEnumerable<object> args) {
			string key = $"Mods.{modName}.{id}.{id2}.{s}";
			result = args != null ? Language.GetTextValue(key, args.ToArray()) : Language.GetTextValue(key);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string Lang(this string s, string modName, L_ID1 id, IEnumerable<object> args) => s.Lang(modName, out string result, id, args) ? result : "";
		public static bool Lang(this string s, string modName, out string result, L_ID1 id, IEnumerable<object> args) {
			string key = $"Mods.{modName}.{id}.{s}";
			result = args != null ? Language.GetTextValue(key, args.ToArray()) : Language.GetTextValue(key);

			if (result == key) {
				return false;
			}

			return true;
		}
		public static string GetTextValue(this string key, IEnumerable<object> args) {
            return Language.GetTextValue(key, args);
        }

		#region AddOrCombine

		public static void AddOrCombine(this IDictionary<int, int> dict1, IDictionary<int, int> dict2) {
            foreach (var pair in dict2) {
                dict1.AddOrCombine(pair);
            }
        }
        public static void AddOrCombine(this IDictionary<int, int> dict1, KeyValuePair<int, int> pair) {
            int key = pair.Key;
            if (dict1.ContainsKey(key)) {
                dict1[key] += pair.Value;
            }
            else {
                dict1.Add(key, pair.Value);
            }
        }
        public static void AddOrCombine<T>(this IDictionary<T, int> dict1, (T, int) pair) =>
            dict1.AddOrCombine(pair.Item1, pair.Item2);
        public static void AddOrCombine<T>(this IDictionary<T, int> dict, T key, int value) {
            if (dict.ContainsKey(key)) {
                dict[key] += value;
            }
            else {
                dict.Add(key, value);
            }
        }
        public static void TrySubtractRemove<T>(this IDictionary<T, int> dict, T key, int value) {
            if (dict.ContainsKey(key)) {
                dict[key] -= value;
                if (dict[key] <= 0)
                    dict.Remove(key);
            }
        }
        public static void AddOrCombine<TKey, T>(this IDictionary<TKey, List<T>> dict, TKey key, T value) {
			if (dict.ContainsKey(key)) {
				dict[key].Add(value);
			}
			else {
				dict.Add(key, new() { value });
			}
		}
        public static void AddOrCombine<TKey, T>(this IDictionary<TKey, HashSet<T>> dict, TKey key, T value) {
            if (dict.ContainsKey(key)) {
                dict[key].Add(value);
            }
            else {
                dict.Add(key, new() { value });
            }
        }
        public static void AddOrCombine<TKey, T>(this IDictionary<TKey, SortedSet<T>> dict, TKey key, T value) {
            if (dict.ContainsKey(key)) {
                dict[key].Add(value);
            }
            else {
                dict.Add(key, new() { value });
            }
        }
        public static void AddOrCombine<TKey, T>(this IDictionary<TKey, HashSet<T>> dict, TKey key, HashSet<T> value) {
            if (dict.ContainsKey(key)) {
                dict[key] = dict[key].Concat(value).ToHashSet();
            }
            else {
                dict.Add(key, value);
            }
        }
        public static void AddOrCombine<TKey, T>(this IDictionary<TKey, (HashSet<T>, HashSet<T>)> dict, TKey key, (HashSet<T>, HashSet<T>) value) {
            if (dict.ContainsKey(key)) {
                dict[key].Item1.UnionWith(value.Item1);
                dict[key].Item2.UnionWith(value.Item2);
            }
            else {
                dict.Add(key, value);
            }
        }
        public static void AddOrCombineTouple<TKey, T1, T2>(this IDictionary<TKey, List<(T1, T2)>> dict, TKey key, (T1, T2) value) {
            if (dict.ContainsKey(key)) {
                dict[key].Add(value);
            }
            else {
                dict.Add(key, new List<(T1, T2)> { value });
            }
        }
		public static void AddOrCombineSetOrKeepHigher<TKey, TKey2>(this SortedDictionary<TKey, Dictionary<TKey2, int>> dict, TKey key, Dictionary<TKey2, int> dict2) {
            if (dict.ContainsKey(key)) {
                foreach(TKey2 key2 in dict2.Keys) {
                    dict[key].SetOrKeepHigher(key2, dict2[key2]);
				}
            }
            else {
                dict.Add(key, dict2);
            }
        }

        public static void AddOrCombineAddCheckOverflow<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, T newValue) {
			if (dictionary.ContainsKey(key)) {
				dictionary[key] = ModMath.AddCheckOverflow(newValue, dictionary[key]);
			}
			else {
				dictionary.Add(key, newValue);
			}
		}
		/// <summary>
		/// Check if the dictionary contains the value.  If it does, replace it, if not add it.
		/// </summary>
		/// <returns>True if added, false if set.</returns>
        public static bool AddOrSet<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, T value) {
			if (dictionary.ContainsKey(key)) {
                dictionary[key] = value;
				return false;
			}
			else {
				dictionary.Add(key, value);
				return true;
			}
		}

		public static void SetValue<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, T value) {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = value;
            }
            else {
                dictionary.Add(key, value);
            }
        }
        public static void SetOrKeepHigher<TKey>(this IDictionary<TKey, int> dictionary, TKey key, int value) {
            if (dictionary.ContainsKey(key)) {
                if (dictionary[key] < value)
                    dictionary[key] = value;
            }
            else {
                dictionary.Add(key, value);
            }
        }
        public static void SetValue<TKey, T>(this Dictionary<TKey, T> dictionary, TKey key, T value) {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = value;
            }
            else {
                dictionary.Add(key, value);
            }
		}
		public static bool ContainsHashSet(this HashSet<HashSet<int>> ingredients, HashSet<int> requiredItemType) {
			bool contains = false;
			foreach (HashSet<int> ingredientType in ingredients) {
				contains = true;
				if (ingredientType.Count != requiredItemType.Count) {
					contains = false;
				}
				else {
					foreach (int eachIngredientType in ingredientType) {
						if (!requiredItemType.Contains(eachIngredientType)) {
							contains = false;
							break;
						}
					}
				}

				if (contains)
					break;
			}

			return contains;
		}
		public static void CombineHashSet(this HashSet<HashSet<int>> set1, HashSet<HashSet<int>> set2) {
            foreach (HashSet<int> set in set2) {
                if (!set1.ContainsHashSet(set))
                    set1.Add(set);
            }
        }
        public static void TryAdd(this HashSet<HashSet<int>> sets, HashSet<int> newSet) {
			if (!sets.ContainsHashSet(newSet))
				sets.Add(newSet);
		}
        public static int RoundNearest(this float f, int mult) {
            float r = f % mult;
            int result = (int)(f - r);
            if (r >= mult / 2f)
                result += mult;

            return result;
        }

		#endregion

		/*
        public static void Combine<T>(this List<T> list, List<T> list2) {
            foreach(T item in list2) {
                list.Add(item);
			}
		}
        */

		//public static void ApplyTo(this StatModifier statModifier, ref float value) {
		//    value = (value + statModifier.Base) * statModifier.Additive * statModifier.Multiplicative + statModifier.Flat;
		//}
		public static bool NullOrAir(this Item item) => item?.IsAir ?? true;
        public static bool NullOrNotActive(this Player player) => !player?.active ?? true;
        public static SortedList<TKey, TValue> CombineSortedLists<TKey, TValue>(this SortedList<TKey, TValue> list1, SortedList<TKey, TValue> list2) {
            SortedList <TKey, TValue> newList = new SortedList <TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> pair in list1) {
                newList.Add(pair.Key, pair.Value);
			}

            foreach (KeyValuePair<TKey, TValue> pair in list2) {
                newList.Add(pair.Key, pair.Value);
            }

            return newList;
        }
        public static void Clamp(this ref int value, int min = 0, int max = 100) {
            value = value < min ? min : value > max ? max : value;
		}
        public static void Clamp(this ref float value, float min = 0f, float max = 1f) {
            value = value < min ? min : value > max ? max : value;
        }
        public static void CombineLists(this List<Item> list1, IEnumerable<Item> list2, bool noDuplicates = false) {
            List<int> list1Types = list1.Select(i => i.type).ToList();
            List<int> list2Types = list2.Select(i => i.type).ToList();
            foreach (Item item2 in list2) {
                if (!noDuplicates || !list1Types.Contains(item2.type))
                    list1.Add(item2);
			}
		}
        public static bool ValidOwner(this Projectile projectile, out Player player) {
            player = null;
            if (projectile.owner >= 0 && projectile.owner < Main.player.Length) {
                player = Main.player[projectile.owner];
                return true;
			}
            
            return false;
        }
        public const int InventoryHotbarCount = 10;
        public const int InventoryStorageCount = 40;
		public const int InventoryCoinCount = 4;
		public const int InventoryAmmoCount = 4;
		public const int InventoryStorageAndHotbarCount = InventoryHotbarCount + InventoryStorageCount;
		public const int InventoryStorageHotbarCoinsAndAmmo = InventoryStorageAndHotbarCount + InventoryCoinCount + InventoryAmmoCount;
        public static Item[] TakePlayerInventory40(this Item[] inv) => inv.Skip(inv.Length >= InventoryHotbarCount ? InventoryHotbarCount : inv.Length)
            .Take(inv.Length >= InventoryStorageAndHotbarCount ? InventoryStorageCount : inv.Length - InventoryHotbarCount).ToArray();
		public static IEnumerable<Item> TakePlayerInventory58(this Item[] inv) => inv.Take(inv.Length >= InventoryStorageHotbarCoinsAndAmmo ? InventoryStorageHotbarCoinsAndAmmo : inv.Length);
		public static int GetRealDefense(this Player player) => (int)Math.Round((player.statDefense.Positive * (1 + player.statDefense.AdditiveBonus.Value) - player.statDefense.Negative) * player.statDefense.FinalMultiplier.Value);
        public static bool InheritsFrom(this ModType modType, Type parent) => modType.GetType().InheritsFrom(parent);
        public static bool InheritsFrom(this Type type, Type parent) {
            if (type.IsAbstract)
                return false;

            if (type == parent)
                return false;

            return type.IsAssignableTo(parent);
        }
        public static Type GetModItemCompairisonType(this Item item) => item?.ModItem != null ? item.ModItem.GetModItemCompairisonType() : null;
        public static Type GetModItemCompairisonType(this ModItem modItem) => modItem is AndroModItem androModItem ? androModItem.GroupingType : modItem.GetType();
		public static bool TryDepositToChest(this int chest, int itemType, ref int stack) {
			if (chest < 0)
				return false;

			Item item = new(itemType, stack);
			Item[] inv = Main.chest[chest].item;

			return inv.Deposit(item, out int _);
		}
		public static bool Deposit(this IList<Item> inv, Item item, out int index) {
			if (inv == null) {
				index = inv.Count;
				return false;
			}

			if (item.NullOrAir()) {
				index = inv.Count;
				return false;
			}

			if (item.favorited) {
				index = inv.Count;
				return false;
			}

			if (Restock(inv, item, out index))
				return true;

			index = 0;
			while (index < inv.Count && !inv[index].IsAir) {
				index++;
			}

			if (index == inv.Count)
				return false;

			inv[index] = item.Clone();
			if (item.stack == item.maxStack)
				inv.DoCoins(index);

			item.TurnToAir();

			return true;
		}
		public static bool Restock(this IList<Item> inv, Item item, out int index) {
			for (int i = 0; i < inv.Count; i++) {
				Item bagItem = inv[i];
				if (!bagItem.NullOrAir() && bagItem.type == item.type && bagItem.stack < bagItem.maxStack) {
					if (ItemLoader.TryStackItems(bagItem, item, out _)) {
						if (item.stack < 1) {
							item.TurnToAir();
							index = i;
							if (bagItem.stack == bagItem.maxStack)
								inv.DoCoins(i);

							return true;
						}
						else {
							inv.DoCoins(i);
						}
					}
				}
			}

			index = inv.Count;
			return false;
		}
		public static void PercentFull(this IEnumerable<Item> inv, out float stackPercentFull, out float slotsPercentFull) {
			slotsPercentFull = 0;
			float total = 0f;
			foreach (Item item in inv.Reverse()) {
				if (item.NullOrAir())
					continue;

				total += item.stack / (float)item.maxStack;
				slotsPercentFull++;
			}

			int count = inv.Count();
			if (count <= 0) {
				slotsPercentFull = 1f;
				stackPercentFull = 1f;
				return;
			}

			slotsPercentFull /= count;
			stackPercentFull = total / count;
		}
		public static void DoCoins(this IList<Item> inv) {
			int[] coins = new int[4];
			for (int i = 0; i < coins.Length; i++) {
				coins[i] = -1;
			}

			for (int i = 0; i < inv.Count; i++) {
				StackCoins(inv, i, coins);
			}
		}
		private static void StackCoins(IList<Item> inv, int slot, int[] coins) {
			Item item = inv[slot];
			if (item.type < ItemID.CopperCoin || item.type > ItemID.PlatinumCoin)
				return;

			if (item.stack >= item.maxStack) {
				DoCoinsUnsafe(inv, slot, coins);
				return;
			}

			ref int coinIndex = ref coins[item.type - ItemID.CopperCoin];
			if (coinIndex > -1) {
				Item coin = inv[coinIndex];
				int transfer = Math.Min(item.maxStack - coin.stack, item.stack);
				coin.stack += transfer;
				item.stack -= transfer;
				if (item.stack < 1) {
					item.TurnToAir(true);
					item.active = false;
					if (coin.stack == item.maxStack) {
						DoCoinsUnsafe(inv, coinIndex, coins);
						coinIndex = -1;
					}
				}
				else {
					DoCoinsUnsafe(inv, coinIndex, coins);
					coinIndex = slot;
				}
			}
			else {
				coinIndex = slot;
			}
		}
		private static void DoCoinsUnsafe(IList<Item> inv, int slot, int[] coins) {
			Item item = inv[slot];
			if (item.type > ItemID.GoldCoin)
				return;

			if (item.stack != 100)
				return;

			item.SetDefaults(item.type + 1);
			StackCoins(inv, slot, coins);
		}
		public static void DoCoins(this IList<Item> inv, int slot) {
			Item item = inv[slot];
			if (item.type < ItemID.CopperCoin || item.type > ItemID.GoldCoin)
				return;

			if (item.stack != 100)
				return;

			item.SetDefaults(item.type + 1);
			for (int i = 0; i < inv.Count; i++) {
				Item coin = inv[i];
				if (item.IsTheSameAs(coin) && i != slot && coin.stack < coin.maxStack) {
					coin.stack++;
					item.TurnToAir(true);
					item.active = false;
					inv.DoCoins(i);

					break;
				}
			}
		}
		public static List<Item> SearchForItem(int itemType, out int itemCount, IEnumerable<Item> otherLocation = null, bool inventory = true, bool banks = true) => 
			SearchForItem(itemType, out itemCount, otherLocation != null ? new List<IEnumerable<Item>>() { otherLocation } : null, inventory, banks);
		public static List<Item> SearchForItem(int itemType, out int itemCount, IEnumerable<IEnumerable<Item>> otherLocations = null, bool inventory = true, bool banks = true) {
			List<Item> items = new();
			itemCount = 0;
			IEnumerable<IEnumerable<Item>> locationsToSearch = new List<IEnumerable<Item>>();
			if (inventory)
				locationsToSearch = locationsToSearch.Append(Main.LocalPlayer.inventory.TakePlayerInventory58());

			if (banks) {
				List<IEnumerable<Item>> bankItems = new() {
					Main.LocalPlayer.bank.item,
					Main.LocalPlayer.bank2.item,
					Main.LocalPlayer.bank3.item,
					Main.LocalPlayer.bank4.item,
				};

				locationsToSearch = locationsToSearch.Concat(bankItems);
			}

			foreach (IEnumerable<Item> locationToSearch in otherLocations != null ? locationsToSearch.Concat(otherLocations) : locationsToSearch) {
				foreach (Item item in locationToSearch) {
					if (item.type == itemType) {
						items.Add(item);
						itemCount += item.stack;
					}
				}
			}

			return items;
		}
		public static bool SearchForFirstItem(int itemType, out Item foundItem, IEnumerable<IEnumerable<Item>> otherLocations = null, bool inventory = true, bool banks = true) {
			foundItem = null;
			IEnumerable<IEnumerable<Item>> locationsToSearch = new List<IEnumerable<Item>>();
			if (inventory)
				locationsToSearch = locationsToSearch.Append(Main.LocalPlayer.inventory);

			if (banks) {
				List<IEnumerable<Item>> bankItems = new() {
					Main.LocalPlayer.bank.item,
					Main.LocalPlayer.bank2.item,
					Main.LocalPlayer.bank3.item,
					Main.LocalPlayer.bank4.item,
				};

				locationsToSearch = locationsToSearch.Concat(bankItems);
			}

			foreach (IEnumerable<Item> locationToSearch in otherLocations != null ? locationsToSearch.Concat(otherLocations) : locationsToSearch) {
				foreach (Item item in locationToSearch) {
					if (item.type == itemType) {
						foundItem = item;
						return true;
					}
				}
			}

			return false;
		}
		public static void DrawItem(this Item item, SpriteBatch spriteBatch, Vector2 position, float inventoryScale = 1f, bool alwaysDrawStack = true) {
			Color color = Color.White;
			ItemSlot.DrawItemIcon(item, 31, spriteBatch, position, inventoryScale, 20f, color);
			if (item.stack > 0 || alwaysDrawStack)
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), position + new Vector2(-item.width + 3.5f, 0f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
		}
		private static bool IsTheSameAs(this Item item, Item compareItem) {
			if (item.netID == compareItem.netID)
				return item.type == compareItem.type;

			return false;
		}

		public const char DisplayedHeartbeatString = '|';
		public static Vector2 MeasureString(this string s) {
            if (s.Length > 0 && s[s.Length - 1] == DisplayedHeartbeatString)
                s = s.Substring(0, s.Length - 1);

            Vector2 size = FontAssets.MouseText.Value.MeasureString(s);
			if (size.Y <= 0) {
				Vector2 defaultSizeForHeight = FontAssets.MouseText.Value.MeasureString("Z");
				float height = defaultSizeForHeight.Y;
				size = new Vector2(size.X, height);
			}

			return size;
		}
		public static void PlaceTile(int tileTargetX, int tileTargetY, Item sItem) {
			int tileToCreate = sItem.createTile;
			int num = sItem.placeStyle;
			PlantLoader.CheckAndInjectModSapling(tileTargetX, tileTargetY, ref tileToCreate, ref num);
			if (TileObjectData.CustomPlace(tileToCreate, num) && tileToCreate != 82 && tileToCreate != 227) {
				int direction = 1;
				bool canPlace = TileObject.CanPlace(tileTargetX, tileTargetY, (ushort)tileToCreate, num, direction, out TileObject data, onlyCheck: false);
				bool flag = TileObject.Place(data);
				WorldGen.SquareTileFrame(tileTargetX, tileTargetY);
				if (Main.netMode != NetmodeID.Server || !TileID.Sets.IsAContainer[tileToCreate])
					SoundEngine.PlaySound(SoundID.Dig, new Vector2(tileTargetX * 16, tileTargetY * 16));

				TileObjectData.CallPostPlacementPlayerHook(tileTargetX, tileTargetY, tileToCreate, num, direction, data.alternate, data);
				if (Main.netMode == NetmodeID.Server && !Main.tileContainer[tileToCreate] && tileToCreate != 423)
					NetMessage.SendObjectPlacement(-1, tileTargetX, tileTargetY, data.type, data.style, data.alternate, data.random, direction);

				TileLoader.PlaceInWorld(tileTargetX, tileTargetY, sItem);
			}
			else {
				PlaceTile(tileTargetX, tileTargetY, tileToCreate, sItem.placeStyle);
			}
		}
		public static void PlaceTile(int i, int j, int tileToPlace, int style = 0) {
			if (!WorldGen.PlaceTile(i, j, tileToPlace, true, true, style: style)) {
				Tile tile = Main.tile[i, j];
				switch (tileToPlace) {
					case TileID.Grass:
					case TileID.CorruptGrass:
					case TileID.CrimsonGrass:
					case TileID.HallowedGrass:
						tile.HasTile = true;
						tile.TileType = TileID.Dirt;
						break;
					case TileID.JungleGrass:
					case TileID.CorruptJungleGrass:
					case TileID.CrimsonJungleGrass:
					case TileID.MushroomGrass:
						tile.HasTile = true;
						tile.TileType = TileID.Mud;
						break;
					case TileID.AshGrass:
						tile.HasTile = true;
						tile.TileType = TileID.Ash;
						break;
				}

				WorldGen.PlaceTile(i, j, tileToPlace, true, true, style: style);
			}

			WorldGen.SquareTileFrame(i, j);
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendTileSquare(-1, i - 1, j - 1, 3);
		}
		public static Point16 TilePositionToTileTopLeft(int x, int y) {
			Tile tile = Main.tile[x, y];
			int tileType = tile.TileType;
			TileObjectData tileData = TileObjectData.GetTileData(tileType, 0);
			if (tileData == null)
				return new(x, y);

			int tileX = (tile.TileFrameX / 18) % tileData.Width;
			int tileY = (tile.TileFrameY / 18) % tileData.Height;
			return new(x - tileX, y - tileY);
		}
		/// <summary>
		/// Only use this when using the coordinates from placing tiles.
		/// </summary>
		public static Point16 TileOriginToMultiTileTopLeft(int x, int y, int tileType = -1) {
			if (tileType == -1)
				tileType = Main.tile[x, y].TileType;

			Point16 baseCoords = new Point16(x, y);
			TileObjectData.OriginToTopLeft(tileType, 0, ref baseCoords);
			return baseCoords;
		}
		public static IEnumerable<Point16> MultiTileTiles(int x, int y) => MultiTileTilesTopLeft(new Point16(x, y));
		public static IEnumerable<Point16> MultiTileTilesTopLeft(Point16 point16) {
			Point16 topLeft = TilePositionToTileTopLeft(point16.X, point16.Y);
			int width;
			int height;
			Tile tile = Main.tile[topLeft.X, topLeft.Y];
			TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0);
			if (tileData == null) {
				width = 1;
				height = 1;
			}
			else {
				width = tileData.Width;
				height = tileData.Height;
			}

			int xEnd = topLeft.X + width;
			int yEnd = topLeft.Y + height;
			for (int x = topLeft.X; x < xEnd; x++) {
				for (int y = topLeft.Y; y < yEnd; y++) {
					Tile checkTile = Main.tile[x, y];
					if (checkTile.TileType != tile.TileType)
						continue;

					yield return new Point16(x, y);
				}
			}
		}
		public static bool TryGetChest(this Point16 topLeft, out int chest) => TryGetChest(topLeft.X, topLeft.Y, out chest);
		public static bool TryGetChest(int x, int y, out int chestId) {
			chestId = Chest.FindChest(x, y);
			if (chestId > -1)
				return true;

			return false;
		}
		public static bool AnyWire(this Tile tile) => tile.BlueWire || tile.YellowWire || tile.RedWire || tile.GreenWire;
		public static bool AnyTileWireTopLeft(this Point16 topLeft) => AnyTileWireTopLeft(topLeft.X, topLeft.Y);
		public static bool AnyTileWireTopLeft(int topLeftX, int topLeftY) {
			int width;
			int height;
			Tile tile = Main.tile[topLeftX, topLeftY];
			TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0);
			if (tileData == null) {
				width = 1;
				height = 1;
			}
			else {
				width = tileData.Width;
				height = tileData.Height;
			}

			int xEnd = topLeftX + width;
			int yEnd = topLeftY + height;
			for (int x = topLeftX; x < xEnd; x++) {
				for (int y = topLeftY; y < yEnd; y++) {
					Tile checkTile = Main.tile[x, y];
					if (checkTile.TileType != tile.TileType)
						continue;

					if (checkTile.AnyWire())
						return true;
				}
			}

			return false;
		}
		public static bool AnyTileWire(this Point16 topLeft) => AnyTileWire(topLeft.X, topLeft.Y);
		public static bool AnyTileWire(int x, int y) => TilePositionToTileTopLeft(x, y).AnyTileWireTopLeft();
		public static int PositionValue(this Point16 p) => PositionValue(p.X, p.Y);
		public static int PositionValue(int x, int y) => x + y * Main.maxTilesX;
		public static Point16 ValueToPosition(this int v) {
			int y = v / Main.maxTilesX;
			int x = v % Main.maxTilesX;
			return new Point16(x, y);
		}
		public static bool DepositVisualizeChestTransfer(this Chest chest, Item item) {
			Item[] inv = chest?.item;
			if (inv == null)
				return false;

			int stack = item.stack;
			bool transferedAll = chest.item.Deposit(item, out int index);
			int transferred = stack - item.stack;
			if (transferred == 0)
				return false;

			Chest.VisualizeChestTransfer(item.position, new Vector2((chest.x + 1) * 16, (chest.y + 1) * 16), inv[index], transferred);

			return transferedAll;
		}
		public static bool Snowing => Main.SceneMetrics.EnoughTilesForSnow;
		private static Asset<Texture2D> testingDotTexture = ModContent.Request<Texture2D>($"{AndroMod.ModName}/UI/Sprites/TestingDot");
		public static void DrawTestingDot(int tileX, int tileY, Color color, SpriteBatch spriteBatch) => DrawTestingDot(new Point16(tileX, tileY), color, spriteBatch);
		public static void DrawTestingDot(this Point16 tilePosition, Color color, SpriteBatch spriteBatch) => DrawTestingDot(tilePosition.ToWorldCoordinates(), color, spriteBatch);
		public static void DrawTestingDot(float worldX, float worldY, Color color, SpriteBatch spriteBatch) => DrawTestingDot(new Vector2(worldX, worldY), color, spriteBatch);
		public static void DrawTestingDot(this Vector2 worldPosition, Color color, SpriteBatch spriteBatch) {
			Vector2 zero = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) + new Vector2(-3f, -3f);
			Vector2 stackIndicatorPosition = worldPosition;
			spriteBatch.Draw(testingDotTexture.Value, stackIndicatorPosition - Main.screenPosition + zero, color);
		}

		#endregion
	}
}
