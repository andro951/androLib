using androLib.Common.Utility;
using androLib.IO.TerrariaAutomations;
using log4net;
using MagicStorage;
using MonoMod.RuntimeDetour;
using ReLogic.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;
using Terraria.Utilities;

namespace androLib.IO {
	internal static class WorldFileManager {
		internal enum WorldFileID : byte {
			TerrariaAutomations
		}
		private static SortedDictionary<WorldFileID, WorldFileData> AllWorldFileData = new();
		private static IEnumerable<WorldFileData> AllWorldFileDataActiveForSaving => AllWorldFileData.Where(d => d.Value.Loaded).Select(d => d.Value).OrderBy(d => d.ModId);
		private struct WorldFileData {
			public WorldFileID ModId;
			public Action<BinaryWriter> Save;
			public Action<BinaryReader> Load;
			public Func<BinaryReader, bool> ValidateSave;
			public bool Loaded => ModLoader.HasMod(ModId.ToString());
			public WorldFileData(WorldFileID modId, Action<BinaryWriter> save, Action<BinaryReader> load) {
				ModId = modId;
				if (AllWorldFileData.Select(d => d.Value.ModId).Contains(modId))
					throw new Exception($"WorldFileID {modId} is already registered.");

				Save = save;
				Load = load;
			}
		}
		public static void RegisterWorldFileData(WorldFileID modId, Action<BinaryWriter> save, Action<BinaryReader> load) {
			AllWorldFileData.Add(modId, new WorldFileData(modId, save, load));
		}

		#region Load

		private static List<Hook> hooks = new();
		public static void Load() {
			hooks.Add(new(WorldIO_SaveMethodInfo, WorldIO_SaveDetour));
			hooks.Add(new(WorldIO_LoadMethodInfo, WorldIO_LoadDetour));
			hooks.Add(new(WorldIO_MoveToCloudMethodInfo, WorldIO_MoveToCloudDetour));
			hooks.Add(new(WorldIO_MoveToLocalMethodInfo, WorldIO_MoveToLocalDetour));
			hooks.Add(new(WorldIO_LoadBackupMethodInfo, WorldIO_LoadBackupDetour));
			hooks.Add(new(WorldIO_LoadDedServBackupMethodInfo, WorldIO_LoadDedServBackupDetour));
			hooks.Add(new(WorldIO_RevertDedServBackupMethodInfo, WorldIO_RevertDedServBackupDetour));
			hooks.Add(new(WorldIO_EraseWorldMethodInfo, WorldIO_EraseWorldDetour));
			foreach (Hook hook in hooks) {
				hook.Apply();
			}

			TA_WorldFile.Load();
		}

