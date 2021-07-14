using System.Collections.Generic;
using System.Linq;

namespace Bolt
{
	/// <summary>
	/// Returns the sum of two or more scalars.
	/// </summary>
	[UnitCategory("Math/Scalar")]
	[UnitTitle("Sum")]
	public sealed class ScalarSum : Sum<float>
	{
		public override float Operation(float a, float b)
		{
			return a + b;
		}

		public override float Operation(IEnumerable<float> values)
		{
			return values.Sum();
		}
	}
}