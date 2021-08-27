using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using UnityEditor;

namespace Bolt.Extend
{
    [Widget(typeof(FunctionSuperUnit))]
    public class FunctionSuperUnitWidget : UnitWidget<FunctionSuperUnit>
    {
        public FunctionSuperUnitWidget(FlowCanvas canvas, FunctionSuperUnit unit) : base(canvas, unit) { }

        protected override void OnDoubleClick()
        {
            base.OnDoubleClick();
            var gameObject = Flow.Predict(unit.self, reference) as GameObject;
            if(gameObject != null)
            {
                Selection.activeObject = gameObject;
                var machine = gameObject.GetComponent<FlowMachine>();
                if(machine != null)
                {
                    var functionReference = machine.GetFunctionReference(unit.functionName);
                    if(functionReference != null)
                    {
                        GraphWindow.OpenActive(functionReference);
                    }
                }
            }
        }
    }
}
