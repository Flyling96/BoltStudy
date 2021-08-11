using Ludiq;

namespace Bolt.Extend
{
	[TypeIcon(typeof(FlowGraph))]
	[UnitCategory("Extend")]
	[CustomRutimeType]
	public sealed class CustomSuperUnit : SuperUnit
	{
		public CustomSuperUnit() : base() { }

		public CustomSuperUnit(FlowMacro macro) : base(macro) { }

		[Serialize]
		public string VariableName { get; set; }

		[DoNotSerialize]
		[PortLabelHidden]
		ValueInput self;

		protected override void Definition()
		{
			foreach (var definition in nest.graph.validPortDefinitions)
			{
				if (definition is ControlInputDefinition)
				{
					var controlInputDefinition = (ControlInputDefinition)definition;
					var key = controlInputDefinition.key;

					ControlInput(key, (flow) =>
					{
						flow.stack.EnterParentElement(this);

						var subFlowMachine = Variables.AutoSubFlow(flow.stack.gameObject)[VariableName] as SubFlowMachine;

						if(subFlowMachine != null)
                        {
							subFlowMachine.GraphControlInput(flow,key);
                        }

						return null;
					});
				}
				else if (definition is ValueInputDefinition)
				{
					var valueInputDefinition = (ValueInputDefinition)definition;
					var key = valueInputDefinition.key;
					var type = valueInputDefinition.type;
					var hasDefaultValue = valueInputDefinition.hasDefaultValue;
					var defaultValue = valueInputDefinition.defaultValue;

					var port = ValueInput(type, key);

					if (hasDefaultValue)
					{
						port.SetDefaultValue(defaultValue);
					}
				}
			}
			self = ValueInput(nameof(self), VariableName);
		}
	}
}