		private delegate void orig_WorldIO_Save(string path, bool isCloudSave);
		private delegate void hook_WorldIO_Save(orig_WorldIO_Save orig, string path, bool isCloudSave);
		private static Assembly tModLoaderAssembly = typeof(ItemLoader).Assembly;
		private static Type WorldIOType = tModLoaderAssembly.GetType("Terraria.ModLoader.IO.WorldIO");
		private static readonly MethodInfo WorldIO_SaveMethodInfo = WorldIOType.GetMethod("Save", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_SaveDetour(orig_WorldIO_Save orig, string path, bool isCloudSave) {
			orig(path, isCloudSave);
			SaveAllWorldData(path, isCloudSave);
		}

		private delegate void orig_WorldIO_Load(string path, bool isCloudSave);
		private delegate void hook_WorldIO_Load(orig_WorldIO_Load orig, string path, bool isCloudSave);
		private static readonly MethodInfo WorldIO_LoadMethodInfo = WorldIOType.GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_LoadDetour(orig_WorldIO_Load orig, string path, bool isCloudSave) {
			orig(path, isCloudSave);
			LoadAllWorldData(path, isCloudSave);
		}

		private delegate void orig_WorldIO_MoveToCloud(string localPath, string cloudPath);
		private delegate void hook_WorldIO_MoveToCloud(orig_WorldIO_MoveToCloud orig, string localPath, string cloudPath);
		private static readonly MethodInfo WorldIO_MoveToCloudMethodInfo = WorldIOType.GetMethod("MoveToCloud", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_MoveToCloudDetour(orig_WorldIO_MoveToCloud orig, string localPath, string cloudPath) {
			orig(localPath, cloudPath);
			MoveToCloud(localPath, cloudPath);
		}

		private delegate void orig_WorldIO_MoveToLocal(string cloudPath, string localPath);
		private delegate void hook_WorldIO_MoveToLocal(orig_WorldIO_MoveToLocal orig, string cloudPath, string localPath);
		private static readonly MethodInfo WorldIO_MoveToLocalMethodInfo = WorldIOType.GetMethod("MoveToLocal", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_MoveToLocalDetour(orig_WorldIO_MoveToLocal orig, string cloudPath, string localPath) {
			orig(cloudPath, localPath);
			MoveToLocal(cloudPath, localPath);
		}

		private delegate void orig_WorldIO_LoadBackup(string path, bool cloudSave);
		private delegate void hook_WorldIO_LoadBackup(orig_WorldIO_LoadBackup orig, string path, bool cloudSave);
		private static readonly MethodInfo WorldIO_LoadBackupMethodInfo = WorldIOType.GetMethod("LoadBackup", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_LoadBackupDetour(orig_WorldIO_LoadBackup orig, string path, bool cloudSave) {
			orig(path, cloudSave);
			LoadBackup(path, cloudSave);
		}

		private delegate void orig_WorldIO_LoadDedServBackup(string path, bool cloudSave);
		private delegate void hook_WorldIO_LoadDedServBackup(orig_WorldIO_LoadDedServBackup orig, string path, bool cloudSave);
		private static readonly MethodInfo WorldIO_LoadDedServBackupMethodInfo = WorldIOType.GetMethod("LoadDedServBackup", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_LoadDedServBackupDetour(orig_WorldIO_LoadDedServBackup orig, string path, bool cloudSave) {
			orig(path, cloudSave);
			LoadDedServBackup(path, cloudSave);
		}

		private delegate void orig_WorldIO_RevertDedServBackup(string path, bool cloudSave);
		private delegate void hook_WorldIO_RevertDedServBackup(orig_WorldIO_RevertDedServBackup orig, string path, bool cloudSave);
		private static readonly MethodInfo WorldIO_RevertDedServBackupMethodInfo = WorldIOType.GetMethod("RevertDedServBackup", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_RevertDedServBackupDetour(orig_WorldIO_RevertDedServBackup orig, string path, bool cloudSave) {
			orig(path, cloudSave);
			RevertDedServBackup(path, cloudSave);
		}

		private delegate void orig_WorldIO_EraseWorld(string path, bool cloudSave);
		private delegate void hook_WorldIO_EraseWorld(orig_WorldIO_EraseWorld orig, string path, bool cloudSave);
		private static readonly MethodInfo WorldIO_EraseWorldMethodInfo = WorldIOType.GetMethod("EraseWorld", BindingFlags.NonPublic | BindingFlags.Static);
		private static void WorldIO_EraseWorldDetour(orig_WorldIO_EraseWorld orig, string path, bool cloudSave) {
			orig(path, cloudSave);
			EraseWorld(path, cloudSave);
		}

		#endregion

		internal static ILog worldFileManagerLogger { get; } = LogManager.GetLogger("AndroModWorldFileManager");
		private const string AndroModWorldFileExtension = ".amwld";
		private static void SaveAllWorldData(string path, bool isCloudSave) {
			path = Path.ChangeExtension(path, AndroModWorldFileExtension);

			IEnumerable<WorldFileData> allActiveWorldFileData = AllWorldFileDataActiveForSaving;
			if (!allActiveWorldFileData.Any())
				return;

			int num;
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream(31457280)) {
				using (BinaryWriter writer = new BinaryWriter(memoryStream)) {
					writer.Write((byte)allActiveWorldFileData.Count());

					foreach (WorldFileData dataFile in allActiveWorldFileData) {
						writer.Write((byte)dataFile.ModId);
					}

					foreach (WorldFileData worldFileData in allActiveWorldFileData) {
						worldFileData.Save(writer);
					}
				}

				array = memoryStream.ToArray();
				num = array.Length;
			}

			if (FileUtilities.Exists(path, isCloudSave))
				FileUtilities.Copy(path, path + ".bak", isCloudSave);

			FileUtilities.Write(path, array, num, isCloudSave);

			if (Debugger.IsAttached) {
				LoadAllWorldData(path, isCloudSave);
				TA_WorldFile.CheckLoadVsBeforeSave();
			}
		}
		private static void LoadAllWorldData(string path, bool isCloudSave) {
			path = Path.ChangeExtension(path, AndroModWorldFileExtension);
			if (!FileUtilities.Exists(path, isCloudSave))
				return;

			using MemoryStream memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(path, isCloudSave));
			using BinaryReader binaryReader = new BinaryReader(memoryStream);

			byte dataCount = binaryReader.ReadByte();
			if (dataCount == 0)
				return;

			SortedSet<WorldFileID> savedDataIds = new(binaryReader.ReadBytes(dataCount).Select(b => (WorldFileID)b));
			foreach (byte savedDataIDByte in savedDataIds) {
				WorldFileID savedDataID = (WorldFileID)savedDataIDByte;
				if (!AllWorldFileData.TryGetValue(savedDataID, out WorldFileData data))
					throw new Exception($"Unable to load AndroLibWorldFileData because WorldFileData manager for {savedDataID} isn't loaded.");
			}

			foreach (WorldFileData worldFileData in AllWorldFileData.Where(d => savedDataIds.Contains(d.Key)).Select(d => d.Value).OrderBy(d => d.ModId)) {
				worldFileData.Load(binaryReader);
			}
		}
		private static void MoveToCloud(string localPath, string cloudPath) {
			localPath = Path.ChangeExtension(localPath, AndroModWorldFileExtension);
			cloudPath = Path.ChangeExtension(cloudPath, AndroModWorldFileExtension);
			if (File.Exists(localPath)) {
				FileUtilities.MoveToCloud(localPath, cloudPath);
			}
		}
		private static void MoveToLocal(string cloudPath, string localPath) {
			cloudPath = Path.ChangeExtension(cloudPath, AndroModWorldFileExtension);
			localPath = Path.ChangeExtension(localPath, AndroModWorldFileExtension);
			if (FileUtilities.Exists(cloudPath, true)) {
				FileUtilities.MoveToLocal(cloudPath, localPath);
			}
		}
		private static void LoadBackup(string path, bool cloudSave) {
			path = Path.ChangeExtension(path, AndroModWorldFileExtension);
			if (FileUtilities.Exists(path + ".bak", cloudSave)) {
				FileUtilities.Move(path + ".bak", path, cloudSave, true);
			}
		}
		private static void LoadDedServBackup(string path, bool cloudSave) {
			path = Path.ChangeExtension(path, AndroModWorldFileExtension);
			if (FileUtilities.Exists(path, cloudSave)) {
				FileUtilities.Copy(path, path + ".bad", cloudSave, true);
			}

			if (FileUtilities.Exists(path + ".bak", cloudSave)) {
				FileUtilities.Copy(path + ".bak", path, cloudSave, true);
				FileUtilities.Delete(path + ".bak", cloudSave);
			}
		}
		private static void RevertDedServBackup(string path, bool cloudSave) {
			path = Path.ChangeExtension(path, AndroModWorldFileExtension);
			if (FileUtilities.Exists(path, cloudSave)) {
				FileUtilities.Copy(path, path + ".bak", cloudSave, true);
			}

			if (FileUtilities.Exists(path + ".bad", cloudSave)) {
				FileUtilities.Copy(path + ".bad", path, cloudSave, true);
				FileUtilities.Delete(path + ".bad", cloudSave);
			}
		}
		private static void EraseWorld(string path, bool cloudSave) {
			path = Path.ChangeExtension(path, AndroModWorldFileExtension);
			if (!cloudSave) {
				Platform.Get<IPathService>().MoveToRecycleBin(path);
				Platform.Get<IPathService>().MoveToRecycleBin(path + ".bak");
			}
			else if (SocialAPI.Cloud != null) {
				SocialAPI.Cloud.Delete(path);
			}
		}
	}
}
