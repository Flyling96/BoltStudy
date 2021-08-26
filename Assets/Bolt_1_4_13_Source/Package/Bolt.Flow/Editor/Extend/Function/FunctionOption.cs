using Ludiq;
using System;
using UnityEngine;

namespace Bolt.Extend
{
    [FuzzyOption(typeof(FunctionSuperUnit))]
    public class FunctionOption : UnitOption<FunctionSuperUnit>
    {

        public FunctionOption() : base() { }

        public FunctionOption(FunctionSuperUnit unit) : base(unit) { }

        public FunctionOption(FlowFunctionDeclaration function)
        {
            unit = (FunctionSuperUnit)Activator.CreateInstance(typeof(FunctionSuperUnit));
            unit.functionName = function.name;
            unit.function = function;
            unit.Define();
            FillFromUnit();
        }

        protected override string Label(bool human)
        {
            if (unit == null)
            {
                return "FunctionDefault";
            }
            else
            {
                return unit.functionName;
            }
        }

        public override void PreconfigureUnit(FunctionSuperUnit unit)
        {
            base.PreconfigureUnit(unit);
        }
    }
}
