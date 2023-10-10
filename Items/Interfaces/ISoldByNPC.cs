using Terraria;
using Terraria.ModLoader;
using androLib.Common.Utility;
using System;

namespace androLib.Items
{
	public interface ISoldByNPC
	{
		public abstract Func<int> SoldByNPCNetID { get; }
		public virtual SellCondition SellCondition => SellCondition.AnyTime;
		public virtual float SellPriceModifier => SellCondition.GetSellPriceModifier();
		public virtual Func<ModItem, string, string> GetNonStandardWikiLinkString => null;
	}
}
