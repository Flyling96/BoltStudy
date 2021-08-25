using System.Linq;

namespace Bolt
{
	/// <summary>
	/// Passes output values from this graph to the parent super unit.
	/// </summary>
	[UnitCategory("Nesting")]
	[UnitOrder(2)]
	[UnitTitle("Output")]
	public sealed class GraphOutput : Unit
	{
		public override bool canDefine => graph != null;

		protected override void Definition()
		{
			isControlRoot = true;

			foreach (var controlOutputDefinition in graph.validPortDefinitions.OfType<ControlOutputDefinition>())
			{
				var key = controlOutputDefinition.key;

				ControlInput(key, (flow) =>
				{
					if (flow.stack.IsWithin<SuperUnit>())
					{
						var superUnit = flow.stack.GetParent<SuperUnit>();

						flow.stack.ExitParentElement();

						superUnit.EnsureDefined();

						return superUnit.controlOutputs[key];
					}
					else if (flow.stack.IsWithin<Extend.FlowFunctionDeclaration>())
					{
						var function = flow.stack.GetParent<Extend.FlowFunctionDeclaration>();

						if (function.executeElement != null && function.executeElement is Extend.FunctionSuperUnit functionSuperUnit)
						{
							flow.stack.ExitFunctionElement();

							functionSuperUnit.EnsureDefined();

							return functionSuperUnit.controlOutputs[key];
						}
					}

					return null;
				});
			}

			foreach (var valueOutputDefinition in graph.validPortDefinitions.OfType<ValueOutputDefinition>())
			{
				var key = valueOutputDefinition.key;
				var type = valueOutputDefinition.type;

				ValueInput(type, key);
			}
		}
		
		protected override void AfterDefine()
		{
			graph.onPortDefinitionsChanged += Define;
		}

		protected override void BeforeUndefine()
		{
			graph.onPortDefinitionsChanged -= Define;
		}
	}
}