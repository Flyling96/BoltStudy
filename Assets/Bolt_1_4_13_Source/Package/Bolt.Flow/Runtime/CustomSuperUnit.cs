using Ludiq;

namespace Bolt.Extend
{
	[TypeIcon(typeof(FlowGraph))]
	[UnitCategory("Extend")]
	[CustomRutimeType]
	public sealed class CustomSuperUnit : SuperUnit
	{
		public CustomSuperUnit(FlowMacro macro) : base(macro) { }

		[Serialize]
		public string VariableName { get; set; }

		[DoNotSerialize]
		[PortLabelHidden]
		ValueInput self;

		protected override void Definition()
		{
			base.Definition();
			self = ValueInput(nameof(self), VariableName);
		}
	}
}
