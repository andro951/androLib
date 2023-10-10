﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using androLib.Common.Utility;
using Terraria.ModLoader;

namespace androLib.Common.Utility.LogSystem
{
	public class RecipeData
    {
        private static SortedDictionary<string, Func<Recipe, RecipeData>> constructors = new();
        public static void RegisterConstructor(Mod mod, Func<Recipe, RecipeData> constructor) {
            constructors.Add(mod.Name, constructor);
        }
        public static RecipeData Create(Recipe recipe) {
			if (recipe.createItem.ModItem?.Mod?.Name is string modName && modName != null && constructors.TryGetValue(modName, out Func<Recipe, RecipeData> constructor))
				return constructor(recipe);

			return new RecipeData(recipe);
		}
        public CommonItemList createItem;
        public CommonItemList requiredItem;
        public CommonItemList requiredTile;
        public readonly IEnumerable<int> ReverseRecipeItemTypes;
        public RecipeData(Recipe recipe) {
            List<Item> tempCreateItem = new() { recipe.createItem.Clone() };
            List<Item> tempRequiredItem = recipe.requiredItem.Select(i => i.Clone()).ToList();
            List<Item> reverseRecipe = GetAllOtherCraftedItems(tempCreateItem[0], tempRequiredItem);
            ReverseRecipeItemTypes = reverseRecipe.Select(i => i.type);
            tempCreateItem.CombineLists(reverseRecipe);
            createItem = new(tempCreateItem);
            requiredItem = new(tempRequiredItem);
            requiredTile = new(recipe.requiredTile.Select(i => i.GetItemFromTileType()).ToList());
            foreach (int acceptedGroupID in recipe.acceptedGroups) {
				requiredItem.AddUniqueListFromACommonItem(acceptedGroupID);
			}
        }
        private List<Item> GetAllOtherCraftedItems(Item item, List<Item> consumedItems) => consumedItems.SelectMany(c => GetOtherCraftedItems(item, c)).ToList();
        protected virtual List<Item> GetOtherCraftedItems(Item item, Item consumedItem) => new();
        public virtual bool TryAdd(RecipeData other) {
            if (!createItem.SameCommonItems(other.createItem))
                return false;

            bool added = false;
            if (SameExceptOneTile(other)) {
                if (requiredTile.Add(other.requiredTile))
                    added = true;
            }

            if (SameExceptOneRequiredItem(other)) {
                if (requiredItem.Add(other.requiredItem))
                    added = true;
            }

            if (added)
                createItem.Add(other.createItem);

            return added;
        }
        public bool SameExceptOneTile(RecipeData other) {
            if (!requiredItem.ExactSame(other.requiredItem))
                return false;

            if (!requiredTile.SameCommonItems(other.requiredTile))
                return false;

            return true;
        }
        public bool SameExeptOneTileCompareTypesIgnoreStack(RecipeData other) {
            if (!requiredItem.ExactSame(other.requiredItem))
                return false;

            if (!requiredTile.SameCommonItemsCompareTypesIgnoreStack(other.requiredTile))
                return false;

            return true;
        }
        public bool SameExceptOneRequiredItem(RecipeData other) {
            if (!requiredTile.ExactSame(other.requiredTile))
                return false;

            if (!requiredItem.SameCommonItems(other.requiredItem))
                return false;

            return true;
        }
        public bool SameExceptOneRequiredItemCompareTypesIgnoreStack(RecipeData other) {
            if (!requiredTile.ExactSame(other.requiredTile))
                return false;

            if (!requiredItem.SameCommonItemsCompareTypesIgnoreStack(other.requiredItem))
                return false;

            return true;
        }
        public virtual bool TryCondenseRecipe(RecipeData other) {
			if (!createItem.SameCommonItemsCompareTypesIgnoreStack(other.createItem))
                return false;

            bool added = false;
            if (SameExeptOneTileCompareTypesIgnoreStack(other)) {
                if (requiredTile.TryAdd(other.requiredTile))
                    added = true;
            }

            if (SameExceptOneRequiredItemCompareTypesIgnoreStack(other)) {
                if (requiredItem.TryAdd(other.requiredItem))
                    added = true;
            }

            if (added)
                createItem.TryAdd(other.createItem);

            return added;
        }
        public bool IsReverseRecipe(int type) => ReverseRecipeItemTypes.Contains(type);
        public override string ToString() {
            string s = "";
            if (createItem.Count > 0) {
                s += createItem.ToString();
                s += "     ";
            }

            if (requiredItem.Count > 0) {
                s += requiredItem.ToString();
                s += "     ";
            }

            if (requiredTile.Count > 0)
                s += requiredTile.ToString();

            return s;
        }
    }
}
