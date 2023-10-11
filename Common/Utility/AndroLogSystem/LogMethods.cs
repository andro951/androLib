using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace androLib.Common.Utility
{
	public static class ChatMessagesIDs {
		public const int AlwaysShowInfusionError = -6;
		public const int AlwaysShowItemInfusionPowersNotSetup = -5;
		public const int AlwaysShowDuplicateItemInWitchsShop = -4;
		public const int AlwaysShowFailedToReplaceOldItem = -3;
		public const int AlwaysShowFailedToLocateAngler = -2;
		public const int AlwaysShowUnloadedItemToInvenory = -1;
		public const int CloneFailGetEnchantedItem = 0;
		public const int GainXPPreventedLoosingExperience = 1;
		public const int DamageNPCPreventLoosingXP = 2;
		public const int DamageNPCPreventLoosingXP2 = 3;
		public const int BossBagDropsFailToFind = 4;
		public const int FailedDetermineDropItem = 5;
		public const int FailedInfuseItem = 6;
		public const int UpdatedInfusionDamageAgain = 7;
		public const int FailedUpdateItemStats = 8;
		public const int PreventInfusionDamageMultLessThan0 = 9;
		public const int FailedGetEnchantmentValueByName = 10;
		public const int FailedCheckConvertExcessExperience = 11;
		public const int LowDamagePerHitXPBoost = 12;
		public const int DetectedNonEnchantmentItem = 13;
		public const int OreInfusionPowerNotSetup = 14;
		public const int NPCSpawnSourceNotSetup = 15;
	}
	public static class LogMethods {
        private static int charNum = 0;
		public readonly static bool debugging = false;
		public readonly static bool debuggingOnTick = false;
		private static Dictionary<string, double> logsT = new Dictionary<string, double>();
		public static string reportMessage => $"\n{AndroLibGameMessages.ReportErrorToAndro.ToString().Lang(AndroMod.ModName, L_ID1.GameMessages)}";
		public static HashSet<int> LoggedChatMessagesIDs = new HashSet<int>();

		/// <summary>
		/// Prints a message to the .log file.
		/// If you want to use Log() for following execution of methods, put "\\/" as the first characters in a message at the start of a method 
		/// and "/\\" as the first characters in a message at the end of a method.
		/// This will add a "|" to Log messages made within the method to help visually follow the exicution in the .log file.
		/// </summary>
		/// <param name="s">Message that will be printed</param>
		public static void Log(this string s) {
            s.UpdateCharNum();
            ModContent.GetInstance<AndroMod>().Logger.Info(s.AddCharToFront());
            s.UpdateCharNum(true);
        }

        public static void LogSimple(this string s) {
            ModContent.GetInstance<AndroMod>().Logger.Info(s);
        }
        private static void UpdateCharNum(this string s, bool afterString = false) {
            if (afterString && s.Substring(0, 2) == "\\/") {
                charNum++;
            }
            else if (!afterString && s.Substring(0, 2) == "/\\") {
                charNum--;
            }
        }
        private static string AddCharToFront(this string s, char c = '|') => new string(c, charNum) + s;

		/// <summary>
		/// Prints a message to the .log file.
		/// Will not print the exact same string more than once per second. (Good for logging methods that get called every tick)
		/// If you want to use Log() for following execution of methods, put "\\/" as the first characters in a message at the start of a method 
		/// and "/\\" as the first characters in a message at the end of a method.
		/// This will add a "|" to Log messages made within the method to help visually follow the exicution in the .log file.
		/// </summary>
		/// <param name="s">Message that will be printed</param>
		public static void LogT(this string s) {
			//Try to remove any messages that were called 60 ticks or more before now.
			foreach (string key in logsT.Keys) {
				if (logsT[key] + 60 <= Main.GameUpdateCount)
					logsT.Remove(key);
			}

			if (!logsT.ContainsKey(s)) {
				s.Log();
				logsT.Add(s, Main.GameUpdateCount);
			}
		}

		public static void LogILRest(this ILCursor c, int goBack = 0) {
			int index = c.Index;
            c.Index = index - goBack;
			while (c.Next != null) {
				bool catchingExceptions = true;
				$"c.Index: {c.Index}, Instruction: {c.Next}{(c.Index == index ? $" (Cursor Location)" : "")}".LogSimple();
				while (catchingExceptions) {
					c.Index++;
					try {
						if (c.Next != null) {
							string tempString = c.Next.ToString();
						}
						catchingExceptions = false;
					}
					catch (Exception e) {
						$"c.Index: {c.Index}, Instruction: {e.ToString().Substring(0, 20)}".LogSimple();
					}
				}
			}

			c.Index = index;
		}


		/// <summary>
		/// Prints a message in game and the .log file.<br/>
		/// Adds reportMessage to the end:<br/>
		/// Please report this to andro951(Weapon Enchantments) allong with a description of what you were doing at the time.<br/>
		/// <see cref="ChatMessagesIDs">ChatMessagesIDs</see><br/>
		/// </summary>
		/// <param name="s">Message that will be printed </param>
		/// <param name="messageID"></param>
		public static void LogNT(this string s, int messageID) {
			s += AndroLibGameMessages.MainUpdateCount.ToString().Lang(AndroMod.ModName, L_ID1.GameMessages, new object[] { Main.GameUpdateCount }) + reportMessage;

			if (Main.netMode < NetmodeID.Server) {
				bool doChatMessage = messageID < 0;
				if (!doChatMessage) {
					if (AndroMod.clientConfig.OnlyShowErrorMessagesInChatOnce) {
						if (!LoggedChatMessagesIDs.Contains(messageID)) {
							LoggedChatMessagesIDs.Add(messageID);
							doChatMessage = true;
						}
					}
					else if (!AndroMod.clientConfig.DisableAllErrorMessagesInChat) {
						doChatMessage = true;
					}
				}

				if (doChatMessage)
					Main.NewText(s);
			}

			s.Log();
		}

		public static void LogSimpleNT(this string s) {
			if (Main.netMode < NetmodeID.Server && !Main.gameMenu)
				Main.NewText(s);

			s.LogSimple();
		}
	}
}
