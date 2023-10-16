using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using androLib.Common.Utility.LogSystem;
using androLib.Common.Utility.LogSystem.WebpageComponenets;
using static Terraria.Localization.GameCulture;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using androLib.Common.Configs;
using System.Collections;
using androLib.Items;
using androLib.Common.Utility;
using androLib.Common.Globals;
using androLib.Content.NPCs;

namespace androLib.Common.Utility.LogSystem
{
    public abstract class Wiki {
        public static Dictionary<int, List<RecipeData>> createItemRecipes;
        public static Dictionary<int, List<RecipeData>> recipesUsedIn;
        public static Dictionary<int, Dictionary<int, DropRateInfo>> enemyDrops;
        public static Dictionary<int, Dictionary<ChestID, float>> chestDrops;
        public static Dictionary<int, Dictionary<CrateID, float>> crateDrops;
        protected static int min;
        protected static int max;
        public static DirectoryInfo logsDirectory = new(Folder.LogsFolder);

		public static string nowString = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");
        public static Folder wikiFolder = null;
        public static string changesSumary = null;
        private static string lastWikiDirectory = "";
		public static string LastWikiDirectory {
            get {
                if (lastWikiDirectory == "") {
					string weWiki = $"{ModNameStatic}Wiki";
					string mostRecent = null;
					int[] mostRecentData = null;
					IEnumerable<string> directoryNames = logsDirectory.GetDirectories().Where(d => d.FullName != wikiPath).Select(d => d.Name);
					foreach (string directoryName in directoryNames) {
						if (directoryName.StartsWith(weWiki)) {
							if (mostRecent == null) {
								mostRecent = directoryName;
								string dataBeforeParse = directoryName.Substring(weWiki.Length + 1, directoryName.Length - 4 - weWiki.Length)
									.Replace(" ", "_");
								mostRecentData = dataBeforeParse.Split("_").Select(s => int.Parse(s)).ToArray();
                                if (directoryName.EndsWith("PM"))
                                    mostRecentData[3] += 12;
							}
							else {
								string dataBeforeParse = directoryName.Substring(weWiki.Length + 1, directoryName.Length - 4 - weWiki.Length)
									.Replace(" ", "_");
								int[] newData = dataBeforeParse.Split("_").Select(s => int.Parse(s)).ToArray();
								if (directoryName.EndsWith("PM"))
									newData[3] += 12;

								for (int i = 0; i < mostRecentData.Length; i++) {
                                    if (newData[i] == mostRecentData[i]) {
                                        continue;
                                    }
                                    else if (newData[i] > mostRecentData[i]) {
										mostRecent = directoryName;
										mostRecentData = newData;
                                    }
                                    else {
                                        break;
                                    }
                                }
							}
						}
					}

					lastWikiDirectory = mostRecent;
				}

                return lastWikiDirectory;
            }        
        }
        public abstract bool ShouldPrintWiki { get; }
        public abstract Func<Mod> GetMod { get; }
		public static string wikiPath => $"{logsDirectory.FullName}\\{ModNameStatic}Wiki_{nowString}";
        public abstract string ModName { get; }
        public static string ModNameStatic { get; private set; }
		public void PrintWiki() {
            if (!ShouldPrintWiki)
                return;

            ModNameStatic = ModName;

			if (Debugger.IsAttached) {
                changesSumary = "";
				Directory.CreateDirectory(wikiPath);

				if (LastWikiDirectory != null) {
					string lastWikiDirectoryPath = $"{logsDirectory.FullName}\\{LastWikiDirectory}";
					wikiFolder = new(lastWikiDirectoryPath);
				}
			}

			List<WebPage> webPages = new();

			IEnumerable<ModItem> modItems = GetMod().GetContent<ModItem>();
			GetMinMax(modItems, out min, out max);
			GetRecpies();
			GetDrops();

			AddWikiPages(webPages, modItems);

            string wiki = "\n\n";

			foreach (WebPage webPage in webPages) {
				if (Debugger.IsAttached) {
                    webPage.WriteAllTextToFile();
				}
				else {
					wiki += $"Page: {webPage.HeaderName}\n{webPage}{"\n".FillString(5)}";
				}
			}

            if (Debugger.IsAttached) {
				File.WriteAllText($"{wikiPath}\\Change Sumary.txt", changesSumary);
			}
            else {
				wiki.LogSimple();
			}
        }
        protected abstract void AddWikiPages(List<WebPage> webPages, IEnumerable<ModItem> modItems);
        public static void GetMinMax(IEnumerable<ModItem> modItems, out int min, out int max) {
            min = int.MaxValue;
            max = int.MinValue;
            foreach (ModItem modItem in modItems) {
                int type = modItem.Type;
                if (type < min) {
                    min = type;
                }
                else if (type > max) {
                    max = type;
                }
            }
        }
        private static void GetRecpies() {
            createItemRecipes = new();
            recipesUsedIn = new();

            for (int i = 0; i < Recipe.numRecipes; i++) {
                Recipe recipe = Main.recipe[i];
                int type = recipe.createItem.type;
                if (type >= min && type <= max) {
                    RecipeData recipeData = RecipeData.Create(recipe);
                    if (createItemRecipes.ContainsKey(type)) {
                        bool recipeAdded = false;

                        foreach (RecipeData createItemRecipe in createItemRecipes[type]) {
                            if (createItemRecipe.TryAdd(recipeData)) {
                                recipeAdded = true;
                                break;
                            }
                        }

                        if (!recipeAdded)
                            createItemRecipes[type].Add(recipeData);
                    }
                    else {
                        createItemRecipes.Add(type, new List<RecipeData> { recipeData });
                    }
                }

                foreach (Item item in recipe.requiredItem) {
                    int requiredItemType = item.type;
                    if (requiredItemType >= min && requiredItemType <= max) {
						RecipeData recipeData = RecipeData.Create(recipe);
						if (recipesUsedIn.ContainsKey(requiredItemType)) {
                            bool recipeAdded = false;
                            foreach (RecipeData usedInRecipe in recipesUsedIn[requiredItemType]) {
                                if (usedInRecipe.TryAdd(recipeData)) {
                                    recipeAdded = true;
                                    break;
                                }
                            }

                            if (!recipeAdded)
                                recipesUsedIn[requiredItemType].Add(recipeData);
                        }
                        else {
                            recipesUsedIn.Add(requiredItemType, new List<RecipeData> { recipeData });
                        }
                    }
                }
            }

            chestDrops = new();
            foreach (ChestID chestID in Enum.GetValues(typeof(ChestID)).Cast<ChestID>().ToList().Where(c => c != ChestID.None)) {
                AndroModSystem.GetChestLoot(chestID, out List<DropData> dropData, out float baseChance);
                if (dropData == null)
                    continue;

                string name = chestID.ToString() + " Chest";
                float total = 0f;
                IEnumerable<DropData> weightedDrops = dropData.Where(d => d.Chance <= 0f);
				IEnumerable<DropData> chanceDrops = dropData.Where(d => d.Chance > 0f);

                foreach (DropData data in chanceDrops) { 
                    float randFloat = Main.rand.NextFloat();
                    float chance = data.Chance;
                    if (AndroModSystem.TryGetChestSpawnChanceMultiplier(data.ID, out float mult))
                        chance *= mult;

					if (chestDrops.ContainsKey(data.ID)) {
						chestDrops[data.ID].Add(chestID, chance);
					}
					else {
						chestDrops.Add(data.ID, new() { { chestID, chance } });
					}
				}

				foreach (DropData data in weightedDrops) {
                    total += data.Weight;
                }

                foreach (DropData data in weightedDrops) {
                    Item sampleItem = ContentSamples.ItemsByType[data.ID];
                    int type = sampleItem.type;
                    if (!(type >= min && type <= max))
                        continue;

                    if (chestDrops.ContainsKey(type)) {
                        chestDrops[type].Add(chestID, baseChance * data.Weight / total);
					}
					else {
                        chestDrops.Add(type, new() { { chestID, baseChance * data.Weight / total } });
                    }
                }
            }

            crateDrops = new();
            foreach (KeyValuePair<int, List<DropData>> crate in GlobalCrates.crateDrops) {
                string name = ((CrateID)crate.Key).ToString() + " Crate";
                float total = 0f;

				IEnumerable<DropData> weightedDrops = crate.Value.Where(d => d.Chance <= 0f);
				IEnumerable<DropData> chanceDrops = crate.Value.Where(d => d.Chance > 0f);

				foreach (DropData data in chanceDrops) {
					float randFloat = Main.rand.NextFloat();
					float chance = data.Chance;
                    if (AndroModSystem.TryGetCrateSpawnChanceMultiplier(data.ID, out float mult))
                        chance *= mult;

					if (crateDrops.ContainsKey(data.ID)) {
                        Item tempItem = data.ID.CSI();
                        CrateID crateID = (CrateID)crate.Key;
						bool alreadyHas = crateDrops[data.ID].ContainsKey((CrateID)crate.Key);
						crateDrops[data.ID].Add((CrateID)crate.Key, chance);
					}
					else {
						crateDrops.Add(data.ID, new() { { (CrateID)crate.Key, chance } });
					}
				}

				foreach (DropData data in crate.Value) {
                    if (data.Chance < 0f)
                        total += data.Weight;
                }

                foreach (DropData data in crate.Value) {
                    Item sampleItem = ContentSamples.ItemsByType[data.ID];
                    int type = sampleItem.type;
                    if (!(type >= min && type <= max))
                        continue;

                    float baseChance = GlobalCrates.GetCrateEnchantmentDropChance(crate.Key);
                    if (crateDrops.ContainsKey(type)) {
                        if (crateDrops[type].ContainsKey((CrateID)crate.Key)) {
                            $"New: item: {sampleItem.S()}, CrateID: {(CrateID)crate.Key}, chance: {baseChance * data.Weight / total}.  Old chance: {crateDrops[type][(CrateID)crate.Key]}".LogSimple();
                            continue;
                            /*
[23:42:04.661] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Attack Speed Enchantment Basic, CrateID: Iron, chance: 0.008305647.  Old chance: 0.008305647
[23:42:04.663] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Critical Strike Chance Enchantment Basic, CrateID: Iron, chance: 0.008305647.  Old chance: 0.008305647
[23:42:04.664] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Ammo Cost Enchantment Basic, CrateID: Iron, chance: 0.008305647.  Old chance: 0.008305647
[23:42:04.666] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Crate Enchantment Basic, CrateID: Iron, chance: 0.016611295.  Old chance: 0.016611295
[23:42:04.668] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Danger Sense Enchantment Basic, CrateID: Iron, chance: 0.008305647.  Old chance: 0.008305647
[23:42:04.670] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Fishing Enchantment Basic, CrateID: Iron, chance: 0.016611295.  Old chance: 0.016611295
[23:42:04.671] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Hunter Enchantment Basic, CrateID: Iron, chance: 0.008305647.  Old chance: 0.008305647
[23:42:04.672] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Obsidian Skin Enchantment Basic, CrateID: Iron, chance: 0.008305647.  Old chance: 0.008305647
[23:42:04.673] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Sonar Enchantment Basic, CrateID: Iron, chance: 0.016611295.  Old chance: 0.016611295
[23:42:04.675] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Spelunker Enchantment Basic, CrateID: Iron, chance: 0.008305647.  Old chance: 0.008305647
[23:42:04.677] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Critical Strike Chance Enchantment Basic, CrateID: Jungle, chance: 0.040983606.  Old chance: 0.040983606
[23:42:04.679] [.NET ThreadPool Worker/INFO] [WeaponEnchantments]: New: item: Luck Enchantment Basic, CrateID: Seaside_OceanHard, chance: 0.008333334.  Old chance: 0.008333334
                            */
                        }

                        crateDrops[type].Add((CrateID)crate.Key, baseChance * data.Weight / total);
                    }
                    else {
                        crateDrops.Add(type, new() { { (CrateID)crate.Key, baseChance * data.Weight / total } });
                    }
                }
            }
        }
        private static void GetDrops() {
            enemyDrops = new();
            for (int npcNetID = NPCID.NegativeIDCount + 1; npcNetID < NPCLoader.NPCCount; npcNetID++) {
                foreach (IItemDropRule dropRule in Main.ItemDropsDB.GetRulesForNPCID(npcNetID)) {
                    List<DropRateInfo> dropRates = new();
                    DropRateInfoChainFeed dropRateInfoChainFeed = new(1f);
                    dropRule.ReportDroprates(dropRates, dropRateInfoChainFeed);
                    foreach (DropRateInfo dropRate in dropRates) {
                        int itemType = dropRate.itemId;
                        if (itemType >= min && itemType <= max) {
                            if (enemyDrops.ContainsKey(itemType)) {
                                if (enemyDrops[itemType].ContainsKey(npcNetID)) {
                                    $"itemType: {new Item(itemType).S()}, npcType{ContentSamples.NpcsByNetId[npcNetID]}".LogSimple();
                                }
                                else {
                                    enemyDrops[itemType].Add(npcNetID, dropRate);
                                }
                            }
                            else {
                                enemyDrops.Add(itemType, new() { { npcNetID, dropRate } });
                            }
                        }
                    }
                }
            }
        }
		protected static void AddItemTable(IEnumerable<ModItem> modItems, string label, WebPage webPage, int columns = 4) {
			List<List<string>> tableData = new();
			int column = 0;
			List<string> row = new();
			foreach (ModItem modItem in modItems) {
				row.Add(modItem.Item.ToItemPNG(link: true));
				column++;
				if (column >= columns) {
					tableData.Add(row);
					row = new();
					column = 0;
				}
			}

            if (row.Count > 0)
                tableData.Add(row);

			webPage.AddSubHeading(label);
			webPage.AddTable(tableData, label: label);
			webPage.NewLine();
		}
	}

