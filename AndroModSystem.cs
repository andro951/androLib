﻿using androLib.Common.Utility;
using androLib.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace androLib
{
	internal class AndroModSystem : ModSystem {
		public static bool FavoriteKeyDown => Main.keyState.IsKeyDown(Main.FavoriteKey);
		public override void PostDrawInterface(SpriteBatch spriteBatch) {
			MasterUIManager.PostDrawInterface(spriteBatch);
		}
		public override void AddRecipeGroups() {
			RecipeGroup group = new RecipeGroup(() => "Any Common Gem", GemSets.CommonGems.ToArray());
			RecipeGroup.RegisterGroup("androLib:CommonGems", group);

			group = new RecipeGroup(() => "Any Rare Gem", GemSets.RareGems.ToArray());
			RecipeGroup.RegisterGroup("androLib:RareGems", group);
		}
	}
}
