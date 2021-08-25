using System.Linq;
using Ludiq;

namespace Bolt
{
	/// <summary>
	/// Fetches input values from the parent super unit for this graph.
	/// </summary>
	[UnitCategory("Nesting")]
	[UnitOrder(1)]
	[UnitTitle("Input")]
	public sealed class GraphInput : Unit
	{
		public override bool canDefine => graph != null;

		protected override void Definition()
		{
			isControlRoot = true;

			foreach (var controlInputDefinition in graph.validPortDefinitions.OfType<ControlInputDefinition>())
			{
				ControlOutput(controlInputDefinition.key);
			}

			foreach (var valueInputDefinition in graph.validPortDefinitions.OfType<ValueInputDefinition>())
			{
				var key = valueInputDefinition.key;
				var type = valueInputDefinition.type;

				ValueOutput(type, key, (flow) =>
				{
					if (flow.stack.IsWithin<SuperUnit>())
					{
						var superUnit = flow.stack.GetParent<SuperUnit>();

						if (flow.enableDebug)
						{
							var editorData = flow.stack.GetElementDebugData<IUnitDebugData>(superUnit);

							editorData.lastInvokeFrame = EditorTimeBinding.frame;
							editorData.lastInvokeTime = EditorTimeBinding.time;
						}

						flow.stack.ExitParentElement();
						superUnit.EnsureDefined();
						var value = flow.GetValue(superUnit.valueInputs[key], type);
						flow.stack.EnterParentElement(superUnit);
						return value;
					}
					else if(flow.stack.IsWithin<Extend.FlowFunctionDeclaration>())
                    {
						var function = flow.stack.GetParent<Extend.FlowFunctionDeclaration>();
						if(function.executeElement != null && function.executeElement is Extend.FunctionSuperUnit functionSuperUnit)
                        {
							if (flow.enableDebug)
							{
								var editorData = flow.stack.GetElementDebugData<IUnitDebugData>(functionSuperUnit);

								editorData.lastInvokeFrame = EditorTimeBinding.frame;
								editorData.lastInvokeTime = EditorTimeBinding.time;
							}

							flow.stack.ExitFunctionElement();
							functionSuperUnit.EnsureDefined();
							var value = flow.GetValue(functionSuperUnit.valueInputs[key], type);
							flow.stack.EnterFunctionElement(function);
							return value;
						}
					}

					return null;
				});
			}
		}
		
		protected override void AfterDefine()
		{
            if(graph != null)
            {
                graph.onPortDefinitionsChanged += Define;
            }
		}

		protected override void BeforeUndefine()
		{
            if(graph != null)
            {
    			graph.onPortDefinitionsChanged -= Define;
            }
		}
	}
}