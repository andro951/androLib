using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using androLib.Common.Utility;
using androLib.Common.Globals;
using androLib.Common.Utility.LogSystem;
using androLib.Items;

namespace androLib.Common.Utility.LogSystem.WebpageComponenets
{
    public class ItemInfoBox : WebpageComponent {
		public ItemInfoBox(AndroModItem androModItem, FloatID id = FloatID.none) {
			this.androModItem = androModItem;
			name = androModItem.Item.Name;
			itemInfo = new ItemInfo(androModItem);
			AlignID = id;
		}
		public string Name {
			get {
				if (name == null)
					return "";
			
				return name;
			}
		}
		private string name;
		public string WikiDescription {
			get {
				if (androModItem == null)
					return "";
			
				string wikiDescription = androModItem.WikiDescription;
			
				return wikiDescription ?? "";
			}
		}

		public AndroModItem androModItem;
		ItemInfo itemInfo;
		public void AddStatistics(WebPage webPage) => webPage.Add(this);
		public void AddDrops(WebPage webPage) {
			if (androModItem == null)
				return;

			itemInfo.AddDrops(webPage);
		}
		public void AddInfo(WebPage webPage) {
			if (androModItem == null)
				return;

			itemInfo.AddInfo(webPage);
		}
		public void AddRecipes(WebPage webPage) {
			webPage.AddSubHeading("Crafting");
			webPage.AddTable(itemInfo.RecipesCreateItemTable);
			webPage.AddTable(itemInfo.RecipesUsedInTable);
			webPage.AddTable(itemInfo.RecipesReverseRecipesTable);
		}
		public void TryAddWikiDescription(WebPage webPage) {
			string text = WikiDescription;
			if (text != null)
				webPage.AddParagraph(text);
		}
		public override string ToString() {
			if (androModItem == null)
				return "";
			
			string text = 
				$"{"{{"}Item Infobox\n" + 
				$"| name     = {name}\n\n";

			text += $"| image   = {itemInfo.Image}\n";

			itemInfo.GetArtists(out string artistString, out string artModifiedBy);

			if (artistString != null || artModifiedBy != null)
				text += $"| artist  = {artistString}{artModifiedBy}\n";

			text +=
				$"| type    = {itemInfo.GetItemTypes()}\n" +
				$"| tooltip = <i>'{itemInfo.Tooltip}'</i>\n" +
				$"| rarity  = {itemInfo.Rarity}\n" +
				$"| buy      = {itemInfo.ShopPrice.GetCoinsPNG()}\n" +
				$"| sell    = {(itemInfo.Item.value / 5).GetCoinsPNG()}\n\n";

			text += 
				$"| research = {itemInfo.Research}\n" +
				$"{"}}"}\n";
		
            return text;
		}
    }
}
