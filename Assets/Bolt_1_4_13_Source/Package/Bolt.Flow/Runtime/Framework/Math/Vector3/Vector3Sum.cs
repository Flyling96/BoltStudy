using System.Collections.Generic;

namespace Bolt
{
	/// <summary>
	/// Returns the sum of two or more 3D vectors.
	/// </summary>
	[UnitCategory("Math/Vector 3")]
	[UnitTitle("Sum")]
	public sealed class Vector3Sum : Sum<UnityEngine.Vector3>
	{
		public override UnityEngine.Vector3 Operation(UnityEngine.Vector3 a, UnityEngine.Vector3 b)
		{
			return a + b;
		}

		public override UnityEngine.Vector3 Operation(IEnumerable<UnityEngine.Vector3> values)
		{
			var sum = UnityEngine.Vector3.zero;

			foreach (var value in values)
			{
				sum += value;
			}

			return sum;
		}
	}
}