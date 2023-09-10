/*
Terraria world space has an inverted y axis.  
		0
	High	0
		High
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace androLib.Common.Utility
{

	public static class VectorMath
	{
		public static readonly Vector2 PlayerNotMoving = new(0f, 0.4f);
		public static readonly float PsudoZero = 0.00001f;//100000
		public static float LinearEvaluate(Vector2 p1, Vector2 p2, float x) => (p2.Y - p1.Y) / (p2.X - p1.X) * (x - p1.X) + p1.Y;
		public static bool QuadraticEvaluate(float len, float height1, float height2HypotenuseRatio, out float height2) {
			float a = 1f - height2HypotenuseRatio * height2HypotenuseRatio;
			float b = 2f * height1;
			float c = height1 * height1 + len * len;
			float discriminant = b * b - 4f * a * c;
			if (discriminant < 0f) {
				height2 = 0f;
				return false;
			}

			float sqrtDiscriminant = (float)Math.Sqrt(discriminant);
			if (a == 0f)
				a = PsudoZero;

			float x1 = (-b + sqrtDiscriminant) / (2f * a);
			float x2 = (-b - sqrtDiscriminant) / (2f * a);
			float x = x1 > 0f ? x1 : x2;
			height2 = x;

			return true;
		}

		/// <returns>Slope of the vector</returns>
		public static float Slope(this Vector2 velocity) => velocity.Y / (velocity.X != 0f ? velocity.X : PsudoZero);

		/// <returns>Slope perpendicular to the vector's slope</returns>
		public static float InvertedSlope(this Vector2 velocity) =>  -velocity.X / (velocity.Y != 0f ? velocity.Y : PsudoZero);

		/// <summary>
		/// y1 = m1 * x1 + c1<br/>
		/// y2 = m2 * x2 + c2<br/>
		/// y1 = y2, solve for x<br/>
		/// Returns the default value of <see cref="TryLineIntercept"/> if the slopes are equal which would divide by zero.
		/// </summary>
		/// <param name="m1">Slope 1</param>
		/// <param name="c1">Y intercept 1</param>
		/// <param name="m2">Slope 2</param>
		/// <param name="c2">Y intercept 2</param>
		/// <returns>Intersection point</returns>
		public static Vector2 LineIntercept(float m1, float c1, float m2, float c2) {
			TryLineIntercept(m1, c1, m2, c2, out Vector2 intercept);

			return intercept;
		}

		/// <summary>
		/// y1 = m1 * x1 + c1<br/>
		/// y2 = m2 * x2 + c2<br/>
		/// y1 = y2, solve for x<br/>
		/// Returns Vector2.Zero if m1 and m2 are equal which would divide by zero.
		/// </summary>
		/// <param name="m1">Slope 1</param>
		/// <param name="c1">Y intercept 1</param>
		/// <param name="m2">Slope 2</param>
		/// <param name="c2">Y intercept 2</param>
		/// <param name="intercept">Intersection point</param>
		/// <returns>false if m1 and m2 are equal which would divide by zero.  true otherwise.</returns>
		public static bool TryLineIntercept(float m1, float c1, float m2, float c2, out Vector2 intercept) {
			float denom = m1 - m2;
			float x = (c2 - c1) / (denom != 0f ? denom : PsudoZero);
			float y = m1 * x + c1;

			intercept = new(x, y);
			return true;
		}

		/// <summary>
		/// Finds the X value where the projectile's velocity is equal to the target's Y position.
		/// </summary>
		/// <param name="velocity">Velocity of the target</param>
		/// <returns>X Intercept</returns>
		public static float XIntercept(Vector2 velocity, Vector2 targetPosition, Vector2 projectilePosition) => XIntercept(velocity.Slope(), targetPosition.X, targetPosition.Y, projectilePosition.Y);

		/// <summary>
		/// Finds the X value where the projectile's velocity is equal to the target's Y position.
		/// </summary>
		/// <param name="slope">Slope of the target's velocity</param>
		/// <param name="targetY">Target's Y position</param>
		/// <param name="projectileY">Projectile's Y position</param>
		/// <returns>X Intercept</returns>
		public static float XIntercept(float slope, float targetX, float targetY, float projectileY) => targetX - (targetY - projectileY) / (slope != 0f ? slope : PsudoZero);
		
		/// <summary>
		/// Finds the relative origin backtracking from the target position in the opposite direction of the target's velocity to where it intersects with the projectile's X position.
		/// </summary>
		/// <param name="velocity">Velocity of the target</param>
		/// <returns>Relative Origin</returns>
		public static Vector2 RelativeOrigin(Vector2 velocity, Vector2 targetPosition, Vector2 projectilePosition) {
			float x = XIntercept(velocity, targetPosition, projectilePosition);
			float y = projectilePosition.Y;
			return new Vector2(x, y);
		}

		/// <summary>
		/// Creates 2 lines using the relative origin.<br/>
		/// Line 1 is the target's velocity which has a c value of 0 because the velocity is used to calculate the relative origin.<br/>
		/// Line 2 is the line perpendicular to the target's velocity which passes through the projectile's position.<br/>
		/// Returns the default value of <see cref="TryLineIntercept"/> if the slopes are equal which would divide by zero.<br/>
		/// </summary>
		/// <param name="velocity">Velocity of the target</param>
		/// <returns>Intersection point of the vector and line perpendicular through the projectilePosition.X.</returns>
		public static Vector2 GetPerpendicularLineIntercept(Vector2 velocity, Vector2 targetPosition, Vector2 projectilePosition) {
			TryGetPerpendicularLineIntercept(velocity, targetPosition, projectilePosition, out Vector2 intercept);
			return intercept;
		}

		/// <summary>
		/// Creates 2 lines using the relative origin.<br/>
		/// Line 1 is the target's velocity which has a c value of 0 because the velocity is used to calculate the relative origin.<br/>
		/// Line 2 is the line perpendicular to the target's velocity which passes through the projectile's position.<br/>
		/// Returns the default value of <see cref="TryLineIntercept"/> if the slopes are equal which would divide by zero.<br/>
		/// </summary>
		/// <param name="velocity">Velocity of the target</param>
		/// <param name="targetPosition"></param>
		/// <param name="projectilePosition"></param>
		/// <param name="intercept">Intersection point of the vector and line perpendicular through the projectilePosition.X.</param>
		/// <returns>false if m1 and m2 are equal which would divide by zero.  true otherwise.</returns>
		public static bool TryGetPerpendicularLineIntercept(Vector2 velocity, Vector2 targetPosition, Vector2 projectilePosition, out Vector2 intercept) {
			Vector2 relativeOrigin = RelativeOrigin(velocity, targetPosition, projectilePosition);
			float m1 = velocity.Slope();
			float c1 = relativeOrigin.Y;
			float m2 = velocity.InvertedSlope();
			float c2 = projectilePosition.Y - m2 * (projectilePosition.X - relativeOrigin.X);
			//Main.NewText($"m1: {m1}, c1: {c1}, m2: {m2}, c2: {c2}");
			bool lineResult = TryLineIntercept(m1, c1, m2, c2, out intercept);
			intercept.X += relativeOrigin.X;
			
			return lineResult;
		}
		public delegate Vector2 GetTargetCenter();
		public delegate Vector2 GetTargetVelocity(Vector2 targetCenter);
		public delegate float GetProjectileVelocityLength();
		public delegate Vector2 GetProjectileCenter();
		public delegate void SetProjectileVelocity(Vector2 velocity);
		public delegate void SetProjectileCenter(Vector2 center);
		/// <summary>
		/// Calculated the velocity of a projectile to home in on a target.<br/>
		/// </summary>
		/// <param name="getTargetCenter"><see cref="GetTargetCenter"/></param>
		/// <param name="getTargetVelocity"><see cref="GetTargetVelocity"/></param>
		/// <param name="getProjectileVelocityLength"><see cref="GetProjectileVelocityLength"/></param>
		/// <param name="getProjectileCenter"><see cref="GetProjectileCenter"/></param>
		/// <param name="setProjectileVelocity"><see cref="SetProjectileVelocity"/></param>
		/// <param name="setProjectileCenter"><see cref="SetProjectileCenter"/></param>
		/// <returns>true if position was set to the center of the target.  false if only velocity was updated.</returns>
		public static bool CalculateHomingVelocity(
			GetTargetCenter getTargetCenter,
			GetTargetVelocity getTargetVelocity,
			GetProjectileVelocityLength getProjectileVelocityLength,
			GetProjectileCenter getProjectileCenter,
			SetProjectileVelocity setProjectileVelocity,
			SetProjectileCenter setProjectileCenter
			) {
			Vector2 targetCenter = getTargetCenter();
			Vector2 targetVelocity = getTargetVelocity(targetCenter);
			float targetVelocityLength = targetVelocity.Length();
			float projectileVelocityLength = getProjectileVelocityLength();
			Vector2 targetVelocityNormal = targetVelocity.SafeNormalize(Vector2.Zero);
			float velcityRatio = projectileVelocityLength / (targetVelocityLength != 0f ? targetVelocityLength : PsudoZero);
			if (velcityRatio > 1f) {
				//Homing
				Vector2 homingTarget = targetCenter;
				Vector2 projectileCenter = getProjectileCenter();
				if (TryGetPerpendicularLineIntercept(targetVelocity, targetCenter, projectileCenter, out Vector2 perpendicualarIntercept)) {
					float relativeB1 = targetCenter.Distance(perpendicualarIntercept);
					float relativeA = projectileCenter.Distance(perpendicualarIntercept);
					if (QuadraticEvaluate(relativeA, relativeB1, velcityRatio, out float targetVectorLength)) {
						//Less than 1 tick away, show full distance traveled in the tick.
						if (targetVectorLength < targetVelocityLength)
							targetVectorLength = targetVelocityLength;

						Vector2 homingTargetVector = targetVelocityNormal * (targetVectorLength);
						homingTarget = homingTargetVector + targetCenter;
					}
				}

				Vector2 projectileDirection = (homingTarget - projectileCenter).SafeNormalize(Vector2.Zero);
				float remainingDistance = homingTarget.Distance(projectileCenter);

				float remainingDistanceRatio = remainingDistance / (projectileVelocityLength != 0f ? projectileVelocityLength : PsudoZero);
				if (projectileVelocityLength > remainingDistance) {
					if (projectileVelocityLength > remainingDistance + targetVelocityLength) {
						setProjectileVelocity(targetVelocity);
						setProjectileCenter(targetCenter);
						return true;
					}
					else {
						projectileVelocityLength *= remainingDistanceRatio;
					}
				}

				Vector2 projectileVelocity = projectileDirection * projectileVelocityLength;
				setProjectileVelocity(projectileVelocity);
			}
			else {
				//Parallel (projectile is slower than target)
				Vector2 projectileVelocity = targetVelocityNormal * projectileVelocityLength;
				setProjectileVelocity(projectileVelocity);
			}

			return false;
		}

		/*
		Example:
						|										  .
						|									  .
						|								  .
						|							  .
						|						  .
						|					  .
						|				  .
						| (3000, 1000) .
						|			 *
						|         .   .
						|	   .	   .
						|	.		    .
		(2166.67, 3500)	|________________* (4000, 3500)
		*/
		private static readonly Vector2 EXAMPLE_TARGET_POSITION = new Vector2(3000f, 1000f);
		private static readonly Vector2 EXAMPLE_TARGET_VELOCITY = new Vector2(100f, -300f);
		private static readonly Vector2 EXAMPLE_PROJECTILE_POSITION = new Vector2(4000f, 3500f);
		private static readonly Vector2 EXAMPLE_PROJECTILE_VELOCITY = new Vector2(300f, -300f);
		public static void PrintExampleTragectoryCalculation() {
			float targetVelocityLength = EXAMPLE_TARGET_VELOCITY.Length();
			float projectileVelocityLength = EXAMPLE_PROJECTILE_VELOCITY.Length();
			Vector2 targetVelocityNormal = EXAMPLE_TARGET_VELOCITY.SafeNormalize(Vector2.Zero);
			float velcityRatio = projectileVelocityLength / targetVelocityLength;
			if (velcityRatio > 1f) {
				//Home
				if (TryGetPerpendicularLineIntercept(EXAMPLE_TARGET_VELOCITY, EXAMPLE_TARGET_POSITION, EXAMPLE_PROJECTILE_POSITION, out Vector2 perpendicualarIntercept)) {
					float relativeB1 = EXAMPLE_TARGET_POSITION.Distance(perpendicualarIntercept);
					float relativeA = EXAMPLE_PROJECTILE_POSITION.Distance(perpendicualarIntercept);
					Vector2 homingTargetVector = targetVelocityNormal * (EXAMPLE_TARGET_VELOCITY.Length() + relativeB1);
					Vector2 homingTarget = homingTargetVector + EXAMPLE_TARGET_POSITION;
					Vector2 projectileDirection = (homingTarget - EXAMPLE_PROJECTILE_POSITION).SafeNormalize(Vector2.Zero);
					Vector2 projectileVelocity = projectileDirection * projectileVelocityLength;
					Main.NewText($"I: {perpendicualarIntercept}, B1: {relativeB1}, A: {relativeA}, VT: {homingTargetVector}, HT: {homingTarget}, PV: {projectileVelocity}");
				}
				else {
					Main.NewText($"Div by zero!!  perpendicualarIntercept: {perpendicualarIntercept}");
				}
			}
			else {
				//Parallel
				Vector2 projectileVelocity = targetVelocityNormal * projectileVelocityLength;

				Main.NewText($"To slow to home, projectileVelocity: {projectileVelocity}");
			}
		}
	}
}
