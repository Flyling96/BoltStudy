using System.Collections.Generic;
using System.Linq;
using Ludiq;

namespace Bolt
{
	[UnitOrder(303)]
	public abstract class Sum<T> : MultiInputUnit<T>
	{
		/// <summary>
		/// The sum.
		/// </summary>
		[DoNotSerialize]
		[PortLabelHidden]
		public ValueOutput sum { get; private set; }

		protected override void Definition()
		{
			base.Definition();

			sum = ValueOutput(nameof(sum), Operation).Predictable();

			foreach (var multiInput in multiInputs)
			{
				Requirement(multiInput, sum);
			}
		}

		public abstract T Operation(T a, T b);

		public abstract T Operation(IEnumerable<T> values);

		public T Operation(Flow flow)
		{
			if (inputCount == 2)
			{
				return Operation(flow.GetValue<T>(multiInputs[0]), flow.GetValue<T>(multiInputs[1]));
			}
			else
			{
				return Operation(multiInputs.Select(flow.GetValue<T>));
			}
		}
	}
}