    public static class WikiStaticMethods
    {
        private static Dictionary<int, int> itemsFromTiles;
        public static string ToLink(this string s, string text = null) => $"[[{s}{(text != null ? $"|{text}" : "")}]]";
        public static string ToSectionLink(this string s, string text = null, string page = null) => $"{(page != null ? page : "")}#{s}".ToLink(text ?? s);
        public static string ToExternalLink(this string s, string text = null) => $"[{s}{(text != null ? $" {text}" : "")}]";
        public static string ToVanillaWikiLink(this string s, string text = null) => $"https://terraria.fandom.com/wiki/{s}".ToExternalLink(text != null ? text : s.Replace('_', ' '));
        public static string ToVanillaWikiLink(this InvasionID id, string text = null) => $"{id}".ToVanillaWikiLink(text);
        public static string ToPNG(this string s, bool removeSpaces = true) => $"[[File:{(removeSpaces ? s.RemoveSpaces() : s)}.png]]";
        public static string ToPNGLink(this string s) => s.ToPNG() + s.ToLink();
        public static string ToLabledPNG(this string s) => s.ToPNG() + s;
        public static string ToItemPNG(this Item item, bool link = false, bool displayName = true, bool displayNum = false, string linkText = null) {
            string name;
            string file = "";
            string linkString = "";
            if (item.type < ItemID.Count) {
				//manually changing the item
				switch (item.type) {
					case ItemID.Fake_GoldChest:
						item = new Item(ItemID.GoldChest);
						name = linkText ?? "Locked Gold Chest";
						break;
                    case ItemID.Fake_ShadowChest:
                        item = new Item(ItemID.ShadowChest);
                        name = linkText ?? "Locked Shadow Chest";
                        break;
					default:
						name = linkText ?? item.Name;
						break;
				}

                file = item.type.GetItemPNGLink();
				if (link)
					linkString = $"https://terraria.fandom.com/wiki/{item.Name.Replace(" ", "_")}".ToExternalLink(name);
			}
            else {
                ModItem modItem = item.ModItem;
                bool androModItem = modItem is AndroModItem;
                if (androModItem) {
					file = modItem.Name.ToPNG();
					name = linkText ?? modItem.Name.AddSpaces();
				}
                else {
					//file = item.ModFullName().GetModItemPNGLink();
					name = linkText ?? modItem.Name.AddSpaces();
				}

                if (link) {
					if (modItem is ISoldByNPC soldByNPC && soldByNPC.GetNonStandardWikiLinkString != null) {
                        linkString = soldByNPC.GetNonStandardWikiLinkString(item.ModItem, name);
					}
					else {
                        linkString = androModItem ? name.ToLink() : item.ModFullName().GetModItemLink().ToExternalLink(name);
                    }
                }
            }

			int stack = item.stack;
            return $"{(!displayName && displayNum ? $"<span>{stack}</span>" : "")}{file}{(link ? " " + linkString : displayName ? " " + name : "")}{(displayName && stack > 1 ? $" ({stack})" : "")}";
        }
        public static string ToItemPNG(this int type, int num = 1, bool link = false, bool displayName = true, bool dislpayNum = false, string label = null) {
            return new Item(type, num).ToItemPNG(link, displayName, dislpayNum, label);
        }
        public static string ToItemPNG(this short type, int num = 1, bool link = false, bool displayName = true, bool dislpayNum = false, string label = null) =>
            ToItemPNG((int)type, num, link, displayName, dislpayNum, label);
        public static string ToItemPNG(this string s, bool link = false, bool displayName = true, string linkText = null) {
            return $"{s.ToPNG()} {(link ? s.ToLink(linkText) : displayName ? s : "")}";
        }
        public static string ToItemPNGs(this string recipeGroupKey, bool link = false, bool displayName = true) =>
            RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs[recipeGroupKey]].ValidItems.Select(i => i.ToItemPNG(link: link, displayName: displayName)).JoinList(", ");
        public static string ToNpcPNG(this int npcNetID, bool link = false, bool displayName = true, bool displayPNG = true) {
            string name;
            string file = "";
            string pngLinkString = "";
            NPC npc = ContentSamples.NpcsByNetId[npcNetID];
            if (npcNetID < NPCID.Count) {
                if (displayPNG) {
                    file = npcNetID.GetNPCPNGLink();
                    if (file == "")
                        file = $"NPC_{npc.netID}".ToPNG();
                }

                name = npc.netID < 0 ? NPCID.Search.GetName(npc.netID).AddSpaces(true) : npc.FullName();
                if (link)
                    pngLinkString = $"https://terraria.fandom.com/wiki/{npc.FullName().Replace(" ", "_")}".ToExternalLink(name);
            }
            else {
                ModNPC modNPC = npc.ModNPC;
                string modNpcFullName = modNPC.FullName;
				name = npc.FullName();

                bool androModNpc = modNPC is INPCWikiInfo;

                if (displayPNG && androModNpc)
                    file = name.ToPNG();

                if (link)
                    pngLinkString = androModNpc ? name.ToLink() : modNpcFullName.GetModNpcLink().ToExternalLink(name);
            }

            return $"{file}{(file != null && link ? " " : "")}{(link ? pngLinkString : displayName ? " " + name : "")}";
		}
        public static string ToNpcPNG(this short npcNetID, bool link = false, bool displayName = true, bool displayPNG = true) =>
            ((int)npcNetID).ToNpcPNG(link, displayName, displayPNG);
        public static string GetCoinsPNG(this int sellPrice) {
            int coinType = ItemID.PlatinumCoin;
            int coinValue = 1000000;
            string text = "";
            bool first = true;
            while (sellPrice > 0) {
                int numCoinsToSpawn = sellPrice / coinValue;
                if (numCoinsToSpawn > 0) {
                    if (first) {
                        first = false;
					}
                    else {
                        text += " ";
					}

                    text += coinType.ToItemPNG(numCoinsToSpawn, displayName: false, dislpayNum: true);
                }

                sellPrice %= coinValue;
                coinType--;
                coinValue /= 100;
            }

            return text;
        }
        public static string ToItemFromTilePNG(this int tileNum) {
            if (tileNum <= 0) {
                return "NoTileRequired.png";
            }

            int itemType = GetItemTypeFromTileType(tileNum);
            if (itemType <= 0) {
                $"Failed to find an item for tileNum: {tileNum}".LogSimple();
                return "NoTileRequired.png";
            }

            Item item = new(itemType);

            return item.ToItemPNG();
        }
        public static int GetItemTypeFromTileType(this int tileType) {
            if (tileType <= 0)
                return -1;

            if (itemsFromTiles == null) {
                itemsFromTiles = new();
            }
            else {
                if (itemsFromTiles.ContainsKey(tileType))
                    return itemsFromTiles[tileType];
            }

            for (int i = 0; i < ItemLoader.ItemCount; i++) {
                Item sampleItem = ContentSamples.ItemsByType[i];
                if (sampleItem == null)
                    continue;

                if (sampleItem.createTile == tileType) {
                    itemsFromTiles.Add(tileType, i);
                    return i;
                }
            }

            return -1;
        }
        public static Item GetItemFromTileType(this int tileType) {
            return new(GetItemTypeFromTileType(tileType));
        }
        public static string ToNPCSellText(this SellCondition condition, Func<int> npcNetID, string value) {
            string npcString = npcNetID().ToNpcPNG(link: true, displayPNG: false);
            switch (condition) {
                case SellCondition.Never:
                    return $"can never appear in the {npcString}'s shop.";
                case SellCondition.Always:
                    return $"will always appear in the {npcString}'s shop.";
                case SellCondition.IgnoreCondition:
                case SellCondition.AnyTime:
                    return $"can appear in the {npcString}'s shop any time for {value}.";
                case SellCondition.AnyTimeRare:
                    return $"can appear in the {npcString}'s shop any time for {value}, but is rare.";
                case SellCondition.PostKingSlime:
                    return $"can appear in the {npcString}'s shop after the {NPCID.KingSlime.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostEyeOfCthulhu:
                    return $"can appear in the {npcString}'s shop after the {NPCID.EyeofCthulhu.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostEaterOfWorldsOrBrainOfCthulhu:
                    return $"can appear in the {npcString}'s shop after the {NPCID.EaterofWorldsHead.ToNpcPNG(link: true, displayPNG: false)} or {NPCID.BrainofCthulhu.ToNpcPNG(link: true, displayPNG: false)} have been defeated for {value}.";
                case SellCondition.PostSkeletron:
                    return $"can appear in the {npcString}'s shop after {NPCID.Skeleton.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostQueenBee:
                    return $"can appear in the {npcString}'s shop after the {NPCID.QueenBee.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostDeerclops:
                    return $"can appear in the {npcString}'s shop after {NPCID.Deerclops.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostGoblinInvasion:
                    return $"can appear in the {npcString}'s shop after the {InvasionID.Goblin_Army.ToVanillaWikiLink()} has been defeated for {value}.";
                case SellCondition.Luck:
                    return $"can appear in the {npcString}'s shop any time for {value}, but is extremely rare.";
                case SellCondition.HardMode:
                    return $"can appear in the {npcString}'s shop during hard mode for {value}.";
                case SellCondition.PostQueenSlime:
                    return $"can appear in the {npcString}'s shop after the {NPCID.QueenSlimeBoss.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostPirateInvasion:
                    return $"can appear in the {npcString}'s shop after a {InvasionID.Pirate_Invasion.ToVanillaWikiLink()} has been defeated for {value}.";
                case SellCondition.PostTwins:
                    return $"can appear in the {npcString}'s shop after {"The_Twins".ToVanillaWikiLink()} have been defeated for {value}.";
                case SellCondition.PostDestroyer:
                    return $"can appear in the {npcString}'s shop after {NPCID.TheDestroyer.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostSkeletronPrime:
                    return $"can appear in the {npcString}'s shop after {NPCID.SkeletronPrime.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostPlantera:
                    return $"can appear in the {npcString}'s shop after {NPCID.Plantera.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostGolem:
                    return $"can appear in the {npcString}'s shop after the {NPCID.Golem.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostMartianInvasion:
                    return $"can appear in the {npcString}'s shop after {InvasionID.Martian_Madness} has been defeated for {value}.";
                case SellCondition.PostDukeFishron:
                    return $"can appear in the {npcString}'s shop after {NPCID.DukeFishron.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostEmpressOfLight:
                    return $"can appear in the {npcString}'s shop after the {NPCID.HallowBoss.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostCultist:
                    return $"can appear in the {npcString}'s shop after the {NPCID.CultistBoss.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostSolarTower:
                    return $"can appear in the {npcString}'s shop after the {NPCID.LunarTowerSolar.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostNebulaTower:
                    return $"can appear in the {npcString}'s shop after the {NPCID.LunarTowerNebula.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostStardustTower:
                    return $"can appear in the {npcString}'s shop after the {NPCID.LunarTowerStardust.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostVortexTower:
                    return $"can appear in the {npcString}'s shop after the {NPCID.LunarTowerVortex.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                case SellCondition.PostMoonLord:
                    return $"can appear in the {npcString}'s shop after the {NPCID.MoonLordHead.ToNpcPNG(link: true, displayPNG: false)} has been defeated for {value}.";
                default:
                    return "SellConditionTextNotFound";
            }
        }
    }
}
