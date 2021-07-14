using System.Collections.Generic;

namespace Bolt
{
	/// <summary>
	/// Returns the sum of two or more 4D vectors.
	/// </summary>
	[UnitCategory("Math/Vector 4")]
	[UnitTitle("Sum")]
	public sealed class Vector4Sum : Sum<UnityEngine.Vector4>
	{
		public override UnityEngine.Vector4 Operation(UnityEngine.Vector4 a, UnityEngine.Vector4 b)
		{
			return a + b;
		}

		public override UnityEngine.Vector4 Operation(IEnumerable<UnityEngine.Vector4> values)
		{
			var sum = UnityEngine.Vector4.zero;

			foreach (var value in values)
			{
				sum += value;
			}

			return sum;
		}
	}
}