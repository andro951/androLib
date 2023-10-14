using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace androLib.Common.Utility
{
	public class TXT : MyFile
	{
		string MyText => File.ReadAllText(ActualPath);
		public TXT(string name, MyFile parent) : base(name, parent) {
			CheckCreate(parent.ActualPath, name);
		}

		public bool DifferentText(string text) => text != MyText;
		public static void CheckCreate(string path, string fileName, bool addDotTxt = false) {
			path += @$"\{fileName}";
			if (addDotTxt)
				path += ".txt";

			if (!File.Exists(path)) {
				File.Create(path).Close();
			}
		}
	}
}
