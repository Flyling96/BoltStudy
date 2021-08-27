using Ludiq;
using UnityEngine;

namespace Bolt.Extend
{
    [Editor(typeof(CustomSuperUnit))]
    public class CustomSuperUnitEditor : SuperUnitEditor
    {
        public CustomSuperUnitEditor(Metadata metadata) : base(metadata)
        {
            //variableNameInspector = new CustomVariableNameInspector(variableNameMetadata, GetVariablesName, UpdateMacro);
            //macroInspector = new CustomUnityObjectInspector(macroMetadata);
        }


        //protected new CustomVariableNameInspector variableNameInspector = null;
        //private CustomUnityObjectInspector macroInspector = null;

        private Metadata macroMetadata => metadata[nameof(CustomSuperUnit.nest)][nameof(IGraphNest.macro)];

        protected override void OnInspectorGUI(Rect position)
        {
            base.OnInspectorGUI(position);
            //using (LudiqGUIUtility.currentInspectorWidth.Override(position.width))
            //using (Inspector.adaptiveWidth.Override(true))
            //{
            //    variableNameInspector.Draw(position, GUIContent.none);
            //    Rect graphPosition = new Rect(position.position + new Vector2(0, 20), position.size);
            //    macroInspector.Draw(graphPosition, GUIContent.none);
            //}
        }


        public void UpdateMacro()
        {
            string value = metadata[nameof(SpecifyVariableUnit.variableName)].value as string;
            FlowMachine machine = Variables.AutoSubFlow(reference.gameObject).Get(value) as FlowMachine;
            if (machine != null)
            {
                macroMetadata.value = machine.nest.macro;
            }
        }

    }
}
