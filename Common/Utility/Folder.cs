using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace androLib.Common.Utility
{
	public class Folder : MyFile
	{
		DirectoryInfo MyDirectoryInfo;
		public List<MyFile> files = new();
		private List<TXT> txtFiles = null;
		public List<TXT> TXTFiles {
			get {
				if (txtFiles == null) {
					txtFiles = new();
					foreach (TXT txt in files.OfType<TXT>()) {
						txtFiles.Add(txt);
					}

					foreach (Folder folder in files.OfType<Folder>()) {
						List<TXT> folderFiles = folder.TXTFiles;
						txtFiles = txtFiles.Concat(folderFiles).ToList();
					}
				}

				return txtFiles;
			}
		}
		public bool HasFile(string path, out TXT txt) {
			List<TXT> txtFiles = TXTFiles.Where(t => t.CompairPath == path).ToList();
			txt = txtFiles.Count > 0 ? txtFiles[0] : null;

			return txt != null;
		}
		public bool NewFile(string path, string text, out bool textChanged) {
			bool hasFile = HasFile(path, out TXT txt);
			textChanged = hasFile ? txt.DifferentText(text) : false;

			return !hasFile;
		}
		public Folder(string name) : base(name, null) {
			Name = name;
			string path = ActualPath;
			CheckCreate(path);
			MyDirectoryInfo = new DirectoryInfo(path);
			GetFiles();
		}

		public static void CheckCreate(string path) {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}

		public void CheckCreateTXT(string subfolderString, string fileName) {//TODO: change this to List<string> subfolders and add the folders and new file to this file.
			string path = @$"{ActualPath}\{subfolderString}";
			CheckCreate(path);
			TXT.CheckCreate(path, fileName, true);
		}

		public void WriteTXT(string subfolderString, string fileName, string text) {//Could be better integrated with TXT instead of manually creating the path.
			CheckCreateTXT(subfolderString, fileName);
			string path = @$"{ActualPath}\{subfolderString}\{fileName}.txt";
			File.WriteAllText(path, text);
		}

		private Folder(DirectoryInfo directoryInfo, Folder parent) : base(directoryInfo.Name, parent) {
			MyDirectoryInfo = directoryInfo;
			GetFiles();
		}

		public void SetParent() {
			IsMaster = true;
		}

		private void GetFiles() {
			foreach (DirectoryInfo directoryInfo in MyDirectoryInfo.GetDirectories()) {
				Folder newFolder = new(directoryInfo, this);
				files.Add(newFolder);
			}

			foreach (FileInfo fileInfo in MyDirectoryInfo.GetFiles()) {
				TXT newTXT = new(fileInfo.Name, this);
				files.Add(newTXT);
			}
		}
	}
}
