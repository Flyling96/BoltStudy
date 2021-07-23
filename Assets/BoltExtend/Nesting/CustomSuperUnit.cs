//using Ludiq;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Bolt.Extend
//{
//	[SpecialUnit]
//	[TypeIcon(typeof(FlowGraph))]
//    [UnitCategory("Nesting")]
//    public sealed class CustomSuperUnit : Unit, IGraphEventListener,IGraphParentElement
//    {
//		private FlowGraph m_Graph = null;

//		private FlowMacro m_Marco = null;

//		[DoNotSerialize]
//		public IGraph childGraph => m_Graph;

//		[DoNotSerialize]
//		public bool isSerializationRoot => true;

//		[DoNotSerialize]
//		public UnityEngine.Object serializedObject => m_Marco;

//        public override void AfterAdd()
//        {
//            base.AfterAdd();
//        }

//        protected override void Definition()
//		{
//			if(m_Graph == null)
//            {
//				return;
//            }

//			isControlRoot = true; // TODO: Infer relations instead

//			// Using portDefinitions and type checks instead of specific definition collections
//			// to avoid duplicates. Iterating only once for speed.

//			foreach (var definition in m_Graph.validPortDefinitions)
//			{
//				if (definition is ControlInputDefinition)
//				{
//					var controlInputDefinition = (ControlInputDefinition)definition;
//					var key = controlInputDefinition.key;

//					ControlInput(key, (flow) =>
//					{
//						foreach (var unit in m_Graph.units)
//						{
//							if (unit is GraphInput)
//							{
//								var inputUnit = (GraphInput)unit;

//								flow.stack.EnterParentElement(this);

//								return inputUnit.controlOutputs[key];
//							}
//						}

//						return null;
//					});
//				}
//				else if (definition is ValueInputDefinition)
//				{
//					var valueInputDefinition = (ValueInputDefinition)definition;
//					var key = valueInputDefinition.key;
//					var type = valueInputDefinition.type;
//					var hasDefaultValue = valueInputDefinition.hasDefaultValue;
//					var defaultValue = valueInputDefinition.defaultValue;

//					var port = ValueInput(type, key);

//					if (hasDefaultValue)
//					{
//						port.SetDefaultValue(defaultValue);
//					}
//				}
//				else if (definition is ControlOutputDefinition)
//				{
//					var controlOutputDefinition = (ControlOutputDefinition)definition;
//					var key = controlOutputDefinition.key;

//					ControlOutput(key);
//				}
//				else if (definition is ValueOutputDefinition)
//				{
//					var valueOutputDefinition = (ValueOutputDefinition)definition;
//					var key = valueOutputDefinition.key;
//					var type = valueOutputDefinition.type;

//					ValueOutput(type, key, (flow) =>
//					{
//						flow.stack.EnterParentElement(this);

//						// Manual looping to avoid LINQ allocation
//						// Also removing check for multiple output units for speed 
//						// (The first output unit will be used without any error)

//						foreach (var unit in m_Graph.units)
//						{
//							if (unit is GraphOutput)
//							{
//								var outputUnit = (GraphOutput)unit;

//								var value = flow.GetValue(outputUnit.valueInputs[key]);

//								flow.stack.ExitParentElement();

//								return value;
//							}
//						}

//						flow.stack.ExitParentElement();

//						throw new InvalidOperationException("Missing output unit when to get value.");
//					});
//				}
//			}
//		}

//		public bool IsListening(GraphPointer pointer)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void StartListening(GraphStack stack)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void StopListening(GraphStack stack)
//        {
//            throw new System.NotImplementedException();
//        }

//        public IGraph DefaultGraph()
//        {
//			return FlowGraph.WithInputOutput();
//		}
//    }
//}
