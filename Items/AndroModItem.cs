using androLib.Common.Utility;
using androLib.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace androLib.Items
{
	public abstract class AndroModItem : ModItem {
		public abstract List<WikiTypeID> WikiItemTypes { get; }
		public abstract string Artist { get; }
		public virtual string ArtModifiedBy => null;
		public abstract string Designer { get; }
		public virtual string WikiDescription => null;
		public virtual string LocalizationTooltip { protected set; get; }
		protected string localizationTooltip;
		public override void SetStaticDefaults() {
			if (Tooltip != LocalizedText.Empty)
				this.AddLocalizationTooltip(LocalizationTooltip);
		}
	}
}
