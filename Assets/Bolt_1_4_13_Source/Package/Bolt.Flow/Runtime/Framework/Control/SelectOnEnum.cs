using System;
using System.Collections.Generic;
using Ludiq;

namespace Bolt
{
	/// <summary>
	/// Selects a value from a set by switching over an enum.
	/// </summary>
	[UnitCategory("Control")]
	[UnitTitle("Select On Enum")]
	[UnitShortTitle("Select")]
	[UnitSubtitle("On Enum")]
	[UnitOrder(7)]
	[TypeIcon(typeof(ISelectUnit))]
	public sealed partial class SelectOnEnum : Unit, ISelectUnit
	{
		[DoNotSerialize]
		public Dictionary<object, ValueInput> branches { get; private set; }

		/// <summary>
		/// The value on which to select.
		/// </summary>
		[DoNotSerialize]
		[PortLabelHidden]
		public ValueInput selector { get; private set; }

		/// <summary>
		/// The selected value.
		/// </summary>
		[DoNotSerialize]
		[PortLabelHidden]
		public ValueOutput selection { get; private set; }

		[Serialize]
		[Inspectable, UnitHeaderInspectable]
		[TypeFilter(Enums = true, Classes = false, Interfaces = false, Structs = false, Primitives = false)]
		public Type enumType { get; set; }

		public override bool canDefine => enumType != null && enumType.IsEnum;

		protected override void Definition()
		{
			branches = new Dictionary<object, ValueInput>();

			selection = ValueOutput(nameof(selection), Branch).Predictable();

			selector = ValueInput(enumType, nameof(selector));

			Requirement(selector, selection);

			foreach (var enumValue in Enum.GetValues(enumType))
			{
				var branch = ValueInput<object>("%" + enumValue).AllowsNull();

				branches.Add(enumValue, branch);

				Requirement(branch, selection);
			}
		}

		public object Branch(Flow flow)
		{
			var selector = flow.GetValue(this.selector, enumType);

			return flow.GetValue(branches[selector]);
		}
	}
}