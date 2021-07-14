using System.Collections.Generic;

namespace Bolt
{
	/// <summary>
	/// Returns the sum of two or more 2D vectors.
	/// </summary>
	[UnitCategory("Math/Vector 2")]
	[UnitTitle("Sum")]
	public sealed class Vector2Sum : Sum<UnityEngine.Vector2>
	{
		public override UnityEngine.Vector2 Operation(UnityEngine.Vector2 a, UnityEngine.Vector2 b)
		{
			return a + b;
		}

		public override UnityEngine.Vector2 Operation(IEnumerable<UnityEngine.Vector2> values)
		{
			var sum = UnityEngine.Vector2.zero;

			foreach (var value in values)
			{
				sum += value;
			}

			return sum;
		}
	}
}