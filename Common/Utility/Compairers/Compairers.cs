using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace androLib.Common.Utility.Compairers {
	public static class Compairers {
		public class PointComparer : IComparer<Point> {
			public int Compare(Point point, Point other) {
				if (Math.Abs(point.X - other.X) > 0)
					return point.X.CompareTo(other.X);

				return point.Y.CompareTo(other.Y);
			}
		}
	}
}
