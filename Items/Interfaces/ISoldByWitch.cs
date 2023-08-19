using Terraria;
using Terraria.ModLoader;
using androLib.Common.Utility;

namespace androLib.Items
{
	public interface ISoldByWitch
	{
		public virtual SellCondition SellCondition => SellCondition.AnyTime;
		public virtual float SellPriceModifier => SellCondition.GetSellPriceModifier();
	}
}
