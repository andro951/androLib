﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using androLib.Common.Utility;
using androLib.Common.Globals;

namespace androLib.Common.Utility.LogSystem.WebpageComponenets
{
    public class WebPage
    {
        public string HeaderName { private set; get; }
        private string MyDirectoryNoHeaderName => !IsMaster ? $"{Parent.MyDirectory}" : $"{Wiki.wikiPath}";
        private string MyDirectory => !IsMaster ? $"{Parent.MyDirectory}\\{HeaderName}" : $"{Wiki.wikiPath}\\{HeaderName}";
        public string ParentDirectory => !IsMaster ? $"{Parent.ParentDirectory}\\{HeaderName}" : $"{HeaderName}";
        public bool IsParent = false;
        public bool IsMaster = false;
        public string Path => $"{MyDirectoryNoHeaderName}\\{CheckDiffHeaderName()}.txt";
        public string CompairPath => $"{ParentDirectory}.txt";
        public WebPage Parent;
        private List<object> _elements = new();
        protected string MainWikiPage = null;
        public static string MainPageName = "Main Page";
		public WebPage(string headerName, WebPage parent = null, string mainWikiPage = null) {
            HeaderName = headerName;
            Parent = parent;
            if (Parent != null) {
				Parent.MakeParent();
				AddLink(MainWikiPage, MainPageName);
			}
            else {
                MakeParent();
				MainWikiPage = mainWikiPage;
			}
        }
        public void Add(object obj) => _elements.Add(obj);
        public void AddSubHeading(string text, int num = 1) => Add(new SubHeading(text, num));
        public void AddParagraph(string text) => Add(new Paragraph(text));
        public void AddLink(string s, string text = null, bool png = false) => Add(new Link(s, text, png));
        public void AddPNG(string s, bool removeSpaces = true) => Add(s.ToPNG(removeSpaces) + "<br/>");
        public void AddBulletedList(bool png = false, bool links = false, params object[] elements) => Add(new BulletedList(png, links, elements));
        public void AddTable<T>(IEnumerable<IEnumerable<T>> elements, IEnumerable<string> headers = null, string label = null, bool firstRowHeaders = false, bool sortable = false, bool collapsible = false, bool collapsed = false, bool rowspanColumns = false, bool automaticCollapse = false, int maxWidth = 0, FloatID alignID = FloatID.none) where T : class {
            if (elements.Count() > 0)
                Add(new Table<T>(elements, headers, label, firstRowHeaders, sortable, collapsible, collapsed, rowspanColumns, automaticCollapse, maxWidth, alignID));
        }
        public void AddTable(Table<string> table) {
            if (table != null && table.Count > 0)
                Add(table);
		}
        public void NewLine(int num = 1) => _elements.Add('\n'.FillString(num - 1));
        public void WriteAllTextToFile() {
			File.WriteAllText(Path, $"{this}");
		}
        public void MakeParent() {
            if (IsParent)
                return;

			if (Parent == null)
				IsMaster = true;

			if (!IsParent && Debugger.IsAttached)
				Directory.CreateDirectory($"{MyDirectory}");

			IsParent = true;
		}
        public string CheckDiffHeaderName() {
            string headerName = HeaderName;
            if (Wiki.LastWikiDirectory != null) {
                bool addToChangeSumary = false;
                bool textChanged = false;
				if (Wiki.wikiFolder?.NewFile(CompairPath, $"{this}", out textChanged) == true) {
					headerName += " (New)";
                    addToChangeSumary = true;
				}
				else if (textChanged) {
					headerName += " (Changed)";
					addToChangeSumary = true;
				}

                if (addToChangeSumary)
				    Wiki.changesSumary += $"{headerName}\n";
			}

			return headerName;
        }
		public override string ToString() {
            string text = "";
            object last = new();
            Dictionary<FloatID, List<object>> alignedLists = _elements.GroupBy(l => l is WebpageComponent component ? component.AlignID : FloatID.none).ToDictionary(g => g.Key, g => g.ToList());
            for(FloatID i = FloatID.right; i >= FloatID.none; i--) {
                if (!alignedLists.ContainsKey(i))
                    continue;

                List<object> list = alignedLists[i];
                if (i != FloatID.none)
                    text += $"<div style=\"float:{i}\">\n";

                foreach (object element in list) {
                    text += $"{element}\n";
                }

                if (i != FloatID.none)
                    text += "</div>\n";
            }

            return text;
        }
    }
}
