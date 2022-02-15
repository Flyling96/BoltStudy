using Ludiq;
using System;

namespace Bolt.Extend
{
	[TypeIcon(typeof(FlowGraph))]
	[UnitCategory("Extend")]
	[CustomRutimeType]
	public sealed partial class CustomSuperUnit : SuperUnit
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
			isControlRoot = true;

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
				else if (definition is ControlOutputDefinition)
				{
					var controlOutputDefinition = (ControlOutputDefinition)definition;
					var key = controlOutputDefinition.key;

					ControlOutput(key);
				}
				else if (definition is ValueOutputDefinition)
				{
					var valueOutputDefinition = (ValueOutputDefinition)definition;
					var key = valueOutputDefinition.key;
					var type = valueOutputDefinition.type;

					ValueOutput(type, key, (flow) =>
					{
						flow.stack.EnterParentElement(this);

						// Manual looping to avoid LINQ allocation
						// Also removing check for multiple output units for speed 
						// (The first output unit will be used without any error)

						foreach (var unit in nest.graph.units)
						{
							if (unit is GraphOutput)
							{
								var outputUnit = (GraphOutput)unit;

								var value = flow.GetValue(outputUnit.valueInputs[key]);

								flow.stack.ExitParentElement();

								return value;
							}
						}

						flow.stack.ExitParentElement();

						throw new InvalidOperationException("Missing output unit when to get value.");
					});
				}
			}
			self = ValueInput(nameof(self), VariableName);
		}
	}
}
