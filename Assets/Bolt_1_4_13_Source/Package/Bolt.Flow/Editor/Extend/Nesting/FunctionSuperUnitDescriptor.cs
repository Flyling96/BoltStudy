using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;

namespace Bolt.Extend
{
    [Descriptor(typeof(FunctionSuperUnit))]
    public class FunctionSuperUnitDescriptor : UnitDescriptor<FunctionSuperUnit>
    {
        public FunctionSuperUnitDescriptor(FunctionSuperUnit unit) : base(unit) { }

        protected override string DefinedShortTitle()
        {
            return string.Format("Func: {0}", unit.functionName);
        }

        EditorTexture icon { get; set; }

        protected override EditorTexture DefinedIcon()
        {
            if(icon == null)
            {
                icon = EditorTexture.Single(LudiqGraphs.Icons.window?[IconSize.Large]);
            }

            return icon;
        }
    }
}
