﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Personalities;
using androLib.Common.Utility;

namespace androLib.Content.NPCs
{
	public interface INPCWikiInfo
	{
		public virtual DropRestrictionsID DropRestrictionsID => DropRestrictionsID.None;
		public List<WikiTypeID> WikiNPCTypes { get; }
		public string Artist { get; }
		public string ArtModifiedBy => null;
		public bool TownNPC { get; }
		public Dictionary<IShoppingBiome, AffectionLevel> BiomeAffections { get; }
		public Dictionary<int, AffectionLevel> NPCAffections { get; }
		public string SpawnCondition { get; }
	}
}
