using androLib;
using androLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.ID.ContentSamples.CreativeHelper;
using Terraria.ModLoader;
using Terraria;

namespace VacuumBags.Items {
	public abstract class AllowedListBagModItem : BagModItem, INeedsSetUpAllowedList {
		public override void RegisterWithAndroLib(Mod mod) {
			base.RegisterWithAndroLib(mod);

			INeedsSetUpAllowedList.RegisterAllowedItemsManager(BagStorageID, CreateAllowedItemsManager);
		}
		public override bool ItemAllowedToBeStored(Item item) => AllowedItems.Contains(item.type);
		protected override void UpdateAllowedList(int item, bool add) {
			if (add) {
				AllowedItems.Add(item);
			}
			else {
				AllowedItems.Remove(item);
			}
		}
		public SortedSet<int> AllowedItems => GetAllowedItemsManager.AllowedItems;
		public AllowedItemsManager GetAllowedItemsManager => INeedsSetUpAllowedList.AllowedItemsManagers[BagStorageID];
		public Func<AllowedItemsManager> CreateAllowedItemsManager => () => new(GetBagType, () => BagStorageID, DevCheck, DevWhiteList, DevModWhiteList, DevBlackList, DevModBlackList, ItemGroups, EndWords, SearchWords, PostSetup);
		public virtual void PostSetup() {}
		public virtual bool? DevCheck(ItemSetInfo info, SortedSet<ItemGroup> itemGroups, SortedSet<string> endWords, SortedSet<string> searchWords) {
			return null;
		}
		public virtual SortedSet<int> DevWhiteList() {
			SortedSet<int> devWhiteList = new();

			return devWhiteList;
		}
		public virtual SortedSet<string> DevModWhiteList() {
			SortedSet<string> devModWhiteList = new() {

			};

			return devModWhiteList;
		}
		public virtual SortedSet<int> DevBlackList() {
			SortedSet<int> devBlackList = new() {

			};

			return devBlackList;
		}
		public virtual SortedSet<string> DevModBlackList() {
			SortedSet<string> devModBlackList = new() {

			};

			return devModBlackList;
		}
		public virtual SortedSet<ItemGroup> ItemGroups() {
			SortedSet<ItemGroup> itemGroups = new() {

			};

			return itemGroups;
		}
		public virtual SortedSet<string> EndWords() {
			SortedSet<string> endWords = new() {

			};

			return endWords;
		}
		public virtual SortedSet<string> SearchWords() {
			SortedSet<string> searchWords = new() {

			};

			return searchWords;
		}
	}
}
