using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace androLib.Common.Utility.PathFinders {
	public static class AllConnectedPathFinder {
		private static SortedDictionary<int, Queue<Action>> pathChecks = new();
		private static Queue<Action> activePath = null;
		public const int PathCheckDefaultID = -1;
		private static int pathsChecked = 0;
		/// <summary>
		/// Make things happen for if the check fails or succeeds inside CountsAsTarget
		/// Check and mark as found inside of HasBeenChecked
		/// </summary>
		public static int FindAll(int x, int y, Func<int, int, int, bool> CheckPathShouldContinue, int maxPathsToCheck, int pathCheckID, int XMax, int YMax, int XMin = 0, int YMin = 0, int distance = 0) {
			if (pathCheckID == PathCheckDefaultID) {
				pathCheckID = GetNextOpenPathCheckID();
				pathChecks.Add(pathCheckID, new());
			}

			activePath = pathChecks[pathCheckID];
			checkPathShouldContinue = CheckPathShouldContinue;
			xMax = XMax;
			yMax = YMax;
			xMin = XMin;
			yMin = YMin;
			pathsChecked = 0;
			if (activePath.Count == 0) {
				pathsChecked++;
				CheckPath(x, y, distance);
			}

			for (; pathsChecked < maxPathsToCheck && activePath.Count > 0; pathsChecked++) {
				activePath.Dequeue()();
			}

			if (activePath.Count < 1) {
				pathChecks.Remove(pathCheckID);
				activePath = null;

				return PathCheckDefaultID;
			}

			return pathCheckID;
		}
		private static int GetNextOpenPathCheckID() {
			int i = 0;
			while (pathChecks.ContainsKey(i)) {
				i++;
			}

			return i;
		}
		private static Func<int, int, int, bool> checkPathShouldContinue;
		private static int xMax;
		private static int yMax;
		private static int xMin;
		private static int yMin;
		private static void CheckPath(int x, int y, int currentDistance, int fromDirection = -1) {
			if (!checkPathShouldContinue(x, y, currentDistance) && fromDirection != -1)
				return;

			int opposite = PathDirectionID.GetOppositeDirection(fromDirection);
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

				int directionIDForPathAction = directionID;
				activePath.Enqueue(() => {
					CheckPath(x2, y2, currentDistance + 1, directionIDForPathAction);
				});
			}
		}
	}
}
