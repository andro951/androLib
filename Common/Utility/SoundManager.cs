using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;

namespace androLib.Common.Utility
{
	public struct SoundWithPosition {
		public SoundStyle SoundStyle { get; }
		public Vector2 WorldPosition { get; }
		public SoundWithPosition(SoundStyle soundStyle, Vector2 worldPosition) {
			SoundStyle = soundStyle;
			WorldPosition = worldPosition;
		}
	}
	public static class SoundManager {
		private static int delayedSoundsCounter = 0;
		private static DuplicateDictionary<int, SoundWithPosition> delayedSounds = new();

		public static void QueDelyedSound(SoundStyle soundStyle, Vector2 worldPosition, int delay) {
			delayedSounds.Add(delayedSoundsCounter + delay, new(soundStyle, worldPosition));
		}

		public static void Update() {
			if (delayedSounds.Count > 0) {
				delayedSoundsCounter++;
				while (delayedSounds.Count > 0 && delayedSoundsCounter >= delayedSounds.Keys[0]) {
					int firstKey = delayedSounds.Keys[0];
					SoundWithPosition firstSound = delayedSounds[firstKey];
					SoundEngine.PlaySound(firstSound.SoundStyle, firstSound.WorldPosition);
					delayedSounds.Remove(firstKey);
				}
			}
			else {
				delayedSoundsCounter = 0;
			}
		}
	}
}
