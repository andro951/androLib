using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace androLib.Common.Utility
{
    public static class AndroLogMethods {
        private static int charNum = 0;

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


        public static void LogRest(this ILCursor c, int goBack = 0) {
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
    }
}
