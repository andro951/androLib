using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace androLib.Common.Utility {
	public static class PathFinderSharedFunctions {
		public static void GetBoundaries(int x, int y, int xMin, int xMax, int yMin, int yMax, int radius, out int left, out int up, out int right, out int down) {
			left = Math.Max(-radius, xMin - x);
			up = Math.Max(-radius, yMin - y);
			right = Math.Min(radius, xMax - x);
			down = Math.Min(radius, yMax - y);
		}

		/// <summary>
		/// Prevents the path from going almost in a circle by doing something such as down 1, left 1, up 1 if the previous coordinates count as a path.<br/>
		/// x0 and y0 are the coordinates of the current search.  x and y will be back 1 in both of the previous directions.<br/>
		/// . . 3 0<br/>
		/// . . 2 1<br/>
		/// . . . .<br/>
		/// . . . .<br/>
		/// Prevents the path to 3 because it can be reached by a shorter path from the starting point, 0.
		/// </summary>
		public static void GetPreviousDirection(int x0, int y0, int fromDirection, int previousFrom, out int x, out int y) {
			//directionID:  i,  j
			//down		0:  0,  1
			//right		1:  1,  0
			//up		2:  0, -1
			//left		3: -1,  0
			x = x0;
			y = y0;
			bool fromPositiveY = fromDirection == 0 || previousFrom == 0;
			bool fromPositiveX = fromDirection == 1 || previousFrom == 1;
			if (fromPositiveY) {
				y--;
			}
			else {
				y++;
			}

			if (fromPositiveX) {
				x--;
			}
			else {
				x++;
			}
		}
		public static void FillArray<T>(ref T[,] arr, int xLen, int yLen, T value) {
			arr = new T[xLen, yLen];
			for (int y = 0; y < yLen; y++) {
				for (int x = 0; x < xLen; x++) {
					arr[x, y] = value;
				}
			}
		}
	}
	public static class PathDirectionID {
		public const int Down = 0;
		public const int Right = 1;
		public const int Up = 2;
		public const int Left = 3;
		public static void GetDirection(int directionID, out int i, out int j) {
			i = directionID % 2;
			j = 1 - i;
			if (directionID > 1) {
				i *= -1;
				j *= -1;
			}

			//directionID:  i,  j
			//down		0:  0,  1
			//right		1:  1,  0
			//up		2:  0, -1
			//left		3: -1,  0
		}
		public static void GetDirection(int directionID, int x, int y, out int i, out int j) {
			GetDirection(directionID, out i, out j);
			i += x;
			j += y;
		}

		public static int GetOppositeDirection(int directionID) => directionID >= 0 ? (directionID + 2) % 4 : -1;
	}
}
