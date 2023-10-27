using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static androLib.Common.Utility.PathFinderSharedFunctions;

namespace androLib.Common.Utility {
	public static class MaxDistancePathFinder {
		public static bool HasPath(int x, int y, int MaxDistance, Func<int, int, bool> CountsAsPath, Func<int, int, bool> CountsAsTarget, int XMax, int YMax, int XMin = 0, int YMin = 0) {
			countsAsPath = CountsAsPath;
			countsAsTarget = CountsAsTarget;
			maxDistance = MaxDistance;
			//Create a rectangle that is either smaller than or equal to min and max limits provided, minimizing the size of PathGrid.
			GetBoundaries(x, y, XMin, XMax, YMin, YMax, maxDistance, out int left, out int up, out int right, out int down);
			//If not limited by the min or max values, left will be negative maxDistance, right positive maxDistance, up negative maxDistance, down positive maxDistance.
			int gridSizeX = -left + right + 1;
			int gridSizeY = -up + down + 1;
			//centerX and centerY are the converted x and y coordinates in PathGrid.
			centerX = -left;
			centerY = -up;
			//xStart and yStart are the difference between the provided x and y and the center of PathGrid.
			//In FindPath(), x and y are in PathGrid coordinates.  x + xStart, y + yStart will be in the original coordinates.
			xStart = x - centerX;
			yStart = y - centerY;
			xMax = gridSizeX - 1;
			xMin = 0;
			yMax = gridSizeY - 1;
			yMin = 0;
			//Resizes PathGrid to gridSizeX by gridSizeY, and sets all values to int.MaxValue
			FillArray(ref PathGrid, gridSizeX, gridSizeY, int.MaxValue);
			//Set the starting point to 0 to prevent paths from trying to go back through it.
			PathGrid[centerX, centerY] = 0;

			bool hasPath = FindPath(centerX, centerY, 0);
			//if (Debugger.IsAttached && !hasPath) PrintPathGrid();

			PathGrid = null;
			countsAsPath = null;
			countsAsTarget = null;

			return hasPath;
		}
		private static void PrintPathGrid() {
			string path = "\n";
			int longest = 0;
			for (int x = xMin; x <= xMax; x++) {
				for (int y = yMin; y <= yMax; y++) {
					int pathGridValue = PathGrid[x, y];
					string pathGridString = pathGridValue switch {
						int.MaxValue => "X",
						0 => "S",
						_ => pathGridValue.ToString()
					};

					if (pathGridString.Length > longest)
						longest = pathGridString.Length;
				}
			}

			for (int y = yMin; y <= yMax; y++) {
				bool first = true;
				for (int x = xMin; x <= xMax; x++) {
					if (first) {
						first = false;
					}
					else {
						path += ", ";
					}

					int pathGridValue = PathGrid[x, y];
					string pathGridString = pathGridValue switch {
						int.MaxValue => "X",
						0 => "S",
						_ => pathGridValue.ToString()
					};

					path += pathGridString.PadLeft(longest);
				}

				path += "\n";
			}

			path += "\n";
			path.LogSimple();
		}
		private static int[,] PathGrid;
		private static int xStart;
		private static int yStart;
		private static int centerX;
		private static int centerY;
		private static Func<int, int, bool> countsAsPath;
		private static Func<int, int, bool> countsAsTarget;
		private static int maxDistance;
		private static int xMax;
		private static int yMax;
		private static int xMin;
		private static int yMin;
		/// <summary>
		/// Searches for a path to a position satisfied by countsAsTarget only through positions satisfied by countsAsPath.<br/>
		/// </summary>
		private static bool FindPath(int x, int y, int currentDistance, int fromDirection = -1, int previousFrom = -1) {
			//$"{x}, {y}, ({x + xStart}, {y + yStart}), currentDistance: {currentDistance}, fromDirection: {fromDirection}, previousFrom: {previousFrom}".LogSimple();
			//opposite and previousOpposite are the opposite directionIDs for the path taken to get to this point.
			//For instance, if the path taken to get here was directionID 0 (down), opposite will be 2 (up).
			//previousFrom is used to track the previous x if fromDirection is tracking y or vice versa because the path should generally go away from previous paths.
			int opposite = PathDirectionID.GetOppositeDirection(fromDirection);
			int previousOpposite = PathDirectionID.GetOppositeDirection(previousFrom);
			List<Func<bool>> directionsToCheck = new();
			for (int directionID = 0; directionID < 4; directionID++) {
				PathDirectionID.GetDirection(directionID, out int i, out int j);

				if (opposite == directionID)
					continue;

				int x2 = x + i;
				//Prevent out of bounds
				if (x2 < xMin || x2 > xMax)
					continue;

				int y2 = y + j;
				//Prevent out of bounds
				if (y2 < yMin || y2 > yMax)
					continue;

				//Increment the distance every time we move to a new position.
				int distance = currentDistance + 1;
				//PathGrid stores the distance required to get to this point by previous paths or int.MaxValue if not yet reached.
				//There is no reason to continue checking a path on a tile that has been reached by a shorter path.
				if (PathGrid[x2, y2] <= distance)
					continue;

				if (distance > maxDistance)
					continue;

				//realX and realY are converting the search area coordinates to coordinates of the original FindPath() request.
				int realX = x2 + xStart;
				int realY = y2 + yStart;

				//Base case
				if (countsAsTarget(realX, realY))
					return true;

				//Mark the grid with the distance required to get to this point on this path.
				PathGrid[x2, y2] = distance;

				int directionIDForFunc = directionID;
				directionsToCheck.Add(() => {
					if (!countsAsPath(realX, realY))
						return false;

					//Usually, the searches should only go out in 2 directions, an x direction and a y direction.
					//However, there are some situations where backtracking is the only option, so only skip the backtrack path if the
					//	backTrackX, backTrackY position isn't the starting point and doesn't count as a path.
					if (previousOpposite == directionIDForFunc) {
						GetPreviousDirection(x + xStart, y + yStart, fromDirection, previousFrom, out int backTrackX, out int backTrackY);
						if (backTrackX == centerX && backTrackY == centerY || countsAsPath(backTrackX, backTrackY))
							return false;
					}

					return FindPath(x2, y2, distance, directionIDForFunc, fromDirection == directionIDForFunc ? previousFrom : fromDirection);
				});
			}

			foreach (Func<bool> directionToCheck in directionsToCheck) {
				if (directionToCheck())
					return true;
			}

			return false;
		}
	}
}
