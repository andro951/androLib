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
