using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static androLib.Common.Utility.AndroUtilityMethods;

namespace androLib.Common.Utility
{
	public static class ModMath
	{
		#region bools

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		/// <param name="remainder">0 if result is < int.MaxValue.  = result - int.MaxValue otherwise</param>
		/// <returns>True if the result is > int.MaxValue</returns>
		public static bool AddCheckOverflow(this ref int n1, int n2, out long remainder) {
			n1 = AddCheckOverflow(n1, n2, out remainder);
			return remainder != 0;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at int.MaxValue.
		/// </summary>
		/// <param name="remainder">0 if result is < int.MaxValue.  = result - int.MaxValue otherwise</param>
		/// <returns>True if the result is > int.MaxValue</returns>
		public static bool MultiplyCheckOverflow(this ref int n1, int n2, out long remainder) {
			n1 = MultiplyCheckOverflow(n1, n2, out remainder);
			return remainder != 0;
		}

		#endregion

		#region voids

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		public static void AddCheckOverflow(this ref int n1, int n2) {
			int maxN2 = int.MaxValue - n1;
			if (n2 > maxN2) {
				n1 = int.MaxValue;
				return;
			}

			n1 += n2;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at int.MaxValue.
		/// </summary>
		public static void MultiplyCheckOverflow(this ref int n1, int n2) {
			int maxN2 = int.MaxValue / n1;
			if (n2 > maxN2) {
				n1 = int.MaxValue;
				return;
			}

			n1 *= n2;
		}

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		public static void AddCheckOverflow(this ref int n1, float n2) => n1.AddCheckOverflow((int)n2);

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at int.MaxValue.
		/// </summary>
		public static void MultiplyCheckOverflow(this ref int n1, float n2) {
			float maxN2 = (float)int.MaxValue / (float)n1;
			if (n2 > maxN2) {
				n1 = int.MaxValue;
				return;
			}

			n1 = (int)Math.Round((float)n1 * n2);
		}

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		public static void AddCheckOverflow(this ref float n1, float n2) {
			float maxN2 = float.MaxValue - n1;
			if (n2 > maxN2) {
				n1 = float.MaxValue;
				return;
			}

			n1 += n2;
		}

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		public static void AddCheckOverflow(this ref long n1, long n2) {
			long maxN2 = long.MaxValue - n1;
			if (n2 > maxN2) {
				n1 = long.MaxValue;
				return;
			}

			n1 += n2;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at float.MaxValue.
		/// </summary>
		public static void MultiplyCheckOverflow(this ref float n1, float n2) {
			float maxN2 = float.MaxValue / n1;
			if (n2 > maxN2) {
				n1 = float.MaxValue;
				return;
			}

			n1 *= n2;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at float.MaxValue.
		/// </summary>
		public static void MultiplyCheckOverflow(this ref long n1, long n2) {
			long maxN2 = long.MaxValue / n1;
			if (n2 > maxN2) {
				n1 = long.MaxValue;
				return;
			}

			n1 *= n2;
		}

		#endregion

		#region returns

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		/// <param name="remainder">0 if result is < int.MaxValue.  = result - int.MaxValue otherwise</param>
		/// <returns>True if the result is > int.MaxValue</returns>
		public static int AddCheckOverflow(int n1, int n2, out long remainder) {
			int maxN2 = int.MaxValue - n1;
			if (n2 > maxN2) {
				remainder = (long)n1 + (long)n2 - int.MaxValue;
				return int.MaxValue;
			}

			remainder = 0;
			return n1 + n2;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at int.MaxValue.
		/// </summary>
		/// <param name="remainder">0 if result is < int.MaxValue.  = result - int.MaxValue otherwise</param>
		/// <returns>True if the result is > int.MaxValue</returns>
		public static int MultiplyCheckOverflow(int n1, int n2, out long remainder) {
			int maxN2 = int.MaxValue / n1;
			if (n2 > maxN2) {
				remainder = (long)n1 * (long)n2 - int.MaxValue;
				return int.MaxValue;
			}

			remainder = 0;
			return n1 * n2;
		}

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		public static int AddCheckOverflow(int n1, int n2) {
			n1.AddCheckOverflow(n2);
			return n1;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at int.MaxValue.
		/// </summary>
		public static int MultiplyCheckOverflow(int n1, int n2) {
			n1.MultiplyCheckOverflow(n2);
			return n1;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at int.MaxValue.
		/// </summary>
		public static int MultiplyCheckOverflow(int n1, float n2) {
			n1.MultiplyCheckOverflow(n2);
			return n1;
		}

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		public static float AddCheckOverflow(float n1, float n2) {
			n1.AddCheckOverflow(n2);
			return n1;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at float.MaxValue.
		/// </summary>
		public static float MultiplyCheckOverflow(float n1, float n2) {
			n1.MultiplyCheckOverflow(n2);
			return n1;
		}

		/// <summary>
		/// Multiplies n1 by n2 and caps n1 at float.MaxValue.
		/// </summary>
		public static long MultiplyCheckOverflow(long n1, long n2) {
			n1.MultiplyCheckOverflow(n2);
			return n1;
		}

		/// <summary>
		/// Adds n2 to n1 and caps n1 at int.MaxValue.
		/// </summary>
		public static double AddCheckOverflow(this double n1, double n2) {
			double maxN2 = double.MaxValue - n1;
			if (n2 > maxN2) {
				n1 = double.MaxValue;
				return n1;
			}
			
			return n1 += n2;
		}
		public static dynamic AddCheckOverflow(dynamic n1, dynamic n2) {
			dynamic maxValue = (dynamic)n1.GetType().GetField("MaxValue").GetValue(null);
			dynamic maxN2 = maxValue - n1;
			if (n2 > maxN2) {
				n1 = maxValue;
				return n1;
			}

			return n1 += n2;
		}
		public static dynamic SumCheckOverFlow(dynamic enumerable) {
			dynamic maxValue = (dynamic)enumerable[0].GetType().GetField("MaxValue").GetValue(null);
			dynamic total = 0;
			foreach(dynamic value in enumerable) {
				dynamic max = maxValue - total;
				if (value > max) {
					total = maxValue;
					return total;
				}

				total += value;
			}

			return total;
		}
		public static int SlowCeilingDivide(this int num, int denom) {
			int result = num / denom;
			int r = num % denom;
			if (r > 0)
				result++;

			return result;
		}
		public static int CeilingDivide(this int num, int denom) => (num - 1) / denom + 1;
		public static int RoundDivide(this int num, int denom) {
			int result = num / denom;
			int r = num % denom;
			if (r >= denom / 2)
				result++;

			return result;
		}
		public static int Ceiling(this float f) {
			int result = (int)f;
			if (result < f)
				result++;

			return result;
		}
		public static int Round(this float f) {
			int result = (int)f;
			if (f - result >= 0.5f)
				result++;

			return result;
		}
		public static int Abs(this int v) => v < 0 ? -v : v;
		public static int Distance(this Point point1, Point point2) {
			int x = point1.X - point2.X;
			int y = point1.Y - point2.Y;
			return (int)Math.Sqrt(x * x + y * y);
		}

		#endregion

		#region Distributions

		//Floats----------------------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 1 when value is 0.  0.5 when value == middle.  Approaches 0 as value approaches infinity.
		/// Middle aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistribuitonDecreasingLinear(this float value, float middle) => LogisticDistributionDecreasing(value / middle);

		/// <summary>
		/// 0 when value is 0.  0.5 when value == middle.  Approaches 1 as value approaches infinity.
		/// Middle aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistribuitonIncreasingLinear(this float value, float middle) => LogisticDistributionIncreasing(value / middle);

		/// <summary>
		/// 1 when value is 0.  0.5 when value == middle.  Approaches 0 as value approaches infinity.
		/// High aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionDecreasingRoot(this float value, float middle, float root = 2f) => LogisticDistributionDecreasingExponential(value, middle, 1f / root);

		/// <summary>
		/// 0 when value is 0.  0.5 when value == middle.  Approaches 1 as value approaches infinity.
		/// High aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionIncreasingRoot(this float value, float middle, float root = 2f) => LogisticDistributionIncreasingExponential(value, middle, 1f / root);

		/// <summary>
		/// 1 when value is 0.  0.5 when value == middle.  Approaches 0 as value approaches infinity.
		/// Low aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionDecreasingExponential(this float value, float middle, float exponent = 2f) => LogisticDistributionDecreasing((float)Math.Pow(value / middle, exponent));

		/// <summary>
		/// 0 when value is 0.  0.5 when value == middle.  Approaches 1 as value approaches infinity.
		/// Low aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionIncreasingExponential(this float value, float middle, float exponent = 2f) => LogisticDistributionIncreasing((float)Math.Pow(value / middle, exponent));

		/// <summary>
		/// 1 when value is 0.  0.5 when value == 1.  Approaches 0 as value approaches infinity.
		/// Div by zero if value == -1.
		/// </summary>
		public static float LogisticDistributionDecreasing(float value) => 1f / (1f + value);

		/// <summary>
		/// 0 when value is 0.  0.5 when value == 1.  Approaches 1 as value approaches infinity.
		/// Div by zero if value == -1.
		/// </summary>
		public static float LogisticDistributionIncreasing(float value) => 1f - LogisticDistributionDecreasing(value);

		//Integers----------------------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 1 when value is 0.  0.5 when value == middle.  Approaches 0 as value approaches infinity.
		/// Middle aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistribuitonDecreasingLinear(this int value, float middle) => LogisticDistribuitonDecreasingLinear((float)value,  middle);

		/// <summary>
		/// 0 when value is 0.  0.5 when value == middle.  Approaches 1 as value approaches infinity.
		/// Middle aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistribuitonIncreasingLinear(this int value, float middle) => LogisticDistribuitonIncreasingLinear((float)value, middle);

		/// <summary>
		/// 1 when value is 0.  0.5 when value == middle.  Approaches 0 as value approaches infinity.
		/// High aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionDecreasingRoot(this int value, float middle, float root = 2f) => LogisticDistributionDecreasingExponential((float)value, middle, 1f / root);

		/// <summary>
		/// 0 when value is 0.  0.5 when value == middle.  Approaches 1 as value approaches infinity.
		/// High aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionIncreasingRoot(this int value, float middle, float root = 2f) => LogisticDistributionIncreasingExponential((float)value, middle, 1f / root);

		/// <summary>
		/// 1 when value is 0.  0.5 when value == middle.  Approaches 0 as value approaches infinity.
		/// Low aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionDecreasingExponential(this int value, float middle, float exponent = 2f) => LogisticDistributionDecreasingExponential((float)value, middle, exponent);

		/// <summary>
		/// 0 when value is 0.  0.5 when value == middle.  Approaches 1 as value approaches infinity.
		/// Low aggressiveness
		/// Div by zero if value / middle == -1.
		/// </summary>
		public static float LogisticDistributionIncreasingExponential(this int value, float middle, float exponent = 2f) => LogisticDistributionIncreasingExponential((float)value, middle, exponent);

		/// <summary>
		/// 1 when value is 0.  0.5 when value == 1.  Approaches 0 as value approaches infinity.
		/// Div by zero if value == -1.
		/// </summary>
		public static float LogisticDistributionDecreasing(int value) => LogisticDistributionDecreasing((float)value);

		/// <summary>
		/// 0 when value is 0.  0.5 when value == 1.  Approaches 1 as value approaches infinity.
		/// Div by zero if value == -1.
		/// </summary>
		public static float LogisticDistributionIncreasing(int value) => 1f - LogisticDistributionIncreasing((float)value);

		#endregion
	}
}
