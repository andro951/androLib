using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using androLib.Common.Utility;
using androLib.Common.Globals;

namespace androLib.Common.Utility.LogSystem.WebpageComponenets
{
	public class ObjectList
	{
		List<object> list = new();
		public void Add(object obj) => list.Add(obj);
		public void AddTable(Table<string> table) {
			if (table != null && table.Count > 0)
				list.Add(table);
		}
		public override string ToString() {
			List<string> strings = new List<string>();
			foreach(object obj in list) {
				string s = obj.ToString();
				strings.Add(s);
			}

			string text = strings.JoinList("\n");

			return text;
		}
	}
}
