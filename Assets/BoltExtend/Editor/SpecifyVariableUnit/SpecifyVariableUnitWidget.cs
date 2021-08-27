using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System;

namespace Bolt.Extend
{
    [Widget(typeof(SpecifyVariableUnit))]
    public class SpecifyVariableUnitWidget : UnitWidget<SpecifyVariableUnit>
    {
        private VariableNameInspector nameInspector;
        private Func<Metadata, VariableNameInspector> nameInspectorConstructor;

        public SpecifyVariableUnitWidget(FlowCanvas canvas,SpecifyVariableUnit unit):base(canvas,unit)
        {
            nameInspectorConstructor = (metadata) => new VariableNameInspector(metadata, GetNameSuggestions);
        }

        public override Inspector GetPortInspector(IUnitPort port, Metadata metadata)
        {
            if(port == unit.name)
            {
                InspectorProvider.instance.Renew(ref nameInspector, metadata, nameInspectorConstructor);

                return nameInspector;
            }

            return base.GetPortInspector(port, metadata);
        }

        private IEnumerable<string> GetNameSuggestions()
        {
            if(unit is SubFlowUnit)
            {
                return EditorVariablesUtility.GetVariableNameSuggestions(VariableKind.AutoSubFlow, reference);
            }
            else if(unit is SceneObjectUnit)
            {
                return EditorVariablesUtility.GetVariableNameSuggestions(VariableKind.AutoSceneObject, reference);
            }

            return null;
        }
    }
}
