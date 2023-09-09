using androLib.Common.Globals;
using androLib.Common.Utility;
using androLib.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace androLib
{
	public class AndroModSystem : ModSystem {
		public static bool FavoriteKeyDown => Main.keyState.IsKeyDown(Main.FavoriteKey);
		public static bool StartedPostAddRecipes { get; private set; } = false;
		public override void PostAddRecipes() {
			StartedPostAddRecipes = true;
		}
		public override void PostDrawInterface(SpriteBatch spriteBatch) {
			MasterUIManager.PostDrawInterface(spriteBatch);
		}
		private static bool printModItemName => false;
		public override void PostUpdateEverything() {
			if (Debugger.IsAttached && !Main.LocalPlayer.HeldItem.NullOrAir()) {//temp
				Item item = Main.LocalPlayer.HeldItem;
				string temp = item.ModFullName();
				int createTile = item.createTile;
				int useStyle = item.useStyle;
				bool useTurn = item.useTurn;
				bool autoReuse = item.autoReuse;
				bool consumable = item.consumable;
				if (item.DamageType != DamageClass.Default) {
					string temp2 = item.DamageType.Name;
				}

				if (item.ModItem != null) {
					ModItem modItem = item.ModItem;
					string modItemName = modItem.Name;
					if (modItem is UnloadedItem unloadedItem) {
						string unloadedItemName = unloadedItem.Name;
						string unloadedItemFullName = unloadedItem.FullName;
						string unloadedItemItemName = unloadedItem.ItemName;
						string unloadedItemModName = unloadedItem.ModName;
					}
				}

				if (printModItemName && !Main.mouseItem.NullOrAir() && Main.mouseItem.ModItem != null)
					Main.NewText(Main.mouseItem.ModItem.Name);
			}

			if (Debugger.IsAttached && !Main.HoverItem.NullOrAir()) {
				string hoverItemName = Main.HoverItem.ModFullName();
				if (printModItemName)
					Main.NewText(hoverItemName);
			}

			SoundManager.Update();
		}
		public override void AddRecipeGroups() {
			RecipeGroup group = new RecipeGroup(() => "Any Common Gem", GemSets.CommonGems.ToArray());
			RecipeGroup.RegisterGroup("androLib:CommonGems", group);

			group = new RecipeGroup(() => "Any Rare Gem", GemSets.RareGems.ToArray());
			RecipeGroup.RegisterGroup("androLib:RareGems", group);
		}
	}
}
